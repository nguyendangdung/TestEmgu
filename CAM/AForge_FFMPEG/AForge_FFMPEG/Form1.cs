using AForge.Video;
using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AForge_FFMPEG
{
    public partial class Form1 : Form
    {
        const string streamUrl = @"rtsp://giangnt:123456@10.16.0.137/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast";
        //const string streamUrl = @"D:\Dat chuot - Khien.avi";
        const string filePath = @"C:\testdll\Test\AForge_FFMPEG\AForge_FFMPEG\bin\Debug\Result\output.mp4";
        const string imagePath = @"C:\testdll\Test\AForge_FFMPEG\AForge_FFMPEG\bin\Debug\Result\Images";
        private static AForge.Video.FFMPEG.VideoFileSource _VideoStream;
        private static VideoFileWriter _Writer;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSource.Text = streamUrl;
            videoSourcePlayer.NewFrame += videoSourcePlayer_NewFrame;
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            var files = System.IO.Directory.GetFiles(imagePath);
            foreach (var file in files)
            {
                System.IO.File.Delete(file);
            }
            _Writer = new VideoFileWriter();
            _VideoStream.NewFrame += VideoStream_NewFrame;
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            _VideoStream.NewFrame -= VideoStream_NewFrame;
            _Writer.Close();
            _Writer = null;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        int i = 0;
        void VideoStream_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                this.Text = "Got frame " + i.ToString();
                if (!_Writer.IsOpen)
                {
                    _Writer.Open(filePath, eventArgs.Frame.Width, eventArgs.Frame.Height, 20, VideoCodec.MPEG4);
                }
                if (_Writer.IsOpen)
                {
                    _Writer.WriteVideoFrame(eventArgs.Frame);
                    string fileName = string.Format(@"{0}\{1}.jpg", imagePath, i++);
                    eventArgs.Frame.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {

            }
        }

        // New frame received by the player
        private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            return;

            DateTime now = DateTime.Now;
            Graphics g = Graphics.FromImage(image);

            // paint current time
            SolidBrush brush = new SolidBrush(Color.Red);
            g.DrawString(now.ToString(), this.Font, brush, new PointF(5, 5));
            brush.Dispose();

            g.Dispose();
        }

        private void btnStartPlaying_Click(object sender, EventArgs e)
        {
            _VideoStream = new AForge.Video.FFMPEG.VideoFileSource(txtSource.Text);
            videoSourcePlayer.VideoSource = _VideoStream;
            _VideoStream.Start();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            var form = (Form)sender;
            videoSourcePlayer.Size = new Size(form.Size.Width, form.Size.Height);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            if (_VideoStream != null)
            {
                _VideoStream.SignalToStop();
                _VideoStream.WaitForStop();
                _VideoStream.Stop();
            }

            if (_Writer != null)
            {
                _Writer.Close();
            }
        }
    }
}
