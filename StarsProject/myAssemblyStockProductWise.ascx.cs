using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myAssemblyStockProductWise : System.Web.UI.UserControl
    {
        int totrec;
        int ReturnCode = 0;
        string ReturnMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }

        public string pageProductID
        {
            get { return hdnProductID.Value; }
            set { hdnProductID.Value = value; }
        }
        public string pageQuantity
        {
            get { return hdnQuantity.Value; }
            set { hdnQuantity.Value = value; }
        }
        public string pageFinishProductID
        {
            get { return hdnFinishProductID.Value; }
            set { hdnFinishProductID.Value = value; }
        }

        public void BindAssemblyStock()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:loadAssemblyStockSummaryProductWise('" + pageView + "'," + pageProductID + "," + pageQuantity + ");", true);
        }
    }
}