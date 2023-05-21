using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class EmailTemplateSQL : BaseSqlManager
    {
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Email Template 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual List<Entity.EmailTemplate> GetEmailTemplateList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmailTemplateList";
            cmdGet.Parameters.AddWithValue("@TemplateID", "");
            cmdGet.Parameters.AddWithValue("@Category", "");
            //cmdGet.Parameters.AddWithValue("@Category", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);  
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.EmailTemplate> lstObject = new List<Entity.EmailTemplate>();
            while (dr.Read())
            {
                Entity.EmailTemplate objEntity = new Entity.EmailTemplate();
                objEntity.TemplateID = GetTextVale(dr, "TemplateID");
                objEntity.Category = GetTextVale(dr, "Category");
                objEntity.Description = GetTextVale(dr, "Description");
                objEntity.Subject = GetTextVale(dr, "Subject");
                objEntity.ContentData = GetTextVale(dr, "ContentData");
                objEntity.ContentDataSMS = GetTextVale(dr, "ContentDataSMS");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                objEntity.ProductAttachment = GetBoolean(dr, "ProductAttachment");
                objEntity.ImageAttachment1 = GetTextVale(dr, "ImageAttachment1");
                objEntity.ImageAttachment2 = GetTextVale(dr, "ImageAttachment2");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.EmailTemplate> GetEmailTemplate(string TemplateID, string Category, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.EmailTemplate> lstLocation = new List<Entity.EmailTemplate>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmailTemplateList";
            cmdGet.Parameters.AddWithValue("@TemplateID", TemplateID);
            cmdGet.Parameters.AddWithValue("@Category", Category);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);  
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.EmailTemplate objEntity = new Entity.EmailTemplate();
                objEntity.TemplateID = GetTextVale(dr, "TemplateID");
                objEntity.Category = GetTextVale(dr, "Category");
                objEntity.Description = GetTextVale(dr, "Description");
                objEntity.Subject = GetTextVale(dr, "Subject");
                objEntity.ContentData = GetTextVale(dr, "ContentData");
                objEntity.ContentDataSMS = GetTextVale(dr, "ContentDataSMS");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                objEntity.ProductAttachment = GetBoolean(dr, "ProductAttachment");
                objEntity.ImageAttachment1 = GetTextVale(dr, "ImageAttachment1");
                objEntity.ImageAttachment2 = GetTextVale(dr, "ImageAttachment2");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateEmailTemplate(Entity.EmailTemplate objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "EmailTemplate_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@TemplateID", objEntity.TemplateID);
            cmdAdd.Parameters.AddWithValue("@Category", objEntity.Category);
            cmdAdd.Parameters.AddWithValue("@Description", objEntity.Description);
            cmdAdd.Parameters.AddWithValue("@Subject", objEntity.Subject);
            cmdAdd.Parameters.AddWithValue("@ContentData", objEntity.ContentData);
            cmdAdd.Parameters.AddWithValue("@ImageAttachment1", objEntity.ImageAttachment1);
            cmdAdd.Parameters.AddWithValue("@ImageAttachment2", objEntity.ImageAttachment2);
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


        public virtual void AddUpdateCampaignLog(Entity.CampaignLog objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "CampaignLog_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@CampaignID", objEntity.CampaignID);
            cmdAdd.Parameters.AddWithValue("@CampaignCategory", objEntity.CampaignCategory);
            cmdAdd.Parameters.AddWithValue("@CampaignFor", objEntity.CampaignFor);
            cmdAdd.Parameters.AddWithValue("@CampaignContact", objEntity.CampaignContact);
            cmdAdd.Parameters.AddWithValue("@CampaignStatus", objEntity.CampaignStatus);
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


        public virtual List<Entity.Customer> GetCampaignCustomerList(string Category,string CustCategory)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CampaignCustomerList";
            cmdGet.Parameters.AddWithValue("@Category", Category);
            cmdGet.Parameters.AddWithValue("@CustCategory", CustCategory);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");                
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.CampaignSentOn = GetTextVale(dr, "CampaignSentOn");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetCampaignEmployeeList(string Category, string empDesignation)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CampaignEmployeeList";
            cmdGet.Parameters.AddWithValue("@Category", Category);
            cmdGet.Parameters.AddWithValue("@empDesignation", empDesignation);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.CampaignSentOn = GetTextVale(dr, "CampaignSentOn");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }



        public virtual void DeleteEmailTemplate(string TemplateID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "EmailTemplate_DEL";
            cmdDel.Parameters.AddWithValue("@TemplateID", TemplateID);
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

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Campaign Template 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual List<Entity.CampaignTemplate> GetCampaignList(Int64 CampaignID, String CampaignCategory, String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CampaignList";
            cmdGet.Parameters.AddWithValue("@CampaignID", CampaignID);
            cmdGet.Parameters.AddWithValue("@CampaignCategory", CampaignCategory);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CampaignTemplate> lstObject = new List<Entity.CampaignTemplate>();
            while (dr.Read())
            {
                Entity.CampaignTemplate objEntity = new Entity.CampaignTemplate();
                objEntity.CampaignID = GetInt64(dr, "CampaignID");
                objEntity.CampaignCategory = GetTextVale(dr, "CampaignCategory");
                objEntity.CampaignSubject = GetTextVale(dr, "CampaignSubject");
                objEntity.CampaignHeader = GetTextVale(dr, "CampaignHeader");
                objEntity.CampaignFooter = GetTextVale(dr, "CampaignFooter");
                objEntity.CampaignImageUrl = GetTextVale(dr, "CampaignImageUrl");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // General Template 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

        public virtual List<Entity.EmailTemplate> GetGeneralTemplate()
        {
            List<Entity.EmailTemplate> lstLocation = new List<Entity.EmailTemplate>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GeneralTemplateList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.EmailTemplate objEntity = new Entity.EmailTemplate();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Subject = GetTextVale(dr, "Subject");
                objEntity.ContentData = GetTextVale(dr, "ContentData");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            //TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.EmailTemplate> GetGeneralTemplate(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.EmailTemplate> lstLocation = new List<Entity.EmailTemplate>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GeneralTemplateList";
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
                Entity.EmailTemplate objEntity = new Entity.EmailTemplate();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Subject = GetTextVale(dr, "Subject");
                objEntity.ContentData = GetTextVale(dr, "ContentData");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.EmailTemplate> GetGeneralTemplate(Int64 pkID, string LoginUserID,string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.EmailTemplate> lstLocation = new List<Entity.EmailTemplate>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GeneralTemplateList";
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
                Entity.EmailTemplate objEntity = new Entity.EmailTemplate();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Subject = GetTextVale(dr, "Subject");
                objEntity.ContentData = GetTextVale(dr, "ContentData");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateGeneralTemplate(Entity.EmailTemplate objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "GeneralTemplate_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@Subject", objEntity.Subject);
            cmdAdd.Parameters.AddWithValue("@ContentData", objEntity.ContentData);
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
        public virtual void DeleteGeneralTemplate(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "GeneralTemplate_DEL";
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
