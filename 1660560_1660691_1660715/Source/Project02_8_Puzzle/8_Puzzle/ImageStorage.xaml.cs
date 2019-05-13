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

namespace _8_Puzzle
{
    /// <summary>
    /// Interaction logic for ImageStorage.xaml
    /// </summary>
    /// 
    public class ImageLoader {
        public string imageSource { get; set; }
    }
    public partial class ImageStorage : Window
    {
        public string SampleImageSource = null;

        public ImageStorage()
        {
            InitializeComponent();

            List<ImageLoader> imageList = new List<ImageLoader>()
            {
                new ImageLoader()
                {
                    imageSource = "Resource/bg1.jpg"
                },
                new ImageLoader()
                {
                    imageSource = "Resource/invention.jpg"
                },
                new ImageLoader()
                {
                    imageSource = "Resource/luxurious.jpg"
                }
            };

            imageListView.ItemsSource = imageList;
        }

        private void imageListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var img = imageListView.SelectedItem as ImageLoader;
            SampleImageSource = img.imageSource;
            this.DialogResult = true;
        }
    }
}
