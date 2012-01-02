using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml;
using System.IO.Compression;
using Utils;

namespace Utils
{
    public class HeaderClient : WebClient
    {
        public bool HeadOnly
        {
            get;
            set;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            req.Timeout = 25000;
            if (HeadOnly && req.Method == "GET")
            {
                req.Method = "HEAD";
            }
            return req;
        }
    }

    public class FWebClient : WebClient
    {
        private CookieContainer f_CookieContainer = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest) {
                (request as HttpWebRequest).CookieContainer = f_CookieContainer;
            }
            return request;
        }
    }
    public delegate void UploadProgressDelegate(long pos, long total);
    public class NWebClient : WebClient, IDisposable, ICertificatePolicy
    {
        private int                         f_Timeout;
        private WebProxy            		f_WebProxy;
        private HttpWebRequest  			f_HttpWebRequest;
        private HttpWebResponse 			f_HttpWebResponse;
        private string                      f_Referer;
        private string                      f_UserAgent;
        private bool                        f_AutoReferer;
        private string                      f_LastUrl = String.Empty;
        private string f_CurrentURL = String.Empty;
        private CookieContainer             f_CookieContainer;
        public string ResponseUri =         String.Empty;
        public string Error       =         String.Empty;
        public bool autoredir = true;
        public bool cookiepathfix = false;
        public bool expect100continue = true;
        public bool keepalive = false;
       
        public event UploadProgressDelegate UploadProgress;
        private List<string> f_headerFilter = new List<string>();

        public List<string> headerFilter
        {
            get
            {
                return f_headerFilter;
            }
            set
            {
                f_headerFilter = value;
            }
        }


        public string f_lasturl
        {
            get
            {
                return f_LastUrl;
            }
            set
            {
                f_LastUrl = value;
            }
        }

        public string f_currenturl
        {
            get
            {
                return f_CurrentURL;
            }
            set
            {
                f_CurrentURL = value;
            }
        }

        
        public CookieContainer CookieContainer {
            get {
                return f_CookieContainer;
            }
            set {
                f_CookieContainer = value;
            }
        }

        public string Referer {
            get {
                return f_Referer;
            }
            set {
                f_Referer = value;
            }
        }

        public int TimeOut
        {
            get {
                return f_Timeout;
            }
            set {
                f_Timeout = value;
            }
        }

        public string UserAgent
        {
            get {
                return f_UserAgent;
            }
            set {
                f_UserAgent = value;
            }
        }

        public string ContentType = "application/x-www-form-urlencoded";

        public static Random random = new Random();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest) {
                (request as HttpWebRequest).CookieContainer = f_CookieContainer;
            }
            return request;
        }

        public string DownloadString(string address)
        {
            f_lasturl = f_currenturl;
            f_currenturl = address;

            f_lasturl = f_currenturl;
            f_currenturl = address;
			ServicePointManager.CertificatePolicy = new NWebClient (10000);	
            try {
                if (!String.IsNullOrEmpty(f_Referer)) {
                    base.Headers.Add(HttpRequestHeader.Referer, f_Referer);
                }

                if (f_WebProxy != null) {
                    base.Proxy = f_WebProxy;
                }

                if (!String.IsNullOrEmpty(f_UserAgent)) {
                    base.Headers.Add(HttpRequestHeader.UserAgent, f_UserAgent);
                }

                if (!keepalive) {
                    base.Headers.Add(HttpRequestHeader.KeepAlive,"false");
                } else {
                    base.Headers.Add(HttpRequestHeader.KeepAlive, "true");
                }

                return base.DownloadString(address);
            } catch (WebException e) {
                Error = e.Message;
                using (WebResponse response = e.Response) {
                    if (response != null) {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        Stream responseStream = httpResponse.GetResponseStream();
                        if (httpResponse.ContentEncoding.ToLower().Contains("gzip")) {
                            responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                        } else if (httpResponse.ContentEncoding.ToLower().Contains("deflate")) {
                            responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                        }
                        string text = String.Empty;
                        using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8)) {
                            text = reader.ReadToEnd();
                            if (String.IsNullOrEmpty(text)) {
                                return String.Empty;
                            }
                            return text;
                        }
                    }
                }
            } catch (Exception e) {
                // Console.WriteLine(e.Message);
            }
            return String.Empty;
        }

        public NWebClient(int timeout) : this(timeout, false) {
            System.Net.ServicePointManager.MaxServicePointIdleTime = 20000;
        }


        public NWebClient(int timeout,bool is_licensecheck,bool yes) : this(timeout, false) {
        }	
		
        public NWebClient(int timeout, bool autoReferer)
        {
            f_AutoReferer = autoReferer;
            f_Timeout = timeout;
            f_CookieContainer = new CookieContainer();
            System.Net.ServicePointManager.MaxServicePointIdleTime = 20000;
        }

        public NWebClient(string proxyAddress, int proxyPort, int timeout) {
            try {
                f_WebProxy = new WebProxy(proxyAddress, proxyPort);
                f_Timeout = timeout;
                f_CookieContainer = new CookieContainer();
                //System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.MaxServicePointIdleTime = 20000;
                //System.Net.ServicePointManager.UseNagleAlgorithm = true;
                //System.Net.ServicePointManager.DefaultConnectionLimit = 100;
            } catch {
            }
        }
		
		
		public bool CheckValidationResult (ServicePoint srvPoint, System.Security.Cryptography.X509Certificates.X509Certificate certificate, WebRequest request, int certificateProblem)
		{
				return true;
		}


        public string PostMultipartData(string target, Dictionary<string, string> dict, string streamfieldname, byte[] FileData) {
            try {
                f_HttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(target);
                if (!autoredir) {
                    f_HttpWebRequest.AllowAutoRedirect = false;
                }
                f_HttpWebRequest.Timeout = System.Threading.Timeout.Infinite;
                f_HttpWebRequest.ReadWriteTimeout = System.Threading.Timeout.Infinite;
                f_HttpWebRequest.Accept = "image/gif, image/jpeg, image/pjpeg, " +
                                          "application/x-ms-application, application/vnd.ms-xpsdocument, " +
                                          "application/xaml+xml, application/x-ms-xbap, application/x-shockwave-" +
                                          "flash, application/vnd.ms-excel, application/vnd.ms-powerpoint," +
                                          "application/xhtml+xml, " +
                                          "application/xml, " +
                                          "application/msword, */*";
                //f_HttpWebRequest.KeepAlive = true;


                if (!String.IsNullOrEmpty(f_Referer)) {
                    f_HttpWebRequest.Referer = f_Referer;
                }

                if (f_WebProxy != null) {
                    f_HttpWebRequest.Proxy = f_WebProxy;
                }

                if (!String.IsNullOrEmpty(f_UserAgent)) {
                    f_HttpWebRequest.UserAgent = f_UserAgent;
                }

                if (!keepalive) {
                    f_HttpWebRequest.KeepAlive = false;
                } else {
                    f_HttpWebRequest.KeepAlive = true;
                }

                f_HttpWebRequest.CookieContainer = f_CookieContainer;

                f_HttpWebRequest.Method = "POST";
                //f_HttpWebRequest.KeepAlive = true;
               

                System.Net.ServicePointManager.Expect100Continue = false;

                string boundary = "-------------------------" + DateTime.Now.Ticks.ToString("x");
                string header = System.Environment.NewLine + "--" + boundary + System.Environment.NewLine;
                string footer = System.Environment.NewLine + "--" + boundary + System.Environment.NewLine;

                f_HttpWebRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);

                StringBuilder contents = new StringBuilder();
                contents.Append(System.Environment.NewLine);


                foreach (KeyValuePair<string, string> d in dict) {                   
                    contents.Append(header);
                    if (d.Key.Contains("ARRAYHACK")) {
                        contents.Append("Content-Disposition: form-data; name=\"" + Regex.Match(d.Key,"(?<m>.+?)ARRAYHACK",RegexOptions.Singleline).Groups["m"].Value + "\"" + System.Environment.NewLine);
                    } else {
                        contents.Append("Content-Disposition: form-data; name=\"" + d.Key + "\"" + System.Environment.NewLine);
                    }
                    contents.Append(System.Environment.NewLine);
                    contents.Append(d.Value);
                }

                contents.Append(header);
                contents.Append("Content-Disposition: form-data; name=\""+streamfieldname+"\"; filename=\""+streamfieldname+".jpg\"" + System.Environment.NewLine);
                contents.Append("Content-Type: image/jpeg" + System.Environment.NewLine);
                contents.Append(System.Environment.NewLine);

                byte[] BodyBytes = Encoding.UTF8.GetBytes(contents.ToString());
                byte[] footerBytes = Encoding.UTF8.GetBytes(footer);

                f_HttpWebRequest.ContentLength = BodyBytes.Length + FileData.Length + footerBytes.Length;
//                f_HttpWebRequest.ContentLength = BodyBytes.Length + File.Length + footerBytes.Length;
                Stream requestStream = f_HttpWebRequest.GetRequestStream();
                requestStream.Write(BodyBytes, 0, BodyBytes.Length);
//                requestStream.Write(FileData, 0, FileData.Length);
                Stream fs = new MemoryStream(FileData);
                int bytesRead = 0;
                long bytesSoFar = 0;
                byte[] buffer = new byte[8192];
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                {
                    bytesSoFar += bytesRead;
                    requestStream.Write(buffer, 0, bytesRead);
                  //  Debug.WriteLine(String.Format("sending file data {0:0.000}%", (bytesSoFar * 100.0f) / fs.Length));
                    UploadProgress(bytesSoFar, fs.Length);
                }



                requestStream.Write(footerBytes, 0, footerBytes.Length);
                requestStream.Flush();
                requestStream.Close();


                return new StreamReader(f_HttpWebRequest.GetResponse().GetResponseStream()).ReadToEnd();

            } catch(Exception e) {
                
                return e.Message;
            }
        }


        public string PostAction888(string address, Dictionary<string, string> dict)
        {
            if (!expect100continue) {
                System.Net.ServicePointManager.Expect100Continue = false;
            } else {
                System.Net.ServicePointManager.Expect100Continue = true;
            }

            try {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                if (!String.IsNullOrEmpty(f_Referer)) {
                    base.Headers.Add(HttpRequestHeader.Referer, f_Referer);
                }

                if (f_WebProxy != null) {
                    base.Proxy = f_WebProxy;
                }

                if (!String.IsNullOrEmpty(f_UserAgent)) {
                    base.Headers.Add(HttpRequestHeader.UserAgent, f_UserAgent);
                }

                if (!keepalive) {
                    base.Headers.Add(HttpRequestHeader.KeepAlive, "false");
                } else {
                    base.Headers.Add(HttpRequestHeader.KeepAlive, "true");
                }

                NameValueCollection postData = new NameValueCollection();
                foreach (KeyValuePair<string, string> kvp in dict) {
                    postData.Add(kvp.Key, kvp.Value);
                }
                base.Encoding = Encoding.UTF8;
                return encoding.GetString(base.UploadValues(address, "POST", postData));
            } catch (WebException e) {
                if (e.Response != null) {
                    using (WebResponse response = e.Response) {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;                      
                        Stream responseStream = httpResponse.GetResponseStream();
                        if (httpResponse.ContentEncoding.ToLower().Contains("gzip")) {
                            responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                        } else if (httpResponse.ContentEncoding.ToLower().Contains("deflate")) {
                            responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                        }
                        string text = String.Empty;
                        if (e.Status == WebExceptionStatus.RequestCanceled) {
                            return String.Empty;
                        }
                        using (StreamReader reader = new StreamReader(responseStream, Encoding.Default)) {
                            text = reader.ReadToEnd();
                            if (String.IsNullOrEmpty(text)) {
                                return String.Empty;
                            }
                            return text;
                        }
                    }
                }
                return String.Empty;
            } catch (Exception e) {
                return String.Empty;
            }
        }

        public string PostAction(string target, Dictionary<string, string> dict) {
            f_lasturl = f_currenturl;
            f_currenturl = target;

			ServicePointManager.CertificatePolicy = new NWebClient (10000);	
            if (f_AutoReferer)
            {
                f_Referer = f_LastUrl;
                f_LastUrl = target;
            }

            string output = String.Empty;
            try {
                f_HttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(target);
                    f_HttpWebRequest.AllowAutoRedirect = true;
                f_HttpWebRequest.Timeout = f_Timeout;
                f_HttpWebRequest.Accept = "image/gif, image/jpeg, image/pjpeg, " +
                                          "application/x-ms-application, application/vnd.ms-xpsdocument, " +
                                          "application/xaml+xml, application/x-ms-xbap, application/x-shockwave-" +
                                          "flash, application/vnd.ms-excel, application/vnd.ms-powerpoint," +
                                          "application/xhtml+xml, " +
                                          "application/xml, " +
                                          "application/msword, */*";

                //f_HttpWebRequest.KeepAlive = true;                
                f_HttpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                //f_HttpWebRequest.Headers.Add(HttpRequestHeader.Expect, "100-continue");
                //f_HttpWebRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
                if (!String.IsNullOrEmpty(f_UserAgent)) {
                    f_HttpWebRequest.UserAgent = f_UserAgent;
                }
                
                if (!String.IsNullOrEmpty(f_Referer)) {
                    f_HttpWebRequest.Referer = f_Referer;
                }

                if (f_WebProxy != null)
                {
                    f_HttpWebRequest.Proxy = f_WebProxy;
                }

                f_HttpWebRequest.CookieContainer = f_CookieContainer;
                f_HttpWebRequest.Timeout = f_Timeout;
                f_HttpWebRequest.Method = "POST";
                //f_HttpWebRequest.KeepAlive = false;

                if (!keepalive) {
                    f_HttpWebRequest.KeepAlive = false;
                } else {
                    f_HttpWebRequest.KeepAlive = true;
                }
		
                
                f_HttpWebRequest.ContentType = ContentType;
                UTF8Encoding encoding = new UTF8Encoding();

                string postData = String.Empty;

                foreach (KeyValuePair<string, string> d in dict) {
                    postData += String.Format("&{0}={1}", d.Key, d.Value);
                }
                
                postData = postData.Substring(1, postData.Length - 1);
                f_HttpWebRequest.ContentLength = encoding.GetBytes(postData).Length;                
                Stream stream = f_HttpWebRequest.GetRequestStream();
                stream.Write(encoding.GetBytes(postData), 0, encoding.GetBytes(postData).Length);
                stream.Close();

                //f_HttpWebResponse = (HttpWebResponse)f_HttpWebRequest.GetResponse();

                f_HttpWebResponse = (HttpWebResponse)f_HttpWebRequest.GetResponse();
                Stream responseStream = f_HttpWebResponse.GetResponseStream();

                if (f_HttpWebResponse.ContentEncoding.ToLower().Contains("gzip")) {
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                } else if (f_HttpWebResponse.ContentEncoding.ToLower().Contains("deflate")) {
                    responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                }

                //string theResponse = String.Empty;

                using (StreamReader reader = new StreamReader(responseStream, Encoding.Default)) {
                    output = reader.ReadToEnd();
                }

				

                //using (StreamReader streamReader = new StreamReader(f_HttpWebResponse.GetResponseStream())) {                  
                //    output = streamReader.ReadToEnd();
                //}
				
				foreach(Cookie c in f_HttpWebResponse.Cookies){
                    if (cookiepathfix) {
                        f_CookieContainer.SetCookies(new Uri(String.Format("http://{0}",c.Domain)), c.Name+"="+c.Value);
                    }
#if DEBUG
                    // Console.WriteLine("-------------- cookie --------------");
                    // Console.WriteLine("2:"+c.Name+":"+c.Value+":"+c.Path);
                    // Console.WriteLine("-------------- /cookie -------------");
#endif
				}
            } catch (WebException e) {
                try {
                    Error = e.Message;
                    using( WebResponse response = e.Response ) {
                        if( response != null ) {
                            f_HttpWebResponse = ( HttpWebResponse ) response;
                            // Console.WriteLine("Error code: {0}", f_HttpWebResponse.StatusCode);
                            // Console.WriteLine(e.Message); 
                            using( Stream data = response.GetResponseStream() ) {
                                string text = new StreamReader( data ).ReadToEnd();
                                return text;
                            }
                        } else {
                            return output;
                        }
                    }
                } catch {
                }
            } catch(Exception e) {
                // Console.WriteLine("Exception => {0}", e.Message);
            }
            return output;
        }
        
        public new byte[] DownloadData(string address)
        {
            try {
                f_HttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(address);
                f_HttpWebRequest.Accept = "image/gif, image/jpeg, image/pjpeg, " +
                                          "application/x-ms-application, application/vnd.ms-xpsdocument, " +
                                          "application/xaml+xml, application/x-ms-xbap, application/x-shockwave-" +
                                          "flash, application/vnd.ms-excel, application/vnd.ms-powerpoint," +
                                          "application/xhtml+xml, " +
                                          "application/xml, " +
                                          "application/msword, */*";
                //f_HttpWebRequest.KeepAlive = true;

                if (!keepalive) {
                    f_HttpWebRequest.KeepAlive = false;
                } else {
                    f_HttpWebRequest.KeepAlive = true;
                }

                f_HttpWebRequest.CookieContainer = f_CookieContainer;

                MemoryStream memoryStream = new MemoryStream(0x10000);

                using (Stream responseStream = f_HttpWebRequest.GetResponse().GetResponseStream()) {
                    byte[] buffer = new byte[0x1000];
                    int bytes;
                    while ((bytes = responseStream.Read(buffer, 0, buffer.Length)) > 0) {
                        memoryStream.Write(buffer, 0, bytes);
                    }
                }
                return memoryStream.ToArray();
            } catch (WebException e) {
                Error = e.Message;
                using (WebResponse response = e.Response) {                   
                    MemoryStream memoryStream = new MemoryStream(0x10000);
                    if (response != null) {
                        //   f_HttpWebResponse = (HttpWebResponse)response;
                        using (Stream responseStream = response.GetResponseStream()) {
                            byte[] buffer = new byte[0x1000];
                            int bytes;
                            while ((bytes = responseStream.Read(buffer, 0, buffer.Length)) > 0) {
                                memoryStream.Write(buffer, 0, bytes);
                            }
                        }
                        return memoryStream.ToArray();
                    } else {
                        return null;
                    }
                    }                 
            } catch (Exception e) {
                // Console.WriteLine("Exception => {0}", e.Message);
                return null;
            }
        }
        
        public string UploadFileEx(string url, Dictionary<string, string> querystring, byte[] picturebytes)
        {
            string ret = String.Empty;
            try
            {
                long length = 0;
                string boundary = String.Format(
                        "----------------------------{0}",
                        DateTime.Now.Ticks.ToString("x")
                );

                f_HttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                f_HttpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                f_HttpWebRequest.Method = "POST";
                f_HttpWebRequest.KeepAlive = false;

                f_HttpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Stream memStream = new System.IO.MemoryStream();
                byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");

                //write first boundary
                memStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                length += boundaryBytes.Length;

                foreach (KeyValuePair<String, string> valpair in querystring) {
                    //Send post fields
                    byte[] PostData = System.Text.Encoding.UTF8.GetBytes("content-disposition: form-data; name=\"" + valpair.Key + "\"\r\n\r\n" + valpair.Value);
                    length += PostData.Length;
                    memStream.Write(PostData, 0, PostData.Length);
                    //write first boundary
                    memStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    length += boundaryBytes.Length;
                }
                string headerTemplate = "content-disposition: form-data; name=\"" + "pict" + "\"; filename=\"pic.jpg\"\r\nContent-Type: application/octet-stream\r\n\r\n";

                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(headerTemplate);
                memStream.Write(headerbytes, 0, headerbytes.Length);
                length += headerbytes.Length;

                MemoryStream fileStream = new MemoryStream(picturebytes);
                byte[] buffer = new byte[1024];

                int bytesRead = 0;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                    length += bytesRead;
                }

                memStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                length += boundaryBytes.Length;

                fileStream.Close();


                f_HttpWebRequest.ContentLength = memStream.Length;
                Stream requestStream = f_HttpWebRequest.GetRequestStream();

                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                requestStream.Close();


                WebResponse webResponse2 = f_HttpWebRequest.GetResponse();

                Stream stream2 = webResponse2.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);

                ret = reader2.ReadToEnd();
                //   MessageBox.Show(ret);
                webResponse2.Close();
                f_HttpWebRequest = null;
                webResponse2 = null;

                string[] retarr = ret.Split('|');
                if (retarr.Length > 4)
                {
                    ret = retarr[5];
                }
            }
            catch(Exception e)
            {
                Error = e.Message;
                ret = String.Empty;
            }
            return ret;
        }

        public void Ping(String ping, String url, String name)
        {
            try
            {
                f_HttpWebRequest = (HttpWebRequest)WebRequest.Create(ping);
                if (f_WebProxy != null)
                {
                    f_HttpWebRequest.Proxy = f_WebProxy;
                }
                f_HttpWebRequest.Method = "POST";
                f_HttpWebRequest.ContentType = "text/xml";
                f_HttpWebRequest.Timeout = 3000;
                AddXmlToRequest(f_HttpWebRequest, name, url);
                f_HttpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
            }
        }

        private static void AddXmlToRequest(HttpWebRequest request, String name, String url)
        {
            Stream stream = (Stream)request.GetRequestStream();
            using (XmlTextWriter xml = new XmlTextWriter(stream, Encoding.UTF8))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement("methodCall");
                xml.WriteElementString("methodName", "weblogUpdates.ping");
                xml.WriteStartElement("params");
                xml.WriteStartElement("param");
                xml.WriteElementString("value", name);
                xml.WriteEndElement();
                xml.WriteStartElement("param");
                xml.WriteElementString("value", url);
                xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndElement();
            }
        }

    }
}