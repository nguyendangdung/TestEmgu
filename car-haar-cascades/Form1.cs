using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Structure;

namespace car_haar_cascades
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			using (var videoCapture = new VideoCapture("cars.mp4"))
			using (CascadeClassifier cars = new CascadeClassifier("cars.xml"))
			{
				while (true)
				{
					var frame = videoCapture.QueryFrame();
					if (frame == null)
					{
						break;
					}
					using (UMat ugray = new UMat())
					{
						CvInvoke.CvtColor(frame, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

						CvInvoke.EqualizeHist(ugray, ugray);
						var faces = cars.DetectMultiScale(ugray);
						foreach (var face in faces)
							CvInvoke.Rectangle(frame, face, new Bgr(Color.Red).MCvScalar, 2);


						imageBox1.Image = frame;
						Application.DoEvents();
					}
				}
			}
		}
	}
}