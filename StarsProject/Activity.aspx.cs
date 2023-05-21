using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Activity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindValidation();
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString();
                Session["OldUserID"] = "";
                // ----------------------------------------
                ClearAllField();
                BindUserData();
            }
        }
        
        public void BindUserData()
        {
            List<Entity.Activity> lstEntity = new List<Entity.Activity>();
            int TotalCount = 0;
            lstEntity = BAL.ActivityMgmt.GetActivity("");
            rptUser.DataSource = lstEntity;
            rptUser.DataBind();
            pageGrid.BindPageing(TotalCount);
        }

        public void BindValidation()
        {
            //rfvFirstName.ErrorMessage = ErrorMessages.ShowRequiredFieldMsg("Activity Code");
            //regFirstName.ValidationExpression = RegularExpression.alphanumericexpresion();
            //regFirstName.ErrorMessage = ErrorMessages.ShowValidMsg("Activity Code");

            //rfvLastName.ErrorMessage = ErrorMessages.ShowRequiredFieldMsg("Activity Name");
            //regLastName.ValidationExpression = RegularExpression.alphanumericexpresion();
            //regLastName.ErrorMessage = ErrorMessages.ShowValidMsg("Activity Name");

        }


        public void ClearAllField()
        {
            Session["OldUserID"] = "";
            txtActivityCode.Text = "";
            txtActivityName.Text = "";
            
            chkActive.Checked = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            // -----------------------------------------------------
            Entity.Activity objEntity = new Entity.Activity();

            objEntity.ActivityCode = txtActivityCode.Text.Trim();
            objEntity.ActivityName = txtActivityName.Text.Trim();
            objEntity.ActiveFlag = chkActive.Checked;
            objEntity.LoginUserID = Session["LoginUserID"].ToString();

            // --------------------------------- Insert/Update 
            BAL.ActivityMgmt.AddUpdateActivity(objEntity, out ReturnCode, out ReturnMsg);
            // ---------------------------------
            ScriptManager.RegisterStartupScript(this, typeof(string), "msg", "javascript:showmessage('" + ReturnMsg + "');", true);
            BindUserData();
            ClearAllField();

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void rptUser_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Edit")
            {
                List<Entity.Activity> lstEntity = new List<Entity.Activity>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.ActivityMgmt.GetActivity(e.CommandArgument.ToString());

                txtActivityCode.Text = lstEntity[0].ActivityCode;
                txtActivityName.Text = lstEntity[0].ActivityName;
                chkActive.Checked = lstEntity[0].ActiveFlag;
                // -------------------------------------------------------------------------
                BindUserData();
            }
            else if (e.CommandName.ToString() == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // ----------------------------------- Delete Record
                BAL.ActivityMgmt.DeleteActivity(e.CommandArgument.ToString(), out ReturnCode, out ReturnMsg);
                ScriptManager.RegisterStartupScript(this, typeof(string), "msg", "javascript:showmessage('" + ReturnMsg + "');", true);
                // -------------------------------------------------------------------------
                BindUserData();
            }
        }

        protected void Pager_Changed(object sender, PagerEventArgs e)
        {
            BindUserData();
            ScriptManager.RegisterStartupScript(this, typeof(string), "color", "javascript:changecolor();", true);
        }
    }
}