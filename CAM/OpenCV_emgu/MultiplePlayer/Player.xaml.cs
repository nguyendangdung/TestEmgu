using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI.GLView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiplePlayer
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        private VideoCapture _VideoCapture;
        private Emgu.CV.UI.ImageBox _imageBox;
        private GLImageView _glImageView;
        private Mat _frame = new Mat();
        private Image<Bgr, Byte> _image = null;
        private bool _showImage;
        private bool _ckGLImageView;
        private GeometricChange _GeometricChange = new GeometricChange() { Rotation = 90 };

        public Player()
        {
            InitializeComponent();
        }

        void MouseEventHandler(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                MessageBox.Show("mouse");
        }

        public async void PlayAsync(string streamUrl, bool showImage, bool ckGLImageView)
        {
            if (ckGLImageView)
            {
                _glImageView = new GLImageView();
                _glImageView.Location = new System.Drawing.Point(-1, 6);
                _glImageView.Size = new System.Drawing.Size(797, 484);
                formHost.Child = _glImageView;
            }
            else
            {
                _imageBox = new Emgu.CV.UI.ImageBox();
                _imageBox.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
                _imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                _imageBox.MouseClick += MouseEventHandler;
                formHost.Child = _imageBox;
            }

            _showImage = showImage;
            _ckGLImageView = ckGLImageView;

            //Chạy Async OK không như VLC
            await Task.Run(() =>
            {
                _VideoCapture = new VideoCapture(streamUrl);
                _VideoCapture.ImageGrabbed += VideoStream_NewFrame;
                _VideoCapture.Start();
            });
        }

        public async void StopAsync()
        {
            //Chạy Async OK không như VLC
            await Task.Run(() =>
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
            });
        }

        private void VideoStream_NewFrame(object sender, EventArgs eventArgs)
        {
            try
            {
                if (_VideoCapture != null && _VideoCapture.Ptr != IntPtr.Zero)
                {
                    bool captured = false;
                    //Lấy dữ liệu từ stream và bind lên màn hình
                    if (_showImage)
                    {
                        if (_ckGLImageView)
                        {
                            if (_image != null)
                            {
                                //Lấy thẳng vào _image, không phải lấy qua Mat rồi thêm bước convert
                                captured = _VideoCapture.Retrieve(_image, 0);
                            }
                            else
                            {
                                Mat frame = new Mat();
                                captured = _VideoCapture.Retrieve(frame, 0);
                                _image = frame.ToImage<Bgr, Byte>(tryShareData: false);
                                frame.Dispose();
                            }

                            _glImageView.Invoke((System.Windows.Forms.MethodInvoker)delegate
                            {
                                try
                                {
                                    //Để Rotation mặc định thì sẽ bị hiển thị dọc, chưa biết tại sao
                                    _glImageView.SetImage(_image, _GeometricChange, _glImageView.Size);
                                }
                                catch { }
                            });
                        }
                        else
                        {
                            captured = _VideoCapture.Retrieve(_frame, 0);
                            _imageBox.Image = _frame;
                            //Thêm layout phía trên
                            DateTime now = DateTime.Now;
                            Graphics g = Graphics.FromImage(_imageBox.Image.Bitmap);

                            // paint current time
                            SolidBrush brush = new SolidBrush(System.Drawing.Color.Red);
                            Font font = new Font(new System.Drawing.FontFamily("Comic Sans MS"), 10);
                            g.DrawString(now.ToString(), font, brush, new PointF(5, 5));
                            brush.Dispose();

                            g.Dispose();
                        }

                        if (captured == false)
                        {

                        }
                    }
                    else
                    {
                        _VideoCapture.Retrieve(_image, 0);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
