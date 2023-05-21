using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ShiftMaster : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnShiftCode.Value = Request.QueryString["id"].ToString();

                    if (hdnShiftCode.Value == "0" || hdnShiftCode.Value == "")
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtShiftName.Text) || String.IsNullOrEmpty(txtStartTime.Text) || String.IsNullOrEmpty(txtEndTime.Text) || String.IsNullOrEmpty(txtGraceMins.Text) || String.IsNullOrEmpty(txtMinHrsHalfDay.Text) || String.IsNullOrEmpty(txtMinHrsFullDay.Text) 
                || String.IsNullOrEmpty(txtLunchFrom.Text) || String.IsNullOrEmpty(txtLunchTo.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtShiftName.Text))
                    strErr += "<li>" + "Shift Name is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtStartTime.Text))
                    strErr += "<li>" + "Start Time is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtEndTime.Text))
                    strErr += "<li>" + "End Time is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtGraceMins.Text))
                    strErr += "<li>" + "Grace Minutes is mandatory !" + "</li>";


                if (String.IsNullOrEmpty(txtMinHrsHalfDay.Text))
                    strErr += "<li>" + "Minimum Hours Of HalfDay is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtMinHrsFullDay.Text))
                    strErr += "<li>" + "Minimum Hours Of FullDayis mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtLunchFrom.Text))
                    strErr += "<li>" + "Lunch From is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtLunchTo.Text))
                    strErr += "<li>" + "Lunch To is mandatory !" + "</li>";
            }
            //if (!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtToDate.Text))
            //{

            //    if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            //    {
            //        _pageValid = false;
            //        strErr += "<li>" + "From Date is Always Less then To Date." + "</li>";
            //    }
            //}

            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.ShiftMaster objEntity = new Entity.ShiftMaster();

                if (!String.IsNullOrEmpty(hdnShiftCode.Value))
                    objEntity.ShiftCode = Convert.ToInt64(hdnShiftCode.Value);

                objEntity.ShiftName = txtShiftName.Text;
                objEntity.StartTime = txtStartTime.Text;
                objEntity.EndTime = txtEndTime.Text;
                //objEntity.FromDate = Convert.ToDateTime(txtFromDate.Text);
                //objEntity.ToDate = Convert.ToDateTime((Convert.ToDateTime(txtToDate.Text)).ToString("yyyy-MM-dd 23:59:59"));
                //if (!String.IsNullOrEmpty(txtStartTime.Text))
                //    objEntity.StartTime = Convert.ToDateTime(Convert.ToDateTime(txtStartTime.Text + " " + txtFromTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                //if (!String.IsNullOrEmpty(txtToDate.Text) && !String.IsNullOrEmpty(txtToTime.Text))
                //    objEntity.ToDate = Convert.ToDateTime(Convert.ToDateTime(txtToDate.Text + " " + txtToTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                objEntity.GraceMins = Convert.ToInt64(txtGraceMins.Text);
                objEntity.MinHrsHalfDay = Convert.ToDecimal(txtMinHrsHalfDay.Text);
                objEntity.MinHrsFullDay = Convert.ToDecimal(txtMinHrsFullDay.Text);
                objEntity.LunchFrom = txtLunchFrom.Text;
                objEntity.LunchTo = txtLunchTo.Text;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ShiftMasterMgmt.AddUpdateShiftMaster(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                }
            }
            if (ReturnCode > 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
        }

        public void ClearAllField()
        {
            txtShiftName.Text = "";
            txtStartTime.Text = "";
            txtEndTime.Text = "";
            txtGraceMins.Text = "";
            txtMinHrsHalfDay.Text = "";
            txtMinHrsFullDay.Text = "";
            txtLunchFrom.Text = "";
            txtLunchTo.Text = "";

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.ShiftMaster> lstEntity = new List<Entity.ShiftMaster>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();

                lstEntity = BAL.ShiftMasterMgmt.GetShiftMaster(Convert.ToInt64(hdnShiftCode.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnShiftCode.Value = lstEntity[0].ShiftCode.ToString();
                txtShiftName.Text = lstEntity[0].ShiftName;
                txtStartTime.Text = lstEntity[0].StartTime.ToString();
                txtEndTime.Text = lstEntity[0].EndTime.ToString();
                txtGraceMins.Text = lstEntity[0].GraceMins.ToString();
                txtMinHrsHalfDay.Text = lstEntity[0].MinHrsHalfDay.ToString();
                txtMinHrsFullDay.Text = lstEntity[0].MinHrsFullDay.ToString();
                txtLunchFrom.Text = lstEntity[0].LunchFrom.ToString();
                txtLunchTo.Text = lstEntity[0].LunchTo.ToString();

                txtShiftName.Focus();
            }
        }
        public void OnlyViewControls()
        {
            
            txtShiftName.ReadOnly = true;
            txtStartTime.ReadOnly = true;
            txtEndTime.ReadOnly = true;
            txtGraceMins.ReadOnly = true;
            txtMinHrsHalfDay.ReadOnly = true;
            txtMinHrsFullDay.ReadOnly = true;
            txtLunchFrom.ReadOnly = true;
            txtLunchTo.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        [System.Web.Services.WebMethod]
        public static string DeleteShiftMaster(Int64 ShiftCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ShiftMasterMgmt.DeleteShiftMaster(ShiftCode, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

    }
   
}