using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using System.IO;

namespace _8_Puzzle
{
    /// <summary>
    /// Interaction logic for gamePlayWindow.xaml
    /// </summary>
    public partial class gamePlayWindow : Window
    {
        private string sampleImageSource;

        public string SampleImageSource
        {
            get
            {
                return sampleImageSource;
            }

            set
            {
                sampleImageSource = value;
            }
        }

        List<string> imgTagList;
        bool checkGameSaved;
        bool noGameSaved = false;
        bool isDragging = false;
        Point lastPositon;
        int selectedIndex = -1;
        double _oldLeft, _oldTop;
        int _lastCurIndex;
        List<Image> imgList;
        const int size = 175;
        static bool[] checker;
        int steps;
        string level;
        DispatcherTimer timer;
        int min, sec;

        public gamePlayWindow()
        {
            InitializeComponent();
        }

        public gamePlayWindow(string _level, bool _noGameSaved)
        {
            InitializeComponent();
            level = _level;
            noGameSaved = _noGameSaved;
        }

        void SetLevel(string level)
        {
            if (level == "easy")
            {
                min = 3;
                sec = 0;
                timeLabel.Content = "03:00";
            }
            else if (level == "medium")
            {
                min = 1;
                sec = 30;
                timeLabel.Content = "01:30";
            }
            else
            {
                min = 0;
                sec = 30;
                timeLabel.Content = "00:30";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {  
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("gamesaved.txt"))
                {
                    if(!string.IsNullOrEmpty(file.ReadToEnd()))
                    {
                        checkGameSaved = true;
                    }
                    else
                    {
                        checkGameSaved = false;
                    }

                    file.Close();
                }
            }
            catch
            {
                MessageBox.Show("Cannot read gamesaved.txt", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if(checkGameSaved)
            {

                using (System.IO.StreamReader file = new System.IO.StreamReader("gamesaved.txt"))
                {
                    sampleImageSource = file.ReadLine();

                    imgTagList = new List<string>();
                    imgTagList.Add(file.ReadLine());
                    imgTagList.Add(file.ReadLine());
                    imgTagList.Add(file.ReadLine());
                    imgTagList.Add(file.ReadLine());
                    imgTagList.Add(file.ReadLine());
                    imgTagList.Add(file.ReadLine());
                    imgTagList.Add(file.ReadLine());
                    imgTagList.Add(file.ReadLine());
                    imgTagList.Add(file.ReadLine());

                    if (!int.TryParse(file.ReadLine(), out min))
                        return;

                    if (!int.TryParse(file.ReadLine(), out sec))
                        return;

                    if (!int.TryParse(file.ReadLine(), out steps))
                        return;

                    level = file.ReadLine();
               
                    file.Close();
                }

                File.WriteAllText("gamesaved.txt", string.Empty);

                string _min, _sec;

                if (min < 10)
                    _min = "0" + min.ToString();
                else
                    _min = min.ToString();

                if (sec < 10)
                    _sec = "0" + sec.ToString();
                else
                    _sec = sec.ToString();

                timeLabel.Content = _min + ":" + _sec;

                sampleImage.Source = new BitmapImage(new Uri(sampleImageSource));
                cropImage(sampleImageSource);
            }
            else
            {
                if(noGameSaved == false)
                {
                    var mainScreen = new MainWindow();
                    this.Hide();
                    mainScreen.ShowDialog();
                    this.Close();
                }
                else
                {
                    cropImage(sampleImage.Source.ToString());
                    SetLevel(level);
                    steps = 0;
                }

            }

            levelValue.Content = level;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            stepsValue.Content = steps.ToString();
        }

        void cropImage(string src)
        {
            var image = new BitmapImage(new Uri(src));
            var width = image.PixelWidth / 3;
            var height = image.PixelHeight / 3;

            board.Children.Clear();

            checker = new bool[8];
            for (var i = 0; i < 8; i++)
                checker[i] = false;

            imgList = new List<Image>();

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var cropped = new CroppedBitmap(image, new Int32Rect(i * width, j * height, width, height));

                    var img = new Image();

                    img.Source = cropped;
                    img.Width = size;
                    img.Height = size;
                    img.Tag = i * 3 + j;

                    imgList.Add(img);
                }
            }

            var random = new Random();
            var indices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };

