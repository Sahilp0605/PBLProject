using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public delegate void ActionClick();
    public partial class myNotificationPanel : System.Web.UI.UserControl
    {
        public event ActionClick FSaveEmailClick;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string CustomerID
        {
            get { return hdnNotiCustomerID.Value; }
            set { hdnNotiCustomerID.Value = value; }
        }
        public string checkEmail
        {
            get { return hdnSendEmail.Value; }
            set { hdnSendEmail.Value = value; }
        }
        public string checkSMS
        {
            get { return hdnSendSMS.Value; }
            set { hdnSendSMS.Value = value; }
        }
        public string checkWhatsapp
        {
            get { return hdnSendWhatsapp.Value; }
            set { hdnSendWhatsapp.Value = value; }
        }

        public string getEmailTo
        {
            get
            {
                return txtEmailTo.Text.Trim() + ";";
            }
        }
        public string getEmailCC
        {
            get
            {
                return txtEmailCC.Text.Trim() + ";";
            }
        }
        public string getEmailBCC
        {
            get
            {
                return txtEmailBCC.Text.Trim() + ";";
            }
        }

        public string getSMSContacts
        {
            get {
                string tmpVal = "";
                if (!String.IsNullOrEmpty(txtSMSTo.Text))
                    tmpVal = txtSMSTo.Text.Trim();
                //if (!String.IsNullOrEmpty(txtSMSCC.Text))
                //    tmpVal += txtSMSCC.Text.Trim();
                return tmpVal;
            }
        }
        // -----------------------------------------------------
        protected void btnFSaveEmail_Click(object sender, EventArgs e)
        {
            if (FSaveEmailClick != null)
            {
                FSaveEmailClick();
            }
        }

        public void BindNotificationData()
        {
            int TotalCount = 0;
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            if (!String.IsNullOrEmpty(hdnNotiCustomerID.Value))
            {
                lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnNotiCustomerID.Value), Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                // ----------------------------------------------------
                txtEmailTo.Text = lstEntity[0].EmailAddress;
                txtSMSTo.Text = lstEntity[0].ContactNo1;
            }
        }
    }
}