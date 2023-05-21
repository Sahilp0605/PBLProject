using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class PaggingUserControl : System.Web.UI.UserControl
    {
        public event EventHandler<PagerEventArgs> PagerChanged;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // BindCompanyPageing(Convert.ToInt64(Session["TotalRecords"]));
            }
            
        }
        
        #region Company Pagging
        protected void rptPaging_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var args = new PagerEventArgs { };
            if (e.CommandName == "ChangePage")
            {
                Session["PageNo"] = e.CommandArgument;
                //BindCompanyList();
                //SetHideShowColumn();
            }
            this.PagerChanged(this,args);
        }
        protected void rptPaging_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
           // var args = new PagerEventArgs { };
            LinkButton lnkPageNo = (LinkButton)e.Item.FindControl("lnkPageNo");
            if (Convert.ToInt32(Session["PageNo"]) == Convert.ToInt32(lnkPageNo.Text))
            {
                lnkPageNo.Attributes.Add("class", "currpage");
            }
          //  this.PagerChanged(this, args);
        }
        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
          var args = new PagerEventArgs { };
            Session["PageNo"] = Convert.ToInt32(Session["PageNo"]) - 1;
            //BindCompanyList();
            //SetHideShowColumn();
           this.PagerChanged(this, args);
        }
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            var args = new PagerEventArgs { };
            Session["PageNo"] = Convert.ToInt32(Session["PageNo"]) + 1;
            //BindCompanyList();
            //SetHideShowColumn();
            this.PagerChanged(this, args);
        }
        public void BindPageing(long RowCount)
        {
            int PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(RowCount) / Convert.ToInt32(Session["PageSize"])));
            if (PageCount > 1)
            {
                if (PageCount < Convert.ToInt32(Session["PageNo"]))
                {
                    Session["PageNo"] = 1;
                }
                List<int> lstPage = Pagging.GetPaggingdata(RowCount, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]));
                rptPaging.Visible = true;
                lblcount.Text = RowCount.ToString();
                lblend.Text = (Convert.ToInt32(Session["PageNo"]) * Convert.ToInt32(Session["PageSize"])) > RowCount ? RowCount.ToString() : (Convert.ToInt32(Session["PageNo"]) * Convert.ToInt32(Session["PageSize"])).ToString();
                lblstrat.Text = (Convert.ToInt32(Session["PageNo"]) * Convert.ToInt32(Session["PageSize"]) - (Convert.ToInt32(Session["PageSize"]) - 1)).ToString();
                
                rptPaging.DataSource = lstPage;
                rptPaging.DataBind();
                if (Convert.ToInt32(Session["PageNo"]) == 1)
                    lnkPrevious.Visible = false;
                else
                    lnkPrevious.Visible = true;

                if (Convert.ToInt32(Session["PageNo"]) == PageCount)
                    lnkNext.Visible = false;
                else
                    lnkNext.Visible = true;
            }
            else
            {
                rptPaging.Visible = false;
                lnkNext.Visible = false;
                lnkPrevious.Visible = false;

                lblcount.Text = RowCount.ToString();
                lblend.Text = (Convert.ToInt32(Session["PageNo"]) * Convert.ToInt32(Session["PageSize"])) > RowCount ? RowCount.ToString() : (Convert.ToInt32(Session["PageNo"]) * Convert.ToInt32(Session["PageSize"])).ToString();
                lblstrat.Text = (Convert.ToInt32(Session["PageNo"]) * Convert.ToInt32(Session["PageSize"]) - (Convert.ToInt32(Session["PageSize"]) - 1)).ToString();
                
            }
        }
        #endregion
    }

    public class PagerEventArgs : EventArgs
    {

    }
}