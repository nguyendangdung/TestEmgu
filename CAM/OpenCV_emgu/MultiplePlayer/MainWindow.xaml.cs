using Emgu.CV;
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

namespace MultiplePlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string streamUrl = @"rtsp://giangnt:123456@10.16.0.200/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast";
        //const string streamUrl = @"rtsp://giangnt:123456@10.16.0.200/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=multicast";
        //const string streamUrl = @"D:\Kong.Skull.Island.2017.ViE.mHD.BluRay.DD5.1.x264-EPiK\Kong.Skull.Island.2017.ViE.mHD.BluRay.DD5.1.x264-EPiK.mkv";
        //const string streamUrl = "rtsp://10.16.0.100:8295/proxyStream-1";
        //const string streamUrl = "rtsp://10.16.0.100:8201/proxyStream";
        //const string streamUrl = "rtsp://192.168.2.11:8554/video.mkv";

        List<Player> _lstPlayer = new List<Player>();
        public MainWindow()
        {
            InitializeComponent();
            ckUseOpenCL.IsChecked = CvInvoke.UseOpenCL = CvInvoke.HaveOpenCL;
            ckUseOptimized.IsChecked = CvInvoke.UseOptimized;
            ckShowImage.IsChecked = true;
            ckGLImageView.IsChecked = true;
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(playerCount.Text);
            _lstPlayer = grdMain.Children.OfType<Player>().Take(count).ToList();
            foreach (var player in _lstPlayer)
            {
                player.PlayAsync(streamUrl, ckShowImage.IsChecked.Value, ckGLImageView.IsChecked.Value);
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            foreach (var player in _lstPlayer)
            {
                player.StopAsync();
            }
        }

        private void ckUseOpenCL_Checked(object sender, RoutedEventArgs e)
        {
            CvInvoke.UseOpenCL = ((CheckBox)sender).IsChecked.Value;
        }

        private void ckUseOptimized_Checked(object sender, RoutedEventArgs e)
        {
            CvInvoke.UseOptimized = ((CheckBox)sender).IsChecked.Value;
        }
    }
}
