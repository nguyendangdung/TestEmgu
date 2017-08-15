using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace mouse
{
	public partial class Form1 : Form
	{

		MCvScalar SCALAR_BLACK = new MCvScalar(0.0, 0.0, 0.0);

		MCvScalar SCALAR_WHITE = new MCvScalar(255.0, 255.0, 255.0);

		MCvScalar SCALAR_BLUE = new MCvScalar(255.0, 0.0, 0.0);

		MCvScalar SCALAR_GREEN = new MCvScalar(0.0, 255.0, 0.0);

		MCvScalar SCALAR_RED = new MCvScalar(0.0, 0.0, 255.0);


		Point currentMousePosition = new Point();


		List<Point> mousePositions = new List<Point>();


		List<Point> predictedMousePosition = new List<Point>();


		Mat imgBlank = default(Mat);

		//=======================================================
		//Service provided by Telerik (www.telerik.com)
		//Conversion powered by NRefactory.
		//Twitter: @telerik
		//Facebook: facebook.com/telerik
		//=======================================================

		public Form1()
		{
			InitializeComponent();
		}

		private void imageBox1_MouseMove(object sender, MouseEventArgs e)
		{

			currentMousePosition = imageBox1.PointToClient(Cursor.Position);

			mousePositions.Add(currentMousePosition);

			Point predictedMousePosition = PredictNextPosition(mousePositions);

			label1.Text = ("current position        = " + mousePositions.Last().X.ToString() + ", " + mousePositions.Last().Y.ToString());
			label2.Text = ("next predicted position = " + predictedMousePosition.X.ToString() + ", " + predictedMousePosition.Y.ToString());

			imgBlank = new Mat(imageBox1.Size, DepthType.Cv8U, 3);

			DrawCross(imgBlank, mousePositions.Last(), SCALAR_WHITE);
			DrawCross(imgBlank, predictedMousePosition, SCALAR_BLUE);

			imageBox1.Image = imgBlank;

			Application.DoEvents();
		}

		public void DrawCross(Mat imgBlank, Point center, MCvScalar color)
		{
			CvInvoke.Line(imgBlank, new Point(center.X - 5, center.Y - 5), new Point(center.X + 5, center.Y + 5), color, 2);
			CvInvoke.Line(imgBlank, new Point(center.X + 5, center.Y - 5), new Point(center.X - 5, center.Y + 5), color, 2);
		}

		private Point PredictNextPosition(List<Point> positions)
		{
			var predictedPosition = new Point();
			var numPositions = mousePositions.Count;
			if (numPositions == 1)
			{
				return positions[0];
			}
			if (numPositions == 2)
			{
				var deltaX = positions[1].X - positions[0].X;
				var deltaY = positions[1].Y - positions[0].Y;

				predictedPosition.X = positions.Last().X + deltaX;
				predictedPosition.Y = positions.Last().Y + deltaY;
			}
			if (numPositions == 3)
			{
				var sumOfXChanges = ((positions[2].X - positions[1].X) * 2) +
									((positions[1].X - positions[0].X) * 1);


				var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 3.0)));


				var sumOfYChanges = ((positions[2].Y - positions[1].Y) * 2) +
									((positions[1].Y - positions[0].Y) * 1);


				var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 3.0)));


				predictedPosition.X = positions.Last().X + deltaX;

				predictedPosition.Y = positions.Last().Y + deltaY;
			}
			if (numPositions == 4)
			{
				var sumOfXChanges = ((positions[3].X - positions[2].X) * 3) +
									((positions[2].X - positions[1].X) * 2) +
									((positions[1].X - positions[0].X) * 1);


				var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 6.0)));


				var sumOfYChanges = ((positions[3].Y - positions[2].Y) * 3) +
									((positions[2].Y - positions[1].Y) * 2) +
									((positions[1].Y - positions[0].Y) * 1);


				var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 6.0)));


				predictedPosition.X = positions.Last().X + deltaX;

				predictedPosition.Y = positions.Last().Y + deltaY;
			}
			if (numPositions >= 5)
			{
				var sumOfXChanges = ((positions[numPositions - 1].X - positions[numPositions - 2].X) * 4) +
									((positions[numPositions - 2].X - positions[numPositions - 3].X) * 3) +
									((positions[numPositions - 3].X - positions[numPositions - 4].X) * 2) +
									((positions[numPositions - 4].X - positions[numPositions - 5].X) * 1);


				var deltaX = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfXChanges / 10.0)));


				var sumOfYChanges = ((positions[numPositions - 1].Y - positions[numPositions - 2].Y) * 4) +
									((positions[numPositions - 2].Y - positions[numPositions - 3].Y) * 3) +
									((positions[numPositions - 3].Y - positions[numPositions - 4].Y) * 2) +
									((positions[numPositions - 4].Y - positions[numPositions - 5].Y) * 1);


				var deltaY = Convert.ToInt32(Math.Round(Convert.ToDouble(sumOfYChanges / 10.0)));


				predictedPosition.X = positions.Last().X + deltaX;

				predictedPosition.Y = positions.Last().Y + deltaY;
			}

			return predictedPosition;
		}
	}
}