            var savedIndexes = new List<int>();
            if (checkGameSaved)
            {
                foreach (var imgTag in imgTagList)
                {
                    string[] tag = imgTag.Split(new string[] { "-" }, StringSplitOptions.None);

                    if (tag.Length == 2)
                    {
                        savedIndexes.Add(Convert.ToInt32(tag[1]));
                    }
                }
            }

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var currentIndex = i * 3 + j;

                    if(checkGameSaved)
                    {
                        for (var k = 0; k < savedIndexes.Count; k++)
                        {
                            if (savedIndexes[k] == currentIndex)
                            {
                                var img = imgList[k];

                                CheckImageIndex(k, currentIndex);

                                img.Tag = img.Tag.ToString() + "-" + currentIndex.ToString();

                                board.Children.Add(img);

                                Canvas.SetLeft(img, i * size);
                                Canvas.SetTop(img, j * size);
                            }
                        }
                    }
                    else
                    {
                        if (i != 2 || j != 2)
                        {
                            var randomIndex = random.Next(indices.Count);
                            var index = indices[randomIndex];
                            var img = imgList[index];
                            img.Tag = img.Tag.ToString() + "-" + currentIndex.ToString();

                            CheckImageIndex(index, currentIndex);

                            board.Children.Add(img);

                            Canvas.SetLeft(img, i * size);
                            Canvas.SetTop(img, j * size);

                            indices.RemoveAt(randomIndex);
                        }
                    }
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            string _min, _sec;

            sec--;

            if(sec == 0 && min == 0)
            {
                if (!CheckWinning())
                {
                    _min = "00";
                    _sec = "00";
                    timeLabel.Content = _min + ":" + _sec;
               
                    timer.Stop();

                    if (MessageBox.Show("YOU LOST !", "", MessageBoxButton.OK, MessageBoxImage.Stop) == MessageBoxResult.OK)
                    {
                        var mainScreen = new MainWindow();
                        this.Hide();
                        mainScreen.ShowDialog();
                        this.Close();
                    }
                }
            }

            if (sec < 0)
            {
                min--;
                sec = 59;     
            }

            if (min < 10)
                _min = "0" + min.ToString();
            else
                _min = min.ToString();

            if (sec < 10)
                _sec = "0" + sec.ToString();
            else
                _sec = sec.ToString();

            timeLabel.Content = _min + ":" + _sec;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            var mainSrceen = new MainWindow();
            this.Hide();
            timer.Stop();
            mainSrceen.ShowDialog();
            this.Close();
        }

        private void changeImageButton_Click(object sender, RoutedEventArgs e)
        {
            var storage = new ImageStorage();
            storage.ShowDialog();

            if(storage.DialogResult == true)
            {
                SampleImageSource = storage.SampleImageSource;
                steps = storage.steps;
                stepsValue.Content = steps.ToString();
                sampleImage.Source = new BitmapImage(new Uri(SampleImageSource, UriKind.Relative));
                cropImage(sampleImage.Source.ToString());
                SetLevel(levelValue.Content.ToString());
                steps = 0;
                stepsValue.Content = steps.ToString();
            }
        }

        private void board_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            lastPositon = e.GetPosition(this);

            foreach (var img in imgList)
                Canvas.SetZIndex(img, 0);

