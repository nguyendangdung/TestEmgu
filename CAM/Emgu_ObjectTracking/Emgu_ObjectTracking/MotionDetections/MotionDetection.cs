using Emgu.CV;
using Emgu.CV.BgSegm;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;


namespace Emgu_ObjectTracking.MotionDetections
{
    //http://opencv-python-tutroals.readthedocs.io/en/latest/py_tutorials/py_video/py_bg_subtraction/py_bg_subtraction.html
    class MotionDetection
    {
        public double threadhold = 0.5;
        public int iterations = 2;

        bool isInit = true;
        Mat kernel;
        BackgroundSubtractorGMG fgbg;
        Mat fgmask = new Mat();
        Mat temps = new Mat();

        public void Tracking(Mat img_curr)
        {
            if (isInit)
            {
                //kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE,(3,3))
                kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));

                //fgbg = cv2.createBackgroundSubtractorGMG()
                fgbg = new BackgroundSubtractorGMG(2, threadhold);

                isInit = false;
                return;
            }
            
            CvInvoke.CvtColor(img_curr, temps, ColorConversion.Bgr2Gray);

            //fgmask = fgbg.apply(frame)
            fgbg.Apply(temps, fgmask);

            //fgmask = cv2.morphologyEx(fgmask, cv2.MORPH_OPEN, kernel)
            CvInvoke.MorphologyEx(fgmask, temps, MorphOp.Open, kernel, new Point(-1, -1), iterations, BorderType.Default, new MCvScalar(31));

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(temps, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);
            int count = contours.Size;
            for (int i = 0; i < count; i++)
            {
                using (VectorOfPoint contour = contours[i])
                {
                    Rectangle rect = CvInvoke.BoundingRectangle(contour);
                    CvInvoke.Rectangle(img_curr, rect, new MCvScalar(), 1);
                }
            }
        }
    }
}
