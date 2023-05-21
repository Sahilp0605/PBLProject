using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenPop;
using System.Data;

namespace StarsProject
{
    public partial class DailyActivity : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public string objAuthEmployeeName,TotCategory;
        public decimal TotTaskDuration;
        int totrec;
        int ReturnCode = 0;
        string ReturnMsg = "";
        string strErr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["TotTaskDuration"] = 0;
                ViewState["TotCategory"] = "";
                TotTaskDuration = 0;
                TotCategory = "";

                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 100000;
                txtActivityDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                // -----------------------------------------------------------------
                hdnEmailTemplate.Value = (!String.IsNullOrEmpty(Request.QueryString["emailid"])) ? Request.QueryString["emailid"].ToString() : "";
                    // -----------------------------------------------------------------
                    if (!String.IsNullOrEmpty(Request.QueryString["MenuID"]))
                    hdnMenuID.Value = Request.QueryString["MenuID"].ToString().Trim();
                else
                    hdnMenuID.Value = "0";
                // -----------------------------------------------------------------
                //  Add / Edit / Delete Flag 
                // -----------------------------------------------------------------
                List<Entity.ApplicationMenu> lstMenu = new List<Entity.ApplicationMenu>();
                lstMenu = BAL.CommonMgmt.GetMenuAddEditDelList(Convert.ToInt64(hdnMenuID.Value), Session["LoginUserID"].ToString());
                hdnAddFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].AddFlag.ToString().ToLower() : "true";
                hdnEditFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].EditFlag.ToString().ToLower() : "true";
                hdnDelFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].DelFlag.ToString().ToLower() : "true";
                // ----------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                objAuthEmployeeName = objAuth.EmployeeName;
                hdnRole.Value = objAuth.RoleCode;
                hdnLoginUserID.Value = objAuth.UserID;
                drpEmployee.Focus();
                // ----------------------------------------------------------
                BindDropDown();
                // ----------------------------------------------------------
                BindDataGrid();
                // ----------------------------------------------------------
                ClearData();
            }
        }

        public void BindDataGrid()
        {
            Int64 tmpEmpID = 0;
            tmpEmpID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue) && drpEmployee.SelectedValue != "0") ? Convert.ToInt64(drpEmployee.SelectedValue) : 0;
            rptDailyActivity.DataSource = BAL.DailyActivityMgmt.GetDailyActivityList(0, tmpEmpID, Convert.ToDateTime(txtActivityDate.Text).ToString("yyyy-MM-dd"), Session["LoginUserID"].ToString(), 1, 100000, out totrec);
            rptDailyActivity.DataBind();
        }
        public void BindDropDown()
        {
            drpTaskCategory1.DataSource = BindTaskCategoryList();
            drpTaskCategory1.DataValueField = "pkID";
            drpTaskCategory1.DataTextField = "TaskCategoryName";
            drpTaskCategory1.DataBind();
            drpTaskCategory1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
            // ---------------- Assign TODO ------------------------
            List<Entity.ToDo> lstTODO = new List<Entity.ToDo>();
            lstTODO = BAL.ToDoMgmt.GetToDoListByUser(Session["LoginUserID"].ToString(), 0, 0);
            lstTODO = lstTODO.Where(x => (x.TaskStatus.ToLower() == "pending" || x.TaskStatus.ToLower() == "overdue")).ToList();

            drpTODO.DataSource = lstTODO;
            drpTODO.DataValueField = "pkID";
            drpTODO.DataTextField = "TaskDescription";
            drpTODO.DataBind();
            drpTODO.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

            // ---------------- Assign Employee ------------------------
            
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            if (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin")
                lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(hdnLoginUserID.Value);
            else
                lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList("", Session["LoginUserID"].ToString(), 1, 9999, out totrec);

            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();


        }
        public void ClearData()
        {
            hdnpkID.Value = "0";
            txtTaskDescription1.Text = "";
            txtTaskDuration1.Text = "";
            txtActivityDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            drpTaskCategory1.SelectedValue = "0";

            ViewState["TotTaskDuration"] = 0;
            TotTaskDuration = 0;
        }

        protected void rptDailyActivity_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            string strErr = "";

            if (e.CommandName.ToString() == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // -------------------------------------------------------------- Delete Record
                BAL.DailyActivityMgmt.DeleteDailyActivity(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    ClearData();
                }
                // -------------------------------------------------------------------------
                BindDataGrid();
            }
            else if (e.CommandName.ToString() == "Save")
            {
                hdnpkID.Value = ((HiddenField)e.Item.FindControl("hdnpkID")).Value;
                drpTaskCategory1.SelectedValue = ((HiddenField)e.Item.FindControl("hdnCatID")).Value;
                txtTaskDescription1.Text = ((Label)e.Item.FindControl("txtTaskDescription")).Text;
                txtTaskDuration1.Text = ((Label)e.Item.FindControl("txtTaskDuration")).Text;
            }
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
        }

        protected void txtActivityDate_TextChanged(object sender, EventArgs e)
        {
            BindDataGrid();
        }
        protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataGrid();
            txtActivityDate.Focus();
        }

        public List<Entity.TaskCategory> BindTaskCategoryList()
        {
            int TotalRec;
            List<Entity.TaskCategory> lstTaskCategory = new List<Entity.TaskCategory>();
            lstTaskCategory = BAL.DailyActivityMgmt.GetTaskCategoryList(0, "TODO", 1, 5000, out TotalRec);
            return lstTaskCategory;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _pageValid = true;
            int totrec;
            int ReturnCode = 0;
            string ReturnMsg = "";
            string strErr = "";
            string currEmpID = "", currUserID = "";
            currEmpID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? drpEmployee.SelectedValue : "0";
            List<Entity.OrganizationEmployee> lstCurr = new List<Entity.OrganizationEmployee>();
            lstCurr = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(Convert.ToInt64(currEmpID), 1, 9999, out totrec);
            currUserID = (lstCurr.Count > 0) ? lstCurr[0].UserID : "";

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["LoginDetail"];

            if (String.IsNullOrEmpty(txtActivityDate.Text) || String.IsNullOrEmpty(txtTaskDescription1.Text) ||
                String.IsNullOrEmpty(drpTaskCategory1.SelectedValue) || String.IsNullOrEmpty(txtTaskDuration1.Text) ||
                txtTaskDuration1.Text == "0" ||
                objAuth.EmployeeID.ToString() != drpEmployee.SelectedValue)
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtActivityDate.Text))
                    strErr += "<li>" + "Activity Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtTaskDescription1.Text))
                    strErr += "<li>" + "Description is required." + "</li>";

                if (String.IsNullOrEmpty(drpTaskCategory1.SelectedValue))
                    strErr += "<li>" + "Activity Category is Required." + "</li>";

                if (String.IsNullOrEmpty(txtTaskDuration1.Text) || txtTaskDuration1.Text == "0")
                    strErr += "<li>" + "Task Duration is required." + "</li>";

                if (objAuth.EmployeeID.ToString() != drpEmployee.SelectedValue)
                    strErr += "<li>" + "You Cannot Add Activity For Others." + "</li>";

                //if (txtTaskDescription1.Text.Length > 200)
                //    strErr += "<li>" + "Task Description should not greater than 200 characters." + "</li>";

            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.DailyActivity objEntity = new Entity.DailyActivity();
                objEntity.pkID = (!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0") ? Convert.ToInt64(hdnpkID.Value) : 0;
                objEntity.ActivityDate = Convert.ToDateTime(txtActivityDate.Text);
                objEntity.TaskDescription = txtTaskDescription1.Text;
                objEntity.TaskDuration = Convert.ToDecimal(txtTaskDuration1.Text);
                if (!String.IsNullOrEmpty(drpTaskCategory1.SelectedValue))
                    objEntity.TaskCategoryID = Convert.ToInt64(drpTaskCategory1.SelectedValue);
                objEntity.ToDOID = (!String.IsNullOrEmpty(drpTODO.SelectedValue) && drpTODO.SelectedValue != "0") ? Convert.ToInt64(drpTODO.SelectedValue) : 0;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                BAL.DailyActivityMgmt.AddUpdateDailyActivity(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
                // -------------------------------------------------------------------------
                if (ReturnCode > 0)
                {
                    ClearData();
                }
            }
            // ------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }

            // -------------------------------------------------------------------------
            BindDataGrid();
        }

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            _pageValid = true;

            string currEmpID = "", currUserID = "";
            currEmpID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? drpEmployee.SelectedValue : "0";
            currUserID = BAL.CommonMgmt.GetUserIDByEmployeeID(Convert.ToInt64(currEmpID));

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];

            DataTable dtDetail = new DataTable();
            List<Entity.DailyActivity> lst = BAL.DailyActivityMgmt.GetDailyActivityList(0, Convert.ToDateTime(txtActivityDate.Text).ToString("yyyy-MM-dd"), Session["LoginUserID"].ToString(), 1, 100000, out totrec);
            dtDetail = PageBase.ConvertListToDataTable(lst);
            Session.Add("dtDailyActivity", dtDetail);

            if (currUserID.ToLower() != objAuth.UserID.ToLower() || dtDetail.Rows.Count<=0)
            {
                _pageValid = false;

                if (currUserID.ToLower() != objAuth.UserID.ToLower())
                    strErr += "<li>" + "Login ID is not matching with Selected Employee." + "</li>";

                if (dtDetail.Rows.Count <= 0)
                    strErr += "<li>" + "No Entries Found." + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                try
                {
                    // Sending Email Notification ...
                    try
                    {
                        String respVal = "";
                        respVal = BAL.CommonMgmt.SendEmailNotify(hdnEmailTemplate.Value, Session["LoginUserID"].ToString(), dtDetail);

                        strErr += "<li>" + @ReturnMsg + " and Email Sent Successfully !" + "</li>";
                    }
                    catch (Exception ex)
                    {
                        strErr += "<li>" + @ReturnMsg + " and Sending Email Failed !" + "</li>";
                    }
                }
                catch (Exception ex)
                {
                    strErr += "<li>" + ((ReturnCode > 0) ? ReturnMsg : ReturnMsg) + " and Sending Email Failed !" + "</li>";
                }
                // ---------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            }
            // ------------------------------------------------------
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
            ClearData();
        }

        protected void rptDailyActivity_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Decimal v1;

                ImageButton btnUpd = ((ImageButton)e.Item.FindControl("btnUpdate"));
                ImageButton btnDel = ((ImageButton)e.Item.FindControl("btnDelete"));
                btnUpd.Visible = (hdnEditFlag.Value.ToLower() == "true") ? true : false;
                btnDel.Visible = (hdnDelFlag.Value.ToLower() == "true") ? true : false;

                v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "TaskDuration"));
                TotTaskDuration += v1;

                Decimal Hour = Math.Truncate(TotTaskDuration);
                Decimal Minuts = TotTaskDuration - Math.Truncate(TotTaskDuration);

                if (Minuts >= Convert.ToDecimal(0.6))
                {
                    Hour = Hour + 1;
                    Minuts = Minuts - Convert.ToDecimal(0.60);
                }

                TotTaskDuration = Convert.ToDecimal(Hour + Minuts);

                ViewState["TotTaskDuration"] = (!String.IsNullOrEmpty(TotTaskDuration.ToString())) ? Convert.ToDecimal(TotTaskDuration) : 0;

            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label txtTotTaskDuration = (Label)e.Item.FindControl("txtTotTaskDuration");
                Label lblTaskCat = (Label)e.Item.FindControl("lblTaskCat");

                

                txtTotTaskDuration.Text = TotTaskDuration.ToString("0.00");
                lblTaskCat.Text = "TOTAL TASK DURATION - ";
            }
        }

        
    }
}