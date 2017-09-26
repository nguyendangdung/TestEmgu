using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI.GLView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenCV_emgu
{
    public partial class Form1 : Form
    {
        const string streamUrl = @"rtsp://giangnt:123456@10.16.0.200/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast";
        //const string streamUrl = "rtsp://10.16.0.100:8295/proxyStream-1";
        //const string streamUrl = @"D:\Dat chuot - Khien.avi";
        const string filePath = @"C:\testdll\Test\AForge_FFMPEG\AForge_FFMPEG\bin\Debug\Result\output.mp4";
        const string imagePath = @"C:\testdll\Test\AForge_FFMPEG\AForge_FFMPEG\bin\Debug\Result\Images";
        private VideoCapture _VideoCapture;
        private VideoWriter _Writer;
        private Mat _frame = new Mat();
        private Image<Bgr, Byte> _image = null;
        private object _locker = new object();
        GLImageView _glView = new GLImageView();

        public Form1()
        {
            InitializeComponent();
            ckUseOpenCL.Checked = CvInvoke.UseOpenCL = CvInvoke.HaveOpenCL;
            videoSourcePlayer.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            videoSourcePlayer.SizeMode = PictureBoxSizeMode.StretchImage;

            _glView.Location = new System.Drawing.Point(-1, 6);
            _glView.Size = new System.Drawing.Size(797, 484);

            this.Controls.Add(_glView);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSource.Text = streamUrl;
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            var files = System.IO.Directory.GetFiles(imagePath);
            foreach (var file in files)
            {
                System.IO.File.Delete(file);
            }
            int frameWidth = (int)_VideoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
            int frameHeight = (int)_VideoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);

            _Writer = new VideoWriter(filePath, 10, new Size(frameWidth, frameHeight), false);
        }

        private void btnStartPlaying_Click(object sender, EventArgs e)
        {
            var vc = new VideoCapture(streamUrl);
            vc.ImageGrabbed += VideoStream_NewFrame;
            vc.Start();
            if (vc.IsOpened)
                _VideoCapture = vc;
            else
            {
                MessageBox.Show("Video source can not connect");
                vc.Dispose();
            }

            if (chkUseGLView.Checked)
            {
                _glView.Visible = true;
                videoSourcePlayer.Visible = false;
            }
            else
            {
                _glView.Visible = false;
                videoSourcePlayer.Visible = true;
            }
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            if (_Writer != null)
            {
                _Writer.Dispose();
                _Writer = null;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        int i = 0;

        private void VideoStream_NewFrame(object sender, EventArgs eventArgs)
        {
            try
            {
                if (_VideoCapture != null && _VideoCapture.Ptr != IntPtr.Zero)
                {
                    if (chkShowImage.Checked)
                    {
                        if (chkUseGLView.Checked)
                        {
                            //Có vẻ bởi vì tryShareData = true nên sẽ đi cặp với _frame => Không phải gọi lại nhiều lần
                            //=> Không phải dispose sau khi hiển thị (Nếu không sẽ tăng size liên tục và rất nhanh)
                            //Phải convert ở đây vì giờ mới có dữ liệu

                            if (_image != null)
                            {
                                _VideoCapture.Retrieve(_image, 0);
                            }
                            else
                            {
                                Mat frame = new Mat();
                                _VideoCapture.Retrieve(frame, 0);
                                _image = frame.ToImage<Bgr, Byte>(tryShareData: false);
                                frame.Dispose();
                            }

                            _glView.Invoke((MethodInvoker)delegate
                            {
                                try
                                {
                                    //Để Rotation mặc định thì sẽ bị hiển thị dọc, chưa biết tại sao
                                    _glView.SetImage(_image, new GeometricChange() { Rotation = 90 }, _glView.Size);
                                }
                                catch { }
                            });
                        }
                        else
                        {
                            _VideoCapture.Retrieve(_frame, 0);
                            videoSourcePlayer.Image = _frame;
                        }
                    }

                    //public void SetImage(Image<Emgu.CV.Structure.Bgr, byte> image, GeometricChange geometricChange);

                    //Ghi hình
                    if (_Writer != null)
                    {
                        _Writer.Write(_frame);
                        string fileName = string.Format(@"{0}\{1}.jpg", imagePath, i++);
                        _frame.Save(fileName);
                    }

                    ////Thêm layout phía trên
                    //DateTime now = DateTime.Now;
                    //Graphics g = Graphics.FromImage(videoSourcePlayer.Image.Bitmap);

                    //// paint current time
                    //SolidBrush brush = new SolidBrush(Color.Red);
                    //g.DrawString(now.ToString(), this.Font, brush, new PointF(5, 5));
                    //brush.Dispose();

                    //g.Dispose();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            var form = (Form)sender;
            _glView.Width = form.Width;
            _glView.Height = form.Height;

            //videoSourcePlayer.Size = new Size(form.Size.Width, form.Size.Height);
            //var size = videoSourcePlayer.Image.Size;
            //size.Width = form.Size.Width;
            //size.Height = form.Size.Height;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        private void Stop()
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

            if (_image != null)
            {
                try
                {
                    _image.Dispose();
                }
                catch { }
                _image = null;
            }
        }

        private void ckUseOpenCL_CheckedChanged(object sender, EventArgs e)
        {
            CvInvoke.UseOpenCL = ((CheckBox)sender).Checked;
        }
    }
}
