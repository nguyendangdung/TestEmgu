//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Security.Cryptography;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
//using System.ServiceModel.Description;
//using System.ServiceModel.Security;
//using System.Text;
//using System.Threading.Tasks;
//using TestOnvifService.DeviceMngtService;
//using TestOnvifService.MediaService;
//using TestOnvifService.PTZService;

//namespace TestOnvifService
//{
//    class Program
//    {
//        //static ServiceProvider provider = new ServiceProvider("http://192.168.2.21:10000/onvif/Media", "admin", "admin1");
//        static ServiceProvider provider = new ServiceProvider("http://192.168.2.16/onvif/device_service", "giangnt", "123456");

//        public static string GetSHA1String(byte[] input)
//        {
//            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
//            byte[] hashedDataBytes = sha1Hasher.ComputeHash(input);
//            return Convert.ToBase64String(hashedDataBytes);
//        }

//        public static string GetSHA1String(string phrase)
//        {
//            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
//            byte[] hashedDataBytes = sha1Hasher.ComputeHash(Encoding.UTF8.GetBytes(phrase));
//            return Convert.ToBase64String(hashedDataBytes);
//        }

//        static void Main(string[] args)
//        {
//            byte[] date = Encoding.UTF8.GetBytes("2017-05-18T11:51:01Z");
//            byte[] password = Encoding.UTF8.GetBytes("123456");
//            byte[] nonce = Convert.FromBase64String("MzFiN2VhMjctZjUxYS00MGNlLWIwNGUtMTkzNzZkNzc4MDQ3");

//            List<byte> lstB = new List<byte>();
//            lstB.AddRange(nonce);
//            lstB.AddRange(date);
//            lstB.AddRange(password);
//            string ss = Encoding.UTF8.GetString(lstB.ToArray());
//            var s = GetSHA1String(lstB.ToArray());
//            //YBCPSgpGYaaItaZAa5HaytT3CRM=
//            //tuOSpGlFlIXsozq4HFNeeGeFLEI=

//            try
//            {
//                Process();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//            }
//            Console.WriteLine("DONE");
//            Console.ReadLine();
//        }

//        private static async void Process()
//        {
//            //NonAuthen();
//            //Test();
//            //ProcessDevice();
//            //ProcessMedia();
//            ProcessPTZ("profile_1_h264");
//        }

//        private static void NonAuthen()
//        {
//            var messageElement = new TextMessageEncodingBindingElement();
//            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
//            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
//            var _customBinding = new CustomBinding(messageElement, httpBinding);
//            EndpointAddress _mediaAddress = new EndpointAddress("http://192.168.2.16/onvif/device_service");

//            var clientMedia = new DeviceClient(_customBinding, _mediaAddress);
//            var dt = clientMedia.GetSystemDateAndTime();
//            var services = clientMedia.GetServices(true);

//        }

//        //private static void Test()
//        //{
//        //    var binding = new CustomBinding();
//        //    var security = TransportSecurityBindingElement.CreateUserNameOverTransportBindingElement();
//        //    security.AllowInsecureTransport = true;
//        //    security.IncludeTimestamp = false;
//        //    security.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
//        //    security.MessageSecurityVersion = MessageSecurityVersion.Default;

//        //    var encoding = new TextMessageEncodingBindingElement();
//        //    encoding.MessageVersion = MessageVersion.Soap12;

//        //    var transport = new HttpTransportBindingElement();
//        //    transport.MaxReceivedMessageSize = 20000000; // 20 megs

//        //    binding.Elements.Add(security);
//        //    binding.Elements.Add(encoding);
//        //    binding.Elements.Add(transport);

//        //    EndpointAddress _mediaAddress = new EndpointAddress("http://192.168.2.16/onvif/device_service");

//        //    // create the device object to use the service 
//        //    var clientMedia = new DeviceClient(binding, _mediaAddress);

//        //    var x = clientMedia.ChannelFactory.Endpoint.Behaviors.Remove<System.ServiceModel.Description.ClientCredentials>();
//        //    clientMedia.ChannelFactory.Endpoint.Behaviors.Add(new CustomCredentials());

