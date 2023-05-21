using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ProductOpeningStock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];

            if (!IsPostBack)
            {
                BindDropDown();
                BindProductOpening();
            }
        }

        public void BindProductOpening()
        {
            int totrec;
            //rptOpeningStk.DataSource = BAL.ProductMgmt.GetProductOpeningStockList(0, Session["LoginUserID"].ToString(), "01/04/2020-31/03/2021", "", 1, 100, out totrec);
            //rptOpeningStk.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void imgBtnSave_Click(object sender, ImageClickEventArgs e)
        {
            bool _pageValid = true;
            divErrorMessage.InnerHtml = "";
            String strErr = "";
            //if (String.IsNullOrEmpty(txtContactPerson1.Text) || String.IsNullOrEmpty(txtContactNumber1.Text))
            //{
            //    _pageValid = false;

            //    divErrorMessage.Style.Remove("color");
            //    divErrorMessage.Style.Add("color", "red");

            //    if (String.IsNullOrEmpty(txtContactPerson1.Text))
            //        strErr += "<li>" + "Contact Person Name is required." + "</li>";

            //    if (String.IsNullOrEmpty(txtContactNumber1.Text))
            //        strErr += "<li>" + "Contact Number is required." + "</li>";
            //}
            // -------------------------------------------------------------------------
            if (_pageValid)
            {

                //DataTable dtCustomer = new DataTable();
                //dtCustomer = (DataTable)Session["dtCustomer"];

                //DataRow dr = dtCustomer.NewRow();

                //string cdesig = drpContactDesigCode1.SelectedValue;
                //string cname = txtContactPerson1.Text;
                //string cnumber = txtContactNumber1.Text;
                //string cemail = txtContactEmail1.Text;

                //Int64 cntRow = dtCustomer.Rows.Count + 1;
                //dr["pkID"] = cntRow;
                //dr["CustomerID"] = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                //dr["ContactDesigCode1"] = cdesig;
                //dr["ContactPerson1"] = cname;
                //dr["ContactNumber1"] = cnumber;
                //dr["ContactEmail1"] = cemail;

                //dtCustomer.Rows.Add(dr);

                //Session.Add("dtCustomer", dtCustomer);
                //// ---------------------------------------------------------------
                //rptContacts.DataSource = dtCustomer;
                //rptContacts.DataBind();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Contact Added .. But Dont Forget To SAVE Entry !');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
            }
        }

        public void BindDropDown()
        {

        }

        protected void rptOpeningStk_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptOpeningStk_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

    }
}