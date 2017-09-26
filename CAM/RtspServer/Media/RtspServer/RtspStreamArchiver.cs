using Media.Rtsp.Server.MediaTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Media.Rtsp.Server
{
    public class RtspStreamArchiver : Common.BaseDisposable
    {
        // Trinh tu ghi vao cac Driver
        // Gioi han Size cua tung file de roll

        private string _baseDirectory;
        private IDictionary<IMedia, RtpTools.RtpDump.Program> Attached = new System.Collections.Concurrent.ConcurrentDictionary<IMedia, RtpTools.RtpDump.Program>();

        public RtspStreamArchiver(string directory)
        {
            if(string.IsNullOrWhiteSpace(directory))
                directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "Archive";

            _baseDirectory = directory;
            if (!System.IO.Directory.Exists(_baseDirectory))
                System.IO.Directory.CreateDirectory(_baseDirectory);
        }

        //Creates directories
        private void Prepare(IMedia stream)
        {
            if (!System.IO.Directory.Exists(_baseDirectory + '/' + stream.Id))
            {
                System.IO.Directory.CreateDirectory(_baseDirectory + '/' + stream.Id);
            }
        }

        //Determine if directory is created
        public virtual bool IsArchiving(IMedia stream)
        {
            return Attached.ContainsKey(stream);
        }

        //Writes a .Sdp file
        public virtual void WriteDescription(IMedia stream, Sdp.SessionDescription sdp)
        {
            if (!IsArchiving(stream)) return;

            //Add lines with Alias info?

            System.IO.File.WriteAllText(_baseDirectory + '/' + stream.Id + '/' + "SessionDescription.sdp", sdp.ToString());
        }

        //Writes a RtpToolEntry for the packet
        private void WritePacket(IMedia stream, Common.IPacket packet)
        {
            if (stream == null) return;

            RtpTools.RtpDump.Program program;
            if (!Attached.TryGetValue(stream, out program)) return;

            if (packet is Rtp.RtpPacket) program.Writer.WritePacket(packet as Rtp.RtpPacket);
            program.Writer.WritePacket(packet as Rtcp.RtcpPacket);
        }

        public virtual void Start(IMedia stream, RtpTools.FileFormat format = RtpTools.FileFormat.Binary)
        {
            if (stream is RtpSource)
            {
                RtpTools.RtpDump.Program program;
                if (Attached.TryGetValue(stream, out program)) return;

                Prepare(stream);

                program = new RtpTools.RtpDump.Program();
                Attached.Add(stream, program);

                (stream as RtpSource).RtpClient.RtpPacketReceieved += RtpClientPacketReceieved;
                (stream as RtpSource).RtpClient.RtcpPacketReceieved += RtpClientPacketReceieved;
            }
        }

        void RtpClientPacketReceieved(object sender, Common.IPacket packet)
        {
            if (sender is Rtp.RtpClient)
                WritePacket(Attached.Keys.FirstOrDefault(s => (s as RtpSource).RtpClient == sender as Rtp.RtpClient), packet);
        }

        //Stop recoding a stream
        public virtual void Stop(IMedia stream)
        {
            if (stream is RtpSource)
            {
                RtpTools.RtpDump.Program program;
                if (!Attached.TryGetValue(stream, out program)) return;

                program.Dispose();
                Attached.Remove(stream);

                (stream as RtpSource).RtpClient.RtpPacketReceieved -= RtpClientPacketReceieved;
                (stream as RtpSource).RtpClient.RtcpPacketReceieved -= RtpClientPacketReceieved;
            }
        }

        public override void Dispose()
        {

            if (Disposed) return;

            base.Dispose();

            foreach (var stream in Attached.Keys.ToArray())
                Stop(stream);

            Attached = null;
        }
    }
}
