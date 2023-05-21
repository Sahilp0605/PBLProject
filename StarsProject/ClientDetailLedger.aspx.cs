using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ClientDetailLedger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    int totrec;
                    hdnCustomerID.Value = Request.QueryString["id"].ToString();
                    lblCustomerName.InnerText = Request.QueryString["custname"].ToString();
                    List<Entity.Customer> lstCustomer = new List<Entity.Customer>();
                    lstCustomer = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 1000, out totrec);
                    lblGSTNO.InnerText = (lstCustomer.Count > 0) ? lstCustomer[0].GSTNo : "Not Applied";
                    BindClientDetailLedger();
                }
            }

        }
        public void BindClientDetailLedger()
        {
            myClientDetailLedger.BindClientLedger(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString());
        }
    }
}