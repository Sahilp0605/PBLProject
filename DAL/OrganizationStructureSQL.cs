using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OrganizationStructureSQL: BaseSqlManager
    {
        public virtual List<Entity.OrganizationStructure> GetEmployeeLocation(Int64 pEmployeeID, DateTime pStartDate, DateTime pEndDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetEmployeeLocation";
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmployeeID);
            cmdGet.Parameters.AddWithValue("@StartDate", pStartDate);
            cmdGet.Parameters.AddWithValue("@EndDate", pEndDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationStructure> lstObject = new List<Entity.OrganizationStructure>();
            while (dr.Read())
            {
                Entity.OrganizationStructure objEntity = new Entity.OrganizationStructure();
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.FollowUpDate = GetDateTime(dr, "FollowUpDate");
                objEntity.Latitude = GetDecimal(dr, "Latitude");
                objEntity.Longitude = GetDecimal(dr, "Longitude");
                
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationStructure> GetOrganizationStructureList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationStructureList";
            cmdGet.Parameters.AddWithValue("@OrgCode", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 1000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationStructure> lstObject = new List<Entity.OrganizationStructure>();
            while (dr.Read())
            {
                Entity.OrganizationStructure objEntity = new Entity.OrganizationStructure();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");
                objEntity.OrgType = GetTextVale(dr, "OrgType");
                objEntity.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.Landline1 = GetTextVale(dr, "Landline1");
                objEntity.Landline2 = GetTextVale(dr, "Landline2");
                objEntity.Fax1 = GetTextVale(dr, "Fax1");
                objEntity.Fax2 = GetTextVale(dr, "Fax2");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ReportTo_OrgCode = GetTextVale(dr, "ReportTo_OrgCode");
                objEntity.ReportTo_OrgName = GetTextVale(dr, "ReportTo_OrgName");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.GSTIN = GetTextVale(dr, "GSTIN");
                objEntity.PANNO = GetTextVale(dr, "PANNO");
                objEntity.OrgHead = GetInt64(dr, "OrgHead");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationStructure> GetOrganizationStructure(string OrgCode, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrganizationStructure> lstLocation = new List<Entity.OrganizationStructure>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationStructureList";
            cmdGet.Parameters.AddWithValue("@OrgCode", OrgCode);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);   
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OrganizationStructure objEntity = new Entity.OrganizationStructure();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");
                objEntity.OrgType = GetTextVale(dr, "OrgType");
                objEntity.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objEntity.GSTIN = GetTextVale(dr, "GSTIN");
                objEntity.PANNO = GetTextVale(dr, "PANNO");
                objEntity.CINNO = GetTextVale(dr, "CINNO");

                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.Landline1 = GetTextVale(dr, "Landline1");
                objEntity.Landline2 = GetTextVale(dr, "Landline2");
                objEntity.Fax1 = GetTextVale(dr, "Fax1");
                objEntity.Fax2 = GetTextVale(dr, "Fax2");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ReportTo_OrgCode = GetTextVale(dr, "ReportTo_OrgCode");
                objEntity.ReportTo_OrgName = GetTextVale(dr, "ReportTo_OrgName");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.GSTStateCode = GetTextVale(dr, "GSTStateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.OrgHead = GetInt64(dr, "OrgHead");
                objEntity.OrgHeadName = GetTextVale(dr, "OrgHeadName");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.OrganizationStructure> GetOrganizationStructure(string OrgCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrganizationStructure> lstLocation = new List<Entity.OrganizationStructure>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationStructureList";
            cmdGet.Parameters.AddWithValue("@OrgCode", OrgCode);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
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
                Entity.OrganizationStructure objEntity = new Entity.OrganizationStructure();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");
                objEntity.OrgType = GetTextVale(dr, "OrgType");
                objEntity.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objEntity.GSTIN = GetTextVale(dr, "GSTIN");
                objEntity.PANNO = GetTextVale(dr, "PANNO");

                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.Landline1 = GetTextVale(dr, "Landline1");
                objEntity.Landline2 = GetTextVale(dr, "Landline2");
                objEntity.Fax1 = GetTextVale(dr, "Fax1");
                objEntity.Fax2 = GetTextVale(dr, "Fax2");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ReportTo_OrgCode = GetTextVale(dr, "ReportTo_OrgCode");
                objEntity.ReportTo_OrgName = GetTextVale(dr, "ReportTo_OrgName");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.OrgHead = GetInt64(dr, "OrgHead");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.OrganizationStructure> GetOrganizationStructureDropDownList(string pListMode, string pLoginUserID)
        {
            List<Entity.OrganizationStructure> lstLocation = new List<Entity.OrganizationStructure>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationStructureList";
            cmdGet.Parameters.AddWithValue("@OrgCode", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 5000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OrganizationStructure objEntity = new Entity.OrganizationStructure();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateOrganizationStructure(Entity.OrganizationStructure objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OrganizationStructure_INS_UPD";

            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrgCode", objEntity.OrgCode);
            cmdAdd.Parameters.AddWithValue("@OrgName", objEntity.OrgName);
            cmdAdd.Parameters.AddWithValue("@OrgTypeCode", objEntity.OrgTypeCode);
            cmdAdd.Parameters.AddWithValue("@Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);
            cmdAdd.Parameters.AddWithValue("@Pincode", objEntity.Pincode);
            cmdAdd.Parameters.AddWithValue("@Landline1", objEntity.Landline1);
            cmdAdd.Parameters.AddWithValue("@Fax1", objEntity.Fax1);
            cmdAdd.Parameters.AddWithValue("@EmailAddress", objEntity.EmailAddress);
            cmdAdd.Parameters.AddWithValue("@ReportTo_OrgCode", objEntity.ReportTo_OrgCode);
            cmdAdd.Parameters.AddWithValue("@ActiveFlag", objEntity.ActiveFlag);
            cmdAdd.Parameters.AddWithValue("@OrgHead", objEntity.OrgHead);
            cmdAdd.Parameters.AddWithValue("@GSTIN", objEntity.GSTIN);
            cmdAdd.Parameters.AddWithValue("@PANNO", objEntity.PANNO);
            cmdAdd.Parameters.AddWithValue("@CINNO", objEntity.CINNO);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);

            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteOrganizationStructure(string OrgCode, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OrganizationStructure_DEL";
           
            cmdDel.Parameters.AddWithValue("@OrgCode", OrgCode);
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

        public virtual List<Entity.OrganizationBank> GetOrganizationBankList(Int64 pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrganizationBank> lstLocation = new List<Entity.OrganizationBank>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationBankList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OrganizationBank objEntity = new Entity.OrganizationBank();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                //objEntity.OrgName = GetTextVale(dr, "OrgName");
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                //objEntity.GSTNo = GetTextVale(dr, "GSTNo");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.OrganizationBank> GetOrganizationBankListByCompID(Int64 CompanyID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrganizationBank> lstLocation = new List<Entity.OrganizationBank>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationBankListByCompID";
            cmdGet.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OrganizationBank objEntity = new Entity.OrganizationBank();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CompanyID = GetTextVale(dr, "CompanyID");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSWIFT = GetTextVale(dr, "BankSWIFT");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
    }
}
