﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Media.Rtsp.Server.MediaTypes
{
    /// <summary>
    /// The base class of all sources the RtspServer can service.
    /// </summary>
    /// <remarks>
    /// Provides a way to augment all classes from one place.
    /// </remarks>
    public abstract class SourceMedia : Common.BaseDisposable, IMediaSource
    {
        const string UriScheme = "rtspserver://";

        #region StreamState Enumeration

        public enum StreamState
        {
            Stopped,
            Started,
            //Faulted
        }

        #endregion

        #region Fields

        internal DateTime? m_StartedTimeUtc;
        internal Guid m_Id = Guid.NewGuid();
        internal string m_Name;
        internal Uri m_Source;
        internal NetworkCredential m_SourceCred;
        internal List<string> m_Aliases = new List<string>();
        //internal bool m_Child = false;
        public virtual Sdp.SessionDescription SessionDescription { get; protected set; }

        //Maybe should be m_AllowUdp?
        internal bool m_ForceTCP;//= true; // To force clients to utilize TCP, Interleaved in Rtsp or Rtp

        internal bool m_DisableQOS; //Disabled optional quality of service, In Rtp this is Rtcp

        #endregion

        #region Properties

        /// <summary>
        /// The amount of time the Stream has been Started
        /// </summary>
        public TimeSpan Uptime { get { if (m_StartedTimeUtc.HasValue) return DateTime.UtcNow - m_StartedTimeUtc.Value; return TimeSpan.MinValue; } }

        /// <summary>
        /// The unique Id of the RtspStream
        /// </summary>
        public Guid Id { get { return m_Id; } private set { m_Id = value; } }

        /// <summary>
        /// The name of this stream, also used as the location on the server
        /// </summary>
        public virtual string Name { get { return m_Name; } set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name", "Cannot be null or consist only of whitespace"); m_Aliases.Add(m_Name); m_Name = value; } }

        /// <summary>
        /// Any Aliases the stream is known by
        /// </summary>
        public virtual IEnumerable<string> Aliases { get { return m_Aliases; } }

        /// <summary>
        /// The credential the source requires
        /// </summary>
        public virtual NetworkCredential SourceCredential { get { return m_SourceCred; } set { m_SourceCred = value; } }

        /// <summary>
        /// The type of Authentication the source requires for the SourceCredential
        /// </summary>
        public virtual AuthenticationSchemes SourceAuthenticationScheme { get; set; }

        /// <summary>
        /// Gets a Uri which indicates to the RtspServer the name of this stream reguardless of alias
        /// </summary>
        public virtual Uri ServerLocation { get { return new Uri(UriScheme + Id.ToString()); } }

        /// <summary>
        /// State of the stream 
        /// </summary>
        public virtual StreamState State { get; protected set; }

        /// <summary>
        /// The Uri to the source media
        /// </summary>
        public virtual Uri Source { get { return m_Source; } set { m_Source = value; } }

        /// <summary>
        /// Indicates the source is ready to have clients connect
        /// </summary>
        public virtual bool Ready { get; protected set; }

        /// <summary>
        /// Indicates if the souce should attempt to decode frames which change.
        /// </summary>
        public bool DecodeFrames { get; protected set; }

        #endregion

        public bool EnableArchive { get; private set; }

        #region Constructor        

        protected SourceMedia(string name, Uri source, bool enableArchive)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("The stream name cannot be null or consist only of whitespace", "name");
           
            m_Name = name;
            m_Source = source;
            EnableArchive = enableArchive;
        }

        protected SourceMedia(string name, Uri source, NetworkCredential sourceCredential, bool enableArchive)
            :this(name, source, enableArchive)
        {
            m_SourceCred = sourceCredential;
        }

        #endregion

        #region Events

        public delegate void FrameDecodedHandler(object sender, System.Drawing.Image decoded);

        public delegate void DataDecodedHandler(object sender, byte[] decoded);

        public event FrameDecodedHandler FrameDecoded;

        public event DataDecodedHandler DataDecoded;

        internal void OnFrameDecoded(System.Drawing.Image decoded) { if (DecodeFrames && decoded != null && FrameDecoded != null) FrameDecoded(this, decoded); }

        internal void OnFrameDecoded(byte[] decoded) { if (DecodeFrames && decoded != null && DataDecoded != null) DataDecoded(this, decoded); }

        #endregion

        #region Methods

        //Sets the State = StreamState.Started
        public virtual void Start() { State = StreamState.Started; m_StartedTimeUtc = DateTime.UtcNow; }

        //Sets the State = StreamState.Stopped
        public virtual void Stop() { State = StreamState.Stopped; m_StartedTimeUtc = null; }

        public void AddAlias(string name)
        {
            if (m_Aliases.Contains(name)) return;
            m_Aliases.Add(name);
        }

        public void RemoveAlias(string alias)
        {
            m_Aliases.Remove(alias);
        }

        public void ClearAliases() { m_Aliases.Clear(); }

        #endregion

        public override void Dispose()
        {
            if (Disposed) return;

            base.Dispose();

            Stop();
        }
    }
}
