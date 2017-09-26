using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AxisPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }

        private void Play()
        {
            List<AxAXISMEDIACONTROLLib.AxAxisMediaControl> lstPlayer = new List<AxAXISMEDIACONTROLLib.AxAxisMediaControl>();
            lstPlayer.Add(axAxisMediaControl1);
            lstPlayer.Add(axAxisMediaControl2);
            lstPlayer.Add(axAxisMediaControl3);
            lstPlayer.Add(axAxisMediaControl4);
            lstPlayer.Add(axAxisMediaControl5);
            lstPlayer.Add(axAxisMediaControl6);
            lstPlayer.Add(axAxisMediaControl7);
            lstPlayer.Add(axAxisMediaControl8);
            lstPlayer.Add(axAxisMediaControl9);
            lstPlayer.Add(axAxisMediaControl10);
            lstPlayer.Add(axAxisMediaControl11);
            lstPlayer.Add(axAxisMediaControl12);
            lstPlayer.Add(axAxisMediaControl13);
            lstPlayer.Add(axAxisMediaControl14);
            lstPlayer.Add(axAxisMediaControl15);
            lstPlayer.Add(axAxisMediaControl16);
            lstPlayer.Add(axAxisMediaControl17);
            lstPlayer.Add(axAxisMediaControl18);
            lstPlayer.Add(axAxisMediaControl19);
            lstPlayer.Add(axAxisMediaControl20);
            lstPlayer.Add(axAxisMediaControl21);

            foreach (var item in lstPlayer)
            {
                item.MediaURL = textBox1.Text;// "http://root:root@localhost:40001/axis-cgi/mjpg/video.cgi?resolution=1280x720&FPS=15";
                item.Play();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Play();
        }
    }
}
