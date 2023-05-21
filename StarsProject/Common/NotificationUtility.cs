using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

namespace StarsProject.Common
{
    public class NotificationUtility
    {
        private static string INSTANCE_ID = "17";
        private static string CLIENT_ID = "info@sharvayainfotech.com";
        private static string CLIENT_SECRET = "d4f5bb582f544636a13eca721ecbebb6";

        private static string API_URL = "http://api.whatsmate.net/v3/whatsapp/single/text/message/" + INSTANCE_ID;
        private static string DOCUMENT_SINGLE_API_URL = "http://api.whatsmate.net/v3/whatsapp/single/document/message/" + INSTANCE_ID;

        static public string convertFileToBase64(string fullPathToDoc)
        {
            Byte[] bytes = File.ReadAllBytes(fullPathToDoc);
            String base64Encoded = Convert.ToBase64String(bytes);
            return base64Encoded;
        }

        public bool sendDocument(string number, string base64Content, string fn)
        {
            bool success = true;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                    client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                    Entity.SingleDocPayload payloadObj = new Entity.SingleDocPayload() { number = number, document = base64Content, filename = fn };
                    string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                    client.Encoding = Encoding.UTF8;
                    string response = client.UploadString(DOCUMENT_SINGLE_API_URL, postData);
                    Console.WriteLine(response);
                }
            }
            catch (WebException webEx)
            {
                Console.WriteLine(((HttpWebResponse)webEx.Response).StatusCode);
                Stream stream = ((HttpWebResponse)webEx.Response).GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                String body = reader.ReadToEnd();
                Console.WriteLine(body);
                success = false;
            }

            return success;
        }


        public bool SendMsgNotification(string senderMobileNo, string msgDescription, string senderCountryCode="91")
        {
            bool success = true;
            // ------------------------------------------------------------
            // Getting Msg91 credentials from Procedure AuthenticateUser
            // ------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)System.Web.HttpContext.Current.Session["logindetail"];
            // -------------------------------------------------------
            if (!String.IsNullOrEmpty(objAuth.SMS_Uri) && !String.IsNullOrEmpty(objAuth.SMS_AuthKey) 
                && !String.IsNullOrEmpty(objAuth.SMS_SenderID) && !String.IsNullOrEmpty(senderMobileNo) 
                && !String.IsNullOrEmpty(msgDescription.Trim()))
            {
                string sendSMSUri = objAuth.SMS_Uri;
                string authKey = objAuth.SMS_AuthKey;
                string senderId = objAuth.SMS_SenderID;
                //string message = HttpUtility.UrlEncode(msgDescription);

                //Prepare you post parameters
                System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();
                //sbPostData.AppendFormat("authkey={0}", authKey);
                //sbPostData.AppendFormat("&country={0}", ((String.IsNullOrEmpty(senderCountryCode) ? "91" : senderCountryCode)));
                //sbPostData.AppendFormat("&mobiles={0}", senderMobileNo);
                //sbPostData.AppendFormat("&message={0}", message);
                //sbPostData.AppendFormat("&sender={0}", senderId);
                //sbPostData.AppendFormat("&route={0}", "default");

                senderMobileNo = "919099988301";
                sbPostData.AppendFormat("authkey={0}", authKey);
                sbPostData.AppendFormat("&mobiles={0}", senderMobileNo);
                sbPostData.AppendFormat("&message={0}", msgDescription);
                sbPostData.AppendFormat("&sender={0}", senderId);
                sbPostData.AppendFormat("&route={0}", 4); //If your operator supports multiple routes then give one route name. Eg: route=1 for promotional, route=4 for transactional SMS.

                sendSMSUri = "http://api.msg91.com/api/sendhttp.php";                //Call Send SMS API


                try
                {

                    System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sendSMSUri);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    byte[] data = encoding.GetBytes(sbPostData.ToString());
                    //Specify post method
                    httpWReq.Method = "POST";
                    httpWReq.ContentType = "application/x-www-form-urlencoded";
                    httpWReq.ContentLength = data.Length;
                    using (System.IO.Stream stream = httpWReq.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    // Get the response
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httpWReq.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseString = reader.ReadToEnd();

                    // Close the response
                    reader.Close();
                    response.Close();
                
                }
                catch (SystemException ex)
                {
                    success = false;
                }
            }
            return success;
        }
    }

}