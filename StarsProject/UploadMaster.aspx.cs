using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data.SqlTypes;

namespace StarsProject
{
    public partial class UploadMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["view"]))
                    hdnMode.Value = Request.QueryString["view"].ToString().Trim();
            }
            else
            {
                if (FileUpload1.PostedFile != null)
                {
                    if (FileUpload1.HasFile)
                    {
                        uploadCustomer();
                    }
                }
            }
            // ------------------------------------
            imgFormat.ImageUrl = "images/no-figure2.png";
            if (hdnMode.Value.ToLower() == "customer")
                imgFormat.ImageUrl = "images/CustomerFormat.png";
            else if (hdnMode.Value.ToLower() == "products")
                imgFormat.ImageUrl = "images/ProductFormat.png";
            else if (hdnMode.Value.ToLower() == "attendance")
                imgFormat.ImageUrl = "images/AttendanceFormat.png";

        }

        public void uploadCustomer()
        {
            int ReturnCode = 0;
            String ReturnMsg = "";
            Int64 totalCount = 0, failedCount = 0, dataIssueCount = 0;

            if (FileUpload1.PostedFile != null)
            {

                string filePath = FileUpload1.PostedFile.FileName;
                string filename1 = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename1);

                // Upload and save the file
                string excelPath = Server.MapPath("~/PDF/") + FileUpload1.PostedFile.FileName;
                FileUpload1.SaveAs(excelPath);
                // ------------------------------------------------------------------------------
                // Connection String to Excel Workbook
                // ------------------------------------------------------------------------------
                string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", excelPath);

                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString = excelConnectionString;

                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                connection.Open();

                // Create DbDataReader to Data Worksheet
                OleDbDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    if (hdnMode.Value.ToLower() == "customer")
                    {
                        Entity.Customer objEntity = new Entity.Customer();
                        if (!String.IsNullOrEmpty(dr["CustomerName"].ToString()) && !String.IsNullOrEmpty(dr["CityName"].ToString()) && !String.IsNullOrEmpty(dr["StateName"].ToString()))
                        {
                            objEntity.CustomerID = Convert.ToInt64(dr["CustomerID"].ToString());
                            objEntity.CustomerName = dr["CustomerName"].ToString();
                            objEntity.CustomerType = dr["CustomerType"].ToString();
                            objEntity.Address = dr["Address"].ToString();
                            objEntity.Area = dr["Area"].ToString();
                            objEntity.CityName = dr["CityName"].ToString();
                            objEntity.StateName = dr["StateName"].ToString();
                            objEntity.Pincode = dr["PinCode"].ToString();
                            objEntity.ContactNo1 = dr["ContactNo1"].ToString();
                            objEntity.ContactNo2 = dr["ContactNo2"].ToString();
                            objEntity.EmailAddress = dr["EmailAddress"].ToString();
                            objEntity.GSTNo = dr["GSTNo"].ToString();
                            objEntity.PANNo = dr["PANNo"].ToString();
                            objEntity.CINNo = dr["CINNo"].ToString();
                            objEntity.PriceListID = Convert.ToInt64(dr["PriceListID"].ToString());
                            objEntity.LoginUserID = Session["LoginUserID"].ToString();
                            // -------------------------------------------------------------- Insert/Update Record
                            BAL.CustomerMgmt.AddUpdateCustomerUPDOWN(objEntity, out ReturnCode, out ReturnMsg);
                            totalCount = totalCount + ((ReturnCode > 0) ? 1 : 0);
                            failedCount = failedCount + ((ReturnCode <= 0) ? 1 : 0);
                        }
                        else
                        {
                            dataIssueCount = dataIssueCount + ((ReturnCode <= 0) ? 1 : 0);
                        }
                    }
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    if (hdnMode.Value.ToLower() == "products")
                    {
                        Entity.Products objEntity = new Entity.Products();
                        if (!String.IsNullOrEmpty(dr["ProductName"].ToString()) && !String.IsNullOrEmpty(dr["ProductGroupName"].ToString()) && !String.IsNullOrEmpty(dr["BrandName"].ToString()))
                        {
                            objEntity.pkID = Convert.ToInt64(dr["pkID"].ToString());
                            objEntity.ProductName = dr["ProductName"].ToString();
                            objEntity.ProductAlias = dr["ProductAlias"].ToString();
                            objEntity.ProductGroupName = dr["ProductGroupName"].ToString();
                            objEntity.BrandName = dr["BrandName"].ToString();
                            objEntity.ProductSpecification = dr["ProductSpecification"].ToString();
                            objEntity.Unit = dr["Unit"].ToString();
                            objEntity.UnitPrice = (!String.IsNullOrEmpty(dr["UnitPrice"].ToString())) ? Convert.ToDecimal(dr["UnitPrice"].ToString()) : 0;
                            objEntity.TaxRate = (!String.IsNullOrEmpty(dr["TaxRate"].ToString())) ? Convert.ToDecimal(dr["TaxRate"].ToString()) : 0;
                            objEntity.AddTaxPer = (!String.IsNullOrEmpty(dr["AddTaxPer"].ToString())) ? Convert.ToDecimal(dr["AddTaxPer"].ToString()) : 0;
                            objEntity.TaxType = (!String.IsNullOrEmpty(dr["TaxType"].ToString())) ? Convert.ToInt16(dr["TaxType"].ToString()) : 0;
                            objEntity.HSNCode = (!String.IsNullOrEmpty(dr["HSNCode"].ToString())) ? Convert.ToString(dr["HSNCode"].ToString()) : "";
                            objEntity.ActiveFlag = (!String.IsNullOrEmpty(dr["ActiveFlag"].ToString())) ? Convert.ToBoolean(dr["ActiveFlag"].ToString()) : false;
                            objEntity.ProductType = (!String.IsNullOrEmpty(dr["ProductType"].ToString())) ? Convert.ToString(dr["ProductType"].ToString()) : "";
                            objEntity.OpeningSTK = (!String.IsNullOrEmpty(dr["OpeningSTK"].ToString())) ? Convert.ToDecimal(dr["OpeningSTK"].ToString()) : 1;
                            objEntity.LoginUserID = Session["LoginUserID"].ToString();

                            BAL.ProductMgmt.AddUpdateProductUPDOWN(objEntity, out ReturnCode, out ReturnMsg);

                            totalCount = totalCount + ((ReturnCode > 0) ? 1 : 0);
                            failedCount = failedCount + ((ReturnCode <= 0) ? 1 : 0);
                        }
                        else
                        {
                            dataIssueCount = dataIssueCount + ((ReturnCode <= 0) ? 1 : 0);
                        }

                    }
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    if (hdnMode.Value.ToLower() == "attendance")
                    {
                        Entity.Attendance objEntity = new Entity.Attendance();

                        objEntity.pkID = 0;
                        if (!String.IsNullOrEmpty(dr["EmployeeID"].ToString()) && !String.IsNullOrEmpty(dr["Date"].ToString()) && !String.IsNullOrEmpty(dr["In Time"].ToString()) && !String.IsNullOrEmpty(dr["Out Time"].ToString()))
                        {
                            objEntity.EmployeeID = Convert.ToInt64(dr["EmployeeID"].ToString());
                            // ---------------------------------------------------------------------
                            DateTime d1 = DateTime.Parse(dr["In Time"].ToString().Replace(".", ":"));
                            objEntity.TimeIn = d1.ToString("HH:mm");
                            // ---------------------------------------------------------------------
                            DateTime d2 = DateTime.Parse(dr["Out Time"].ToString().Replace(".", ":"));
                            objEntity.TimeOut = d2.ToString("HH:mm");
                            // ---------------------------------------------------------------------
                            DateTime tmpDate = Convert.ToDateTime(dr["Date"].ToString());
                            objEntity.PresenceDate = Convert.ToDateTime(tmpDate.Year.ToString() + "-" + tmpDate.Month.ToString() + "-" + tmpDate.Day.ToString());
                            // ---------------------------------------------------------------------
                            objEntity.LoginUserID = Session["LoginUserID"].ToString();
                            BAL.AttendanceMgmt.AddUpdateAttendance(objEntity, out ReturnCode, out ReturnMsg);

                            totalCount = totalCount + ((ReturnCode > 0) ? 1 : 0);
                            failedCount = failedCount + ((ReturnCode <= 0) ? 1 : 0);
                        }
                        else
                        {
                            dataIssueCount = dataIssueCount + ((ReturnCode <= 0) ? 1 : 0);
                        }
                    }
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    if (hdnMode.Value.ToLower() == "telecaller")
                    {
                        Entity.ExternalLeads objEntity = new Entity.ExternalLeads();
                        if (!String.IsNullOrEmpty(dr["Company Name"].ToString()) && !String.IsNullOrEmpty(dr["Contact Person"].ToString()))
                        {
                            objEntity.SenderName = dr["Contact Person"].ToString();
                            objEntity.SenderMail = dr["Email"].ToString();
                            //objEntity.QueryDatetime = DateTime.ParseExact(dr["QueryDatetime"].ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            objEntity.QueryDatetime = Convert.ToDateTime(dr["Query Date"].ToString());
                            objEntity.CompanyName = dr["Company Name"].ToString();
                            objEntity.Message = dr["Detail"].ToString();
                            objEntity.Address = dr["Address"].ToString();
                            objEntity.City = dr["City"].ToString();
                            objEntity.Pincode = dr["Pincode"].ToString();
                            objEntity.State = dr["State"].ToString();
                            objEntity.ProductID = Convert.ToInt64(dr["Product"].ToString());
                            objEntity.PrimaryMobileNo = dr["Primary Contact #"].ToString();
                            objEntity.SecondaryMobileNo = dr["Alternate Contact"].ToString();
                            objEntity.LeadSource = dr["LeadSource"].ToString();
                            objEntity.CountryCode = dr["Country"].ToString();
                            objEntity.LeadStatus = dr["Lead Status"].ToString();
                            objEntity.ExLeadClosure = Convert.ToInt64(dr["Reason For Disqualify"].ToString());
                            objEntity.EmployeeID = Convert.ToInt64(dr["Assign To Emp"].ToString());
                            objEntity.FollowupDate = (!String.IsNullOrEmpty(dr["Followup Date"].ToString())) ? Convert.ToDateTime(dr["Followup Date"].ToString()) : SqlDateTime.MinValue.Value; 
                            objEntity.FollowupNotes = dr["Detail"].ToString();
                            objEntity.LoginUserID = Session["LoginUserID"].ToString();
                            // -------------------------------------------------------------- Insert/Update Record
                            BAL.ExternalLeadsMgmt.AddUpdateExternalLeadsUPDOWN(objEntity, out ReturnCode, out ReturnMsg);
                            totalCount = totalCount + ((ReturnCode > 0) ? 1 : 0);
                            failedCount = failedCount + ((ReturnCode <= 0) ? 1 : 0);
                        }
                        else
                        {
                            dataIssueCount = dataIssueCount + ((ReturnCode <= 0) ? 1 : 0);
                        }
                    }
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    if (hdnMode.Value.ToLower() == "pricelist")
                    {
                        Entity.PriceListDetail objEntity = new Entity.PriceListDetail();
                        if (!String.IsNullOrEmpty(dr["Price List Name"].ToString()))
                        {
                            objEntity.PriceList = (!String.IsNullOrEmpty(dr["Price List Name"].ToString())) ? Convert.ToString(dr["Price List Name"].ToString()) : "";
                            objEntity.UnitPrice = (!String.IsNullOrEmpty(dr["Rate"].ToString())) ? Convert.ToDecimal(dr["Rate"].ToString()) : 0;
                            objEntity.Discount = (!String.IsNullOrEmpty(dr["Discount"].ToString())) ? Convert.ToDecimal(dr["Discount"].ToString()) : 0;
                            objEntity.ProductID = (!String.IsNullOrEmpty(dr["Product ID"].ToString())) ? Convert.ToInt64(dr["Product ID"].ToString()) : 0;
                            objEntity.LoginUserID = Session["LoginUserID"].ToString();
                            BAL.PriceListMgmt.AddUpdatePriceListUPDOWN(objEntity, out ReturnCode, out ReturnMsg);

                            totalCount = totalCount + ((ReturnCode > 0) ? 1 : 0);
                            failedCount = failedCount + ((ReturnCode <= 0) ? 1 : 0);
                        }
                        else
                        {
                            dataIssueCount = dataIssueCount + ((ReturnCode <= 0) ? 1 : 0);
                        }

                    }
                }

                connection.Close();
                Response.Write("<script>alert('Data Has Been Uploaded Successfully');</script>");
            }
            // -----------------------------------------
            lblSuccessCount.Text = "Success Count : " + totalCount.ToString();
            lblFailedCount.Text = "Failed Count  : " + failedCount.ToString();
            lbldataIssueCount.Text = "Invalid Data Count  : " + dataIssueCount.ToString();
        }
    }
}