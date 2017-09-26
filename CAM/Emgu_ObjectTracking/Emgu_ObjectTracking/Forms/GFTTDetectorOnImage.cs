using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Emgu_ObjectTracking.Forms
{
    public partial class GFTTDetectorOnImage : Form
    {
        public GFTTDetectorOnImage()
        {
            InitializeComponent();
        }

        private void GFTTDetectorOnImage_Load(object sender, System.EventArgs e)
        {
            this.Location = Screen.AllScreens[1].WorkingArea.Location;
            Process();
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            Process();
        }

        private void commonEvent(object sender, System.EventArgs e)
        {
            Process();
        }

        private void Process()
        {
            try
            {
                string file = @"C:\testdll\Test\Emgu_ObjectTracking\Emgu_ObjectTracking\Images\opticalfb.jpg";
                UMat image = new UMat(file, ImreadModes.Color);

                TestGFTTDetector detector = new TestGFTTDetector();
                detector.qualityLevel = (double)qualityLevel.Value / 100;
                detector.blockSize = blockSize.Value;
                detector.minDistance = minDistance.Value;

                detector.RunCornerSubPix = ckCornerSubPix.Checked;
                detector.subPixWinSize = new Size(subPixWinSize.Value, subPixWinSize.Value);
                detector.maxIteration = maxIteration.Value;
                detector.eps = (double)eps.Value / 100;

                detector.Tracking(image);

                imageBox1.Image = image;

                //Để vẽ cho xong
                Thread.Sleep(20);
            }
            catch { }
        }

        class TestGFTTDetector
        {
            //GFTTDetector
            public double qualityLevel = 0.01;
            public int blockSize = 3;
            public double minDistance = 1;

            //CornerSubPix
            public Size subPixWinSize = new Size(10, 10);
            public int maxIteration = 20; //Số lần lặp lại lớn nhất
            public double eps = 0.03;
            public bool RunCornerSubPix = false;

            public void Tracking(UMat img_curr)
            {
                Mat grayFrame = new Mat();
                CvInvoke.CvtColor(img_curr, grayFrame, ColorConversion.Bgr2Gray);

                VectorOfPointF vectorAllPoints = GetFeatures(grayFrame);

                Draw(img_curr, vectorAllPoints.ToArray());
            }

            private VectorOfPointF GetFeatures(IInputOutputArray grayFrame)
            {
                GFTTDetector detector = new GFTTDetector(qualityLevel: qualityLevel, minDistance: minDistance, blockSize: blockSize);
                MKeyPoint[] keyPoints = detector.Detect(grayFrame);
                var allPoints = keyPoints.Select(c => c.Point);
                var vectorAllPoints = new VectorOfPointF(allPoints.ToArray());

                if (RunCornerSubPix)
                {
                    CvInvoke.CornerSubPix(grayFrame, vectorAllPoints, subPixWinSize, new Size(10, 10), new MCvTermCriteria(maxIteration, eps));
                }

                return vectorAllPoints;
            }

            private void Draw(IInputOutputArray frame, PointF[] points)
            {
                foreach (var item in points)
                {
                    CvInvoke.Circle(frame, item.ToPoint(), 3, new MCvScalar(0, 255, 0));
                }
            }
        }
    }
}
