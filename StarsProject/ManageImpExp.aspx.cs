using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace StarsProject
{
    public partial class ManageImpExp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------------
            //try
            //{
                //string dirpath = "D:\\SamvithImages\\";
                //DirectoryInfo dir = new DirectoryInfo(dirpath);
                //foreach (FileInfo files in dir.GetFiles())
                //{
                //    DropDownList1.Items.Add(files.Name);
                //    int ReturnCode = 0;
                //    string ReturnMsg = "";
                //    BAL.ManageImpExpMgmt.AddMemberPhotoID(Convert.ToInt64(files.Name.ToString()), out ReturnCode, out ReturnMsg);
                //}
                // --------------------------------------------------------------------------
                //SqlConnection con = new SqlConnection(DBHandler.GetConnectionString());

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    if (con.State == ConnectionState.Open)
            //        con.Close();
            //}

        }

        //private void loadImageIDs()
        //{
        //    #region Load Image Ids
        //    SqlConnection con = new SqlConnection(DBHandler.GetConnectionString());
        //    SqlCommand cmd = new SqlCommand("ReadAllImageIDs", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    if (con.State == ConnectionState.Closed)
        //        con.Open();
        //    adp.Fill(dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        cmbImageID.DataSource = dt;
        //        cmbImageID.ValueMember = "ImageID";
        //        cmbImageID.DisplayMember = "ImageID";
        //        cmbImageID.SelectedIndex = 0;
        //    }
        //    #endregion
        //}
    }
}