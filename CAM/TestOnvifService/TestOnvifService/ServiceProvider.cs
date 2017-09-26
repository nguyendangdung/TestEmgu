using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using TestOnvifService.DeviceMngtService;
using TestOnvifService.MediaService;
using TestOnvifService.PTZService;

namespace TestOnvifService
{
    public class ServiceProvider
    {
        public EndpointAddress DeviceServiceAddress
        {
            get { return new EndpointAddress(string.Format("{0}://{1}:{2}/onvif/device_service", _protocol, _ip, _port)); }
        }

        private Service _MediaService;
        public EndpointAddress MediaServiceAddress
        {
            get { return new EndpointAddress(_MediaService.XAddr); }
        }

        private Service _PTZService;
        public EndpointAddress PTZServiceAddress
        {
            get { return new EndpointAddress(_PTZService.XAddr); }
        }

        private Service _EventService;
        public EndpointAddress EventServiceAddress
        {
            get { return new EndpointAddress(_EventService.XAddr); }
        }

        public CustomBinding NonAuthCustomBinding
        {
            get
            {
                //Nếu lấy từ file App config:
                return new CustomBinding("DeviceBinding");

                //var messageElement = new TextMessageEncodingBindingElement();
                //messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
                //HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
                //return new CustomBinding(messageElement, httpBinding);
            }
        }

        private CustomBinding GetSecurityCustomBinding(string configurationName)
        {
            var security = TransportSecurityBindingElement.CreateUserNameOverTransportBindingElement();
            security.AllowInsecureTransport = true;
            security.IncludeTimestamp = false;
            security.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
            security.MessageSecurityVersion = MessageSecurityVersion.Default;

            var binding = new CustomBinding(configurationName);
            binding.Elements.Insert(0, security);
            return binding;
        }

        private string _ip;
        private string _userName;
        private string _password;
        private NetworkProtocolType _protocol;
        private int _port;

        /// <summary>
        ///Thời gian của Camera, Giá trị này được lấy từ Camera qua method GetSystemDateAndTime()
        ///=> Lưu lại để tạo request (đồng bộ) với Camera mỗi khi gọi, prevent REPLAY ATTACK
        ///Hầu hết camera sẽ là Require, 1 số camera không cần vì đã disable chức năng này (Enable replay attack protection = FALSE) nhưng tốt nhất nên có
        /// </summary>
        private System.DateTime? _CameraDTG { get; set; }
        /// <summary>
        /// Thời gian của Client tại thời điểm gọi GetSystemDateAndTime()
        /// </summary>
        private System.DateTime _SyncDTG { get; set; }

        public ServiceProvider(NetworkProtocolType protocol, string ip, int port, string userName, string password)
        {
            this._protocol = protocol;
            this._ip = ip;
            this._port = port;
            this._userName = userName;
            this._password = password;
        }

        /// <summary>
        /// Lưu thời gian của Camera cho các lần gọi sau
        /// </summary>
        /// <param name="cameraDTG">UTCDateTime</param>
        public void SyncTime(DeviceMngtService.DateTime cameraDTG)
        {
            _CameraDTG = new System.DateTime(cameraDTG.Date.Year, cameraDTG.Date.Month, cameraDTG.Date.Day, cameraDTG.Time.Hour, cameraDTG.Time.Minute, cameraDTG.Time.Second);
            _SyncDTG = System.DateTime.Now;
        }

        /// <summary>
        /// Lưu thông tin (Địa chỉ,...) các service mà camera hỗ trợ
        /// </summary>
        /// <param name="services"></param>
        public void StoreServices(Service[] services)
        {
            //var deviceService = services.FirstOrDefault(c => c.Namespace == @"http://www.onvif.org/ver10/device/wsdl");
            _MediaService = services.FirstOrDefault(c => c.Namespace == @"http://www.onvif.org/ver10/media/wsdl");
            _PTZService = services.FirstOrDefault(c => c.Namespace == @"http://www.onvif.org/ver20/ptz/wsdl");
            _EventService = services.FirstOrDefault(c => c.Namespace == @"http://www.onvif.org/ver10/events/wsdl");
            //var analyticsService = services.FirstOrDefault(c => c.Namespace == @"http://www.onvif.org/ver20/analytics/wsdl");
        }

        public PTZClient CreatePTZClient()
        {
            var binding = GetSecurityCustomBinding("PTZBinding");
            var client = new PTZClient(binding, PTZServiceAddress);
            SetCredential(client);
            return client;
        }

