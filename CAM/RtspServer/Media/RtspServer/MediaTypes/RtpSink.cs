
using System;
using System.Collections.Generic;
using System.Linq;

namespace Media.Rtsp.Server.MediaTypes
{
    /// <summary>
    /// Provides the basic opertions for any locally created Rtp data
    /// </summary>
    public class RtpSink : RtpSource, IMediaSink
    {
        public RtpSink(string name, Uri source, bool enableArchive) : base(name, source, enableArchive) { }

        public virtual bool Loop { get; set; }

        protected Queue<Common.IPacket> Packets = new Queue<Common.IPacket>();

        public void SendData(byte[] data)
        {
            if (RtpClient != null) RtpClient.OnRtpPacketReceieved(new Rtp.RtpPacket(data, 0));
        }

        public void EnqueData(byte[] data)
        {
            if (RtpClient != null) Packets.Enqueue(new Rtp.RtpPacket(data, 0));
        }

        public void SendPacket(Common.IPacket packet)
        {
            if (RtpClient != null)
            {
                if (packet is Rtp.RtpPacket) RtpClient.OnRtpPacketReceieved(packet as Rtp.RtpPacket);
                else if (packet is Rtcp.RtcpPacket) RtpClient.OnRtcpPacketReceieved(packet as Rtcp.RtcpPacket);
            }
        }

        public void EnquePacket(Common.IPacket packet)
        {
            if (RtpClient != null) Packets.Enqueue(packet);
        }

        public void SendReports()
        {
            if (RtpClient != null) RtpClient.SendReports();
        }

        internal virtual void SendPackets()
        {
            while (State == StreamState.Started)
            {
                try
                {
                    if (Packets.Count == 0)
                    {
                        System.Threading.Thread.Sleep(0);
                        continue;
                    }

                    //Dequeue a frame or die
                     Common.IPacket packet = Packets.Dequeue();

                     SendPacket(packet);

                    //If we are to loop images then add it back at the end
                    if (Loop) Packets.Enqueue(packet);

                    //Check for bandwidth and sleep if necessary
                }
                catch (Exception ex)
                {
                    if (ex is System.Threading.ThreadAbortException) return;
                    continue;
                }
            }
        }
    }
}
