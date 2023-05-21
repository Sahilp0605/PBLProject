using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Configuration;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Reflection;


namespace StarsProject
{
    public class PageBase : System.Web.UI.Page
    {

        #region Variables
        public static string AppPath = GetAppPath();
        public static string AppPhysicalPath = HttpContext.Current.Server.MapPath("~");
        private bool _RequireSSL;
        public string strsitename = ConfigurationManager.AppSettings["SiteName"];
        public static string LiveAppPath = System.Configuration.ConfigurationManager.AppSettings["LivePath"].ToString();
        // -----------------------------------------------------------------------
        // Below declaration for Encryption/Decryption Logic
        // -----------------------------------------------------------------------
        private byte[] key = { };
        private byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };
        
        #endregion

        #region Property
        [Browsable(true)]
        [Description("Indicates whether or not this page should be forced into or out of SSL")]
        public virtual bool RequireSSL
        {
            get
            {
                return _RequireSSL;
            }
            set
            {
                _RequireSSL = value;
            }
        }
        [Browsable(true)]
        [Description("Indicates the web.config key of Css")]
        public virtual string CssKey { get; set; }

        [Browsable(true)]
        [Description("Indicates the web.config key of Js")]
        public virtual string JsKey { get; set; }

        //[Browsable(true)]
        //[Description("Current User")]
        //public virtual Security.CustomIdentity CurrentUser { get { return (Security.CustomIdentity)HttpContext.Current.User.Identity; } }
        #endregion

