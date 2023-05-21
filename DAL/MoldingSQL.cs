using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class MoldingSQL : BaseSqlManager
    {
        public virtual List<Entity.MoldingDetail> GetProductsByOrderNo(String OrderNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select ProductID,DBO.fnGetProductName(ProductID) As ProductName,Quantity , mp.ProductName + ' - ' + Convert(nvarchar(50),Quantity) As ProNameLong " +
                                    " from SalesOrder_Detail sod " +
                                    " Inner JOin MST_Product mp on sod.ProductID = mp.pkID " +
                                    " Where OrderNo = '" + OrderNo + "'";
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.MoldingDetail> lstLocation = new List<Entity.MoldingDetail>();
            while (dr.Read())
            {
                Entity.MoldingDetail objLocation = new Entity.MoldingDetail();
                objLocation.ProductID = GetInt64(dr, "ProductID");
                objLocation.ProductNameLong = GetTextVale(dr, "ProNameLong");

                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Molding> GetMoldingList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MoldingList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Molding> lstObject = new List<Entity.Molding>();
            while (dr.Read())
            {
                Entity.Molding objEntity = new Entity.Molding();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.MoldingNo = GetTextVale(dr, "MoldingNo");
                objEntity.MoldingDate = GetDateTime(dr, "MoldingDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.WorkType = GetTextVale(dr, "WorkType");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Molding> GetMoldingList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Molding> lstLocation = new List<Entity.Molding>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MoldingList";
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
                Entity.Molding objEntity = new Entity.Molding();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.MoldingNo = GetTextVale(dr, "MoldingNo");
                objEntity.MoldingDate = GetDateTime(dr, "MoldingDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.WorkType = GetTextVale(dr, "WorkType");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Molding> GetMoldingList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Molding> lstLocation = new List<Entity.Molding>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MoldingList";
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
                Entity.Molding objEntity = new Entity.Molding();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.MoldingNo = GetTextVale(dr, "MoldingNo");
                objEntity.MoldingDate = GetDateTime(dr, "MoldingDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.WorkType = GetTextVale(dr, "WorkType");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateMolding(Entity.Molding objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnMoldingNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Molding_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@MoldingNo", objEntity.MoldingNo);
            cmdAdd.Parameters.AddWithValue("@MoldingDate", objEntity.MoldingDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@WorkType", objEntity.WorkType);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnMoldingNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnMoldingNo = cmdAdd.Parameters["@ReturnMoldingNo"].Value.ToString();

            ForceCloseConncetion();
        }

        public virtual void DeleteMolding(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Molding_DEL";
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

        public virtual void AddUpdateMoldingDetail(Entity.MoldingDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "MoldingDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@MoldingNo", objEntity.MoldingNo);
            cmdAdd.Parameters.AddWithValue("@WorkType", objEntity.WorkType);
            cmdAdd.Parameters.AddWithValue("@WorkerName", objEntity.WorkerName);
            cmdAdd.Parameters.AddWithValue("@ClientId", objEntity.ClientID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Die", objEntity.Die);
            cmdAdd.Parameters.AddWithValue("@Cavity", objEntity.Cavity);
            cmdAdd.Parameters.AddWithValue("@DieNo", objEntity.DieNo);
            cmdAdd.Parameters.AddWithValue("@Material", objEntity.Material);
            cmdAdd.Parameters.AddWithValue("@Hardness", objEntity.Hardness);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
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
        public virtual void DeleteMoldingDetailByMoldingNo(string pMoldingNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "MoldingDetailByMoldingNo_DEL";
            cmdDel.Parameters.AddWithValue("@MoldingNo", pMoldingNo);
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

        public DataTable GetMoldingDetail(string MoldingNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = " SELECT qtd.pkID, qtd.MoldingNo, qtd.WorkType, qt.MoldingDate,qtd.ClientID,dbo.fnGetCustomerName(qtd.ClientID) As ClientName, " +
                                    " qtd.ProductID, item.ProductName, '[' + item.ProductAlias + '] - ' + item.ProductName As ProductNameLong, " +
                                    " item.ProductSpecification,  qtd.Quantity, qtd.Die, qtd.Cavity, qtd.DieNo, qtd.Material, qtd.Hardness, qtd.WorkerName, qtd.OrderNo,0 As RemQ " +
                                    " From Molding_Details qtd " +
                                    " Inner Join MST_Molding qt On qt.MoldingNo = qtd.MoldingNo" +
                                    " Inner Join MST_Product item On qtd.ProductID = item.pkID " +
                                    " Where qt.MoldingNo = '" + @MoldingNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetMoldingProducts(string OrderNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = " Select * from( " +
                                    " Select WorkType,WorkerName,OrderNo,ProductID,ProductName,ProductNameLong,0 as Die,0 As Cavity,Cast('' As nvarchar(50)) As DieNo,Cast('' As nvarchar(50)) As Material,Cast('' As nvarchar(50)) As Hardness,(ISNULL(SumQ,0) - ISNULL(IssuedQ,0)) As Quantity,(ISNULL(SumQ,0) - ISNULL(IssuedQ,0)) As RemQ  from ( " +
                                    " Select WorkType,Cast('' As nvarchar(50)) As WorkerName,OrderNo,ProductID ,ProductName, '[' + ProductAlias + '] - ' + ProductName As ProductNameLong , Sum(ISNULL(Quantity,0)) As SumQ, " +
                                    " (Select Sum(ISNULL(Quantity,0)) from Molding_Details Where ProductID = md.ProductID And OrderNo = md.OrderNo AND WorkType = 'finishing' Group BY ProductID) As IssuedQ " +
                                    " from Molding_Details md " +
                                    " Inner Join MST_Product mp on md.ProductID = mp.pkID Where WorkType = 'molding' And OrderNo = '" + OrderNo + "' " +
                                    " Group By WorkType,OrderNo,ProductID,ProductName,ProductAlias) As Temp ) As Temp1 Where RemQ > 0";
                                    //" Inner JOin MST_Product mp on md.ProductID = mp.pkID" +
                                    //" Where md.WorkType = 'molding' And md.OrderNo = '" + OrderNo + "' " +
                                    //" group by md.pkID,md.WorkType,WorkerName,md.OrderNo,ProductID,ProductName,ProductAlias,Die, Cavity) As Temp) As Temp Where RemQ > 0";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetSOProducts(string OrderNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = " Select * from( " +
                                    " Select ProductID,ProductName,ProductNameLong,OrderNo,WorkType,WorkerName,Die,Cavity,Cast('' As nvarchar(50)) As DieNo,Cast('' As nvarchar(50)) As Material,Cast('' As nvarchar(50)) As Hardness,(ISNULL(OrderQ,0) - ISNULL(UsedQ,0)) As RemQ,(ISNULL(OrderQ,0) - ISNULL(UsedQ,0)) As Quantity From(" +
                                    " Select ProductID,ProductName , '[' + ProductAlias + '] - ' + ProductName As ProductNameLong,sod.OrderNo ,Cast('' As nvarchar(50)) As WorkType," +
                                    " Cast('' As nvarchar(50)) As WorkerName,Cast(0 As Decimal(12,2)) AS Die,Cast(0 As Decimal(12,2)) AS Cavity,SUm(Quantity) As OrderQ," +
                                    " (Select sum(Quantity) from Molding_Details Where OrderNo = sod.OrderNo And ProductID = sod.ProductID ANd WorkType = 'molding') as UsedQ " +
                                    " from SalesOrder_Detail sod Inner join SalesOrder so on sod.OrderNo = So.OrderNo Inner Join MST_Product mp on sod.ProductID = mp.pkID" +
                                    " Where sod.OrderNo = '"+ OrderNo+ "' group by ProductID,ProductName,ProductAlias,sod.OrderNo) As Temp ) as Temp1 where Quantity <> 0 ";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
    }
}
