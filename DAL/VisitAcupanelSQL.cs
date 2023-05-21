using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class VisitAcupanelSQL : BaseSqlManager
    {
        //====================================Complaint Visit Acupanel detail=========================================================
        public DataTable GetComplaintVisitDetail(Int64 ComplaintNo,Int64 ParentID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select vad.pkID, vad.ComplaintNo, vad.NewRep, vad.ProductName, vad.SrNo, vad.ReplaceProduct, vad.NewSrNo, vad.Remarks from VisitAcupanel_Detail vad Where vad.ComplaintNo =" + ComplaintNo + " AND ParentID = " + ParentID;
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.VisitAcupanel> GetComplaintVisitDetailList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.VisitAcupanel> lstLocation = new List<Entity.VisitAcupanel>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "VisitAcupanelDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.VisitAcupanel objEntity = new Entity.VisitAcupanel();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ComplaintNo = GetInt64(dr, "ComplaintNo");
                objEntity.NewRep = GetTextVale(dr, "NewRep");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.SrNo = GetTextVale(dr, "SrNo");
                objEntity.ReplaceProduct = GetTextVale(dr, "ReplaceProduct");
                objEntity.NewSrNo = GetTextVale(dr, "NewSrNo");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateComplaintVisitDetail(Entity.VisitAcupanel objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "VisitAcupanelDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ComplaintNo", objEntity.ComplaintNo);
            cmdAdd.Parameters.AddWithValue("@ParentID", objEntity.ParentID);
            cmdAdd.Parameters.AddWithValue("@NewRep", objEntity.NewRep);
            cmdAdd.Parameters.AddWithValue("@ProductName", objEntity.ProductName);
            cmdAdd.Parameters.AddWithValue("@SrNo", objEntity.SrNo);
            cmdAdd.Parameters.AddWithValue("@ReplaceProduct", objEntity.ReplaceProduct);
            cmdAdd.Parameters.AddWithValue("@NewSrNo", objEntity.NewSrNo);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);

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

        public virtual void DeleteComplaintVisitDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "VisitAcupanelDetail_DEL";
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

        public virtual void DeleteComplaintVisitDetailByComplaintNo(Int64 ComplaintNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "VisitAcupanelDetailByComplaintNo_DEL";
            cmdDel.Parameters.AddWithValue("@ComplaintNo", ComplaintNo);
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
