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

//using DocumentFormat.OpenXml.Packaging;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Services;
//using System.Web.UI;
//using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class DashboardInventory : System.Web.UI.Page
    {
        int totrec;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnLoginUserID.Value = Session["LoginUserID"].ToString();
                hdnWelcomeGreet.Value = BAL.CommonMgmt.GetConstant("WelcomeGreet", 0, 1);
                hdnLocationWiseStock.Value = BAL.CommonMgmt.GetConstant("LocationWiseStock", 0, 1);
                if (String.IsNullOrEmpty(hdnLocationWiseStock.Value) || hdnLocationWiseStock.Value.ToLower()=="no")
                    drpLocation.Style.Add("display", "none");

                // ----------------------------------------
                BindDropDown();

                BindMonthYear();
                BindMinStockList();
                BindAssemblyStockSummary();
                BindAssemblyStockSummaryProductWise();
                // ----------------------------------------
                BindMaterialPurcStatus();
                BindMaterialSaleStatus();
                BindJobCardStatus();
                BindLocationProductStock();
                BindMaterialIndent();
                
            }
        }
        public void BindDropDown()
        {
            
            List<Entity.Location> lstOrgDept2 = new List<Entity.Location>();
            lstOrgDept2 = BAL.LocationMgmt.GetLocationList(Session["LoginUserID"].ToString());
            drpLocation.DataSource = lstOrgDept2;
            drpLocation.DataValueField = "pkID";
            drpLocation.DataTextField = "LocationName";
            drpLocation.DataBind();
            drpLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Location --", ""));
            // ----------------------------------------
            List<Entity.SalesOrder> lstAssemblyOrder = new List<Entity.SalesOrder>();
            lstAssemblyOrder = BAL.SalesOrderMgmt.GetSalesOrderListByStatus("Approved", Session["LoginUserID"].ToString(), 1, 9999, out totrec);
            drpAssemblyOrder.DataSource = lstAssemblyOrder;
            drpAssemblyOrder.DataValueField = "OrderNo";
            drpAssemblyOrder.DataTextField = "OrderNo";
            drpAssemblyOrder.DataBind();
            drpAssemblyOrder.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Order --", ""));

            //List<Entity.ProductAssemblyStock> lstStock = new List<Entity.ProductAssemblyStock>();
            //lstStock = BAL.ProductMgmt.GetAssemblyStockSummaryProductWise(drpAssemblyStatus.SelectedValue, 0);
            //drpProduct.DataSource = lstStock.Select(m => new { m.FinishProductID, m.FinishProductName }).Distinct().ToList();
            //drpProduct.DataValueField = "FinishProductID";
            //drpProduct.DataTextField = "FinishProductName";

            List<Entity.Products> lstProducts = new List<Entity.Products>();
            lstProducts = BAL.ProductMgmt.GetProductList();
            lstProducts = lstProducts.Where(e =>(e.ProductType.ToLower() == "finished")).ToList();
            drpProduct.DataSource = lstProducts;
            drpProduct.DataValueField = "pkID";
            drpProduct.DataTextField = "ProductName";
            drpProduct.DataBind();
            drpProduct.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Product --", ""));
        }
        // --------------------------------------------------------
        // Section : Binding Assembly Stock Summary
        // --------------------------------------------------------
        public void BindAssemblyStockSummary()
        {
            myAssemblyStock.pageOrderNo = drpAssemblyOrder.SelectedValue;
            myAssemblyStock.pageView = drpAssemblyStatus.SelectedValue;
            myAssemblyStock.BindAssemblyStock();
        }

        public void BindAssemblyStockSummaryProductWise()
        {
            myAssemblyStockProductWise.pageQuantity = (!String.IsNullOrEmpty(txtQuantity.Text)) ? txtQuantity.Text : "0";
            myAssemblyStockProductWise.pageProductID = (!String.IsNullOrEmpty(drpProduct.SelectedValue)) ? drpProduct.SelectedValue : "0";
            myAssemblyStockProductWise.pageView = drpAssemblyStatus1.SelectedValue;
            myAssemblyStockProductWise.BindAssemblyStock();
        }

        // --------------------------------------------------------
        // Section : Binding Minimum Stock 
        // --------------------------------------------------------
        public void BindMinStockList()
        {
            myMinStockLevel.BindMinimumStock();
        }

        // --------------------------------------------------------
        // Section : Binding PurchaseOrder & SalesOrder Status 
        // --------------------------------------------------------
        public void BindMaterialPurcStatus()
        {
            myPurchaseStatus.pageViewType = drpPurcOrderStatus.SelectedValue;
            myPurchaseStatus.pageView = "purchase";
            myPurchaseStatus.pageStatus = drpPurcStatusType.SelectedValue;
            myPurchaseStatus.pageMonth = drpInvMonth.SelectedValue;
            myPurchaseStatus.pageYear = drpInvYear.SelectedValue;
            myPurchaseStatus.BindMaterialStatus();
        }

        public void BindMaterialSaleStatus()
        {
            mySalesOrderStatus.pageViewType = drpSalesOrderStatus.SelectedValue;
            mySalesOrderStatus.pageView = "sales";
            mySalesOrderStatus.pageStatus = drpSalesStatusType.SelectedValue;
            mySalesOrderStatus.pageMonth = drpInvMonth.SelectedValue;
            mySalesOrderStatus.pageYear = drpInvYear.SelectedValue;
            mySalesOrderStatus.BindMaterialStatus();
        }
        public void BindJobCardStatus()
        {
            myJobCardStatus.pageViewType = drpJobCardStatus.SelectedValue;
            myJobCardStatus.pageView = "";
            myJobCardStatus.pageStatus = drpJobCardStatusType.SelectedValue;
            myJobCardStatus.pageMonth = drpInvMonth.SelectedValue;
            myJobCardStatus.pageYear = drpInvYear.SelectedValue;
            myJobCardStatus.BindJobCardStatus();
        }

        public void BindLocationProductStock()
        {
            myLocationProductStock.pageView = drpLocationCategory.SelectedValue;
            myLocationProductStock.pageLocation = drpLocation.SelectedValue;
            myLocationProductStock.pageProduct = "0";
            myLocationProductStock.BindLocationProductList();
        }

        public void BindMaterialIndent()
        {
            myMaterialIndent.pageApprovalStatus = drpApprovalIndent.SelectedValue;
            myMaterialIndent.pageMonth = drpInvMonth.SelectedValue;
            myMaterialIndent.pageYear = drpInvYear.SelectedValue;
            myMaterialIndent.BindMaterialIndent(drpApprovalIndent.SelectedValue);
        }
        // --------------------------------------------------------
        // Binding Month & Year Dropdown 
        // --------------------------------------------------------
        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("January", "1"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("February", "2"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("March", "3"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("April", "4"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("May", "5"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("June", "6"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("July", "7"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("August", "8"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("September", "9"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("October", "10"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("November", "11"));
            drpInvMonth.Items.Add(new System.Web.UI.WebControls.ListItem("December", "12"));

            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpInvYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            drpInvYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpInvYear.SelectedValue = DateTime.Now.Year.ToString();
        }
        protected void drpInvMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMaterialPurcStatus();
            BindMaterialSaleStatus();
            BindJobCardStatus();
        }

        protected void drpSalesOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMaterialSaleStatus();
        }

        protected void drpPurcOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMaterialPurcStatus();
        }

        protected void drpJobCardStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindJobCardStatus();
        }

        protected void drpJobCardStatusType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindJobCardStatus();
        }

        protected void drpLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindLocationProductStock();
        }
        protected void drpAssemblyStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAssemblyStockSummary();
        }

        protected void drpAssemblyStatus1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAssemblyStockSummaryProductWise();
        }
        protected void drpApprovalIndent_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMaterialIndent();
        }
        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            BindAssemblyStockSummaryProductWise();
        }

        protected void btnExportRequest_Click(object sender, ImageClickEventArgs e)
        {
            string exportFile = "crmdonwloadedfile";
            DataTable dt = new DataTable();
            //dt = BAL.CommonMgmt.GetExportDataList("materialrequestdetail");
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(drpInvMonth.SelectedValue)) ? Convert.ToInt64(drpInvMonth.SelectedValue) : 0;
            pYear = (!String.IsNullOrEmpty(drpInvYear.SelectedValue)) ? Convert.ToInt64(drpInvYear.SelectedValue) : 0;
            // ------------------------------------------------------------------
            List<Entity.DispatchStatus> lstObject = new List<Entity.DispatchStatus>();
            lstObject = BAL.ReportMgmt.SupplierMaterialStatusList(drpPurcOrderStatus.SelectedValue, pMon, pYear, Session["LoginUserID"].ToString());
            if (!String.IsNullOrEmpty(drpPurcStatusType.SelectedValue))
                lstObject = lstObject.Where(it => (it.RequestStatus == drpPurcStatusType.SelectedValue)).ToList();
            dt = PageBase.ConvertListToDataTable(lstObject);
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
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

        protected void btnExportDispatch_Click(object sender, ImageClickEventArgs e)
        {
            string exportFile = "crmdonwloadedfile";
            DataTable dt = new DataTable();
            //dt = BAL.CommonMgmt.GetExportDataList("materialdispatchdetail");

            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(drpInvMonth.SelectedValue)) ? Convert.ToInt64(drpInvMonth.SelectedValue) : 0;
            pYear = (!String.IsNullOrEmpty(drpInvYear.SelectedValue)) ? Convert.ToInt64(drpInvYear.SelectedValue) : 0;
            // ------------------------------------------------------------------
            List<Entity.DispatchStatus> lstObject = new List<Entity.DispatchStatus>();
            lstObject = BAL.ReportMgmt.DispatchStatusList(drpSalesOrderStatus.SelectedValue, pMon, pYear, Session["LoginUserID"].ToString());
            if (!String.IsNullOrEmpty(drpSalesStatusType.SelectedValue))
                lstObject = lstObject.Where(it => (it.RequestStatus == drpSalesStatusType.SelectedValue)).ToList();
            dt = PageBase.ConvertListToDataTable(lstObject);

            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            ExportDataTableToExcel(dt, exportFile);
            // -------------------------------------------------------
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

        protected void btnExportAssemblyStkProductwise_Click(object sender, ImageClickEventArgs e)
        {
            string exportFile = "crmdonwloadedfile";
            DataTable dt = new DataTable();
            //dt = BAL.CommonMgmt.GetExportDataList("assemblystkproductwise");
            List<Entity.ProductAssemblyStock> lstEntity = new List<Entity.ProductAssemblyStock>();
            lstEntity = BAL.ProductMgmt.GetAssemblyStockSummaryProductWise(drpAssemblyStatus1.SelectedValue, Convert.ToInt64(string.IsNullOrEmpty(drpProduct.SelectedValue) ? 0 : Convert.ToInt64(drpProduct.SelectedValue)), Convert.ToDouble(string.IsNullOrEmpty(txtQuantity.Text) ? 1 : Convert.ToInt64(txtQuantity.Text)));
            // -------------------------------------------------------
            dt =  PageBase.ConvertListToDataTable(lstEntity);
            //--------------------------------------------------------
            ExportDataTableToExcel(dt, exportFile);
            // -------------------------------------------------------
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
    }
}