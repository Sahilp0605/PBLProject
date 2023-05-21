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

namespace StarsProject
{
    public partial class UploadSalesOrder : System.Web.UI.Page
    {
        string GetSOString = "";
        string ExcelCol, DBCol;
        string ExcelCol1, DBCol1;
        //string ConncetionString = System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
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
                        uploadSalesOrder();
                    }
                }
            }
        }

        public void uploadSalesOrder()
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

                
                if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["SalesOrder"]))
                {
                    GetSOString = System.Configuration.ConfigurationManager.AppSettings["SalesOrder"];
                }
                string[] values = GetSOString.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                    
                    ExcelCol = values[i].Substring(0, values[i].IndexOf("="));
                    DBCol = values[i].Substring(values[i].IndexOf("=") + 1, values[i].Length - values[i].IndexOf("=") - 1);

                    if (String.IsNullOrEmpty(ExcelCol1))
                        ExcelCol1 = ExcelCol;
                    else
                        ExcelCol1 = ExcelCol1 + "," + ExcelCol;

                    if (String.IsNullOrEmpty(DBCol1))
                        DBCol1 = DBCol;
                    else
                        DBCol1 = DBCol1 + "," + DBCol;
                }

                //string ExcelCol1 = String.Join(",", ExcelCol);
                //string DBCol1 = String.Join(",", DBCol);
                try
                {
                    SqlBulkCopy oSqlBulk = null;
                    connection.Open();

                    // GET DATA FROM EXCEL SHEET.
                    OleDbCommand objOleDB =
                        new OleDbCommand("select " + ExcelCol1 + " from [Sheet1$]", connection);

                    // READ THE DATA EXTRACTED FROM THE EXCEL FILE.
                    OleDbDataReader objBulkReader = null;
                    objBulkReader = objOleDB.ExecuteReader();

                    // SET THE CONNECTION STRING.
                    string sCon = System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(sCon))
                    {
                        con.Open();

                        // FINALLY, LOAD DATA INTO THE DATABASE TABLE.
                        oSqlBulk = new SqlBulkCopy(con);
                        oSqlBulk.DestinationTableName = "SalesOrder_Detail"; // TABLE NAME.
                        oSqlBulk.WriteToServer(objBulkReader);
                    }

                    lblConfirm.Text = "DATA IMPORTED SUCCESSFULLY.";
                    lblConfirm.Attributes.Add("style", "color:green");
                    //DataTable dt = new DataTable();

                    //string qry = "select " + ExcelCol1 + " from [Sheet1$]";
                    //OleDbCommand command = new OleDbCommand("select " + ExcelCol1 + " from [Sheet1$]", connection);

                    //connection.Open();
                    //OleDbDataReader dr = command.ExecuteReader();
                    //DataSet ds = new DataSet();
                    //OleDbDataAdapter oda = new OleDbDataAdapter("select " + ExcelCol1 + " from [Sheet1$]", connection);
                    //connection.Close();
                    //oda.Fill(ds);
                    //DataTable Exceldt = ds.Tables[0];

                    ////dt.Load(dr);

                    //string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;

                    //SqlBulkCopy bulkInsert = new SqlBulkCopy(ConnectionString);
                    //bulkInsert.DestinationTableName = "SalesOrder_Detail";

                    //string[] val = DBCol1.Split(',');
                    //string[] val1 = ExcelCol1.Split(',');
                    //for (int i = 0; i < val.Length; i++)
                    //{
                    //    bulkInsert.ColumnMappings.Add(val1[i], val[i]);
                    //}
                    ////bulkInsert.ColumnMappings.Add("ProductID", "ProductID");
                    ////bulkInsert.ColumnMappings.Add("SO", "OrderNo");
                    ////bulkInsert.ColumnMappings.Add("Tmp", "Quantity");
                    ////bulkInsert.WriteToServer(dr);
                    //SqlConnection sCon = new SqlConnection();
                    //sCon.ConnectionString = ConnectionString;
                    //sCon.Open();
                    //bulkInsert.WriteToServer(Exceldt);
                    //sCon.Close();
                }
                catch (Exception ex)
                {
                    ReturnCode = 0;
                    ReturnMsg = ex.Message.ToString();
                }
                //while (dr.Read())
                //{
                //    if (!String.IsNullOrEmpty(txtCustomerName.Text))
                //    {
                //        try
                //        {
                //            //Insert into DailyAttendance(EmployeeID) Select EmployeeID from DailyAttendence_New
                //            string query = "insert into SalesOrder_Detail (" + DBCol1 + ") " + command.CommandText;
                //            SqlCommand cmdAdd = new SqlCommand(query);
                //            ExecuteNonQuery(cmdAdd);
                //            //SqlConnection sCon = new SqlConnection();
                //            //sCon.ConnectionString = ConncetionString;
                //            //cmdAdd.Connection = sCon;
                //            //cmdAdd.CommandTimeout = 30000;
                //            //sCon.Open();
                //            //ExecuteNonQuery(cmdAdd);
                //            //sCon.Close();

                //            ReturnCode = 1;
                //            ReturnMsg = "File Uploaded Successfully !";
                //        }
                //        catch (Exception ex)
                //        {
                //            ReturnCode = 0;
                //            ReturnMsg = ex.Message.ToString();
                //        }
                //        connection.Close();
                //        SqlConnection.ClearAllPools();
                //        ////--------------------------------------------------------------Insert / Update Record
                //        //BAL.CommonMgmt.AddUpdateSOUPDOWN(dt, out ReturnCode, out ReturnMsg);
                //        totalCount = totalCount + ((ReturnCode > 0) ? 1 : 0);
                //        failedCount = failedCount + ((ReturnCode <= 0) ? 1 : 0);
                //    }
                //    else
                //    {
                //        dataIssueCount = dataIssueCount + ((ReturnCode <= 0) ? 1 : 0);
                //    }
                //}
            }
            }
        #region ConncetionString
        public static string ConncetionString()
        {
           return System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
        }
        #endregion
        #region OpenConncetion
        public static void OpenConncetion(SqlCommand command)
        {
            SqlConnection sCon = new SqlConnection();
            sCon.ConnectionString = ConncetionString();
            command.Connection = sCon;
            command.CommandTimeout = 30000;
            sCon.Open();
        }
        #endregion
        public static int ExecuteNonQuery(SqlCommand command)
        {
            OpenConncetion(command);
            return command.ExecuteNonQuery();
        }
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}