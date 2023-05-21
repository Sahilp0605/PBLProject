using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class InternalWorkOrderSpecification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnSpecFormat.Value = BAL.CommonMgmt.GetConstant("QuotationSpecificationFormat", 0, 1);
                if (hdnSpecFormat.Value == "old")
                {
                    txtScope.Attributes.Remove("content");
                    txtScope.Attributes.Add("class", "form-control");
                    txtScopeClient.Attributes.Remove("content");
                    txtScopeClient.Attributes.Add("class", "form-control");
                    txtRemarks.Attributes.Remove("content");
                    txtRemarks.Attributes.Add("class", "form-control");
                }
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["DocNo"]))
                    hdnDocNo.Value = Request.QueryString["DocNo"].ToString();

                //if (!String.IsNullOrEmpty(Request.QueryString["Module"]))
                //    hdnModule.Value = Request.QueryString["Module"].ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["FinishProductID"]))
                {
                    hdnFinishProductID.Value = Request.QueryString["FinishProductID"].ToString();
                    if (String.IsNullOrEmpty(hdnFinishProductID.Value) || hdnFinishProductID.Value == "")
                        hdnFinishProductID.Value = "0";
                }
                // --------------------------------------------------------

                //hdnSpecRemark.Value = BAL.CommonMgmt.GetConstant("QuatSpecRemark", 0, 1);


                BindProductSpecs();
            }
        }

        private void BindProductSpecs()
        {
            List<Entity.InternalWorkOrderDetail> ProdSpec = new List<Entity.InternalWorkOrderDetail>();
            if (!String.IsNullOrEmpty(hdnDocNo.Value) || hdnDocNo.Value != "")
            {
                ProdSpec = BAL.InternalWorkOrderMgmt.GetInternalWorkOrderProdSpecList(hdnDocNo.Value, Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                if (ProdSpec.Count == 0)
                {
                    ProdSpec = BAL.InternalWorkOrderMgmt.GetInternalWorkOrderProdSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                }
            }
            else
                ProdSpec = BAL.InternalWorkOrderMgmt.GetInternalWorkOrderProdSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());

            if (ProdSpec.Count > 0)
            {
                txtScope.Text = ProdSpec[0].ScopeOfWork.ToString();
                txtScopeClient.Text = ProdSpec[0].ScopeOfWork_Client.ToString();
                txtRemarks.Text = ProdSpec[0].Remarks.ToString();
            }
            // --------------------------------------------------------------

            if (Session["mySpecs"] == null)
            {
                Session["mySpecs"] = PageBase.ConvertListToDataTable(ProdSpec);
            }
        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dtSpecs = new DataTable();
            if (Session["mySpecs"] != null)
            {
                Boolean itemAdded = false;
                DataTable mySpecs = new DataTable();
                mySpecs = (DataTable)Session["mySpecs"];
                foreach (DataRow row in mySpecs.Rows)
                {
                    if (row["pkID"].ToString() == hdnFinishProductID.Value)
                    {
                        row["ScopeOfWork"] = txtScope.Text;
                        row["ScopeOfWork_Client"] = txtScopeClient.Text;
                        row["Remarks"] = txtRemarks.Text;
                        itemAdded = true;
                    }
                }
                if (!itemAdded)
                {
                    DataRow dr = mySpecs.NewRow();
                    dr["pkID"] = Convert.ToInt64(hdnFinishProductID.Value);
                    dr["ScopeOfWork"] = txtScope.Text;
                    dr["ScopeOfWork_Client"] = txtScopeClient.Text;
                    dr["Remarks"] = txtRemarks.Text;
                    mySpecs.Rows.Add(dr);
                }
                mySpecs.AcceptChanges();
                Session.Add("mySpecs", mySpecs);
                divErrorMessage.InnerHtml = " <center>  Specification Added Successfuly </br> <b> Note : Don't forget to 'Save'  From Main Screen.</b> </center>";
            }
        }
        }
}