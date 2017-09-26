using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emgu_ObjectTracking.Biz
{
    //Dense Optical flow: These algorithms help estimate the motion vector of every pixel in a video frame.
    //motion vector for each pixel
    class OpticalFlowFarneback
    {
        UMat pre_Frame = null;
        UMat curr_Frame = null;
        Mat flow = null;

        public double pyrScale = 0.5;
        public int levels = 3;
        public int winSize = 15;
        public int iterations = 3;
        public int polyN = 6;
        public double polySigma = 1.3;

        public void Tracking(UMat img_curr)
        {
            var grayImage = new UMat();
            CvInvoke.CvtColor(img_curr, grayImage, ColorConversion.Bgr2Gray);
            if (curr_Frame == null)
            {
                curr_Frame = grayImage;
                flow = new Mat();
                return;
            }

            if (pre_Frame != null)
                pre_Frame.Dispose();
            pre_Frame = curr_Frame;
            curr_Frame = grayImage;
            //CvInvoke.CalcOpticalFlowFarneback(pre_Frame, curr_Frame, flow, 0.5, 3, 15, 3, 6, 1.3, Emgu.CV.CvEnum.OpticalflowFarnebackFlag.Default);
            CvInvoke.CalcOpticalFlowFarneback(pre_Frame, curr_Frame, flow, pyrScale, levels, winSize, iterations, polyN, polySigma, Emgu.CV.CvEnum.OpticalflowFarnebackFlag.Default);
           
            Draw(img_curr, flow);
        }

        public static void Draw(UMat frame, Mat flow)
        {
            for (int y = 0; y < flow.Rows; y += 15)
            {
                for (int x = 0; x < flow.Cols; x += 15)
                {
                    // get the flow from y, x position * 10 for better visibility
                    // const Point2f flowatxy = flow.at<Point2f>(y, x) * 10;
                    //Độ dịch chuyển của 1 point
                    var b = flow.GetData(y, x);
                    var dx = System.BitConverter.ToSingle(b, 0);
                    var dy = System.BitConverter.ToSingle(b, 4);

                    Point flowatxy = new Point((int)(dx * 1), (int)(dy * 1));// *10;
                    // draw line at flow direction
                    //line(original, Point(x, y), Point(cvRound(x + flowatxy.x), cvRound(y + flowatxy.y)), Scalar(255, 0, 0));
                    //CvInvoke.Line(img_curr, new Point(x, y), new Point(x + flowatxy.X, y + flowatxy.Y), new MCvScalar(255, 0, 0));

                    if (flowatxy.X != 0 && flowatxy.Y != 0)
                    {
                        CvInvoke.Line(frame, new Point(x, y), new Point(x + flowatxy.X, y + flowatxy.Y), new MCvScalar(255, 0, 0));
                        CvInvoke.Circle(frame, new Point(x + flowatxy.X, y + flowatxy.Y), 1, new MCvScalar(0, 0, 0), 1);

                    }
                    // draw initial point
                    //circle(original, new Point(x, y), 1, Scalar(0, 0, 0), -1);
                    //CvInvoke.Circle(img_curr, new Point(x, y), 1, new MCvScalar(0, 0, 0), -1);
                }
            }
        }
    }
}
