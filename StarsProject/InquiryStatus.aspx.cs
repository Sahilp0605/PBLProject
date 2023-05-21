using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class InquiryStatus : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            // ----------------------------------------------
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["OldUserID"] = "";

                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkid.Value = Request.QueryString["id"].ToString();

                    if (hdnpkid.Value == "0" || hdnpkid.Value == "")
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
            
            txtInquiryStatus.ReadOnly = true;
            drpCategory.Attributes.Add("disabled", "disabled");
            
            btnSave.Visible = false;
            btnReset.Visible = false;

        }

        public void ClearAllField()
        {
            hdnpkid.Value = "";
            txtInquiryStatus.Text = "";
            drpCategory.SelectedValue = "";
            txtInquiryStatus.Focus();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                List<Entity.InquiryStatus> lstEntity = new List<Entity.InquiryStatus>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.InquiryStatusMgmt.GetInquiryStatusList(Convert.ToInt64(hdnpkid.Value), "", Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                hdnpkid.Value = lstEntity[0].pkID.ToString();
                txtInquiryStatus.Text = lstEntity[0].InquiryStatusName;
                drpCategory.SelectedValue = lstEntity[0].StatusCategory;
                txtInquiryStatus.Focus();
                // -------------------------------------------------------------------------
                categoryStatusCaption();

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";

            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(txtInquiryStatus.Text) || String.IsNullOrEmpty(drpCategory.SelectedValue))
            {

                _pageValid = false;

                if (String.IsNullOrEmpty(txtInquiryStatus.Text))
                    _pageErrMsg = "<li>Inquiry Status is required.</li>";

            }
            // -----------------------------------------------------------------
            Entity.InquiryStatus objEntity = new Entity.InquiryStatus();
            if (_pageValid)
            {
                if (!String.IsNullOrEmpty(hdnpkid.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkid.Value);
                objEntity.InquiryStatusName = txtInquiryStatus.Text;
                objEntity.StatusCategory = drpCategory.SelectedValue;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                // -------------------------------------------------------------- Insert/Update Record
                BAL.InquiryStatusMgmt.AddUpdateInquiryStatus(objEntity, out ReturnCode, out ReturnMsg);
                _pageErrMsg += "<li>" + ReturnMsg + "</li>";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);
        }

        [System.Web.Services.WebMethod]
        public static string DeleteInquiryStatus(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.InquiryStatusMgmt.DeleteInquiryStatus(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoryStatusCaption();
        }

        public void categoryStatusCaption()
        {
            if (drpCategory.SelectedValue == "InquiryStatus")
                lblInquiryStatus.InnerText = "Lead Status";
            else if (drpCategory.SelectedValue == "FollowupType")
                lblInquiryStatus.InnerText = "Followup Type";
            else if (drpCategory.SelectedValue == "InquirySource")
                lblInquiryStatus.InnerText = "Lead Source";
            else if (drpCategory.SelectedValue == "DisQualifiedReason")
                lblInquiryStatus.InnerText = "DisQuali. Reason";

            txtInquiryStatus.Focus();
        }
    }
}