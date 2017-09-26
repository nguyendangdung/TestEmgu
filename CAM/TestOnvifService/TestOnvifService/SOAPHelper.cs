using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace TestOnvifService
{
    public static class SOAPHelper
    {
        /// <summary>
        /// Call web service by SOAP
        /// </summary>
        /// <param name="url">Uri of Web service</param>
        /// <param name="action">SOAPAction, refer to web service info</param>
        /// <param name="soapxml"></param>
        /// <returns>Original SOAP xml result</returns>
        public static string CallWebService(string url, string action, string soapxml)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(soapxml);
                HttpWebRequest webRequest = CreateWebRequest(url, action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                // get the response from the completed web request.
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        return soapResult;
                    }
                }
            }
            finally
            {
                Console.WriteLine("CallWebService:  [{0}]ms", watch.ElapsedMilliseconds);
            }
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string soapxml)
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(soapxml);
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        static void TestSOAPHelper()
        {
            string soapAction = "http://www.onvif.org/ver10/media/wsdl/GetProfiles";

            //Gọi OK
            var soapXml11 = @"
<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:u=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
  <s:Header>
    <o:Security s:mustUnderstand=""1"" xmlns:o=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
      <o:UsernameToken>
        <o:Username>giangnt</o:Username>
        <o:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest"">/7d73A8ef2Y6SrYMviCj/kTi/80=</o:Password>
        <o:Nonce EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"">MzBmNWNiNjMtNGVmNy00ZGRlLWI2ZGItOTRkOGJhOWFlZDk3</o:Nonce>
        <u:Created>2017-06-17T09:26:53.131Z</u:Created>
      </o:UsernameToken>
    </o:Security>
  </s:Header>
  <s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <GetProfiles xmlns=""http://www.onvif.org/ver10/media/wsdl""/>
  </s:Body>
</s:Envelope>
            ";

            string _serviceUrl = "http://10.16.0.137/onvif/services";

            string originalSoapResult = SOAPHelper.CallWebService(_serviceUrl, soapAction, soapXml11);
        }

        //public static void CallWebService()
        //{
        //    var _url = "http://localhost:4512/WebService/FlexECMService.asmx";
        //    var _action = "softmart.net.vn/DownloadDocument";

        //    XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
        //    HttpWebRequest webRequest = CreateWebRequest(_url, _action);
        //    InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

        //    // begin async call to web request.
        //    IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

        //    // suspend this thread until call is complete. You might want to
        //    // do something usefull here like update your UI.
        //    asyncResult.AsyncWaitHandle.WaitOne();

        //    // get the response from the completed web request.
        //    string soapResult;
        //    using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
        //    {
        //        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
        //        {
        //            soapResult = rd.ReadToEnd();
        //        }
        //        Console.Write(soapResult);
        //    }
        //}
    }
}
