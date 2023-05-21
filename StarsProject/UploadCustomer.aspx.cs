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
using System.Data.SqlTypes;

namespace StarsProject
{
    public partial class UploadCustomer : System.Web.UI.Page
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
                        uploadDetails();
                    }
                }
            }
        }

        public void uploadDetails()
        {

            int ReturnCode = 0, ReturnCode1 = 0;
            String ReturnMsg = "", ReturnMsg1 = "";

            if (FileUpload1.PostedFile != null)
            {
                string filePath = FileUpload1.PostedFile.FileName;
                string filename1 = Path.GetFileName(filePath);
                string ext = Path.GetExtension(filename1);

                // Upload and save the file
                string excelPath = Server.MapPath("~/PDF/") + FileUpload1.PostedFile.FileName;
                FileUpload1.SaveAs(excelPath);
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

                        objEntity.CustomerID = Convert.ToInt64(dr["CustomerID"].ToString());
                        objEntity.CustomerName = dr["CustomerName"].ToString();
                        objEntity.CustomerType = dr["CustomerType"].ToString();
                        objEntity.Address = dr["Address"].ToString();
                        objEntity.Area = dr["Area"].ToString();
                        objEntity.CityName = dr["CityName"].ToString();
                        objEntity.StateName = dr["StateName"].ToString();
                        objEntity.CountryName = dr["CountryName"].ToString();
                        objEntity.Pincode = dr["PinCode"].ToString();
                        objEntity.ContactNo1 = dr["ContactNo1"].ToString();
                        objEntity.ContactNo2 = dr["ContactNo2"].ToString();
                        objEntity.EmailAddress = dr["EmailAddress"].ToString();
                        objEntity.WebsiteAddress = dr["WebsiteAddress"].ToString();
                        objEntity.GSTNo = dr["GSTNo"].ToString();
                        objEntity.PANNo = dr["PANNo"].ToString();
                        objEntity.CINNo = dr["CINNo"].ToString();
                        objEntity.CountryName = dr["CountryName"].ToString();
                        objEntity.CustomerSourceName = dr["Source"].ToString();
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        BAL.CustomerMgmt.AddUpdateCustomerUPDOWN(objEntity, out ReturnCode, out ReturnMsg);

                        if (ReturnCode > 0)
                        {
                            Entity.CustomerContacts objEntity1 = new Entity.CustomerContacts();

                            objEntity1.CustomerID = ReturnCode;
                            objEntity1.ContactPerson1 = dr["ContactPersonName"].ToString();
                            objEntity1.ContactNumber1 = dr["ContactPersonNumber"].ToString();
                            objEntity1.ContactEmail1 = dr["ContactPersonEmail"].ToString();
                            objEntity1.ContactDesigCode1 = dr["ContactPersonDesignation"].ToString();
                            objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                            BAL.CustomerContactsMgmt.AddUpdateCustomerContacts(objEntity1, out ReturnCode1, out ReturnMsg1);
                        }
                    }
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    if (hdnMode.Value.ToLower() == "products")
                    {
                        Entity.Products objEntity = new Entity.Products();

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
                        objEntity.ManPower = (!String.IsNullOrEmpty(dr["ManPower"].ToString())) ? Convert.ToDecimal(dr["ManPower"].ToString()) : 0;
                        objEntity.HorsePower = (!String.IsNullOrEmpty(dr["HorsePower"].ToString())) ? Convert.ToDecimal(dr["HorsePower"].ToString()) : 0;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.ProductMgmt.AddUpdateProductUPDOWN(objEntity, out ReturnCode, out ReturnMsg);
                    }
                }
                connection.Close();
                Response.Write("<script>alert('Data Has Been Uploaded Successfully');</script>");
            }
        }
    }
}