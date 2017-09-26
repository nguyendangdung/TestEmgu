
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Media.Rtsp.Server.MediaTypes
{
    /// <summary>
    /// Provides the basic operations for consuming a remote rtp stream for which there is an existing <see cref="SessionDescription"/>
    /// </summary>
    public class RtpSource : SourceMedia, Common.IThreadReference
    {
        public RtpSource(string name, Uri source, bool enableArchive) : base(name, source, enableArchive) { }
        
        public bool RtcpDisabled { get { return m_DisableQOS; } set { m_DisableQOS = value; } }

        public virtual Rtp.RtpClient RtpClient { get; protected set; }

        //This will take effect after the change, existing clients will still have their connection
        public bool ForceTCP { get { return m_ForceTCP; } set { m_ForceTCP = value; } } 
        
        public override void Start()
        {
            //Add handler for frame events
            if (State == StreamState.Stopped)
            {
                if (RtpClient != null)
                {
                    RtpClient.Connect();

                    base.Ready = true;

                    base.Start();
                }
            }
        }

        public override void Stop()
        {
            //Remove handler
            if (State == StreamState.Started)
            {
                if (RtpClient != null) RtpClient.Disconnect();

                base.Ready = false;

                base.Stop();
            }
        }

        public override void Dispose()
        {
            if (Disposed) return;
            base.Dispose();
            if (RtpClient != null) RtpClient.Dispose();
        }

        public RtpSource(string name, Sdp.SessionDescription sessionDescription, bool enableArchive)
            : base(name, new Uri(Rtp.RtpClient.RtpProtcolScheme + "://" + ((Sdp.Lines.SessionConnectionLine)sessionDescription.ConnectionLine).IPAddress), enableArchive)
        {
            if (sessionDescription == null) throw new ArgumentNullException("sessionDescription");

            RtpClient = Rtp.RtpClient.FromSessionDescription(SessionDescription = sessionDescription);
        }

        IEnumerable<System.Threading.Thread> Common.IThreadReference.GetReferencedThreads()
        {
            return RtpClient != null ? Utility.Yield(RtpClient.m_WorkerThread) : null;
        }
    }
}