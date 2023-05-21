using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SalesTarget : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;

                DateTime dt = DateTime.Now; //Your Date

                DateTime start = new DateTime(dt.Year, dt.Month, 1); //First Date of the month
                DateTime end = start.AddMonths(1).AddDays(-1); //Last Date of the month

                txtFromDate.Text = start.ToString("dd-MM-yyyy");
                txtToDate.Text = end.ToString("dd-MM-yyyy");

                BindDropDown();
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
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
            }
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.SalesTarget> lstEntity = new List<Entity.SalesTarget>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();

                
                lstEntity = BAL.SalesTargetMgmt.GetSalesTarget(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();

                if (lstEntity[0].TargetType.ToString().Trim() == "A")
                    rdblSalesTargetType.SelectedIndex = 0;
                else if (lstEntity[0].TargetType.ToString().Trim() == "Q")
                    rdblSalesTargetType.SelectedIndex = 1;

                if (!String.IsNullOrEmpty(lstEntity[0].ProductGroupID.ToString()) || lstEntity[0].ProductGroupID.ToString() != "0")
                    drpProductGroup.SelectedValue = lstEntity[0].ProductGroupID.ToString();

                if (!String.IsNullOrEmpty(lstEntity[0].BrandID.ToString()) || lstEntity[0].BrandID.ToString() != "0")
                    drpBrand.SelectedValue = lstEntity[0].BrandID.ToString();

                hdnProductID.Value = lstEntity[0].ProductID.ToString();
                txtProductName.Text = lstEntity[0].ProductName.ToString();
                //if (!String.IsNullOrEmpty(lstEntity[0].ProductID.ToString()) || lstEntity[0].ProductID.ToString() != "0")
                //    drpProduct.SelectedValue = lstEntity[0].ProductID.ToString();

                //txtFromDate.Text = lstEntity[0].FromDate.ToString("dd-MM-yyyy");
                txtFromDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].FromDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
               
                //txtToDate.Text = lstEntity[0].ToDate.ToString("dd-MM-yyyy");
                txtToDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].ToDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                txtTargetAmount.Text = lstEntity[0].TargetAmount.ToString();
                drpEmployee.Focus();
            }
        }
        public void BindDropDown()
        {
            // ---------------- Assign Employee ------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));

            // ---------------- Product Group List -------------------------------------
            List<Entity.ProductGroup> lstEvents = new List<Entity.ProductGroup>();
            lstEvents = BAL.ProductGroupMgmt.GetProductGroupList();
            drpProductGroup.DataSource = lstEvents;
            drpProductGroup.DataValueField = "pkID";
            drpProductGroup.DataTextField = "ProductGroupName";
            drpProductGroup.DataBind();
            drpProductGroup.Items.Insert(0, new ListItem("-- Select --", "0"));

            // ---------------- Brand List -------------------------------------
            List<Entity.Brand> lstEvents1 = new List<Entity.Brand>();
            lstEvents1 = BAL.BrandMgmt.GetBrandList();
            drpBrand.DataSource = lstEvents1;
            drpBrand.DataValueField = "pkID";
            drpBrand.DataTextField = "BrandName";
            drpBrand.DataBind();
            drpBrand.Items.Insert(0, new ListItem("-- Select --", "0"));

            // ---------------- Product List -------------------------------------
            //List<Entity.Products> lstProduct = new List<Entity.Products>();
            //lstProduct = BAL.ProductMgmt.GetProductList();
            //drpProduct.DataSource = lstProduct;
            //drpProduct.DataValueField = "pkID";
            //drpProduct.DataTextField = "ProductNameLong";
            //drpProduct.DataBind();
            //drpProduct.Items.Insert(0, new ListItem("-- Select --", ""));
        }
        
        public void OnlyViewControls()
        {
            txtTargetAmount.ReadOnly = true;
            drpEmployee.Attributes.Add("disabled", "disabled");
            drpProductGroup.Attributes.Add("disabled", "disabled");
            drpBrand.Attributes.Add("disabled", "disabled");
            //drpProduct.Attributes.Add("disabled", "disabled");
            txtProductName.ReadOnly = true; 
            txtFromDate.ReadOnly = true;
            txtToDate.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void ClearAllField()
        {
            txtTargetAmount.Text = "";
            hdnpkID.Value = String.Empty;
            txtFromDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            drpEmployee.SelectedValue = "0";
            drpProductGroup.SelectedValue = "0";
            drpBrand.SelectedValue = "0";
            //drpProduct.SelectedValue = "0";
            txtProductName.Text = ""; 
            drpEmployee.Focus();
            btnSave.Disabled = false;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            string strErr = "";

            if ((!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtToDate.Text)))
            {
                if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "From Date is Always Less then To Date." + "</li>";
                }
            }

            if (String.IsNullOrEmpty(drpEmployee.SelectedValue) || String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtToDate.Text) || (String.IsNullOrEmpty(txtTargetAmount.Text) ? 0 : Convert.ToDecimal(txtTargetAmount.Text)) == 0 || String.IsNullOrEmpty(drpProductGroup.SelectedValue) && String.IsNullOrEmpty(drpBrand.SelectedValue) && String.IsNullOrEmpty(hdnProductID.Value))
            {
                if (String.IsNullOrEmpty(txtFromDate.Text))
                {
                    strErr += "<li>" + "From Date is required." + "</li>";
                }

                if (String.IsNullOrEmpty(txtToDate.Text))
                {
                    strErr += "<li>" + "To Date is required." + "</li>";
                }

                if (drpEmployee.SelectedValue == "0")
                    strErr += "<li>" + "Employee Selection is required." + "</li>";

                if ((String.IsNullOrEmpty(txtTargetAmount.Text) ? 0 : Convert.ToDecimal(txtTargetAmount.Text)) == 0)
                    strErr += "<li>" + "Target Amount/Quantity must be greater than zero" + "</li>";

                if ((String.IsNullOrEmpty(drpProductGroup.SelectedValue) || String.IsNullOrEmpty(drpBrand.SelectedValue)))
                    strErr += "<li>" + "Please Select ProductBrand/ProductGroup/ProductName" + "</li>";

                if ( Convert.ToInt64(hdnProductID.Value) == 0)
                    strErr += "<li>" + "Select Proper Product From List." + "</li>";

                _pageValid = false;
            }


            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.SalesTarget objEntity = new Entity.SalesTarget();

                objEntity.TargetType = "A";

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                
                //if (rdblSalesTargetType.SelectedIndex == 0)
                //    objEntity.TargetType = "A";
                //else if (rdblSalesTargetType.SelectedIndex == 1)
                //    objEntity.TargetType = "Q";

                objEntity.TargetAmount = String.IsNullOrEmpty(txtTargetAmount.Text) ? 0 : Convert.ToDecimal(txtTargetAmount.Text); 
                objEntity.FromDate = Convert.ToDateTime(txtFromDate.Text);
                objEntity.ToDate = Convert.ToDateTime(txtToDate.Text);
                objEntity.EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);
                objEntity.ProductGroupID  = Convert.ToInt64(drpProductGroup.SelectedValue);
                objEntity.BrandID = Convert.ToInt64(drpBrand.SelectedValue);
                objEntity.ProductID = String.IsNullOrEmpty(hdnProductID.Value) ? 0 : Convert.ToInt64(hdnProductID.Value);
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.SalesTargetMgmt.AddUpdateSalesTarget(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
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
        }

        protected void rdblSalesTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblSalesTarget.Text = (rdblSalesTargetType.SelectedIndex ==0) ? "Enter Target Amount" : "Enter Target Quantity";;
        }
        
        [System.Web.Services.WebMethod]
        public static string DeleteSalesTarget(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.SalesTargetMgmt.DeleteSalesTarget(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
      
    }
}