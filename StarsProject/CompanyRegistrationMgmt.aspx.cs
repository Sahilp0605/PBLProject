using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class CompanyRegistrationMgmt : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        private static Random random = new Random();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSerialKey.CssClass = "form-control";
                //txtInstallationDate.CssClass = "form-control";

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        string rand_no=RandomString(16);

                        int cnt = 0;
                        char[] charArr = rand_no.ToCharArray();
                        string rand_no_new = "";
                        foreach (char ch in charArr)
                        {
                            
                            if(cnt==4)
                            {
                                cnt=0;
                                rand_no_new = string.Concat(rand_no_new,"-");
                                rand_no_new = string.Concat(rand_no_new,ch);
                            }
                            else
                            {
                                rand_no_new = string.Concat(rand_no_new,ch);
                            }
                            cnt = cnt + 1;
                        }
                        txtSerialKey.Text = rand_no_new;

                    }
                        
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            if (Request.QueryString["mode"].ToString() == "view")
                                OnlyViewControls();
                        }
                    }
                }
            }
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.CompanyRegistration> lstEntity = new List<Entity.CompanyRegistration>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();

                lstEntity = BAL.CompanyRegistrationMgmt.GetCompanyRegistration(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtCompanyName.Text = lstEntity[0].CompanyName.ToString();
                txtNoOfUsers.Text = lstEntity[0].NoOfUsers.ToString();
                txtSerialKey.Text = lstEntity[0].SerialKey.ToString();

                txtDBIP.Text = lstEntity[0].DBIP.ToString();
                txtDBName.Text = lstEntity[0].DBName.ToString();
                txtDBUserName.Text = lstEntity[0].DBUsername.ToString();
                txtDBPassword.Text = lstEntity[0].DBPassword.ToString();
                txtInstallationDate.Text = lstEntity[0].InstallationDate.ToString("dd-MM-yyyy");
                txtExpiryDate.Text = lstEntity[0].ExpiryDate.ToString("dd-MM-yyyy");

                txtRootPath.Text = lstEntity[0].RootPath.ToString();
                txtSiteURL.Text = lstEntity[0].SiteURL.ToString();
                txtIndiaMartKey.Text = lstEntity[0].IndiaMartKey.ToString();
                txtIndiaMartMobile.Text = lstEntity[0].IndiaMartMobile.ToString();
                txtIndiaMartAcAlias.Text = lstEntity[0].IndiaMartAcAlias.ToString();

                txtIndiaMartKey2.Text = lstEntity[0].IndiaMartKey2.ToString();
                txtIndiaMartMobile2.Text = lstEntity[0].IndiaMartMobile2.ToString();
                txtIndiaMartAcAlias2.Text = lstEntity[0].IndiaMartAcAlias2.ToString();

                txtInstallationDate.Focus();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            _pageValid = true;
            divErrorMessage.InnerHtml = "";

            //if (String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtToDate.Text))
            //{
            //    _pageValid = false;

            //    divErrorMessage.Style.Remove("color");
            //    divErrorMessage.Style.Add("color", "red");
            //    if (String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtToDate.Text))
            //        divErrorMessage.Controls.Add(new LiteralControl("<li>" + "From and To Date is mandatory !" + "</li>"));

            //    if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            //        divErrorMessage.Controls.Add(new LiteralControl("<li>" + "From Date is Always Less then To Date." + "</li>"));
            //}
            //if (!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtToDate.Text))
            //{

            //    if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            //    {
            //        _pageValid = false;

            //        divErrorMessage.Style.Remove("color");
            //        divErrorMessage.Style.Add("color", "red");

            //        divErrorMessage.Controls.Add(new LiteralControl("<li>" + "FromDate & ToDate range selection is wrong." + "</li>"));
            //    }
            //}
            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.CompanyRegistration objEntity = new Entity.CompanyRegistration();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.CompanyName = txtCompanyName.Text;
                objEntity.NoOfUsers = Convert.ToInt64(txtNoOfUsers.Text);
                objEntity.SerialKey = txtSerialKey.Text;
                objEntity.DBIP  = txtDBIP.Text;
                objEntity.DBName  = txtDBName.Text;
                objEntity.DBUsername  = txtDBUserName.Text;
                objEntity.DBPassword = txtDBPassword.Text;

                objEntity.RootPath = txtRootPath.Text;
                objEntity.SiteURL =  txtSiteURL.Text;
                objEntity.IndiaMartKey = txtIndiaMartKey.Text;
                objEntity.IndiaMartMobile = txtIndiaMartMobile.Text;
                objEntity.IndiaMartAcAlias = txtIndiaMartAcAlias.Text;

                objEntity.IndiaMartKey2 = txtIndiaMartKey2.Text;
                objEntity.IndiaMartMobile2 = txtIndiaMartMobile2.Text;
                objEntity.IndiaMartAcAlias2 = txtIndiaMartAcAlias2.Text;

                if (!String.IsNullOrEmpty(txtInstallationDate.Text))
                    objEntity.InstallationDate = Convert.ToDateTime(txtInstallationDate.Text);
               
                if (!String.IsNullOrEmpty(txtExpiryDate.Text))
                    objEntity.ExpiryDate = Convert.ToDateTime(txtExpiryDate.Text);
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CompanyRegistrationMgmt.AddUpdateCompanyRegistration(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------

                divErrorMessage.InnerHtml = ReturnMsg;

            }

        }
        public void OnlyViewControls()
        {
            txtCompanyName.ReadOnly = true;
            txtNoOfUsers.ReadOnly = true;
            txtSerialKey.ReadOnly = true;
            txtDBIP.ReadOnly = true;
            txtDBName.ReadOnly = true;
            txtDBUserName.ReadOnly = true;
            txtDBPassword.ReadOnly = true;

            txtRootPath.ReadOnly = true;
            txtSiteURL.ReadOnly = true;
            txtIndiaMartKey.ReadOnly = true;
            txtIndiaMartMobile.ReadOnly = true;
            txtIndiaMartAcAlias.ReadOnly = true;

            txtIndiaMartKey2.ReadOnly = true;
            txtIndiaMartMobile2.ReadOnly = true;
            txtIndiaMartAcAlias2.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void ClearAllField()
        {
            txtCompanyName.Text = "";
            txtNoOfUsers.Text = "";
            txtSerialKey.Text = "";
            txtDBIP.Text = "";
            txtDBName.Text = "";
            txtDBUserName.Text = "";
            txtDBPassword.Text = "";
            txtInstallationDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            txtExpiryDate.Text = String.Empty;
            txtRootPath.Text = "";
            txtSiteURL.Text = "";
            txtIndiaMartKey.Text = "";
            txtIndiaMartMobile.Text = "";
            txtIndiaMartAcAlias.Text = "";

            txtIndiaMartKey2.Text = "";
            txtIndiaMartMobile2.Text = "";
            txtIndiaMartAcAlias2.Text = "";

            txtInstallationDate.Focus();
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
    }
}