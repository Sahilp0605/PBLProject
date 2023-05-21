using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class CourierInfoSQL : BaseSqlManager
    {
        public virtual List<Entity.CourierInfo> GetCourierInfoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.CourierInfo> lstLocation = new List<Entity.CourierInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CourierInfoList";
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
                Entity.CourierInfo objEntity = new Entity.CourierInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.SerialNo = GetTextVale(dr, "SerialNo");
                objEntity.DocketNo = GetTextVale(dr, "DocketNo");
                objEntity.ActivityDate = GetDateTime(dr, "ActivityDate");
                objEntity.DocumentType = GetTextVale(dr, "DocumentType");
                objEntity.AcceptanceType = GetTextVale(dr, "AcceptanceType");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CourierContact = GetTextVale(dr, "CourierContact");
                objEntity.CourierEmail = GetTextVale(dr, "CourierEmail");
                objEntity.CourierName = GetTextVale(dr, "CourierName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "PinCode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.Country = GetTextVale(dr, "Country");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.CourierImage = GetTextVale(dr, "CourierImage");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.CourierInfo> GetCourierInfoList(Int64 pkID, string LoginUserID, string SearchKey, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.CourierInfo> lstLocation = new List<Entity.CourierInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CourierInfoList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.CourierInfo objEntity = new Entity.CourierInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.SerialNo = GetTextVale(dr, "SerialNo");
                objEntity.DocketNo = GetTextVale(dr, "DocketNo");
                objEntity.ActivityDate = GetDateTime(dr, "ActivityDate");
                objEntity.DocumentType = GetTextVale(dr, "DocumentType");
                objEntity.AcceptanceType = GetTextVale(dr, "AcceptanceType");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CourierContact = GetTextVale(dr, "CourierContact");
                objEntity.CourierEmail = GetTextVale(dr, "CourierEmail");
                objEntity.CourierName = GetTextVale(dr, "CourierName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "PinCode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.Country = GetTextVale(dr, "Country");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.CourierImage= GetTextVale(dr, "CourierImage");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateCourierInfo(Entity.CourierInfo objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnSerialNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "CourierInfo_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@SerialNo", objEntity.SerialNo);
            cmdAdd.Parameters.AddWithValue("@DocketNo", objEntity.DocketNo);
            cmdAdd.Parameters.AddWithValue("@ActivityDate", objEntity.ActivityDate);
            cmdAdd.Parameters.AddWithValue("@DocumentType", objEntity.DocumentType);
            cmdAdd.Parameters.AddWithValue("@AcceptanceType", objEntity.AcceptanceType);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@CourierContact", objEntity.CourierContact);
            cmdAdd.Parameters.AddWithValue("@CourierEmail", objEntity.CourierEmail);
            cmdAdd.Parameters.AddWithValue("@CourierName", objEntity.CourierName);
            cmdAdd.Parameters.AddWithValue("@Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("@City", objEntity.City);
            cmdAdd.Parameters.AddWithValue("@PinCode", objEntity.PinCode);
            cmdAdd.Parameters.AddWithValue("@State", objEntity.State);
            cmdAdd.Parameters.AddWithValue("@Country", objEntity.Country);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@CourierImage", objEntity.CourierImage);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnSerialNo", SqlDbType.NVarChar, 30);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnSerialNo = cmdAdd.Parameters["@ReturnSerialNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteCourierInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CourierInfo_DEL";
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

        /*-------------------------------------------------------------------------*/
        public virtual List<Entity.CourierInfo> GetCourierImageList(Int64 pkID, String CourierNo)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CourierImageList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@CourierNo", CourierNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CourierInfo> lstLocation = new List<Entity.CourierInfo>();
            while (dr.Read())
            {
                Entity.CourierInfo objLocation = new Entity.CourierInfo();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.CourierNo = GetTextVale(dr, "CourierNo");
                objLocation.Name = GetTextVale(dr, "Name");
                objLocation.Type = GetTextVale(dr, "Type");
                //objLocation.FileData = GetBase64(dr, "Data");

                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateCourierImages(Entity.CourierInfo objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "CourierImages_INS_UPD";
            cmdAdd.Parameters.Add("@CourierNo", objEntity.CourierNo);
            cmdAdd.Parameters.Add("@Name", objEntity.Name);
            cmdAdd.Parameters.Add("@Type", objEntity.Type);
            cmdAdd.Parameters.Add("@LoginUserID", objEntity.LoginUserID);
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

        public virtual void DeleteCourierImage(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CourierImages_DEL";
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
        public virtual void DeleteCourierImageByCourierID(String CourierNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CourierImagesByCourierID_DEL";
            cmdDel.Parameters.AddWithValue("@CourierNo", CourierNo);
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
        /*-------------------------------------------------------------------------*/
    }
}
