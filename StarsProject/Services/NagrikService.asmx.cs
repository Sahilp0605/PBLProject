using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace StarsProject.Services
{


    [WebService(Namespace = "http://demo.sharvayainfotech.in/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    

    [System.Web.Script.Services.ScriptService]
    public class NagrikService : System.Web.Services.WebService
    {
        int totrec = 0;
        int ReturnCode = 0;
        string ReturnMsg = "";
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string GetMobileQuotation(string userid, string password, Int64 quotid)
        {
            string tmpVal = "success";

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = BAL.UserMgmt.AuthenticateUser(userid, password);

            Session["logindetail"] = objAuth;
            Session["LoginUserID"] = objAuth.UserID;
            Session["CompanyID"] = objAuth.CompanyID;
            Session["CompanyName"] = objAuth.CompanyName;
            Session["CompanyType"] = objAuth.CompanyType;
            Session["SerialKey"] = objAuth.SerialKey;
            Session["RoleCode"] = objAuth.RoleCode;
            if (objAuth.StateCode > 0)
                Session["CompanyStateCode"] = objAuth.StateCode;
            else
                Session["CompanyStateCode"] = 0;
            // ----------------------------------------------------------------------------
            //string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            //string LoginUserID = HttpContext.Current.Session["LoginUserID"].ToString();
            //string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            //string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/images");
            //Int32 CompanyId = 0;

            //StarsProject.QuotationEagle.serialkey = tmpSerialKey;
            //StarsProject.QuotationEagle.LoginUserID = LoginUserID;
            //StarsProject.QuotationEagle.printheader = flagPrintHeader;
            //StarsProject.QuotationEagle.path = Path;
            //StarsProject.QuotationEagle.imagepath = imagepath;
            //StarsProject.QuotationEagle.companyid = CompanyId;
            //StarsProject.QuotationEagle.printModule = "Quotation";
            // ----------------------------------------------------------------------------
            //QuotationEagle qt = new QuotationEagle();
            //qt.GenerateQuotationMobile(userid, password, quotid);
            // ------------------------------------------------------
            StarsProject.Class1 qt = new StarsProject.Class1();
            qt.GenerateQuotation_Sharvaya_Orig(quotid);

            return tmpVal;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetPageHiddenControls(string pPageName)
        {
            String varResult = "";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Entity.ApplicationMenu> rows = new List<Entity.ApplicationMenu>();

            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            varResult = BAL.CommonMgmt.GetPageHiddenControls(pPageName);
            return varResult;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetUserAddEditDel()
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Entity.ApplicationMenu> rows = new List<Entity.ApplicationMenu>();
            
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.ApplicationMenu> lstEntity = new List<Entity.ApplicationMenu>();
            if (Session["LoginUserID"] != null)
            {
                lstEntity = BAL.CommonMgmt.GetMenuAddEditDelList(14, Session["LoginUserID"].ToString());
                foreach (Entity.ApplicationMenu point in lstEntity)
                {
                    Entity.ApplicationMenu row = new Entity.ApplicationMenu();
                    row.AddFlag = point.AddFlag;
                    row.EditFlag = point.EditFlag;
                    row.DelFlag = point.DelFlag;
                    rows.Add(row);
                }
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GeneralAddEditDel(string menuid)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Entity.ApplicationMenu> rows = new List<Entity.ApplicationMenu>();

            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.ApplicationMenu> lstEntity = new List<Entity.ApplicationMenu>();
            if (Session["LoginUserID"] != null)
            {
                lstEntity = BAL.CommonMgmt.GetMenuGeneralAddEditDelList(menuid, Session["LoginUserID"].ToString());
                foreach (Entity.ApplicationMenu point in lstEntity)
                {
                    Entity.ApplicationMenu row = new Entity.ApplicationMenu();
                    row.AddFlag = point.AddFlag;
                    row.EditFlag = point.EditFlag;
                    row.DelFlag = point.DelFlag;
                    rows.Add(row);
                }
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetReminderList()
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            Int64 totcount = 0;
            // -------------------------------------------------------
            List<Entity.DashboardNotification> lstEntity = new List<Entity.DashboardNotification>();
            if (Session["LoginUserID"] != null)
            {
                lstEntity = BAL.CommonMgmt.GetNotificationList(Session["LoginUserID"].ToString(), "reminder");
                foreach (Entity.DashboardNotification point in lstEntity)
                {
                    row = new Dictionary<string, object>();

                    row.Add("ModuleName", point.ModuleName);
                    row.Add("Description", point.Description);
                    rows.Add(row);
                }
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string webGetMenuIconList()
        {
            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            //serializer.MaxJsonLength = Int32.MaxValue;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.ApplicationMenu> lstEntity = new List<Entity.ApplicationMenu>();
            lstEntity = BAL.CommonMgmt.GetMenuIconList(Session["LoginUserID"].ToString());
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string webGetGeneralMenuList()
        {
            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            //serializer.MaxJsonLength = Int32.MaxValue;
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.ApplicationMenu> lstEntity = new List<Entity.ApplicationMenu>();
            lstEntity = BAL.CommonMgmt.GetGeneralMenuList(Session["LoginUserID"].ToString());
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetReportsList()
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.ReportMenu> lstEntity = new List<Entity.ReportMenu>();
            lstEntity = BAL.CommonMgmt.GetReportsList();
            foreach (Entity.ReportMenu point in lstEntity)
            {
                row = new Dictionary<string, object>();
                row.Add("pkID", point.pkID);
                row.Add("ReportName", point.ReportName);
                row.Add("ReportText", point.ReportText);
                row.Add("ParentID", point.ParentID);
                row.Add("ReportURL", point.ReportURL);
                row.Add("ReportOrder", point.ReportOrder);
                row.Add("ReportImage", point.ReportImage);
                row.Add("ReportImageHeight", point.ReportImageHeight);
                row.Add("ReportImageWidth", point.ReportImageWidth);

                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string setCustomerInfo(Entity.Customer cust)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            BAL.CustomerMgmt.AddUpdateCustomer(cust, out ReturnCode, out ReturnMsg);
            // -------------------------------------------------------
            row = new Dictionary<string, object>();
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetCalenderList(Int64 pMonth, Int64 pYear)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.CalenderEvent> lstEntity = new List<Entity.CalenderEvent>();
            lstEntity = BAL.CommonMgmt.GetCalenderList(pMonth, pYear, Session["LoginUserID"].ToString());
            foreach (Entity.CalenderEvent point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("pkID", point.pkID);
                row.Add("Title", point.Title);
                row.Add("StartDate", point.StartDate.ToLocalTime());
                row.Add("EndDate", point.EndDate.ToLocalTime());
                row.Add("EmployeeName", point.EmployeeName);
                row.Add("Status", point.Status);
                row.Add("className", point.className);
                row.Add("ImageUrl", point.imageurl);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetCalenderListByEmployee(Int64 pMonth, Int64 pYear, Int64 pEmployeeID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.CalenderEvent> lstEntity = new List<Entity.CalenderEvent>();
            lstEntity = BAL.CommonMgmt.GetCalenderListByEmployee(pMonth, pYear, pEmployeeID, Session["LoginUserID"].ToString());
            foreach (Entity.CalenderEvent point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("pkID", point.pkID);
                row.Add("Title", point.Title);
                row.Add("StartDate", point.StartDate.ToLocalTime());
                row.Add("EndDate", point.EndDate.ToLocalTime());
                row.Add("EmployeeName", point.EmployeeName);
                row.Add("Status", point.Status);
                row.Add("className", point.className);
                row.Add("ImageUrl", point.imageurl);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetWidgetList(String LoginUserID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetWidgetList(Session["LoginUserID"].ToString());
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("flag", point.flag);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardInquirySummary(string pLoginUserID, Int64 pMon, Int64 pYear, String pCategory)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardInquirySummary(pLoginUserID, pMon, pYear, pCategory);
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("value", point.value);
                if (pCategory == "YEARLY")
                {
                    row.Add("value1", point.value1);
                    row.Add("value2", point.value2);
                    row.Add("value3", point.value3);
                    row.Add("value4", point.value4);
                    row.Add("value5", point.value5);
                    row.Add("value6", point.value6);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardInquirySourceSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardInquirySourceSummary(pLoginUserID, pMonth, pYear);
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("Jan", point.Jan);
                row.Add("Feb", point.Feb);
                row.Add("Mar", point.Mar);
                row.Add("Apr", point.Apr);
                row.Add("May", point.May);
                row.Add("Jun", point.Jun);
                row.Add("Jul", point.Jul);
                row.Add("Aug", point.Aug);
                row.Add("Sep", point.Sep);
                row.Add("Oct", point.Oct);
                row.Add("Nov", point.Nov);
                row.Add("Dec", point.Dec);
                row.Add("value", point.value);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardInquiryDisQualiSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardInquiryDisQualiSummary(pLoginUserID, pMonth, pYear);
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("Jan", point.Jan);
                row.Add("Feb", point.Feb);
                row.Add("Mar", point.Mar);
                row.Add("Apr", point.Apr);
                row.Add("May", point.May);
                row.Add("Jun", point.Jun);
                row.Add("Jul", point.Jul);
                row.Add("Aug", point.Aug);
                row.Add("Sep", point.Sep);
                row.Add("Oct", point.Oct);
                row.Add("Nov", point.Nov);
                row.Add("Dec", point.Dec);
                row.Add("value", point.value);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardInquiryTeleCallStatusSummary(string pLoginUserID, Int64 pYear)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardInquiryTeleCallStatusSummary(pLoginUserID, pYear);
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("Jan", point.Jan);
                row.Add("Feb", point.Feb);
                row.Add("Mar", point.Mar);
                row.Add("Apr", point.Apr);
                row.Add("May", point.May);
                row.Add("Jun", point.Jun);
                row.Add("Jul", point.Jul);
                row.Add("Aug", point.Aug);
                row.Add("Sep", point.Sep);
                row.Add("Oct", point.Oct);
                row.Add("Nov", point.Nov);
                row.Add("Dec", point.Dec);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardInquiryTeleDisQualiSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardInquiryTeleDisQualiSummary(pLoginUserID, pMonth, pYear);
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("Jan", point.Jan);
                row.Add("Feb", point.Feb);
                row.Add("Mar", point.Mar);
                row.Add("Apr", point.Apr);
                row.Add("May", point.May);
                row.Add("Jun", point.Jun);
                row.Add("Jul", point.Jul);
                row.Add("Aug", point.Aug);
                row.Add("Sep", point.Sep);
                row.Add("Oct", point.Oct);
                row.Add("Nov", point.Nov);
                row.Add("Dec", point.Dec);
                row.Add("value", point.value);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardInquiryTeleConversionSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardInquiryTeleConversionSummary(pLoginUserID, pMonth, pYear);
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("value", point.value);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardTeleEntrySummary(string pLoginUserID, string pLeadSource, string pCategory, Int64 pMonth, Int64 pYear)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardTeleEntrySummary(pLoginUserID, pLeadSource, pCategory, pMonth, pYear);

            if (pCategory.ToLower() == "monthly")
            {
                foreach (Entity.DashboardCountSummary point in lstEntity)
                {
                    row = new Dictionary<string, object>();
                    row.Add("label", point.label);
                    row.Add("Jan", point.Jan);
                    row.Add("Feb", point.Feb);
                    row.Add("Mar", point.Mar);
                    row.Add("Apr", point.Apr);
                    row.Add("May", point.May);
                    row.Add("Jun", point.Jun);
                    row.Add("Jul", point.Jul);
                    row.Add("Aug", point.Aug);
                    row.Add("Sep", point.Sep);
                    row.Add("Oct", point.Oct);
                    row.Add("Nov", point.Nov);
                    row.Add("Dec", point.Dec);
                    rows.Add(row);
                }
            }
            else if (pCategory.ToLower() == "daily")
            {
                foreach (Entity.DashboardCountSummary point in lstEntity)
                {
                    row = new Dictionary<string, object>();
                    row.Add("label", point.label);
                    row.Add("d1", point.d1);
                    row.Add("d2", point.d2);
                    row.Add("d3", point.d3);
                    row.Add("d4", point.d4);
                    row.Add("d5", point.d5);
                    row.Add("d6", point.d6);
                    row.Add("d7", point.d7);
                    row.Add("d8", point.d8);
                    row.Add("d9", point.d9);
                    row.Add("d10", point.d10);
                    row.Add("d11", point.d11);
                    row.Add("d12", point.d12);
                    row.Add("d13", point.d13);
                    row.Add("d14", point.d14);
                    row.Add("d15", point.d15);
                    row.Add("d16", point.d16);
                    row.Add("d17", point.d17);
                    row.Add("d18", point.d18);
                    row.Add("d19", point.d19);
                    row.Add("d20", point.d20);
                    row.Add("d21", point.d21);
                    row.Add("d22", point.d22);
                    row.Add("d23", point.d23);
                    row.Add("d24", point.d24);
                    row.Add("d25", point.d25);
                    row.Add("d26", point.d26);
                    row.Add("d27", point.d27);
                    row.Add("d28", point.d28);
                    row.Add("d29", point.d29);
                    row.Add("d30", point.d30);
                    row.Add("d31", point.d31);
                    rows.Add(row);
                }
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardSalesSummary(Int64 pMon, Int64 pYear, String pCategory)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardSalesSummary(Session["LoginUserID"].ToString(), pMon, pYear, pCategory);
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("value", point.value);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardExternalSummary(Int64 pMon, Int64 pYear, String pCategory)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardExternalSummary(Session["LoginUserID"].ToString(), pMon, pYear, pCategory);
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("value", point.value);
                row.Add("value1", point.value1);
                row.Add("value2", point.value2);
                row.Add("value3", point.value3);
                row.Add("value4", point.value4);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboard2Summary(String pCategory, DateTime date1, DateTime date2)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // -------------------------------------------------------
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboard2_Summary(pCategory, date1, date2, Session["LoginUserID"].ToString());
            foreach (Entity.DashboardCountSummary point in lstEntity)
            {
                row = new Dictionary<string, object>();

                row.Add("label", point.label);
                row.Add("value", point.value);
                row.Add("value1", point.value1);
                row.Add("value2", point.value2);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string GetServerTimestamp()
        {
            string tmpVal = BAL.CommonMgmt.GetServerTimestamp();
            return tmpVal;
        }
        // ======================================================================================
        // Module : Inquiry Module 
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string GetInquiryNo(string pDate)
        {
            string tmpVal = BAL.CommonMgmt.GetInquiryNo(pDate);
            return tmpVal;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string GetInquiryProduct(string pInquiryNo)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(pInquiryNo))
            {
                lstEntity = BAL.InquiryInfoMgmt.GetInquiryProductGroupList(pInquiryNo);
                foreach (Entity.InquiryInfo point in lstEntity)
                {
                    row = new Dictionary<string, object>();

                    row.Add("pkID", point.pkID);
                    row.Add("ProductGroupName", point.ProductGroupName);
                    row.Add("InquiryNo", pInquiryNo);
                    row.Add("Quantity", point.Quantity);

                    rows.Add(row);
                }

            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webFinancialTrans(string currPageNo,string TrType)
        {
            List<Entity.FinancialTrans> lstEntity = new List<Entity.FinancialTrans>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetFinancialTransList(0, Session["LoginUserID"].ToString(), "", TrType, Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webFinancialTransSearch(string SearchKey, string TrType)
        {
            List<Entity.FinancialTrans> lstEntity = new List<Entity.FinancialTrans>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetFinancialTransList(0, Session["LoginUserID"].ToString(), SearchKey, TrType, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        // ======================================================================================
        // Module : Project Sheet
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProjectSheet(string currPageNo)
        {
            List<Entity.ProjectSheet> lstEntity = new List<Entity.ProjectSheet>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProjectSheetMgmt.GetProjectSheetList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProjectSheetSearch(string SearchKey)
        {
            List<Entity.ProjectSheet> lstEntity = new List<Entity.ProjectSheet>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProjectSheetMgmt.GetProjectSheetList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // Module : Job Card
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobCard(string currPageNo)
        {
            List<Entity.JobCard> lstEntity = new List<Entity.JobCard>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobCardMgmt.GetJobCardList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobCardSearch(string SearchKey)
        {
            List<Entity.JobCard> lstEntity = new List<Entity.JobCard>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobCardMgmt.GetJobCardList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // Module : SiteSurvay Module 
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSiteSurvay(string currPageNo)
        {
            List<Entity.SiteSurvay> lstEntity = new List<Entity.SiteSurvay>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SiteSurvayMgmt.GetSiteSurvay(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSiteSurvaySearch(string SearchKey)
        {
            List<Entity.SiteSurvay> lstEntity = new List<Entity.SiteSurvay>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SiteSurvayMgmt.GetSiteSurvay(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // ======================================================================================
        // Module : SiteSurvayReport Module 
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSiteSurvayReport(string currPageNo)
        {
            List<Entity.SiteSurveyReport> lstEntity = new List<Entity.SiteSurveyReport>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SiteSurveyReportMgmt.GetSiteSurveyReport(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSiteSurvayReportSearch(string SearchKey)
        {
            List<Entity.SiteSurveyReport> lstEntity = new List<Entity.SiteSurveyReport>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SiteSurveyReportMgmt.GetSiteSurveyReport(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPettyCash(string currPageNo)
        {
            List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetPettyCashList(0, Session["LoginUserID"].ToString(), Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPettyCashSearch(string SearchKey)
        {
            List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetPettyCashList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDBCRNote(string currPageNo, string DBC)
        {
            List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetDBCRNoteList(0, DBC, Session["LoginUserID"].ToString(), Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDBCRNoteSearch(string SearchKey, string DBC)
        {
            List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetDBCRNoteList(0, DBC, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJournalVoucher(string currPageNo)
        {
            List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetJournalVoucherList(0, Session["LoginUserID"].ToString(), Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJournalVoucherSearch(string SearchKey)
        {
            List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetJournalVoucherList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webExpenseVoucher(string currPageNo)
        {
            List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetExpenseVoucherList(0, Session["LoginUserID"].ToString(), Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webExpenseVoucherSearch(string SearchKey)
        {
            List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FinancialTransMgmt.GetExpenseVoucherList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInquiryDetailByStatus(string currPageNo)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoList(0, Session["LoginUserID"].ToString(), "", "", 0, 0, Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInquiryDetailByStatusSearch(string SearchKey)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoList(0, Session["LoginUserID"].ToString(), SearchKey, "", 0, 0, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInquiryClinicDetail()
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            //lstEntity = BAL.InquiryInfoClinicMgmt.GetInquiryInfoClinicList(0,Session["LoginUserID"].ToString(), 1, 50000, out TotRecCount);
            lstEntity = BAL.InquiryInfoClinicMgmt.GetInquiryInfoClinicList("", Session["LoginUserID"].ToString(), 0, 0);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInquiryByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoList("", pLoginUserID, pMonth, pYear);

            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInquiryByUserPeriod(string pLoginUserID, string pFromDate, string pToDate)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoByUserPeriod(pLoginUserID, pFromDate, pToDate);

            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webQuotationByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.QuotationMgmt.GetQuotationByUser(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webQuotationByUserPeriod(string pLoginUserID, string pFromDate, string pToDate)
        {
            List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.QuotationMgmt.GetQuotationByUserPeriod(pLoginUserID, pFromDate, pToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webFollowUpByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FollowupMgmt.GetFollowupByUser(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webFollowUpByUserPeriod(string pLoginUserID, string pFromDate, string pToDate)
        {
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FollowupMgmt.GetFollowupByUserPeriod(pLoginUserID, pFromDate, pToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webToDOListByUser(String pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.ToDo> lstEntity = new List<Entity.ToDo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ToDoMgmt.GetToDoListByUser(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webToDOListByUserPeriod(String pLoginUserID, string pFromDate, string pToDate)
        {
            List<Entity.ToDo> lstEntity = new List<Entity.ToDo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ToDoMgmt.GetToDoListByUserPeriod(pLoginUserID, pFromDate, pToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLeaveRequestListByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.LeaveRequest> lstEntity = new List<Entity.LeaveRequest>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LeaveRequestMgmt.GetLeaveRequestListByUser(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLeaveRequestListByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            List<Entity.LeaveRequest> lstEntity = new List<Entity.LeaveRequest>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LeaveRequestMgmt.GetLeaveRequestListByUserPeriod(pLoginUserID, FromDate, ToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLatePunchByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.Attendance> lstEntity = new List<Entity.Attendance>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.AttendanceMgmt.GetLatePunchList(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLatePunchByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            List<Entity.Attendance> lstEntity = new List<Entity.Attendance>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.AttendanceMgmt.GetLatePunchListPeriod(pLoginUserID, FromDate, ToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDailyActivityByUser(string pLoginUserID, string pActivityDate, Int64 pMonth, Int64 pYear)
        {
            List<Entity.DailyActivity> lstEntity = new List<Entity.DailyActivity>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.DailyActivityMgmt.GetDailyActivityListByUser(pLoginUserID, pActivityDate, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDailyActivityByUserPeriod(string pLoginUserID, string pActivityDate, string FromDate, string ToDate)
        {
            List<Entity.DailyActivity> lstEntity = new List<Entity.DailyActivity>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.DailyActivityMgmt.GetDailyActivityListByUserPeriod(pLoginUserID, pActivityDate, FromDate, ToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesOrderByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesOrderMgmt.GetSalesOrderList("", pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesOrderByUserPeriod(string pLoginUserID, string pFromDate, string pToDate)
        {
            List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesOrderMgmt.GetSalesOrderListPeriod(pLoginUserID, pFromDate, pToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webContactByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CustomerMgmt.GetCustomerList(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webContactByUserPeriod(string pLoginUserID, string pFromDate, string pToDate)
        {
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CustomerMgmt.GetCustomerList(pLoginUserID, 0, 0, pFromDate, pToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLoginLogoutByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.Users> lstEntity = new List<Entity.Users>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.UserMgmt.GetUserLogList(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLoginLogoutByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            List<Entity.Users> lstEntity = new List<Entity.Users>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.UserMgmt.GetUserLogListPeriod(pLoginUserID, FromDate, ToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInquiryStatusList(string pLoginUserID, string pStatus, Int64 pMonth, Int64 pYear)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryInfoMgmt.GetInquiryStatusList(pLoginUserID, pStatus, pMonth, pYear);
            lstEntity = lstEntity.OrderBy(x => x.EmployeeName).ThenBy(x => x.InquiryDate).ToList();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // Module : Leave Request
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLeaveRequest(string currPageNo)
        {
            List<Entity.LeaveRequest> lstEntity = new List<Entity.LeaveRequest>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LeaveRequestMgmt.GetLeaveRequestList(0, Session["LoginUserID"].ToString(), "", 0, 0, Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLeaveRequestSearch(string SearchKey)
        {
            List<Entity.LeaveRequest> lstEntity = new List<Entity.LeaveRequest>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LeaveRequestMgmt.GetLeaveRequestList(0, Session["LoginUserID"].ToString(), SearchKey, 0, 0, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        // ======================================================================================
        // Module : OverTime
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOverTime(string currPageNo)
        {
            List<Entity.OverTime> lstEntity = new List<Entity.OverTime>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OverTimeMgmt.GetOverTimeList(0, Session["LoginUserID"].ToString(), "", 0, 0, Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOverTimeSearch(string SearchKey)
        {
            List<Entity.OverTime> lstEntity = new List<Entity.OverTime>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OverTimeMgmt.GetOverTimeList(0, Session["LoginUserID"].ToString(), SearchKey, 0, 0, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // Module : Followup
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webFollowupDetail(string currPageNo)
        {
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FollowupMgmt.GetFollowupList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webFollowupDetailSearch( string SearchKey)
        {
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.FollowupMgmt.GetFollowupList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        // ======================================================================================
        // Module : Projects
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProjectsDetail(string currPageNo)
        {
            List<Entity.Projects> lstEntity = new List<Entity.Projects>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProjectsMgmt.GetProjectsList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProjectsDetailSearch(string SearchKey)
        {
            List<Entity.Projects> lstEntity = new List<Entity.Projects>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProjectsMgmt.GetProjectsList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // Module : TODO
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webToDoDetail(string currPageNo)
        {
            List<Entity.ToDo> lstEntity = new List<Entity.ToDo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ToDoMgmt.GetToDoList(0, Session["LoginUserID"].ToString(), Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webToDoDetailSearch(string SearchKey)
        {
            List<Entity.ToDo> lstEntity = new List<Entity.ToDo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ToDoMgmt.GetToDoList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // Module : Missed Punch
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMissedPunch(string currPageNo)
        {
            List<Entity.MissedPunch> lstEntity = new List<Entity.MissedPunch>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.AttendanceMgmt.GetMissedPunchList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMissedPunchSearch(string SearchKey)
        {
            List<Entity.MissedPunch> lstEntity = new List<Entity.MissedPunch>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.AttendanceMgmt.GetMissedPunchList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        // ======================================================================================
        // Module : Loan
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLoanInstallment(string currPageNo)
        {
            List<Entity.Loan> lstEntity = new List<Entity.Loan>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LoanMgmt.GetLoan("Loan", 0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLoanInstallmentSearch(string SearchKey)
        {
            List<Entity.Loan> lstEntity = new List<Entity.Loan>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LoanMgmt.GetLoan("Loan", 0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        // ======================================================================================
        // Module : Loan
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webAdvanceSalary(string currPageNo)
        {
            List<Entity.Loan> lstEntity = new List<Entity.Loan>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LoanMgmt.GetLoan("Advance", 0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webAdvanceSalarySearch(string SearchKey)
        {
            List<Entity.Loan> lstEntity = new List<Entity.Loan>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LoanMgmt.GetLoan("Advance", 0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        // ======================================================================================
        // Module : Quotation
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string GetQuotationNo(string pDate)
        {
            string tmpVal = BAL.CommonMgmt.GetQuotationNo(pDate);
            return tmpVal;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webQuotationDetail(string currPageNo)
        {
            List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.QuotationMgmt.GetQuotationList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webQuotationDetailSearch(string SearchKey)
        {
            List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.QuotationMgmt.GetQuotationList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // Module : Sales Order
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string GetSalesOrderNo(string pDate)
        {
            string tmpVal = BAL.CommonMgmt.GetSalesOrderNo(pDate);
            return tmpVal;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesOrderDetail(string currPageNo)
        {
            List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesOrderMgmt.GetSalesOrderList(0, Session["SerialKey"].ToString(),Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesOrderDetailSearch(string SearchKey)
        {
            List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesOrderMgmt.GetSalesOrderList(0, Session["SerialKey"].ToString(), Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesOrderDealerDetail(string currPageNo)
        {
            List<Entity.SalesOrderDealer> lstEntity = new List<Entity.SalesOrderDealer>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesOrderDealerDetailSearch(string SearchKey)
        {
            List<Entity.SalesOrderDealer> lstEntity = new List<Entity.SalesOrderDealer>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPurchaseOrderDetail(string currPageNo)
        {
            List<Entity.PurchaseOrder> lstEntity = new List<Entity.PurchaseOrder>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PurchaseOrderMgmt.GetPurchaseOrderList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPurchaseOrderDetailSearch(string SearchKey)
        {
            List<Entity.PurchaseOrder> lstEntity = new List<Entity.PurchaseOrder>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PurchaseOrderMgmt.GetPurchaseOrderList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webWorkOrderDetail(string currPageNo)
        {
            List<Entity.WorkOrderComm> lstEntity = new List<Entity.WorkOrderComm>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.WorkOrderCommMgmt.GetWorkOrderCommList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webWorkOrderDetailSearch(string SearchKey)
        {
            List<Entity.WorkOrderComm> lstEntity = new List<Entity.WorkOrderComm>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.WorkOrderCommMgmt.GetWorkOrderCommList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductionDetail(string currPageNo)
        {
            List<Entity.Production> lstEntity = new List<Entity.Production>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductionMgmt.GetProduction(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductionDetailSearch(string SearchKey)
        {
            List<Entity.Production> lstEntity = new List<Entity.Production>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductionMgmt.GetProduction(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductionBySODetail(string currPageNo)
        {
            List<Entity.Production> lstEntity = new List<Entity.Production>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductionMgmt.GetProductionBySoList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductionBySODetailSearch(string SearchKey)
        {
            List<Entity.Production> lstEntity = new List<Entity.Production>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductionMgmt.GetProductionBySoList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInwardDetail(string currPageNo)
        {
            List<Entity.Inward> lstEntity = new List<Entity.Inward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InwardMgmt.GetInwardList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString(); 
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInwardDetailSearch(string SearchKey)
        {
            List<Entity.Inward> lstEntity = new List<Entity.Inward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InwardMgmt.GetInwardList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //-------  Created for Acupanel------------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCompalaintDetail(string currPageNo)
        {
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(0,0,"",0,0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCompalaintDetailSearch(string SearchKey)
        {
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(0, 0, "", 0, 0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        //-------  Created for Acupanel------------------------------

        //----------------------------Production For Piyush ---------------------------

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMoldingDetail(string currPageNo)
        {
            List<Entity.Molding> lstEntity = new List<Entity.Molding>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.MoldingMgmt.GetMoldingList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMoldingDetailSearch(string SearchKey)
        {
            List<Entity.Molding> lstEntity = new List<Entity.Molding>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.MoldingMgmt.GetMoldingList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //-----------------------------Internal Work Order-------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInternalWorkOrderDetail(string currPageNo)
        {
            List<Entity.InternalWorkOrder> lstEntity = new List<Entity.InternalWorkOrder>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InternalWorkOrderMgmt.GetInternalWorkOrderList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webwebInternalWorkOrderDetailSearch(string SearchKey)
        {
            List<Entity.InternalWorkOrder> lstEntity = new List<Entity.InternalWorkOrder>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InternalWorkOrderMgmt.GetInternalWorkOrderList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        //------------------------Job Card inWard ----------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobCardInwardDetail(string currPageNo)
        {
            List<Entity.JobCardInward> lstEntity = new List<Entity.JobCardInward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobCardInwardMgmt.GetJobCardInwardList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobCardInwardDetailSearch(string SearchKey)
        {
            List<Entity.JobCardInward> lstEntity = new List<Entity.JobCardInward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobCardInwardMgmt.GetJobCardInwardList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //------------------------Material Indent ----------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webIndentDetail(string currPageNo)
        {
            List<Entity.MaterialIndent> lstEntity = new List<Entity.MaterialIndent>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.MaterialIndentMgmt.GetMaterialIndent(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webIndentDetailSearch(string SearchKey)
        {
            List<Entity.MaterialIndent> lstEntity = new List<Entity.MaterialIndent>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.MaterialIndentMgmt.GetMaterialIndent(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //--------------------------------------------------------------------

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOutwardDetail(string currPageNo)
        {
            List<Entity.Outward> lstEntity = new List<Entity.Outward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OutwardMgmt.GetOutwardList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOutwardDetailSearch(string SearchKey)
        {
            List<Entity.Outward> lstEntity = new List<Entity.Outward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OutwardMgmt.GetOutwardList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //---------------------------Job Card Outward-------------------------
        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobCardOutwardDetail(string currPageNo)
        {
            List<Entity.JobCardOutward> lstEntity = new List<Entity.JobCardOutward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobCardOutwardMgmt.GetJobCardOutwardList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobCardOutwardDetailSearch(string SearchKey)
        {
            List<Entity.JobCardOutward> lstEntity = new List<Entity.JobCardOutward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobCardOutwardMgmt.GetJobCardOutwardList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //--------------------------------------------------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webBroadcastMessage(string currPageNo)
        {
            List<Entity.BroadCastMessage> lstEntity = new List<Entity.BroadCastMessage>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.BroadCastMessageMgmt.GetBroadCastMessage(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webBroadcastMessageSearch(string SearchKey)
        {
            List<Entity.BroadCastMessage> lstEntity = new List<Entity.BroadCastMessage>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.BroadCastMessageMgmt.GetBroadCastMessage(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string testmodule(string currPageNo)
        {
            List<Entity.Vehicle> lstEntity = new List<Entity.Vehicle>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VehicleMgmt.GetVehicleList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string testmoduleSearch(string SearchKey)
        {
            List<Entity.Vehicle> lstEntity = new List<Entity.Vehicle>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VehicleMgmt.GetVehicleList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webVehicleMaster(string currPageNo)
        {
            List<Entity.Vehicle> lstEntity = new List<Entity.Vehicle>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VehicleMgmt.GetVehicleList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webVehicleMasterSearch(string SearchKey)
        {
            List<Entity.Vehicle> lstEntity = new List<Entity.Vehicle>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VehicleMgmt.GetVehicleList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webVehicleMasterTrip(string currPageNo)
        {
            List<Entity.VehicleTrip> lstEntity = new List<Entity.VehicleTrip>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VehicleMgmt.GetVehicleTripList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webVehicleMasterTripSearch(string SearchKey)
        {
            List<Entity.VehicleTrip> lstEntity = new List<Entity.VehicleTrip>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VehicleMgmt.GetVehicleTripList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webTransporterMaster(string currPageNo)
        {
            List<Entity.Vehicle> lstEntity = new List<Entity.Vehicle>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VehicleMgmt.GetTransporterList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webTransporterMasterSearch(string SearchKey)
        {
            List<Entity.Vehicle> lstEntity = new List<Entity.Vehicle>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VehicleMgmt.GetTransporterList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesTarget(string currPageNo)
        {
            List<Entity.SalesTarget> lstEntity = new List<Entity.SalesTarget>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesTargetMgmt.GetSalesTarget(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesTargetSearch(string SearchKey)
        {
            List<Entity.SalesTarget> lstEntity = new List<Entity.SalesTarget>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesTargetMgmt.GetSalesTarget(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string getSessionObject()
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            string tmpVal = Session["ReturnTotalRecord"].ToString();
            //string tmpVal = "";
            try
            {
                row = new Dictionary<string, object>();
                row.Add("label", "pageno");
                row.Add("value", tmpVal);
                rows.Add(row);
            }
            catch(Exception ex) {
                throw;
            }
            return serializer.Serialize(rows);
        }

       
        // ======================================================================================
        // Module : Customers
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCustomerDetail(string currPageNo)
        {
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CustomerMgmt.GetCustomerList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();            
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCustomerDetailSearch(string SearchKey)
        {
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CustomerMgmt.GetCustomerList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();  
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCustomerSearchInfo(string pCustName, string pType, string pSource, string pContact, string pEmail, string pState, string pCity)
        {
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CustomerMgmt.GetCustomerSearchInfo(pCustName, pType, pSource, pContact, pEmail, pState, pCity);
            return serializer.Serialize(lstEntity);
        }

        // ======================================================================================
        // Module : Products
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductsDetail(string currPageNo)
        {
            List<Entity.Products> lstEntity = new List<Entity.Products>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            Session["PageNo"] = currPageNo;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductMgmt.GetProductList(0, Session["LoginUserID"].ToString(), Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // ======================================================================================
        // Module : Clean Session 
        // ======================================================================================
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public void cleanSessionData(string varSession)
        {
            Session[varSession] = null;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductsDetailSearch(string SearchKey)
        {
            List<Entity.Products> lstEntity = new List<Entity.Products>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            Session["PageNo"] = 1;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductMgmt.GetProductList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductBrandDetail(string currPageNo)
        {
            List<Entity.Brand> lstEntity = new List<Entity.Brand>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.BrandMgmt.GetBrandList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductBrandDetailSearch(string SearchKey)
        {
            List<Entity.Brand> lstEntity = new List<Entity.Brand>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.BrandMgmt.GetBrandList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPriceList(string currPageNo)
        {
            List<Entity.PriceList> lstEntity = new List<Entity.PriceList>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PriceListMgmt.GetPriceList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPriceListSearch(string SearchKey)
        {
            List<Entity.PriceList> lstEntity = new List<Entity.PriceList>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PriceListMgmt.GetPriceList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCustomerCategory(string currPageNo)
        {
            List<Entity.CustomerCategory> lstEntity = new List<Entity.CustomerCategory>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CustomerCategoryMgmt.GetCustomerCategoryList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCustomerCategorySearch(string SearchKey)
        {
            List<Entity.CustomerCategory> lstEntity = new List<Entity.CustomerCategory>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CustomerCategoryMgmt.GetCustomerCategoryList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductGroupDetail(string currPageNo)
        {
            List<Entity.ProductGroup> lstEntity = new List<Entity.ProductGroup>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductGroupMgmt.GetProductGroupList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();

            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductGroupDetailSearch(string SearchKey)
        {
            List<Entity.ProductGroup> lstEntity = new List<Entity.ProductGroup>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductGroupMgmt.GetProductGroupList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInquiryStatusDetail(string currPageNo)
        {
            List<Entity.InquiryStatus> lstEntity = new List<Entity.InquiryStatus>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryStatusMgmt.GetInquiryStatusList(0, "", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInquiryStatusDetailSearch(string SearchKey)
        {
            List<Entity.InquiryStatus> lstEntity = new List<Entity.InquiryStatus>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryStatusMgmt.GetInquiryStatusList(0, "", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        //------------------------------Cateory Description-----------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCategoryDescriptionDetail(string currPageNo)
        {
            List<Entity.CategoryDescription> lstEntity = new List<Entity.CategoryDescription>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CategoryDescriptionMgmt.GetCategoryDescriptionList(0, "", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCategoryDescriptionDetailSearch(string SearchKey)
        {
            List<Entity.CategoryDescription> lstEntity = new List<Entity.CategoryDescription>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CategoryDescriptionMgmt.GetCategoryDescriptionList(0, "", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webContentsDetail(string currPageNo)
        {
            List<Entity.Contents> lstEntity = new List<Entity.Contents>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CommonMgmt.GetContentList(0, "", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webContentsDetailSearch(string SearchKey)
        {
            List<Entity.Contents> lstEntity = new List<Entity.Contents>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CommonMgmt.GetContentList(0, "", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webFollowupStatusDetail()
        {
            List<Entity.InquiryStatus> lstEntity = new List<Entity.InquiryStatus>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryStatusMgmt.GetInquiryStatusList("FollowupType");
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webFollowupStatusDetailSearch(string SearchKey)
        {
            List<Entity.InquiryStatus> lstEntity = new List<Entity.InquiryStatus>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryStatusMgmt.GetInquiryStatusList("FollowupType");
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOrgStructureDetail(string currPageNo)
        {
            List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOrgStructureDetailSearch(string SearchKey)
        {
            List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOrgEmployeeDetail(string currPageNo)
        {

            List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList("", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOrgEmployeeDetailSearch(string SearchKey)
        {

            List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList("", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDesignationDetail(string currPageNo)
        {
            List<Entity.Designations> lstEntity = new List<Entity.Designations>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.DesignationMgmt.GetDesignation("", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDesignationDetailSearch(string SearchKey)
        {
            List<Entity.Designations> lstEntity = new List<Entity.Designations>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.DesignationMgmt.GetDesignation("", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOrgTypeDetail(string currPageNo)
        {
            List<Entity.OrgTypes> lstEntity = new List<Entity.OrgTypes>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OrgTypeMgmt.GetOrgType(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOrgTypeDetailSearch(string SearchKey)
        {
            List<Entity.OrgTypes> lstEntity = new List<Entity.OrgTypes>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OrgTypeMgmt.GetOrgType(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProofDetail()
        {
            List<Entity.Proof> lstEntity = new List<Entity.Proof>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProofMgmt.GetProofList();
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webUsersDetail(string currPageNo)
        {
            List<Entity.Users> lstEntity = new List<Entity.Users>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.UserMgmt.GetLoginUserList("", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webUsersDetailSearch(string SearchKey)
        {
            List<Entity.Users> lstEntity = new List<Entity.Users>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.UserMgmt.GetLoginUserList("", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webRolesDetail(string currPageNo)
        {
            List<Entity.Roles> lstEntity = new List<Entity.Roles>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.RolesMgmt.GetRole("", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webRolesDetailSearch(string SearchKey)
        {
            List<Entity.Roles> lstEntity = new List<Entity.Roles>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.RolesMgmt.GetRole("", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webClusterDetail(string currPageNo)
        {
            List<Entity.ZoneCluster> lstEntity = new List<Entity.ZoneCluster>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ZoneClusterMgmt.GetZoneClusterList(0, Session["LoginUserID"].ToString(), Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webClusterDetailSearch(string SearchKey)
        {
            List<Entity.ZoneCluster> lstEntity = new List<Entity.ZoneCluster>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ZoneClusterMgmt.GetZoneClusterList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCityDetail(string currPageNo)
        {
            List<Entity.City> lstEntity = new List<Entity.City>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CityMgmt.GetCity(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCityDetailSearch(string SearchKey)
        {
            List<Entity.City> lstEntity = new List<Entity.City>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CityMgmt.GetCity(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webStateDetail(string currPageNo)
        {
            List<Entity.State> lstEntity = new List<Entity.State>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.StateMgmt.GetState(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webStateDetailSearch(string SearchKey)
        {
            List<Entity.State> lstEntity = new List<Entity.State>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.StateMgmt.GetState(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCountryDetail(string currPageNo)
        {
            List<Entity.Country> lstEntity = new List<Entity.Country>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CountryMgmt.GetCountry("", Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCountryDetailSearch(string SearchKey)
        {
            List<Entity.Country> lstEntity = new List<Entity.Country>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CountryMgmt.GetCountry("", Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesBillDetail(string currPageNo)
        {
            List<Entity.SalesBill> lstEntity = new List<Entity.SalesBill>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesBillMgmt.GetSalesBillList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesBillDetailSearch(string SearchKey)
        {
            List<Entity.SalesBill> lstEntity = new List<Entity.SalesBill>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesBillMgmt.GetSalesBillList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesChallanDetail(string currPageNo)
        {
            List<Entity.SalesChallan> lstEntity = new List<Entity.SalesChallan>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesChallanMgmt.GetSalesChallanList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesChallanDetailSearch(string SearchKey)
        {
            List<Entity.SalesChallan> lstEntity = new List<Entity.SalesChallan>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesChallanMgmt.GetSalesChallanList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPurchaseBillDetail(string currPageNo)
        {
            List<Entity.PurchaseBill> lstEntity = new List<Entity.PurchaseBill>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PurchaseBillMgmt.GetPurchaseBillList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPurchaseBillDetailSearch(string SearchKey)
        {
            List<Entity.PurchaseBill> lstEntity = new List<Entity.PurchaseBill>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PurchaseBillMgmt.GetPurchaseBillList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webproductspecificationDetail()
        {
            List<Entity.ProductSpecification> lstEntity = new List<Entity.ProductSpecification>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductSpecificationMgmt.GetProductSpecificationList();
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webConstantDetail()
        {
            List<Entity.Constant> lstEntity = new List<Entity.Constant>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ConstantMgmt.GetConstantList();
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getResponseTeamLocation(Int64 pEmployeeID, DateTime pDate1, DateTime pDate2)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // ----------- Below declaration for ResponseLocation Team Data
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            // ------------------------------------------------------------------------------------------------
            // Adding Response Vehicle (Not Static from OrgStructure ... Whose Latitude & Logitude is blank
            // ------------------------------------------------------------------------------------------------
            List<Entity.OrganizationStructure> lstOrgStruc = new List<Entity.OrganizationStructure>();
            lstOrgStruc = BAL.OrganizationStructureMgmt.GetEmployeeLocation(pEmployeeID, pDate1, pDate2);
            // ------------------------------------------------------------------------------------------------
            foreach (Entity.OrganizationStructure point in lstOrgStruc)
            {
                row = new Dictionary<string, object>();
                row.Add("CustomerName", point.CustomerName);
                row.Add("EmployeeName", point.EmployeeName);
                row.Add("InquiryNo", point.InquiryNo);
                row.Add("FollowUpDate", point.FollowUpDate);
                row.Add("Latitude", point.Latitude);
                row.Add("Longitude", point.Longitude);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOtherChargeDetail(string currPageNo)
        {
            List<Entity.OtherCharge> lstEntity = new List<Entity.OtherCharge>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OtherChargeMgmt.GetOtherChargeList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOtherChargeDetailSearch(string SearchKey)
        {
            List<Entity.OtherCharge> lstEntity = new List<Entity.OtherCharge>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OtherChargeMgmt.GetOtherChargeList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webBundleDetail()
        {
            List<Entity.Bundle> lstEntity = new List<Entity.Bundle>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.BundleMgmt.GetBundleList();
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCampaignTemplateDetail(string Category, string currPageNo)
        {
            List<Entity.CampaignTemplate> lstEntity = new List<Entity.CampaignTemplate>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CampaignTemplateMgmt.GetCampaignTemplate(0, Category, "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCampaignTemplateDetailSearch(string Category, string SearchKey)
        {
            List<Entity.CampaignTemplate> lstEntity = new List<Entity.CampaignTemplate>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CampaignTemplateMgmt.GetCampaignTemplate(0, Category, SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webWalletDetail(string currPageNo)
        {
            List<Entity.Wallet> lstEntity = new List<Entity.Wallet>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.WalletMgmt.GetWallet(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webWalletDetailSearch(string SearchKey)
        {
            List<Entity.Wallet> lstEntity = new List<Entity.Wallet>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.WalletMgmt.GetWallet(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //-----------------Location---------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLocation(string currPageNo)
        {
            List<Entity.Location> lstEntity = new List<Entity.Location>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LocationMgmt.GetLocation(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webLocationSearch(string SearchKey)
        {
            List<Entity.Location> lstEntity = new List<Entity.Location>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.LocationMgmt.GetLocation(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //-----------------Color---------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webColor(string currPageNo)
        {
            List<Entity.Color> lstEntity = new List<Entity.Color>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ColorMgmt.GetColor(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webColorSearch(string SearchKey)
        {
            List<Entity.Color> lstEntity = new List<Entity.Color>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ColorMgmt.GetColor(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //-----------------Grade---------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webGrade(string currPageNo)
        {
            List<Entity.Grade> lstEntity = new List<Entity.Grade>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.GradeMgmt.GetGrade(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webGradeSearch(string SearchKey)
        {
            List<Entity.Grade> lstEntity = new List<Entity.Grade>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.GradeMgmt.GetGrade(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //------------------------Batch-----------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webBatch(string currPageNo)
        {
            List<Entity.Batch> lstEntity = new List<Entity.Batch>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.BatchMgmt.GetBatch(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webBatchSearch(string SearchKey)
        {
            List<Entity.Batch> lstEntity = new List<Entity.Batch>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.BatchMgmt.GetBatch(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //------------------------Size-----------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSize(string currPageNo)
        {
            List<Entity.Size> lstEntity = new List<Entity.Size>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SizeMgmt.GetSize(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSizeSearch(string SearchKey)
        {
            List<Entity.Size> lstEntity = new List<Entity.Size>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SizeMgmt.GetSize(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //------------------------Shade-----------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webShade(string currPageNo)
        {
            List<Entity.Shade> lstEntity = new List<Entity.Shade>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ShadeMgmt.GetShade(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webShadeSearch(string SearchKey)
        {
            List<Entity.Shade> lstEntity = new List<Entity.Shade>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ShadeMgmt.GetShade(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //------------------------Punch-----------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPunch(string currPageNo)
        {
            List<Entity.Punch> lstEntity = new List<Entity.Punch>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PunchMgmt.GetPunch(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPunchSearch(string SearchKey)
        {
            List<Entity.Punch> lstEntity = new List<Entity.Punch>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PunchMgmt.GetPunch(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //------------------------Nature Of Call-----------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webNatureOfCall(string currPageNo)
        {
            List<Entity.NatureCall> lstEntity = new List<Entity.NatureCall>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.NatureOfCallMgmt.GetNatureCallList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webNatureOfCallSearch(string SearchKey)
        {
            List<Entity.NatureCall> lstEntity = new List<Entity.NatureCall>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.NatureOfCallMgmt.GetNatureCallList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        //-----------------------------Bank Details------------------------------//
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOrganizationBankDetail(string currPageNo)
        {
            List<Entity.OrganizationBankInfo> lstEntity = new List<Entity.OrganizationBankInfo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CommonMgmt.GetBankInfo(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOrganizationBankDetailSearch(string SearchKey)
        {
            List<Entity.OrganizationBankInfo> lstEntity = new List<Entity.OrganizationBankInfo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CommonMgmt.GetBankInfo(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        //-----------------------------Bank Details------------------------------//
        //-----------------Bank Details---------------------//


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webExpenseTypeDetail(string currPageNo)
        {
            List<Entity.ExpenseType> lstEntity = new List<Entity.ExpenseType>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExpenseTypeMgmt.GetExpenseTypeList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webExpenseTypeDetailSearch(string SearchKey)
        {
            List<Entity.ExpenseType> lstEntity = new List<Entity.ExpenseType>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExpenseTypeMgmt.GetExpenseTypeList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webExpenseDetail(string currPageNo)
        {
            List<Entity.Expense> lstEntity = new List<Entity.Expense>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExpenseMgmt.GetExpenseList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webExpenseDetailSearch(string SearchKey)
        {
            List<Entity.Expense> lstEntity = new List<Entity.Expense>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExpenseMgmt.GetExpenseList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMultiExpense(string currPageNo)
        {
            List<Entity.OfficeExpense> lstEntity = new List<Entity.OfficeExpense>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExpenseMgmt.GetMultiExpenseList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMultiExpenseSearch(string SearchKey)
        {
            List<Entity.OfficeExpense> lstEntity = new List<Entity.OfficeExpense>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExpenseMgmt.GetMultiExpenseList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webEmailTemplate(string currPageNo)
        {
            List<Entity.EmailTemplate> lstEntity = new List<Entity.EmailTemplate>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.EmailTemplateMgmt.GetEmailTemplate("", "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webEmailTemplateSearch(string SearchKey)
        {
            List<Entity.EmailTemplate> lstEntity = new List<Entity.EmailTemplate>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.EmailTemplateMgmt.GetEmailTemplateList();
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        //----------------------------------------------------------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webGenTemplate(string currPageNo)
        {
            List<Entity.EmailTemplate> lstEntity = new List<Entity.EmailTemplate>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.EmailTemplateMgmt.GetGeneralTemplate(0, "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webGenTemplateSearch(string SearchKey)
        {
            List<Entity.EmailTemplate> lstEntity = new List<Entity.EmailTemplate>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.EmailTemplateMgmt.GetGeneralTemplate(0, Session["LoginUserID"].ToString(), SearchKey, 1, Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webHolidayDetail(string currPageNo)
        {
            List<Entity.Holiday> lstEntity = new List<Entity.Holiday>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.HolidayMgmt.GetHolidayList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webHolidayDetailSearch(string SearchKey)
        {
            List<Entity.Holiday> lstEntity = new List<Entity.Holiday>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.HolidayMgmt.GetHolidayList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webTODOCategoryDetail(string currPageNo)
        {
            List<Entity.ToDoCategory> lstEntity = new List<Entity.ToDoCategory>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ToDoCategoryMgmt.GetTODOCategoryList(0,Session["LoginUserID"].ToString(),"", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webTODOCategoryDetailSearch(string SearchKey)
        {
            List<Entity.ToDoCategory> lstEntity = new List<Entity.ToDoCategory>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ToDoCategoryMgmt.GetTODOCategoryList(0,Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string webComplaintList(string ComplaintStatus, string currPageNo)
        {
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(0, 0, ComplaintStatus, 0, 0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string webComplaintListSearch(string ComplaintStatus,string SearchKey)
        {
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(0, 0, ComplaintStatus, 0, 0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string webComplaintVisitList(string currPageNo)
        {
            List<Entity.ComplaintVisit> lstEntity = new List<Entity.ComplaintVisit>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ComplaintMgmt.GetComplaintVisitList(0, 0, 0, 0, "", "", Session["LoginUserID"].ToString());
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string webComplaintVisitListSearch(string SearchKey)
        {
            List<Entity.ComplaintVisit> lstEntity = new List<Entity.ComplaintVisit>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ComplaintMgmt.GetComplaintVisitList(0, 0, 0, 0, "", SearchKey, Session["LoginUserID"].ToString());
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string webExternalLeadList(string source, string acid, string cat, string currPageNo)
        {
            List<Entity.ExternalLeads> lstEntity = new List<Entity.ExternalLeads>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExternalLeadsMgmt.GetExternalLeadListByStatus(0, acid, cat, source, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string webExternalLeadView(string status, string source, Int64 month, Int64 year, string userid)
        {
            List<Entity.ExternalLeads> lstEntity = new List<Entity.ExternalLeads>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExternalLeadsMgmt.GetExternalLeadView(status, source, month, year, userid, 1, 50000, out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string webExternalLeadListSearch(string source, string acid, string cat, string SearchKey)
        {
            List<Entity.ExternalLeads> lstEntity = new List<Entity.ExternalLeads>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ExternalLeadsMgmt.GetExternalLeadListByStatus(0, acid, cat, source, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webContractInfoDetailByStatus(string currPageNo)
        {
            List<Entity.ContractInfo> lstEntity = new List<Entity.ContractInfo>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ContractInfoMgmt.GetContractInfoList(0, Session["LoginUserID"].ToString(), "", "", 0, 0, Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webContractInfoDetailByStatusSearch(string SearchKey)
        {
            List<Entity.ContractInfo> lstEntity = new List<Entity.ContractInfo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ContractInfoMgmt.GetContractInfoList(0, Session["LoginUserID"].ToString(), SearchKey, "", 0, 0, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webVisitorInfoDetailByStatus(string currPageNo)
        {
            List<Entity.VisitorInfo> lstEntity = new List<Entity.VisitorInfo>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VisitorInfoMgmt.GetVisitorInfoList(0, Session["LoginUserID"].ToString(), "", "", 0, 0, Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webVisitorInfoDetailByStatusSearch(string SearchKey)
        {
            List<Entity.VisitorInfo> lstEntity = new List<Entity.VisitorInfo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.VisitorInfoMgmt.GetVisitorInfoList(0, Session["LoginUserID"].ToString(), SearchKey, "", 0, 0, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCourierInfoDetailByStatus(string currPageNo)
        {
            List<Entity.CourierInfo> lstEntity = new List<Entity.CourierInfo>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CourierInfoMgmt.GetCourierInfoList(0, Session["LoginUserID"].ToString(), "", 0, 0, Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);

            return serializer.Serialize(lstEntity);
        }

        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCourierInfoDetailByStatusSearch(string SearchKey)
        {
            List<Entity.CourierInfo> lstEntity = new List<Entity.CourierInfo>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CourierInfoMgmt.GetCourierInfoList(0, Session["LoginUserID"].ToString(), SearchKey, 0, 0, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);

            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesBillByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.SalesBill> lstEntity = new List<Entity.SalesBill>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesBillMgmt.GetSalesBillList(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webSalesBillByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            List<Entity.SalesBill> lstEntity = new List<Entity.SalesBill>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.SalesBillMgmt.GetSalesBillListPeriod(pLoginUserID, FromDate, ToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPurchaseBillByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.PurchaseBill> lstEntity = new List<Entity.PurchaseBill>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PurchaseBillMgmt.GetPurchaseBillList(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webPurchaseBillByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            List<Entity.PurchaseBill> lstEntity = new List<Entity.PurchaseBill>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.PurchaseBillMgmt.GetPurchaseBillListPeriod(pLoginUserID, FromDate, ToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInwardByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.Inward> lstEntity = new List<Entity.Inward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InwardMgmt.GetInwardList(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInwardByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            List<Entity.Inward> lstEntity = new List<Entity.Inward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InwardMgmt.GetInwardListPeriod(pLoginUserID, FromDate, ToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        //Created by Vikram Rajput
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOutwardByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.Outward> lstEntity = new List<Entity.Outward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OutwardMgmt.GetOutwardList(pLoginUserID, pMonth, pYear);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webOutwardByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            List<Entity.Outward> lstEntity = new List<Entity.Outward>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.OutwardMgmt.GetOutwardListPeriod(pLoginUserID, FromDate, ToDate);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string webChatBoxList(string pFrom, string pTo)
        {
            List<Entity.Chat> lstEntity = new List<Entity.Chat>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CommonMgmt.GetChatBoxList(pFrom, pTo);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webProductOpeningDetail(string currPageNo)
        {
            List<Entity.ProductOpeningStk> lstEntity = new List<Entity.ProductOpeningStk>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            Session["PageNo"] = currPageNo;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductMgmt.GetProductOpeningStockList(0, Session["LoginUserID"].ToString(), "01/04/2020-31/03/2021", "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec); ;
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webShiftMaster(string currPageNo)
        {
            List<Entity.ShiftMaster> lstEntity = new List<Entity.ShiftMaster>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ShiftMasterMgmt.GetShiftMaster(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webShiftMasterSearch(string SearchKey)
        {
            List<Entity.ShiftMaster> lstEntity = new List<Entity.ShiftMaster>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ShiftMasterMgmt.GetShiftMaster(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCheckList(string currPageNo)
        {
            List<Entity.CheckList> lstEntity = new List<Entity.CheckList>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CheckListMgmt.GetCheckList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCheckListSearch(string SearchKey)
        {
            List<Entity.CheckList> lstEntity = new List<Entity.CheckList>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CheckListMgmt.GetCheckList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        // -------------------------------------------------------------------------
        // For Solar Customer : Material Movement 
        // -------------------------------------------------------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMaterialMovementInward(string currPageNo)
        {
            List<Entity.Material_Movement> lstEntity = new List<Entity.Material_Movement>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.Material_MovementMgmt.GetMaterial_Movement(0, Session["LoginUserID"].ToString(), "", "IN", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMaterialMovementInwardSearch(string SearchKey)
        {
            List<Entity.Material_Movement> lstEntity = new List<Entity.Material_Movement>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.Material_MovementMgmt.GetMaterial_Movement(0, Session["LoginUserID"].ToString(), SearchKey, "IN", Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMaterialMovementOutward(string currPageNo)
        {
            List<Entity.Material_Movement> lstEntity = new List<Entity.Material_Movement>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.Material_MovementMgmt.GetMaterial_Movement(0, Session["LoginUserID"].ToString(), "", "OUT", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMaterialMovementOutwardSearch(string SearchKey)
        {
            List<Entity.Material_Movement> lstEntity = new List<Entity.Material_Movement>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.Material_MovementMgmt.GetMaterial_Movement(0, Session["LoginUserID"].ToString(), SearchKey, "OUT", Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }


        // -------------------------------------------------------------------------
        // Job Work Movement 
        // -------------------------------------------------------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobWorkMovementInward(string currPageNo)
        {
            List<Entity.JobWork_Movement> lstEntity = new List<Entity.JobWork_Movement>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobWork_MovementMgmt.GetJobWork_Movement(0, Session["LoginUserID"].ToString(), "", "IN", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobWorkMovementInwardSearch(string SearchKey)
        {
            List<Entity.JobWork_Movement> lstEntity = new List<Entity.JobWork_Movement>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobWork_MovementMgmt.GetJobWork_Movement(0, Session["LoginUserID"].ToString(), SearchKey, "IN", Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobWorkMovementOutward(string currPageNo)
        {
            List<Entity.JobWork_Movement> lstEntity = new List<Entity.JobWork_Movement>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobWork_MovementMgmt.GetJobWork_Movement(0, Session["LoginUserID"].ToString(), "", "OUT", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webJobWorkMovementOutwardSearch(string SearchKey)
        {
            List<Entity.JobWork_Movement> lstEntity = new List<Entity.JobWork_Movement>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.JobWork_MovementMgmt.GetJobWork_Movement(0, Session["LoginUserID"].ToString(), SearchKey, "OUT", Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        // -------------------------------------------------------------------------
        // For Solar Customer : Material Consumption
        // -------------------------------------------------------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMaterialConsumption(string currPageNo)
        {
            List<Entity.Material_Cons> lstEntity = new List<Entity.Material_Cons>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.Material_ConsMgmt.GetMaterial_Cons(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webMaterialConsumptionSearch(string SearchKey)
        {
            List<Entity.Material_Cons> lstEntity = new List<Entity.Material_Cons>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.Material_ConsMgmt.GetMaterial_Cons(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }

        // -------------------------------------------------------------------------
        // For Solar Customer : Material Consumption
        // -------------------------------------------------------------------------
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInspectionList(string currPageNo)
        {
            List<Entity.Inspection> lstEntity = new List<Entity.Inspection>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InspectionMgmt.GetInspectionList(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webInspectionListSearch(string SearchKey)
        {
            List<Entity.Inspection> lstEntity = new List<Entity.Inspection>();
            //// --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InspectionMgmt.GetInspectionList(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDocumentType(string currPageNo)
        {
            List<Entity.Document_Type> lstEntity = new List<Entity.Document_Type>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.Document_TypeMgmt.GetDocument_Type(0, Session["LoginUserID"].ToString(), "", Convert.ToInt32(currPageNo), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDocumentTypeSearch(string SearchKey)
        {
            List<Entity.Document_Type> lstEntity = new List<Entity.Document_Type>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.Document_TypeMgmt.GetDocument_Type(0, Session["LoginUserID"].ToString(), SearchKey, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out totrec);
            Session["ReturnTotalRecord"] = totrec.ToString();
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webAssemblyStockSummary(string Status, string OrderNo)
        {
            List<Entity.ProductAssemblyStock> lstEntity = new List<Entity.ProductAssemblyStock>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductMgmt.GetAssemblyStockSummary(Status, OrderNo);
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webAssemblyStockSummaryProductWise(string Status, string ProductID, string Quantity)
        {
            List<Entity.ProductAssemblyStock> lstEntity = new List<Entity.ProductAssemblyStock>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.ProductMgmt.GetAssemblyStockSummaryProductWise(Status, Convert.ToInt64(string.IsNullOrEmpty(ProductID) ? 0 : Convert.ToInt64(ProductID)), Convert.ToDouble(string.IsNullOrEmpty(Quantity) ? 1 : Convert.ToInt64(Quantity)));
            return serializer.Serialize(lstEntity);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public void setGridPageSize(Int64 pSize)
        {
            Session["PageSize"] = Convert.ToInt64(pSize);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webDashboardDailySummary(string xType, Int64 xMonth, Int64 xYear, string xLoginUserID)
        {
            List<Entity.CRMSummary> lstEntity = new List<Entity.CRMSummary>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.CommonMgmt.GetDashboardDailySummary(xType, xMonth, xYear, xLoginUserID);
            return serializer.Serialize(lstEntity);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public string webCrmAnalysis(string pType, Int64 pMonth, Int64 pYear)
        {
            List<Entity.CRMSummary> lstEntity = new List<Entity.CRMSummary>();
            // --------------------------------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------------------------------------------------
            lstEntity = BAL.InquiryInfoMgmt.GetCrmAnalysisReport(pType, pMonth, pYear);
            return serializer.Serialize(lstEntity);
        }
    }
}
