
using System;
using System.Windows.Forms;

using Media.Rtsp;
using Media.Rtsp.Server.MediaTypes;

namespace Media.UI.Management
{
    public partial class FormMain : Form
    {
        private RtspServer _server = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_server != null)
                return;

            IMediaSource source = null;
            switch (cboSourceType.Text)
            {
                case "JPEG":
                    source = new JPEGMedia(txtSourceName.Text, txtSourceLink.Text, chkSaveResult.Checked);
                    break;
                case "MJPEG":
                    source = new MJPEGMedia(txtSourceName.Text, txtSourceLink.Text, chkSaveResult.Checked);
                    break;
                default:
                    source = new RtspSource(txtSourceName.Text, txtSourceLink.Text, chkSaveResult.Checked);
                    break;
            }

            _server = new RtspServer(listenPort: int.Parse(txtServerPort.Text));
            if (chkSaveResult.Checked && !string.IsNullOrWhiteSpace(txtArchiveFolder.Text))
                _server.Archiver = new Rtsp.Server.RtspStreamArchiver(txtArchiveFolder.Text);
            _server.AddMedia(source);
            txtOutLink.Text = source.ServerLocation.ToString(); var a = System.Threading.Timeout.InfiniteTimeSpan;
            _server.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_server != null)
            {
                _server.Stop();
                _server.Dispose();
                _server = null;
            }
        }
    }
}