        public DeviceClient CreateDeviceClient()
        {
            var binding = GetSecurityCustomBinding("DeviceBinding");
            var client = new DeviceClient(binding, DeviceServiceAddress);
            SetCredential(client);
            return client;
        }

        public MediaClient CreateMediaClient()
        {
            var binding = GetSecurityCustomBinding("MediaBinding");
            var client = new MediaClient(binding, MediaServiceAddress);
            SetCredential(client);
            return client;
        }

        public EventService.PullPointClient CreatePullPointClient()
        {
            var binding = GetSecurityCustomBinding("EventBinding");
            var client = new EventService.PullPointClient(binding, EventServiceAddress);
            SetCredential(client);
            return client;
        }

        public EventService.EventPortTypeClient CreateEventPortTypeClient()
        {
            var binding = GetSecurityCustomBinding("EventBinding");
            var client = new EventService.EventPortTypeClient(binding, EventServiceAddress);
            SetCredential(client);
            return client;
        }

        public EventService.NotificationConsumerClient CreateNotificationConsumerClient()
        {
            var binding = GetSecurityCustomBinding("EventBinding");
            var client = new EventService.NotificationConsumerClient(binding, EventServiceAddress);
            SetCredential(client);
            return client;
        }

        public EventService.PullPointSubscriptionClient CreatePullPointSubscriptionClient(string address)
        {
            var add = address == null ? EventServiceAddress : new EndpointAddress(address);
            var binding = GetSecurityCustomBinding("EventBinding");
            var client = new EventService.PullPointSubscriptionClient(binding, add);
            SetCredential(client);
            return client;
        }

        public EventService.NotificationProducerClient CreateNotificationProducerClient(string address)
        {
            var add = address == null ? EventServiceAddress : new EndpointAddress(address);
            var binding = GetSecurityCustomBinding("EventBinding");
            var client = new EventService.NotificationProducerClient(binding, add);
            SetCredential(client);
            return client;
        }

        private void SetCredential<T>(ClientBase<T> client) where T : class
        {
            var x = client.ChannelFactory.Endpoint.Behaviors.Remove<System.ServiceModel.Description.ClientCredentials>();

            //Tính thời gian của server dựa trên CameraDTG SyncDTG và Now
            //Thời gian ước tính hiện tại của Camera, được tính dựa trên CameraDTG, SyncDTG và Now
            System.DateTime clientDTG = _CameraDTG == null ? System.DateTime.Now : (_CameraDTG.Value.Add(System.DateTime.Now - _SyncDTG));
            client.ChannelFactory.Endpoint.Behaviors.Add(new CustomCredentials(clientDTG));

            client.ClientCredentials.UserName.UserName = _userName;
            client.ClientCredentials.UserName.Password = _password;
        }

        public enum NetworkProtocolType
        {
            HTTP,
            HTTPS,
            RTSP,
        }

        //public static MediaClient CreateMediaClient(string address, string userName, string password)
        //{
        //    var client = new MediaClient();
        //    ConfigClient(client, address, userName, password);
        //    return client;
        //}

        //private static ClientBase<T> ConfigClient<T>(ClientBase<T> client, string address, string userName, string password) where T : class
        //{
        //    var messageElement = new TextMessageEncodingBindingElement();
        //    messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
        //    HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
        //    httpBinding.AuthenticationScheme = AuthenticationSchemes.Basic;
        //    EndpointAddress mediaAddress = new EndpointAddress(address);

        //    client.Endpoint.Address = new EndpointAddress(address);
        //    client.Endpoint.Binding = new CustomBinding(messageElement, httpBinding);

        //    client.ClientCredentials.UserName.UserName = userName;
        //    client.ClientCredentials.UserName.Password = password;
        //    return client;
        //}

        //CustomBinding _customBinding
        //{
        //    get
        //    {
        //        var binding = new CustomBinding();
        //        var security = TransportSecurityBindingElement.CreateUserNameOverTransportBindingElement();
        //        security.AllowInsecureTransport = true;
        //        security.IncludeTimestamp = false;
        //        security.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256;
        //        security.MessageSecurityVersion = MessageSecurityVersion.Default;

        //        var encoding = new TextMessageEncodingBindingElement();
        //        encoding.MessageVersion = MessageVersion.Soap12;

        //        var transport = new HttpTransportBindingElement();
        //        transport.MaxReceivedMessageSize = 20000000; // 20 megs

        //        binding.Elements.Add(security);
        //        binding.Elements.Add(encoding);
        //        binding.Elements.Add(transport);
        //        return binding;
        //    }
        //}
    }
}
