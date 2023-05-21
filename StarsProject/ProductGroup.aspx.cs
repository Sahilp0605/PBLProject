using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ProductGroup : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
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

        public void OnlyViewControls()
        {
            txtProductGroupName.ReadOnly = true;
            chkActive.Enabled = false;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.ProductGroup> lstEntity = new List<Entity.ProductGroup>();
                // ----------------------------------------------------
                lstEntity = BAL.ProductGroupMgmt.GetProductGroupList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtProductGroupName.Text = lstEntity[0].ProductGroupName;
                chkActive.Checked = lstEntity[0].ActiveFlag;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _pageValid = true;
            divErrorMessage.InnerHtml = "";

            if (String.IsNullOrEmpty(txtProductGroupName.Text)) 
            {
                _pageValid = false;

                divErrorMessage.Style.Remove("color");
                divErrorMessage.Style.Add("color", "red");

                if (String.IsNullOrEmpty(txtProductGroupName.Text))
                    divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Product Group Name is required." + "</li>"));

            }
            // -------------------------------------------------------------
            if (_pageValid)
            {

                int ReturnCode = 0;
                string ReturnMsg = "";

                Entity.ProductGroup objEntity = new Entity.ProductGroup();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.ProductGroupName = txtProductGroupName.Text;
                objEntity.ActiveFlag = chkActive.Checked;


                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                // -------------------------------------------------------------- Insert/Update Record
                BAL.ProductGroupMgmt.AddUpdateProductGroup(objEntity, out ReturnCode, out ReturnMsg);
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
            txtProductGroupName.Text = "";
            chkActive.Checked = false;
            txtProductGroupName.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteProductGroup(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ProductGroupMgmt.DeleteProductGroup(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}