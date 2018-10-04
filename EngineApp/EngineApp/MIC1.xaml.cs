using System;
using System.Collections.Generic;
using System.Configuration;
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
using Util;

namespace EngineApp
{
    /// <summary>
    /// MIC1.xaml 的交互逻辑
    /// </summary>
    public partial class MIC1 : Window
    {
        private int type = 1;
        private string echo_len = "100";
        private string mic_interval = "0.06";
        private string select_angle = "-30";
        private bool aes_status = true;
        private string aes_level = "1";
        private string nr_level = "1";
        private bool agc_status = true;
        private bool drc_status = true;
        private string gain = "4";

        public MIC1(int status)
        {
            InitializeComponent();
            this.type = status;
            initMic(type);
            
        }

        private void aecmenuitem_Click(object sender, RoutedEventArgs e)
        {
            menuitem_Click((MenuItem)sender, aecmenuitem1, aecmenuitem2, aecmenuitem3);
            echo_len = ((MenuItem)sender).Header.ToString();

        }

        private void doamenuitem_Click(object sender, RoutedEventArgs e)
        {
            menuitem_Click((MenuItem)sender,doamenuitem1,doamenuitem2,doamenuitem3);
            if (((MenuItem)sender).Header.ToString().Equals("6cm")) {
                mic_interval = "0.06";
            }
            else if (((MenuItem)sender).Header.ToString().Equals("8cm")) {
                mic_interval = "0.08";
            }
            else if (((MenuItem)sender).Header.ToString().Equals("10cm")) {
                mic_interval = "0.1";
            }
        }

        private void beammenuitem_Click(object sender, RoutedEventArgs e) {
            menuitem_Click((MenuItem)sender, beammenuitem1, beammenuitem2, beammenuitem3);
            select_angle = ((MenuItem)sender).Header.ToString().TrimEnd('°');
        }

        private void aesmenuitemon_Click(object sender, RoutedEventArgs e) {
            MenuItem nowMenuItem=(MenuItem)sender;
            nowMenuItem.Foreground = Brushes.Blue;
            if (nowMenuItem.Name.ToLower().EndsWith("on"))
            {
                aesmenuitemoff.Foreground = Brushes.Black;
                aesstatusbar2.IsEnabled = true;
                aes_status = true;
            }
            else
            {
                aesmenuitemon.Foreground = Brushes.Black;
                aesstatusbar2.IsEnabled = false;
                aes_status = false;
            }
            
        }

        private void aesmenuitem_Click(object sender, RoutedEventArgs e) {
            menuitem_Click((MenuItem)sender, aesmenuitem1, aesmenuitem2, aesmenuitem3);
            aes_level = ((MenuItem)sender).Header.ToString();
        }

        private void nrmenuitem_Click(object sender, RoutedEventArgs e) {
            menuitem_Click((MenuItem)sender, nrmenuitem1, nrmenuitem2, nrmenuitem3);
            nr_level= ((MenuItem)sender).Header.ToString();
        }

        private void agcmenuitemon_Click(object sender, RoutedEventArgs e) {
            MenuItem nowMenuItem = (MenuItem)sender;
            nowMenuItem.Foreground = Brushes.Blue;
            if (nowMenuItem.Name.ToLower().EndsWith("on"))
            {
                agcmenuitemoff.Foreground = Brushes.Black;
                agc_status = true;
            }
            else
            {
                agcmenuitemon.Foreground = Brushes.Black;
                agc_status = false;
            }
        }

        private void drcmenuitemon_Click(object sender, RoutedEventArgs e) {
            MenuItem nowMenuItem = (MenuItem)sender;
            nowMenuItem.Foreground = Brushes.Blue;
            if (nowMenuItem.Name.ToLower().EndsWith("on"))
            {
                drcmenuitemoff.Foreground = Brushes.Black;
                drcstatusbar2.IsEnabled = true;
                drc_status = true;
            }
            else
            {
                drcmenuitemon.Foreground = Brushes.Black;
                drcstatusbar2.IsEnabled = false;
                drc_status = false;
            }
        }

        private void drcmenuitem_Click(object sender, RoutedEventArgs e) {
            menuitem_Click((MenuItem)sender, drcmenuitem1, drcmenuitem2, drcmenuitem3);
            gain= ((MenuItem)sender).Header.ToString();
        }

        private void initMic(int type) {
            if (type == 1)
            {
                nowimg.Source = new BitmapImage(new Uri(@"img\micflow\omic.png", UriKind.Relative));
                doalable.IsEnabled = false;
                doastatusbar.IsEnabled = false;
                beamlable.IsEnabled = false;
                beamstatusbar.IsEnabled = false;

            }
            else if (type == 2)
            {
                nowimg.Source = new BitmapImage(new Uri(@"img\micflow\dmic.png", UriKind.Relative));
                doamenuitem1.Foreground = Brushes.Blue;
                beammenuitem1.Foreground = Brushes.Blue;
            }
            aecmenuitem1.Foreground = Brushes.Blue;
            aesmenuitemon.Foreground = Brushes.Blue;
            aesmenuitem1.Foreground = Brushes.Blue;
            nrmenuitem1.Foreground = Brushes.Blue;
            agcmenuitemon.Foreground = Brushes.Blue;
            drcmenuitemon.Foreground = Brushes.Blue;
            drcmenuitem1.Foreground = Brushes.Blue;
        }

        private void menuitem_Click(MenuItem nowMenuItem, MenuItem menuItem1, MenuItem menuItem2, MenuItem menuItem3)
        {
            nowMenuItem.Foreground = Brushes.Blue;
            if (nowMenuItem.Name.EndsWith("1"))
            {
                menuItem2.Foreground = Brushes.Black;
                menuItem3.Foreground = Brushes.Black;
            }
            else if (nowMenuItem.Name.EndsWith("2"))
            {
                menuItem1.Foreground = Brushes.Black;
                menuItem3.Foreground = Brushes.Black;
            }
            else if(nowMenuItem.Name.EndsWith("3"))
            {
                menuItem1.Foreground = Brushes.Black;
                menuItem2.Foreground = Brushes.Black;
            }
        }

        private void configbtn_Click(object sender, RoutedEventArgs e)//生成配置文件
        {
            MICSetting micSetting = new MICSetting();
            micSetting.AEC_Length = echo_len;
            micSetting.DOA_MIC_Interval = mic_interval;
            micSetting.BF_Select_Angle = select_angle;
            micSetting.AES_Status = aes_status;
            micSetting.AES_Level = aes_level;
            micSetting.NR_Level = nr_level;
            micSetting.AGC_Status = agc_status;
            micSetting.DRC_Status = drc_status;
            micSetting.DRC_Gain = gain;
            micSetting.MIC_Type = type.ToString();
            FileHelper fileHelper = new FileHelper();
            try {
                fileHelper.CreateMICConf(micSetting, ConfigurationManager.AppSettings["LocalMICConfPath"]);
                MessageBox.Show("Configuration file successfully");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void downloadbtn_Click(object sender, RoutedEventArgs e)//下载配置文件到机车服务器
        {

        }

        private void uploadbtn_Click(object sender, RoutedEventArgs e)//上传配置文件到机车服务器
        {

        }
    }
}
