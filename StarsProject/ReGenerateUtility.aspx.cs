using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace StarsProject
{
    public partial class ReGenerateUtility : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        // --------------------------------------------------------------------------
        // Section : Regenerate Trial Balance
        // --------------------------------------------------------------------------
        public void ReGenTrialBalance()
        {
            List<Entity.Customer> lstCustomer = new List<Entity.Customer>();
            lstCustomer = BAL.CommonMgmt.ReGenerateTrialBalance();
            spnTrialCount.InnerText = lstCustomer.Count.ToString();
            lblTrialOpening.Text = lstCustomer.Sum(it => it.OpeningAmount).ToString();
            lblTrialDebit.Text = lstCustomer.Sum(it => it.DebitAmount).ToString();
            lblTrialCredit.Text = lstCustomer.Sum(it => it.CreditAmount).ToString();
            lblTrialClosing.Text = lstCustomer.Sum(it => it.ClosingAmount).ToString();
            rptTrialBalance.DataSource = lstCustomer;
            rptTrialBalance.DataBind();
        }
        protected void btnSaveTrial_Click(object sender, EventArgs e)
        {
            ReGenTrialBalance();
        }

        // --------------------------------------------------------------------------
        // Section : Regenerate Stock
        // --------------------------------------------------------------------------
        public void ReGenStock()
        {
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.CommonMgmt.ReGenerateStock(true);
            spnStockCount.InnerText = lstProduct.Count.ToString();
            rptStock.DataSource = lstProduct;
            rptStock.DataBind();

        }
        protected void btnSaveStock_Click(object sender, EventArgs e)
        {
            ReGenStock();
        }
    }
}