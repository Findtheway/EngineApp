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

namespace EngineApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int status = 1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void menuitem1_Click(object sender, RoutedEventArgs e)
        {
            menuitem1.Foreground = Brushes.Blue;
            menuitem1.Background = Brushes.LightBlue;
            menuitem2.Foreground = Brushes.Black;
            menuitem2.Background = Brushes.White;
            status = 1;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MIC1 mic = new MIC1(status);           
            mic.Show();
            this.Close();
        }

        private void menuitem2_Click(object sender, RoutedEventArgs e)
        {
            menuitem1.Foreground = Brushes.Black;
            menuitem1.Background = Brushes.White;
            menuitem2.Foreground = Brushes.Blue;
            menuitem2.Background = Brushes.LightBlue;
            status = 2;
        }
    }
}
