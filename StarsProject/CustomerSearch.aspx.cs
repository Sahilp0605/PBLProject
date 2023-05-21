using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;
using System.Web.Services;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace StarsProject
{
    public partial class CustomerSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown();
            }
        }

        public void BindCustomerData()
        {
            string xCustName = txtCustomerName.Text;
            string xType = (String.IsNullOrEmpty(drpCustomerType.SelectedItem.Value)) ? "" : drpCustomerType.SelectedItem.Text; 
            string xSource = (String.IsNullOrEmpty(drpCustomerSource.SelectedItem.Value)) ? "" : drpCustomerSource.SelectedItem.Text; 
            string xContact = txtContact.Text;
            string xEmail = txtEmail.Text;
            string xState = (String.IsNullOrEmpty(drpState.SelectedItem.Value)) ? "" : drpState.SelectedItem.Text;
            string xCity = (String.IsNullOrEmpty(drpCity.SelectedItem.Value)) ? "" : drpCity.SelectedItem.Text;
            string filterString = "'" + xCustName + "','" + xType + "','" + xSource + "','" + xContact + "','" + xEmail + "','" + xState + "','" + xCity + "'";
            ScriptManager.RegisterStartupScript(this, typeof(string), "opload", "javascript:loadCustomer(" + filterString + ");", true);
        }

        public void BindDropDown()
        {
            List<Entity.State> lstState = new List<Entity.State>();
            lstState = BAL.StateMgmt.GetStateList();
            drpState.DataSource = lstState;
            drpState.DataValueField = "StateCode";
            drpState.DataTextField = "StateName";
            drpState.DataBind();
            drpState.Items.Insert(0, new ListItem("-- All State --", ""));

            // ---------------- Customer Category List -------------------------------------
            List<Entity.CustomerCategory> lstCustCat = new List<Entity.CustomerCategory>();
            lstCustCat = BAL.CustomerCategoryMgmt.GetCustomerCategoryList();
            drpCustomerType.DataSource = lstCustCat;
            drpCustomerType.DataValueField = "CategoryName";
            drpCustomerType.DataTextField = "CategoryName";
            drpCustomerType.DataBind();
            drpCustomerType.Items.Insert(0, new ListItem("-- All Category --", ""));

            // ---------------- Designation List  -------------------------------------
            List<Entity.InquiryStatus> lstSource = new List<Entity.InquiryStatus>();
            lstSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("InquirySource");
            drpCustomerSource.DataSource = lstSource;
            drpCustomerSource.DataValueField = "pkID";
            drpCustomerSource.DataTextField = "InquiryStatusName";
            drpCustomerSource.DataBind();
            drpCustomerSource.Items.Insert(0, new ListItem("-- Select --", ""));
            // ---------------- Customer Category List -------------------------------------
            drpCity.Items.Insert(0, new ListItem("-- All City --", ""));

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCustomerData();
        }

        public void ClearAllField()
        {
            txtCustomerName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            //drpCustomerSource.SelectedValue = "";
            drpCustomerType.SelectedValue = "";
            drpState.SelectedValue = "";
            drpCity.SelectedValue = "";
            txtCustomerName.Focus();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            BindCustomerData();
        }

        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpState.SelectedValue))
            {
                if (Convert.ToInt64(drpState.SelectedValue) > 0)
                {
                    List<Entity.City> lstEvents = new List<Entity.City>();
                    lstEvents = BAL.CityMgmt.GetCityByState(Convert.ToInt64(drpState.SelectedValue));
                    drpCity.DataSource = lstEvents;
                    drpCity.DataValueField = "CityCode";
                    drpCity.DataTextField = "CityName";
                    drpCity.DataBind();
                    drpCity.Items.Insert(0, new ListItem("-- All City --", ""));
                    drpCity.Enabled = true;
                }

            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string downloadQuery = String.Empty;
            string sections = "Download";
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
                catch (Exception ex) { }
                if (String.IsNullOrEmpty(downloadQuery))
                    downloadQuery = " SELECT MST_Product.pkID, ProductName, ProductAlias, Unit, UnitPrice, TaxRate, ProductSpecification, " +
                                    " MST_ProductGroup.ProductGroupName,MST_Brand.BrandName,MST_Product.AddTaxPer,MST_Product.TaxType,MST_Product.HSNCode," +
                                    " MST_Product.ActiveFlag,MST_Product.ManPower,MST_Product.HorsePower" +
                                    " FROM MST_Product" +
                                    " left join MST_ProductGroup on MST_Product.ProductGroupID=MST_ProductGroup.pkID" +
                                    " left join MST_Brand on MST_Product.BrandID=MST_Brand.pkID";
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
                            if (dt.Rows.Count>0)
                            {
                                sda.Fill(dt);
                                ExportDataTableToExcel(dt, sections);
                            }
                        }
                    }
                }
                con.Close();
                con.Dispose();
            }
            if (File.Exists(Server.MapPath(sections + ".xlsx")))
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
            //Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
            /*using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                Byte[] bytes=File.ReadAllBytes(Server.MapPath("Customers.xlsx"));                                    
                for (int i = 0; i < bytes.Length; i++)
                {
                    MyMemoryStream.WriteByte(bytes[i]);
                }
                MyMemoryStream.WriteTo(Response.OutputStream);                   
                Response.Flush();
                Response.End();
            }*/

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "uploaddoc", "openUploadDocument('" + hdnView.Value.ToLower() + "');", true);
        }

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