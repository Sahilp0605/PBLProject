using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
//using DAL;
using StarsProject;
using System.ComponentModel;
using CrystalDecisions.Web;
using System.Drawing.Printing;

namespace StarsProject
{
    public partial class MyReportViewer : System.Web.UI.Page
    {
        DateTime startDate, endEdate;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnReportName.Value = (!String.IsNullOrEmpty(Request.QueryString["ReportName"].ToString())) ? Request.QueryString["ReportName"].ToString() : "";
                hdnKeyVal.Value = (!String.IsNullOrEmpty(Request.QueryString["KeyVal"].ToString())) ? Request.QueryString["KeyVal"].ToString() : "";

                if (hdnReportName.Value.ToLower() == "quotation_template_1")
                {
                    Report_QuotationList(hdnKeyVal.Value);
                }
            }


        }
        public void Report_QuotationList(String pkID)
        {
            string pQuotationNo = "";
            int totrec;
            if (!String.IsNullOrEmpty(pkID))
            {
                List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
                lstEntity = BAL.QuotationMgmt.GetQuotationList(Convert.ToInt64(pkID), Session["LoginUserID"].ToString(), 1, 1000, out totrec);
                // -----------------------------------------------------------------
                pQuotationNo = lstEntity[0].QuotationNo.ToString();
                //if (!String.IsNullOrEmpty(pQuotationNo) && pQuotationNo != "0")
                //{
                //    ReportDocument crystalreport = new ReportDocument();
                  
                //    crystalreport.Load(Server.MapPath("~/Reports/Quotation_Template_1.rpt"));
                //    CrystalReportViewer1.ReportSource = crystalreport;
                //    string[] parm = { "@QuotationNo", "@PageNo", "@PageSize" };
                //    string[] parmvalue = { pQuotationNo, "1", "1000" };
                //    crystalreport.SetParameterValue("@QuotationNo", pQuotationNo, "Quotation_ProductDetail");
                //    ReportBinder(parm, parmvalue, crystalreport);
                //    Session["report"] = crystalreport;
                //}
                string ReportFilePath = "";
                if (!String.IsNullOrEmpty(pQuotationNo) && pQuotationNo != "0")
                {
                    ReportDocument reportdocument = new ReportDocument();
                    ReportFilePath = Server.MapPath("~/Reports/Quotation_Template_1.rpt");
                    reportdocument.Load(ReportFilePath);
                    string con = System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
                    SqlConnectionStringBuilder scon = new SqlConnectionStringBuilder(con);
                    reportdocument.SetDatabaseLogon(scon.UserID, scon.Password, scon.DataSource, scon.InitialCatalog);

                    reportdocument.SetParameterValue("@QuotationNo", pQuotationNo);
                    reportdocument.SetParameterValue("@PageNo", "1");
                    reportdocument.SetParameterValue("@PageSize", "1000");

                    reportdocument.SetParameterValue("@QuotationNo", pQuotationNo, "Quotation_ProductDetail.rpt");
                    reportdocument.SetParameterValue("@PageNo", "1", "Quotation_ProductDetail.rpt");
                    reportdocument.SetParameterValue("@PageSize", "1000", "Quotation_ProductDetail.rpt");

                    reportdocument.SetDatabaseLogon(scon.UserID, scon.Password, scon.DataSource, scon.InitialCatalog);
                    CrystalReportViewer1.ReportSource = reportdocument;

                }
            }
        }

        void ReportBinder(string[] parm, string[] parmvalues, ReportDocument reportDocument)
        {
            ParameterFields parameterFields = new ParameterFields();
            ParameterField parameterField = null;
            ParameterDiscreteValue parameterValue = null;

            for (int i = 0; i < parm.Length; i++)
            {
                parameterField = new ParameterField();
                parameterValue = new ParameterDiscreteValue();
                parameterField.Name = parm[i];
                parameterValue.Value = parmvalues[i];
                parameterField.CurrentValues.Add(parameterValue);
                parameterFields.Add(parameterField);
            }
            CrystalReportViewer1.ParameterFieldInfo = parameterFields;


            string con = System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;

            SqlConnectionStringBuilder scon = new SqlConnectionStringBuilder(con);
            ConnectionInfo myConnectionInfo = new ConnectionInfo();

            myConnectionInfo.ServerName = scon.DataSource;
            myConnectionInfo.DatabaseName = scon.InitialCatalog;
            myConnectionInfo.UserID = scon.UserID;
            myConnectionInfo.Password = scon.Password;
            setDBLOGONforREPORT(myConnectionInfo, reportDocument);
        }

        void setDBLOGONforREPORT(ConnectionInfo myconnectioninfo, ReportDocument reportDocument)
        {
            Tables tables = reportDocument.Database.Tables;
            foreach (ReportDocument subreport in reportDocument.Subreports)
            {
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
                {
                    TableLogOnInfos mytableloginfos = new TableLogOnInfos();
                    mytableloginfos = CrystalReportViewer1.LogOnInfo;
                    foreach (TableLogOnInfo myTableLogOnInfo in mytableloginfos)
                    {
                        myTableLogOnInfo.ConnectionInfo = myconnectioninfo;
                    }
                }
            }
        }

        public void alertMessage()
        {
            Response.Write("<script>alert('No data Found');</script>");
        }
    }
}