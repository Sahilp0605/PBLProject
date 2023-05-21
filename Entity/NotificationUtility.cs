using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class NotificationUtility
    {
    }

    //WhatsApp Notification Classes
    //****************************************
    public class SingleDocPayload
    {
        public string number { get; set; }
        public string document { get; set; }
        public string filename { get; set; }
    }

    public class Payload
    {
        public string number { get; set; }
        public string message { get; set; }
        public string filename { get; set; }
    }
    //****************************************

}
