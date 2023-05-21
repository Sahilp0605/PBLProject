using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace StarsProject
{
    public partial class myToDo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                BindTaskList(Session["LoginUserID"].ToString(), TaskStatus);
            }
        }

        public void BindTaskList(string loginuserid, String pTaskStatus)
        {
            int TotRec;
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            TaskStatus = pTaskStatus;
            List<Entity.ToDo> lstEntity = new List<Entity.ToDo>();
            lstEntity = BAL.ToDoMgmt.GetDashboardToDoList(TaskStatus, pMon, pYear, loginuserid, 1, 100000, out TotRec);
            rptTODO.DataSource = lstEntity;
            rptTODO.DataBind();
        }
        public Int64 ToDoCount
        {
            get
            {
                return Convert.ToInt64(rptTODO.Items.Count);
            }
        }
        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }

        public string pageMonth
        {
            get { return hdnMonth.Value; }
            set { hdnMonth.Value = value; }
        }

        public string pageYear
        {
            get { return hdnYear.Value; }
            set { hdnYear.Value = value; }
        }
        public string TaskStatus
        {
            get { return hdnTaskStatus.Value; }
            set { hdnTaskStatus.Value = value; }
        }
        protected void rptTODO_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn0 = ((HiddenField)e.Item.FindControl("hdnDueDate"));
                HiddenField hdn1 = ((HiddenField)e.Item.FindControl("hdnCompletionDate"));
                HtmlGenericControl dv = ((HtmlGenericControl)e.Item.FindControl("ltrCompletion"));
                DateTime dtdue = DateTime.Now;
                DateTime dtcurr = DateTime.Now;

                if (String.IsNullOrEmpty(hdn0.Value))
                    dtdue = Convert.ToDateTime(hdn1.Value);

                if (String.IsNullOrEmpty(hdn1.Value))
                {
                    dv.InnerText = (dtdue < dtcurr) ? "OverDue" : "Not Applied";
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(hdn1.Value);
                    if (dt.Year < 2000)
                        dv.InnerText = (dtdue<dtcurr) ? "OverDue" : "Not Applied";
                }

            }
        }
    }
}