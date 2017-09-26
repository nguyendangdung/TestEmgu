using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Tracking;
using Emgu.CV.Util;
using Emgu_ObjectTracking.Biz;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emgu_ObjectTracking.Tracking
{
    //https://www.learnopencv.com/object-tracking-using-opencv-cpp-python/

    class TestTracker
    {
        bool isInit = true;
        Tracker tracker;
        Rectangle bbox;

        public void Tracking(UMat img_curr)
        {
            Mat frame = new Mat();
            img_curr.CopyTo(frame);
            if (isInit)
            {
                // Set up tracker. 
                // Instead of MIL, you can also use 
                // BOOSTING, KCF, TLD, MEDIANFLOW or GOTURN  
                //Ptr<TestTracker> tracker = Tracker::create( "MIL" );
                tracker = new Tracker("KCF");

                // Define an initial bounding box
                bbox = new Rectangle(287, 23, 86, 320);
                ObjectDetector od = new ObjectDetector(@"Cars.xml");
                var objRecs = od.Detect(img_curr);
                bbox = objRecs.Last();

                // Uncomment the line below if you 
                // want to choose the bounding box
                // bbox = selectROI(frame, false);

                // Initialize tracker with first frame and bounding box
                //tracker->init(frame, bbox);
                tracker.Init(frame, bbox);
                isInit = false;
                return;
            }
            // Update tracking results
            //tracker->update(frame, bbox);
            tracker.Update(frame, out bbox);

            // Draw bounding box
            //rectangle(frame, bbox, Scalar( 255, 0, 0 ), 2, 1 );
            CvInvoke.Rectangle(img_curr, bbox, new MCvScalar(255, 0, 0));

            // Display result
            //imshow("Tracking", frame);
            //int k = waitKey(1);
            //if(k == 27) break;


        }
    }
}
