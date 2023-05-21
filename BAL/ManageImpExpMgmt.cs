using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BAL
{
    public class ManageImpExpMgmt
    {
        // ===================================================================================
        // Member Photo ID .. 
        // ===================================================================================
        #region Upload Product Photo ID
        public static void AddProductPhoto(Int64 pkID, byte[] imgData1, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ManageImpExpSQL().AddProductPhoto(pkID, imgData1, out ReturnCode, out ReturnMsg);
        }
        #endregion

        #region Upload Product Photo ID
        public static byte[] GetProductPhotoID(Int64 pkID)
        {
            return new DAL.ManageImpExpSQL().GetProductPhotoID(pkID);
        }
        #endregion

        // ===================================================================================
        // Member Photo ID .. 
        // ===================================================================================
        #region Upload Member Photo ID
        public static void AddMemberPhoto(string pRegistrationNo, byte[] imgData1, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ManageImpExpSQL().AddMemberPhoto(pRegistrationNo, imgData1, out ReturnCode, out ReturnMsg);
        }
        #endregion
        
        #region Upload Member Photo ID
        public static byte[] GetMemberPhotoID(string pApplicantNo)
        {
            return new DAL.ManageImpExpSQL().GetMemberPhotoID(pApplicantNo);
        }
        #endregion
        
        // ===================================================================================
        // Driver Photo ID ..
        // ===================================================================================
        #region Upload Driver Photo ID
        public static void AddDriverPhoto(Int64 pDriverId, byte[] imgData1, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ManageImpExpSQL().AddDriverPhoto(pDriverId, imgData1, out ReturnCode, out ReturnMsg);
        }
        #endregion

        #region Upload Driver Photo ID
        public static byte[] GetDriverPhotoID(Int64 pDriverId)
        {
            return new DAL.ManageImpExpSQL().GetDriverPhotoID(pDriverId);
        }
        #endregion
    }
}
