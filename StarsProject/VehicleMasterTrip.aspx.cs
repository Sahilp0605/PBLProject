using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class VehicleMasterTrip : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindVehicles();
                BindEmployees();
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

        public void BindVehicles()
        {
            int totrec = 0;
            drpVehicle.Items.Clear();
            List<Entity.Vehicle> lstVehicle = new List<Entity.Vehicle>();
            lstVehicle = BAL.VehicleMgmt.GetVehicleList(0, Session["LoginUserID"].ToString(), 1, 1000, out totrec);
            drpVehicle.DataSource = lstVehicle;
            drpVehicle.DataValueField = "pkID";
            drpVehicle.DataTextField = "RegistrationNo";
            drpVehicle.DataBind();
            drpVehicle.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void BindEmployees()
        {
            drpDriverName.Items.Clear();
            List<Entity.OrganizationEmployee> lstemployee = new List<Entity.OrganizationEmployee>();
            lstemployee = BAL.OrganizationEmployeeMgmt.GetEmployeeList("");
            drpDriverName.DataSource = lstemployee;
            drpDriverName.DataValueField = "pkID";
            drpDriverName.DataTextField = "EmployeeName";
            drpDriverName.DataBind();
            drpDriverName.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.VehicleTrip> lstEntity = new List<Entity.VehicleTrip>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();

                lstEntity = BAL.VehicleMgmt.GetVehicleTripList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtTripDate.Text = lstEntity[0].TripDate.ToString("yyyy-MM-dd");
                drpVehicle.SelectedValue = lstEntity[0].VehicleID.ToString();
                drpDriverName.Text = lstEntity[0].DriverName;
                drpDriverName.SelectedValue = lstEntity[0].EmployeeID.ToString();
                txtFromStation.Text = lstEntity[0].From_Station;
                txtToStation.Text = lstEntity[0].To_Station;
                txtReading1.Text = lstEntity[0].Reading1.ToString();
                txtReading2.Text = lstEntity[0].Reading2.ToString();
                txtKilometers.Text = lstEntity[0].Kilometers.ToString();
                txtDieselCharge.Text = lstEntity[0].DieselCharge.ToString();
                txtAmount.Text = lstEntity[0].Amount.ToString();
                txtTripCost.Text = lstEntity[0].TripCost.ToString();
                txtToll.Text = lstEntity[0].Toll.ToString();
                txtBhatthu.Text = lstEntity[0].Bhatthu.ToString();
                txtDriverAllowance.Text = lstEntity[0].DriverAllowance.ToString();
                txtRemarks.Text = lstEntity[0].Remarks.ToString();

                //----------------Newly Added---------------------------------
                txtInsuranceAmount.Text = lstEntity[0].InsuranceAmount.ToString();
                txtInsurancePerTrip.Text = lstEntity[0].InsurancePerTrip.ToString();
                txtGovernmentTax.Text = lstEntity[0].GovernmentTax.ToString();
                txtExplosiveTax.Text = lstEntity[0].ExplosiveTax.ToString();
                txtVehicleAmount.Text = lstEntity[0].VehicleAmount.ToString();
                txtDepreciationPerDay.Text = lstEntity[0].DepreciationPerDay.ToString();
                txtWeightKgPerCylinderQty.Text = lstEntity[0].WeightKgPerCylinderQty.ToString();
                txtMaterialName.Text = lstEntity[0].MaterialName.ToString();
                //--------------------------------------------------------

                txtTripDate.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtTripDate.Text) || String.IsNullOrEmpty(drpVehicle.SelectedValue) || String.IsNullOrEmpty(drpDriverName.SelectedValue) ||
                String.IsNullOrEmpty(txtFromStation.Text) || String.IsNullOrEmpty(txtToStation.Text) || String.IsNullOrEmpty(txtReading1.Text) || String.IsNullOrEmpty(txtReading2.Text) ||
                (String.IsNullOrEmpty(txtKilometers.Text) || txtKilometers.Text == "0") || String.IsNullOrEmpty(txtDieselCharge.Text) || String.IsNullOrEmpty(txtTripCost.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtTripDate.Text))
                    strErr += "<li>" + "Trip Date is required !" + "</li>";

                if (String.IsNullOrEmpty(drpVehicle.SelectedValue))
                    strErr += "<li>" + "Vehicle is required !" + "</li>";

                if (String.IsNullOrEmpty(drpDriverName.SelectedValue))
                    strErr += "<li>" + "Driver Name is required !" + "</li>";

                if (String.IsNullOrEmpty(txtFromStation.Text))
                    strErr += "<li>" + "From Location is required !" + "</li>";

                if (String.IsNullOrEmpty(txtToStation.Text))
                    strErr += "<li>" + "To Location is required !" + "</li>";

                if (String.IsNullOrEmpty(txtReading1.Text))
                    strErr += "<li>" + "Start Odometer is required !" + "</li>";

                if (String.IsNullOrEmpty(txtReading2.Text))
                    strErr += "<li>" + "End Odometer is required !" + "</li>";

                if (String.IsNullOrEmpty(txtKilometers.Text) || txtKilometers.Text == "0")
                    strErr += "<li>" + "Kilometers is required !" + "</li>";

                if (String.IsNullOrEmpty(txtDieselCharge.Text))
                    strErr += "<li>" + "Diesel In (Ltrs) is required !" + "</li>";

                if (String.IsNullOrEmpty(txtTripCost.Text))
                    strErr += "<li>" + "Trip AVG is required !" + "</li>";
            }

            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.VehicleTrip objEntity = new Entity.VehicleTrip();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                if (!String.IsNullOrEmpty(txtTripDate.Text))
                    objEntity.TripDate = Convert.ToDateTime(Convert.ToDateTime(txtTripDate.Text).ToString("yyyy-MM-dd"));

                objEntity.VehicleID = Convert.ToInt64(drpVehicle.SelectedValue);
                objEntity.EmployeeID = Convert.ToInt64(drpDriverName.SelectedValue);
                objEntity.DriverName = drpDriverName.SelectedItem.Text;
                objEntity.From_Station = txtFromStation.Text;
                objEntity.To_Station = txtToStation.Text;

                objEntity.Reading1 = Convert.ToInt64(txtReading1.Text);
                objEntity.Reading2 = Convert.ToInt64(txtReading2.Text);
                objEntity.Kilometers = Convert.ToInt64(txtKilometers.Text);

                objEntity.DieselCharge = (!String.IsNullOrEmpty(txtDieselCharge.Text)) ? Convert.ToDecimal(txtDieselCharge.Text) : 0;
                objEntity.Amount = (!String.IsNullOrEmpty(txtAmount.Text)) ? Convert.ToDecimal(txtAmount.Text) : 0;
                objEntity.TripCost = (!String.IsNullOrEmpty(txtTripCost.Text)) ? Convert.ToDecimal(txtTripCost.Text) : 0;
                objEntity.Toll = (!String.IsNullOrEmpty(txtToll.Text)) ? Convert.ToDecimal(txtToll.Text) : 0;
                objEntity.Bhatthu= (!String.IsNullOrEmpty(txtBhatthu.Text)) ? Convert.ToDecimal(txtBhatthu.Text) : 0;
                objEntity.DriverAllowance = (!String.IsNullOrEmpty(txtDriverAllowance.Text)) ? Convert.ToDecimal(txtDriverAllowance.Text) : 0;
                objEntity.Remarks = txtRemarks.Text;

                //--------------Newly Addded-------------------------------------------------------------------
                objEntity.InsuranceAmount = (!String.IsNullOrEmpty(txtInsuranceAmount.Text)) ? Convert.ToDecimal(txtInsuranceAmount.Text) : 0;
                objEntity.InsurancePerTrip= (!String.IsNullOrEmpty(txtInsurancePerTrip.Text)) ? Convert.ToDecimal(txtInsurancePerTrip.Text) : 0;
                objEntity.GovernmentTax = (!String.IsNullOrEmpty(txtGovernmentTax.Text)) ? Convert.ToDecimal(txtGovernmentTax.Text) : 0;
                objEntity.ExplosiveTax= (!String.IsNullOrEmpty(txtExplosiveTax.Text)) ? Convert.ToDecimal(txtExplosiveTax.Text) : 0;
                objEntity.VehicleAmount = (!String.IsNullOrEmpty(txtVehicleAmount.Text)) ? Convert.ToDecimal(txtVehicleAmount.Text) : 0;
                objEntity.DepreciationPerDay= (!String.IsNullOrEmpty(txtDepreciationPerDay.Text)) ? Convert.ToDecimal(txtDepreciationPerDay.Text) : 0;
                objEntity.WeightKgPerCylinderQty= (!String.IsNullOrEmpty(txtWeightKgPerCylinderQty.Text)) ? Convert.ToDecimal(txtWeightKgPerCylinderQty.Text) : 0;
                objEntity.MaterialName= txtMaterialName.Text;
                //--------------------------------------------------------------------------------------------------------

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.VehicleMgmt.AddUpdateVehicleTrip(objEntity, out ReturnCode, out ReturnMsg);
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
            txtTripDate.ReadOnly = true;
            drpVehicle.Attributes.Add("disabled", "disabled");
            drpDriverName.Attributes.Add("disabled", "disabled");
            txtFromStation.ReadOnly = true;
            txtToStation.ReadOnly = true;
            txtReading1.ReadOnly = true;
            txtReading2.ReadOnly = true;
            //txtKilometers.ReadOnly = true;
            txtDieselCharge.ReadOnly = true;
            txtAmount.ReadOnly = true;
            txtToll.ReadOnly =true;
            txtBhatthu.ReadOnly = true;
            txtDriverAllowance.ReadOnly = true;
            txtRemarks.ReadOnly = true;
            //txtTripCost.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
            //------------ Newly Added -------------------
            txtInsuranceAmount.ReadOnly = true;
            txtInsurancePerTrip.ReadOnly = true;
            txtGovernmentTax.ReadOnly = true;
            txtExplosiveTax.ReadOnly = true;
            txtVehicleAmount.ReadOnly = true;
            txtDepreciationPerDay.ReadOnly = true;
            txtWeightKgPerCylinderQty.ReadOnly = true;
            txtMaterialName.ReadOnly = true;
            //---------------------------------------





        }

        public void ClearAllField()
        {
            txtTripDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            drpVehicle.SelectedValue = "0";
            drpDriverName.SelectedValue = "0";
            txtFromStation.Text = "";
            txtToStation.Text = "";
            txtReading1.Text = "";
            txtReading2.Text = "";
            txtKilometers.Text = "";
            txtDieselCharge.Text = "";
            txtAmount.Text = "";
            txtTripCost.Text = "";
            txtTripDate.Focus();
            txtToll.Text = "";
            txtBhatthu.Text = "";
            txtDriverAllowance.Text = "";
            txtRemarks.Text = "";

            //----------Newly Added---------------------
            txtInsuranceAmount.Text = "";
            txtInsurancePerTrip.Text = "";
            txtGovernmentTax.Text = "";
            txtExplosiveTax.Text = "";
            txtVehicleAmount.Text = "";
            txtDepreciationPerDay.Text = "";
            txtWeightKgPerCylinderQty.Text = "";
            txtMaterialName.Text = "";
            //----------------------------------------------

            btnSave.Disabled = false;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteVehicleTrip(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.VehicleMgmt.DeleteVehicleTrip(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void CalculateTrip(object sender, EventArgs e)
        {
            Int64 r1, r2, km;
            Decimal dc, cost = 0;
            r1 = String.IsNullOrEmpty(txtReading1.Text) ? 0 : Convert.ToInt64(txtReading1.Text);
            r2 = String.IsNullOrEmpty(txtReading2.Text) ? 0 : Convert.ToInt64(txtReading2.Text);
            km = ((r2 - r1) > 0) ? (r2 - r1) : 0;
            txtKilometers.Text = km.ToString();
            dc = String.IsNullOrEmpty(txtDieselCharge.Text) ? 0 : Convert.ToDecimal(txtDieselCharge.Text);
            if (dc > 0)
                cost = km / dc;
            txtTripCost.Text = cost.ToString();
        }
    }
}