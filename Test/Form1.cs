using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using cam_counting;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
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
	    private readonly MCvScalar _scalarRed = new MCvScalar(0.0, 0.0, 255.0);
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
            
			

			PolygonOverlay a = new PolygonOverlay(this.imageBox1, Color.Blue);
			a.SetPolygon(imageBox1.Size, new List<PointF>()
			{
				new PointF(50, 400),
				new PointF(800, 400),
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
	        _countingService = new CountingService(a.GetPolygon(), l.GetPolygon());
	        //_countingService.Setup(null, null);
	        _countingService.Increment += Increment;
	        _countingService.Decrement += Decrement;
	        videoCapture.ImageGrabbed += ImageGrabbed;
	        videoCapture.Start();

		}

	    private void ImageGrabbed(object sender, EventArgs e)
	    {
			Thread.Sleep(100);
			((VideoCapture)sender).Retrieve(_frame);
		    //CvInvoke.Resize(_frame, _frame, new Size(400, 300));
			var rec = _countingService.PushFrame(_frame);
		    Draw(rec, _frame);
		    imageBox1.Image = _frame;

			_frameCount++;
		    if (_frameCount != (int) videoCapture.GetCaptureProperty(CapProp.FrameCount)) return;
		    _frameCount = 0;
		    videoCapture.SetCaptureProperty(CapProp.PosFrames, 0);
	    }

	    public void Draw(List<Rectangle> rectangles, Mat mat)
	    {
		    for (var i = 0; i <= rectangles.Count - 1; i++)
		    {
			    CvInvoke.Rectangle(mat, rectangles[i], _scalarRed, 2, LineType.AntiAlias);
			}
	    }
	}
}
