using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Loan : System.Web.UI.Page
    {
        bool _pageValid = true;
        String strErr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindMonthYear();
                BindEmployee();
                // --------------------------------------------------------
                hdnMode.Value = (!String.IsNullOrEmpty(Request.QueryString["mode"])) ? Request.QueryString["mode"].ToString().ToLower() : "";
                hdnModuleView.Value = (!String.IsNullOrEmpty(Request.QueryString["view"])) ? Request.QueryString["view"].ToString().ToLower() : "";
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        drpFromMonth.SelectedValue = DateTime.Today.Month.ToString();
                        drpFromYear.SelectedValue = DateTime.Today.Year.ToString();
                        drpToMonth.SelectedValue = DateTime.Today.Month.ToString();
                        drpToYear.SelectedValue = DateTime.Today.Year.ToString();
                    }
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(hdnMode.Value))
                        {
                            if (hdnMode.Value == "view")
                                OnlyViewControls();
                        }
                    }
                }
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
            }
            // -----------------------------------------------
            if (hdnModuleView.Value != "loan")
            {
                divLoanType.Style.Add("display", "none");
                divEndDate.Style.Add("display", "none");
                divInstallment1.Style.Add("display", "none");
                divInstallment2.Style.Add("display", "none");
                divApprovalStatus.Style.Add("display", "none");
            }
        }
        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("January", "1"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("February", "2"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("March", "3"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("April", "4"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("May", "5"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("June", "6"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("July", "7"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("August", "8"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("September", "9"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("October", "10"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("November", "11"));
            drpFromMonth.Items.Add(new System.Web.UI.WebControls.ListItem("December", "12"));

            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("January", "1"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("February", "2"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("March", "3"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("April", "4"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("May", "5"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("June", "6"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("July", "7"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("August", "8"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("September", "9"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("October", "10"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("November", "11"));
            drpToMonth.Items.Add(new System.Web.UI.WebControls.ListItem("December", "12"));
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpFromYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                drpToYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            drpFromYear.SelectedValue = DateTime.Now.Year.ToString();
            drpToYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        public void BindEmployee()
        {
            // ---------------- Assign Employee ------------------------
            int totrec;
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList("", Session["LoginUserID"].ToString(), 1, 99999, out totrec);
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));

        }
        public void OnlyViewControls()
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            // -----------------------------------------------------------------------
            btnSave.Visible = (hdnMode.Value != "view") ? true : false;
            btnReset.Visible = (hdnMode.Value != "view") ? true : false;
            // -------------------------------------------------------------------
            drpEmployee.Attributes.Add("disabled", "disabled");
            drpFromMonth.Attributes.Add("disabled", "disabled");
            drpFromYear.Attributes.Add("disabled", "disabled");
            drpToMonth.Attributes.Add("disabled", "disabled");
            drpToYear.Attributes.Add("disabled", "disabled");
            txtLoanAmount.ReadOnly = true;
            txtNoOfInstallments.ReadOnly = true;
            txtInstallmentAmount.ReadOnly = true;
            txtRemarks.ReadOnly = true;
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;

                // -----------------------------------------------------------------------------------
                List<Entity.Loan> lstEntity = new List<Entity.Loan>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.LoanMgmt.GetLoan(hdnModuleView.Value, Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), "", Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtLoanAmount.Text = lstEntity[0].LoanAmount.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();
                lblApprovalStatus.Text = lstEntity[0].ApprovalStatus.ToString();

                drpFromMonth.Text = (lstEntity[0].StartDate.Year <= 1900) ? "" : lstEntity[0].StartDate.Month.ToString();
                drpFromYear.Text = (lstEntity[0].StartDate.Year <= 1900) ? "" : lstEntity[0].StartDate.Year.ToString();

                if (hdnModuleView.Value == "loan")
                {
                    drpToMonth.Text = (lstEntity[0].EndDate.Year <= 1900) ? "" : lstEntity[0].EndDate.Month.ToString();
                    drpToYear.Text = (lstEntity[0].EndDate.Year <= 1900) ? "" : lstEntity[0].EndDate.Year.ToString();

                    txtNoOfInstallments.Text = lstEntity[0].NoOfInstallments.ToString();
                    txtInstallmentAmount.Text = lstEntity[0].InstallmentAmount.ToString();
                }
                txtRemarks.Text = lstEntity[0].Remarks.ToString();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0; 
            string ReturnMsg = "";
            DateTime pStartDate, pEndDate;
            pStartDate = DateTime.Now;
            pEndDate = DateTime.Now;
            _pageValid = true;

            pStartDate = Convert.ToDateTime(drpFromYear.SelectedValue.ToString() + "-" + drpFromMonth.SelectedValue.ToString() + "-01");


            if (hdnModuleView.Value.ToLower() == "loan")
            {
                pEndDate = Convert.ToDateTime(drpToYear.SelectedValue.ToString() + "-" + drpToMonth.SelectedValue.ToString() + "-01");

                if (String.IsNullOrEmpty(drpEmployee.SelectedValue) || String.IsNullOrEmpty(pStartDate.ToString()) || String.IsNullOrEmpty(pEndDate.ToString())
                || String.IsNullOrEmpty(txtLoanAmount.Text))
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(drpEmployee.SelectedValue) || drpEmployee.SelectedValue == "0")
                        strErr += "<li>" + "Employee Selection is required." + "</li>";

                    if (String.IsNullOrEmpty(txtLoanAmount.Text))
                        strErr += "<li>" + "Loan Amount is required" + "</li>";

                }

                if (!String.IsNullOrEmpty(pStartDate.ToString()) || !String.IsNullOrEmpty(pEndDate.ToString()))
                {
                    if (pEndDate < pStartDate)
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Due Date should be greater than Start Date." + "</li>";
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(drpEmployee.SelectedValue) || String.IsNullOrEmpty(txtLoanAmount.Text))
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(drpEmployee.SelectedValue) || drpEmployee.SelectedValue == "0")
                        strErr += "<li>" + "Employee Selection is required." + "</li>";

                    if (String.IsNullOrEmpty(txtLoanAmount.Text))
                        strErr += "<li>" + "Loan Amount is required" + "</li>";

                }
            }
            // --------------------------------------------------------------
            if (_pageValid == true)
            {
                Entity.Loan objEntity = new Entity.Loan();

                objEntity.LoanCategory = hdnModuleView.Value;
                objEntity.LoanType = drpLoanType.SelectedValue;
                objEntity.pkID = (!String.IsNullOrEmpty(hdnpkID.Value)) ? Convert.ToInt64(hdnpkID.Value) : 0;
                objEntity.EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);
                objEntity.LoanAmount = (!String.IsNullOrEmpty(txtLoanAmount.Text)) ? Convert.ToDecimal(txtLoanAmount.Text) : 0;
                if (!String.IsNullOrEmpty(pStartDate.ToString()))
                    objEntity.StartDate = pStartDate;

                if (hdnModuleView.Value.ToLower() == "loan")
                { 
                    if (!String.IsNullOrEmpty(pEndDate.ToString()))
                        objEntity.EndDate = pEndDate;

                    objEntity.NoOfInstallments = (!String.IsNullOrEmpty(txtNoOfInstallments.Text)) ? Convert.ToInt32(txtNoOfInstallments.Text) : 0;
                    objEntity.InstallmentAmount = (!String.IsNullOrEmpty(txtInstallmentAmount.Text)) ? Convert.ToDecimal(txtInstallmentAmount.Text) : 0;
                }
                objEntity.Remarks = txtRemarks.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.LoanMgmt.AddUpdateLoan(objEntity, out ReturnCode, out ReturnMsg);

                strErr += "<li>" + ReturnMsg + "</li>";
                btnSave.Disabled = (ReturnCode > 0) ? true : false;
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        public void ClearAllField()
        {
            hdnpkID.Value = "";
            drpEmployee.SelectedValue = "";
            txtLoanAmount.Text = "";
            txtNoOfInstallments.Text = "";
            txtInstallmentAmount.Text = "";
            txtRemarks.Text = "";
            btnSave.Disabled = false;
        }
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);        
        }
        
        [System.Web.Services.WebMethod]
        public static string DeleteLoan(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.LoanMgmt.DeleteLoan(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void txtLoanAmount_TextChanged(object sender, EventArgs e)
        {

            if (hdnModuleView.Value == "loan")
            {
                DateTime pStartDate, pEndDate;
                pStartDate = Convert.ToDateTime(drpFromYear.SelectedValue.ToString() + "-" + drpFromMonth.SelectedValue.ToString() + "-01");
                pEndDate = Convert.ToDateTime(drpToYear.SelectedValue.ToString() + "-" + drpToMonth.SelectedValue.ToString() + "-01");

                if (pEndDate >= pStartDate && !String.IsNullOrEmpty(txtLoanAmount.Text))
                {
                    txtNoOfInstallments.Text = GetMonthDifference(pStartDate, pEndDate).ToString();
                    txtInstallmentAmount.Text = Math.Round(Convert.ToDecimal(txtLoanAmount.Text) / (Convert.ToInt32(txtNoOfInstallments.Text) > 0 ? Convert.ToInt32(txtNoOfInstallments.Text) : 1), 2).ToString();
                }
            }
        }
    }
}