using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{ 
    public partial class Proof : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        

        protected void Page_Load(object sender, EventArgs e)
        {

            BindValidation();
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["OldUserID"] = "";
                // ----------------------------------------
                ClearAllField();
                BindUserData();
            }
        }

        public void BindUserData()
        {
            List<Entity.Proof> lstEntity = new List<Entity.Proof>();
            int TotalCount = 0;
            lstEntity = BAL.ProofMgmt.GetProof(0, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
            rptUser.DataSource = lstEntity;
            rptUser.DataBind();
            pageGrid.BindPageing(TotalCount);
        }

        public void BindValidation()
        {
            //rfvFirstName.ErrorMessage = ErrorMessages.ShowRequiredFieldMsg("OrgDepartment Code");
            //regFirstName.ValidationExpression = RegularExpression.alphanumericexpresion();
            //regFirstName.ErrorMessage = ErrorMessages.ShowValidMsg("OrgDepartment Code");

            //rfvLastName.ErrorMessage = ErrorMessages.ShowRequiredFieldMsg("OrgDepartment Name");
            //regLastName.ValidationExpression = RegularExpression.alphanumericexpresion();
            //regLastName.ErrorMessage = ErrorMessages.ShowValidMsg("OrgDepartment Name");

        }


        public void ClearAllField()
        {
            Session["OldUserID"] = "";
            txtProofName.Text = "";
            chkActive.Checked = false;
            chkAddress.Checked = false;
            chkIdentity.Checked = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";


            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(txtProofName.Text))
            {

                _pageValid = false;

                if (String.IsNullOrEmpty(txtProofName.Text))
                    _pageErrMsg = "Designation Code is required.<br>";

                

                divErrorMessage.InnerHtml = _pageErrMsg;
            }
            // -----------------------------------------------------------------

            Entity.Proof objEntity = new Entity.Proof();

            if (_pageValid)
            {
                if (hdnproofid.Value != null && hdnproofid.Value == "0")
                {
                    objEntity.ProofID = Convert.ToInt64(hdnproofid.Value);
                }
                //else
                //{
                //    objEntity.ProofID = Convert.ToInt64(hdnproofid.Value);
                //}
                objEntity.ProofName = txtProofName.Text.Trim();
                objEntity.IsAddressProof = chkAddress.Checked;
                objEntity.IsIdentityProof = chkIdentity.Checked;
                objEntity.ActiveFlag = chkActive.Checked;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // ----------------------------------------------------------------------
                BAL.ProofMgmt.AddUpdateProof(objEntity, out ReturnCode, out ReturnMsg);
                // ----------------------------------------------------------------------
                divErrorMessage.InnerHtml = ReturnMsg;
                BindUserData();
                ClearAllField();
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void rptUser_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Edit")
            {
                int TotalCount = 0;
                List<Entity.Proof> lstEntity = new List<Entity.Proof>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.ProofMgmt.GetProof(Convert.ToInt64(e.CommandArgument.ToString()), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                hdnproofid.Value = lstEntity[0].ProofID.ToString();
                txtProofName.Text = lstEntity[0].ProofName;
                chkActive.Checked = lstEntity[0].ActiveFlag;
                chkAddress.Checked = lstEntity[0].IsAddressProof;
                chkIdentity.Checked = lstEntity[0].IsIdentityProof;
                // -------------------------------------------------------------------------

                BindUserData();
            }
            else if (e.CommandName.ToString() == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                BAL.ProofMgmt.DeleteProof(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
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