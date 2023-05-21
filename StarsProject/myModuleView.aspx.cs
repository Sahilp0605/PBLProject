using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace StarsProject
{
    public partial class myModuleView : System.Web.UI.Page
    {
        
        public string setCurrPageNo(string pgno)
        {
            HttpContext.Current.Session["PageNo"] = pgno;
            return pgno;
        }

        [WebMethod(EnableSession = true)]
        public static string getCurrPageNo()
        {
            return HttpContext.Current.Session["PageNo"].ToString();
        }

        [WebMethod(EnableSession = true)]
        public string getSessionObject()
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            serializer.MaxJsonLength = Int32.MaxValue;
            string tmpVal = Session["ReturnTotalRecord"].ToString();
            try
            {
                row = new Dictionary<string, object>();
                row.Add("label", "pageno");
                row.Add("value", tmpVal);
                rows.Add(row);
            }
            catch (Exception ex)
            {
                throw;
            }
            return serializer.Serialize(rows);
        }
        protected void drpPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PageSize"] = Convert.ToInt64(drpPageSize.SelectedValue);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageNo"] = 1;
            Session["PageSize"] = (!String.IsNullOrEmpty(drpPageSize.SelectedValue)) ? Convert.ToInt64(drpPageSize.SelectedValue) : 10;

            if (!IsPostBack)
            {
                hdnSerialKey.Value = Session["SerialKey"].ToString();
                hdnLoginUserID.Value = Session["LoginUserID"].ToString();
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                hdnLoginUserRole.Value = objAuth.RoleCode.ToLower();

                // ---------------------------------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["view"]))
                    hdnView.Value = Request.QueryString["view"].ToString().Trim();

                if (!String.IsNullOrEmpty(Request.QueryString["keyID"]))
                    hdnKeyID.Value = Request.QueryString["keyID"].ToString().Trim();

                if (!String.IsNullOrEmpty(Request.QueryString["Title"]))
                {
                    hdnTitle.Value = Request.QueryString["Title"].ToString().Trim();
                    spnPageHeader.InnerHtml = hdnTitle.Value;
                }
                if (!String.IsNullOrEmpty(Request.QueryString["Action"]))
                    hdnActionFlag.Value = Request.QueryString["Action"].ToString().Trim();

                if (!String.IsNullOrEmpty(Request.QueryString["MenuID"]))
                    hdnMenuID.Value = Request.QueryString["MenuID"].ToString().Trim();
                else
                    hdnMenuID.Value = "0";
                // ---------------------------------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["para1"]))
                    hdnPara1.Value = Request.QueryString["para1"].ToString().Trim();

                if (!String.IsNullOrEmpty(Request.QueryString["para2"]))
                    hdnPara2.Value = Request.QueryString["para2"].ToString().Trim();

                if (!String.IsNullOrEmpty(Request.QueryString["para3"]))
                    hdnPara3.Value = Request.QueryString["para3"].ToString().Trim();

                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("OutwardVersion", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                    hdnOutwardVersion.Value = BAL.CommonMgmt.GetConstant("OutwardVersion", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("QuotationVersion", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                    hdnQuotationVersion.Value = BAL.CommonMgmt.GetConstant("QuotationVersion", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("SalesOrderVersion", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                    hdnSalesOrderVersion.Value = BAL.CommonMgmt.GetConstant("SalesOrderVersion", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                hdnClientERPIntegration.Value = BAL.CommonMgmt.GetConstant("ClientERPIntegration", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("InquiryShare", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                    hdnInquiryShare.Value = BAL.CommonMgmt.GetConstant("InquiryShare", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("InquiryAssign", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                    hdnInquiryAssign.Value = BAL.CommonMgmt.GetConstant("InquiryAssign", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("InquiryShareRoles", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                    hdnInquiryShareRoles.Value = BAL.CommonMgmt.GetConstant("InquiryShareRoles", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("popupPrintHeader", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                {
                    hdnpopupPrintHeader.Value = BAL.CommonMgmt.GetConstant("popupPrintHeader", 0, Convert.ToInt64(Session["CompanyID"].ToString()));
                    hdnpopupPrintHeader.Value = hdnpopupPrintHeader.Value.ToLower();
                }

                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("ConversationLog", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                    hdnConversationLog.Value = BAL.CommonMgmt.GetConstant("ConversationLog", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                // -----------------------------------------------------------------
                //  Add / Edit / Delete Flag 
                // -----------------------------------------------------------------
                List<Entity.ApplicationMenu> lstMenu = new List<Entity.ApplicationMenu>();
                lstMenu = BAL.CommonMgmt.GetMenuAddEditDelList(Convert.ToInt64(hdnMenuID.Value), Session["LoginUserID"].ToString());
                hdnAddFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].AddFlag.ToString().ToLower() : "true";
                hdnEditFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].EditFlag.ToString().ToLower() : "true";
                hdnDelFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].DelFlag.ToString().ToLower() : "true";

                // -----------------------------------------------------------------
                drpExternalLeadAc.Visible = (hdnView.Value.ToLower() == "externalleads") ? true : false;
                lblExternalLeadAc.Visible = (hdnView.Value.ToLower() == "externalleads") ? true : false;
                drpExternalLeadCat.Visible = (hdnView.Value.ToLower() == "externalleads" || hdnView.Value.ToLower() == "telecaller") ? true : false;
                lblExternalLeadCat.Visible = (hdnView.Value.ToLower() == "externalleads" || hdnView.Value.ToLower() == "telecaller") ? true : false;
                drpComplainStatus.Visible = (hdnView.Value.ToLower() == "complaint" ) ? true : false;
                lblComplainStatus.Visible = (hdnView.Value.ToLower() == "complaint" ) ? true : false;

                if (hdnView.Value.ToLower() == "externalleads")
                {

                    string Qry="";
                    Qry = "select IndiaMartAcAlias,IndiaMartKey from TBL_DBConfig where SerialKey='" + Session["SerialKey"].ToString() + "' and isnull(indiamartkey,'')<>''" +
                          " union all " +
                          " select IndiaMartAcAlias2,IndiaMartKey2 from TBL_DBConfig where SerialKey='" + Session["SerialKey"].ToString() + "' and isnull(indiamartkey2,'')<>''";

                    //string constr = ConfigurationManager.ConnectionStrings["RegConfigConnection"].ConnectionString;
                    //using (SqlConnection con = new SqlConnection(constr))
                    //{
                    //    using (SqlCommand cmd = new SqlCommand(Qry))
                    //    {
                    //        using (SqlDataAdapter sda = new SqlDataAdapter())
                    //        {
                    //            cmd.Connection = con;
                    //            sda.SelectCommand = cmd;
                    //            using (DataTable dt = new DataTable())
                    //            {
                    //                sda.Fill(dt);

                    //                if(dt.Rows.Count > 0)
                    //                {
                    //                    drpExternalLeadAc.DataSource = dt;
                    //                    drpExternalLeadAc.DataValueField = "IndiaMartKey";
                    //                    drpExternalLeadAc.DataTextField = "IndiaMartAcAlias";
                    //                    drpExternalLeadAc.DataBind();
                    //                    //drpExternalLeadAc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                    //                }
                                    
                    //            }
                    //        }
                    //    }
                    //    con.Close();
                    //    con.Dispose();
                    //}
                }
            }
            // ---------------------------------------------
            divTaxSummary.Visible = false;
            divSearchArea.Visible = false;
            if (hdnView.Value=="quotation" || hdnView.Value == "purchasebill" || hdnView.Value == "salesbill")
            {
                
                divSearchArea.Visible = false;
                //divTaxSummary.Visible = true;
                //myTaxSummary.BindTaxSummaryWidget(hdnView.Value, "", true, null, null);
            }
            else
            {
                divSearchArea.Visible = true;
                //divTaxSummary.Visible = false;
            }
            // ---------------------------------------------
            lnkAddNew.Visible = (hdnAddFlag.Value.ToLower() == "true") ? true : false;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {

            string downloadQuery = String.Empty;
            string sections = "Download";
            //BAL.CommonMgmt.GetExportDataList(hdnView.Value.ToLower());
            //ExportDataTableToExcel(dt, sections);
            // -----------------------------------------------------------
            if (hdnView.Value.ToLower() == "customer")
            {
                sections = "Customers";
                //  downloadQuery = BAL.CommonMgmt.GetConstant("CustDownload", 0);               
                if (String.IsNullOrEmpty(downloadQuery))
                    downloadQuery = " SELECT CustomerID, CustomerName, Address, Area, mst_city.CityName,mst_state.StateName, Pincode, ContactNo1, ContactNo2, " +
                                    " EmailAddress, CustomerType,GSTNO, PANNO, CINNO " +
                                    " FROM MST_Customer " +
                                    " left join mst_city on MST_Customer.CityCode =MST_City.CityCode " + 
                                    " LEFT join mst_state on mst_customer.StateCode= Mst_State.Statecode";
            }
            else if (hdnView.Value.ToLower() == "products")
            {
                sections = "Products";
                try
                {
                    downloadQuery = BAL.CommonMgmt.GetConstant("ProductDownload", 0);
                }
                catch (Exception ex){}
                if (String.IsNullOrEmpty(downloadQuery))
                    downloadQuery = " SELECT MST_Product.pkID, ProductName, ProductAlias, Unit, UnitPrice, TaxRate, ProductSpecification, " +
                                    " MST_ProductGroup.ProductGroupName,MST_Brand.BrandName,MST_Product.AddTaxPer,MST_Product.TaxType,MST_Product.HSNCode," + 
                                    " MST_Product.ActiveFlag,MST_Product.ManPower,MST_Product.HorsePower" +
                                    " FROM MST_Product" +
                                    " left join MST_ProductGroup on MST_Product.ProductGroupID=MST_ProductGroup.pkID" +
                                    " left join MST_Brand on MST_Product.BrandID=MST_Brand.pkID" ;
            }
            // -------------------------------------------------------------------------------------------
            string constr = ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(downloadQuery))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            ExportDataTableToExcel(dt, sections);
                        }
                    }
                }
                con.Close();
                con.Dispose();
            }
            if (File.Exists(Server.MapPath(sections+ ".xlsx")))
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + sections + ".xlsx");
                Response.AddHeader("Content-Type", "application/Excel");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.WriteFile(Server.MapPath(sections + ".xlsx"));
                Response.End();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "uploaddoc", "openUploadDocument('" + hdnView.Value.ToLower() + "');", true);
        }

        [WebMethod]
        private void ExportDataTableToExcel(DataTable table,string filename)
        {
            using (var workbook = SpreadsheetDocument.Create(Server.MapPath(filename+".xlsx"), DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
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

        [WebMethod]
        public void ExportDataTableToPdf(DataTable dt, string filename)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(filename), FileMode.Create));
            document.Open();
            iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

            PdfPRow row = null;
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            //float[] widths = new float[] { 4f, 4f, 4f, 4f };
            //table.SetWidths(widths);
            table.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("Export Data"));

            cell.Colspan = dt.Columns.Count;

            foreach (DataColumn c in dt.Columns)
            {
                table.AddCell(new Phrase(c.ColumnName, font5));
            }

            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    table.AddCell(new Phrase(r[0].ToString(), font5));
                    table.AddCell(new Phrase(r[1].ToString(), font5));
                    table.AddCell(new Phrase(r[2].ToString(), font5));
                    table.AddCell(new Phrase(r[3].ToString(), font5));
                }
            } document.Add(table);
            document.Close();
        }

        protected void btnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            string exportFile = "crmdonwloadedfile";
            DataTable dt = new DataTable();
            dt = BAL.CommonMgmt.GetExportDataList(hdnView.Value.ToLower());
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

        protected void btnExportToPdf_Click(object sender, ImageClickEventArgs e)
        {
            string exportFile = "crmdonwloadedfile.pdf";
            DataTable dt = new DataTable();
            dt = BAL.CommonMgmt.GetExportDataList(hdnView.Value.ToLower());
            // -------------------------------------------------
            ExportDataTableToPdf(dt, exportFile);
            // -------------------------------------------------
            if (File.Exists(Server.MapPath(exportFile)))
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + exportFile);
                Response.WriteFile(Server.MapPath(exportFile));
                Response.End();  
            }
        }

        [System.Web.Services.WebMethod]
        public static string setPrintHeader(string strpath)
        {
            HttpContext.Current.Session["PrintHeader"] = strpath;
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string getPrintHeader(string strpath)
        {
            return (string)HttpContext.Current.Session["PrintHeader"];
        }

        protected void drpExternalLeadAc_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnCurrPageNo.Value = "1";
        }

        protected void drpExternalLeadCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnCurrPageNo.Value = "1";
        }

        protected void drpComplainStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnCurrPageNo.Value = "1";
        }
    }
}