            for (var i = 0; i < imgList.Count; i++)
            {
                if(HitTest(imgList[i]))
                {
                    selectedIndex = i;

                    var img = imgList[selectedIndex];
                    _lastCurIndex = GetCurrentIndex(img);
                    _oldLeft = Canvas.GetLeft(img);
                    _oldTop = Canvas.GetTop(img);
                    Canvas.SetZIndex(img, 1000);
                }
            }
        }

        bool HitTest(Image img)
        {
            var oldLeft = Canvas.GetLeft(img);
            var oldTop = Canvas.GetTop(img);

            return ((oldLeft <= lastPositon.X && lastPositon.X <= oldLeft + size)
                    && (oldTop <= lastPositon.Y && lastPositon.Y <= oldTop + size));
        }

        int emptyIndex;

        private void board_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedIndex == -1)
                return;

            bool check = true;

            var currentPosition = e.GetPosition(this);

            var curX = (int)currentPosition.X / size;
            var curY = (int)currentPosition.Y / size;

            var existedIndexes = new List<bool>() { false, false, false, false, false, false, false, false, false };

            for (var i = 0; i < imgList.Count; i++)
            {
                string[] tag = imgList[i].Tag.ToString().Split(new string[] { "-" }, StringSplitOptions.None);

                if(tag.Length == 2)
                {
                    var index = Convert.ToInt32(tag[1]);
                    existedIndexes[index] = true;
                }
            }

            var indexToStep = curX * 3 + curY;

            for(var k = 0; k < existedIndexes.Count; k++)
            {
                if (existedIndexes[k] == false)
                {
                    emptyIndex = k;
                    break;
                }
            }

            if (indexToStep != emptyIndex)
            {
                check = false;
            }
            else
            {
                int top = _lastCurIndex - 1, bottom = _lastCurIndex + 1, left = _lastCurIndex - 3, right = _lastCurIndex + 3;

                if (indexToStep != top && indexToStep != bottom && indexToStep != left && indexToStep != right)
                {
                    check = false;
                }
            }

            if (check)
            {
                if (isDragging)
                {
                    isDragging = false;

                    var currentPos = e.GetPosition(this);

                    int i = ((int)currentPos.X) / size * size;
                    int j = ((int)currentPos.Y) / size * size;

                    Canvas.SetLeft(imgList[selectedIndex], i);
                    Canvas.SetTop(imgList[selectedIndex], j);

                    CheckImageIndex(selectedIndex,indexToStep);

                    imgList[selectedIndex].Tag = selectedIndex.ToString() + "-" + indexToStep.ToString();

                    steps++;
                    stepsValue.Content = steps.ToString();

                    if (CheckWinning() && min >= 0 && sec >= 0)
                    {
                        timer.Stop();
                        if(MessageBox.Show("YOU WIN !","Congratulation",MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                        {
                            var mainScreen = new MainWindow();
                            this.Hide();
                            mainScreen.ShowDialog();
                            this.Close();
                        }    
                    }
                }
            }
            else
            {
                if (isDragging)
                {
                    isDragging = false;
                    var img = imgList[selectedIndex];
                    Canvas.SetLeft(img, _oldLeft);
                    Canvas.SetTop(img, _oldTop);
                }
            }
        }

        int GetCurrentIndex(Image img)
        {
            var newLeft = Canvas.GetLeft(img);
            var newTop = Canvas.GetTop(img);

            var row = newTop / size;
            var col = newLeft / size;

            return Convert.ToInt32(row + col * 3);
        }

        void CheckImageIndex(int index, int currentIndex)
        {
            if (index == currentIndex)
                checker[index] = true;
            else
                checker[index] = false;
        }

        private void saveGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Save game and exit?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            var file = new System.IO.StreamWriter("gamesaved.txt");

            file.WriteLine(sampleImage.Source);

            foreach (var img in imgList)
                file.WriteLine(img.Tag);

            file.WriteLine(min);
            file.WriteLine(sec);
            file.WriteLine(steps);
            file.WriteLine(levelValue.Content);

            file.Close();
            this.Hide();
            MessageBox.Show("Game saved");
            this.Close();
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Using your mouse to hold and move each image cell to the empty cell such as all cells combine to become as the sample image.", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        bool CheckWinning()
        {
            foreach (bool flag in checker)
                if (flag == false)
                    return flag;
            return true;
        }

        private void board_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedIndex != -1)
            {
                var currentPosition = e.GetPosition(this);

                var img = imgList[selectedIndex];

                var dx = currentPosition.X - lastPositon.X;
                var dy = currentPosition.Y - lastPositon.Y;

                var oldLeft = Canvas.GetLeft(img);
                var oldTop = Canvas.GetTop(img);

                var newLeft = oldLeft + dx;
                var newTop = oldTop + dy;

                if (newLeft < 0)
                    newLeft = 0;
                else if (newLeft + size > board.Width)
                    newLeft = board.Width - size;

                if (newTop < 0)
                    newTop = 0;
                else if (newTop + size > board.Height)
                    newTop = board.Height - size;

                Canvas.SetLeft(img, newLeft);
                Canvas.SetTop(img, newTop);

                lastPositon = currentPosition;
            }
        }
    }
}
