using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cam_counting;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Test
{
    public partial class Form1 : Form
    {
        private int _count;
	    readonly Mat _frame = new Mat();
	    CountingService _countingService;
		public Form1()
        {
            InitializeComponent();
        }

        private void Decrement(object sender, EventArgs eventArgs)
        {

        }

        private void Increment(object sender, EventArgs eventArgs)
        {
            _count++;

            label1.Invoke((MethodInvoker)delegate
            {
                try
                {
                    label1.Text = _count.ToString();
                }
                catch { }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _countingService = new CountingService();
            _countingService.Increment += Increment;
            _countingService.Decrement += Decrement;

            var videoCapture = new VideoCapture(@"cars.mp4");
            videoCapture.ImageGrabbed += ImageGrabbed;
			videoCapture.Start();
        }

	    private void ImageGrabbed(object sender, EventArgs e)
	    {
			((VideoCapture)sender).Retrieve(_frame, 0);
			imageBox1.Image = _frame;
			_countingService.PushFrame(_frame);
		}
	}
}
