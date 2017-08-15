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
		readonly MCvScalar _scalarWhite = new MCvScalar(255.0, 255.0, 255.0);

		readonly MCvScalar _scalarBlue = new MCvScalar(255.0, 0.0, 0.0);


		Point _currentMousePosition;


		readonly List<Point> _mousePositions = new List<Point>();


		Mat _imgBlank = default(Mat);

		public Form1()
		{
			InitializeComponent();
		}

		private void imageBox1_MouseMove(object sender, MouseEventArgs e)
		{

			_currentMousePosition = imageBox1.PointToClient(Cursor.Position);

			_mousePositions.Add(_currentMousePosition);

			Point predictedMousePosition = PredictNextPosition(_mousePositions);

			label1.Text = ("current position        = " + _mousePositions.Last().X + ", " + _mousePositions.Last().Y);
			label2.Text = ("next predicted position = " + predictedMousePosition.X + ", " + predictedMousePosition.Y);

			_imgBlank = new Mat(imageBox1.Size, DepthType.Cv8U, 3);

			DrawCross(_imgBlank, _mousePositions.Last(), _scalarWhite);
			DrawCross(_imgBlank, predictedMousePosition, _scalarBlue);

			imageBox1.Image = _imgBlank;

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
			var numPositions = _mousePositions.Count;
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


				var deltaX = Convert.ToInt32(Math.Round((sumOfXChanges / 3.0)));


				var sumOfYChanges = ((positions[2].Y - positions[1].Y) * 2) +
									((positions[1].Y - positions[0].Y) * 1);


				var deltaY = Convert.ToInt32(Math.Round((sumOfYChanges / 3.0)));


				predictedPosition.X = positions.Last().X + deltaX;

				predictedPosition.Y = positions.Last().Y + deltaY;
			}
			if (numPositions == 4)
			{
				var sumOfXChanges = ((positions[3].X - positions[2].X) * 3) +
									((positions[2].X - positions[1].X) * 2) +
									((positions[1].X - positions[0].X) * 1);


				var deltaX = Convert.ToInt32(Math.Round((sumOfXChanges / 6.0)));


				var sumOfYChanges = ((positions[3].Y - positions[2].Y) * 3) +
									((positions[2].Y - positions[1].Y) * 2) +
									((positions[1].Y - positions[0].Y) * 1);


				var deltaY = Convert.ToInt32(Math.Round((sumOfYChanges / 6.0)));


				predictedPosition.X = positions.Last().X + deltaX;

				predictedPosition.Y = positions.Last().Y + deltaY;
			}
			if (numPositions >= 5)
			{
				var sumOfXChanges = ((positions[numPositions - 1].X - positions[numPositions - 2].X) * 4) +
									((positions[numPositions - 2].X - positions[numPositions - 3].X) * 3) +
									((positions[numPositions - 3].X - positions[numPositions - 4].X) * 2) +
									((positions[numPositions - 4].X - positions[numPositions - 5].X) * 1);


				var deltaX = Convert.ToInt32(Math.Round((sumOfXChanges / 10.0)));


				var sumOfYChanges = ((positions[numPositions - 1].Y - positions[numPositions - 2].Y) * 4) +
									((positions[numPositions - 2].Y - positions[numPositions - 3].Y) * 3) +
									((positions[numPositions - 3].Y - positions[numPositions - 4].Y) * 2) +
									((positions[numPositions - 4].Y - positions[numPositions - 5].Y) * 1);


				var deltaY = Convert.ToInt32(Math.Round((sumOfYChanges / 10.0)));


				predictedPosition.X = positions.Last().X + deltaX;

				predictedPosition.Y = positions.Last().Y + deltaY;
			}

			return predictedPosition;
		}
	}
}
