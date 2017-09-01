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
    /// Test vẽ đường chuyển động
    /// </summary>
    class OpticalFlowPyrLK2
    {
        UMat pre_Frame = null;
        UMat next_Frame = null;
        PointF[] prevFeatures = null;

        GFTTDetector detector = new GFTTDetector();

        public double pyrScale = 0.5;
        public int levels = 3;
        public int winSize = 15;
        public int iterations = 3;
        public int polyN = 6;
        public double polySigma = 1.3;

        int min_features = 50;
        int max_features = 1000;
        double quality_level = 0.01;
        int min_distance = 3;

        UMat img_input_prev = new UMat();
        PointF[] points0 = new PointF[] { };
        PointF[] points1 = new PointF[] { };
        PointF[] initial = new PointF[] { };
        bool firstTime = true;

        public void Tracking(UMat img_input)
        {

            if (img_input.IsEmpty)
                return;

            if (img_input_prev.IsEmpty)
            {
                img_input.CopyTo(img_input_prev);
                return;
            }

            //------------------------------------------------------------------
            // Get features to track
            // Determines strong corners on an image.
            // http://opencv.willowgarage.com/documentation/cpp/imgproc_feature_detection.html#cv-goodfeaturestotrack
            //------------------------------------------------------------------
            // if new feature points must be added
            if (points0.Length <= min_features)
            {
                GFTTDetector detector = new GFTTDetector(max_features, quality_level, min_distance);
                MKeyPoint[] keyPoints = detector.Detect(img_input);
                var features = keyPoints.Select(c => c.Point).ToArray();

                var points0Temp = points0.ToList();
                points0Temp.AddRange(features);
                points0 = points0Temp.ToArray();

                var initialTemp = initial.ToList();
                initialTemp.AddRange(features);
                initial = initialTemp.ToArray();

                //// detect feature points
                //cv::goodFeaturesToTrack(
                //  img_input,     // The input 8-bit or floating-point 32-bit, single-channel image
                //  features,      // the output detected features
                //  max_features,  // the maximum number of features 
                //  quality_level, // quality level
                //  min_distance   // The minimum possible Euclidean distance between the returned corners
                //  );

                //// add the detected features to the currently tracked features
                //points0.insert(points0.end(), features.begin(), features.end());
                //initial.insert(initial.end(), features.begin(), features.end());
            }

            byte[] status;
            float[] err;

            // Calculates the optical flow for a sparse feature set using the iterative Lucas-Kanade method with pyramids
            // http://opencv.willowgarage.com/documentation/cpp/video_motion_analysis_and_object_tracking.html#cv-calcopticalflowpyrlk
            // The function implements the sparse iterative version of the Lucas-Kanade optical flow in pyramids, see Bouguet00.
            CvInvoke.CalcOpticalFlowPyrLK(
              img_input_prev, // prevImg – The first 8-bit single-channel or 3-channel input image.
              img_input,      // nextImg – The second input image of the same size and the same type as prevImg.
              points0.ToArray(),      // prevPts – Vector of points for which the flow needs to be found.
              new Size(winSize, winSize),
              levels,
              new MCvTermCriteria(),

              out points1,      // nextPts – The output vector of points containing the calculated new positions of the input features in the second image.
              out status,         // status  – The output status vector. Each element of the vector is set to 1 if the flow for the corresponding features has been found, 0 otherwise.
              out err);           // err     – The output vector that will contain the difference between patches around the original and moved points

            // loop over the tracked points to reject the undesirables
            int k = 0;
            for (int i = 0; i < points1.Length; i++)
            {
                // do we keep this point?
                if (status[i] == 1 && (Math.Abs(points0[i].X - points1[i].X) + (Math.Abs(points0[i].Y - points1[i].Y)) > 2))
                {
                    // keep this point in vector
                    initial[k] = initial[i];
                    points1[k] = points1[i];
                    k++;
                }
            }

            // eliminate unsuccesful points
            //points1.resize(k);
            points1 = points1.Take(k).ToArray();
            //initial.resize(k);
            initial = initial.Take(k).ToArray();

            // for all tracked points
            for (int i = 0; i < points1.Length; i++)
            {
                Point p1 = initial[i].ToPoint();
                Point p2 = points1[i].ToPoint();

                // draw line and circle
                CvInvoke.Line(img_input, p1, p2, new MCvScalar(255, 255, 255));
                CvInvoke.Circle(img_input, p2, 3, new MCvScalar(0, 255, 0), -1);
            }
            //UMat img_good_features = new UMat();
            //CvInvoke.CvtColor(img_input, img_good_features, ColorConversion.Gray2Bgr);
            //for (int i = 0; i < points1.Length; i++)
            //{
            //    Point p1 = PointFToPoint(initial[i]);
            //    Point p2 = PointFToPoint(points1[i]);

            //    // draw line and circle
            //    CvInvoke.Line(img_good_features, p1, p2, new MCvScalar(255, 255, 255));
            //    CvInvoke.Circle(img_good_features, p2, 3, new MCvScalar(0, 255, 0), -1);
            //}

            //if(showOutput)
            //  cv::imshow("KLTTracking", img_good_features);

            // current points become previous ones
            //  std::swap(points1, points0);
            var temp = points1;
            points1 = points0;
            points0 = temp;

            //img_good_features.CopyTo(img_output);

            img_input.CopyTo(img_input_prev);

            firstTime = false;
        }

        public static void Draw(UMat frame, PointF[] prevFeatures, PointF[] nextFeatures, byte[] status, float[] trackError)
        {
            int count0 = status.Count(c => c == 0);
            if (count0 > 0)
                return;
            foreach (var item in prevFeatures)
            {
                CvInvoke.Circle(frame, item.ToPoint(), 1, new MCvScalar(0, 0, 0), 1);
            }
        }
    }
}
