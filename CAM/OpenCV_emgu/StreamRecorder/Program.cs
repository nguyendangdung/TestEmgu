using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamRecorder
{
    class Program
    {
        static Recorder _recorder = new Recorder();

        static void Main(string[] args)
        {
            string url = "rtsp://giangnt:123456@10.16.0.200/onvif-media/media.amp?profile=OneVision_Record_motion&sessiontimeout=60&streamtype=unicast";
            //@"rtsp://giangnt:123456@10.16.0.200/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast"
            _recorder.Start(url,
                            @"E:\Projects\21. Camera\OneVision\OneVision.Service\OneVision.Service.Console\bin\Debug\Temporary\");

            Console.ReadLine();
            _recorder.MotionStart();

            Console.ReadLine();
            _recorder.MotionEnd();
            _recorder.Stop();
            Console.ReadLine();
        }
    }
}
