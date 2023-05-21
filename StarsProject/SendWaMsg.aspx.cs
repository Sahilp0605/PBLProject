using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

namespace StarsProject
{
    public partial class SendWaMsg : System.Web.UI.Page
    {
        private static string INSTANCE_ID = "17";
        private static string CLIENT_ID = "info@sharvayainfotech.com";
        private static string CLIENT_SECRET = "d4f5bb582f544636a13eca721ecbebb6";

        private static string API_URL = "http://api.whatsmate.net/v3/whatsapp/single/text/message/" + INSTANCE_ID;
        private static string DOCUMENT_SINGLE_API_URL = "http://api.whatsmate.net/v3/whatsapp/single/document/message/" + INSTANCE_ID;

        protected void Page_Load(object sender, EventArgs e)
        {
            string base64Content = convertFileToBase64("D:\\MYCRM\\StarsProject\\PDF\\SO-SEP20-001.pdf");
            //sendMessage("917435048773", "Test Message");
            //sendDocument("917435048773", base64Content, "SO-SEP20-001.pdf");

        }

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

                    SingleDocPayload payloadObj = new SingleDocPayload() { number = number, document = base64Content, filename = fn };
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

        public bool sendMessage(string number, string message)
        {
            bool success = true;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                    client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                    Payload payloadObj = new Payload() { number = number, message = message };
                    string postData = (new JavaScriptSerializer()).Serialize(payloadObj);


                    client.Encoding = Encoding.UTF8;
                    string response = client.UploadString(API_URL, postData);
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

        public class SingleDocPayload
        {
            public string number { get; set; }
            public string document { get; set; }
            public string filename { get; set; }
        }

        public class Payload
        {
            public string number { get; set; }
            public string message { get; set; }
            public string filename { get; set; }
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            // --------------------------------------------------------------
            // ReDirect this page from Default.aspx after successful login
            // --------------------------------------------------------------

            Boolean testFlag = sendMessage("919099988301", "Om Gm Ganpate Namah, God Bless Us All");
            SendSMSCampaign("","919099988301", "Om Gm Ganpate Namah, God Bless Us All");
            //Common.NotificationUtility clinst = new Common.NotificationUtility();
            //Boolean testFlag = clinst.SendMsgNotification("9638621973", "Greetings to you, From Sharvaya Infotech");
            //string testString = SendSMSCampaign("admin", "", "AAAAAAAAA");
        }

        public virtual string SendSMSCampaign(String pLoginUserID, String MobileNo, String strMsg)
        {
            //http://sms.hspsms.com/sendSMS?username=femicure&message=Test&sendername=FEMICURE&smstype=TRANS&numbers=8140939704&apikey=5119b0de-8b01-4542-9b25-675e9a4d4e58

            string authKey = "344471AjSVnjECNQz5f86bd38P1";
            string mobileNumber = "9099988301";      //Multiple mobiles numbers separated by comma  
            //string mobileNumber = "917435048773";
            string SMSType = "4";                               //Route Or transactional/Promotional
            string senderId = "SHINFO";                         //Sender ID,While using route4 sender id should be 6 characters long.
            string message = HttpUtility.UrlEncode(strMsg);     //Your message to send, Add URL encoding here.

            try
            {
                string sendSMSUri = "";
                System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();    
                sbPostData.AppendFormat("authkey={0}", authKey);
                sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
                sbPostData.AppendFormat("&message={0}", message);
                sbPostData.AppendFormat("&sender={0}", senderId);
                sbPostData.AppendFormat("&route={0}", SMSType);
                sendSMSUri = "http://api.msg91.com/api/sendhttp.php";                

                System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sendSMSUri);       //Create HTTPWebrequest
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();         
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                httpWReq.Method = "POST";                                                   
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (System.IO.Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httpWReq.GetResponse();   //Get the response
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                reader.Close();       
                response.Close();

                return responseString;
            }
            catch (SystemException ex)
            {
                return "SMS Sending Failed! :" + ex.Message;
                //MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}