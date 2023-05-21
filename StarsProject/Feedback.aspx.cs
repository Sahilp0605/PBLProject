using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Feedback : System.Web.UI.Page
    {
        bool _pageValid = true;
        PageBase pb = new PageBase();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["CustomerId"]))
                {
                    //string test=pb.Encrypt("20017", "bob1nroy");
                    hdnCustomerID.Value =  pb.Decrypt(Request.QueryString["CustomerId"].ToString(),"bob1nroy");
                    BindCustomerDetail();
                 
                    txtCustomerName.CssClass = "form-control";
                    txtAddress.CssClass = "form-control";
                }
                //BindFeedBackList();
            }
        }
        public void BindCustomerDetail()
        {
            int TotalCount = 0;
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), "",1, 10, out TotalCount);
            txtCustomerName.Text = lstEntity[0].CustomerName;
            txtAddress.Text = lstEntity[0].Address;
        }
        //public void BindFeedBackList()
        //{
        //    List<Entity.Feedback> lstEntity = new List<Entity.Feedback>();
        //    lstEntity = BAL.FeedbackMgmt.GetFeedbackQuestions();
        //    rptFeedback.DataSource = lstEntity;
        //    rptFeedback.DataBind();
        //}
        protected void btnSave_Click(object sender, EventArgs e)
        {

        //    _pageValid = true;
        //    divErrorMessage.InnerHtml = "";

        //    if (String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtToDate.Text))
        //    {
        //        _pageValid = false;

        //        divErrorMessage.Style.Remove("color");
        //        divErrorMessage.Style.Add("color", "red");
        //        if (String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtToDate.Text))
        //            divErrorMessage.Controls.Add(new LiteralControl("<li>" + "From and To Date is mandatory !" + "</li>"));

        //        if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
        //            divErrorMessage.Controls.Add(new LiteralControl("<li>" + "From Date is Always Less then To Date." + "</li>"));
        //    }
        //    if (!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtToDate.Text))
        //    {

        //        if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
        //        {
        //            _pageValid = false;

        //            divErrorMessage.Style.Remove("color");
        //            divErrorMessage.Style.Add("color", "red");

        //            divErrorMessage.Controls.Add(new LiteralControl("<li>" + "FromDate & ToDate range selection is wrong." + "</li>"));
        //        }
        //    }
        //    // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.Feedback objEntity = new Entity.Feedback();

                //if (!String.IsNullOrEmpty(hdnpkID.Value))
                //    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

               
                for (int i = 0;i<tblFeedback.Rows.Count ;i++)
                {
                    //1st Question
                    if (tblFeedback.Rows[i].Cells[0].InnerHtml == "Product Satisfaction")
                    {
                            if (rdbGrade1.Checked)
                                objEntity.Product_Satisfaction = 4;                         
                            else if (rdbGrade2.Checked)                           
                                objEntity.Product_Satisfaction = 3;
                            else if (rdbGrade3.Checked)
                                objEntity.Product_Satisfaction = 2;
                            else if (rdbGrade4.Checked)
                                objEntity.Product_Satisfaction = 1;                                  
                     }
                    //2nd Question
                    if (tblFeedback.Rows[i].Cells[0].InnerHtml == "Sales Executive Presentation")
                    {
                        if (rdbGrade11.Checked)
                            objEntity.SalesExecutive_Presentation = 4;
                        else if (rdbGrade22.Checked)
                            objEntity.SalesExecutive_Presentation = 3;
                        else if (rdbGrade33.Checked)
                            objEntity.SalesExecutive_Presentation = 2;
                        else if (rdbGrade44.Checked)
                            objEntity.SalesExecutive_Presentation = 1;
                    }
                    //3rd Question
                    if (tblFeedback.Rows[i].Cells[0].InnerHtml == "Product Features")
                    {
                        if (rdbGrade111.Checked)
                            objEntity.Product_Features = 4;
                        else if (rdbGrade222.Checked)
                            objEntity.Product_Features = 3;
                        else if (rdbGrade333.Checked)
                            objEntity.Product_Features = 2;
                        else if (rdbGrade444.Checked)
                            objEntity.Product_Features = 1;
                    }
                    //4th Question
                    if (tblFeedback.Rows[i].Cells[0].InnerHtml == "Product Presentation")
                    {
                        if (rdbGrade1111.Checked)
                            objEntity.Product_Presentation = 4;
                        else if (rdbGrade2222.Checked)
                            objEntity.Product_Presentation = 3;
                        else if (rdbGrade3333.Checked)
                            objEntity.Product_Presentation = 2;
                        else if (rdbGrade4444.Checked)
                            objEntity.Product_Presentation = 1;
                    }
                }
                objEntity.pkID = 0;
                objEntity.CustomerId = hdnCustomerID.Value;
                objEntity.Comment=txtComment.Text;

                if (rdblRecommend.SelectedIndex == 0)
                    objEntity.Recommendyn = 1;
                else if (rdblRecommend.SelectedIndex == 1)
                    objEntity.Recommendyn = 0;
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.FeedbackMgmt.AddUpdateFeedback(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------

                divErrorMessage.InnerHtml = ReturnMsg;

            }

        }
       
    }
}