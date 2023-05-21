using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ProjectsSQL : BaseSqlManager
    {
        public virtual List<Entity.Projects> GetProjectsList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Projects> lstLocation = new List<Entity.Projects>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProjectsList";
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
                Entity.Projects objEntity = new Entity.Projects();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.ProjectDescription = GetTextVale(dr, "ProjectDescription");
                objEntity.StartDate = String.IsNullOrEmpty(dr["StartDate"].ToString()) ? (DateTime?)null : GetDateTime(dr, "StartDate");
                objEntity.DueDate = String.IsNullOrEmpty(dr["DueDate"].ToString()) ? (DateTime?)null : GetDateTime(dr, "DueDate");
                objEntity.CompletionDate = String.IsNullOrEmpty(dr["CompletionDate"].ToString()) ? (DateTime?)null : GetDateTime(dr, "CompletionDate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Projects> GetProjectsList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Projects> lstLocation = new List<Entity.Projects>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProjectsList";
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
                Entity.Projects objEntity = new Entity.Projects();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.ProjectDescription = GetTextVale(dr, "ProjectDescription");
                objEntity.StartDate = String.IsNullOrEmpty(dr["StartDate"].ToString()) ? (DateTime?)null : GetDateTime(dr, "StartDate");
                objEntity.DueDate = String.IsNullOrEmpty(dr["DueDate"].ToString()) ? (DateTime?)null : GetDateTime(dr, "DueDate");
                objEntity.CompletionDate = String.IsNullOrEmpty(dr["CompletionDate"].ToString()) ? (DateTime?)null : GetDateTime(dr, "CompletionDate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateProjects(Entity.Projects objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Projects_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ProjectName", objEntity.ProjectName);
            cmdAdd.Parameters.AddWithValue("@ProjectDescription", objEntity.ProjectDescription);
            cmdAdd.Parameters.AddWithValue("@StartDate", objEntity.StartDate);
            cmdAdd.Parameters.AddWithValue("@DueDate", objEntity.DueDate);
            cmdAdd.Parameters.AddWithValue("@CompletionDate", objEntity.CompletionDate);
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

        public virtual void DeleteProjects(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Projects_DEL";
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
    public class ProjectSheetSQL : BaseSqlManager
    {
        public virtual List<Entity.ProjectSheet> GetProjectSheetList(string LoginUserID, out int TotalRecord)
        {
            List<Entity.ProjectSheet> lstLocation = new List<Entity.ProjectSheet>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProjectSheetList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize",1000); ;
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProjectSheet objEntity = new Entity.ProjectSheet();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProjectSheetNo = GetTextVale(dr, "ProjectSheetNo");
                objEntity.ProjectSheetDate = GetDateTime(dr, "ProjectSheetDate");
                objEntity.SiteNo = GetTextVale(dr, "SiteNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.SiteArea = GetTextVale(dr, "SiteArea");
                objEntity.SiteCityID = GetInt64(dr, "SiteCityID");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.SiteStateID = GetInt64(dr, "SiteStateID");
                objEntity.StateName= GetTextVale(dr, "StateName");
                objEntity.SiteCountryID = GetTextVale(dr, "SiteCountryID");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.SitePincode = GetTextVale(dr, "SitePincode");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.ProjectSheet> GetProjectSheetList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ProjectSheet> lstLocation = new List<Entity.ProjectSheet>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProjectSheetList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize); ;
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProjectSheet objEntity = new Entity.ProjectSheet();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProjectSheetNo = GetTextVale(dr, "ProjectSheetNo");
                objEntity.ProjectSheetDate = GetDateTime(dr, "ProjectSheetDate");
                objEntity.SiteNo = GetTextVale(dr, "SiteNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.SiteArea = GetTextVale(dr, "SiteArea");
                objEntity.SiteCityID = GetInt64(dr, "SiteCityID");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.SiteStateID = GetInt64(dr, "SiteStateID");
                objEntity.StateName= GetTextVale(dr, "StateName");
                objEntity.SiteCountryID = GetTextVale(dr, "SiteCountryID");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.SitePincode = GetTextVale(dr, "SitePincode");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.ProjectSheet> GetProjectSheetList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ProjectSheet> lstLocation = new List<Entity.ProjectSheet>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProjectSheetList";
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
                Entity.ProjectSheet objEntity = new Entity.ProjectSheet();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProjectSheetNo = GetTextVale(dr, "ProjectSheetNo");
                objEntity.ProjectSheetDate = GetDateTime(dr, "ProjectSheetDate");
                objEntity.SiteNo = GetTextVale(dr, "SiteNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.SiteArea = GetTextVale(dr, "SiteArea");
                objEntity.SiteCityID = GetInt64(dr, "SiteCityID");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.SiteStateID = GetInt64(dr, "SiteStateID");
                objEntity.StateName= GetTextVale(dr, "StateName");
                objEntity.SiteCountryID = GetTextVale(dr, "SiteCountryID");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.SitePincode = GetTextVale(dr, "SitePincode");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void AddUpdateProjectSheet(Entity.ProjectSheet objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnProjectNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ProjectSheet_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ProjectSheetNo", objEntity.ProjectSheetNo);
            cmdAdd.Parameters.AddWithValue("@ProjectSheetDate", objEntity.ProjectSheetDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@SiteNo", objEntity.SiteNo);
            cmdAdd.Parameters.AddWithValue("@SiteAddress", objEntity.SiteAddress);
            cmdAdd.Parameters.AddWithValue("@SiteArea", objEntity.SiteArea);
            cmdAdd.Parameters.AddWithValue("@SiteCityID", objEntity.SiteCityID);
            cmdAdd.Parameters.AddWithValue("@SiteStateID", objEntity.SiteStateID);
            cmdAdd.Parameters.AddWithValue("@SiteCountryID", objEntity.SiteCountryID);
            cmdAdd.Parameters.AddWithValue("@SitePincode", objEntity.SitePincode);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnProjectNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnProjectNo = cmdAdd.Parameters["@ReturnProjectNo"].Value.ToString();
            ForceCloseConncetion();
        }
        public virtual void DeleteProjectSheet(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProjectSheet_DEL";
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
        //-----------------------------------------------------------------------------------------------
        public virtual void AddUpdateProject_Detail(Entity.Project_Detail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ProjectDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ProjectSheetNo", objEntity.ProjectSheetNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@SysCapacity", objEntity.SysCapacity);
            cmdAdd.Parameters.AddWithValue("@PanalWattage", objEntity.PanalWattage);
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
        public DataTable GetProjectProductDetail(string pProjectNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT ip.pkID,ip.ProjectSheetNo ,ip.ProductID, CAST(it.ProductName AS NVARCHAR(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, ip.Unit,ip.PanalWattage,ip.SysCapacity From Project_Detail ip Inner Join MST_Product it On ip.ProductID = it.pkID Where ip.ProjectSheetNo = '" + @pProjectNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        public virtual void DeleteProjectDetailsBySheetNo(string pInwardNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "DeleteProductByProjectNo";
            cmdDel.Parameters.AddWithValue("@ProjectSheetNo", pInwardNo);
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

        public virtual void AddUpdateProject_Assembly(Entity.ProjectAssembly objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ProjectAssembly_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ProjectSheetNo", objEntity.ProjectSheetNo);
            cmdAdd.Parameters.AddWithValue("@FinishedProductID", objEntity.FinishedProductID);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@ProductMake", objEntity.ProductMake);
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

        public DataTable GetProjectAssemblyDetail(string pProjectNo, Int64 FinishedProID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = " Select pa.pkID,FinishedProductID As FinishProductID,dbo.fnGetProductName(FinishedProductID) As FinishProductName, " +
                                    " dbo.fnGetProductName(FinishedProductID) As FinishProductNameLong, " +
                                    " ProductID,dbo.fnGetProductName(ProductID) As ProductName," +
                                    " Quantity, Unit, Remarks, ProductMake As BrandID, BrandName "+
                                    " from Project_Assembly pa Left Join MST_Brand mb on pa.ProductMake = mb.pkID"+
                                    " Where ProjectSheetNo = '" + @pProjectNo + "' And FinishedProductID = " + FinishedProID;
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        public virtual void DeleteAssemblyByProjectNo(string pProjectSheetNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "DeleteAssemblyByProjectNo";
            cmdDel.Parameters.AddWithValue("@ProjectSheetNo", pProjectSheetNo);
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
