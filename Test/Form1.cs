﻿using System;
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
using OneVision.View.UI.UserControls.LiveView;

namespace Test
{
    public partial class Form1 : Form
    {
	    private int _frameCount;
        private int _count;
	    readonly Mat _frame = new Mat();
	    CountingService _countingService;
	    private VideoCapture videoCapture = new VideoCapture(@"cars.mp4");
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
			videoCapture.ImageGrabbed += ImageGrabbed;
			videoCapture.Start();

	        PolygonOverlay a = new PolygonOverlay(this.imageBox1, Color.Blue);
			a.SetPolygon(imageBox1.Size, new List<PointF>()
			{
				new PointF(50, 300),
				new PointF(800, 300),
				new PointF(600, 50),
				new PointF(50, 20),
				new PointF(10, 150),
			});
			LineOverlay l = new LineOverlay(this.imageBox1, Color.Red);
			l.SetPolygon(imageBox1.Size, new List<PointF>()
			{
				new PointF(20, 70),
				new PointF(800, 280),
			});

		}

	    private void ImageGrabbed(object sender, EventArgs e)
	    {
			((VideoCapture)sender).Retrieve(_frame, 0);
			imageBox1.Image = _frame;
			_countingService.PushFrame(_frame);

		    _frameCount++;
		    if (_frameCount != (int) videoCapture.GetCaptureProperty(CapProp.FrameCount)) return;
		    _frameCount = 0;
		    videoCapture.SetCaptureProperty(CapProp.PosFrames, 0);
	    }
	}
}
