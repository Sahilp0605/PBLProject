using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ExpenseSQL : BaseSqlManager
    {
        public virtual List<Entity.Expense> GetExpenseList(string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExpenseList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Expense> lstObject = new List<Entity.Expense>();
            while (dr.Read())
            {
                Entity.Expense objEntity = new Entity.Expense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseDate = GetDateTime(dr, "ExpenseDate");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ExpenseTypeId = GetInt64(dr, "ExpenseTypeId");
                objEntity.ExpenseTypeName = GetTextVale(dr, "ExpenseTypeName");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.ExpenseNotes = GetTextVale(dr, "ExpenseNotes");
                //objEntity.ExpenseImage = GetTextVale(dr, "ExpenseImage");
                objEntity.FromLocation = GetTextVale(dr, "FromLocation");
                objEntity.ToLocation = GetTextVale(dr, "ToLocation");
                objEntity.DistanceCovered = GetDecimal(dr, "DistanceCovered");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Expense> GetExpense(Int64 pkID, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExpenseList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Expense> lstObject = new List<Entity.Expense>();
            while (dr.Read())
            {
                Entity.Expense objEntity = new Entity.Expense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseDate = GetDateTime(dr, "ExpenseDate");
                objEntity.ExpenseTypeId = GetInt64(dr, "ExpenseTypeId");
                objEntity.ExpenseTypeName = GetTextVale(dr, "ExpenseTypeName");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.ExpenseNotes = GetTextVale(dr, "ExpenseNotes");
                //objEntity.ExpenseImage = GetTextVale(dr, "ExpenseImage");
                objEntity.FromLocation = GetTextVale(dr, "FromLocation");
                objEntity.ToLocation = GetTextVale(dr, "ToLocation");
                objEntity.DistanceCovered = GetDecimal(dr, "DistanceCovered");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Expense> GetExpenseList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExpenseList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);

            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Expense> lstObject = new List<Entity.Expense>();
            while (dr.Read())
            {
                Entity.Expense objEntity = new Entity.Expense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseDate = GetDateTime(dr, "ExpenseDate");
                objEntity.ExpenseTypeId = GetInt64(dr, "ExpenseTypeId");
                objEntity.ExpenseTypeName = GetTextVale(dr, "ExpenseTypeName");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.ExpenseNotes = GetTextVale(dr, "ExpenseNotes");
                //objEntity.ExpenseImage = GetTextVale(dr, "ExpenseImage");
                objEntity.FromLocation = GetTextVale(dr, "FromLocation");
                objEntity.ToLocation = GetTextVale(dr, "ToLocation");
                objEntity.DistanceCovered = GetDecimal(dr, "DistanceCovered");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateExpense(Entity.Expense objEntity, out int ReturnCode, out string ReturnMsg, out int ReturnExpenseId)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Expense_INS_UPD";
            cmdAdd.Parameters.AddWithValue("pkID",objEntity.pkID); 
            cmdAdd.Parameters.AddWithValue("ExpenseDate",objEntity.ExpenseDate);
            cmdAdd.Parameters.AddWithValue("CustomerName", objEntity.CustomerName);
            cmdAdd.Parameters.AddWithValue("ExpenseTypeId",objEntity.ExpenseTypeId); 
            cmdAdd.Parameters.AddWithValue("Amount",objEntity.Amount );
            cmdAdd.Parameters.AddWithValue("ExpenseNotes",objEntity.ExpenseNotes); 
            cmdAdd.Parameters.AddWithValue("FromLocation",objEntity.FromLocation );
            cmdAdd.Parameters.AddWithValue("ToLocation",objEntity.ToLocation );
            cmdAdd.Parameters.AddWithValue("DistanceCovered", objEntity.DistanceCovered);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnExpenseId", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnExpenseId = Convert.ToInt32(cmdAdd.Parameters["@ReturnExpenseId"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteExpense(Int64 ChargeId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Expense_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", ChargeId);
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
        public virtual List<Entity.Expense> GetExpenseImageList(Int64 pkID, Int64 ExpenseID)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExpenseImageList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ExpenseID", ExpenseID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Expense> lstLocation = new List<Entity.Expense>();
            while (dr.Read())
            {
                Entity.Expense objLocation = new Entity.Expense();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.ExpenseID = GetInt64(dr, "ExpenseID");
                objLocation.Name = GetTextVale(dr, "Name");
                objLocation.Type = GetTextVale(dr, "Type");
                //objLocation.FileData = GetBase64(dr, "Data");
               
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateExpenseImages(Entity.Expense objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ExpenseImage_INS_UPD";
            cmdAdd.Parameters.Add("@ExpenseID", objEntity.ExpenseID);
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

        public virtual void DeleteExpenseImage(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ExpenseImage_DEL";
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
        public virtual void DeleteExpenseImageByExpenseID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ExpenseImageByExpenseID_DEL";
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
        public virtual List<Entity.OfficeExpense> GetMultiExpenseList(string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OfficeExpenseList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OfficeExpense> lstObject = new List<Entity.OfficeExpense>();
            while (dr.Read())
            {
                Entity.OfficeExpense objEntity = new Entity.OfficeExpense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseDate = GetDateTime(dr, "ExpenseDate");
                objEntity.ExpenseNotes = GetTextVale(dr, "ExpenseNotes");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OfficeExpense> GetMultiExpenseList(Int64 pkID, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OfficeExpenseList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OfficeExpense> lstObject = new List<Entity.OfficeExpense>();
            while (dr.Read())
            {
                Entity.OfficeExpense objEntity = new Entity.OfficeExpense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseDate = GetDateTime(dr, "ExpenseDate");
                objEntity.ExpenseNotes = GetTextVale(dr, "ExpenseNotes");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OfficeExpense> GetMultiExpenseList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OfficeExpenseList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OfficeExpense> lstObject = new List<Entity.OfficeExpense>();
            while (dr.Read())
            {
                Entity.OfficeExpense objEntity = new Entity.OfficeExpense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseDate = GetDateTime(dr, "ExpenseDate");
                objEntity.ExpenseNotes = GetTextVale(dr, "ExpenseNotes");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateMultiExpense(Entity.OfficeExpense objEntity, out int ReturnCode, out string ReturnMsg, out int ReturnExpenseId)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OfficeExpense_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ExpenseDate", objEntity.ExpenseDate);
            cmdAdd.Parameters.AddWithValue("@ExpenseNotes", objEntity.ExpenseNotes);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnExpenseId", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnExpenseId = Convert.ToInt32(cmdAdd.Parameters["@ReturnExpenseId"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteMultiExpense(Int64 ChargeId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OfficeExpense_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", ChargeId);
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
        public virtual List<Entity.OfficeExpense> GetMultiExpenseDetailList(string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OfficeExpenseDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OfficeExpense> lstObject = new List<Entity.OfficeExpense>();
            while (dr.Read())
            {
                Entity.OfficeExpense objEntity = new Entity.OfficeExpense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RefpkID = GetInt64(dr, "RefpkID");
                objEntity.ExpenseTypeId = GetInt64(dr, "ExpenseTypeId");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.Voucher = (!String.IsNullOrEmpty(GetTextVale(dr, "Voucher"))) ? GetTextVale(dr, "Voucher") : "images/no-figure.png";
                objEntity.ExpenseTypeName = GetTextVale(dr, "ExpenseTypeName");
                
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OfficeExpense> GetMultiExpenseDetailList(Int64 pkID, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OfficeExpenseDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OfficeExpense> lstObject = new List<Entity.OfficeExpense>();
            while (dr.Read())
            {
                Entity.OfficeExpense objEntity = new Entity.OfficeExpense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RefpkID = GetInt64(dr, "RefpkID");
                objEntity.ExpenseTypeId = GetInt64(dr, "ExpenseTypeId");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.Voucher = (!String.IsNullOrEmpty(GetTextVale(dr, "Voucher"))) ? GetTextVale(dr, "Voucher") : "images/no-figure.png";
                objEntity.ExpenseTypeName = GetTextVale(dr, "ExpenseTypeName");
                objEntity.ExpenseDateDetail = GetDateTime(dr, "ExpenseDateDetail");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OfficeExpense> GetMultiExpenseDetailList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OfficeExpenseDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OfficeExpense> lstObject = new List<Entity.OfficeExpense>();
            while (dr.Read())
            {
                Entity.OfficeExpense objEntity = new Entity.OfficeExpense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RefpkID = GetInt64(dr, "RefpkID");
                objEntity.ExpenseTypeId = GetInt64(dr, "ExpenseTypeId");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.Voucher = (!String.IsNullOrEmpty(GetTextVale(dr, "Voucher"))) ? GetTextVale(dr, "Voucher") : "images/no-figure.png";
                objEntity.ExpenseTypeName = GetTextVale(dr, "ExpenseTypeName");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateMultiExpenseDetail(Entity.OfficeExpense objEntity, out int ReturnCode, out string ReturnMsg,out Int64 RetExpDetailId)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OfficeExpense_Detail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@RefpkID", objEntity.RefpkID);
            cmdAdd.Parameters.AddWithValue("@ExpenseTypeId", objEntity.ExpenseTypeId);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@Voucher", objEntity.Voucher);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            cmdAdd.Parameters.AddWithValue("@ExpenseDateDetail", objEntity.ExpenseDateDetail);

            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@RetExpDetailId", SqlDbType.BigInt, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            RetExpDetailId = Convert.ToInt64(cmdAdd.Parameters["@RetExpDetailId"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteMultiExpenseDetailByExpenseNo(Int64 ChargeId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OfficeExpenseDetailByExpenseNo_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", ChargeId);
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
