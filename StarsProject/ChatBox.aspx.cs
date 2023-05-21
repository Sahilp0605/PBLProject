using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ChatBox : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                spnLoginUserName.InnerText = objAuth.EmployeeName;
                imgLoginUser.Src = (!String.IsNullOrEmpty(objAuth.EmployeeImage)) ? objAuth.EmployeeImage : "images/customer.png";
                hdnLoginUserID.Value = objAuth.UserID;
                // ------------------------------------------------------
                BindChatUserList();
            }
        }

        public void BindChatBoxList(String pToUserID)
        {
            List<Entity.Chat> lstEntity = new List<Entity.Chat>();
            lstEntity = BAL.CommonMgmt.GetChatBoxList(hdnLoginUserID.Value, pToUserID);
            rptChatBoxList.DataSource = lstEntity;
            rptChatBoxList.DataBind();
            btnFocus.Focus();
        }

        protected void rptChatBoxList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn1 = ((HiddenField)e.Item.FindControl("hdnFlag"));
                HtmlGenericControl dv = ((HtmlGenericControl)e.Item.FindControl("dvItem"));
                HtmlImage img = ((HtmlImage)e.Item.FindControl("imgChatUser"));

                if (hdn1.Value == "Send")
                {
                    dv.Attributes.Add("class", "chat chat-right");
                    img.Src = imgLoginUser.Src;
                }
                else
                {
                    dv.Attributes.Add("class", "chat");
                    img.Src = imgCurrUser.Src;
                }

            }
        }
        // ==================================================================
        public void BindChatUserList()
        {
            List<Entity.Chat> lstUserList = new List<Entity.Chat>();
            lstUserList = BAL.CommonMgmt.GetChatBoxUserList(hdnLoginUserID.Value);
            //lstUserList = lstUserList.Where(e => (e.UserID != hdnLoginUserID.Value)).ToList();
            rptUserList.DataSource = lstUserList;
            rptUserList.DataBind();
            if (lstUserList.Count > 0)
            {
                spnCurrentUser.InnerText = lstUserList[0].EmployeeName;
                hdnCurrentUserID.Value = lstUserList[0].UserID;
                imgCurrUser.Src = (!String.IsNullOrEmpty(lstUserList[0].EmployeeImage)) ? lstUserList[0].EmployeeImage : "images/customer.png";
                BindChatBoxList(hdnCurrentUserID.Value);
            }
            // ---------------------------------------------------------
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:ScrollToBottom();", true);
        }
        // ==================================================================
        protected void rptUserList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl spnUnread = ((HtmlGenericControl)e.Item.FindControl("spnUnread"));
                
                if (spnUnread.InnerText == "" || spnUnread.InnerText == "0")
                {
                    spnUnread.Attributes.Remove("class");
                    spnUnread.Attributes.Add("class", "badge pill white");
                    
                }
                else
                {
                    spnUnread.Attributes.Remove("class");
                    spnUnread.Attributes.Add("class", "badge pill red");
                    
                }

            }
        }

        protected void rptUserList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int totrec = 0;
            foreach (RepeaterItem ri in rptUserList.Items)
            {
                HtmlGenericControl ctrl = ((HtmlGenericControl)ri.FindControl("divUser"));
                ctrl.Attributes.Remove("class");
                ctrl.Attributes.Add("class","chat-user");
            }
            // ------------------------------------------------------------
            if (e.CommandName.ToString() == "Select")
            {
                hdnCurrentUserID.Value = e.CommandArgument.ToString();

                HtmlGenericControl spnUnread = ((HtmlGenericControl)e.Item.FindControl("spnUnread"));
                if (spnUnread.InnerText != "" && spnUnread.InnerText != "0")
                {
                    spnUnread.Attributes.Remove("class");
                    spnUnread.Attributes.Add("class", "badge pill white");
                }
                // ----------------------------------------------------
                // Updating : Last Seen Messages Timestamp
                // ----------------------------------------------------
                BAL.CommonMgmt.UpdateChatLastTimestamp(hdnLoginUserID.Value, hdnCurrentUserID.Value);
                // ---------------------------------------------------------------------------
                HtmlGenericControl ctrl = ((HtmlGenericControl)e.Item.FindControl("divUser"));
                ctrl.Attributes.Remove("class");
                ctrl.Attributes.Add("class", "chat-user active");
                // -------------------------------------------------

                List<Entity.Chat> lstUserList = new List<Entity.Chat>();
                lstUserList = BAL.CommonMgmt.GetChatBoxUserList(hdnLoginUserID.Value);
                lstUserList = lstUserList.Where(it => (it.UserID == hdnCurrentUserID.Value)).ToList();
                if (lstUserList.Count > 0)
                {
                    spnCurrentUser.InnerText = lstUserList[0].EmployeeName;
                    hdnCurrentUserID.Value = lstUserList[0].UserID;
                    BindChatBoxList(hdnCurrentUserID.Value);
                    imgCurrUser.Src = lstUserList[0].EmployeeImage;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String strErr = "";
            int ReturnCode;
            String ReturnMsg;
            Entity.Chat objEntity = new Entity.Chat();
            objEntity.FromUser = hdnLoginUserID.Value;
            objEntity.ToUser = hdnCurrentUserID.Value;
            objEntity.Message = txtMessage.Text;

            if (!String.IsNullOrEmpty(hdnLoginUserID.Value) && !String.IsNullOrEmpty(hdnCurrentUserID.Value) && !String.IsNullOrEmpty(txtMessage.Text))
            {
                BAL.CommonMgmt.AddUpdateChatBox(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
                if (ReturnCode > 0)
                {
                    txtMessage.Text = "";
                    BindChatBoxList(hdnCurrentUserID.Value);
                }
                    
            }

        }

    }
}