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
using System.Windows.Forms;
using System.ComponentModel;

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

        public gamePlayWindow()
        {
            InitializeComponent();
        }

        void cropImage(string src)
        {
            board.Children.Clear();
            var image = new BitmapImage(new Uri(src));
            var width = image.PixelWidth / 3;
            var height = image.PixelHeight / 3;

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var cropped = new CroppedBitmap(image, new Int32Rect(i * width, j * height, width, height));

                    var img = new Image();

                    img.Source = cropped;
                    const int size = 110;
                    img.Width = size;
                    img.Height = size;

                    if(i != 2 || j != 2)
                    {
                        board.Children.Add(img);
                        Canvas.SetLeft(img, i * (size + 5));
                        Canvas.SetTop(img, j * (size + 5));
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cropImage(sampleImage.Source.ToString());
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            var mainSrceen = new MainWindow();
            this.Hide();
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
                sampleImage.Source = new BitmapImage(new Uri(SampleImageSource, UriKind.Relative));
                cropImage(sampleImage.Source.ToString());
                //System.Windows.MessageBox.Show(sampleImage.Source.ToString());
            }
        }
    }
}
