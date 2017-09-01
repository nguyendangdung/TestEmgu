using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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
    class OpticalFlowPyrLK3
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

        private bool addRemovePt = false;
        private const int MAX_COUNT = 500;
        private bool needToInit = true;
        private bool nightMode = false;
        private Size subPixWinSize = new Size(10, 10);
        private Size winSizeSize = new Size(31, 31);


        private Mat gray = new Mat();
        private Mat prevGray = new Mat();
        private Mat image = new Mat();

        private VectorOfPointF points0 = new VectorOfPointF();
        private VectorOfPointF points1 = new VectorOfPointF();

        int count = 0;
        public void Tracking(UMat img_curr)
        {
            //TermCriteria termcrit(TermCriteria::COUNT|TermCriteria::EPS,20,0.03);

            //sau 20 frame lấy lại Features 1 lần
            count++;
            if (count % 20 == 0)
                needToInit = true;

            //for(;;)
            //{
            //    cap >> frame;
            //    if( frame.empty() )
            //        break;

            img_curr.CopyTo(image);
            CvInvoke.CvtColor(image, gray, ColorConversion.Bgr2Gray);

            if (nightMode)
                //image = Scalar::all(0);
                image.SetTo(new MCvScalar(0));

            if (needToInit)
            {
                ObjectDetector od = new ObjectDetector();
                var objRecs = od.Detect(img_curr, @"Cars.xml");

                // automatic initialization
                //goodFeaturesToTrack(gray, points1, MAX_COUNT, 0.01, 10, Mat(), 3, 0, 0.04);
                MKeyPoint[] keyPoints = detector.Detect(gray);
                var temp = keyPoints.Select(c => c.Point);
                var temp2 = new List<PointF>();
                foreach (var objRec in objRecs)
                {
                    temp2.AddRange(temp.Where(c => c.InsideRectangle(objRec)).ToList());
                }
                points1.Push(temp2.ToArray());

                //cornerSubPix(gray, points1, subPixWinSize, Size(-1, -1), termcrit);
                CvInvoke.CornerSubPix(gray, points1, subPixWinSize, new Size(-1, -1), new MCvTermCriteria(20, 0.03));
                addRemovePt = false;
            }
            //else if (!points0.empty())
            else if (points0.Size > 0)
            {
                VectorOfByte status = new VectorOfByte();
                VectorOfFloat err = new VectorOfFloat();
                if (prevGray.IsEmpty)
                    gray.CopyTo(prevGray);
                //calcOpticalFlowPyrLK(prevGray, gray, points0, points1, status, err, winSize, 3, termcrit, 0, 0.001);
                var termcrit = new MCvTermCriteria();
                CvInvoke.CalcOpticalFlowPyrLK(prevGray, gray, points0, points1, status, err, winSizeSize, levels, termcrit);


                //size_t i, k;
                int i, k;

                List<PointF> tempPoint1 = points1.ToArray().ToList();
                for (i = k = 0; i < points1.Size; i++)
                {
                    //if (addRemovePt)
                    //{
                    //    if (norm(point - points1[i]) <= 5)
                    //    {
                    //        addRemovePt = false;
                    //        continue;
                    //    }
                    //}

                    //if (!status[i])
                    if (status[i] == 0)
                        continue;

                    //points1[k++] = points1[i];
                    tempPoint1[k++] = tempPoint1[i];
                    CvInvoke.Circle(img_curr, points1[i].ToPoint(), 3, new MCvScalar(0, 255, 0));
                }
                //points1.resize(k);
                tempPoint1.RemoveRange(k, tempPoint1.Count - k);
                points1 = new VectorOfPointF(tempPoint1.ToArray());
            }

            //if( addRemovePt && points1.size() < (size_t)MAX_COUNT )
            //{
            //    vector<Point2f> tmp;
            //    tmp.push_back(point);
            //    cornerSubPix( gray, tmp, winSize, Size(-1,-1), termcrit);
            //    points1.push_back(tmp[0]);
            //    addRemovePt = false;
            //}

            needToInit = false;
            //imshow("LK Demo", image);

            //char c = (char)waitKey(10);
            //if( c == 27 )
            //    break;
            //switch( c )
            //{
            //case 'r':
            //    needToInit = true;
            //    break;
            //case 'c':
            //    points0.clear();
            //    points1.clear();
            //    break;
            //case 'n':
            //    nightMode = !nightMode;
            //    break;
            //}
            //"\tESC - quit the program\n"
            //"\tr - auto-initialize tracking\n"
            //"\tc - delete all the points\n"
            //"\tn - switch the \"night\" mode on/off\n"
            //"To add/remove a feature point click it\n" << endl;

            //std::swap(points1, points0);
            var tempPoints = points0;
            points0 = points1;
            points1 = tempPoints;

            //cv::swap(prevGray, gray);
            var tempGray = gray;
            gray = prevGray;
            prevGray = tempGray;
            
            //}

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
