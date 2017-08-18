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

namespace Test
{
	public partial class Form1 : Form
	{
		private readonly ICountingService _countingService;
		private VideoCapture _videoCapture;
		private int _count = 0;
		public Form1()
		{
			InitializeComponent();
			_countingService = new CountingService();
			_countingService.Increment += Increment;
			_countingService.Decrement += Decrement;
		}

		private void Decrement(object sender, EventArgs eventArgs)
		{
			
		}

		private void Increment(object sender, EventArgs eventArgs)
		{
			_count++;
			label1.Text = _count.ToString();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_videoCapture = new VideoCapture(@"cars.mp4");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			while (true)
			{
				var frame = _videoCapture.QueryFrame();

				if (frame != null)
				{
					imageBox1.Image = frame;
					_countingService.PushFrame(frame);
				}
				else
				{
					break;
				}
				Application.DoEvents();
			}
		}
	}
}
