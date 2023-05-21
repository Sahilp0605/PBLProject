using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Web.Services;

namespace StarsProject
{
    public partial class PayrollSlipView : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        private static DataTable dtDetail;

        Int64 pMon = 0, pYear = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
                hdnSerialKey.Value = Session["SerialKey"].ToString();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    // -------------------------------------------------------
                    if (!String.IsNullOrEmpty(Request.QueryString["month"]))
                    {
                        hdnMonth.Value = Request.QueryString["month"].ToString();
                        hdnMonth.Value = (hdnMonth.Value.Length == 1) ? "0" + hdnMonth.Value : hdnMonth.Value;
                    }

                    if (!String.IsNullOrEmpty(Request.QueryString["year"]))
                        hdnYear.Value = Request.QueryString["year"].ToString();


                    // -------------------------------------------------------
                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        EnableDisableControl();
                    }
                    else
                    {
                        EnableDisableControl();
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

        public void EnableDisableControl()
        {
            if (hdnSerialKey.Value == "H0PX-EMRW-23IJ-C1TD")
            {
                txtBasic.ReadOnly = true;
                txtHRA.ReadOnly = true;
                txtDA.ReadOnly = false;
                txtConveyance.ReadOnly = false;
                txtMedical.ReadOnly = false;
                txtSpecial.ReadOnly = false;
            }
            else
            {
                txtBasic.ReadOnly = true;
                if (hdnSerialKey.Value == "LVK4-MN01-K121-NGVL")
                    txtHRA.ReadOnly = false;
                else
                    txtHRA.ReadOnly = true;
                txtDA.ReadOnly = true;
                txtConveyance.ReadOnly = true;
                txtMedical.ReadOnly = true;
                txtSpecial.ReadOnly = true;
            }
        }

        public void OnlyViewControls()
        {
            txtPayDate.ReadOnly = true;
            drpEmployee.Attributes.Add("disabled", "disabled");
            txtWDays.ReadOnly = true;
            txtPDays.ReadOnly = true;
            txtLDays.ReadOnly = true;
            txtHDays.ReadOnly = true;
            txtFixedSalary.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            // ---------------- Report To List -------------------------------------
            List<Entity.OrganizationEmployee> lstOrgDept2 = new List<Entity.OrganizationEmployee>();
            lstOrgDept2 = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpEmployee.DataSource = lstOrgDept2;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;

                // -----------------------------------------------------------------------------------
                List<Entity.Payroll> lstEntity = new List<Entity.Payroll>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.PayrollMgmt.GetPayrollList(Convert.ToInt64(hdnpkID.Value), 0, 0, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtPayDate.Text = lstEntity[0].PayDate.ToString("yyyy-MM-dd");
                drpEmployee.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].EmployeeID.ToString())) ? lstEntity[0].EmployeeID.ToString() : "";
                lblBasicPer.Text = lstEntity[0].BasicPer.ToString();
                lblPDays.InnerText = (lblBasicPer.Text.ToLower() == "hourly") ? "Work Hrs" : "Pres.Days";
                lblTotDays.InnerText = (lblBasicPer.Text.ToLower() == "hourly") ? "Net Hrs" : "Net Days";

                txtWDays.Text = lstEntity[0].WDays.ToString();
                txtPDays.Text = lstEntity[0].PDays.ToString();
                txtLDays.Text = lstEntity[0].LDays.ToString();
                txtODays.Text = lstEntity[0].ODays.ToString();
                txtHDays.Text = lstEntity[0].HDays.ToString();
                txtTotDays.Text = ((lstEntity[0].PDays + lstEntity[0].HDays) - lstEntity[0].LDays).ToString();
                txtFixedSalary.Text = lstEntity[0].FixedSalary.ToString();

                txtBasic.Text = lstEntity[0].Basic.ToString();
                txtHRA.Text = lstEntity[0].HRA.ToString();
                txtConveyance.Text = lstEntity[0].Conveyance.ToString();
                txtMedical.Text = lstEntity[0].Medical.ToString();
                txtDA.Text = lstEntity[0].DA.ToString();
                txtSpecial.Text = lstEntity[0].Special.ToString();
                txtOverTime.Text = lstEntity[0].OverTime.ToString();
                txtTotal_Income.Text = lstEntity[0].Total_Income.ToString();

                txtPF.Text = lstEntity[0].PF.ToString();
                txtESI.Text = lstEntity[0].ESI.ToString();
                txtPT.Text = lstEntity[0].PT.ToString();
                txtTDS.Text = lstEntity[0].TDS.ToString();
                txtLoan.Text = lstEntity[0].Loan.ToString();
                txtTotal_Deduct.Text = lstEntity[0].Total_Deduct.ToString();

                txtNetSalary.Text = lstEntity[0].NetSalary.ToString();
                // -----------------------------------------------------------------
                hdnMonth.Value = Convert.ToDateTime(txtPayDate.Text).Month.ToString();
                hdnYear.Value = Convert.ToDateTime(txtPayDate.Text).Year.ToString();


            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";

            _pageValid = true;

            string strErr = "";

            if (String.IsNullOrEmpty(drpEmployee.SelectedValue) || String.IsNullOrEmpty(txtPayDate.Text) || String.IsNullOrEmpty(txtNetSalary.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(drpEmployee.SelectedValue))
                    strErr += "<li>" + "Employee Selection is required." + "</li>";

                if (String.IsNullOrEmpty(txtPayDate.Text))
                    strErr += "<li>" + "Salary Date is required." + "</li>";

                //if (String.IsNullOrEmpty(txtNetSalary.Text) || Convert.ToDecimal(txtNetSalary.Text) <= 0)
                //    strErr += "<li>" + "Net Pay Invalid." + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.Payroll objEntity = new Entity.Payroll();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.EmployeeID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : Convert.ToInt64("0");
                objEntity.PayDate = Convert.ToDateTime(txtPayDate.Text);
                objEntity.WDays = (!String.IsNullOrEmpty(txtWDays.Text)) ? Convert.ToInt64(txtWDays.Text) : 0;
                objEntity.PDays = (!String.IsNullOrEmpty(txtPDays.Text)) ? Convert.ToDecimal(txtPDays.Text) : 0;
                objEntity.HDays = (!String.IsNullOrEmpty(txtHDays.Text)) ? Convert.ToDecimal(txtHDays.Text) : 0;
                objEntity.ODays = (!String.IsNullOrEmpty(txtODays.Text)) ? Convert.ToDecimal(txtODays.Text) : 0;
                objEntity.LDays = (!String.IsNullOrEmpty(txtLDays.Text)) ? Convert.ToDecimal(txtLDays.Text) : 0;
                objEntity.FixedSalary = (!String.IsNullOrEmpty(txtFixedSalary.Text)) ? Convert.ToDecimal(txtFixedSalary.Text) : 0;

                objEntity.Basic = (!String.IsNullOrEmpty(txtBasic.Text)) ? Convert.ToDecimal(txtBasic.Text) : 0;
                objEntity.HRA = (!String.IsNullOrEmpty(txtHRA.Text)) ? Convert.ToDecimal(txtHRA.Text) : 0;
                objEntity.DA = (!String.IsNullOrEmpty(txtDA.Text)) ? Convert.ToDecimal(txtDA.Text) : 0;
                objEntity.Conveyance = (!String.IsNullOrEmpty(txtConveyance.Text)) ? Convert.ToDecimal(txtConveyance.Text) : 0;
                objEntity.Medical = (!String.IsNullOrEmpty(txtMedical.Text)) ? Convert.ToDecimal(txtMedical.Text) : 0;
                objEntity.Special = (!String.IsNullOrEmpty(txtSpecial.Text)) ? Convert.ToDecimal(txtSpecial.Text) : 0;
                objEntity.OverTime = (!String.IsNullOrEmpty(txtOverTime.Text)) ? Convert.ToDecimal(txtOverTime.Text) : 0;
                objEntity.Total_Income = Convert.ToDecimal(txtTotal_Income.Text);

                objEntity.PF = (!String.IsNullOrEmpty(txtPF.Text)) ? Convert.ToDecimal(txtPF.Text) : 0;
                objEntity.ESI = (!String.IsNullOrEmpty(txtESI.Text)) ? Convert.ToDecimal(txtESI.Text) : 0;
                objEntity.PT = (!String.IsNullOrEmpty(txtPT.Text)) ? Convert.ToDecimal(txtPT.Text) : 0;
                objEntity.TDS = (!String.IsNullOrEmpty(txtTDS.Text)) ? Convert.ToDecimal(txtTDS.Text) : 0;
                objEntity.Loan = (!String.IsNullOrEmpty(txtLoan.Text)) ? Convert.ToDecimal(txtLoan.Text) : 0;
                objEntity.Total_Deduct = Convert.ToDecimal(txtTotal_Deduct.Text);
                objEntity.NetSalary = Convert.ToDecimal(txtNetSalary.Text);

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.PayrollMgmt.AddUpdatePayroll(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                }
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
            //txtPayDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            var daysInMonths = DateTime.DaysInMonth(Convert.ToInt16(hdnYear.Value), Convert.ToInt16(hdnMonth.Value));
            txtPayDate.Text = hdnYear.Value + "-" + hdnMonth.Value + "-" + daysInMonths.ToString();
            drpEmployee.SelectedValue = "";
            txtWDays.Text = "";
            txtPDays.Text = "";
            txtHDays.Text = "";
            txtLDays.Text = "";
            txtODays.Text = "";
            txtFixedSalary.Text = "";
            txtBasic.Text = "";
            txtHRA.Text = "";
            txtDA.Text = "";
            txtConveyance.Text = "";
            txtMedical.Text = "";
            txtSpecial.Text = "";
            txtTotal_Income.Text = "";
            txtPF.Text = "";
            txtESI.Text = "";
            txtPT.Text = "";
            txtTDS.Text = "";
            txtLoan.Text = "";
            txtTotal_Deduct.Text = "";
            txtNetSalary.Text = "";
            drpEmployee.Focus();
            btnSave.Disabled = false;
        }

        protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculateEmployeeInfo();
        }

        protected void txtPayDate_TextChanged(object sender, EventArgs e)
        {
            GetAllDays();
            // -------------------------------------------
            calculateEmployeeInfo();
        }

        public void calculateEmployeeInfo()
        {
            if (!String.IsNullOrEmpty(drpEmployee.SelectedValue))
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                if (lstEntity.Count > 0)
                {

                    // ----------------------------------------------------------------
                    txtFixedSalary.Text = lstEntity[0].FixedSalary.ToString();
                    lblBasicPer.Text = lstEntity[0].BasicPer.ToString();
                    lblPDays.InnerText = (lblBasicPer.Text.ToLower() == "hourly") ? "Work Hrs" : "Pres.Days";
                    lblTotDays.InnerText = (lblBasicPer.Text.ToLower() == "hourly") ? "Net Hrs" : "Net Days";

                    // ----------------------------------------------------------------
                    GetAllDays();
                    // ----------------------------------------------------------------
                    if (!String.IsNullOrEmpty(txtWDays.Text) && !String.IsNullOrEmpty(txtPDays.Text) && !String.IsNullOrEmpty(txtFixedSalary.Text))
                    {
                        Int16 tmpDay1 = (!String.IsNullOrEmpty(txtWDays.Text)) ? Convert.ToInt16(txtWDays.Text) : Convert.ToInt16(0);
                        Decimal tmpDay2 = (!String.IsNullOrEmpty(txtPDays.Text)) ? Convert.ToDecimal(txtPDays.Text) : Convert.ToInt16(0);
                        Decimal tmpSal = (!String.IsNullOrEmpty(txtFixedSalary.Text)) ? Convert.ToDecimal(txtFixedSalary.Text) : 0;
                        // ----------------------------------------------------------
                        if (tmpDay1>0 && tmpDay2>0 && tmpSal > 0)
                            calculatePayslip();
                        else
                        {
                            txtBasic.Text = "";
                            txtHRA.Text = "";
                            txtDA.Text = "";
                            txtConveyance.Text = "";
                            txtMedical.Text = "";
                            txtSpecial.Text = "";
                            txtOverTime.Text = "";
                            txtTotal_Income.Text = "";
                            txtPF.Text = "";
                            txtESI.Text = "";
                            txtPT.Text = "";
                            txtTDS.Text = "";
                            txtLoan.Text = "";
                            txtTotal_Deduct.Text = "";
                            txtNetSalary.Text = "";
                        }
                    }
                }
            }
        }

        public void calculatePayslip()
        {
            if (!String.IsNullOrEmpty(drpEmployee.SelectedValue) && !String.IsNullOrEmpty(txtPayDate.Text) && !String.IsNullOrEmpty(txtWDays.Text))
            {
                int TotalCount = 0;
                bool PF_Calculation = false, PT_Calculation = false;
                // -----------------------------------------------------------------------------------
                List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
                lstEntity = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                if (lstEntity.Count > 0)
                {
                    PF_Calculation = lstEntity[0].PF_Calculation;
                    PT_Calculation = lstEntity[0].PT_Calculation;
                }
                // -----------------------------------------------------------------------------------

                Decimal tmpFixed = 0, tmpBasic = 0, tmpHRA = 0, tmpDA = 0, tmpConv = 0, tmpMedical = 0, tmpSpecial = 0, tmpOverTime = 0, tmpTotalInc = 0;
                Decimal tmpPF = 0, tmpESI = 0, tmpPT = 0, tmpTDS = 0, tmpLoan = 0, tmpTotalDed = 0;

                if (hdnSerialKey.Value == "H0PX-EMRW-23IJ-C1TD" || hdnSerialKey.Value == "SIV3-DIO4-09IK-98RE")    // Steelman Payslip Calculations
                {
                    tmpFixed = Math.Round((Convert.ToDecimal(txtPDays.Text) * Convert.ToDecimal(txtFixedSalary.Text)), 0);
                    tmpBasic = tmpFixed;
                    tmpHRA = Math.Round((Convert.ToDecimal(txtHDays.Text) * Convert.ToDecimal(txtFixedSalary.Text)), 0);
                    tmpConv = 0;
                    tmpMedical = 0;
                    if (!String.IsNullOrEmpty(drpEmployee.SelectedValue) && !String.IsNullOrEmpty(hdnMonth.Value) && !String.IsNullOrEmpty(hdnYear.Value))
                        tmpSpecial = Convert.ToDecimal(BAL.CommonMgmt.GetDrivingAllowance(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(hdnMonth.Value), Convert.ToInt64(hdnYear.Value)));
                    else
                        tmpSpecial = 0;

                    tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial));

                    tmpPF = 0;
                    tmpESI = 0;
                    tmpPT = 0;
                    tmpTDS = 0;
                    tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToDateTime(txtPayDate.Text).Month, Convert.ToDateTime(txtPayDate.Text).Year); 

                    tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                }
                else if (hdnSerialKey.Value == "PI01-YU02-RUBB-03ER")    // Piyush Rubber
                {
                    Decimal wd, pd, hd, od, ld, tdays, fixsal;
                    fixsal = Convert.ToDecimal(txtFixedSalary.Text);
                    wd = (!String.IsNullOrEmpty(txtWDays.Text)) ? Convert.ToDecimal(txtWDays.Text) : 0;
                    pd = (!String.IsNullOrEmpty(txtPDays.Text)) ? Convert.ToDecimal(txtPDays.Text) : 0;
                    hd = (!String.IsNullOrEmpty(txtHDays.Text)) ? Convert.ToDecimal(txtHDays.Text) : 0;
                    od = (!String.IsNullOrEmpty(txtODays.Text)) ? Convert.ToDecimal(txtODays.Text) : 0;
                    ld = (!String.IsNullOrEmpty(txtLDays.Text)) ? Convert.ToDecimal(txtLDays.Text) : 0;
                    tdays = pd;

                    if (lblBasicPer.Text.ToLower() == "daily")
                        tmpFixed = Math.Round((fixsal * tdays), 0);
                    // --------------------------------------------------------------------------------
                    tmpBasic = tmpFixed;
                    tmpHRA = 0;
                    tmpConv = 0;
                    tmpMedical = 0;
                    tmpSpecial = 0;
                    if (od > 0)
                    {
                        Decimal OTHrsRate = 0;
                        OTHrsRate = BAL.OverTimeMgmt.GetOverTimeAllow(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(hdnMonth.Value), Convert.ToInt64(hdnYear.Value));
                        // --------------------------------------------------
                        decimal myHrs = 0, myHrs1 = 0;
                        Decimal totot = 0, myMins = 0, myMins1 = 0, myMins2 = 0;

                        myHrs = Math.Floor(od);    // 2.15
                        myMins = ((od - myHrs) * 100);     // 15

                        //myMins1 = Convert.ToInt64((myMins * 100)/60); // 25
                        // --------------------------------------------------
                        tmpOverTime = Math.Round((myHrs * OTHrsRate) + ((myMins * OTHrsRate)/60), 0);
                    }

                    tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));


                    if (PF_Calculation == true)
                    {
                        tmpPF = (tmpTotalInc * 12 / 100);
                    }

                    tmpESI = 0;

                    if (PT_Calculation == true)
                    {
                        if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                            tmpPT = 80;
                        else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                            tmpPT = 150;
                        else if (tmpTotalInc > 11999)
                            tmpPT = 200;
                    }

                    tmpTDS = 0;
                    tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("", Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToDateTime(txtPayDate.Text).Month, Convert.ToDateTime(txtPayDate.Text).Year);

                    tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                }
                else if (hdnSerialKey.Value == "LVK4-MN01-K121-NGVL")    // M.N.Rubber
                {
                    Decimal wd, pd, hd, od, ld, tdays, fixsal;
                    fixsal = Convert.ToDecimal(txtFixedSalary.Text);
                    wd = (!String.IsNullOrEmpty(txtWDays.Text)) ? Convert.ToDecimal(txtWDays.Text) : 0;
                    pd = (!String.IsNullOrEmpty(txtPDays.Text)) ? Convert.ToDecimal(txtPDays.Text) : 0;
                    hd = (!String.IsNullOrEmpty(txtHDays.Text)) ? Convert.ToDecimal(txtHDays.Text) : 0;
                    od = (!String.IsNullOrEmpty(txtODays.Text)) ? Convert.ToDecimal(txtODays.Text) : 0;
                    ld = (!String.IsNullOrEmpty(txtLDays.Text)) ? Convert.ToDecimal(txtLDays.Text) : 0;
                    tdays = ((pd + hd) - ld);

                    if (lblBasicPer.Text.ToLower() == "daily")
                        tmpFixed = Math.Round((fixsal * tdays), 0);
                    else
                        tmpFixed = Math.Round(((fixsal / wd) * tdays), 0);
                    // --------------------------------------------------------------------------------
                    tmpBasic = tmpFixed;
                    tmpHRA = 0;
                    tmpConv = 0;
                    tmpMedical = 0;
                    tmpSpecial = 0;
                    if (od > 0)
                    {
                        Decimal OTHrsRate = 0;
                        OTHrsRate = BAL.OverTimeMgmt.GetOverTimeAllow(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(hdnMonth.Value), Convert.ToInt64(hdnYear.Value));
                        tmpOverTime = Math.Round((od * OTHrsRate), 0);
                    }

                    tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));


                    if (PF_Calculation == true)
                    {
                        //tmpPF = (tmpTotalInc * 12 / 100);
                        tmpPF = ((Convert.ToDecimal(tmpBasic) - Convert.ToDecimal(tmpHRA)) * 12 / 100);
                    }

                    tmpESI = 0;

                    if (PT_Calculation == true)
                    {
                        if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                            tmpPT = 80;
                        else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                            tmpPT = 150;
                        else if (tmpTotalInc > 11999)
                            tmpPT = 200;
                    }

                    tmpTDS = 0;
                    tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("", Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToDateTime(txtPayDate.Text).Month, Convert.ToDateTime(txtPayDate.Text).Year); 

                    tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);

                }
                else if (hdnSerialKey.Value == "SO5H-DH90-E34L-SIOF")    // Soleos
                {
                    Decimal wd, pd, hd, od, ld, tdays, fixsal, fixbasic, fixhra, fixconv, fixspecial;
                    
                    wd = (!String.IsNullOrEmpty(txtWDays.Text)) ? Convert.ToDecimal(txtWDays.Text) : 0;
                    pd = (!String.IsNullOrEmpty(txtPDays.Text)) ? Convert.ToDecimal(txtPDays.Text) : 0;
                    hd = (!String.IsNullOrEmpty(txtHDays.Text)) ? Convert.ToDecimal(txtHDays.Text) : 0;
                    od = (!String.IsNullOrEmpty(txtODays.Text)) ? Convert.ToDecimal(txtODays.Text) : 0;
                    ld = (!String.IsNullOrEmpty(txtLDays.Text)) ? Convert.ToDecimal(txtLDays.Text) : 0;
                    tdays = ((pd + hd));
                    // ---------------------------------------------------------------
                    fixsal = Convert.ToDecimal(txtFixedSalary.Text);
                    fixbasic = lstEntity[0].FixedBasic;
                    fixhra = lstEntity[0].FixedHRA;
                    fixconv = lstEntity[0].FixedConv;
                    fixspecial = lstEntity[0].FixedSpecial;
                    // ---------------------------------------------------------------
                    // Income Side
                    // ---------------------------------------------------------------
                    tmpBasic = Math.Round(((fixbasic / wd) * tdays), 0);
                    tmpHRA = Math.Round(((fixhra / wd) * tdays), 0);
                    tmpConv = Math.Round(((fixconv / wd) * tdays), 0);
                    tmpMedical = 0;
                    tmpSpecial = Math.Round(((fixspecial / wd) * tdays), 0);
                    //if (od > 0)
                    //{
                    //    Decimal OTHrsRate = 0;
                    //    OTHrsRate = BAL.OverTimeMgmt.GetOverTimeAllow(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(hdnMonth.Value), Convert.ToInt64(hdnYear.Value));
                    //    tmpOverTime = Math.Round((od * OTHrsRate), 0);
                    //}
                    tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));

                    // ---------------------------------------------------------------
                    // Deduction Side
                    // ---------------------------------------------------------------
                    //tmpPF = (PF_Calculation == true) ? (tmpTotalInc * 12 / 100) : 0;
                    tmpPF = 0;
                    tmpESI = 0;
                    if (PT_Calculation == true)
                    {
                        if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                            tmpPT = 80;
                        else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                            tmpPT = 150;
                        else if (tmpTotalInc > 11999)
                            tmpPT = 200;
                    }
                    tmpTDS = 0;
                    tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToDateTime(txtPayDate.Text).Month, Convert.ToDateTime(txtPayDate.Text).Year);
                    tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                }
                else if (hdnSerialKey.Value == "PRI9-DG8H-G6GF-TP5V")    // Perfect Rotto Motors
                {
                    Decimal wd, pd, hd, od, ld, tdays, fixsal, fixbasic, fixhra, fixconv, fixspecial;

                    wd = (!String.IsNullOrEmpty(txtWDays.Text)) ? Convert.ToDecimal(txtWDays.Text) : 0;
                    pd = (!String.IsNullOrEmpty(txtPDays.Text)) ? Convert.ToDecimal(txtPDays.Text) : 0;
                    hd = (!String.IsNullOrEmpty(txtHDays.Text)) ? Convert.ToDecimal(txtHDays.Text) : 0;
                    od = (!String.IsNullOrEmpty(txtODays.Text)) ? Convert.ToDecimal(txtODays.Text) : 0;
                    ld = (!String.IsNullOrEmpty(txtLDays.Text)) ? Convert.ToDecimal(txtLDays.Text) : 0;
                    tdays = ((pd + hd));
                    // ---------------------------------------------------------------
                    fixsal = (!String.IsNullOrEmpty(txtFixedSalary.Text)) ? Convert.ToDecimal(txtFixedSalary.Text) : 0;
                    fixbasic = fixsal;
                    fixhra = 0;
                    fixconv = 0;
                    fixspecial = 0;
                    // ---------------------------------------------------------------
                    // Income Side
                    // ---------------------------------------------------------------
                    tmpBasic = Math.Round((fixbasic * pd), 0);
                    tmpHRA = 0;
                    tmpConv = 0;
                    tmpMedical = 0;
                    tmpSpecial = 0;
                    tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));

                    // ---------------------------------------------------------------
                    // Deduction Side
                    // ---------------------------------------------------------------
                    tmpPF = 0;
                    tmpESI = 0;
                    if (PT_Calculation == true)
                    {
                        if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                            tmpPT = 80;
                        else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                            tmpPT = 150;
                        else if (tmpTotalInc > 11999)
                            tmpPT = 200;
                    }
                    tmpTDS = 0;
                    tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToDateTime(txtPayDate.Text).Month, Convert.ToDateTime(txtPayDate.Text).Year);
                    tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                }
                else
                {                                              // Others Payslip Calculations
                    tmpFixed = Math.Round((((Convert.ToDecimal(txtPDays.Text) + Convert.ToDecimal(txtHDays.Text)) * Convert.ToDecimal(txtFixedSalary.Text)) / Convert.ToInt64(txtWDays.Text)), 0);
                    tmpBasic = Math.Round((40 * tmpFixed) / 100, 0);
                    tmpHRA = Math.Round((50 * tmpBasic) / 100, 0);
                    tmpConv = (tmpBasic > 0 && tmpHRA > 0) ? 1600 : 0;
                    tmpMedical = tmpBasic > 0 && tmpHRA > 0 ? 1250 : 0;
                    tmpSpecial = tmpBasic > 0 && tmpHRA > 0 ? (Convert.ToDecimal(tmpFixed) - (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical))) : 0;
                    tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial));

                    if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                        tmpPT = 80;
                    else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                        tmpPT = 150;
                    else if (tmpTotalInc > 11999)
                        tmpPT = 200;
                    tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToDateTime(txtPayDate.Text).Month, Convert.ToDateTime(txtPayDate.Text).Year);
                    tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                }
                // -------------------------------------------------
                txtBasic.Text = tmpBasic.ToString();
                txtHRA.Text = tmpHRA.ToString();
                txtDA.Text = tmpDA.ToString();
                txtConveyance.Text = tmpConv.ToString();
                txtMedical.Text = tmpMedical.ToString();
                txtSpecial.Text = tmpSpecial.ToString();
                txtOverTime.Text = tmpOverTime.ToString();
                txtTotal_Income.Text = tmpTotalInc.ToString();

                txtPF.Text = tmpPF.ToString();
                txtESI.Text = tmpESI.ToString();
                txtPT.Text = tmpPT.ToString();
                txtTDS.Text = tmpTDS.ToString();
                txtLoan.Text = tmpLoan.ToString();
                txtTotal_Deduct.Text = tmpTotalDed.ToString();

                txtNetSalary.Text = (Convert.ToDecimal(tmpTotalInc) - Convert.ToDecimal(tmpTotalDed)).ToString();
            }
        }

        protected void txtPDays_TextChanged(object sender, EventArgs e)
        {
            Decimal pd, hd, od, ld;
            pd = (!String.IsNullOrEmpty(txtPDays.Text)) ? Convert.ToDecimal(txtPDays.Text) : 0;
            hd = (!String.IsNullOrEmpty(txtHDays.Text)) ? Convert.ToDecimal(txtHDays.Text) : 0;
            od = (!String.IsNullOrEmpty(txtODays.Text)) ? Convert.ToDecimal(txtODays.Text) : 0;
            ld = (!String.IsNullOrEmpty(txtLDays.Text)) ? Convert.ToDecimal(txtLDays.Text) : 0;
            txtTotDays.Text = (pd + hd).ToString();
            // --------------------------------------
            calculatePayslip();
        }
        protected void EarningDeduction_TextChanged(object sender, EventArgs e)
       {
            Decimal tmpFixed = 0, tmpBasic = 0, tmpHRA = 0, tmpDA = 0, tmpConv = 0, tmpMedical = 0, tmpSpecial = 0, tmpTotalInc = 0, tmpOverTime;
            Decimal tmpPF = 0, tmpESI = 0, tmpPT = 0, tmpTDS = 0, tmpLoan = 0, tmpTotalDed = 0;

            tmpBasic = String.IsNullOrEmpty(txtBasic.Text) ? 0 : Convert.ToDecimal(txtBasic.Text);
            tmpHRA = String.IsNullOrEmpty(txtHRA.Text) ? 0 : Convert.ToDecimal(txtHRA.Text);
            tmpDA = String.IsNullOrEmpty(txtDA.Text) ? 0 : Convert.ToDecimal(txtDA.Text);
            tmpConv = String.IsNullOrEmpty(txtConveyance.Text) ? 0 : Convert.ToDecimal(txtConveyance.Text);
            tmpMedical = String.IsNullOrEmpty(txtMedical.Text) ? 0 : Convert.ToDecimal(txtMedical.Text);
            tmpSpecial = String.IsNullOrEmpty(txtSpecial.Text) ? 0 : Convert.ToDecimal(txtSpecial.Text);
            tmpTotalInc = String.IsNullOrEmpty(txtTotal_Income.Text) ? 0 : Convert.ToDecimal(txtTotal_Income.Text);
            tmpOverTime = String.IsNullOrEmpty(txtOverTime.Text) ? 0 : Convert.ToDecimal(txtOverTime.Text);

            tmpPF = String.IsNullOrEmpty(txtPF.Text) ? 0 : Convert.ToDecimal(txtPF.Text);
            tmpESI = String.IsNullOrEmpty(txtESI.Text) ? 0 : Convert.ToDecimal(txtESI.Text);
            tmpPT = String.IsNullOrEmpty(txtPT.Text) ? 0 : Convert.ToDecimal(txtPT.Text);
            tmpTDS = String.IsNullOrEmpty(txtTDS.Text) ? 0 : Convert.ToDecimal(txtTDS.Text);
            tmpLoan = String.IsNullOrEmpty(txtLoan.Text) ? 0 : Convert.ToDecimal(txtLoan.Text);
            tmpTotalDed = String.IsNullOrEmpty(txtTotal_Deduct.Text) ? 0 : Convert.ToDecimal(txtTotal_Deduct.Text);

            if (hdnSerialKey.Value == "LVK4-MN01-K121-NGVL")     // M N RUBBER
            {
                tmpTotalInc = (tmpBasic + tmpConv + tmpMedical + tmpSpecial + tmpOverTime) - tmpHRA;
                txtTotal_Income.Text = tmpTotalInc.ToString();
                tmpTotalDed = (Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpPT)) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                txtTotal_Deduct.Text = ((Convert.ToDecimal(tmpTotalDed))).ToString();
                txtNetSalary.Text = ((Convert.ToDecimal(tmpTotalInc) - Convert.ToDecimal(tmpTotalDed)) + Convert.ToDecimal(tmpHRA)).ToString();
            }
            else
            {
                tmpTotalInc = (tmpBasic + tmpHRA + tmpConv + tmpMedical + tmpSpecial + tmpOverTime);
                txtTotal_Income.Text = tmpTotalInc.ToString();
                tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                txtTotal_Deduct.Text = tmpTotalDed.ToString();
                txtNetSalary.Text = (Convert.ToDecimal(tmpTotalInc) - Convert.ToDecimal(tmpTotalDed)).ToString();
            }
            
        }

        /* *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-* */
        // Calculation Functions  
        /* *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-* */
        public void GetAllDays()
        {
            GetWorkingDays();
            GetPresenceDays();
            GetHolidays();
            GetOverTimeDays();
            GetLeaveDays();
            // -------------------------------------------------------
            decimal wd, hd, ld, pd;
            wd = (!String.IsNullOrEmpty(txtWDays.Text)) ? Convert.ToDecimal(txtWDays.Text) : Convert.ToDecimal(0);
            pd = (!String.IsNullOrEmpty(txtPDays.Text)) ? Convert.ToDecimal(txtPDays.Text) : Convert.ToDecimal(0);
            hd = (!String.IsNullOrEmpty(txtHDays.Text)) ? Convert.ToDecimal(txtHDays.Text) : Convert.ToDecimal(0);
            ld = (!String.IsNullOrEmpty(txtLDays.Text)) ? Convert.ToDecimal(txtLDays.Text) : Convert.ToDecimal(0);
            txtTotDays.Text = (pd + hd).ToString();
            // -------------------------------------------------------
            calculatePayslip();
        }

        public void GetWorkingDays()
        {
            if (!string.IsNullOrEmpty(txtPayDate.Text))
            {
                // if (hdnSerialKey.Value == "H0PX-EMRW-23IJ-C1TD") 
                pMon = Convert.ToDateTime(txtPayDate.Text).Month;
                pYear = Convert.ToDateTime(txtPayDate.Text).Year;

                var daysInMonths = DateTime.DaysInMonth(Convert.ToInt16(pYear), Convert.ToInt16(pMon));
                txtWDays.Text = daysInMonths.ToString();
            }

        }
        public void GetPresenceDays()
        {
            if (!string.IsNullOrEmpty(txtPayDate.Text))
            {
                pMon = Convert.ToDateTime(txtPayDate.Text).Month;
                pYear = Convert.ToDateTime(txtPayDate.Text).Year;

                if (!String.IsNullOrEmpty(drpEmployee.SelectedValue))
                {
                    //-------------------------Present Days (From Attendance) ----------------------------------------------------
                    List<Entity.Attendance> lstEntity = new List<Entity.Attendance>();
                    lstEntity = BAL.AttendanceMgmt.GetAttendanceList(0, (Convert.ToInt64(drpEmployee.SelectedValue)), Convert.ToInt16(pMon), Convert.ToInt16(pYear));
                    //txtPDays.Text = lstEntity.Where(item => item.TimeIn != null && item.TimeIn != "").Count().ToString();
                    if (lblBasicPer.Text.ToLower() == "monthly" || lblBasicPer.Text.ToLower() == "daily")
                        txtPDays.Text = lstEntity.Sum(item => item.WorkingHrsFlag).ToString();
                    else
                    {
                        decimal myHrs = 0, myHrs1 = 0;
                        Decimal totot = 0, myMins = 0, myMins1 = 0, myMins2 = 0;
                        
                        myHrs = lstEntity.Sum(item => Math.Floor(item.WorkingTotalHrs));    // 180
                        myMins1 = lstEntity.Sum(item => ((item.WorkingTotalHrs - Math.Floor(item.WorkingTotalHrs)) * 100));     // 145

                        myHrs1 = Math.Floor(myMins1 / 60);  // 2
                        myMins2 = Convert.ToInt64(myMins1 - (myHrs1 * 60)); // 25
                        
                        txtPDays.Text = (myHrs + myHrs1).ToString("00") + "." + myMins2.ToString("0");
                    }
                        
                }
            }
        }
        public void GetHolidays()
        {
            if (hdnSerialKey.Value != "LVK4-MN01-K121-NGVL")
            {
                if (!string.IsNullOrEmpty(txtPayDate.Text))
                {
                    pMon = Convert.ToDateTime(txtPayDate.Text).Month;
                    pYear = Convert.ToDateTime(txtPayDate.Text).Year;
                    if (!String.IsNullOrEmpty(drpEmployee.SelectedValue))
                    {
                        List<Entity.Holiday> lstEntity = new List<Entity.Holiday>();
                        lstEntity = BAL.HolidayMgmt.GetHolidayListByCount(Convert.ToInt64(pMon), Convert.ToInt64(pYear));
                        Decimal TotalHolidays = lstEntity.Sum(item => item.TotalHolidays) + GetSundays();
                        txtHDays.Text = TotalHolidays.ToString();
                    }
                }
                else
                {
                    txtHDays.Text = "0";
                }
            }
            else
            {
                txtHDays.Text = "0";
            }
            // ----------------------------------------------------------
            if (lblBasicPer.Text.ToLower() == "hourly")
                txtHDays.Text = "0";
        }

        public void GetOverTimeDays()
        {
            txtODays.Text = "0";
            pMon = Convert.ToDateTime(txtPayDate.Text).Month;
            pYear = Convert.ToDateTime(txtPayDate.Text).Year;

            if (!String.IsNullOrEmpty(drpEmployee.SelectedValue))
            {
                Decimal retVal = 0;
                Decimal myHrs = 0, myMins = 0;
                retVal = BAL.OverTimeMgmt.GetOverTimeHours(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(pMon), Convert.ToInt64(pYear));

                myHrs = Math.Floor(retVal/60);
                myMins = (retVal - (myHrs*60));
                txtODays.Text = Convert.ToUInt64(myHrs).ToString() + "." + Convert.ToInt64(myMins).ToString();
            }
            else
            {
                txtODays.Text = "0";
            }
        }
        public void GetOverTimeAllow()
        {
            txtODays.Text = "0";
            pMon = Convert.ToDateTime(txtPayDate.Text).Month;
            pYear = Convert.ToDateTime(txtPayDate.Text).Year;

            if (!String.IsNullOrEmpty(drpEmployee.SelectedValue))
            {
                Decimal retVal;
                retVal = BAL.OverTimeMgmt.GetOverTimeAllow(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(pMon), Convert.ToInt64(pYear));

                txtOverTime.Text = Math.Round(retVal, 2).ToString();
            }
            else
            {
                txtOverTime.Text = "0";
            }
        }
        public void GetLeaveDays()
        {
            Decimal TotalLeaveDays = 0;
            pMon = Convert.ToDateTime(txtPayDate.Text).Month;
            pYear = Convert.ToDateTime(txtPayDate.Text).Year;

            if (!String.IsNullOrEmpty(drpEmployee.SelectedValue))
            {
                List<Entity.LeaveRequest> lstEntity = new List<Entity.LeaveRequest>();
                lstEntity = BAL.LeaveRequestMgmt.GetLeaveRequestListByEmployeeID(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt16(pMon), Convert.ToInt16(pYear));
                if (hdnSerialKey.Value == "SO5H-DH90-E34L-SIOF")    // Soleos
                    TotalLeaveDays = lstEntity.Where(p => (p.PaidUnpaid.ToLower() == "unpaid" && p.ApprovalStatus.ToLower() == "approved")).Sum(item => item.LeaveDays);
                else
                    TotalLeaveDays = lstEntity.Sum(item => item.LeaveDays);
                // --------------------------------------------------------------
                txtLDays.Text = TotalLeaveDays.ToString();
            }
            else
            {
                txtLDays.Text = "0";
            }
        }

        protected void txtHRA_TextChanged(object sender, EventArgs e)
        {
            if (hdnSerialKey.Value == "LVK4-MN01-K121-NGVL")
            {
                Decimal Basic = (!String.IsNullOrEmpty(txtBasic.Text)) ? Convert.ToDecimal(txtBasic.Text) : 0;
                Decimal HRA = (!String.IsNullOrEmpty(txtHRA.Text)) ? Convert.ToDecimal(txtHRA.Text) : 0;
                Decimal OverTime = (!String.IsNullOrEmpty(txtOverTime.Text)) ? Convert.ToDecimal(txtOverTime.Text) : 0;

                Decimal PF = (((Basic + OverTime) - HRA) * 12) / 100;
                txtPF.Text = PF.ToString("0.00");
            }
            EarningDeduction_TextChanged(null, null);
        }

        //--------------Count Sunday for Current Month in Present Days.----------------------------------------------
        public int GetSundays()
        {
            int returnVal = 0, month = 0, year = 0;

            if (!string.IsNullOrEmpty(txtPayDate.Text))
            {
                month = Convert.ToDateTime(txtPayDate.Text).Month;
                year = Convert.ToDateTime(txtPayDate.Text).Year;
                if (year > 0 && month > 0)
                {
                    var firstDay = new DateTime(year, month, 1);

                    var day29 = firstDay.AddDays(28);
                    var day30 = firstDay.AddDays(29);
                    var day31 = firstDay.AddDays(30);

                    if ((day29.Month == month && day29.DayOfWeek == DayOfWeek.Sunday)
                    || (day30.Month == month && day30.DayOfWeek == DayOfWeek.Sunday)
                    || (day31.Month == month && day31.DayOfWeek == DayOfWeek.Sunday))
                    {
                        returnVal = 5;
                    }
                    else
                    {
                        returnVal = 4;
                    }
                }
            }
            return returnVal;
        }



    }
}