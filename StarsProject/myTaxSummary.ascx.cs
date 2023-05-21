using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myTaxSummary : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public string Module
        {
            get { return hdnModule.Value; }
            set { hdnModule.Value = value; }
        }
        public string KeyID
        {
            get { return hdnKeyID.Value; }
            set { hdnKeyID.Value = value; }
        }
        public string HSNFlag
        {
            get { return hdnHSNFlag.Value; }
            set { hdnHSNFlag.Value = value; }
        }
        public void BindTaxSummaryWidget(string FromDate, string ToDate)
        {
            Boolean tmpHSNFlag = false ;
            tmpHSNFlag = (HSNFlag == "0" || HSNFlag == "") ? false : true;
            List<Entity.SalesTaxDetail> lstTax = new List<Entity.SalesTaxDetail>();
            lstTax = BAL.CommonMgmt.GetTaxSummaryWidget(Module, KeyID, tmpHSNFlag, "", "");
            rptBalances.DataSource = lstTax;
            rptBalances.DataBind();
        }


    }
}