using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TestOnvifService.EventService;

namespace TestOnvifService
{
    /// <summary>
    /// The client reciever service for WS-BaseNotification
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class NotificationConsumerService : NotificationConsumer
    {
        public event EventHandler<EventArgs<Notify1>> NewNotification;

        /// <summary>
        /// Notifies the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <remarks>A </remarks>
        public void Notify(Notify1 request)
        {
            var threadSafeEventHandler = NewNotification;
            if (threadSafeEventHandler != null)
                threadSafeEventHandler.Invoke(this, new EventArgs<Notify1>(request));
        }

        public Task NotifyAsync(Notify1 request)
        {
            throw new NotImplementedException();
        }
    }

    public class EventArgs<T> : EventArgs
    {

        public EventArgs(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }

    public class ServiceHosting
    {
        public static string Start()
        {
            //Prepare NotificationConsumer service 
            var consumer = new NotificationConsumerService();
            consumer.NewNotification += NotificationConsumerService_OnNewNotification;
            var serviceHost = new ServiceHost(consumer);
            serviceHost.Open();
            string consumerAddress = serviceHost.BaseAddresses.First().AbsoluteUri;
            return consumerAddress;
        }

        static void NotificationConsumerService_OnNewNotification(object sender, EventArgs<Notify1> e)
        {
            Console.WriteLine(e.Data.Notify.NotificationMessage);
        }
    }
}
