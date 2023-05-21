using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Ionic.Zip;
using System.Net;
using System.ComponentModel;
using System.IO;
namespace StarsProject 
{
    public partial class DatabaseBackup : System.Web.UI.Page
    {
        //private string Livepath = ConfigurationManager.AppSettings["StarsConnection"].ToString();
        //private SqlConnection con = new SqlConnection(Livepath);
        //private SqlCommand com = new SqlCommand();
        protected void Page_Load(object sender, EventArgs e)
        {
            //SqlCommand myCommand = new SqlCommand();
            //myCommand.CommandType = CommandType.Text;
            //myCommand.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            //SqlDataReader dr = ExecuteDataReader(myCommand);
            //dt.Load(dr);
            //ForceCloseConncetion();
            if(!IsPostBack)
            {
                //rpt_TableDetail.DataSource = BAL.CommonMgmt.getBackupTableList();
                //rpt_TableDetail.DataBind();
            }
        }

        protected void btnGetBackup_Click(object sender, EventArgs e)
        {
            string Livepath = ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(Livepath);
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            try
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "return setBackupStatus('Backup Process Initiated !','on');", true);
                Response.Write("<script>alert('start')</script>");
                string DBName, DBBackupLocation;
                DBName = con.Database.ToString();
                DBBackupLocation = Server.MapPath(DBName + ".bak");
                if (!String.IsNullOrEmpty(DBName) && !String.IsNullOrEmpty(DBBackupLocation))
                {
                    if (File.Exists(Server.MapPath(DBName + ".bak")))
                    {
                        File.Delete(Server.MapPath(DBName + ".bak"));
                    }

                    com.CommandText = "BACKUP DATABASE [" + DBName + "] TO  DISK = '" + DBBackupLocation + "'";
                    con.Open();
                    com.ExecuteNonQuery();
                    // ------------------------------------------------------------
                    string dbfile = Server.MapPath(DBName + ".bak");

                    Response.Redirect("handler/myHandler.ashx?DBName=" + dbfile);
                    //Response.ContentType = "text/plain";
                    //Response.Write("Hello World");
                    //Response.Clear();
                    //Response.ContentType = "application/octect-stream";
                    //Response.AppendHeader("content-disposition", "filename=" + dbfile);
                    //Response.TransmitFile(dbfile);
                    dvStatus.Attributes.Remove("class");
                    //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "$('#dvStatus').removeClass('indeterminate');", true);               
                    Response.End();
                    // ------------------------------------------------------------
                    
                }
                else
                {
                    Response.Write("<script>alert('Database Configuration or Location Settings Missing !');location.href='" + Server.MapPath(DBName + ".bak") + "'</script>");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
            }
            finally
            {
                con.Close();
                
            }
        }

        void ExportCSV(string Dataname, ZipFile zip)
        {
            string constr = ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [" + Dataname + "]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);

                            //Build the CSV file data as a Comma separated string.
                            string csv = string.Empty;

                            foreach (DataColumn column in dt.Columns)
                            {
                                //Add the Header row for CSV file.
                                csv += column.ColumnName + ',';
                            }

                            //Add new line.
                            csv += "\r\n";

                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    //Add the Data rows.
                                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                                }
                                //Add new line.
                                csv += "\r\n";
                            }
                            zip.AddEntry(Dataname + ".csv", csv);
                            //Download the CSV file.                            
                        }
                    }
                    con.Close();
                    con.Dispose();
                }
            }
        }

       
    }

}