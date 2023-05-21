using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class EmailTemplate
    {
        public Int64 pkID { get; set; }
        public string Category { get; set; }
        public string TemplateID { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string ContentData { get; set; }
        public string ContentDataSMS { get; set; }
        public string ImageAttachment1 { get; set; }
        public string ImageAttachment2 { get; set; }
        public Boolean ProductAttachment { get; set; }

        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string LoginUserID { get; set; }
    }

    public class CampaignTemplate
    {
        public Int64 CampaignID { get; set; }
        public string CampaignCategory { get; set; }
        public string CampaignSubject { get; set; }
        public string CampaignHeader { get; set; }
        public string CampaignFooter { get; set; }
        public string CampaignImageUrl { get; set; }
        public string CampaignImagePlacement { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string LoginUserID { get; set; }
    }

    public class CampaignLog
    {
        public Int64 pkID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 CampaignID { get; set; }
        public string CampaignCategory { get; set; }
        public string CampaignFor { get; set; }
        public string CampaignContact { get; set; }
        public DateTime CampaignSentOn { get; set; }
        public string CampaignStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string LoginUserID { get; set; }
    }

    public class EmailStructure
    {
        
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string LoginUserID { get; set; }
    }
}
