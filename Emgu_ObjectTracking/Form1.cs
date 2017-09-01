using Emgu.CV;
using Emgu_ObjectTracking.Biz;
using Emgu_ObjectTracking.Tracking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Emgu_ObjectTracking
{
    public partial class Form1 : Form
    {
        private VideoCapture _capture = null;
        private UMat _frame;
        private UMat _frameSmall = new UMat();
        private OpticalFlowFarneback _OpticalFlowFarneback = new OpticalFlowFarneback();
        private OpticalFlowPyrLK _OpticalFlowPyrLK = new OpticalFlowPyrLK();
        private OpticalFlowPyrLK2 _OpticalFlowPyrLK2 = new OpticalFlowPyrLK2();
        private OpticalFlowPyrLK3 _OpticalFlowPyrLK3 = new OpticalFlowPyrLK3();
        private ObjectDetector _ObjectDetector = new ObjectDetector();
        private TestTracker _TestTracker = new TestTracker();
        private TestMultiTracker _TestMultiTracker = new TestMultiTracker();
        private KalmanFilterTracking _KalmanFilterTracking = new KalmanFilterTracking();
        private MyBackgroundSubtractor _MyBackgroundSubtractor = new MyBackgroundSubtractor();

        public Form1()
        {
            InitializeComponent();

            captureImageBox.SizeMode = grayscaleImageBox.SizeMode = PictureBoxSizeMode.StretchImage;


            try
            {
                //_capture = new VideoCapture();
                //_capture = new VideoCapture(@"C:\Users\Nguyen Truong Giang\Downloads\Sample_App\Sample_App\Example.avi");
                _capture = new VideoCapture(@"cars.mp4");
                //_capture = new VideoCapture(@"C:\2\Buzzing Malaysian Road Traffic (Kuala Lumpur) (1).mp4");
                _capture.ImageGrabbed += ProcessFrame;
                //_capture.FlipHorizontal = true;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            _frame = new UMat();

            _st.Start();
        }



        Stopwatch _st = new Stopwatch();
        int framecount = 0;
        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                //var _frame = new UMat();
                //{ 
                _capture.Retrieve(_frame, 0);
                CvInvoke.Resize(_frame, _frameSmall, new Size(_frame.Size.Width / 2, _frame.Size.Height / 2));
                framecount++;

                try
                {
                    SetOpticalFlowValue();

                    _ObjectDetector.Detect(_frameSmall, @"Cars.xml");
                    //_OpticalFlowPyrLK3.Tracking(_frameSmall);
                    //_OpticalFlowPyrLK2.Tracking(_frameSmall);
                    //_OpticalFlowPyrLK.Tracking(_frameSmall);
                    //_OpticalFlowFarneback.Tracking(_frameSmall);
                    //_TestTracker.Tracking(_frameSmall);
                    //_TestMultiTracker.Tracking(_frameSmall);
                    //_KalmanFilterTracking.Tracking(_frameSmall);
                    //_MyBackgroundSubtractor.Tracking(_frameSmall);

                    grayscaleImageBox.Image = _frameSmall;
                }
                catch { }


                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        Text = string.Format("{0}-{1}-{2}", _st.ElapsedMilliseconds, framecount, framecount / (_st.ElapsedMilliseconds / 1000));
                    }
                    catch { }
                });

                captureImageBox.Image = _frame;

                //grayscaleImageBox.Image = _grayFrame;
                //smoothedGrayscaleImageBox.Image = _smoothedGrayFrame;
                //cannyImageBox.Image = _cannyFrame;
                //Thread.Sleep(20);
                //_frame.Dispose();
                //}
            }
        }

        private void SetOpticalFlowValue()
        {
            this.Invoke((MethodInvoker)delegate
                {
                    _OpticalFlowFarneback.pyrScale = (double)pyrScale.Value / 10;
                    _OpticalFlowFarneback.levels = levels.Value;
                    _OpticalFlowFarneback.winSize = winSize.Value;
                    _OpticalFlowFarneback.iterations = iterations.Value;
                    _OpticalFlowFarneback.polyN = polyN.Value;

                    _MyBackgroundSubtractor.threadhold = (double)threadhold.Value / 10;
                    _MyBackgroundSubtractor.iterations = iterations.Value;
                });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _capture.Start();
        }
    }
}
