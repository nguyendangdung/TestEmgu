using System;
using Emgu.CV;

namespace cam_counting
{
	public interface ICountingService
	{
		EventHandler Increment { get; set; }
		EventHandler Decrement { get; set; }
		void PushFrame(Mat mat);
	}
}