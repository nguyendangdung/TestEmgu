using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestOnvifService
{
    /// <summary>
    /// tt:Message may contain Source, Key, Data of type ItemList and Extension of anyType.
    /// ItemList (Source, Key and Data) is recommended to contain 
    /// </summary>
    [XmlRoot("Message", Namespace = "http://www.onvif.org/ver10/schema")]
    public class NotificationMessage
    {
        [XmlAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
        public DateTime UtcTime { get; set; }

        [XmlAttribute(Namespace = "http://www.onvif.org/ver10/schema")]
        public PropertyOperation PropertyOperation { get; set; }

        [XmlArray("Source", Namespace = "http://www.onvif.org/ver10/schema")]
        [XmlArrayItem("SimpleItem", Namespace = "http://www.onvif.org/ver10/schema")]
        public List<SimpleItem> Source { get; set; }

        [XmlArray("Key", Namespace = "http://www.onvif.org/ver10/schema")]
        [XmlArrayItem("SimpleItem", Namespace = "http://www.onvif.org/ver10/schema")]
        public List<SimpleItem> Key { get; set; }

        [XmlArray("Data", Namespace = "http://www.onvif.org/ver10/schema")]
        [XmlArrayItem("SimpleItem", Namespace = "http://www.onvif.org/ver10/schema")]
        public List<SimpleItem> Data { get; set; }
    }

    /// <summary>
    /// tt:SimpleItem elements with Name and Value attributes
    /// but it could be the more complex ElementItem as well. 
    /// </summary>
    public class SimpleItem
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Value;
    }

    /// <summary>
    /// The optional PropertyOperation attribute tells if the notification
    /// is due to that something has changed or just to inform about the state.
    /// Valid values are: Initialized, Deleted and Changed. 
    /// </summary>
    public enum PropertyOperation
    {
        Initialized,
        Deleted,
        Changed
    }

    [XmlRoot(ElementName = "TopicExpression", Namespace = "http://docs.oasis-open.org/wsn/b-2")]
    public class TopicExpressionFilter
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces namespaces;
        [XmlText]
        public string expression;
        [XmlAttribute(AttributeName = "Dialect", DataType = "anyURI")]
        public string dialect;
    }

    public class Constants
    {
        public class DefaultDialects
        {
            static private readonly string dialect1 = "http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete";
            static private readonly string dialect2 = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";
            static private readonly string dialect3 = "http://www.onvif.org/ver10/tev/messageContentFilter/ItemFilter";
        }

        /// <summary>
        /// The following root topics are defined in the ONVIF Namespace
        /// </summary>
        /// <seealso cref="https://www.onvif.org/specs/core/ONVIF-Core-Specification-v250.pdf"/>
        public class TopicRoots
        {
            public const string Device = "Device";
            public const string VideoSource = "VideoSource";
            public const string VideoEncoder = "VideoEncoder";
            public const string VideoAnalytics = "VideoAnalytics";
            public const string RuleEngine = "RuleEngine";
            public const string PTZController = "PTZController";
            public const string AudioSource = "AudioSource";
            public const string AudioEncoder = "AudioEncoder";
            public const string UserAlarm = "UserAlarm";
            public const string MediaControl = "MediaControl";
            public const string RecordingConfig = "RecordingConfig";
            public const string RecordingHistory = "RecordingHistory";
            public const string VideoOutput = "VideoOutput";
            public const string AudioOutput = "AudioOutput";
            public const string VideoDecoder = "VideoDecoder";
            public const string AudioDecoder = "AudioDecoder";
            public const string Receiver = "Receiver";
            public const string Monitoring = "Monitoring";
        }

        public class TopicVideoAnalytics
        {
            public const string MotionDetection = "MotionDetection";
        }
    }
}
