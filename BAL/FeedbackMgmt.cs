using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class FeedbackMgmt
    {
        public static List<Entity.Feedback> GetFeedbackQuestions()
        {
            return (new DAL.FeedbackSQL().GetFeedbackQuestions());
        }
        public static void AddUpdateFeedback(Entity.Feedback entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FeedbackSQL().AddUpdateFeedback(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