//        //    clientMedia.ClientCredentials.UserName.UserName = "giangnt";
//        //    clientMedia.ClientCredentials.UserName.Password = "123456";

//        //    // The acquisition of the device time 
//        //    var dt = clientMedia.GetSystemDateAndTime();

//        //    // The acquisition of the user info that is registered in a device
//        //    var services = clientMedia.GetServices(true);
//        //    var userInfos = clientMedia.GetUsers();

//        //    string Model;
//        //    string FirmwareVersion;
//        //    string SerialNumber;
//        //    string HardwareId;
//        //    string manufacture = clientMedia.GetDeviceInformation(out Model, out FirmwareVersion, out SerialNumber, out HardwareId);
//        //}

//        //private static void Test()
//        //{
//        //    // create the device object to use the service
//        //    //deviceService = getDeviceService(MyDeviceServiceAddress)
//        //    var messageElement = new TextMessageEncodingBindingElement();
//        //    messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
//        //    HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
//        //    httpBinding.AuthenticationScheme = AuthenticationSchemes.Basic;
//        //    var _customBinding = new CustomBinding(messageElement, httpBinding);
//        //    EndpointAddress _mediaAddress = new EndpointAddress("http://192.168.2.16/onvif/device_service");

//        //    // create the device object to use the service 
//        //    var deviceService = new DeviceClient(_customBinding, _mediaAddress);


//        //    // The user name of a certified user is set
//        //    var wsUsernameToken = new WSUsernameToken();
//        //    wsUsernameToken.Username = "username";
//        //    // A random number value generated with a clientMedia uniquely is set
//        //    //nonceBinaryData = getNonce();
//        //    //nonceBase64 = base64(nonceBinaryData);
//        //    wsUsernameToken.Nonce = "nonceBase64";
//        //    // The time at the time of the request making is set
//        //    //utctimeData = getUTCTime();
//        //    //utctimeStringData = uTCTime2DatetimeString(utctimeData);
//        //    //utctimeBinaryData = string2Binary(utctimeStringData);
//        //    wsUsernameToken.Created = "utctimeStringData";
//        //    // The password digest of a certified user is set
//        //    //password = "userpassword";
//        //    //passwordBinaryData = string2Binary(password);
//        //    //passwordDigest = SHA1(nonceBinaryData + utctimeBinaryData + passwordBinaryData);
//        //    //passwordDigestBase64 = base64(passwordDigest);
//        //    wsUsernameToken.Password = "passwordDigestBase64";
//        //    wsUsernameToken.PasswordType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest";
//        //    // The clientMedia sends the SetHostname request to the device with WS-UsernameToken
//        //    //hostname = “camera1”;
//        //    deviceService.SoapHeader.WsUsernameToken = wsUsernameToken;
//        //    deviceService.ClientCredentials.UserName.
//        //    deviceService.SetHostname("hostname");
//        //}

//        //private static async void ProcessDevice()
//        //{
//        //    //var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
//        //    //basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

//        //    var messageElement = new TextMessageEncodingBindingElement();
//        //    messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
//        //    HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
//        //    //httpBinding.TransferMode = TransferMode.StreamedRequest;
//        //    //httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;
//        //    //httpBinding.AuthenticationScheme = AuthenticationSchemes.Digest;
//        //    //httpBinding.Realm = "AXIS_WS_ACCC8E75790A";
//        //    var _customBinding = new CustomBinding(messageElement, httpBinding);
//        //    EndpointAddress _mediaAddress = new EndpointAddress("http://192.168.2.16/onvif/device_service");

//        //    // create the device object to use the service 
//        //    var clientMedia = new DeviceClient(_customBinding, _mediaAddress);
//        //    clientMedia.ChannelFactory.Endpoint.Behaviors.Remove<System.ServiceModel.Description.ClientCredentials>();
//        //    clientMedia.ChannelFactory.Endpoint.Behaviors.Add(new CustomCredentials());

//        //    clientMedia.ClientCredentials.UserName.UserName = "giangnt";
//        //    clientMedia.ClientCredentials.UserName.Password = "123456";

