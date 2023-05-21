using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Projects : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                        ClearAllField();
                    else
                    {
                        setLayout("Edit");
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            if (Request.QueryString["mode"].ToString() == "view")
                                OnlyViewControls();
                        }
                    }
                }
            }
        }

        public void OnlyViewControls()
        {
            txtStartDate.ReadOnly = true;
            txtDueDate.ReadOnly = true;
            txtCompletionDate.ReadOnly = true;
            txtProjectName.ReadOnly = true;
            txtProjectDescription.ReadOnly = true;
           
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.Projects> lstEntity = new List<Entity.Projects>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.ProjectsMgmt.GetProjectsList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtProjectName.Text = lstEntity[0].ProjectName.ToString();
                txtProjectDescription.Text = lstEntity[0].ProjectDescription.ToString();
                txtStartDate.Text = String.IsNullOrEmpty(lstEntity[0].StartDate.ToString()) ? "" : Convert.ToDateTime(lstEntity[0].StartDate).ToString("dd-MM-yyyy");  
                //lstEntity[0].StartDate.ToString("dd-MM-yyyy");
                txtDueDate.Text = String.IsNullOrEmpty(lstEntity[0].DueDate.ToString()) ? "" : Convert.ToDateTime(lstEntity[0].DueDate).ToString("dd-MM-yyyy"); 
                txtCompletionDate.Text = String.IsNullOrEmpty(lstEntity[0].CompletionDate.ToString()) ? "" : Convert.ToDateTime(lstEntity[0].CompletionDate).ToString("dd-MM-yyyy"); 
                txtProjectName.Focus();
                // -----------------------------------------------------------------------------------
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";

            _pageValid = true;
            divErrorMessage.InnerHtml = "";

            if (!String.IsNullOrEmpty(txtStartDate.Text) && !String.IsNullOrEmpty(txtCompletionDate.Text))
            {
               
                if (Convert.ToDateTime(txtStartDate.Text) > Convert.ToDateTime(txtCompletionDate.Text))
                {
                    _pageValid = false;
                    divErrorMessage.Style.Remove("color");
                    divErrorMessage.Style.Add("color", "red");
                    divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Project Start Date is Always Less then Completion Date." + "</li>"));
                }
                    
            }

            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.Projects objEntity = new Entity.Projects();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.ProjectName = txtProjectName.Text;
                objEntity.ProjectDescription = txtProjectDescription.Text;
                objEntity.StartDate = String.IsNullOrEmpty(txtStartDate.Text.ToString()) ? (DateTime?)null : Convert.ToDateTime(txtStartDate.Text);
                //objEntity.StartDate = Convert.ToDateTime(txtStartDate.Text);
                //objEntity.DueDate = Convert.ToDateTime(txtDueDate.Text);
                objEntity.DueDate = String.IsNullOrEmpty(txtDueDate.Text.ToString()) ? (DateTime?)null : Convert.ToDateTime(txtDueDate.Text);
                //objEntity.CompletionDate = Convert.ToDateTime(txtCompletionDate.Text);
                objEntity.CompletionDate = String.IsNullOrEmpty(txtCompletionDate.Text.ToString()) ? (DateTime?)null : Convert.ToDateTime(txtCompletionDate.Text);
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ProjectsMgmt.AddUpdateProjects(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                divErrorMessage.InnerHtml = ReturnMsg;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDueDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCompletionDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtProjectName.Text = "";
            txtProjectDescription.Text = "";
            txtProjectName.Text = "";
            txtProjectName.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteProjects(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ProjectsMgmt.DeleteProjects(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

    }
}