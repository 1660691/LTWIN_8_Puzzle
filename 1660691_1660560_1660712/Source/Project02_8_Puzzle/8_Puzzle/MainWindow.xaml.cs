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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _8_Puzzle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string level;

        public MainWindow()
        {
            InitializeComponent();
            level = "easy";
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OptionWindow();
            screen.ShowDialog();

            if (screen.DialogResult == true)
            {
                level = screen.level;
                var playScreen = new gamePlayWindow(level, true);
                this.Hide();
                playScreen.ShowDialog();
                this.Close();
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Exit game?", "",MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This game is one of our mini-projects in Windows Programming 2019. Our team has three members who are 1660691 - Tue Vo (Leader), 1660560 - Thanh Pham and 1660712 - Vien Huynh. Have fun and hope you enjoy it!", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
