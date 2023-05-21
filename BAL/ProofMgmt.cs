using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ProofMgmt
    {
        public static List<Entity.Proof> GetProofList()
        {
            return (new DAL.ProofSQL().GetProofList());
        }

        public static List<Entity.Proof> GetProof(long ProofID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProofSQL().GetProof(ProofID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateProof(Entity.Proof entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProofSQL().AddUpdateProof(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteProof(long ProofID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProofSQL().DeleteProof(ProofID, out ReturnCode, out ReturnMsg);
        }
    }
}
