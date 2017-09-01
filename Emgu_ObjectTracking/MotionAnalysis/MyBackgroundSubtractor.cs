using Emgu.CV;
using Emgu.CV.BgSegm;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu_ObjectTracking.Biz
{
    //http://opencv-python-tutroals.readthedocs.io/en/latest/py_tutorials/py_video/py_bg_subtraction/py_bg_subtraction.html
    class MyBackgroundSubtractor
    {
        public double threadhold = 0.7;
        public int iterations = 2;

        bool isInit = true;
        Mat kernel;
        BackgroundSubtractorGMG fgbg;
        Mat fgmask = new Mat();

        public void Tracking(UMat img_curr)
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

            //fgmask = fgbg.apply(frame)
            fgbg.Apply(img_curr, fgmask);

            //fgmask = cv2.morphologyEx(fgmask, cv2.MORPH_OPEN, kernel)
            CvInvoke.MorphologyEx(fgmask, img_curr, MorphOp.Open, kernel, new Point(-1, -1), iterations, BorderType.Default, new MCvScalar(100));
        }
    }
}
