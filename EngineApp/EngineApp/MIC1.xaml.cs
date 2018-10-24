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
        private string serverIp = "192.168.1.4";
        private string serverPort = "22";
        private string serverUser = "root";
        private string serverPWD = "root";
        private string serverConfPath = "/var/speech/dsp_config/";

        public MIC1(int status)
        {
            InitializeComponent();
            //loading.Visibility = Visibility.Visible;
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
            menuitem_Click((MenuItem)sender, doamenuitem1, doamenuitem2, doamenuitem3);
            if (((MenuItem)sender).Header.ToString().Equals("6cm"))
            {
                mic_interval = "0.06";
            }
            else if (((MenuItem)sender).Header.ToString().Equals("8cm"))
            {
                mic_interval = "0.08";
            }
            else if (((MenuItem)sender).Header.ToString().Equals("10cm"))
            {
                mic_interval = "0.1";
            }
        }

        private void beammenuitem_Click(object sender, RoutedEventArgs e)
        {
            menuitem_Click((MenuItem)sender, beammenuitem1, beammenuitem2, beammenuitem3);
            select_angle = ((MenuItem)sender).Header.ToString().TrimEnd('°');
        }

        private void aesmenuitemon_Click(object sender, RoutedEventArgs e)
        {
            MenuItem nowMenuItem = (MenuItem)sender;
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

        private void aesmenuitem_Click(object sender, RoutedEventArgs e)
        {
            menuitem_Click((MenuItem)sender, aesmenuitem1, aesmenuitem2, aesmenuitem3);
            aes_level = ((MenuItem)sender).Header.ToString();
        }

        private void nrmenuitem_Click(object sender, RoutedEventArgs e)
        {
            menuitem_Click((MenuItem)sender, nrmenuitem1, nrmenuitem2, nrmenuitem3);
            nr_level = ((MenuItem)sender).Header.ToString();
        }

        private void agcmenuitemon_Click(object sender, RoutedEventArgs e)
        {
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

        private void drcmenuitemon_Click(object sender, RoutedEventArgs e)
        {
            MenuItem nowMenuItem = (MenuItem)sender;
            nowMenuItem.Foreground = Brushes.Blue;
            nowMenuItem.Background = Brushes.LightBlue;
            if (nowMenuItem.Name.ToLower().EndsWith("on"))
            {
                drcmenuitemoff.Foreground = Brushes.Black;
                drcmenuitemoff.Background = Brushes.White;
                drcstatusbar2.IsEnabled = true;
                drc_status = true;
            }
            else
            {
                drcmenuitemon.Foreground = Brushes.Black;
                drcmenuitemon.Background = Brushes.White;
                drcmenuitem1.Foreground = Brushes.Black;
                drcmenuitem1.Background = Brushes.White;
                drcmenuitem2.Foreground = Brushes.Black;
                drcmenuitem2.Background = Brushes.White;
                drcmenuitem3.Foreground = Brushes.Black;
                drcmenuitem3.Background = Brushes.White;
                drcstatusbar2.IsEnabled = false;
                drc_status = false;
            }
        }

        private void drcmenuitem_Click(object sender, RoutedEventArgs e)
        {
            menuitem_Click((MenuItem)sender, drcmenuitem1, drcmenuitem2, drcmenuitem3);
            gain = ((MenuItem)sender).Header.ToString();
        }

        private void initMic(int type)
        {
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
                doamenuitem1.Background = Brushes.LightBlue;
                beammenuitem1.Foreground = Brushes.Blue;
                beammenuitem1.Background = Brushes.LightBlue;
            }
            agclable.IsEnabled = false;
            agcstatusbar.IsEnabled = false;
            aeslable.IsEnabled = false;
            aesstatusbar1.IsEnabled = false;
            aesstatusbar2.IsEnabled = false;
            aecmenuitem1.Foreground = Brushes.Blue;
            aecmenuitem1.Background = Brushes.LightBlue;
            aesmenuitemon.Foreground = Brushes.Blue;
            //aesmenuitemon.Background = Brushes.LightBlue;
            aesmenuitem1.Foreground = Brushes.Blue;
            //aesmenuitem1.Background = Brushes.LightBlue;
            nrmenuitem1.Foreground = Brushes.Blue;
            nrmenuitem1.Background = Brushes.LightBlue;
            //agcmenuitemon.Foreground = Brushes.Blue;
            drcmenuitemon.Foreground = Brushes.Blue;
            drcmenuitemon.Background = Brushes.LightBlue;
            drcmenuitem1.Foreground = Brushes.Blue;
            drcmenuitem1.Background = Brushes.LightBlue;
        }

        private void menuitem_Click(MenuItem nowMenuItem, MenuItem menuItem1, MenuItem menuItem2, MenuItem menuItem3)
        {
            nowMenuItem.Foreground = Brushes.Blue;
            nowMenuItem.Background = Brushes.LightBlue;
            if (nowMenuItem.Name.EndsWith("1"))
            {
                menuItem2.Foreground = Brushes.Black;
                menuItem2.Background = Brushes.White;
                menuItem3.Foreground = Brushes.Black;
                menuItem3.Background = Brushes.White;
            }
            else if (nowMenuItem.Name.EndsWith("2"))
            {
                menuItem1.Foreground = Brushes.Black;
                menuItem1.Background = Brushes.White;
                menuItem3.Foreground = Brushes.Black;
                menuItem3.Background = Brushes.White;
            }
            else if (nowMenuItem.Name.EndsWith("3"))
            {
                menuItem1.Foreground = Brushes.Black;
                menuItem1.Background = Brushes.White;
                menuItem2.Foreground = Brushes.Black;
                menuItem2.Background = Brushes.White;
            }
        }

        private void configbtn_Click(object sender, RoutedEventArgs e)//生成配置文件
        {
            MICSetting micSetting = new MICSetting();
            micSetting.AEC_Length = echo_len;
            micSetting.DOA_MIC_Interval = mic_interval;
            micSetting.BF_Select_Angle = select_angle;
            micSetting.AES_Status = aes_status;
            micSetting.AES_Level = aesmenuitem3.Header.ToString();
            micSetting.NR_Level = nr_level;
            micSetting.AGC_Status = agc_status;
            micSetting.DRC_Status = drc_status;
            micSetting.DRC_Gain = gain;
            micSetting.MIC_Type = type.ToString();
            FileHelper fileHelper = new FileHelper();
            try
            {
                fileHelper.CreateMICConf(micSetting,System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"cfgfile") , ConfigurationManager.AppSettings["LocalConfFileName"]);
                MessageBox.Show("Configuration file successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async void downloadbtn_Click(object sender, RoutedEventArgs e)//下载配置文件到机车服务器
        {
            try {
                loading.Visibility = Visibility.Visible;
                FileHelper fileHelper = new FileHelper();
                //await fileHelper.UploadMICConfgToServer(
                //     System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfgfile", ConfigurationManager.AppSettings["LocalConfFileName"]),
                //     ConfigurationManager.AppSettings["ServerConfPath"] + ConfigurationManager.AppSettings["LocalConfFileName"],
                //     ConfigurationManager.AppSettings["ServerIP"],
                //     ConfigurationManager.AppSettings["ServerPort"],
                //     ConfigurationManager.AppSettings["ServerUser"],
                //     ConfigurationManager.AppSettings["ServerPassword"]
                //     );
                setServerConf();
                await fileHelper.UploadMICConfgToServer(
                     System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfgfile", ConfigurationManager.AppSettings["LocalConfFileName"]),
                     serverConfPath + ConfigurationManager.AppSettings["LocalConfFileName"],
                     serverIp,
                     serverPort,
                     serverUser,
                     serverPWD
                     );

                this.HideLoading(false);
                MessageBox.Show("WriteToHU successfully");
            }
            catch (Exception ex) {
                loading.Visibility = Visibility.Hidden;
                MessageBox.Show(ex.Message);
            }
            
        }

        private async void uploadbtn_Click(object sender, RoutedEventArgs e)//上传配置文件到机车服务器
        {
            try {
                loading.Visibility = Visibility.Visible;
                FileHelper fileHelper = new FileHelper();
                setServerConf();
                //await fileHelper.DownLoadMICConfg(
                //    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfgfile"),
                //    ConfigurationManager.AppSettings["ServerConfPath"] + ConfigurationManager.AppSettings["LocalConfFileName"],
                //    ConfigurationManager.AppSettings["ServerIP"],
                //    ConfigurationManager.AppSettings["ServerPort"],
                //    ConfigurationManager.AppSettings["ServerUser"],
                //    ConfigurationManager.AppSettings["ServerPassword"]
                //    );
                await fileHelper.DownLoadMICConfg(
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfgfile"),
                    serverConfPath + ConfigurationManager.AppSettings["LocalConfFileName"],
                    serverIp,
                    serverPort,
                    serverUser,
                    serverPWD
                    );

                this.HideLoading(true);
                MessageBox.Show("ReadFromHU and test successfully");
            }
            catch (Exception ex) {
                loading.Visibility = Visibility.Hidden;
                MessageBox.Show(ex.Message);
            }
            
        }

        public void HideLoading(bool flag)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.loading.Visibility = Visibility.Collapsed;
                if (flag)
                {
                    this.refreshConfSet();
                }
                else
                {
                    SCPOperation.RestartDSP(
                        serverIp,
                        serverPort,
                        serverUser,
                        serverPWD,
                        this.type);
                }
            });
        }

        public void refreshConfSet() {
            FileHelper fileHelper = new FileHelper();
            MICSetting nowSetting = fileHelper.readMicConfFile(
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfgfile"),
                ConfigurationManager.AppSettings["LocalConfFileName"]);
            if (nowSetting != null)
            {
                this.initMic(Convert.ToInt32(nowSetting.MIC_Type));
                if (nowSetting.AEC_Length.Equals("100"))
                {
                    aecmenuitem1.Foreground = Brushes.Blue;
                    aecmenuitem1.Background = Brushes.LightBlue;
                    aecmenuitem2.Foreground = Brushes.Black;
                    aecmenuitem2.Background = Brushes.White;
                    aecmenuitem3.Foreground = Brushes.Black;
                    aecmenuitem3.Background = Brushes.White;
                }
                else if (nowSetting.AEC_Length.Equals("160"))
                {
                    aecmenuitem1.Foreground = Brushes.Black;
                    aecmenuitem1.Background = Brushes.White;
                    aecmenuitem2.Foreground = Brushes.Blue;
                    aecmenuitem2.Background = Brushes.LightBlue;
                    aecmenuitem3.Foreground = Brushes.Black;
                    aecmenuitem3.Background = Brushes.White;
                }
                else if (nowSetting.AEC_Length.Equals("200"))
                {
                    aecmenuitem1.Foreground = Brushes.Black;
                    aecmenuitem1.Background = Brushes.White;
                    aecmenuitem2.Foreground = Brushes.Black;
                    aecmenuitem2.Background = Brushes.White;
                    aecmenuitem3.Foreground = Brushes.Blue;
                    aecmenuitem3.Background = Brushes.LightBlue;
                }
                if (nowSetting.MIC_Type.Equals("2"))
                {
                    if (nowSetting.DOA_MIC_Interval.Equals("0.06"))
                    {
                        doamenuitem1.Foreground = Brushes.Blue;
                        doamenuitem1.Background = Brushes.LightBlue;
                        doamenuitem2.Foreground = Brushes.Black;
                        doamenuitem2.Background = Brushes.White;
                        doamenuitem3.Foreground = Brushes.Black;
                        doamenuitem3.Background = Brushes.White;
                    }
                    else if (nowSetting.DOA_MIC_Interval.Equals("0.08"))
                    {
                        doamenuitem1.Foreground = Brushes.Black;
                        doamenuitem1.Background = Brushes.White;
                        doamenuitem2.Foreground = Brushes.Blue;
                        doamenuitem2.Background = Brushes.LightBlue;
                        doamenuitem3.Foreground = Brushes.Black;
                        doamenuitem3.Background = Brushes.White;
                    }
                    else if (nowSetting.DOA_MIC_Interval.Equals("0.1"))
                    {
                        doamenuitem1.Foreground = Brushes.Black;
                        doamenuitem1.Background = Brushes.White;
                        doamenuitem2.Foreground = Brushes.Black;
                        doamenuitem2.Background = Brushes.White;
                        doamenuitem3.Foreground = Brushes.Blue;
                        doamenuitem3.Background = Brushes.LightBlue;
                    }
                    if (nowSetting.BF_Select_Angle.Equals("-30"))
                    {
                        beammenuitem1.Foreground = Brushes.Blue;
                        beammenuitem1.Background = Brushes.LightBlue;
                        beammenuitem2.Foreground = Brushes.Black;
                        beammenuitem2.Background = Brushes.White;
                        beammenuitem3.Foreground = Brushes.Black;
                        beammenuitem3.Background = Brushes.White;
                    }
                    else if (nowSetting.BF_Select_Angle.Equals("-45"))
                    {
                        beammenuitem1.Foreground = Brushes.Black;
                        beammenuitem1.Background = Brushes.White;
                        beammenuitem2.Foreground = Brushes.Blue;
                        beammenuitem2.Background = Brushes.LightBlue;
                        beammenuitem3.Foreground = Brushes.Black;
                        beammenuitem3.Background = Brushes.White;
                    }
                    else if (nowSetting.BF_Select_Angle.Equals("-60"))
                    {
                        beammenuitem1.Foreground = Brushes.Black;
                        beammenuitem1.Background = Brushes.White;
                        beammenuitem2.Foreground = Brushes.Black;
                        beammenuitem2.Background = Brushes.White;
                        beammenuitem3.Foreground = Brushes.Blue;
                        beammenuitem3.Background = Brushes.LightBlue;
                    }
                }
                if (nowSetting.AES_Status)
                {
                    aesmenuitemon.Foreground = Brushes.Blue;
                    aesmenuitemon.Background = Brushes.LightBlue;
                    aesmenuitemoff.Foreground = Brushes.Black;
                    aesmenuitemoff.Background = Brushes.White;
                    aesstatusbar2.IsEnabled = true;
                    if (nowSetting.AES_Level.Equals("1"))
                    {
                        aesmenuitem1.Foreground = Brushes.Blue;
                        aesmenuitem1.Background = Brushes.LightBlue;
                        aesmenuitem2.Foreground = Brushes.Black;
                        aesmenuitem2.Background = Brushes.White;
                        aesmenuitem3.Foreground = Brushes.Black;
                        aesmenuitem3.Background = Brushes.White;
                    }
                    else if (nowSetting.AES_Level.Equals("2"))
                    {
                        aesmenuitem1.Foreground = Brushes.Black;
                        aesmenuitem1.Background = Brushes.White;
                        aesmenuitem2.Foreground = Brushes.Blue;
                        aesmenuitem2.Background = Brushes.LightBlue;
                        aesmenuitem3.Foreground = Brushes.Black;
                        aesmenuitem3.Background = Brushes.White;
                    }
                    else if (nowSetting.AES_Level.Equals("3"))
                    {
                        aesmenuitem1.Foreground = Brushes.Black;
                        aesmenuitem1.Background = Brushes.White;
                        aesmenuitem2.Foreground = Brushes.Black;
                        aesmenuitem2.Background = Brushes.White;
                        aesmenuitem3.Foreground = Brushes.Blue;
                        aesmenuitem3.Background = Brushes.LightBlue;
                    }
                }
                else
                {
                    aesmenuitemon.Foreground = Brushes.Black;
                    aesmenuitemon.Background = Brushes.White;
                    aesmenuitemoff.Foreground = Brushes.Blue;
                    aesmenuitemoff.Background = Brushes.LightBlue;
                    aesstatusbar2.IsEnabled = false;
                    aesmenuitem1.Foreground = Brushes.Black;
                    aesmenuitem1.Background = Brushes.White;
                    aesmenuitem2.Foreground = Brushes.Black;
                    aesmenuitem2.Background = Brushes.White;
                    aesmenuitem3.Foreground = Brushes.Black;
                    aesmenuitem3.Background = Brushes.White;
                }
                if (nowSetting.NR_Level.Equals("1"))
                {
                    nrmenuitem1.Foreground = Brushes.Blue;
                    nrmenuitem1.Background = Brushes.LightBlue;
                    nrmenuitem2.Foreground = Brushes.Black;
                    nrmenuitem2.Background = Brushes.White;
                    nrmenuitem3.Foreground = Brushes.Black;
                    nrmenuitem3.Background = Brushes.White;
                }
                else if (nowSetting.NR_Level.Equals("2"))
                {
                    nrmenuitem1.Foreground = Brushes.Black;
                    nrmenuitem1.Background = Brushes.White;
                    nrmenuitem2.Foreground = Brushes.Blue;
                    nrmenuitem2.Background = Brushes.LightBlue;
                    nrmenuitem3.Foreground = Brushes.Black;
                    nrmenuitem3.Background = Brushes.White;
                }
                else if (nowSetting.NR_Level.Equals("3"))
                {
                    nrmenuitem1.Foreground = Brushes.Black;
                    nrmenuitem1.Background = Brushes.White;
                    nrmenuitem2.Foreground = Brushes.Black;
                    nrmenuitem2.Background = Brushes.White;
                    nrmenuitem3.Foreground = Brushes.Blue;
                    nrmenuitem3.Background = Brushes.LightBlue;
                }
                //if (nowSetting.AGC_Status)
                //{
                //    agcmenuitemon.Foreground = Brushes.Blue;
                //    agcmenuitemoff.Foreground = Brushes.Black;
                //}
                //else
                //{
                //    agcmenuitemon.Foreground = Brushes.Black;
                //    agcmenuitemoff.Foreground = Brushes.Blue;
                //}
                if (nowSetting.DRC_Status)
                {
                    drcmenuitemon.Foreground = Brushes.Blue;
                    drcmenuitemon.Background = Brushes.LightBlue;
                    drcmenuitemoff.Foreground = Brushes.Black;
                    drcmenuitemoff.Background = Brushes.White;
                    drcstatusbar2.IsEnabled = true;
                    if (nowSetting.DRC_Gain.Equals("4"))
                    {
                        drcmenuitem1.Foreground = Brushes.Blue;
                        drcmenuitem1.Background = Brushes.LightBlue;
                        drcmenuitem2.Foreground = Brushes.Black;
                        drcmenuitem2.Background = Brushes.White;
                        drcmenuitem3.Foreground = Brushes.Black;
                        drcmenuitem3.Background = Brushes.White;
                    }
                    else if (nowSetting.DRC_Gain.Equals("6"))
                    {
                        drcmenuitem1.Foreground = Brushes.Black;
                        drcmenuitem1.Background = Brushes.White;
                        drcmenuitem2.Foreground = Brushes.Blue;
                        drcmenuitem2.Background = Brushes.LightBlue;
                        drcmenuitem3.Foreground = Brushes.Black;
                        drcmenuitem3.Background = Brushes.White;
                    }
                    else if (nowSetting.DRC_Gain.Equals("8"))
                    {
                        drcmenuitem1.Foreground = Brushes.Black;
                        drcmenuitem1.Background = Brushes.White;
                        drcmenuitem2.Foreground = Brushes.Black;
                        drcmenuitem2.Background = Brushes.White;
                        drcmenuitem3.Foreground = Brushes.Blue;
                        drcmenuitem3.Background = Brushes.LightBlue;
                    }
                }
                else
                {
                    drcmenuitemon.Foreground = Brushes.Black;
                    drcmenuitemon.Background = Brushes.White;
                    drcmenuitemoff.Foreground = Brushes.Blue;
                    drcmenuitemoff.Background = Brushes.LightBlue;
                    drcstatusbar2.IsEnabled = false;
                    drcmenuitem1.Foreground = Brushes.Black;
                    drcmenuitem1.Background = Brushes.White;
                    drcmenuitem2.Foreground = Brushes.Black;
                    drcmenuitem2.Background = Brushes.White;
                    drcmenuitem3.Foreground = Brushes.Black;
                    drcmenuitem3.Background = Brushes.White;
                }
            }
            else {
                MessageBox.Show(
                    string.Format("\"{0}{1}\"is not exist",
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfgfile"), 
                    ConfigurationManager.AppSettings["LocalConfFileName"]));
            }
        }

        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void setServerConf() {
            if (!string.IsNullOrEmpty(ServerIPText.Text)) {
                serverIp = ServerIPText.Text;
            }
            if (!string.IsNullOrEmpty(ServerPortText.Text))
            {
                serverPort = ServerPortText.Text;
            }
            if (!string.IsNullOrEmpty(ServerUserText.Text))
            {
                serverUser = ServerUserText.Text;
            }
            if (!string.IsNullOrEmpty(ServerPWDText.Text))
            {
                serverPWD = ServerPWDText.Text;
            }
            if (!string.IsNullOrEmpty(ServerConfPathText.Text))
            {
                serverConfPath = ServerConfPathText.Text;
            }
        }
    }
}
