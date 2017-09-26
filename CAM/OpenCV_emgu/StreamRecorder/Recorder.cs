using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamRecorder
{
    public class Recorder
    {
        private VideoCapture _VideoCapture;
        private VideoWriter _Writer;
        private Mat _frame = new Mat();
        private string _filePath = null;
        private string _recordingFilePath = null;

        public void Start(string streamUrl, string filePath)
        {
            _filePath = filePath;

            _VideoCapture = new VideoCapture(streamUrl);
            _VideoCapture.ImageGrabbed += VideoStream_NewFrame;
            _VideoCapture.Start();
        }

        public void Stop()
        {
            if (_VideoCapture != null)
            {
                _VideoCapture.ImageGrabbed -= VideoStream_NewFrame;
                _VideoCapture.Stop();
                //Không đợi thì sẽ lỗi khi _VideoCapture.Retrieve(_frame, 0); nó vẫn nhận là _VideoCapture!= null và chạy tiếp. 
                //Không hiểu sao khi stop, start lại thì ở lần thứ 2 stop mới lỗi
                System.Threading.Thread.Sleep(500);
                _VideoCapture.Dispose();
                _VideoCapture = null;
            }

            if (_Writer != null)
            {
                _Writer.Dispose();
                _Writer = null;
            }
        }

        public void MotionStart()
        {
            if (_Writer == null)
            {
                int frameWidth = (int)_VideoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
                int frameHeight = (int)_VideoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
                int fps = (int)_VideoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                if (fps > 24)
                    fps = 24;

                _recordingFilePath = _filePath + "output" + ".mp4";
                int codec = VideoWriter.Fourcc('P', 'I', 'M', '1');

                _Writer = new VideoWriter(_recordingFilePath, codec, fps, new System.Drawing.Size(frameWidth, frameHeight), true);
                Console.WriteLine("File created: " + _recordingFilePath);
            }
        }

        public void MotionEnd()
        {
            if (_Writer != null)
            {
                _Writer.Dispose();
                _Writer = null;
                Console.WriteLine("File dispose: " + _recordingFilePath);
                _recordingFilePath = null;
            }
        }

        private void VideoStream_NewFrame(object sender, EventArgs eventArgs)
        {
            try
            {
                if (_VideoCapture != null && _VideoCapture.Ptr != IntPtr.Zero)
                {
                    //Lấy dữ liệu từ stream
                    _VideoCapture.Retrieve(_frame, 0);

                    //Ghi hình
                    if (_Writer != null)
                    {
                        _Writer.Write(_frame);
                        //Console.WriteLine("Frame write: ");
                        //string fileName = string.Format(@"{0}\{1}.jpg", _filePath, i++);
                        //_frame.Save(fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("VideoStream_NewFrame error");
            }
        }
    }
}
