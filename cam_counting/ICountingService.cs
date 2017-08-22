using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;

namespace cam_counting
{
	public interface ICountingService
	{
		EventHandler Increment { get; set; }
		EventHandler Decrement { get; set; }
		List<Rectangle> PushFrame(Mat mat);

		void Setup(List<PointF> polygon, List<PointF> line);
	}
}