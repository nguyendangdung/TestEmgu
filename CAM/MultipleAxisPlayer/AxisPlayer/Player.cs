using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxAXISMEDIACONTROLLib;

namespace AxisPlayer
{
    public partial class Player : Form
    {
        public Player()
        {
            InitializeComponent();
        }

        public void Play()
        {
            axAxisMediaControl1.EnableJoystick = true;
            //axAxisMediaControl1.FullScreen = true;
            axAxisMediaControl1.EnableContextMenu = true;
            axAxisMediaControl1.MediaURL = "http://root:root@192.168.2.23:40001/axis-cgi/mjpg/video.cgi?resolution=1280x720&FPS=15";
            axAxisMediaControl1.Play();
        }

        private void axAxisMediaControl1_OnError(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEvent e)
        {
            
        }
    }
}
