using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
//using System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StarsProject
{
    public partial class GeneralMaster : System.Web.UI.Page
    {
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
                // -----------------------------------------------------------------
                //  Add / Edit / Delete Flag 
                // -----------------------------------------------------------------
                //hdnMenuID.Value = (!String.IsNullOrEmpty(Request.QueryString["MenuID"])) ? Request.QueryString["MenuID"].ToString().Trim() : "0";

                //List<Entity.ApplicationMenu> lstMenu = new List<Entity.ApplicationMenu>();
                //lstMenu = BAL.CommonMgmt.GetMenuAddEditDelList(Convert.ToInt64(hdnMenuID.Value), Session["LoginUserID"].ToString());

                //hdnAddFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].AddFlag.ToString().ToLower() : "true";
                //hdnEditFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].EditFlag.ToString().ToLower() : "true";
                //hdnDelFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].DelFlag.ToString().ToLower() : "true";
                //lnkAddNew.Visible = (hdnAddFlag.Value.ToLower() == "true") ? true : false;
                // ------------------------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["currtab"]))
                {
                    hdnCurrentTab.Value = Request.QueryString["currtab"].ToString();
                    hdnView.Value = Request.QueryString["currtab"].ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:loadFromDashboard();", true);
                }
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
                if (requestTarget.ToLower() == "deleterec") {
                }
            }
        }
        protected void drpPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PageSize"] = Convert.ToInt64(drpPageSize.SelectedValue);
        }
        protected void btnFileUpload_Click(object sender, ImageClickEventArgs e)
        {
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
            }
            document.Add(table);
            document.Close();
        }
    }
}