using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class MyExternalLeads : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            hdnRole.Value = objAuth.RoleCode.ToLower();
            hdnLoginUserID.Value = objAuth.LoginUserID;
            // -----------------------------------------------------------------------
            if (!IsPostBack)
            {
                btnGenerateInquiry.Visible = (hdnView.Value == "dashboard") ? false : true;
            }
        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }

        public void BindExternalLeads()
        {
            int totrec;
            rptExternalLeads.DataSource = BAL.ExternalLeadsMgmt.GetExternalLeadList(0, "", "", 1, 99000, out totrec);
            rptExternalLeads.DataBind();
        }

        protected void btnGenerateInquiry_Click(object sender, EventArgs e)
        {
            SendLeadStatus();
        }

        public void SendLeadStatus()
        {
            foreach (RepeaterItem i in rptExternalLeads.Items)
            {
                Entity.ExternalLeads objEntity = new Entity.ExternalLeads();

                HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
                DropDownList ddl = ((DropDownList)i.FindControl("drpLeadStatus"));
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
                {
                    objEntity.pkID = Convert.ToInt64(hdnID.Value);
                    //objEntity.SenderName = ddl.SelectedValue;
                    //objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    //BAL.ExternalLeadsMgmt.UpdateSalesOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
                }
            }
        }

        public List<Entity.OrganizationEmployee> BindAssignToList()
        {
            // ---------------- Assign Employee ------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            return lstEmployee;
        }

        protected void rptExternalLeads_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("drpAssignTo"));
                //ddl.DataSource = BindAssignToList();
                //ddl.DataValueField = "pkID";
                //ddl.DataTextField = "EmployeeName";
                //ddl.DataBind();
                //ddl.Items.Insert(0, new ListItem("-- Select --", ""));

            }
        }
    }
}