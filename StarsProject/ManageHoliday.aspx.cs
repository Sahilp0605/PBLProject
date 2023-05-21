using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
     public partial class ManageHoliday : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown();

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnHolidayID.Value = Request.QueryString["id"].ToString();

                    if (hdnHolidayID.Value == "0" || hdnHolidayID.Value == "")
                    {
                        ClearAllField();
                    }
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
        }
        public void ClearAllField()
        {
            txtHolidayType.Text = "";
            txtName.Text = "";
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtDescription.Text = "";
            txtImageURL.Text = "";
          
        }

        public void OnlyViewControls()
        {          
            txtHolidayType.ReadOnly  = true;
            txtName.ReadOnly = true;
            txtDate.ReadOnly = true;
            txtDescription.ReadOnly = true;
            txtImageURL.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {              
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Holiday> lstEntity = new List<Entity.Holiday>();

                lstEntity = BAL.HolidayMgmt.GetHolidayList(Convert.ToInt64(hdnHolidayID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnHolidayID.Value = lstEntity[0].pkID.ToString();
                txtHolidayType.Text = lstEntity[0].Holiday_Type;
                txtName.Text = lstEntity[0].Holiday_Name;
                txtDate.Text = lstEntity[0].Holiday_Date.ToString("dd-MM-yyyy");
                txtDescription.Text = Convert.ToString(lstEntity[0].Holiday_Description);
                txtImageURL.Text = Convert.ToString(lstEntity[0].imageurl);
                txtHolidayType.Focus();
            }
        }

        public void BindDropDown()
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String errMsg = "";

            if (String.IsNullOrEmpty(txtDate.Text) || String.IsNullOrEmpty(txtName.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtName.Text))
                    errMsg += "<li>" + "Holiday Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtDate.Text) || String.IsNullOrEmpty(txtDate.Text))
                    errMsg += "<li>" + "Holiday Date is Required." + "</li>";

            }

            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.Holiday objEntity = new Entity.Holiday();

                if (!String.IsNullOrEmpty(hdnHolidayID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnHolidayID.Value);

                objEntity.pkID =  Convert.ToInt64(hdnHolidayID.Value);
                objEntity.Holiday_Year = Convert.ToInt64(Convert.ToDateTime(txtDate.Text).Year.ToString());             
                objEntity.Holiday_Type = txtHolidayType.Text;
                objEntity.Holiday_Name = txtName.Text;
                objEntity.Holiday_Date = Convert.ToDateTime(txtDate.Text);
                objEntity.Holiday_Description = txtDescription.Text;
                objEntity.imageurl = txtImageURL.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.HolidayMgmt.AddUpdateHoliday(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                errMsg += "<li>" + ReturnMsg + "</li>";
            }
            // ------------------------------------------------------
            if (!String.IsNullOrEmpty(errMsg))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + errMsg + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + errMsg + "','toast-danger');", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteHoliday(string pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.HolidayMgmt.DeleteHoliday(Convert.ToInt64(pkID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterHolidayType(string pHolidayType)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.HolidayMgmt.GetHolidayList(pHolidayType).Select(sel => new { sel.Holiday_Type });
            return serializer.Serialize(rows);
        }

    }
}