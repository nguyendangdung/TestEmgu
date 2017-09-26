
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Media.Rtsp.Server.MediaTypes
{
    /// <summary>
    /// A remote stream the RtspServer aggregates and can be played by clients.
    /// </summary>    
    public class RtspSource : RtpSource
    {
        /// <summary>
        /// If not null the only type of media which will be setup from the source.
        /// </summary>
        public readonly Sdp.MediaType? SpecificMediaType;

        /// <summary>
        /// If not null, The time at which to start the media in the source.
        /// </summary>
        public readonly TimeSpan? MediaStartTime, MediaEndTime;

        #region Properties

        /// <summary>
        /// Gets the RtspClient this RtspSourceStream uses to provide media
        /// </summary>
        public virtual RtspClient RtspClient { get; set; }

        /// <summary>
        /// Gets the RtpClient used by the RtspClient to provide media
        /// </summary>
        public override Rtp.RtpClient RtpClient { get { return RtspClient.Client; } }

        public override NetworkCredential SourceCredential
        {
            get
            {
                return base.SourceCredential;
            }
            set
            {
                if (RtspClient != null) RtspClient.Credential = value;
                base.SourceCredential = value;
            }
        }

        public override AuthenticationSchemes SourceAuthenticationScheme
        {
            get
            {
                return base.SourceAuthenticationScheme;
            }
            set
            {
                if (RtspClient != null) RtspClient.AuthenticationScheme = value;
                base.SourceAuthenticationScheme = value;
            }
        }

        /// <summary>
        /// SessionDescription from the source RtspClient
        /// </summary>
        public override Sdp.SessionDescription SessionDescription { get { return RtspClient.SessionDescription; } }

        /// <summary>
        /// Gets or sets the source Uri used in the RtspClient
        /// </summary>
        public override Uri Source
        {
            get
            {
                return base.Source;
            }
            set
            {
                //Experimental support for Unreliable and Http enabled with this line commented out
                if (value.Scheme != RtspMessage.ReliableTransport) throw new ArgumentException("value", "Must have the Reliable Transport scheme \"" + RtspMessage.ReliableTransport + "\"");

                base.Source = value;

                if (RtspClient != null)
                {
                    bool wasConnected = RtspClient.Connected;

                    if (wasConnected) Stop();

                    RtspClient.Location = base.Source;

                    if (wasConnected) Start();
                }
            }
        }

        /// <summary>
        /// Indicates if the source RtspClient is Connected and has began to receive data via Rtp
        /// </summary>
        public override bool Ready { get { return base.Ready && RtspClient != null && RtspClient.Playing; } }

        #endregion

        #region Constructor

        public RtspSource(string name, string location, bool enableArchive, RtspClient.ClientProtocolType rtpProtocolType, 
            int bufferSize = 8192, Sdp.MediaType? specificMedia = null, TimeSpan? startTime = null, TimeSpan? endTime = null)
            : this(name, location, enableArchive, null, AuthenticationSchemes.None, rtpProtocolType, bufferSize, specificMedia, startTime, endTime) { }

        public RtspSource(string name, string sourceLocation, bool enableArchive, NetworkCredential credential = null, 
            AuthenticationSchemes authType = AuthenticationSchemes.None, 
            Rtsp.RtspClient.ClientProtocolType? rtpProtocolType = null, 
            int bufferSize = 8192, Sdp.MediaType? specificMedia = null, TimeSpan? startTime = null, TimeSpan? endTime = null)
            : this(name, new Uri(sourceLocation), enableArchive, credential, authType, rtpProtocolType, bufferSize, specificMedia, startTime, endTime) { }

        /// <summary>
        /// Constructs a RtspStream for use in a RtspServer
        /// </summary>
        /// <param name="name">The name given to the stream on the RtspServer</param>
        /// <param name="sourceLocation">The rtsp uri to the media</param>
        /// <param name="credential">The network credential the stream requires</param>
        /// /// <param name="authType">The AuthenticationSchemes the stream requires</param>
        public RtspSource(string name, Uri sourceLocation, bool enableArchive, NetworkCredential credential = null, 
            AuthenticationSchemes authType = AuthenticationSchemes.None, 
            Rtsp.RtspClient.ClientProtocolType? rtpProtocolType = null, 
            int bufferSize = 8192, Sdp.MediaType? specificMedia = null, TimeSpan? startTime = null, TimeSpan? endTime = null)
            : base(name, sourceLocation, enableArchive)
        {
            //Create the listener if we are the top level stream (Parent)
            RtspClient = new RtspClient(m_Source, rtpProtocolType, bufferSize);

            if (credential != null)
            {
                RtspClient.Credential = SourceCredential = credential;

                if (authType != AuthenticationSchemes.None) RtspClient.AuthenticationScheme = SourceAuthenticationScheme = authType;
            }

            //If only certain media should be setup 
            if (specificMedia.HasValue) SpecificMediaType = specificMedia;

            //If there was a start time given
            if (startTime.HasValue) MediaStartTime = startTime;

            if (endTime.HasValue) MediaEndTime = endTime;
        }

        #endregion

        /// <summary>
        /// Beings streaming from the source
        /// </summary>
        public override void Start()
        {
            if (Disposed) return;

            if (!RtspClient.Connected)
            {
                RtspClient.OnConnect += RtspClient_OnConnect;
                RtspClient.OnDisconnect += RtspClient_OnDisconnect;
                RtspClient.OnPlay += RtspClient_OnPlay;
                RtspClient.OnStop += RtspClient_OnStop;
                RtspClient.Connect();
            }
            else if (!RtspClient.Playing) RtspClient.StartPlaying(MediaStartTime, MediaStartTime, SpecificMediaType);
        }

        void RtspClient_OnStop(RtspClient sender, object args)
        {
            base.Ready = false;
        }

        void RtspClient_OnPlay(RtspClient sender, object args)
        {
            RtspClient.Client.FrameChangedEventsEnabled = true;
            base.Ready = true;
        }

        void RtspClient_OnDisconnect(RtspClient sender, object args)
        {
            base.Ready = false;
        }

        void RtspClient_OnConnect(RtspClient sender, object args)
        {
            if (RtspClient != sender || RtspClient.Playing) return;
            RtspClient.OnConnect -= RtspClient_OnConnect;
            try
            {
                //Start listening
                RtspClient.StartPlaying(MediaStartTime, MediaEndTime, SpecificMediaType);

                //Set the time for stats
                m_StartedTimeUtc = DateTime.UtcNow;

                //Call base to set started etc.
                base.Start();
            }
            catch (Common.Exception<RtspClient>)
            {
                //Wrong Credentails etc...

                //Call base to set to stopped
                Stop();
            }
            catch
            {
                //Stop?
                throw;
            }
        }

        /// <summary>
        /// Stops streaming from the source
        /// </summary>
        public override void Stop()
        {

            if (State != StreamState.Started) return;

            if (RtspClient != null && RtspClient.Connected)
            {
                RtspClient.Disconnect();
            }

            RtspClient.OnConnect -= RtspClient_OnConnect;
            RtspClient.OnDisconnect -= RtspClient_OnDisconnect;
            RtspClient.OnPlay -= RtspClient_OnPlay;
            RtspClient.OnStop -= RtspClient_OnStop;

            base.Stop();

            m_StartedTimeUtc = null;
        }

        public override void Dispose()
        {
            if (Disposed) return;

            base.Dispose();

            if (RtspClient != null)
            {
                RtspClient.Dispose();
                RtspClient = null;
            }
        }
    }
}
