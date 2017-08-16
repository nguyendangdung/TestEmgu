using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace MultipleObjectTracking
{
	public partial class Form2 : Form
	{

		MCvScalar SCALAR_BLACK = new MCvScalar(0.0, 0.0, 0.0);
		MCvScalar SCALAR_WHITE = new MCvScalar(255.0, 255.0, 255.0);
		MCvScalar SCALAR_BLUE = new MCvScalar(255.0, 0.0, 0.0);
		MCvScalar SCALAR_GREEN = new MCvScalar(0.0, 200.0, 0.0);
		MCvScalar SCALAR_RED = new MCvScalar(0.0, 0.0, 255.0);

		VideoCapture capVideo;

		bool blnFormClosing = false;

		public Form2()
		{
			InitializeComponent();
			capVideo = new VideoCapture(
				@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\TestEmgu\ImageSubtraction\768x576.avi");
		}

		private void Form2_Load(object sender, EventArgs e)
		{

		}
	}
}