//        //    // The acquisition of the device time 
//        //    var dt = clientMedia.GetSystemDateAndTime();

//        //    // The acquisition of the user info that is registered in a device
//        //    var services = clientMedia.GetServices(true);
//        //    var userInfos = clientMedia.GetUsers();







//        //    string Model;
//        //    string FirmwareVersion;
//        //    string SerialNumber;
//        //    string HardwareId;
//        //    string manufacture = clientMedia.GetDeviceInformation(out Model, out FirmwareVersion, out SerialNumber, out HardwareId);
//        //}


//        private static async void ProcessDevice()
//        {
//            //Device
//            using (var clientMedia = provider.CreateDeviceClient())
//            {
//                //Set Static Address

//                //SetStaticIP("admin", "admin", "http://192.168.2.21:10000", "192.168.2.200", 1);

//                string Model;
//                string FirmwareVersion;
//                string SerialNumber;
//                string HardwareId;
//                string manufacture = clientMedia.GetDeviceInformation(out Model, out FirmwareVersion, out SerialNumber, out HardwareId);
//                var response = await clientMedia.GetDeviceInformationAsync(new GetDeviceInformationRequest());
//            }
//        }

//        public static async void SetStaticIP(string userName, string password, string currentAddress, string newAddress, int timeout)
//        {
//            ServiceProvider provider = new ServiceProvider(currentAddress, userName, password);
//            using (var clientMedia = provider.CreateDeviceClient())
//            {
//                //create the parameter object of the SetNetworkInterfaces
//                var address = new TestOnvifService.DeviceMngtService.PrefixedIPv4Address()
//                {
//                    Address = newAddress, //ip address
//                    PrefixLength = 24, //netmask
//                };
//                string interfaceToken = "eth0"; //select or set the interface token
//                NetworkInterfaceSetConfiguration netCfg = new NetworkInterfaceSetConfiguration();
//                netCfg.Enabled = true; //enable this new configuration
//                netCfg.IPv4 = new IPv4NetworkInterfaceSetConfiguration();
//                netCfg.IPv4.Enabled = true; //enable it to be IPv4
//                netCfg.IPv4.Manual = new TestOnvifService.DeviceMngtService.PrefixedIPv4Address[] { address };
//                netCfg.IPv4.DHCP = false; // static configuration

//                await clientMedia.SetNetworkInterfacesAsync(interfaceToken, netCfg);
//            }
//        }

//        private static async void ProcessMedia()
//        {
//            //Media
//            using (MediaClient clientMedia = provider.CreateMediaClient())
//            {
//                Profile[] profiles = clientMedia.GetProfiles();
//                string profileToken = profiles[0].token;
//                var mediaUri = clientMedia.GetSnapshotUri(profileToken);

//                var profile = clientMedia.GetProfile(profileToken);

//                // setup stream configuration
//                var streamSetup = new StreamSetup();
//                streamSetup.Stream = StreamType.RTPUnicast;
//                streamSetup.Transport = new Transport();
//                streamSetup.Transport.Protocol = TransportProtocol.UDP;
//                // RTP/RTSP/UDP is not a special tunnelling setup (is not requiring)!
//                streamSetup.Transport.Tunnel = null;
//                // get stream URI
//                var streamUri = clientMedia.GetStreamUri(streamSetup, profileToken);
//            }
//        }

//        private static async void ProcessPTZ(string profileToken)
//        {
//            //Chú ý: Các giá trị x/y nhận giá trị float từ (-1, 1)

//            using (var clientMedia = provider.CreatePTZClient())
//            {
//                //PanTilz
//                var translation = new PTZVector() { PanTilt = new PTZService.Vector2D() { x = -01f, y = -0.1f }, };
//                var speed = new TestOnvifService.PTZService.PTZSpeed() { PanTilt = new PTZService.Vector2D() { x = 0.5f, y = 0.5f }, };
//                clientMedia.RelativeMove(profileToken, translation, speed);

