using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace StarsProject
{
    public partial class ConversationChatBox : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                hdnLoginUserID.Value = objAuth.UserID;
                // -----------------------------------
                hdnModuleName.Value = (!String.IsNullOrEmpty(Request.QueryString["modulename"])) ? Request.QueryString["modulename"].ToString() : "";
                // -----------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["keyvalue"]))
                {
                    hdnKeyValue.Value = Request.QueryString["keyvalue"].ToString();
                    spnProjectNo.InnerText = hdnKeyValue.Value;
                }
                // -----------------------------------
                if (!String.IsNullOrEmpty(hdnKeyValue.Value))
                {
                    LoadConversationlog(hdnModuleName.Value, hdnKeyValue.Value);
                }
            }
        }

        public void LoadConversationlog(string ModuleName, string KeyValue)
        {
            List<Entity.ConversationChatBox> lstChat = new List<Entity.ConversationChatBox>();
            lstChat = BAL.CommonMgmt.GetConversationChatBoxList(ModuleName, KeyValue, Session["LoginUserID"].ToString());
            if (lstChat.Count > 0)
            {
                rptChatBoxList.DataSource = lstChat;
                rptChatBoxList.DataBind();
            }
        }
        protected void rptChatBoxList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                HtmlGenericControl spnProjectNo = ((HtmlGenericControl)e.Item.FindControl("spnProjectNo"));
                spnProjectNo.InnerText = hdnKeyValue.Value;
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnFromEmployee = ((HiddenField)e.Item.FindControl("hdnFromEmployee"));
                HiddenField hdnToEmployee = ((HiddenField)e.Item.FindControl("hdnToEmployee"));

                HiddenField hdnFromUser = ((HiddenField)e.Item.FindControl("hdnFromUser"));
                HtmlGenericControl dv = ((HtmlGenericControl)e.Item.FindControl("dvItem"));
                HtmlImage img = ((HtmlImage)e.Item.FindControl("imgChatUser"));

                HtmlGenericControl spnEmployee = ((HtmlGenericControl)e.Item.FindControl("spnEmployee"));

                if (hdnFromUser.Value.ToLower() == hdnLoginUserID.Value.ToLower())
                {
                    dv.Attributes.Add("class", "chat chat-right");
                    img.Src = "images/customer.png";
                    spnEmployee.InnerText = hdnFromEmployee.Value;
                }
                else
                {
                    dv.Attributes.Add("class", "chat");
                    img.Src = "images/courier.jpg";
                    spnEmployee.InnerText = hdnToEmployee.Value;
                }

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            String strErr = "";
            int ReturnCode;
            String ReturnMsg;
            Entity.ConversationChatBox objEntity = new Entity.ConversationChatBox();
            objEntity.ModuleName = hdnModuleName.Value;
            objEntity.KeyValue = hdnKeyValue.Value;
            objEntity.CustomerID = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
            objEntity.FromUser = hdnLoginUserID.Value;
            objEntity.ToUser = hdnLoginUserID.Value;
            objEntity.Message = txtMessage.Text;

            if (!String.IsNullOrEmpty(hdnLoginUserID.Value) && !String.IsNullOrEmpty(txtMessage.Text))
            {
                BAL.CommonMgmt.AddUpdateConversationChatBox(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
                if (ReturnCode > 0)
                {
                    txtMessage.Text = "";
                    LoadConversationlog(hdnModuleName.Value, hdnKeyValue.Value);
                }
            }

        }
    }
}