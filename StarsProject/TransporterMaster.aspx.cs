using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class TransporterMaster : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
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
                List<Entity.Vehicle> lstEntity = new List<Entity.Vehicle>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();

                lstEntity = BAL.VehicleMgmt.GetTransporterList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtRegistrationNo.Text = lstEntity[0].RegistrationNo;
                txtChasisNo.Text = lstEntity[0].ChasisNo;
                txtMfg.Text = lstEntity[0].Mfg;
                txtModel.Text = lstEntity[0].Model;
                txtColor.Text = lstEntity[0].Color;
                txtVehicleType.Text = lstEntity[0].VehicleType;
                txtMfgYear.Text = lstEntity[0].MfgYear;
                txtEngineCC.Text = lstEntity[0].EngineCC;
                txtOwnerName.Text = lstEntity[0].OwnerName;
                txtOwnerAddress.Text = lstEntity[0].OwnerAddress;
                txtOwnerMobile.Text = lstEntity[0].OwnerMobile;
                txtOwnerLandline.Text = lstEntity[0].OwnerLandline;
                txtInsuranceCompany.Text = lstEntity[0].InsuranceCompany;
                txtInsurancePolicyNo.Text = lstEntity[0].InsurancePolicyNo;
                txtInsuranceExpiry.Text = lstEntity[0].InsuranceExpiry.ToString("yyyy-MM-dd");
                txtRatePerKM.Text = lstEntity[0].RatePerKM.ToString("0.00");
                txtGrossWeight.Text = lstEntity[0].Gross_Weight.ToString("0.00");
                txtTareWeight.Text = lstEntity[0].Tare_Weight.ToString("0.00");
                txtNetWeight.Text = lstEntity[0].Net_Weight.ToString("0.00");
                txtLicenseNo.Text = lstEntity[0].LicenseNo.ToString();
                txtTripDate.Text = lstEntity[0].TripDate.ToString("yyyy-MM-dd");
                txtDriverName.Text = lstEntity[0].DriverName.ToString();
                txtFromStation.Text = lstEntity[0].From_Station.ToString();
                txtToStation.Text = lstEntity[0].To_Station.ToString();
                txtKilometers.Text = lstEntity[0].Kilometers.ToString("0.00");
                txtOwnerName.Focus();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtRegistrationNo.Text) || String.IsNullOrEmpty(txtOwnerName.Text) )
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtRegistrationNo.Text))
                {
                    strErr += "<li>" + "RegistrationNo is required !" + "</li>";
                }

                if (String.IsNullOrEmpty(txtOwnerName.Text))
                    strErr += "<li>" + "OwnerName is required !" + "</li>";

            }

            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.Vehicle objEntity = new Entity.Vehicle();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.RegistrationNo = txtRegistrationNo.Text;
                objEntity.ChasisNo = txtChasisNo.Text;
                objEntity.Mfg = txtMfg.Text;
                objEntity.Model = txtModel.Text;
                objEntity.Color = txtColor.Text;
                objEntity.VehicleType = txtVehicleType.Text;
                objEntity.MfgYear = txtMfgYear.Text;
                objEntity.EngineCC = txtEngineCC.Text;
                objEntity.OwnerName = txtOwnerName.Text;
                objEntity.OwnerAddress = txtOwnerAddress.Text;
                objEntity.OwnerMobile = txtOwnerMobile.Text;
                objEntity.OwnerLandline = txtOwnerLandline.Text;
                objEntity.InsuranceCompany = txtInsuranceCompany.Text;
                objEntity.InsurancePolicyNo = txtInsurancePolicyNo.Text;
                objEntity.RatePerKM = (!String.IsNullOrEmpty(txtRatePerKM.Text)) ? Convert.ToDecimal(txtRatePerKM.Text) : 0;
                objEntity.Gross_Weight = (!String.IsNullOrEmpty(txtGrossWeight.Text)) ? Convert.ToDecimal(txtGrossWeight.Text) : 0;
                objEntity.Tare_Weight = (!String.IsNullOrEmpty(txtTareWeight.Text)) ? Convert.ToDecimal(txtTareWeight.Text) : 0;
                objEntity.Net_Weight = (!String.IsNullOrEmpty(txtNetWeight.Text)) ? Convert.ToDecimal(txtNetWeight.Text) : 0;
                objEntity.LicenseNo = (!String.IsNullOrEmpty(txtLicenseNo.Text)) ? txtLicenseNo.Text : "";

                //if (!String.IsNullOrEmpty(txtTripDate.Text))
                //    objEntity.TripDate = Convert.ToDateTime(Convert.ToDateTime(txtTripDate.Text).ToString("yyyy-MM-dd"));
                objEntity.TripDate = (!String.IsNullOrEmpty(txtTripDate.Text)) ? Convert.ToDateTime(txtTripDate.Text) : SqlDateTime.MinValue.Value;
                objEntity.DriverName = txtDriverName.Text;
                objEntity.From_Station = txtFromStation.Text;
                objEntity.To_Station = txtToStation.Text;
                objEntity.Kilometers = (!String.IsNullOrEmpty(txtKilometers.Text)) ? Convert.ToDecimal(txtKilometers.Text) : 0;

                //if (!String.IsNullOrEmpty(txtInsuranceExpiry.Text))
                //    objEntity.InsuranceExpiry = Convert.ToDateTime(Convert.ToDateTime(txtInsuranceExpiry.Text).ToString("yyyy-MM-dd"));
                objEntity.InsuranceExpiry = (!String.IsNullOrEmpty(txtInsuranceExpiry.Text)) ? Convert.ToDateTime(txtInsuranceExpiry.Text) : SqlDateTime.MinValue.Value;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.VehicleMgmt.AddUpdateTransporter(objEntity, out ReturnCode, out ReturnMsg);
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
        public void OnlyViewControls()
        {
            txtRegistrationNo.ReadOnly = true;
            txtChasisNo.ReadOnly = true;
            txtMfg.ReadOnly = true;
            txtModel.ReadOnly = true;
            txtColor.ReadOnly = true;
            txtVehicleType.ReadOnly = true;
            txtMfgYear.ReadOnly = true;
            txtEngineCC.ReadOnly = true;
            txtOwnerName.ReadOnly = true;
            txtOwnerAddress.ReadOnly = true;
            txtOwnerMobile.ReadOnly = true;
            txtOwnerLandline.ReadOnly = true;
            txtInsuranceCompany.ReadOnly = true;
            txtInsurancePolicyNo.ReadOnly = true;
            txtInsuranceExpiry.ReadOnly = true;
            txtRatePerKM.ReadOnly = true;
            txtGrossWeight.ReadOnly = true;
            txtTareWeight.ReadOnly = true;
            txtNetWeight.ReadOnly = true;
            txtLicenseNo.ReadOnly = true;

            txtTripDate.ReadOnly = true;
            txtDriverName.ReadOnly = true;
            txtFromStation.ReadOnly = true;
            txtToStation.ReadOnly = true;
            txtKilometers.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void ClearAllField()
        {
            hdnpkID.Value = "0";
            txtRegistrationNo.Text = "";
            txtChasisNo.Text = "";
            txtMfg.Text = "";
            txtModel.Text = "";
            txtColor.Text = "";
            txtVehicleType.Text = "";
            txtMfgYear.Text = "";
            txtEngineCC.Text = "";
            txtOwnerName.Text = "";
            txtOwnerAddress.Text = "";
            txtOwnerMobile.Text = "";
            txtOwnerLandline.Text = "";
            txtInsuranceCompany.Text = "";
            txtInsurancePolicyNo.Text = "";
            txtInsuranceExpiry.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtRatePerKM.Text = "";
            txtGrossWeight.Text = "";
            txtTareWeight.Text = "";
            txtNetWeight.Text = "";
            txtLicenseNo.Text = "";
            txtRegistrationNo.Focus();

            txtTripDate.Text= DateTime.Today.ToString("yyyy-MM-dd");
            txtDriverName.Text = "";
            txtFromStation.Text = "";
            txtToStation.Text = "";
            txtKilometers.Text = "";

            btnSave.Disabled = false;
        }
        [System.Web.Services.WebMethod]
        public static string DeleteTransporter(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.VehicleMgmt.DeleteTransporter(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
        protected void CalculateTrip(object sender, EventArgs e)
        {
            //Int64 r1, r2, km;
            //Decimal dc, cost = 0;
            //r1 = String.IsNullOrEmpty(txtReading1.Text) ? 0 : Convert.ToInt64(txtReading1.Text);
            //r2 = String.IsNullOrEmpty(txtReading2.Text) ? 0 : Convert.ToInt64(txtReading2.Text);
            //km = ((r2 - r1) > 0) ? (r2 - r1) : 0;
            //txtKilometers.Text = km.ToString();
            //dc = String.IsNullOrEmpty(txtDieselCharge.Text) ? 0 : Convert.ToDecimal(txtDieselCharge.Text);
            //if (dc > 0)
            //    cost = km / dc;
            //txtTripCost.Text = cost.ToString();
        }
    }
}