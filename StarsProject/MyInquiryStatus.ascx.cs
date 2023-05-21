using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class MyInquiryStatus : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        protected string strInquiryStatus = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            hdnRole.Value = objAuth.RoleCode.ToLower();
            hdnLoginUserID.Value = objAuth.LoginUserID;
            strInquiryStatus = hdnInquiryStatus.Value.ToLower();
            // -----------------------------------------------------------------------
            if (!IsPostBack)
            {
                
                //btnApproveReject.Visible = (hdnView.Value == "dashboard") ? false : true;
            }             
        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }

        public string pageMonth
        {
            get { return hdnMonth.Value; }
            set { hdnMonth.Value = value; }
        }

        public string pageYear
        {
            get { return hdnYear.Value; }
            set { hdnYear.Value = value; }
        }

        public string InquiryStatus
        {
            get { return hdnInquiryStatus.Value; }
            set { hdnInquiryStatus.Value = value; }
        }

        public void BindInquiryStatus(String pStatus)
        {
            int totrec;
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;

            rptInquiry.DataSource = BAL.InquiryInfoMgmt.GetInquiryListByStatus(pStatus, Session["LoginUserID"].ToString(), pMon, pYear);
            rptInquiry.DataBind();
        }

        //protected void btnApproveReject_Click(object sender, EventArgs e)
        //{
        //    SendApprovalStatus();
        //}

        //public void SendApprovalStatus()
        //{
        //    foreach (RepeaterItem i in rptApproval.Items)
        //    {
        //        Entity.SalesOrder objEntity = new Entity.SalesOrder();

        //        HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
        //        DropDownList ddl = ((DropDownList)i.FindControl("drpApprovalStatus"));
        //        // --------------------------------------------------------
        //        if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
        //        {
        //            objEntity.pkID = Convert.ToInt64(hdnID.Value);
        //            objEntity.ApprovalStatus = ddl.SelectedValue;
        //            objEntity.LoginUserID = Session["LoginUserID"].ToString();
        //            // -------------------------------------------------------------- Insert/Update Record
        //            BAL.SalesOrderMgmt.UpdateSalesOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
        //        }
        //    }
        //}

        protected void rptInquiry_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                HiddenField hdnCreatedBy = ((HiddenField)e.Item.FindControl("hdnCreatedBy"));
                //String QuotationNo = ((Label)e.Item.FindControl("QuotationNo")).Text;
                //String OrderNo = ((Label)e.Item.FindControl("OrderNo")).Text;
                //String BillNo = ((Label)e.Item.FindControl("BillNo")).Text;
                //if (QuotationNo != null || OrderNo != null || BillNo != null)
                //{
                //    //((HyperLink)e.Item.FindControl("lnkQuotation")).Visible = false;
                //    //((HyperLink)e.Item.FindControl("lnkQuotation")).Visible = false;
                //    //((HyperLink)e.Item.FindControl("lnkQuotation")).Visible = false;
                //}

                //((Label)e.Item.FindControl("RatingLabel")).Visible = true;

                // -----------------------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
            }
        }
    }
}