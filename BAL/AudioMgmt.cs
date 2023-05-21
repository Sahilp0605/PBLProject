using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class AudioMgmt
    {
        public static List<Entity.AudioFiles> GetAudioFiles(Int64 pkID, String ModuleName, String KeyID)
        {
            return (new DAL.AudioSQL().GetAudioFiles(pkID, ModuleName, KeyID));
        }

        public static void AddUpdateAudio(Entity.AudioFiles entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AudioSQL().AddUpdateAudio(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteAudio(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AudioSQL().DeleteAudio(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteAudioByKeyID(String ModuleName, String KeyID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AudioSQL().DeleteAudioByKeyID(ModuleName, KeyID, out ReturnCode, out ReturnMsg);
        }
    }
}
