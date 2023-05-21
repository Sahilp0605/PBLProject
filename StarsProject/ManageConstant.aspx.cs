using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ManageConstant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDataGrid();
            }
            divErrorMessage.InnerHtml = "";
        }
        public void BindDataGrid()
        {
            int TotalRecord = 0;
            rptDailyActivity.DataSource = BAL.ConstantMgmt.GetConstantList();
            rptDailyActivity.DataBind();
        }
        
        protected void rptDailyActivity_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnConstantStyle = ((HiddenField)e.Item.FindControl("hdnConstantStyle"));
                HiddenField hdnConstantValue = ((HiddenField)e.Item.FindControl("hdnConstantValue"));
                HtmlGenericControl divSwitch = ((HtmlGenericControl)e.Item.FindControl("divSwitch"));
                HtmlGenericControl divText = ((HtmlGenericControl)e.Item.FindControl("divText"));
                HtmlGenericControl divDrop = ((HtmlGenericControl)e.Item.FindControl("divDrop"));
                // ------------------------------------------------------
                if (hdnConstantStyle.Value == "switch")
                {
                    divSwitch.Visible = true;
                    CheckBox ctrlSwitch = ((CheckBox)e.Item.FindControl("ctrlSwitch"));
                    ctrlSwitch.Checked = (hdnConstantValue.Value.ToLower() == "yes" || hdnConstantValue.Value.ToLower() == "y") ? true : false;
                }
                else
                {
                    divSwitch.Visible = false;
                }
                // ------------------------------------------------------
                if (hdnConstantStyle.Value == "text")
                {
                    divText.Visible = true;
                    TextBox ctrlText = ((TextBox)e.Item.FindControl("ctrlText"));
                    ctrlText.Text = hdnConstantValue.Value;
                }
                else
                {
                    divText.Visible = false;
                }
                // ------------------------------------------------------
                if (hdnConstantStyle.Value != "switch" && hdnConstantStyle.Value != "text")
                {
                    divDrop.Visible = true;
                    DropDownList ctrlDrop = ((DropDownList)e.Item.FindControl("ctrlDrop"));
                    // -------------------------------------------------
                    string[] split = hdnConstantStyle.Value.Split(',');
                    foreach (string item in split)
                    {
                        ctrlDrop.Items.Add(new ListItem(item, item));
                    }
                    // -------------------------------------------------
                    if (!String.IsNullOrEmpty(hdnConstantValue.Value))
                        ctrlDrop.SelectedValue = hdnConstantValue.Value;
                }
                else
                {
                    divDrop.Visible = false;
                }
            }
        }
        protected void rptDailyActivity_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
             int ReturnCode=0;
             string ReturnMsg="";

            if (e.CommandName.ToString() == "Delete")
            {
                //int ReturnCode = 0;
                //string ReturnMsg = "";
                // -------------------------------------------------------------- Delete Record
                BAL.ConstantMgmt.DeleteConstant(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                // -------------------------------------------------------------------------
                BindDataGrid();
            }
            else if (e.CommandName.ToString() == "Save")
            {
                HiddenField hdnConstantStyle = ((HiddenField)e.Item.FindControl("hdnConstantStyle"));
                HiddenField hdnConstantValue = ((HiddenField)e.Item.FindControl("hdnConstantValue"));
                // ------------------------------------------------------
                String hdnId = ((HiddenField)e.Item.FindControl("hdnpkID")).Value;
                String hdnCompId = ((HiddenField)e.Item.FindControl("hdnCompanyID")).Value;
                String tCategory = ((Label)e.Item.FindControl("txtCategory")).Text;
                String tConstantHead = ((Label)e.Item.FindControl("txtConstantHead")).Text;
                //String tConstantValue = ((TextBox)e.Item.FindControl("txtConstantValue")).Text;
                String tDispOrder = ((TextBox)e.Item.FindControl("txtDisplayOrder")).Text;

                //int ReturnCode;
                //string ReturnMsg;
                if (tConstantHead == "RILPrice")
                {
                    TextBox ctrlText = ((TextBox)e.Item.FindControl("ctrlText"));
                    Decimal rilprice = Convert.ToDecimal(ctrlText.Text);
                    BAL.CommonMgmt.UpdateRILPrice(rilprice);
                    BAL.CommonMgmt.AddRILPrice(Convert.ToDecimal(rilprice));
                }
                    
                Entity.Constant objEntity = new Entity.Constant();
    
                objEntity.pkID = Convert.ToInt64(hdnId);
                objEntity.CompanyID = Convert.ToInt64(hdnCompId);
                objEntity.Category = tCategory;
                objEntity.ConstantHead =tConstantHead;
                if (hdnConstantStyle.Value == "switch")
                {
                    CheckBox ctrlSwitch = ((CheckBox)e.Item.FindControl("ctrlSwitch"));
                    objEntity.ConstantValue = (ctrlSwitch.Checked == true) ? "Yes" : "No";
                }
                if (hdnConstantStyle.Value == "text")
                {
                    TextBox ctrlText = ((TextBox)e.Item.FindControl("ctrlText"));
                    objEntity.ConstantValue = ctrlText.Text;
                }
                if (hdnConstantStyle.Value != "switch" && hdnConstantStyle.Value != "text")
                {
                    DropDownList ctrlDrop = ((DropDownList)e.Item.FindControl("ctrlDrop"));
                    objEntity.ConstantValue = ctrlDrop.SelectedValue;
                }
                objEntity.DisplayOrder = Convert.ToInt64(tDispOrder);
                
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                BAL.ConstantMgmt.AddUpdateConstant(objEntity, out ReturnCode, out ReturnMsg);
                // -------------------------------------------------------------------------
                BindDataGrid();
            }
            else if (e.CommandName.ToString() == "Add")
            {

                String hdnCompId = Session["CompanyID"].ToString();
                String tConstantStyle = ((TextBox)e.Item.FindControl("txtConstantStyle1")).Text;
                String tCategory = ((TextBox)e.Item.FindControl("txtCategory1")).Text;
                String tConstantHead = ((TextBox)e.Item.FindControl("txtConstantHead1")).Text;
                String tConstantValue = ((TextBox)e.Item.FindControl("txtConstantValue1")).Text;
                String tDispOrder = ((TextBox)e.Item.FindControl("txtDisplayOrder1")).Text;

                Entity.Constant objEntity = new Entity.Constant();

                objEntity.pkID = 0;
                objEntity.CompanyID = Convert.ToInt64(hdnCompId);
                objEntity.ConstantStyle = tConstantStyle;
                objEntity.Category = tCategory;
                objEntity.ConstantHead = tConstantHead;
                objEntity.ConstantValue = tConstantValue;
                objEntity.DisplayOrder = (!String.IsNullOrEmpty(tDispOrder)) ? Convert.ToInt64(tDispOrder) : 0;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                BAL.ConstantMgmt.AddUpdateConstant(objEntity, out ReturnCode, out ReturnMsg);
                // -------------------------------------------------------------------------
                BindDataGrid();
            }
            if(ReturnCode<=0)
                divErrorMessage.InnerHtml = ReturnMsg;

        }
    }
}