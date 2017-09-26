using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TestOnvifService
{
    public class SerializeHelper
    {
        /// <summary>
        /// Serialize object to string
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="obj">Object data</param>
        /// <returns>XML string</returns>
        public static string SerializeToString<T>(T obj)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            StringWriter stringWriter = new StringWriter();

            XmlWriter xr = XmlWriter.Create(stringWriter);
            ser.Serialize(xr, obj);
            xr.Flush();
            return stringWriter.ToString();

            /*
            // Create String Writer
            StringWriter sw = new StringWriter();

            // Serialize object to string
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(sw, obj);
            string serializeString = sw.ToString();

            // Close StringWriter
            sw.Close();
            return serializeString;
            */
        }

        /// <summary>
        /// Deserialize string to object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="xml">XML string</param>
        /// <returns>Object with type is T</returns>
        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xml))
            {
                using (XmlTextReader xr = new XmlTextReader(reader))
                {
                    return (T)serializer.Deserialize(xr);
                }
            }
        }

        /// <summary>
        /// Deserialize data from file content to object.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="filePath"></param>
        /// <returns>Object with type is T</returns>
        public static T DeserializeXmlFile<T>(string filePath)
        {
            string fileContent = File.ReadAllText(filePath, Encoding.UTF8);
            return Deserialize<T>(fileContent);
        }

        /// <summary>
        /// Serialize data from object to file.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">Object data</param>
        /// <param name="filePath">Destination file path</param>
        public static void SerializeXmlFile<T>(T obj, string filePath)
        {
            XmlSerializer ser = new XmlSerializer(obj.GetType());
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(fs, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    ser.Serialize(xmlTextWriter, obj);
                    xmlTextWriter.Flush();
                }
            }
        }

        public static byte[] SerializeToByteArray<T>(T item)
        {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, item);

            //This gives you the byte array.
            return mStream.ToArray();
        }

        public static T Deserialize<T>(byte[] data)
        {
            var mStream = new MemoryStream();
            var binFormatter = new BinaryFormatter();

            // Where 'objectBytes' is your byte array.
            mStream.Write(data, 0, data.Length);
            mStream.Position = 0;

            var item = (T)binFormatter.Deserialize(mStream);
            return item;
        }

        public static XmlElement SerializeToXmlElement(object o)
        {
            XmlDocument doc = new XmlDocument();

            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                new XmlSerializer(o.GetType()).Serialize(writer, o);
            }

            return doc.DocumentElement;
        }
    }
}
