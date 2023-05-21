using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class FeedbackSQL : BaseSqlManager
    {
        public virtual List<Entity.Feedback> GetFeedbackQuestions()
        {
            List<Entity.Feedback> lstLocation = new List<Entity.Feedback>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "FeedbackQuestionList";
            //cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Feedback objEntity = new Entity.Feedback();
                objEntity.Questions = GetTextVale(dr, "Questions");
                
                lstLocation.Add(objEntity);
            }
            dr.Close();
           
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateFeedback(Entity.Feedback objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Feedback_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID" ,objEntity.pkID);
	        cmdAdd.Parameters.AddWithValue("@CustomerId",objEntity.CustomerId); 
	        cmdAdd.Parameters.AddWithValue("@Product_Satisfaction",objEntity.Product_Satisfaction);
	        cmdAdd.Parameters.AddWithValue("@SalesExecutive_Presentation",objEntity.SalesExecutive_Presentation);
	        cmdAdd.Parameters.AddWithValue("@Product_Features" ,objEntity.Product_Features);
	        cmdAdd.Parameters.AddWithValue("@Product_Presentation",objEntity.Product_Presentation);
            cmdAdd.Parameters.AddWithValue("@Recommendyn", objEntity.Recommendyn);
            cmdAdd.Parameters.AddWithValue("@Comment", objEntity.Comment);
           
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
    }
}
