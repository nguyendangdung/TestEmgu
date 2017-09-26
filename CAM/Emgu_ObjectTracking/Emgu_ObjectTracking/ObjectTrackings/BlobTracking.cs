using Emgu.CV;
using Emgu.CV.BgSegm;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Drawing;

namespace Emgu_ObjectTracking.ObjectTrackings
{
    /// <summary>
    /// Tracking và đếm object dựa vào BackgroundSubtractor + Blob tracking
    /// </summary>
    class BlobTracking
    {
        public int iterations = 3;

        bool isInit = true;
        Mat kernel;
        BackgroundSubtractorMOG fgbg; //BackgroundSubtractorMOG; BackgroundSubtractorMOG2; BackgroundSubtractorKNN; BackgroundSubtractorGMG
        Mat fgmask = new Mat();
        Mat temps = new Mat();

        private static CvBlobDetector _blobDetector = new CvBlobDetector();
        private static CvTracks _tracker = new CvTracks();

        public void Tracking(Mat img_curr)
        {
            if (isInit)
            {
                //kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE,(3,3))
                kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));

                //fgbg = cv2.createBackgroundSubtractorGMG()
                fgbg = new BackgroundSubtractorMOG();

                isInit = false;
                return;
            }

            CvInvoke.CvtColor(img_curr, temps, ColorConversion.Bgr2Gray);

            //fgmask = fgbg.apply(frame)
            fgbg.Apply(temps, fgmask);

            //fgmask = cv2.morphologyEx(fgmask, cv2.MORPH_OPEN, kernel)
            CvInvoke.MorphologyEx(fgmask, temps, MorphOp.Close, kernel, new Point(-1, -1), iterations, BorderType.Default, new MCvScalar(31));

            //Sử dụng CvTracks
            CvTracks(temps, temps);

            temps.CopyTo(img_curr);
        }

        private void CvTracks(Mat forgroundMask, Mat frame)
        {
            CvBlobs blobs = new CvBlobs();
            _blobDetector.Detect(forgroundMask.ToImage<Gray, byte>(), blobs);
            blobs.FilterByArea(100, int.MaxValue);

            float scale = (frame.Width + frame.Width) / 2.0f;
            _tracker.Update(blobs, 0.01 * scale, 5, 5);

            foreach (var pair in _tracker)
            {
                CvTrack b = pair.Value;
                CvInvoke.Rectangle(frame, b.BoundingBox, new MCvScalar(255.0, 255.0, 255.0), 2);
                CvInvoke.PutText(frame, b.Id.ToString(), new Point((int)Math.Round(b.Centroid.X), (int)Math.Round(b.Centroid.Y)), FontFace.HersheyPlain, 1.0, new MCvScalar(255.0, 255.0, 255.0));
            }
        }
    }
}