//                //Zoom
//                //var ztranslation = new PTZVector() { Zoom = new PTZService.Vector1D() { x = 0.01f, space = null } };
//                //var zspeed = new TestOnvifService.PTZService.PTZSpeed() { Zoom = new PTZService.Vector1D() { x = 1, space = null } };
//                //clientMedia.RelativeMove(profileToken, ztranslation, zspeed);
//            }
//        }

//        private static async void ProcessEvent()
//        {
//            //Device
//            using (var clientMedia = provider.CreateDeviceClient())
//            {
//                string Model;
//                string FirmwareVersion;
//                string SerialNumber;
//                string HardwareId;
//                string manufacture = clientMedia.GetDeviceInformation(out Model, out FirmwareVersion, out SerialNumber, out HardwareId);
//                var response = await clientMedia.GetDeviceInformationAsync(new GetDeviceInformationRequest());
//            }
//        }
//    }

//    public class ServiceProvider
//    {
//        EndpointAddress _mediaAddress
//        {
//            get { return new EndpointAddress(_address); }
//        }
//        CustomBinding _customBinding
//        {
//            get
//            {
//                var binding = new CustomBinding();
//                var security = TransportSecurityBindingElement.CreateUserNameOverTransportBindingElement();
//                security.AllowInsecureTransport = true;
//                security.IncludeTimestamp = false;
//                security.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
//                security.MessageSecurityVersion = MessageSecurityVersion.Default;

//                var encoding = new TextMessageEncodingBindingElement();
//                encoding.MessageVersion = MessageVersion.Soap12;

//                var transport = new HttpTransportBindingElement();
//                transport.MaxReceivedMessageSize = 20000000; // 20 megs

//                binding.Elements.Add(security);
//                binding.Elements.Add(encoding);
//                binding.Elements.Add(transport);
//                return binding;
//            }
//        }

//        private string _address;
//        private string _userName;
//        private string _password;

//        public ServiceProvider(string address, string userName, string password)
//        {
//            this._address = address;
//            this._userName = userName;
//            this._password = password;
//        }

//        public PTZClient CreatePTZClient()
//        {
//            var clientMedia = new PTZClient(_customBinding, _mediaAddress);
//            SetCredential(clientMedia);
//            return clientMedia;
//        }

//        public DeviceClient CreateDeviceClient()
//        {
//            var clientMedia = new DeviceClient(_customBinding, _mediaAddress);
//            SetCredential(clientMedia);
//            return clientMedia;
//        }

//        public MediaClient CreateMediaClient()
//        {
//            var clientMedia = new MediaClient(_customBinding, _mediaAddress);
//            SetCredential(clientMedia);
//            return clientMedia;
//        }

//        private void SetCredential<T>(ClientBase<T> clientMedia) where T : class
//        {
//            var x = clientMedia.ChannelFactory.Endpoint.Behaviors.Remove<System.ServiceModel.Description.ClientCredentials>();
//            clientMedia.ChannelFactory.Endpoint.Behaviors.Add(new CustomCredentials());

//            clientMedia.ClientCredentials.UserName.UserName = _userName;
//            clientMedia.ClientCredentials.UserName.Password = _password;
//        }

//        //public static MediaClient CreateMediaClient(string address, string userName, string password)
//        //{
//        //    var clientMedia = new MediaClient();
//        //    ConfigClient(clientMedia, address, userName, password);
//        //    return clientMedia;
//        //}

//        //private static ClientBase<T> ConfigClient<T>(ClientBase<T> clientMedia, string address, string userName, string password) where T : class
//        //{
//        //    var messageElement = new TextMessageEncodingBindingElement();
//        //    messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
//        //    HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
//        //    httpBinding.AuthenticationScheme = AuthenticationSchemes.Basic;
//        //    EndpointAddress mediaAddress = new EndpointAddress(address);

//        //    clientMedia.Endpoint.Address = new EndpointAddress(address);
//        //    clientMedia.Endpoint.Binding = new CustomBinding(messageElement, httpBinding);

//        //    clientMedia.ClientCredentials.UserName.UserName = userName;
//        //    clientMedia.ClientCredentials.UserName.Password = password;
//        //    return clientMedia;
//        //}
//    }
//}
