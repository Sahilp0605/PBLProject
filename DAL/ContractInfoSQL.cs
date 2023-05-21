using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ContractInfoSQL : BaseSqlManager
    {
        public DataTable GetContractProductDetail(string pInquiryNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT ip.pkID, ip.InquiryNo, ip.ProductID, CAST(it.ProductName AS NVARCHAR(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, it.Unit, ISNULL(ip.UnitPrice,0) AS 'UnitPrice', it.TaxRate, ip.Quantity From Contract_Product ip Inner Join MST_Product it On ip.ProductID = it.pkID Where ip.InquiryNo = '" + @pInquiryNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.ContractInfo> GetContractInfoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ContractInfo> lstLocation = new List<Entity.ContractInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ContractInfoList";
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
                Entity.ContractInfo objEntity = new Entity.ContractInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.ContractType = GetTextVale(dr, "ContractType");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.EndDate = GetDateTime(dr, "EndDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ContactPerson = GetTextVale(dr, "ContactPerson");
                objEntity.ContactNumber = GetTextVale(dr, "ContactNumber");

                objEntity.ContractFooter = GetTextVale(dr, "ContractFooter");
                objEntity.ContractTNC = GetTextVale(dr, "ContractTNC");

                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");

                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.RenewDays = GetInt64(dr, "RenewDays");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.ContractInfo> GetContractInfoList(Int64 pkID, string LoginUserID, string SearchKey, string pStatus, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ContractInfo> lstLocation = new List<Entity.ContractInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ContractInfoList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@Status", pStatus);
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
                Entity.ContractInfo objEntity = new Entity.ContractInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.ContractType = GetTextVale(dr, "ContractType");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.EndDate = GetDateTime(dr, "EndDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ContactPerson = GetTextVale(dr, "ContactPerson");
                objEntity.ContactNumber = GetTextVale(dr, "ContactNumber");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");

                objEntity.ContractFooter = GetTextVale(dr, "ContractFooter");
                objEntity.ContractTNC = GetTextVale(dr, "ContractTNC");

                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");
                objEntity.RenewDays = GetInt64(dr, "RenewDays");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.ContractInfo> GetContractInfoListByCustomer(Int64 pCustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ContractInfoListByCustomer";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustomerID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ContractInfo> lstObject = new List<Entity.ContractInfo>();
            while (dr.Read())
            {
                Entity.ContractInfo objEntity = new Entity.ContractInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.ContractType = GetTextVale(dr, "ContractType");
                objEntity.InquiryNoStatus = GetTextVale(dr, "InquiryNoStatus");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.EndDate = GetDateTime(dr, "EndDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ContactPerson = GetTextVale(dr, "ContactPerson");
                objEntity.ContactNumber = GetTextVale(dr, "ContactNumber");

                objEntity.ContractFooter = GetTextVale(dr, "ContractFooter");
                objEntity.ContractTNC = GetTextVale(dr, "ContractTNC");

                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.RenewDays = GetInt64(dr, "RenewDays");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateContractInfo(Entity.ContractInfo objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnInquiryNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ContractInfo_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@ContractType", objEntity.ContractType);
            cmdAdd.Parameters.AddWithValue("@StartDate", objEntity.StartDate);
            cmdAdd.Parameters.AddWithValue("@EndDate", objEntity.EndDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ContactPerson", objEntity.ContactPerson);
            cmdAdd.Parameters.AddWithValue("@ContactNumber", objEntity.ContactNumber);
            cmdAdd.Parameters.AddWithValue("@ContractFooter", objEntity.ContractFooter);
            cmdAdd.Parameters.AddWithValue("@ContractTNC", objEntity.ContractTNC);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnInquiryNo", SqlDbType.NVarChar, 30);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnInquiryNo = cmdAdd.Parameters["@ReturnInquiryNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteContractInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ContractInfo_DEL";
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

        public virtual List<Entity.ContractInfo> GetContractInfoProductGroupList(string pInquiryNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ContractInfoProductGroupList";
            cmdGet.Parameters.AddWithValue("@InquiryNo", pInquiryNo);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 100);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ContractInfo> lstObject = new List<Entity.ContractInfo>();
            while (dr.Read())
            {
                Entity.ContractInfo objEntity = new Entity.ContractInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateContractInfoProduct(Entity.ContractInfo objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ContractInfoProduct_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@UnitPrice", objEntity.UnitPrice);
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

        public virtual void DeleteContractInfoProductByInquiryNo(string InquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ContractInfoProductByInquiryNo_DEL";
            cmdDel.Parameters.AddWithValue("@InquiryNo", InquiryNo);
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

        public virtual void DeleteContractInfoProduct(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ContractInfoProduct_DEL";
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
    }
}