        #region Push SSL
        //[System.Diagnostics.DebuggerStepThrough()]
        //[System.Diagnostics.Conditional("SECURE")]
        private void PushSSL()
        {
            const string SECURE = "https://";
            const string UNSECURE = "http://";
            //Force required into secure channel
            if (RequireSSL && Request.IsSecureConnection == false)
                Response.Redirect(Request.Url.ToString().Replace(UNSECURE, SECURE));
            //Force non-required out of secure channel
            if (!RequireSSL && Request.IsSecureConnection == true)
                Response.Redirect(Request.Url.ToString().Replace(SECURE, UNSECURE));
        }
        #endregion

        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //PushSSL();
        }
        #endregion

        #region OnLoad
        /// <summary>
        /// Fire common Page Load
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Page.Header != null)
            {
                //HtmlLink favicon = new HtmlLink();
                //favicon.Attributes.Add("rel", "shortcut icon");
                ////favicon.Attributes.Add("type", "image/x-con");
                //favicon.Attributes.Add("href", AppPath + "favicon.ico");
                //Page.Header.Controls.Add(favicon);
            }
            /*if (!String.IsNullOrEmpty(this.CssKey))
            {
                //<link rel="icon"  href="images/favico.ico" type="image/x-con" />
                
                HtmlLink favicon = new HtmlLink();
                favicon.Attributes.Add("rel", "shortcut icon");
                favicon.Attributes.Add("type", "image/x-con");
                favicon.Attributes.Add("href", ConfigurationManager.AppSettings["Livepath"].ToString()+"favico.ico");                
                Page.Header.Controls.Add(favicon);

                HtmlLink lnk = new HtmlLink();
                lnk.Attributes.Add("type", "text/css");
                lnk.Attributes.Add("rel", "stylesheet");
                lnk.Attributes.Add("href", AppPath + "Handler/CssJsHandler.ashx?s=" + this.CssKey + "&t=text/css&v=1");
                Page.Header.Controls.Add(lnk);
                

            }
            if (!String.IsNullOrEmpty(this.JsKey))
            {
                Page.ClientScript.RegisterClientScriptInclude(typeof(string), "BMNHandledJs", AppPath + "Handler/CssJsHandler.ashx?s=" + this.JsKey + "&t=text/javascript&v=1");
            }
            
            Response.Cache.SetNoStore();
*/

        }
        #endregion

        #region OnError
        /// <remarks>
        /// Method Name:-OnError        
        /// Purpose:- To catch the errors of the .aspx pages and register them in the Errors table
        /// Created By:-Bhaumik
        /// Created Date(dd/mm/yyyy):-02/05/2013
        /// </remarks>
        protected override void OnError(EventArgs e)
        {
            string strmessage = "";
            string strsource = "";

            HttpContext ctx = HttpContext.Current;
            Exception exception = ctx.Server.GetLastError();

            string errorInfo =
            "<br>Offending URL: " + ctx.Request.Url.ToString() +
            "<br>Source: " + exception.Source +
            "<br>Message: " + exception.Message +
            "<br>Stack trace: " + exception.StackTrace;
            strmessage = errorInfo;

            //OnlineCMS.MailHelper.SendMailMessage("info@manageamc.com", "bhaumik.metizsoft@gmail.com", "", "", "SMART CMS :: Error ", errorInfo);
            strsource = ctx.Request.Url.ToString();

            ////To add the error to the database 
            //DAL.ErrorMaster objError = new DAL.ErrorMaster();
            //objError.ErrorMessege = strmessage;
            //objError.HostAddress = Request.UserHostAddress;
            //objError.Source = strsource;
            //objError.Name = ctx.User.Identity.IsAuthenticated ? ctx.User.Identity.Name : "Anonymous";
            //objError.Browser = ctx.Request.Browser.Browser + " " + ctx.Request.Browser.Version;
            ////ctx.Request.Browser.ScreenCharactersWidth.ToString() + "x" + ctx.Request.Browser.ScreenCharactersHeight.ToString() + " @" + ctx.Request.Browser.ScreenBitDepth.ToString() + "bit",
            //objError.Session = ctx.Session.SessionID;
            //objError.Cookies = ctx.Request.Browser.Cookies.ToString();
            ////ctx.Request.Browser.JavaScript,
            //objError.Timestamp = DateTime.Now;
            //HttpCookie aCookie = Request.Cookies["JokerData"];
            //BAL.ErrorMgmt.vendorcode = aCookie.Value;
            //long retValue = BAL.ErrorMgmt.AddErrors(objError);

            ////For redirection to the errorpage
            ////string strpath = "admin/AdminErrorPage.aspx";
            ////Response.Redirect(strpath);

            //string strpath = LiveAppPath + "Error.aspx?Id=0";
            ////if (Request.Url.ToString().ToLower().Contains("admin"))
            ////{
            ////    strpath = "~/admin/Error.aspx";
            ////}
            ////else
            ////{
            ////    strpath = "~/error.aspx";
            ////}
            //exception.Source 
                
            Response.Redirect("error.aspx?err=" + exception.Message + "......" + exception.Source );
            //ScriptManager.RegisterStartupScript(this, typeof(string), "error", "javascript:window.location.href='error.aspx';", true);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "error", "javascript:alert('error.aspx');", true);

        }
        #endregion

        #region GetAppPath
        public static string GetAppPath()
        {
            if (HttpContext.Current.Request.ApplicationPath.Length > 1)
                return HttpContext.Current.Request.ApplicationPath + "/";
            else
                return HttpContext.Current.Request.ApplicationPath;
        }
        #endregion

        #region [Page Render]
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
            base.RenderChildren(htmlWriter);
            string html = stringWriter.ToString();
            html = Regex.Replace(html, "<title>(\\s*)PlayerHill", "<title>$1" + strsitename, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            html = Regex.Replace(html, "<title>(\\s*)Player Hill", "<title>$1" + strsitename, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            writer.Write(html);
        }
        #endregion

        #region GetUtcDate
        public static DateTime GetUTCdate()
        {
            return DateTime.UtcNow.AddHours(int.Parse(ConfigurationManager.AppSettings["AddhoursinUTC"].ToString()));
        }
        #endregion

        #region CreateTicket
        //Overloaded function CreateTicket
        public static void CreateTicket(int Id, string UserName)
        {
            CreateTicket(Id, UserName, false);
        }

        public static void CreateTicket(int Type, string UserName, bool IsPersistant)
        {
            System.Web.HttpResponse response = HttpContext.Current.Response;
            FormsAuthenticationTicket tkt;
            string cookiestr;
            HttpCookie ck;

            //if (Type == 1)
            //{
            tkt = new FormsAuthenticationTicket(1,
                UserName,
                DateTime.Now,
                DateTime.Now.AddMinutes(8),
                IsPersistant,
                "user");
            cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            ck.Path = FormsAuthentication.FormsCookiePath;
            response.Cookies.Add(ck);
            //}
        }
        #endregion

        #region ChangeFileName
        /// <remarks>
        /// Method Name:-ChangeFileName
        /// Purpose:- To change the filename of the uploaded image file
        /// Created By:-Snehal
        /// Created Date(dd/mm/yyyy):-17/04/2007
        /// </remarks>
        public static string ChangeFileName(string filename)
        {
            string newFileName;
            string[] Substring;
            char[] delimitdot = { '.' };
            Substring = filename.Replace(" ", "").Split(delimitdot, 2);
            return newFileName = RemoveSpecialCharactor(Substring[0]) + PageBase.GetUTCdate().ToString("MMddyyyyhhmmssfff") + '.' + Substring[1];
        }
        #endregion

        #region RemoveSpecialCharactor
        /// <summary>
        /// To replace special charactor
        /// </summary>
        /// <param name="strToReplace">String to replace</param>
        /// <returns>string Relaced string</returns>
        public static string RemoveSpecialCharactor(string strToReplace)
        {
            strToReplace = strToReplace.Replace("~", "");
            strToReplace = strToReplace.Replace("`", "");
            strToReplace = strToReplace.Replace("!", "");
            strToReplace = strToReplace.Replace("@", "");
            strToReplace = strToReplace.Replace("#", "");
            strToReplace = strToReplace.Replace("$", "");
            strToReplace = strToReplace.Replace("%", "");
            strToReplace = strToReplace.Replace("^", "");
            strToReplace = strToReplace.Replace("&", "");
            strToReplace = strToReplace.Replace("*", "");
            strToReplace = strToReplace.Replace("(", "");
            strToReplace = strToReplace.Replace(")", "");
            strToReplace = strToReplace.Replace("-", "");
            strToReplace = strToReplace.Replace("_", "");
            strToReplace = strToReplace.Replace("+", "");
            strToReplace = strToReplace.Replace("=", "");
            strToReplace = strToReplace.Replace("[", "");
            strToReplace = strToReplace.Replace("]", "");
            strToReplace = strToReplace.Replace("{", "");
            strToReplace = strToReplace.Replace("}", "");
            strToReplace = strToReplace.Replace("|", "");
            strToReplace = strToReplace.Replace("\\", "");
            strToReplace = strToReplace.Replace(":", "");
            strToReplace = strToReplace.Replace(";", "");
            strToReplace = strToReplace.Replace("'", "");
            strToReplace = strToReplace.Replace("\"", "");
            strToReplace = strToReplace.Replace(".", "");
            strToReplace = strToReplace.Replace(",", "");
            strToReplace = strToReplace.Replace("<", "");
            strToReplace = strToReplace.Replace(">", "");
            strToReplace = strToReplace.Replace("''", "");
            strToReplace = strToReplace.Replace("?", "");
            strToReplace = strToReplace.Replace("/", "");
            strToReplace = strToReplace.Replace(" ", "-");
            strToReplace = strToReplace.Replace("  ", "-");
            return strToReplace.ToLower();
        }
        #endregion

        #region CheckSpecialCharactor
        /// <summary>
        /// To replace special charactor
        /// </summary>
        /// <param name="strToReplace">String to replace</param>
        /// <returns>string Relaced string</returns>
        public static bool CheckSpecialCharactor(string strToReplace)
        {
            if (strToReplace.IndexOf("~") > -1 ||
                strToReplace.IndexOf("`") > -1 ||
                strToReplace.IndexOf("!") > -1 ||
                strToReplace.IndexOf("@") > -1 ||
                strToReplace.IndexOf("#") > -1 ||
                strToReplace.IndexOf("$") > -1 ||
                strToReplace.IndexOf("%") > -1 ||
                strToReplace.IndexOf("^") > -1 ||
                strToReplace.IndexOf("&") > -1 ||
                strToReplace.IndexOf("*") > -1 ||
                strToReplace.IndexOf("(") > -1 ||
                strToReplace.IndexOf(")") > -1 ||
                strToReplace.IndexOf("-") > -1 ||
                strToReplace.IndexOf("_") > -1 ||
                strToReplace.IndexOf("+") > -1 ||
                strToReplace.IndexOf("=") > -1 ||
                strToReplace.IndexOf("[") > -1 ||
                strToReplace.IndexOf("]") > -1 ||
                strToReplace.IndexOf("{") > -1 ||
                strToReplace.IndexOf("}") > -1 ||
                strToReplace.IndexOf("|") > -1 ||
                strToReplace.IndexOf("\\") > -1 ||
                strToReplace.IndexOf(":") > -1 ||
                strToReplace.IndexOf(";") > -1 ||
                strToReplace.IndexOf("'") > -1 ||
                strToReplace.IndexOf("\"") > -1 ||
                strToReplace.IndexOf(".") > -1 ||
                strToReplace.IndexOf(",") > -1 ||
                strToReplace.IndexOf("<") > -1 ||
                strToReplace.IndexOf(">") > -1 ||
                strToReplace.IndexOf("''") > -1 ||
                strToReplace.IndexOf("?") > -1 ||
                strToReplace.IndexOf("/") > -1 ||
                strToReplace.IndexOf(" ") > -1 ||
                strToReplace.IndexOf("  ") > -1)

                return false;
            else
                return true;
        }
        #endregion

        #region ResizeImage
        /// <remarks>
        /// Method Name:-ImageResize
        /// Purpose:- To Resize Image
        /// Created By:-Kushan
        /// Created Date(dd/mm/yyyy):-13/06/2007
        /// </remarks>
        public static string ResizeImage(string ImagePath, int RequiredHeight, int Requiredwidth)
        {
            int destHeight = RequiredHeight, destWidth = Requiredwidth;
            if (File.Exists(ImagePath))
            {

                System.Drawing.Image originalImage;
                originalImage = System.Drawing.Image.FromFile(ImagePath);

                int sourceWidth = originalImage.Width;
                int sourceHeight = originalImage.Height;
                if (sourceWidth > Requiredwidth || sourceHeight > RequiredHeight)
                {
                    int destX = 0;
                    int destY = 0;
                    float nPercent = 0;
                    float nPercentW = 0;
                    float nPercentH = 0;
                    nPercentW = ((float)Requiredwidth / (float)sourceWidth);
                    nPercentH = ((float)RequiredHeight / (float)sourceHeight);
                    if (nPercentH < nPercentW)
                    {
                        nPercent = nPercentH;
                        destX = (int)((Requiredwidth - (sourceWidth * nPercent)) / 2);
                    }
                    else
                    {
                        nPercent = nPercentW;
                        destY = (int)((RequiredHeight - (sourceHeight * nPercent)) / 2);
                    }
                    destWidth = (int)(sourceWidth * nPercent);
                    destHeight = (int)(sourceHeight * nPercent);
                }
                else
                {
                    destWidth = sourceWidth;
                    destHeight = sourceHeight;
                }
                originalImage.Dispose();
                return destHeight + "_" + destWidth;
            }
            else
            {
                return destHeight + "_" + destWidth;
            }
        }
        #endregion

        public static string GetStringValue(string val)
        {
            if(val == null)
                return "";
            else
                return val;
        }
        //public static string CheckModuleRight(Entity.AccessRights entity)
        //{
        //    string accessstr = "";
        //    accessstr += entity.All + "," + entity.Add + "," + entity.Edit + "," + entity.View + "," + entity.Delete;
        //    return accessstr;
        //}


        public string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string EncryptQueryString(string strQueryString)
        {
            return Encrypt(strQueryString, "r0b1nr0y");
        }

        public string DecryptQueryString(string strQueryString)
        {
            return Decrypt(strQueryString, "r0b1nr0y");
        }

        public static DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType.Name.Contains("Nullable") ? typeof(string) : prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents   
                        endStr = "Paisa " + endStr;//Cents   
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX   
                bool isDone = false;//test if already translated   
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))   
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric   
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping   
                    String place = "";//digit grouping name:hundres,thousand,etc...   
                    switch (numDigits)
                    {
                        case 1://ones' range   

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range   
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range   
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range   
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range   
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range   
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...   
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)   
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }


                    }
                    //ignore digit grouping names   
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }

        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        public static string ConvertImageToBase64(string xFilePath)
        {
            string base64ImageRepresentation = "";
            byte[] imageArray = System.IO.File.ReadAllBytes(@xFilePath);
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        public static Boolean checkGeneralMenuAccess(string xMenuName)
        {
            Boolean retVal = true;
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)System.Web.HttpContext.Current.Session["logindetail"];
            // --------------------------------------------------
            if (xMenuName.ToLower() == "customer")
                retVal = objAuth.CustomerAccess;
            else if (xMenuName.ToLower() == "product")
                retVal = objAuth.ProductAccess;
            // --------------------------------------------------
            return retVal;
        }
    }
}


