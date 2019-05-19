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
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        public string level;
        public OptionWindow()
        {
            InitializeComponent();

            easyOption.IsChecked = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if(easyOption.IsChecked == true)
            {
                level = "easy";
            }
            else if(mediumOption.IsChecked == true)
            {
                level = "medium";
            }
            else
            {
                level = "hard";
            }
            this.DialogResult = true;
        }
    }
}
