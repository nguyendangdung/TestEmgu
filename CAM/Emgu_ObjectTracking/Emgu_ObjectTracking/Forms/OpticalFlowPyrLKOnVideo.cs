using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu_ObjectTracking.Util;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Emgu_ObjectTracking.Forms
{
    public partial class OpticalFlowPyrLKOnVideo : Form
    {
        string file = @"C:\Users\Nguyen Truong Giang\Documents\Visual Studio 2013\Projects\cam_counting\Test\cars.mp4";
        TestGFTTDetector _detector = new TestGFTTDetector();
        VideoCapture _VideoCapture = null;

        public OpticalFlowPyrLKOnVideo()
        {
            InitializeComponent();
        }

        private void OpticalFlowPyrLKOnVideo_Load(object sender, EventArgs e)
        {
            this.Location = Screen.AllScreens[1].WorkingArea.Location;
            _VideoCapture = new VideoCapture();
            _VideoCapture.FlipHorizontal = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _frame = new UMat();
            _VideoCapture.ImageGrabbed += ProcessFrame;
            _VideoCapture.Start();
        }

        private void commonEvent(object sender, EventArgs e)
        {
            SetDetectorValue();
        }

        private void btnNextFrame_Click(object sender, EventArgs e)
        {
            Mat frame = _VideoCapture.QueryFrame();
            if (_frame == null)
                _frame = new UMat();
            frame.CopyTo(_frame);
            Process(_frame);
        }

        private UMat _frame = null;

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_VideoCapture != null && _VideoCapture.Ptr != IntPtr.Zero)
            {
                _VideoCapture.Retrieve(_frame, 0);
                Process(_frame);
            }
        }

        private void SetDetectorValue()
        {
            this.Invoke((MethodInvoker)delegate
            {
                _detector.qualityLevel = (double)qualityLevel.Value / 100;
                _detector.blockSize = blockSize.Value;
                _detector.minDistance = minDistance.Value;

                _detector.RunCornerSubPix = ckCornerSubPix.Checked;
                _detector.subPixWinSize = new Size(subPixWinSize.Value, subPixWinSize.Value);
                _detector.maxIteration = maxIteration.Value;
                _detector.eps = (double)eps.Value / 100;
            });
        }

        private void Process(UMat frame)
        {
            try
            {
                if (frame == null)
                    return;

                SetDetectorValue();

                _detector.Tracking(frame);

                imageBox1.Image = frame;

                //Để vẽ cho xong
                Thread.Sleep(10);
            }
            catch { }
        }

        class TestGFTTDetector
        {
            //CalcOpticalFlowPyrLK
            public int levels = 3;
            public Size winSize = new Size(31, 31);

            //GFTTDetector
            public int maxCorners = 100;
            public double qualityLevel = 0.01;
            public int blockSize = 3;
            public double minDistance = 1;

            //CornerSubPix
            public Size subPixWinSize = new Size(10, 10);
            public int maxIteration = 20; //Số lần lặp lại lớn nhất
            public double eps = 0.03;
            public bool RunCornerSubPix = false;

            private VectorOfPointF points0 = new VectorOfPointF();
            private VectorOfPointF points1 = new VectorOfPointF();

            private Mat gray = new Mat();
            private Mat prevGray = new Mat();
            private Mat mask = new Mat(); //Vẽ hình lên đây rồi apply vào hình gốc

            public void Tracking(UMat img_curr)
            {
                CvInvoke.CvtColor(img_curr, gray, ColorConversion.Bgr2Gray);

                if (mask.IsEmpty)
                {
                    mask = new Mat(img_curr.Size, img_curr.Depth, img_curr.NumberOfChannels);
                    mask.SetTo(new MCvScalar(0));

                    points0 = GetFeatures(gray);
                    Utility.Swap(ref gray, ref prevGray);
                    return;
                }

                VectorOfByte status = new VectorOfByte();
                VectorOfFloat err = new VectorOfFloat();
                var termcrit = new MCvTermCriteria();
                CvInvoke.CalcOpticalFlowPyrLK(prevGray, gray, points0, points1, status, err, winSize, levels, termcrit);

                var tempPoint0 = points0.ToArray().ToList();
                var tempPoint1 = points1.ToArray();

                //size_t i, k;
                int i, k;
                for (i = k = 0; i < tempPoint1.Length; i++)
                {
                    if (status[i] == 0)
                        continue;

                    tempPoint1[k++] = tempPoint1[i];
                }

                Array.Resize(ref tempPoint1, k);
                points1 = new VectorOfPointF(tempPoint1);


                Draw(img_curr, points1.ToArray());

                //Thêm lên ảnh gốc để hiển thị
                //CvInvoke.Add(img_curr, mask, img_curr);

                //Swap
                Utility.Swap(ref points0, ref points1);
                Utility.Swap(ref gray, ref prevGray);
            }

            private VectorOfPointF GetFeatures(IInputOutputArray grayFrame)
            {
                GFTTDetector detector = new GFTTDetector(maxCorners: maxCorners, qualityLevel: qualityLevel, minDistance: minDistance, blockSize: blockSize);
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
