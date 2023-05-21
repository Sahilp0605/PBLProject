using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class MailBox : System.Web.UI.Page
    {
        Entity.Authenticate objAuth; 
        string _MailServer, _MailPort, _MailUserID, _MailPassword;
        Boolean _MailSSL;

        int ReturnCode = 0;
        string ReturnMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];

                _MailServer = "pop3.yandex.com";
                _MailPort = "995";
                _MailUserID = objAuth.EmailAddress;
                _MailPassword = objAuth.EmailPassword;
                _MailSSL = true;
                spnEmployeeName.InnerText = objAuth.EmployeeName;
                spnEmailAddress.InnerText = objAuth.EmailAddress;
                imgEmployee.Src = (!String.IsNullOrEmpty(objAuth.EmployeeImage)) ? objAuth.EmployeeImage : "images/customer.png";
                // ------------------------------------------
                hdnMailCount.Value = BAL.CommonMgmt.GetConstant("MailCount", 0);
                hdnMailCount.Value = (String.IsNullOrEmpty(hdnMailCount.Value) || hdnMailCount.Value == "0") ? "100" : hdnMailCount.Value;
                // ------------------------------------------
                Read_Emails();
                // ------------------------------------------
                string tmpVal = BAL.MailBoxMgmt.setLastMailTimestamp(objAuth.UserID);
            }
        }

        protected void Read_Emails()
        {
            // For Uid Reference : http://hpop.sourceforge.net/exampleDownloadUnread.php
            // http://hpop.sourceforge.net/examples.php
            // https://gist.github.com/zinto/9d5e0c81c4327f8aeb0e
            Pop3Client pop3Client;
            pop3Client = new Pop3Client();
            pop3Client.Connect(_MailServer, int.Parse(_MailPort), _MailSSL);
            pop3Client.Authenticate(_MailUserID, _MailPassword);
            Session["Pop3Client"] = pop3Client;
            int mailCount = pop3Client.GetMessageCount();

            // ---------------------------------------------------
            for (int i=1; i <= mailCount; i++)
            {
                Message message = pop3Client.GetMessage(i);
                // ----------------------------------------------------
                Entity.MailBox lstMail = new Entity.MailBox();
                if (message.Headers.DateSent > objAuth.MailTimestamp)
                {
                    lstMail.MessageID = message.Headers.MessageId;
                    lstMail.MailDate = message.Headers.DateSent;
                    lstMail.MailFrom = message.Headers.From.ToString();
                    lstMail.MailTo = message.Headers.To.ToString();
                    lstMail.MailCc = message.Headers.Cc.ToString();
                    lstMail.MailSubject = message.Headers.Subject;
                    OpenPop.Mime.MessagePart plainTextPart = message.FindFirstPlainTextVersion();
                    string bodytext = plainTextPart.GetBodyAsText();
                    lstMail.MailBody = bodytext;
                    lstMail.LoginUserID = objAuth.UserID;

                    BAL.MailBoxMgmt.AddUpdateMailBoxEntry(lstMail, out ReturnCode, out ReturnMsg);
                }
            }
            // -----------------------------------------------
            int totrec;
            List<Entity.MailBox> lstMailRec = new List<Entity.MailBox>();
            lstMailRec = BAL.MailBoxMgmt.GetMailBoxList(0, Session["LoginUserID"].ToString(), 1, 10000, out totrec);
            if (lstMailRec.Count > 0)
            {
                lstMailRec.OrderByDescending(xx => xx.MailDate).ToList();
                rptMailList.DataSource = lstMailRec;
                rptMailList.DataBind();
            }
        }

        //public List<RfcMailAddress> ParseMailAddresses(string input)
        //{
        //    if (input == null)
        //        throw new ArgumentNullException("input");

        //    List<RfcMailAddress> returner = new List<RfcMailAddress>();

        //    // MailAddresses are split by commas
        //    IEnumerable<string> mailAddresses = Utility.SplitStringWithCharNotInsideQuotes(input, ',');

        //    // Parse each of these
        //    foreach (string mailAddress in mailAddresses)
        //    {
        //        returner.Add(ParseMailAddress(mailAddress));
        //    }

        //    return returner;
        //}

        public bool DeleteMessageByMessageId(string messageId)
        {
            Pop3Client objClient =  (Pop3Client)Session["Pop3Client"];
            int messageCount = objClient.GetMessageCount();
            for (int messageItem = messageCount; messageItem > 0; messageItem--)
            {
                if (objClient.GetMessageHeaders(messageItem).MessageId == messageId)
                {
                    objClient.DeleteMessage(messageItem);
                    return true;
                }
            }
            return false;
        }

        public static void FindPlainTextInMessage(Message message)
        {
            MessagePart plainText = message.FindFirstPlainTextVersion();
            if (plainText != null)
            {
                //plainText.Save(new FileInfo("plainText.txt"));
            }
        }

        public static void FindHtmlInMessage(Message message)
        {
            MessagePart html = message.FindFirstHtmlVersion();
            if (html != null)
            {
                //html.Save(new FileInfo("html.txt"));
            }
        }

        public static void FindXmlInMessage(Message message)
        {
            MessagePart xml = message.FindFirstMessagePartWithMediaType("text/xml");
            if (xml != null)
            {
                string xmlString = xml.GetBodyAsText();
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                
                doc.LoadXml(xmlString);
                doc.Save("test.xml");
            }
        }

        public List<Message> FetchUnseenMessages(string hostname, int port, bool useSsl, string username, string password, List<string> seenUids)
        {
            Pop3Client objClient = (Pop3Client)Session["Pop3Client"];
            objClient.Connect(hostname, port, useSsl);
            objClient.Authenticate(username, password);
            List<string> uids = objClient.GetMessageUids();

            // Create a list we can return with all new messages
            List<Message> newMessages = new List<Message>();

            // All the new messages not seen by the POP3 client
            for (int i = 0; i < uids.Count; i++)
            {
                string currentUidOnServer = uids[i];
                if (!seenUids.Contains(currentUidOnServer))
                {
                    // We have not seen this message before.
                    // Download it and add this new uid to seen uids

                    // the uids list is in messageNumber order - meaning that the first
                    // uid in the list has messageNumber of 1, and the second has 
                    // messageNumber 2. Therefore we can fetch the message using
                    // i + 1 since messageNumber should be in range [1, messageCount]
                    Message unseenMessage = objClient.GetMessage(i + 1);

                    // Add the message to the new messages
                    newMessages.Add(unseenMessage);

                    // Add the uid to the seen uids, as it has now been seen
                    seenUids.Add(currentUidOnServer);
                }
            }
            return newMessages;
        }
    }
}