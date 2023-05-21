using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CustomerContactsSQL:BaseSqlManager
    {
        // ----------------------------------------------------------------
        // Customer Contacts
        // ----------------------------------------------------------------

        public virtual List<Entity.CustomerContacts> GetCustomerContactsList(String ContactPerson, Int64 CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerContactsListByPersonName";
            cmdGet.Parameters.AddWithValue("@ContactPerson", ContactPerson);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            //SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            //p.Direction = ParameterDirection.Output;
            //cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CustomerContacts> lstObject = new List<Entity.CustomerContacts>();
            while (dr.Read())
            {
                Entity.CustomerContacts objEntity = new Entity.CustomerContacts();
                //objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.ContactPerson1 = GetTextVale(dr, "ContactPerson1");
                objEntity.ContactNumber1 = GetTextVale(dr, "ContactNumber1");
                objEntity.ContactEmail1 = GetTextVale(dr, "ContactEmail1");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        public DataTable GetCustomerContactsDetail(Int64 pCustomerID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT cc.pkID, cc.CustomerID, cc.ContactPerson1, cc.ContactNumber1, cc.ContactEmail1, cc.ContactDesigCode1  From mst_customer_contacts cc Inner Join MST_Customer cs On cs.CustomerID = cc.CustomerID Left Outer Join MST_Designation ds On cc.ContactDesigCode1 = ds.DesigCode Where cc.CustomerID = " + @pCustomerID.ToString();
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetCustomerContacts(Int64 pCustomerID , String PersonName)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT cc.pkID, cc.CustomerID, cc.ContactPerson1, cc.ContactNumber1, cc.ContactEmail1, cc.ContactDesigCode1  From mst_customer_contacts cc Inner Join MST_Customer cs On cs.CustomerID = cc.CustomerID Left Outer Join MST_Designation ds On cc.ContactDesigCode1 = ds.DesigCode Where cc.CustomerID = " + @pCustomerID.ToString() + " AND ContactPerson1 = " + @PersonName.ToString();
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.CustomerContacts> GetCustomerContactsList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerContactsList";
            cmdGet.Parameters.AddWithValue("@CustomerID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CustomerContacts> lstObject = new List<Entity.CustomerContacts>();
            while (dr.Read())
            {
                Entity.CustomerContacts objEntity = new Entity.CustomerContacts();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.ContactPerson1 = GetTextVale(dr, "ContactPerson1");
                objEntity.ContactDesigCode1 = GetTextVale(dr, "ContactDesigCode1");
                objEntity.ContactNumber1 = GetTextVale(dr, "ContactNumber1");
                objEntity.ContactEmail1 = GetTextVale(dr, "ContactEmail1");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.CustomerContacts> GetCustomerContactsList(Int64 CustomerID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerContactsList";
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CustomerContacts> lstObject = new List<Entity.CustomerContacts>();
            while (dr.Read())
            {
                Entity.CustomerContacts objEntity = new Entity.CustomerContacts();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.ContactPerson1 = GetTextVale(dr, "ContactPerson1");
                objEntity.ContactDesigCode1 = GetTextVale(dr, "ContactDesigCode1");
                objEntity.ContactNumber1 = GetTextVale(dr, "ContactNumber1");
                objEntity.ContactEmail1 = GetTextVale(dr, "ContactEmail1");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateCustomerContacts(Entity.CustomerContacts objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "CustomerContacts_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ContactPerson1", objEntity.ContactPerson1);
            cmdAdd.Parameters.AddWithValue("@ContactDesigCode1", objEntity.ContactDesigCode1);
            cmdAdd.Parameters.AddWithValue("@ContactNumber1", objEntity.ContactNumber1);
            cmdAdd.Parameters.AddWithValue("@ContactEmail1", objEntity.ContactEmail1);

            //cmdAdd.Parameters.AddWithValue("@ContactPerson2", objEntity.ContactPerson2);
            //cmdAdd.Parameters.AddWithValue("@ContactDesigCode2", objEntity.ContactDesigCode2);
            //cmdAdd.Parameters.AddWithValue("@ContactNumber2", objEntity.ContactNumber2);
            //cmdAdd.Parameters.AddWithValue("@ContactEmail2", objEntity.ContactEmail2);

            //cmdAdd.Parameters.AddWithValue("@ContactPerson3", objEntity.ContactPerson3);
            //cmdAdd.Parameters.AddWithValue("@ContactDesigCode3", objEntity.ContactDesigCode3);
            //cmdAdd.Parameters.AddWithValue("@ContactNumber3", objEntity.ContactNumber3);
            //cmdAdd.Parameters.AddWithValue("@ContactEmail3", objEntity.ContactEmail3);

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

        public virtual void DeleteCustomerContacts(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CustomerContacts_DEL";
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

        public virtual void DeleteCustomerContactsByCustomer(Int64 CustomerID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CustomerContactsByCustomer_DEL";
            cmdDel.Parameters.AddWithValue("@CustomerID", CustomerID);
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

        // ----------------------------------------------------------------
        // Customer Price List
        // ----------------------------------------------------------------
        public DataTable GetCustomerPrice(Int64 pCustomerID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT PL.pkID, PL.CustomerID, PL.ProductID, pr.ProductName, PL.UnitPrice, PL.Discount From MST_PriceList PL Inner Join MST_Product pr On PL.ProductID = pr.pkID Where PL.CustomerID = " + @pCustomerID.ToString();
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual void AddUpdateCustomerPrice(Entity.CustomerPriceList objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PriceList_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@UnitPrice", objEntity.UnitPrice);
            cmdAdd.Parameters.AddWithValue("@Discount", objEntity.Discount);
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

        public virtual void DeleteCustomerPrice(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PriceList_DEL";
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

        public virtual void DeleteCustomerPriceByCustomer(Int64 CustomerID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PriceListByCustomer_DEL";
            cmdDel.Parameters.AddWithValue("@CustomerID", CustomerID);
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
