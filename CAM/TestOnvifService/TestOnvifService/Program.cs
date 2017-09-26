using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TestOnvifService.DeviceMngtService;
using TestOnvifService.EventService;
using TestOnvifService.MediaService;
using TestOnvifService.PTZService;

namespace TestOnvifService
{
    public class Program
    {
        //Chú ý:
        //1. Fix lỗi không truy cập được:
        //      => System Options/Advanced/Plain Config/Select a group of parameters to modify/WebService/
        //      => Uncheck WebService UsernameToken Enable replay attack protection
        //link local chưa hiểu là gì: 169.254.175.118
        
        public static ServiceProvider provider = new ServiceProvider(ServiceProvider.NetworkProtocolType.HTTP, "10.16.0.137", 80, "giangnt", "123456"); // Onvif version: 1.0.2
        //public static ServiceProvider provider = new ServiceProvider(ServiceProvider.NetworkProtocolType.HTTP, "192.168.0.20", 80, "giangnt", "123456");// Onvif version: 2.50

        static void Main(string[] args)
        {
            try
            {
                Process();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        private static async void Process()
        {
            string profileToken = "profile_1_h264";

            NonAuthen();
            //GetDeviceInfomation();
            //SetDynamicIP();
            //SetStaticIP("192.168.2.137");

            //Zoom("profile_1_h264");
            //PanTilt(profileToken);
            //ContinuousMove(profileToken);
            //Preset(profileToken);

            //GetConfigurations();
            //GetProfileInfo();
            //CreateNewMediaProfile();
            //SetVideoEncoderConfigurationOptionsBeforePlay(profileToken);
            //VideoStreaming_Unicast();
            //VideoStreaming_Multicast();
            //GetVideoEncoderConfigurationOptions(profileToken);
            //MetadataStreaming();

            PullMessage();
            //WSBaseNotification();
            //ProcessEvent();
        }

        #region Device

        private static async void GetDeviceInfomation()
        {
            using (var client = provider.CreateDeviceClient())
            {
                var devcaps = client.GetCapabilities(new CapabilityCategory[] { CapabilityCategory.All });

                string Model; //AXIS P5624-E MkII
                string FirmwareVersion; //6.35.1.1
                string SerialNumber;    //Device ID: ACCC8E75790A
                string HardwareId;  //729
                string manufacture = client.GetDeviceInformation(out Model, out FirmwareVersion, out SerialNumber, out HardwareId); //AXIS

                //Two digit minor version number.
                string ONVIFversion = string.Format("{0}.{1:D2}", devcaps.Device.System.SupportedVersions[0].Major, devcaps.Device.System.SupportedVersions[0].Minor); //1.02

                var npt = client.GetNTP();
                var caps = client.GetServiceCapabilities();
                var logs = client.GetSystemLog(SystemLogType.System); //Method này không chạy với Pelco
                var url = client.GetWsdlUrl();//http://192.168.2.16/onvif/wsdl/onvif/
                var services = client.GetServices(true);
                var interfaces = client.GetNetworkInterfaces();
                var gateway = client.GetNetworkDefaultGateway();
                var protocols = client.GetNetworkProtocols();

                var ipAddress = interfaces[0].IPv4.Config.FromDHCP.Address; //192.168.2.16
                var MACAddress = interfaces[0].Info.HwAddress;//AC-CC-8E-75-79-0A
            }
        }

        public static async void SetStaticIP(string newAddress)
        {
            if (string.IsNullOrWhiteSpace(newAddress))
                throw new ArgumentNullException();

            using (var client = provider.CreateDeviceClient())
            {
                var nwInterface = client.GetNetworkInterfaces()[0];

                //create the parameter object of the SetNetworkInterfaces
                var address = new TestOnvifService.DeviceMngtService.PrefixedIPv4Address()
                {
                    Address = newAddress, //ip address
                    PrefixLength = 24, //netmask
                };
                string interfaceToken = nwInterface.token; //select or set the interface token
                NetworkInterfaceSetConfiguration netCfg = new NetworkInterfaceSetConfiguration();
                netCfg.Enabled = true; //enable this new configuration
                netCfg.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                netCfg.IPv4.Enabled = true; //enable it to be IPv4
                netCfg.IPv4.Manual = new TestOnvifService.DeviceMngtService.PrefixedIPv4Address[] { address };
                netCfg.IPv4.DHCP = false; // static configuration
                netCfg.IPv4.DHCPSpecified = true; // static configuration
                //netCfg.IPv4.EnabledSpecified = true; // static configuration

                var rebootNeeded = client.SetNetworkInterfaces(interfaceToken, netCfg);
                if (rebootNeeded)
                {
                    string message = client.SystemReboot();
                }

                //client.SetNetworkDefaultGateway(new string[] { "192.168.2.1" }, null); //Programmers_Guide: Not referenced
            }
        }

        public static async void SetDynamicIP()
        {
            using (var client = provider.CreateDeviceClient())
            {
                var nwInterface = client.GetNetworkInterfaces()[0];

                string interfaceToken = nwInterface.token; //select or set the interface token
                NetworkInterfaceSetConfiguration netCfg = new NetworkInterfaceSetConfiguration();
                netCfg.Enabled = true; //enable this new configuration
                netCfg.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
                netCfg.IPv4.Enabled = true; //enable it to be IPv4
                netCfg.IPv4.Manual = nwInterface.IPv4.Config.Manual;//Giữ lại thông tin static ip đang có
                netCfg.IPv4.DHCP = true; // static configuration
                netCfg.IPv4.DHCPSpecified = true; // static configuration

                var rebootNeeded = client.SetNetworkInterfaces(interfaceToken, netCfg);
                if (rebootNeeded)
                {
                    string message = client.SystemReboot();
                }
            }
        }

        private static void NonAuthen()
        {
            var client = new DeviceClient(provider.NonAuthCustomBinding, provider.DeviceServiceAddress);
            var dt = client.GetSystemDateAndTime();
            provider.SyncTime(dt.UTCDateTime);

            var services = client.GetServices(true);
            provider.StoreServices(services);
        }

        #endregion

        #region PTZ

        private static async void Zoom(string profileToken)
        {
            //Chú ý: Các giá trị x/y nhận giá trị float từ (-1, 1)
            using (var client = provider.CreatePTZClient())
            {
                var ztranslation = new PTZVector() { Zoom = new PTZService.Vector1D() { x = 0.01f, space = null } };
                var zspeed = new TestOnvifService.PTZService.PTZSpeed() { Zoom = new PTZService.Vector1D() { x = 1, space = null } };
                client.RelativeMove(profileToken, ztranslation, zspeed);
            }
        }

        private static async void PanTilt(string profileToken)
        {
            //Chú ý: Các giá trị x/y nhận giá trị float từ (-1, 1)

            using (var client = provider.CreatePTZClient())
            {
                //PanTilt
                var translation = new PTZVector() { PanTilt = new PTZService.Vector2D() { x = -0.1f, y = -0.1f }, };
                var speed = new TestOnvifService.PTZService.PTZSpeed() { PanTilt = new PTZService.Vector2D() { x = 0.5f, y = 0.5f }, };
                client.RelativeMove(profileToken, translation, speed);
            }
        }

        private static async void ContinuousMove(string profileToken)
        {
            //Chú ý: Các giá trị x/y nhận giá trị float từ (-1, 1)
            //PanTilt
            //var speed = new TestOnvifService.PTZService.PTZSpeed() { PanTilt = new PTZService.Vector2D() { x = 0.1f, y = 0f }, };
            //client.ContinuousMove(profileToken, speed, null);

            //System.Threading.Thread.Sleep(2000);
            //client.Stop(profileToken, true, true);

            const float speed = 0.2f;
            using (var client = provider.CreatePTZClient())
            {
                while (true)
                {
                    var key = Console.ReadKey();
                    Console.WriteLine(key.Key);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            client.Stop(profileToken, true, true);
                            return;
                        case ConsoleKey.RightArrow:
                            {
                                ContinuousMove(client, profileToken, speed, 0);
                                break;
                            }
                        case ConsoleKey.LeftArrow:
                            {
                                ContinuousMove(client, profileToken, -speed, 0);
                                break;
                            }
                        case ConsoleKey.UpArrow:
                            {
                                ContinuousMove(client, profileToken, 0f, speed);
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                ContinuousMove(client, profileToken, 0f, -speed);
                                break;
                            }
                        default:
                            client.Stop(profileToken, true, true);
                            break;
                    }
                }
            }
        }
        private static async void ContinuousMove(PTZClient client, string profileToken, float x, float y)
        {
            var speed = new TestOnvifService.PTZService.PTZSpeed() { PanTilt = new PTZService.Vector2D() { x = x, y = y }, };
            client.ContinuousMove(profileToken, speed, null);
        }

        private static async void Preset(string profileToken)
        {
            using (var client = provider.CreatePTZClient())
            {
                var presets = client.GetPresets(profileToken);
                foreach (var preset in presets)
                {
                    if (preset.Name != "HomeTest")
                        client.RemovePreset(profileToken, preset.token);
                }

                string presetToken = null;
                client.SetPreset(profileToken, "DieuHoa1", ref presetToken);

                var speed = new TestOnvifService.PTZService.PTZSpeed() { PanTilt = new PTZService.Vector2D() { x = 0.5f, y = 0.5f }, };
                client.GotoPreset(profileToken, presetToken, speed);
            }
        }

        #endregion

        #region Media

        private static async void GetProfileInfo()
        {
            //Media
            using (MediaClient client = provider.CreateMediaClient())
            {
                Profile[] profiles = client.GetProfiles();
                foreach (var item in profiles)
                {
                    var profile = client.GetProfile(item.token);
                    var videoEncoder = profile.VideoEncoderConfiguration;

                    string name = profile.Name;
                    string token = profile.token;
                    int userCount = videoEncoder.UseCount;
                    var encoding = videoEncoder.Encoding;
                    string resolution = string.Format("{0}x{1}", videoEncoder.Resolution.Width, videoEncoder.Resolution.Height);
                    string timeOut = profile.VideoEncoderConfiguration.SessionTimeout;
                    float quality = videoEncoder.Quality;
                    var frameRate = videoEncoder.RateControl.FrameRateLimit;
                    var bitRate = videoEncoder.RateControl.BitrateLimit;
                    var encodingInterval = videoEncoder.RateControl.EncodingInterval;
                }
            }
        }

        private static void GetConfigurations()
        {
            using (MediaClient client = provider.CreateMediaClient())
            {
                var optionsEncoder = client.GetVideoEncoderConfigurationOptions(null, null);
                var optionsSource = client.GetVideoSourceConfigurationOptions(null, null);

                var configs1 = client.GetMetadataConfigurations().ToList();
                var configs0 = client.GetMetadataConfigurationOptions("OneVision_Default", configs1[0].token);

                var configs2 = client.GetVideoEncoderConfigurations().ToList();
                var configs3 = client.GetVideoSourceConfigurations().ToList();
            }

            using (PTZClient client = provider.CreatePTZClient())
            {
                var configs4 = client.GetConfigurations().ToList();

            }
        }

        private static async void CreateNewMediaProfile()
        {
            //Có vẻ như không được tạo thêm VideoEncoderConfiguration, chỉ được dùng những item đã được tạo sẵn
            //Các method AddVideoEncoderConfiguration/RemoveVideoEncoderConfiguration chỉ là gán config có sẵn vào userprofile mà thôi
            //Cam Axis cung cấp từ user0 => user15 và 10 loại đã được đặt tên theo mục đích
            //Đổi tên thì ok, sử dụng SetVideoEncoderConfiguration()


            // create the media object to use the service
            using (MediaClient client = provider.CreateMediaClient())
            {

                // create a new profile token
                string name = "new profileName";
                string token = "new_profileToken";
                var mediaProfile = client.CreateProfile(name, token);

                // video source configuration must be added first.
                // get all video source configurations
                var sourceConfigurationsList = client.GetVideoSourceConfigurations();
                //use the first configuration and SourceEncoderConfigurations have at least one
                var sourceConfigurationToken = sourceConfigurationsList[0].token;
                client.AddVideoSourceConfiguration(token, sourceConfigurationToken);
                // add video encoder configuration later
                // get all video encoder configurations
                var encoderConfigurationsList = client.GetVideoEncoderConfigurations();
                //search H.264 streaming
                foreach (var encoderConfiguraton in encoderConfigurationsList)
                {
                    if (encoderConfiguraton.Encoding == MediaService.VideoEncoding.H264)
                    {
                        // add video encoder configuration
                        var encoderConfigurationToken = encoderConfiguraton.token;

                        client.AddVideoEncoderConfiguration(token, encoderConfigurationToken);
                        break;
                    }
                }
                //now the stream can be started as already shown in chapter 7.1 
            }
        }

        private static async void VideoStreaming_Unicast()
        {
            //Flow của ODM khi play video (Tab Video streaming):
            //1. Get toàn bộ profiles của user đang login (tab Profiles)
            //2. Load các giá trị có thể có để bind control từ GetVideoEncoderConfigurationOptions của cả 2 profiles
            //2. Chọn profile đầu tiên làm mặc định (hoặc được chọn), load lên tab Video Streaming làm giá trị hiện tại
            //3. Khi ấn Apply:
            //  3.1. Set lại giá trị cho Profile
            //  3.2. Play lại stream để nhận giá trị mới???????????????

            // => Thực tế với app có thể:
            // Khi user setup profile => Tạo profile tương ứng luôn
            // Khi play => Play luôn từ profile đã tạo.
            // Abnormal cases: Profile bị xóa... => Kiểm tra profile có không, không có thì tạo rồi play

            using (MediaClient client = provider.CreateMediaClient())
            {
                Profile[] profiles = client.GetProfiles();
                var profile = profiles[1];
                string profileToken = profile.token;

                // setup stream configuration
                var streamSetup = GetStreamSetup(StreamType.RTPUnicast);
                // get stream URI
                //rtsp://192.168.2.137/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast
                //Để chạy dc mà ko bị hỏi username/pass: rtsp://giangnt:123456@192.168.2.137:554/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=10&streamtype=unicast
                //Nếu profile chưa được cấu hình VideoEncoderConfiguration thì sẽ báo lỗi
                var streamUri = client.GetStreamUri(streamSetup, profileToken);
            }
        }

        private static async void VideoStreaming_Multicast()
        {
            int i = 0;
            using (MediaClient client = provider.CreateMediaClient())
            {
                // get profiles
                var profilesList = client.GetProfiles();
                var profile = profilesList[i];
                // use the first profile and Proiles have at least one
                var mediaProfileToken = profile.token;

                // The client confirms Multicast parameter inside all of configurations that are added to the selected profile.
                if (profile.VideoEncoderConfiguration != null)
                {
                    SetupMulticast(profile.VideoEncoderConfiguration.Multicast, 0, "239.192.10.11");
                    client.SetVideoEncoderConfiguration(profile.VideoEncoderConfiguration, true);
                }
                if (profile.AudioEncoderConfiguration != null)
                {
                    SetupMulticast(profile.AudioEncoderConfiguration.Multicast);
                    client.SetAudioEncoderConfiguration(profile.AudioEncoderConfiguration, true);
                }
                if (profile.MetadataConfiguration != null)
                {
                    SetupMulticast(profile.MetadataConfiguration.Multicast);
                    client.SetMetadataConfiguration(profile.MetadataConfiguration, true);
                }

                // Setup stream configuration
                var streamSetup = GetStreamSetup(StreamType.RTPMulticast);
                // Get stream URI
                //rtsp://192.168.2.137/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=multicast
                //Để chạy dc mà ko bị hỏi username/pass: rtsp://giangnt:123456@192.168.2.137/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=multicast
                var mediaUri = client.GetStreamUri(streamSetup, mediaProfileToken);
            }
        }

        private static StreamSetup GetStreamSetup(StreamType streamType, TransportProtocol protocol = TransportProtocol.RTSP)
        {
            // Setup stream configuration
            var streamSetup = new StreamSetup();
            streamSetup.Stream = streamType;
            // Set Transport.Protocol to:
            // UDP for RTP/UDP
            // RTSP for RTP/RTSP (over TCP)
            // HTTP for RTP/RTSP/HTTP 
            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = protocol;
            // RTP/RTSP/UDP is not a special tunnelling setup (is not requiring)!
            streamSetup.Transport.Tunnel = null;
            return streamSetup;
        }

        private static void SetupMulticast(TestOnvifService.MediaService.MulticastConfiguration multicastCfg, int port = 0, string ipv4Address = null)
        {
            // set these parameters for UDP multicast.
            multicastCfg.Address.Type = MediaService.IPType.IPv4;
            if (ipv4Address != null)
                multicastCfg.Address.IPv4Address = ipv4Address; //"239.192.10.10" Cần 1 địa chỉ lớp D
            multicastCfg.Port = port; //30004
            multicastCfg.TTL = 5;
        }

        private static async void SetVideoEncoderConfigurationOptionsBeforePlay(string profileToken)
        {
            using (MediaClient client = provider.CreateMediaClient())
            {
                var profiles = client.GetProfiles();
                //var profile = profiles.First(c => c.token.Equals(profileToken, StringComparison.OrdinalIgnoreCase));
                var profile = profiles[2];

                var veConfig = profile.VideoEncoderConfiguration;

                veConfig.Encoding = MediaService.VideoEncoding.JPEG;
                veConfig.RateControl.FrameRateLimit = 10;
                veConfig.Resolution.Width = 192;
                client.SetVideoEncoderConfiguration(veConfig, true);

                veConfig.Encoding = MediaService.VideoEncoding.H264;
                veConfig.RateControl.FrameRateLimit = 11;
                veConfig.Resolution.Width = 176;
                client.SetVideoEncoderConfiguration(veConfig, true);
            }
        }

        /// <summary>
        /// Lấy tất cả các option có thể có.
        /// </summary>
        /// <param name="profileToken"></param>
        private static async void GetVideoEncoderConfigurationOptions(string profileToken)
        {
            using (MediaClient client = provider.CreateMediaClient())
            {
                var metaConfigs = client.GetMetadataConfigurations();
                foreach (var item in metaConfigs)
                {
                    var metaConfig = client.GetMetadataConfiguration(item.token);
                }

                var cveConfigs = client.GetCompatibleVideoEncoderConfigurations(profileToken);

                var vsConfigs = client.GetVideoSourceConfigurations();
                foreach (var item in vsConfigs)
                {
                    var vsConfig = client.GetVideoSourceConfiguration(item.token);
                }

                var veConfigs = client.GetVideoEncoderConfigurations();
                string veConfigNames = string.Join(", ", veConfigs.Select(c => string.Format("{0}", c.Name)));

                StringBuilder sb = new StringBuilder();

                // Nhìn kết quả thì thấy các VideoEncoderConfigurationOptions đều có QualityRange, ResolutionsAvailable, FrameRateRange, GovLengthRange như nhau
                foreach (var item in veConfigs)
                {
                    var cfg = client.GetVideoEncoderConfigurationOptions(null, null);
                    sb.AppendLine(item.token + ", " + item.Name);
                    sb.AppendLine(string.Format("QualityRange: {0}x{1}", cfg.QualityRange.Min, cfg.QualityRange.Max));

                    if (cfg.H264 != null)
                    {
                        sb.AppendLine("ResolutionsAvailable: " + string.Join(", ", cfg.H264.ResolutionsAvailable.Select(c => string.Format("{0}x{1}", c.Width, c.Height))));
                        sb.AppendLine(string.Format("FrameRateRange: {0}x{1}", cfg.H264.FrameRateRange.Min, cfg.H264.FrameRateRange.Max));
                        sb.AppendLine(string.Format("GovLengthRange: {0}x{1}", cfg.H264.GovLengthRange.Min, cfg.H264.GovLengthRange.Max));
                    }
                    if (cfg.JPEG != null)
                    {
                        sb.AppendLine(string.Join(", ", cfg.JPEG.ResolutionsAvailable.Select(c => string.Format("{0}x{1}", c.Width, c.Height))));
                        sb.AppendLine(string.Format("FrameRateRange: {0}x{1}", cfg.JPEG.FrameRateRange.Min, cfg.JPEG.FrameRateRange.Max));
                    }
                    if (cfg.MPEG4 != null)
                    {
                        sb.AppendLine(string.Join(", ", cfg.MPEG4.ResolutionsAvailable.Select(c => string.Format("{0}x{1}", c.Width, c.Height))));
                    }
                    sb.AppendLine();

                    if (cfg.Extension != null)
                    {
                        sb.AppendLine(string.Format("{0}x{1}", cfg.Extension.H264.GovLengthRange.Min, cfg.Extension.H264.GovLengthRange.Max));
                    }
                }
                var sbs = sb.ToString();
            }
        }

        private static async void MetadataStreaming()
        {
            using (MediaClient client = provider.CreateMediaClient())
            {
                var profiles = client.GetProfiles();
                var metaConfigs = client.GetMetadataConfigurations();

                // Get the token for the first configuration
                // and then configure for our needs
                var metadataConfiguration = metaConfigs[0];
                var metadatatoken = metadataConfiguration.token; // Attribute



            }
        }

        public static string GetSnapshotUri()
        {
            using (MediaClient client = provider.CreateMediaClient())
            {
                Profile[] profiles = client.GetProfiles();
                var profile = profiles[1];
                string profileToken = profile.token;
                var mediaUri = client.GetSnapshotUri(profileToken);
                return mediaUri.Uri;
            }
        }

        #endregion

        #region Event



        private static async Task PullMessage()
        {
            //Kiểm tra xem hỗ trợ các event nào
            GetEventPropertiesResponse eventProperties;
            using (var client = provider.CreateEventPortTypeClient())
            {
                try
                {
                    eventProperties = await client.GetEventPropertiesAsync(new EventService.GetEventPropertiesRequest());
                }
                catch { throw; }
            }

            //https://docs.oasis-open.org/wsn/wsn-ws_topics-1.3-spec-os.pdf
            //“tns:t1/t3//.” => This identifies the sub-tree consisting of tns:t1/t3 and all its descendents. 
            //filter expresstion: tns1:Device/tnsvendor:IO//.|tns1:Device/tnsvendor:Sensor/PIR
            //                    tns1:Device//.

            
            //Build danh sách event được hỗ trợ
            //List<string> lstAllEventSupported = GetEventTopicInfo(eventProperties);
            List<string> lstEvent = new List<string>();
            //foreach(var topic in lstAllEventSupported)
            //{
            //    //todo: Kiểm tra có support thì add vào list Event
            //}
            lstEvent.Add(string.Format("tns1:{0}//.", Constants.TopicRoots.Device));
            lstEvent.Add(string.Format("tns1:{0}//.", Constants.TopicRoots.RuleEngine));
            if (lstEvent.Count == 0)
                return;

            var f = new TopicExpressionFilter()
            {
                //Register vào mà không được hỗ trợ (lấy từ GetEventPropertiesAsync) => báo lỗi: TopicNotSupportedFault
                //Axis không chết nhưng Pelco thì đã chết => Chắc ăn thì nên kiểm tra
                dialect = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet",
                expression = string.Join("|", lstEvent),
                namespaces = new XmlSerializerNamespaces()
            };
            f.namespaces.Add("tns1", "http://www.onvif.org/ver10/topics");
            var fe = SerializeHelper.SerializeToXmlElement(f);

            var filter = new EventService.FilterType();
            filter.Any = new XmlElement[] { fe };


            //solution: https://stackoverflow.com/questions/28853553/onvif-pullpointsubscriptionclient-pullmessages
            //1. CreatePullPointSubscription
            TestOnvifService.EventService.CreatePullPointSubscriptionResponse pullpointResponse;
            //Đã chạy OK khi chuyển sang MessageVersion.Soap12WSAddressing10
            var pullpointRequest = new EventService.CreatePullPointSubscriptionRequest()
            {
                Filter = filter,
            };
            using (var client = provider.CreateEventPortTypeClient())
            {
                try
                {
                    pullpointResponse = await client.CreatePullPointSubscriptionAsync(pullpointRequest);
                }
                catch { throw; }
            }
            var oAux1 = pullpointResponse.SubscriptionReference;

            //Chuẩn bị dữ liệu từ CreatePullPointSubscriptionResponse
            // oAux1.Address.Value -> the proxy endpoint address
            // lstHeaders -> headers to add to the SOAP message of the proxy request

            //Address
            //http://10.16.0.137/onvif/services
            string pullpointAddress = oAux1.Address.Value;

            //Timeout
            TimeSpan minTimeout = TimeSpan.FromSeconds(5);
            TimeSpan? timeout = pullpointResponse.TerminationTime - pullpointResponse.CurrentTime;
            if (timeout == null || timeout > minTimeout)
                timeout = minTimeout;
            string strTimeout = System.Xml.XmlConvert.ToString(timeout.Value);

            //Header
            List<MessageHeader> lstHeaders = new List<MessageHeader>();
            if ((oAux1.ReferenceParameters != null) && (oAux1.ReferenceParameters.Any != null))
            {
                foreach (System.Xml.XmlElement oXml in oAux1.ReferenceParameters.Any)
                {
                    string strName = oXml.LocalName;
                    string strNS = oXml.NamespaceURI;
                    string strValue = oXml.InnerXml;

                    lstHeaders.Add(MessageHeader.CreateHeader(strName, strNS, strValue, true));
                }
            }

            try
            {
                //2. Pull message
                while (true)
                {
                    using (var client = provider.CreatePullPointSubscriptionClient(pullpointAddress))
                    {
                        using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)client.InnerChannel))
                        {
                            foreach (var header in lstHeaders)
                            {
                                OperationContext.Current.OutgoingMessageHeaders.Add(header);
                            }

                            //Nếu chạy async thì phải gọi async từ trên xuống, nếu không sẽ die vì client bị dispose rồi trong khi vẫn call service async
                            //var lstMessage = await client.PullMessagesAsync(new EventService.PullMessagesRequest("PT60S", 10, pullpointResponse.SubscriptionReference.Any));

                            // If no notifications are available, the response is delayed.
                            // The PullMessages request does not get a response until timeout or event arrives.
                            System.DateTime terminationTime;
                            EventService.NotificationMessageHolderType[] notificationMessage;
                            var currentTime = client.PullMessages(strTimeout, 10, null, out terminationTime, out notificationMessage);

                            ProcessNotificationMessage(notificationMessage, eventProperties);
                            //System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch { }
        }

        private static void WSBaseNotification()
        {
            //A subscription is set up, and the service handling the notification connects to the specified URL and POST the Notification message
            //The connection on which the Notification is sent is initiated by the producer, and the consumer does not need to be the same entity that sets up the subscription
            //=> Subscriber: Khi subscribe thì cung cấp URL để Producer gọi vào. Consumer không nhất thiết phải là Subscriber mà có thể trỏ sang URL bất kỳ

            //https://stackoverflow.com/questions/16563403/onvif-event-subscription-in-c-sharp
            //http://techqa.info/programming/question/16563403/onvif-event-subscription-in-c
            try
            {
                string consumerAddress = ServiceHosting.Start();

                //Đã chạy OK khi chuyển sang MessageVersion.Soap12WSAddressing10
                using (var client = provider.CreateNotificationProducerClient(null)) //"http://10.16.0.137/onvif/services"
                {
                    var response = client.Subscribe(new EventService.Subscribe()
                    {
                        InitialTerminationTime = "PT5S",
                        ConsumerReference = new EventService.EndpointReferenceType() { Address = new EventService.AttributedURIType() { Value = consumerAddress } },
                    });

                    //todo: Consumer chưa nhận được message sau khi subscribe

                    while (true)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    //Subscribe xong thì message sẽ tự động được đẩy về địa chỉ nhận consumerAddress
                }
            }
            catch { }
        }

        private static async Task ProcessEvent()
        {
            //Chưa biết dùng làm gì
            //finding out what notifications a device supports and what information they contain
            using (var client = provider.CreateEventPortTypeClient())
            {
                var res1 = await client.GetEventPropertiesAsync(new EventService.GetEventPropertiesRequest());
            }

            using (var client = provider.CreateEventPortTypeClient())
            {
                //todo: Riêng thằng này chỉ chạy với MessageVersion.Soap12
                //Trong khi mấy thăng còn lại chỉ chạy được với MessageVersion.Soap12WSAddressing10 / Soap12WSAddressingAugust2004 / Default
                var res2 = await client.GetServiceCapabilitiesAsync();
            }
        }

        private static async Task ProcessNotificationMessage(NotificationMessageHolderType[] theNotificationMessages, GetEventPropertiesResponse eventProperties)
        {
            await Task.Run(() =>
            {
                var now = System.DateTime.Now;
                while (true)
                {
                    if ((System.DateTime.Now - now).Seconds > 60)
                        break;
                }
                foreach (var item in theNotificationMessages)
                {
                    ProcessNotificationMessage(item, eventProperties);
                }
                Console.WriteLine("Event process done");
                //System.Threading.Thread.Sleep(60000);
            });
        }

        private static void ProcessNotificationMessage(NotificationMessageHolderType theNotificationMessage, GetEventPropertiesResponse eventProperties)
        {
            //https://www.onvif.org/specs/core/ONVIF-Core-Specification-v240.pdf
            //Page 111 - 9 Event handling
            //9.7 Topic Structure
            //Event handling is based on the OASIS WS-BaseNotification and WS-Topics specifications

            //The following logical entities participate in the notification pattern:
            //Client: implements the NotificationConsumer interface.
            //Event Service: implements the NotificationProducer interface.
            //Subscription Manager: implements the BaseSubscriptionManager interface.

            //[WS-Topics] 
            //“Web Services Topics 1.3”, OASIS Standard.  http://docs.oasis-open.org/wsn/wsn-ws_topics-1.3-spec-os.pdf 

            #region Example

            //<wsnt:NotificationMessage>
            //  <wsnt:Topic Dialect="http://docs.oasis-open.org/wsn/t-1/TopicExpression/Simple">tns1:VideoAnalytics/tnsaxis:MotionDetection</wsnt:Topic>
            //  <wsnt:ProducerReference>
            //    <wsa5:Address>uri://1ae83dc6-9a06-48fd-8d0d-eacb500f3694/ProducerReference</wsa5:Address>
            //  </wsnt:ProducerReference>
            //  <wsnt:Message>
            //    <tt:Message PropertyOperation="Changed" UtcTime="2017-06-08T07:51:20Z">
            //      <tt:Source>
            //        <tt:SimpleItem Value="0" Name="window"/>
            //      </tt:Source>
            //      <tt:Key/>
            //      <tt:Data>
            //        <tt:SimpleItem Value="0" Name="motion"/>
            //      </tt:Data>
            //    </tt:Message>
            //  </wsnt:Message>
            //</wsnt:NotificationMessage> 

            #endregion

            // Get the topic, the dialect and the producer
            var topic = theNotificationMessage.Topic;
            var topic_dialect = topic.Dialect; // http://docs.oasis-open.org/wsn/t-1/TopicExpression/Simple
            var producer = theNotificationMessage.ProducerReference.Address; //uri://1ae83dc6-9a06-48fd-8d0d-eacb500f3694/ProducerReference

            //tns1:Device/tnsaxis:IO/VirtualInput
            //tns1:VideoAnalytics/tnsaxis:MotionDetection
            var topicValue = topic.Any[0].Value;
            List<string> topics = ParseTopicValues(topicValue);
            var message = SerializeHelper.Deserialize<NotificationMessage>(theNotificationMessage.Message.OuterXml);

            // Process the event data (application specific)
            HandleNotification(topics, message);
            //App.handle_notification(topic, topic_dialect, producer, source_name, source_value, key_name, key_value, data_name, data_value); 
        }

        private static void HandleNotification(List<string> topics, NotificationMessage message)
        {
            string topicRoot = topics[0];
            List<string> innerTopics = topics.Skip(1).ToList();
            switch (topicRoot)
            {
                case Constants.TopicRoots.VideoAnalytics:
                    ProcessVideoAnalytics(innerTopics, message);
                    break;
                case Constants.TopicRoots.Device:

                    break;
                case Constants.TopicRoots.UserAlarm:

                    break;
                case Constants.TopicRoots.PTZController:

                    break;
            }
        }

        private static void ProcessVideoAnalytics(List<string> innerTopics, NotificationMessage message)
        {
            string topic = innerTopics[0];
            switch (topic)
            {
                case Constants.TopicVideoAnalytics.MotionDetection:
                    Console.WriteLine(string.Format("{0:HH:mm:ss ffff} - {1} - {2} - {3}",
                                        message.UtcTime, message.PropertyOperation, message.Data[0].Name, message.Data[0].Value));
                    break;
            }
        }

        private static List<string> ParseTopicValues(string topicValue)
        {
            //tns1:VideoAnalytics/tnsaxis:MotionDetection
            string[] topics = topicValue.Split('/');

            //Remove Namespace
            for (int i = 0; i < topics.Length; i++)
            {
                string topic = topics[i];
                int index = topic.IndexOf(':');
                if (index >= 0)
                    topics[i] = topic.Substring(index + 1);
            }

            return topics.ToList();
        }

        private static List<string> GetEventTopicInfo(GetEventPropertiesResponse eventProperties)
        {
            List<string> lstFunction = new List<string>();
            foreach (var topic in eventProperties.TopicSet.Any)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(topic.OwnerDocument.NameTable);
                nsmgr.AddNamespace(topic.Prefix, topic.NamespaceURI);
                nsmgr.AddNamespace("tnsaxis", "http://www.axis.com/2009/event/topics");

                topic.OwnerDocument.AppendChild(topic);

                var name1 = topic.OwnerDocument.SelectNodes("tns1:VideoAnalytics", nsmgr);
                var name2 = topic.OwnerDocument.SelectNodes("VideoAnalytics", nsmgr);
                var name3 = topic.OwnerDocument.SelectNodes("tns1:Device", nsmgr);
                var name4 = topic.OwnerDocument.SelectNodes("Device", nsmgr);
                var name5 = topic.OwnerDocument.SelectNodes("tns1:Device/tnsaxis:IO/VirtualInput", nsmgr);



                var topicName = topic.Name;
                foreach (XmlNode child in topic.ChildNodes)
                {
                    var childName = child.Name;
                    string name = string.Format("{0}/{1}", topicName, childName);
                    lstFunction.Add(name);
                }
            }
            return lstFunction;
        }

        #endregion
    }
}
