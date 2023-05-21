using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myUserActivity : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDailyStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDailyEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public string pageView
        {
            get { return hdnView1.Value; }
            set { hdnView1.Value = value; }
        }

        public string pageMonth
        {
            get { return hdnMonth1.Value; }
            set { hdnMonth1.Value = value; }
        }

        public string pageYear
        {
            get { return hdnYear1.Value; }
            set { hdnYear1.Value = value; }
        }

        public Int64 UserCount
        {
            get
            {
                return Convert.ToInt64(rptUserActivity.Items.Count);
            }
        }

        public string UserID
        {
            get { return hdnUserID.Value; }
            set { hdnUserID.Value = value; }
        }

        protected void btnLoadUserActivity_Click(object sender, EventArgs e)
        {
            BindUserActivity(UserID);
        }


        public void BindUserActivity(String pLoginUserID)
        {
            int totrec;
            //Int64 pMon, pYear;
            //pMon = (!String.IsNullOrEmpty(hdnMonth1.Value)) ? Convert.ToInt64(hdnMonth1.Value) : 0;
            //pYear = (!String.IsNullOrEmpty(hdnYear1.Value)) ? Convert.ToInt64(hdnYear1.Value) : 0;
            UserID = pLoginUserID;
            // ----------------------------------------------------------------------
            txtDailyStartDate.Text = (String.IsNullOrEmpty(txtDailyStartDate.Text)) ? DateTime.Now.ToString("yyyy-MM-dd") : txtDailyStartDate.Text;
            txtDailyEndDate.Text = (String.IsNullOrEmpty(txtDailyEndDate.Text)) ? DateTime.Now.ToString("yyyy-MM-dd") : txtDailyEndDate.Text;
            // ----------------------------------------------------------------------
            List<Entity.UserLog> lstUserLog = new List<Entity.UserLog>();
            lstUserLog = BAL.UserMgmt.GetUserActivityListByUser(pLoginUserID, Convert.ToDateTime(txtDailyStartDate.Text), Convert.ToDateTime(txtDailyEndDate.Text), 1, 99000, out totrec);
            rptUserActivity.DataSource = lstUserLog;
            rptUserActivity.DataBind();
            hdnUserID.Value = UserID;
            if (lstUserLog.Count > 0)
            {
                spnUserEmployeeName.InnerText = lstUserLog[0].EmployeeName;
                spnUserDesignation.InnerText = lstUserLog[0].Designation;
            }

        }

    }
}