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

        List<Image> imgList;
        const int size = 175;
        static bool[] checker;
        int steps;
        DispatcherTimer timer;
        int min, sec;

        public gamePlayWindow(string level)
        {
            InitializeComponent();
            levelValue.Content = level;
            SetLevel(level);
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
            cropImage(sampleImage.Source.ToString());

            steps = 0;
            stepsValue.Content = steps.ToString();

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();


        }

        void cropImage(string src)
        {
            var image = new BitmapImage(new Uri(src));
            var width = image.PixelWidth / 3;
            var height = image.PixelHeight / 3;

            board.Children.Clear();
            imgList = new List<Image>();
            checker = new bool[8];

            for (var i = 0; i < 8; i++)
                checker[i] = false;

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

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var currentIndex = i * 3 + j;
                    if (i != 2 || j != 2)
                    {
                        var randomIndex = random.Next(indices.Count);
                        var index = indices[randomIndex];

                        var tag = imgList[index].Tag;

                        imgList[index].Tag = tag.ToString() + "-" + currentIndex.ToString();

                        CheckImageIndex(index, currentIndex);

                        board.Children.Add(imgList[index]);

                        Canvas.SetLeft(imgList[index], i * size);
                        Canvas.SetTop(imgList[index], j * size);

                        indices.RemoveAt(randomIndex);
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

        bool isDragging = false;
        Point lastPositon;
        int selectedIndex = -1;
        double _oldLeft, _oldTop;
        int _lastCurIndex;

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

        private void board_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedIndex == -1)
                return;

            bool check = true;

            var currentPosition = e.GetPosition(this);

            var curX = (int)currentPosition.X / size;
            var curY = (int)currentPosition.Y / size;

            var indexToStep =  Convert.ToInt32(curX*3 + curY);

            List<int> existedIndexes = new List<int>();

            for(var i = 0; i < imgList.Count; i++)
            {
                string[] tag = imgList[i].Tag.ToString().Split(new string[] { "-" }, StringSplitOptions.None);
                if (tag.Length == 2)
                {
                    existedIndexes.Add(Convert.ToInt32(tag[1]));
                }
            }

            foreach (var index in existedIndexes)
            {
                if(indexToStep == index)
                {
                    check = false;
                    break;
                }
                else
                {
                    if (indexToStep % 2 == 0 && _lastCurIndex % 2 == 0 || indexToStep % 2 != 0 && _lastCurIndex % 2 != 0)
                    {
                        check = false;
                        break;
                    }
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

                    var currentIndex = GetCurrentIndex(imgList[selectedIndex]);

                    CheckImageIndex(selectedIndex, currentIndex);

                    imgList[selectedIndex].Tag = selectedIndex.ToString() + "-" + currentIndex.ToString();
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
