using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class VehicleSQL:BaseSqlManager
    {
        public virtual List<Entity.Vehicle> GetVehicleList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Vehicle> lstLocation = new List<Entity.Vehicle>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "VehicleList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Vehicle objEntity = new Entity.Vehicle();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RegistrationNo = GetTextVale(dr, "RegistrationNo");
                objEntity.ChasisNo = GetTextVale(dr, "ChasisNo");
                objEntity.Mfg = GetTextVale(dr, "Mfg");
                objEntity.Model = GetTextVale(dr, "Model");
                objEntity.Color = GetTextVale(dr, "Color");
                objEntity.VehicleType = GetTextVale(dr, "VehicleType");
                objEntity.MfgYear = GetTextVale(dr, "MfgYear");
                objEntity.EngineCC = GetTextVale(dr, "EngineCC");
                objEntity.OwnerName = GetTextVale(dr, "OwnerName");
                objEntity.OwnerAddress = GetTextVale(dr, "OwnerAddress");
                objEntity.OwnerMobile = GetTextVale(dr, "OwnerMobile");
                objEntity.OwnerLandline = GetTextVale(dr, "OwnerLandline");
                objEntity.InsuranceCompany = GetTextVale(dr, "InsuranceCompany");
                objEntity.InsurancePolicyNo = GetTextVale(dr, "InsurancePolicyNo");
                objEntity.InsuranceExpiry = GetDateTime(dr, "InsuranceExpiry");
                objEntity.RatePerKM = GetDecimal(dr, "RatePerKM");
                objEntity.Gross_Weight = GetDecimal(dr, "Gross_Weight");
                objEntity.Tare_Weight = GetDecimal(dr, "Tare_Weight");
                objEntity.Net_Weight = GetDecimal(dr, "Net_Weight");
                objEntity.LicenseNo = GetTextVale(dr, "LicenseNo");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Vehicle> GetVehicleList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Vehicle> lstLocation = new List<Entity.Vehicle>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "VehicleList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Vehicle objEntity = new Entity.Vehicle();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RegistrationNo = GetTextVale(dr, "RegistrationNo");
                objEntity.ChasisNo = GetTextVale(dr, "ChasisNo");
                objEntity.Mfg = GetTextVale(dr, "Mfg");
                objEntity.Model = GetTextVale(dr, "Model");
                objEntity.Color = GetTextVale(dr, "Color");
                objEntity.VehicleType = GetTextVale(dr, "VehicleType");
                objEntity.MfgYear = GetTextVale(dr, "MfgYear");
                objEntity.EngineCC = GetTextVale(dr, "EngineCC");

                objEntity.OwnerName = GetTextVale(dr, "OwnerName");
                objEntity.OwnerAddress = GetTextVale(dr, "OwnerAddress");
                objEntity.OwnerMobile = GetTextVale(dr, "OwnerMobile");
                objEntity.OwnerLandline = GetTextVale(dr, "OwnerLandline");
                objEntity.InsuranceCompany = GetTextVale(dr, "InsuranceCompany");
                objEntity.InsurancePolicyNo = GetTextVale(dr, "InsurancePolicyNo");
                objEntity.InsuranceExpiry = GetDateTime(dr, "InsuranceExpiry");
                objEntity.RatePerKM = GetDecimal(dr, "RatePerKM");
                objEntity.Gross_Weight = GetDecimal(dr, "Gross_Weight");
                objEntity.Tare_Weight = GetDecimal(dr, "Tare_Weight");
                objEntity.Net_Weight = GetDecimal(dr, "Net_Weight");
                objEntity.LicenseNo = GetTextVale(dr, "LicenseNo");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateVehicle(Entity.Vehicle objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Vehicle_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@RegistrationNo", objEntity.RegistrationNo);
            cmdAdd.Parameters.AddWithValue("@ChasisNo", objEntity.ChasisNo);
            cmdAdd.Parameters.AddWithValue("@Mfg", objEntity.Mfg);
            cmdAdd.Parameters.AddWithValue("@Model", objEntity.Model);
            cmdAdd.Parameters.AddWithValue("@Color", objEntity.Color);
            cmdAdd.Parameters.AddWithValue("@VehicleType", objEntity.VehicleType);
            cmdAdd.Parameters.AddWithValue("@MfgYear", objEntity.MfgYear);
            cmdAdd.Parameters.AddWithValue("@EngineCC", objEntity.EngineCC);
            cmdAdd.Parameters.AddWithValue("@OwnerName", objEntity.OwnerName);
            cmdAdd.Parameters.AddWithValue("@OwnerAddress", objEntity.OwnerAddress);
            cmdAdd.Parameters.AddWithValue("@OwnerMobile", objEntity.OwnerMobile);
            cmdAdd.Parameters.AddWithValue("@OwnerLandline", objEntity.OwnerLandline);
            cmdAdd.Parameters.AddWithValue("@InsuranceCompany", objEntity.InsuranceCompany);
            cmdAdd.Parameters.AddWithValue("@InsurancePolicyNo", objEntity.InsurancePolicyNo);
            cmdAdd.Parameters.AddWithValue("@InsuranceExpiry", objEntity.InsuranceExpiry);
            cmdAdd.Parameters.AddWithValue("@RatePerKM", objEntity.RatePerKM);
            cmdAdd.Parameters.AddWithValue("@Gross_Weight", objEntity.Gross_Weight);
            cmdAdd.Parameters.AddWithValue("@Tare_Weight", objEntity.Tare_Weight);
            cmdAdd.Parameters.AddWithValue("@Net_Weight", objEntity.Net_Weight);
            cmdAdd.Parameters.AddWithValue("@LicenseNo", objEntity.LicenseNo);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            //cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();

            ForceCloseConncetion();
        }

        public virtual void DeleteVehicle(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Vehicle_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }


        /* ------------------------------------------------------------------------------------- */
        public virtual List<Entity.VehicleTrip> GetVehicleTripList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.VehicleTrip> lstLocation = new List<Entity.VehicleTrip>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "VehicleTripList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.VehicleTrip objEntity = new Entity.VehicleTrip();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.TripDate = GetDateTime(dr, "TripDate");
                objEntity.VehicleID = GetInt64(dr, "VehicleID");
                objEntity.RegistrationNo = GetTextVale(dr, "RegistrationNo");
                objEntity.DriverName = GetTextVale(dr, "DriverName");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.From_Station = GetTextVale(dr, "From_Station");
                objEntity.To_Station = GetTextVale(dr, "To_Station");
                objEntity.Reading1 = GetInt64(dr, "Reading1");
                objEntity.Reading2 = GetInt64(dr, "Reading2");
                objEntity.Kilometers = GetInt64(dr, "Kilometers");
                objEntity.DieselCharge = GetDecimal(dr, "DieselCharge");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.TripCost = GetDecimal(dr, "TripCost");
                objEntity.Toll = GetDecimal(dr, "Toll");
                objEntity.Bhatthu = GetDecimal(dr, "Bhatthu");
                objEntity.DriverAllowance = GetDecimal(dr, "DriverAllowance");
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                //------------Newly Added--------------------------
                //@InsuranceAmount, @InsurancePerTrip,@GovernmentTax,@ExplosiveTax, @VehicleAmount, @DepreciationPerDay, @WeightKgPerCylinderQty, @MaterialName
                objEntity.InsuranceAmount = GetDecimal(dr, "InsuranceAmount");
                objEntity.InsurancePerTrip = GetDecimal(dr, "InsurancePerTrip");
                objEntity.GovernmentTax = GetDecimal(dr, "GovernmentTax");
                objEntity.ExplosiveTax = GetDecimal(dr, "ExplosiveTax");
                objEntity.VehicleAmount = GetDecimal(dr, "VehicleAmount");
                objEntity.DepreciationPerDay = GetDecimal(dr, "DepreciationPerDay");
                objEntity.WeightKgPerCylinderQty = GetDecimal(dr, "WeightKgPerCylinderQty");
                objEntity.MaterialName = GetTextVale(dr, "MaterialName");
                //----------------------------------------------------


                //objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                //objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.VehicleTrip> GetVehicleTripList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.VehicleTrip> lstLocation = new List<Entity.VehicleTrip>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "VehicleTripList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.VehicleTrip objEntity = new Entity.VehicleTrip();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.TripDate = GetDateTime(dr, "TripDate");
                objEntity.VehicleID = GetInt64(dr, "VehicleID");
                objEntity.RegistrationNo = GetTextVale(dr, "RegistrationNo");
                objEntity.DriverName = GetTextVale(dr, "DriverName");
                objEntity.From_Station = GetTextVale(dr, "From_Station");
                objEntity.To_Station = GetTextVale(dr, "To_Station");
                objEntity.Reading1 = GetInt64(dr, "Reading1");
                objEntity.Reading2 = GetInt64(dr, "Reading2");
                objEntity.Kilometers = GetInt64(dr, "Kilometers");
                objEntity.DieselCharge = GetDecimal(dr, "DieselCharge");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.TripCost = GetDecimal(dr, "TripCost");
                objEntity.Toll = GetDecimal(dr, "Toll");
                objEntity.Bhatthu = GetDecimal(dr, "Bhatthu");
                objEntity.DriverAllowance = GetDecimal(dr, "DriverAllowance");
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                //------------Newly Added--------------------------
                objEntity.InsuranceAmount = GetDecimal(dr, "InsuranceAmount");
                objEntity.InsurancePerTrip= GetDecimal(dr, "InsurancePerTrip");
                objEntity.GovernmentTax = GetDecimal(dr, "GovernmentTax");
                objEntity.ExplosiveTax= GetDecimal(dr, "ExplosiveTax");
                objEntity.VehicleAmount= GetDecimal(dr, "VehicleAmount");
                objEntity.DepreciationPerDay = GetDecimal(dr, "DepreciationPerDay");
                objEntity.WeightKgPerCylinderQty= GetDecimal(dr, "WeightKgPerCylinderQty");
                objEntity.MaterialName= GetTextVale(dr, "MaterialName");
                //----------------------------------------------------
                
                //objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                //objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateVehicleTrip(Entity.VehicleTrip objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "VehicleTrip_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@TripDate", objEntity.TripDate);
            cmdAdd.Parameters.AddWithValue("@VehicleID", objEntity.VehicleID);
            cmdAdd.Parameters.AddWithValue("@DriverName", objEntity.DriverName);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@From_Station", objEntity.From_Station);
            cmdAdd.Parameters.AddWithValue("@To_Station", objEntity.To_Station);
            cmdAdd.Parameters.AddWithValue("@Reading1", objEntity.Reading1);
            cmdAdd.Parameters.AddWithValue("@Reading2", objEntity.Reading2);
            cmdAdd.Parameters.AddWithValue("@Kilometers", objEntity.Kilometers);
            cmdAdd.Parameters.AddWithValue("@DieselCharge", objEntity.DieselCharge);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            cmdAdd.Parameters.AddWithValue("@TripCost", objEntity.TripCost);
            cmdAdd.Parameters.AddWithValue("@Toll", objEntity.Toll);
            cmdAdd.Parameters.AddWithValue("@Bhatthu", objEntity.Bhatthu);

            //--------------Newly Added ----------------------------------------------
            cmdAdd.Parameters.AddWithValue("@InsuranceAmount", objEntity.InsuranceAmount);
            cmdAdd.Parameters.AddWithValue("@InsurancePerTrip", objEntity.InsurancePerTrip);
            cmdAdd.Parameters.AddWithValue("@GovernmentTax", objEntity.GovernmentTax);
            cmdAdd.Parameters.AddWithValue("@ExplosiveTax", objEntity.ExplosiveTax);
            cmdAdd.Parameters.AddWithValue("@VehicleAmount", objEntity.VehicleAmount);
            cmdAdd.Parameters.AddWithValue("@DepreciationPerDay", objEntity.DepreciationPerDay);
            cmdAdd.Parameters.AddWithValue("@WeightKgPerCylinderQty", objEntity.WeightKgPerCylinderQty);
            cmdAdd.Parameters.AddWithValue("@MaterialName", objEntity.MaterialName);
            //-------------------------------------------------------------------------

            cmdAdd.Parameters.AddWithValue("@DriverAllowance", objEntity.DriverAllowance);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            //cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();

            ForceCloseConncetion();
        }

        public virtual void DeleteVehicleTrip(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "VehicleTrip_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }
        /* ------------------------------------------------------------------------------------- */
        public virtual List<Entity.Vehicle> GetTransporterList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Vehicle> lstLocation = new List<Entity.Vehicle>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "TransporterList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Vehicle objEntity = new Entity.Vehicle();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RegistrationNo = GetTextVale(dr, "RegistrationNo");
                objEntity.ChasisNo = GetTextVale(dr, "ChasisNo");
                objEntity.Mfg = GetTextVale(dr, "Mfg");
                objEntity.Model = GetTextVale(dr, "Model");
                objEntity.Color = GetTextVale(dr, "Color");
                objEntity.VehicleType = GetTextVale(dr, "VehicleType");
                objEntity.MfgYear = GetTextVale(dr, "MfgYear");
                objEntity.EngineCC = GetTextVale(dr, "EngineCC");
                objEntity.OwnerName = GetTextVale(dr, "OwnerName");
                objEntity.OwnerAddress = GetTextVale(dr, "OwnerAddress");
                objEntity.OwnerMobile = GetTextVale(dr, "OwnerMobile");
                objEntity.OwnerLandline = GetTextVale(dr, "OwnerLandline");
                objEntity.InsuranceCompany = GetTextVale(dr, "InsuranceCompany");
                objEntity.InsurancePolicyNo = GetTextVale(dr, "InsurancePolicyNo");
                objEntity.InsuranceExpiry = GetDateTime(dr, "InsuranceExpiry");
                objEntity.RatePerKM = GetDecimal(dr, "RatePerKM");
                objEntity.Gross_Weight = GetDecimal(dr, "Gross_Weight");
                objEntity.Tare_Weight = GetDecimal(dr, "Tare_Weight");
                objEntity.Net_Weight = GetDecimal(dr, "Net_Weight");
                objEntity.LicenseNo = GetTextVale(dr, "LicenseNo");

                objEntity.TripDate = GetDateTime(dr, "TripDate");
                objEntity.DriverName = GetTextVale(dr, "Driver");
                objEntity.From_Station = GetTextVale(dr, "FromLocation");
                objEntity.To_Station = GetTextVale(dr, "ToLocation");
                objEntity.Kilometers = GetDecimal(dr, "Kilometers");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Vehicle> GetTransporterList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Vehicle> lstLocation = new List<Entity.Vehicle>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "TransporterList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Vehicle objEntity = new Entity.Vehicle();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RegistrationNo = GetTextVale(dr, "RegistrationNo");
                objEntity.ChasisNo = GetTextVale(dr, "ChasisNo");
                objEntity.Mfg = GetTextVale(dr, "Mfg");
                objEntity.Model = GetTextVale(dr, "Model");
                objEntity.Color = GetTextVale(dr, "Color");
                objEntity.VehicleType = GetTextVale(dr, "VehicleType");
                objEntity.MfgYear = GetTextVale(dr, "MfgYear");
                objEntity.EngineCC = GetTextVale(dr, "EngineCC");

                objEntity.OwnerName = GetTextVale(dr, "OwnerName");
                objEntity.OwnerAddress = GetTextVale(dr, "OwnerAddress");
                objEntity.OwnerMobile = GetTextVale(dr, "OwnerMobile");
                objEntity.OwnerLandline = GetTextVale(dr, "OwnerLandline");
                objEntity.InsuranceCompany = GetTextVale(dr, "InsuranceCompany");
                objEntity.InsurancePolicyNo = GetTextVale(dr, "InsurancePolicyNo");
                objEntity.InsuranceExpiry = GetDateTime(dr, "InsuranceExpiry");
                objEntity.RatePerKM = GetDecimal(dr, "RatePerKM");
                objEntity.Gross_Weight = GetDecimal(dr, "Gross_Weight");
                objEntity.Tare_Weight = GetDecimal(dr, "Tare_Weight");
                objEntity.Net_Weight = GetDecimal(dr, "Net_Weight");
                objEntity.LicenseNo = GetTextVale(dr, "LicenseNo");

                objEntity.TripDate = GetDateTime(dr, "TripDate");
                objEntity.DriverName = GetTextVale(dr, "Driver");
                objEntity.From_Station = GetTextVale(dr, "FromLocation");
                objEntity.To_Station = GetTextVale(dr, "ToLocation");
                objEntity.Kilometers = GetDecimal(dr, "Kilometers");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateTransporter(Entity.Vehicle objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Transporter_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@RegistrationNo", objEntity.RegistrationNo);
            cmdAdd.Parameters.AddWithValue("@ChasisNo", objEntity.ChasisNo);
            cmdAdd.Parameters.AddWithValue("@Mfg", objEntity.Mfg);
            cmdAdd.Parameters.AddWithValue("@Model", objEntity.Model);
            cmdAdd.Parameters.AddWithValue("@Color", objEntity.Color);
            cmdAdd.Parameters.AddWithValue("@VehicleType", objEntity.VehicleType);
            cmdAdd.Parameters.AddWithValue("@MfgYear", objEntity.MfgYear);
            cmdAdd.Parameters.AddWithValue("@EngineCC", objEntity.EngineCC);
            cmdAdd.Parameters.AddWithValue("@OwnerName", objEntity.OwnerName);
            cmdAdd.Parameters.AddWithValue("@OwnerAddress", objEntity.OwnerAddress);
            cmdAdd.Parameters.AddWithValue("@OwnerMobile", objEntity.OwnerMobile);
            cmdAdd.Parameters.AddWithValue("@OwnerLandline", objEntity.OwnerLandline);
            cmdAdd.Parameters.AddWithValue("@InsuranceCompany", objEntity.InsuranceCompany);
            cmdAdd.Parameters.AddWithValue("@InsurancePolicyNo", objEntity.InsurancePolicyNo);
            cmdAdd.Parameters.AddWithValue("@InsuranceExpiry", objEntity.InsuranceExpiry);
            cmdAdd.Parameters.AddWithValue("@RatePerKM", objEntity.RatePerKM);
            cmdAdd.Parameters.AddWithValue("@Gross_Weight", objEntity.Gross_Weight);
            cmdAdd.Parameters.AddWithValue("@Tare_Weight", objEntity.Tare_Weight);
            cmdAdd.Parameters.AddWithValue("@Net_Weight", objEntity.Net_Weight);
            cmdAdd.Parameters.AddWithValue("@LicenseNo", objEntity.LicenseNo);

            cmdAdd.Parameters.AddWithValue("@TripDate", objEntity.TripDate);
            cmdAdd.Parameters.AddWithValue("@Driver", objEntity.DriverName);
            cmdAdd.Parameters.AddWithValue("@FromLocation", objEntity.From_Station);
            cmdAdd.Parameters.AddWithValue("@ToLocation", objEntity.To_Station);
            cmdAdd.Parameters.AddWithValue("@Kilometers", objEntity.Kilometers);

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            //cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();

            ForceCloseConncetion();
        }

        public virtual void DeleteTransporter(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Transporter_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }


        /* ------------------------------------------------------------------------------------- */

    }
}
