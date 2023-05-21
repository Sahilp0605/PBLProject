using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace StarsProject.Reports
{
    public partial class Report_ActiveMember : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ReportName"].ToString()))
                {
                    hdnReportName.Value = Request.QueryString["ReportName"].ToString();
                    if (hdnReportName.Value.ToLower() == "activememberlist")
                    {
                        lblHead.Text = "Membership Activation";
                    }
                    else if (hdnReportName.Value.ToLower() == "expirememberlist")
                    {
                        lblHead.Text = "Membership Expiry";
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (hdnReportName.Value.ToLower() == "activememberlist")
            {
                setActiveMemberList();
            }
            else if (hdnReportName.Value.ToLower() == "expirememberlist")
            {
                setExpireMemberList();
            }


        }
        protected void ResetSerch(object sender, EventArgs e)
        {
            strdate.Text = "";
            eddate.Text = "";
            rptviewer.Visible = false;
        }

        public void setActiveMemberList()
        {
            Entity.Report.Report_Member objPara = new Entity.Report.Report_Member();
            if (validateFromTo(hdnStartDate.Value, hdnEndDate.Value, objPara))
            {
                DataTable dt = new DataTable();
                //dt = BAL.ReportMgmt.GetActiveMemberTable(objPara);
                ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                myViewer.LocalReport.ReportPath = "Report_ActiveMember.rdlc";
                myViewer.ProcessingMode = ProcessingMode.Local;
                myViewer.LocalReport.DataSources.Clear();
                myViewer.LocalReport.DataSources.Add(rds);
                myViewer.LocalReport.Refresh();
                if (dt.Rows.Count > 0)
                {
                    rptviewer.Visible = true;
                }
            }
        }

        public void setExpireMemberList()
        {
            Entity.Report.Report_Member objPara = new Entity.Report.Report_Member();
            if (validateFromTo(hdnStartDate.Value, hdnEndDate.Value, objPara))
            {
                DataTable dt = new DataTable();
                //dt = BAL.ReportMgmt.GetExpireMemberTable(objPara);
                ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                myViewer.LocalReport.ReportPath = "Report_ExpireMember.rdlc";
                myViewer.ProcessingMode = ProcessingMode.Local;
                myViewer.LocalReport.DataSources.Clear();
                myViewer.LocalReport.DataSources.Add(rds);
                myViewer.LocalReport.Refresh();
                if (dt.Rows.Count > 0)
                {
                    rptviewer.Visible = true;
                }
            }
        }
        public Boolean validateFromTo(string d1, string d2, Entity.Report.Report_Member objPara)
        {
            
            strdate.Text = d1;
            eddate.Text = d2;
            if (strdate.Text != "" && eddate.Text != "")
            {
                string[] dat = strdate.Text.Split('/');
                objPara.Start_Date = Convert.ToDateTime(dat[1] + "/" + dat[0] + "/" + dat[2]);//Convert Date in MM/dd/yyyy format
                string[] dat2 = eddate.Text.Split('/');
                objPara.End_Date = Convert.ToDateTime(dat2[1] + "/" + dat2[0] + "/" + dat2[2]);

                if (objPara.Start_Date > objPara.End_Date)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "Date", "javascript:alert('Please insert valid Date');", true);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (strdate.Text == "")
                    objPara.Start_Date = Convert.ToDateTime("01/01/1900");
                else
                {
                    string[] dat = strdate.Text.Split('/');
                    objPara.Start_Date = Convert.ToDateTime(dat[0] + "/" + dat[1] + "/" + dat[2]);
                }
                if (eddate.Text == "")
                    objPara.End_Date = Convert.ToDateTime("01/01/1900");
                else
                {
                    string[] dat = eddate.Text.Split('/');
                    objPara.End_Date = Convert.ToDateTime(dat[0] + "/" + dat[1] + "/" + dat[2]);
                }
                return true;
            }
        }
    }
}