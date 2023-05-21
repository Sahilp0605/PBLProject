
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
namespace StarsProject
{
    public partial class DashboardLead : System.Web.UI.Page 
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnLoginUserID.Value = Session["LoginUserID"].ToString();
                hdnWelcomeGreet.Value = BAL.CommonMgmt.GetConstant("WelcomeGreet", 0, 1);
                string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                hdnSerialKey.Value = HttpContext.Current.Session["SerialKey"].ToString();
                // ----------------------------------------
                BindMonthYear();
                BindEmployee();
                BindInquiryStatus();
                // ----------------------------------------
                BindInquirySummary();
                BindExternalSummary();
                BindTeleCallerSummary();
                //ReloadLeadData();
                //ReloadExternalData();
                //ReloadTeleCallerData();
            }
            // ------------------------------------
            
        }

        // --------------------------------------------------------
        // General Bindings 
        // --------------------------------------------------------
        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpLeadMonth.Items.Add(new ListItem("-- All --", "0"));
            drpLeadMonth.Items.Add(new ListItem("January", "1"));
            drpLeadMonth.Items.Add(new ListItem("February", "2"));
            drpLeadMonth.Items.Add(new ListItem("March", "3"));
            drpLeadMonth.Items.Add(new ListItem("April", "4"));
            drpLeadMonth.Items.Add(new ListItem("May", "5"));
            drpLeadMonth.Items.Add(new ListItem("June", "6"));
            drpLeadMonth.Items.Add(new ListItem("July", "7"));
            drpLeadMonth.Items.Add(new ListItem("August", "8"));
            drpLeadMonth.Items.Add(new ListItem("September", "9"));
            drpLeadMonth.Items.Add(new ListItem("October", "10"));
            drpLeadMonth.Items.Add(new ListItem("November", "11"));
            drpLeadMonth.Items.Add(new ListItem("December", "12"));

            drpExtMonth.Items.Add(new ListItem("-- All --", "0"));
            drpExtMonth.Items.Add(new ListItem("January", "1"));
            drpExtMonth.Items.Add(new ListItem("February", "2"));
            drpExtMonth.Items.Add(new ListItem("March", "3"));
            drpExtMonth.Items.Add(new ListItem("April", "4"));
            drpExtMonth.Items.Add(new ListItem("May", "5"));
            drpExtMonth.Items.Add(new ListItem("June", "6"));
            drpExtMonth.Items.Add(new ListItem("July", "7"));
            drpExtMonth.Items.Add(new ListItem("August", "8"));
            drpExtMonth.Items.Add(new ListItem("September", "9"));
            drpExtMonth.Items.Add(new ListItem("October", "10"));
            drpExtMonth.Items.Add(new ListItem("November", "11"));
            drpExtMonth.Items.Add(new ListItem("December", "12"));

            drpTeleMonth.Items.Add(new ListItem("-- All --", "0"));
            drpTeleMonth.Items.Add(new ListItem("January", "1"));
            drpTeleMonth.Items.Add(new ListItem("February", "2"));
            drpTeleMonth.Items.Add(new ListItem("March", "3"));
            drpTeleMonth.Items.Add(new ListItem("April", "4"));
            drpTeleMonth.Items.Add(new ListItem("May", "5"));
            drpTeleMonth.Items.Add(new ListItem("June", "6"));
            drpTeleMonth.Items.Add(new ListItem("July", "7"));
            drpTeleMonth.Items.Add(new ListItem("August", "8"));
            drpTeleMonth.Items.Add(new ListItem("September", "9"));
            drpTeleMonth.Items.Add(new ListItem("October", "10"));
            drpTeleMonth.Items.Add(new ListItem("November", "11"));
            drpTeleMonth.Items.Add(new ListItem("December", "12"));
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpLeadYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                drpExtYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                drpTeleYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            drpLeadYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpExtYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpTeleYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));

            drpLeadYear.SelectedValue = DateTime.Now.Year.ToString();
            drpExtYear.SelectedValue = DateTime.Now.Year.ToString();
            drpTeleYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        public void BindEmployee()
        {
            // ---------------- Employee List  -------------------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(Session["LoginUserID"].ToString());
            drpLeadEmployee.DataSource = lstEmployee;
            drpLeadEmployee.DataValueField = "LoginUserID";
            drpLeadEmployee.DataTextField = "EmployeeName";
            drpLeadEmployee.DataBind();
            if (Session["LoginUserID"].ToString().ToLower() == "admin" || Session["LoginUserID"].ToString().ToLower() == "bradmin")
                drpLeadEmployee.Items.Insert(0, new ListItem("-- All --", "admin"));
            drpLeadEmployee.SelectedValue = Session["LoginUserID"].ToString();

            drpExtEmployee.DataSource = lstEmployee;
            drpExtEmployee.DataValueField = "LoginUserID";
            drpExtEmployee.DataTextField = "EmployeeName";
            drpExtEmployee.DataBind();
            if (Session["LoginUserID"].ToString().ToLower() == "admin" || Session["LoginUserID"].ToString().ToLower() == "bradmin")
                drpExtEmployee.Items.Insert(0, new ListItem("-- All --", "admin"));
            drpExtEmployee.SelectedValue = Session["LoginUserID"].ToString();

            drpTeleEmployee.DataSource = lstEmployee;
            drpTeleEmployee.DataValueField = "LoginUserID";
            drpTeleEmployee.DataTextField = "EmployeeName";
            drpTeleEmployee.DataBind();
            if (Session["LoginUserID"].ToString().ToLower() == "admin" || Session["LoginUserID"].ToString().ToLower() == "bradmin")
                drpTeleEmployee.Items.Insert(0, new ListItem("-- All --", "admin"));
            drpTeleEmployee.SelectedValue = Session["LoginUserID"].ToString();
        }

        // --------------------------------------------------------
        // Lead Section - PostBack
        // --------------------------------------------------------
        public void BindInquiryStatus()
        {
            
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("Inquiry");
            drpInqCat.DataSource = lstDesig;
            drpInqCat.DataValueField = "InquiryStatusName";
            drpInqCat.DataTextField = "InquiryStatusName";
            drpInqCat.DataBind();
        }

        public void BindInquirySummary()
        {
            List<Entity.DashboardInquirySummary> lstEntity = new List<Entity.DashboardInquirySummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardInquiryStatusSummary(drpLeadEmployee.SelectedValue, Convert.ToInt64(drpLeadMonth.SelectedValue), Convert.ToInt64(drpLeadYear.SelectedValue));
            spnLost.InnerText = lstEntity[0].CloseLost.ToString();
            spnSuccess.InnerText = lstEntity[0].CloseSuccess.ToString();
            spnUnknown.InnerText = lstEntity[0].Unknown.ToString();
        }

        protected void drpLeadMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadLeadData();
        }

        protected void drpInqCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadLeadData();
        }

        protected void drpAnalysis_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadLeadData();
        }

        public void ReloadLeadData()
        {
            String selValue = "", selAnalysis = "";
            selValue = drpInqCat.SelectedValue;
            selAnalysis = drpAnalysis.SelectedValue;
            BindInquirySummary();
            ScriptManager.RegisterStartupScript(this, typeof(string), "lead1", "javascript:initInqGraphLayout('Inquiry');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "lead2", "javascript:loadInquiryStatusList('" + selValue + "');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "lead3", "javascript:initInqDonutGraphLayout('InquirySource');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "lead4", "javascript:initInqDonutGraphLayout('DisQuali');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "lead5", "javascript:loadAnalysisReport('" + selAnalysis + "');", true);
        }

        // --------------------------------------------------------
        // External Lead Section 
        // --------------------------------------------------------
        public void BindExternalSummary()
        {
            List<Entity.DashboardCountSummary> lstTele = new List<Entity.DashboardCountSummary>();
            lstTele = BAL.CommonMgmt.GetDashboardExternalSummary(drpExtEmployee.SelectedValue, Convert.ToInt64(drpExtMonth.SelectedValue), Convert.ToInt64(drpExtYear.SelectedValue), "external");
            Decimal totQua = lstTele.Sum(item => item.value1);
            Decimal totDis = lstTele.Sum(item => item.value2);
            Decimal totPrc = lstTele.Sum(item => item.value3);
            Decimal totOth = lstTele.Sum(item => item.value4);
            spnExtUntouch.InnerText = totOth.ToString();
            spnExtProcess.InnerText = totPrc.ToString();
            spnExtQualify.InnerText = totQua.ToString();
            spnExtDisqualify.InnerText = totDis.ToString();
            
            
        }
        protected void drpExtMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadExternalData();
        }

        public void ReloadExternalData()
        {
            BindExternalSummary();
            ScriptManager.RegisterStartupScript(this, typeof(string), "lay", "javascript:initExternalGraph('IndiaMart');", true);
        }

        // --------------------------------------------------------
        // Tele Caller Section 
        // --------------------------------------------------------
        public void BindTeleCallerSummary()
        {
            List<Entity.DashboardCountSummary> lstTele = new List<Entity.DashboardCountSummary>();
            lstTele = BAL.CommonMgmt.GetDashboardExternalSummary(drpTeleEmployee.SelectedValue, Convert.ToInt64(drpTeleMonth.SelectedValue), Convert.ToInt64(drpTeleYear.SelectedValue), "telecaller");
            Decimal totQua = lstTele.Sum(item => item.value1);
            Decimal totDis = lstTele.Sum(item => item.value2);
            Decimal totPrc = lstTele.Sum(item => item.value3);
            Decimal totOth = lstTele.Sum(item => item.value4);
            spnTeleQualify.InnerText = totQua.ToString();
            spnTeleDisqualify.InnerText = totDis.ToString();
            spnTeleUntouch.InnerText = totOth.ToString();
            spnTeleProcess.InnerText = totPrc.ToString();
        }
        protected void drpTeleMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadTeleCallerData();
        }
        public void ReloadTeleCallerData()
        {
            BindTeleCallerSummary();
            ScriptManager.RegisterStartupScript(this, typeof(string), "tele1", "javascript:initInqGraphLayout('TeleCallStatus');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "tele2", "javascript:initTeleDonutGraphLayout('TeleDisQuali');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "tele3", "javascript:initTeleDonutGraphLayout('TeleConversion');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "tele4", "javascript:initTeleEntryGraphLayout('telecaller', 'monthly');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "tele5", "javascript:initTeleEntryGraphLayout('telecaller', 'daily');", true);
        }

        protected void btnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            string exportFile = "crmdonwloadedfile";
            DataTable dt = new DataTable();
            if (!String.IsNullOrEmpty(drpLeadEmployee.SelectedValue))
                dt = BAL.CommonMgmt.GetExportDataList("userinquiry", drpLeadEmployee.SelectedValue, drpInqCat.SelectedValue);
            else
                dt = BAL.CommonMgmt.GetExportDataList("userinquiry", Session["LoginUserID"].ToString(), drpInqCat.SelectedValue);
            // -------------------------------------------------
            ExportDataTableToExcel(dt, exportFile);
            // -------------------------------------------------
            if (File.Exists(Server.MapPath(exportFile + ".xlsx")))
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + exportFile + ".xlsx");
                Response.AddHeader("Content-Type", "application/Excel");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.WriteFile(Server.MapPath(exportFile + ".xlsx"));
                Response.End();
            }
        }

        [WebMethod]
        private void ExportDataTableToExcel(DataTable table, string filename)
        {
            using (var workbook = SpreadsheetDocument.Create(Server.MapPath(filename + ".xlsx"), DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();

                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                uint sheetId = 1;
                if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                sheet.Name = "Sheet1";
                sheets.Append(sheet);
                DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                List<String> columns = new List<string>();
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);
                foreach (System.Data.DataRow dsrow in table.Rows)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    foreach (String col in columns)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }
                workbook.Save();
            }
        }

    }
}