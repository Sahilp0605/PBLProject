using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ManageExternalLeads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------                   
                BindClientDetailLedger();
            }
        }
        public void BindClientDetailLedger()
        {
            MyExternalLeads.BindExternalLeads();
        }
    }
}