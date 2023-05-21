using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class BroadcastMessage : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                //BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                        ClearAllField();
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            if (Request.QueryString["mode"].ToString() == "view")
                                OnlyViewControls();
                        }
                    }
                }
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
            }
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.BroadCastMessage> lstEntity = new List<Entity.BroadCastMessage>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();
                
                lstEntity = BAL.BroadCastMessageMgmt.GetBroadCastMessage(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtMessage.Text = lstEntity[0].Message;
                txtFromDate.Text = lstEntity[0].FromDate.ToString("yyyy-MM-dd");
                txtFromTime.Text = lstEntity[0].FromDate.ToString("HH:mm tt");
                txtToDate.Text = lstEntity[0].ToDate.ToString("yyyy-MM-dd");
                txtToTime.Text = lstEntity[0].ToDate.ToString("HH:mm tt");
                txtMessage.Focus();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtMessage.Text) || String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtToDate.Text) || String.IsNullOrEmpty(txtFromTime.Text) || String.IsNullOrEmpty(txtToTime.Text)
                || txtMessage.Text.Length > 1000)
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtMessage.Text))
                {
                    strErr += "<li>" + "Message is mandatory !" + "</li>";
                }

                if (String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtToDate.Text))
                    strErr += "<li>" + "Period Date is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtFromTime.Text) || String.IsNullOrEmpty(txtToTime.Text))
                    strErr += "<li>" + "Period Time is mandatory !" + "</li>";

                if (txtMessage.Text.Length > 1000)
                    strErr += "<li>" + "Message should not greater than 1000 characters." + "</li>";

            }
            if (!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtToDate.Text))
            {

                if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "From Date is Always Less then To Date." + "</li>";
                }
            }

            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.BroadCastMessage objEntity = new Entity.BroadCastMessage();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID  = Convert.ToInt64(hdnpkID.Value);

                objEntity.Message = txtMessage.Text;
                //objEntity.FromDate = Convert.ToDateTime(txtFromDate.Text);
                //objEntity.ToDate = Convert.ToDateTime((Convert.ToDateTime(txtToDate.Text)).ToString("yyyy-MM-dd 23:59:59"));
                if (!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtFromTime.Text))
                    objEntity.FromDate = Convert.ToDateTime(Convert.ToDateTime(txtFromDate.Text + " " + txtFromTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                if (!String.IsNullOrEmpty(txtToDate.Text) && !String.IsNullOrEmpty(txtToTime.Text))
                    objEntity.ToDate = Convert.ToDateTime(Convert.ToDateTime(txtToDate.Text + " " + txtToTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.BroadCastMessageMgmt.AddUpdateBroadCastMessage(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                }
            }
            if (ReturnCode>0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
        }

        public void OnlyViewControls()
        {
            txtMessage.ReadOnly = true;
            txtFromDate.ReadOnly = true;
            txtToDate.ReadOnly = true;
            txtFromTime.ReadOnly = true;
            txtToTime.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void ClearAllField()
        {
            txtMessage.Text = "";
            txtFromDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFromTime.Text = DateTime.Today.ToString("HH:mm tt");
            txtToTime.Text = DateTime.Today.ToString("HH:mm tt");
            txtMessage.Focus();
            btnSave.Disabled = false;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteBroadCastMessage(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.BroadCastMessageMgmt.DeleteBroadCastMessage(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}