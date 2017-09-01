using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emgu_ObjectTracking.Biz
{
    //The main difference between the two methods is that calcOpticalFlowFarneback computes a dense optical flow (motion vector for each pixel),
    //whereas calcOpticalFlowPyrLK computes a sparse optical flow (motion vector for a set of points in the image).
    //Of course, if you don't need to compute motion for every pixel in the image, LK will be faster.
    //Since LK works only on a set of points, you need to specify which points to compute motion vectors for, which is the prevPts argument.
    //This could be a vector containing the location of every pixel in the image (in which case it would be similar to calcOpticalFlowFarneback, but probably slower).
    //However, it's better to specify certain points in the image / video which are of interest and easy to track.
    //These are usually corner like points (which can be found initially using something like goodFeaturesToTrack).

    //https://www.reddit.com/r/computervision/comments/wgez0/opencv_optical_flow_question/
    //https://stackoverflow.com/questions/23422346/can-we-use-lucas-kanade-optical-flowopencv-for-color-based-detection-or-contou
    //motion vector for a set of points in the image

    /// <summary>
    /// Test cơ bản xem method chạy thế nào
    /// </summary>
    class OpticalFlowPyrLK
    {
        UMat pre_Frame = null;
        UMat curr_Frame = null;
        PointF[] prevFeatures = null;

        GFTTDetector detector = new GFTTDetector();

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
                
                return;
            }

            if (pre_Frame != null)
                pre_Frame.Dispose();
            pre_Frame = curr_Frame;
            curr_Frame = grayImage;

            MKeyPoint[] keyPoints = detector.Detect(pre_Frame);
            prevFeatures = keyPoints.Select(c => c.Point).ToArray();

            byte[] status;
            float[] trackError;
            PointF[] currFeatures = null;
            var criteria = new MCvTermCriteria();
            CvInvoke.CalcOpticalFlowPyrLK(pre_Frame, curr_Frame, prevFeatures, new Size(winSize, winSize), levels, criteria, out currFeatures, out status, out trackError);

            prevFeatures = currFeatures;

            Draw(img_curr, currFeatures);
        }

        public static void Draw(UMat frame, PointF[] features)
        {
            foreach (var item in features)
            {
                CvInvoke.Circle(frame, new Point((int)item.X, (int)item.Y), 1, new MCvScalar(0, 0, 0), 1);
            }
        }
    }
}
