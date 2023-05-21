using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
     public partial class Product_Specification : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindDropDown();

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnProductSpecificationID.Value = Request.QueryString["id"].ToString();

                    if (hdnProductSpecificationID.Value == "0" || hdnProductSpecificationID.Value == "")
                    {
                        ClearAllField();
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
        public void ClearAllField()
        {
            //drpGroupHead.Text = "";
            txtGroupHead.Text = ""; 
            txtMaterialHead.Text = "";
            txtMaterialSpec.Text = "";
            txtMaterialRemarks.Text = "";
            txtItemOrder.Text = "";
        }
        public void OnlyViewControls()
        {
            txtGroupHead.ReadOnly = true;
            txtMaterialHead.ReadOnly = true;
            txtMaterialSpec.ReadOnly = true;
            txtMaterialRemarks.ReadOnly = true;
            txtItemOrder.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
            //drpState.Attributes.Add("disabled", "disabled");
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {              
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.ProductSpecification> lstEntity = new List<Entity.ProductSpecification>();

                lstEntity = BAL.ProductSpecificationMgmt.GetProductSpecificationList(Convert.ToInt64(hdnProductSpecificationID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnProductSpecificationID.Value = lstEntity[0].pkID.ToString();
                txtGroupHead.Text = lstEntity[0].GroupHead;
                txtMaterialHead.Text = lstEntity[0].MaterialHead;
                txtMaterialSpec.Text = lstEntity[0].MaterialSpec;
                txtItemOrder.Text = lstEntity[0].ItemOrder;
                txtMaterialRemarks.Text = lstEntity[0].MaterialRemarks;
                txtGroupHead.Focus();
            }
        }
        //public void BindDropDown()
        //{
        //    List<Entity.ProductSpecification> lstEvents = new List<Entity.ProductSpecification>();
        //    lstEvents = BAL.ProductSpecificationMgmt.GetProductSpecGroupList();
        //    drpGroupHead.DataSource = lstEvents;
        //    drpGroupHead.DataValueField = "GroupHead";
        //    drpGroupHead.DataTextField = "GroupHead";
        //    drpGroupHead.DataBind();
        //    drpGroupHead.Items.Insert(0, new ListItem("-- All Group Head --", ""));
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _pageValid = true;
            divErrorMessage.InnerHtml = "";
            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.ProductSpecification objEntity = new Entity.ProductSpecification();

                if (!String.IsNullOrEmpty(hdnProductSpecificationID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnProductSpecificationID.Value);

                objEntity.GroupHead = txtGroupHead.Text;
                objEntity.MaterialHead = txtMaterialHead.Text;
                objEntity.MaterialSpec = txtMaterialSpec.Text;
                objEntity.MaterialRemarks = txtMaterialRemarks.Text;
                objEntity.ItemOrder = txtItemOrder.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ProductSpecificationMgmt.AddUpdateProductSpecification(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------

                divErrorMessage.InnerHtml = ReturnMsg;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteProductSpecification(string pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ProductSpecificationMgmt.DeleteProductSpecification(Convert.ToInt64(pkID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterGroup(string pGroupName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.ProductSpecificationMgmt.GetProductSpecGroupList(pGroupName).Select(sel => new { sel.GroupHead });
            return serializer.Serialize(rows);
        }

        //protected void txtgroupHead_TextChanged(object sender, EventArgs e)
        //{
        //    int totalrecord;
        //    List<Entity.ProductSpecification> lstEntity = new List<Entity.ProductSpecification>();

        //    if (!String.IsNullOrEmpty(hdnCustomerID.Value))
        //        lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

        //    hdnCustStateID.Value = (lstEntity.Count > 0) ? lstEntity[0].StateCode : "0";
        //    if (Convert.ToInt64(hdnCustStateID.Value) > 0)
        //    {
        //        drpTerminationOfDelivery.SelectedValue = hdnCustStateID.Value;
        //    }

        //}

    }
}