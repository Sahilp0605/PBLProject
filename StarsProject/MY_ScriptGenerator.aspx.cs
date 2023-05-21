using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class MY_ScriptGenerator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GetStructure()
        {
            if (!String.IsNullOrEmpty(txtTableName.Text))
            {
                rptStructure.DataSource = BAL.ScriptGeneratorMgmt.GetTableStructure(txtTableName.Text);
                rptStructure.DataBind();
            }
        }

        protected void btnGetCols_Click(object sender, EventArgs e)
        {
            GetStructure();
        }
        protected void btnSelectAll_ServerClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptStructure.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    System.Web.UI.HtmlControls.HtmlInputCheckBox chkCtrl = (System.Web.UI.HtmlControls.HtmlInputCheckBox)item.FindControl("chkSelect");
                    chkCtrl.Checked = true;
                }
            }
        }
        protected void btnDeSelectAll_ServerClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptStructure.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    System.Web.UI.HtmlControls.HtmlInputCheckBox chkCtrl = (System.Web.UI.HtmlControls.HtmlInputCheckBox)item.FindControl("chkSelect");
                    chkCtrl.Checked = false;
                }
            }
        }
        public void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSPList.Text) && !String.IsNullOrEmpty(txtSPInsUpd.Text) && !String.IsNullOrEmpty(txtSPDel.Text))
            {

                //System.Web.UI.HtmlControls.HtmlInputCheckBox chkCtrl = (System.Web.UI.HtmlControls.HtmlInputCheckBox)item.FindControl("chkSelect");
                //HiddenField hdnpkID = (HiddenField)item.FindControl("hdnpkID");
                //HiddenField hdnColName = (HiddenField)item.FindControl("hdnColName");
                //HiddenField hdnColType = (HiddenField)item.FindControl("hdnColType");
                //HiddenField hdnColIsNull = (HiddenField)item.FindControl("hdnColIsNull");
                //HiddenField hdnColWidth = (HiddenField)item.FindControl("hdnColWidth");
                //HiddenField hdnColScale = (HiddenField)item.FindControl("hdnColScale");
                // -----------------------------------------------------------------------
                string colList = BAL.ScriptGeneratorMgmt.GetTableColumnsList(txtTableName.Text, "");
                string colListIns = BAL.ScriptGeneratorMgmt.GetTableColumnsList(txtTableName.Text, "INS");
                string colListUpd = BAL.ScriptGeneratorMgmt.GetTableColumnsList(txtTableName.Text, "UPD");
                string colListInsUpd = BAL.ScriptGeneratorMgmt.GetTableColumnsList(txtTableName.Text, "INSUPD");

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                // Database Scrit Generation
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                StreamWriter flList = new StreamWriter("D:\\DMLScript.txt", false);
                // -----------------------------------------------------------------------
                //  Section : Listing 
                // -----------------------------------------------------------------------
                string tmpVal = GetMasterSP_List();
                tmpVal = tmpVal.Replace("{SPName}", txtSPList.Text);
                tmpVal = tmpVal.Replace("{TableName}", txtTableName.Text);
                tmpVal = tmpVal.Replace("{ColumnList}", colList);

                flList.WriteLine("-- >>>>> Listing SP");
                flList.Write(tmpVal);

                // -----------------------------------------------------------------------
                //  Section : Insert/Update 
                // -----------------------------------------------------------------------
                tmpVal = GetMasterSP_InsUpd();
                tmpVal = tmpVal.Replace("{SPName}", txtSPInsUpd.Text);
                tmpVal = tmpVal.Replace("{TableName}", txtTableName.Text);
                tmpVal = tmpVal.Replace("{ColumnList}", colListIns.Replace("@","").Replace("LoginUserID", "CreatedBy").Replace("GETDATE()", "CreatedDate"));
                tmpVal = tmpVal.Replace("{InsColumnList}", colListIns);
                tmpVal = tmpVal.Replace("{UpdColumnList}", colListUpd);
                tmpVal = tmpVal.Replace("{InsUpdParameterList}", colListInsUpd + ",");

                flList.WriteLine("-- >>>>> Stored Procedure : Insert/Update");
                flList.Write(tmpVal);

                // -----------------------------------------------------------------------
                //  Section : Delete 
                // -----------------------------------------------------------------------
                tmpVal = GetMasterSP_Del();
                tmpVal = tmpVal.Replace("{SPName}", txtSPDel.Text);
                tmpVal = tmpVal.Replace("{TableName}", txtTableName.Text);

                flList.WriteLine("-- >>>>> Stored Procedure : Delete");
                flList.Write(tmpVal);

                
                flList.Close();
                flList.Dispose();
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                // C# Code 
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                string tmpEnt = "";
                StreamWriter flEntity = new StreamWriter("D:\\CSCode.txt", false);

                tmpEnt = GenerateEntity();
                flEntity.Write(tmpEnt);

                tmpEnt = GenerateBAL();
                flEntity.Write(tmpEnt);

                tmpEnt = GenerateDAL();
                flEntity.Write(tmpEnt);

                flEntity.Close();
                flEntity.Dispose();
            }
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Section : Listing - {SPName} , {TableName} , {ColumnList} 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public string GetMasterSP_List()
        {
            string tmpVal = "";
            tmpVal += "DROP PROCEDURE {SPName}" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "SET ANSI_NULLS ON" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += "SET QUOTED_IDENTIFIER ON" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "CREATE PROCEDURE[dbo].[{SPName}]" + System.Environment.NewLine;
            tmpVal += "    @pkID BIGINT = 0," + System.Environment.NewLine;
            tmpVal += "    @LoginUserID   NVARCHAR(20) = NULL," + System.Environment.NewLine;
            tmpVal += "    @SearchKey NVARCHAR(50) = NULL," + System.Environment.NewLine;
            tmpVal += "    @PageNo INT = 0," + System.Environment.NewLine;
            tmpVal += "    @PageSize INT = 0," + System.Environment.NewLine;
            tmpVal += "    @TotalCount INT = 0 OUTPUT" + System.Environment.NewLine;
            tmpVal += "AS" + System.Environment.NewLine;
            tmpVal += "BEGIN" + System.Environment.NewLine;
            tmpVal += "    SET NOCOUNT ON" + System.Environment.NewLine;
            tmpVal += "    Declare @UserRole NVARCHAR(50), @CompanyID BIGINT, @CompanyType NVARCHAR(20), @EmployeeID BIGINT;" + System.Environment.NewLine;
            tmpVal += "    Select @UserRole = lower(RoleCode), @CompanyID = lower(CompanyID)," + System.Environment.NewLine;
            tmpVal += "           @CompanyType = lower(CompanyType), @EmployeeID = EmployeeID" + System.Environment.NewLine;
            tmpVal += "    From viewCompanyUsers" + System.Environment.NewLine;
            tmpVal += "    Where lower(UserID) = lower(@LoginUserID);" + System.Environment.NewLine;
            tmpVal += "            /* ------------------------------------------------------------------------ */" + System.Environment.NewLine;
            tmpVal += "            DECLARE @StartNo INT" + System.Environment.NewLine;
            tmpVal += "            DECLARE @EndNo INT" + System.Environment.NewLine;
            tmpVal += "    SET @StartNo = (@PageNo * @PageSize) - (@PageSize - 1)" + System.Environment.NewLine;
            tmpVal += "    SET @EndNo = (@PageNo * @PageSize)" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "    SET @TotalCount = (Select COUNT(*) From {TableName})" + System.Environment.NewLine;
            tmpVal += "    /* ------------------------------------------------------------------------ */  " + System.Environment.NewLine;
            tmpVal += "    IF (@SearchKey IS NOT NULL AND @SearchKey <> '')" + System.Environment.NewLine;
            tmpVal += "    BEGIN" + System.Environment.NewLine;
            tmpVal += "        SELECT RowNum, {ColumnList}" + System.Environment.NewLine;
            tmpVal += "        FROM(SELECT ROW_NUMBER() OVER(ORDER BY CreatedDate DESC) AS RowNum," + System.Environment.NewLine;
            tmpVal += "                    {ColumnList}" + System.Environment.NewLine;
            tmpVal += "            From {TableName})  AS Temp" + System.Environment.NewLine;
            tmpVal += "        WHERE Concat({ColumnList}) Like Concat('%', @SearchKey, '%');" + System.Environment.NewLine;
            tmpVal += "            RETURN;" + System.Environment.NewLine;
            tmpVal += "    END" + System.Environment.NewLine;
            tmpVal += "    BEGIN" + System.Environment.NewLine;
            tmpVal += "        IF(@pkID = 0 or @pkID IS NULL)" + System.Environment.NewLine;
            tmpVal += "        BEGIN" + System.Environment.NewLine;
            tmpVal += "           SELECT RowNum, {ColumnList}" + System.Environment.NewLine;
            tmpVal += "            FROM(SELECT ROW_NUMBER() OVER(ORDER BY CreatedDate DESC) AS RowNum," + System.Environment.NewLine;
            tmpVal += "                     {ColumnList}" + System.Environment.NewLine;
            tmpVal += "                From {TableName})  AS Temp" + System.Environment.NewLine;
            tmpVal += "            WHERE RowNum BETWEEN @StartNo AND @EndNo;" + System.Environment.NewLine;
            tmpVal += "        END" + System.Environment.NewLine;
            tmpVal += "        ELSE" + System.Environment.NewLine;
            tmpVal += "        BEGIN" + System.Environment.NewLine;
            tmpVal += "            SELECT RowNum, {ColumnList}" + System.Environment.NewLine;
            tmpVal += "            FROM(SELECT ROW_NUMBER() OVER(ORDER BY CreatedDate DESC) AS RowNum," + System.Environment.NewLine;
            tmpVal += "                    {ColumnList}" + System.Environment.NewLine;
            tmpVal += "                From {TableName}" + System.Environment.NewLine;
            tmpVal += "            Where pkID = @pkID) AS Temp;" + System.Environment.NewLine;
            tmpVal += "        END" + System.Environment.NewLine;
            tmpVal += "   END" + System.Environment.NewLine;
            tmpVal += "END" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;

            return tmpVal;
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Section : InsUpd - {SPName} , {TableName} , {InsUpdParameterList} , 
        //                    {ColumnList} , {InsColumnList} , {UpdColumnList}
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public string GetMasterSP_InsUpd()
        {
            string tmpVal = "";
            tmpVal += "DROP PROCEDURE {SPName}" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "SET ANSI_NULLS ON" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += "SET QUOTED_IDENTIFIER ON" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += "CREATE PROCEDURE[dbo].[{SPName}]" + System.Environment.NewLine;
            tmpVal += "{InsUpdParameterList}" + System.Environment.NewLine;
            tmpVal += "@LoginUserID NVARCHAR(20)," + System.Environment.NewLine;
            tmpVal += "@ReturnCode INT OUTPUT," + System.Environment.NewLine;
            tmpVal += "@ReturnMsg NVARCHAR(255) OUTPUT" + System.Environment.NewLine;
            tmpVal += "AS" + System.Environment.NewLine;
            tmpVal += "BEGIN" + System.Environment.NewLine;
            tmpVal += "    BEGIN TRANSACTION ChkPoint" + System.Environment.NewLine;
            tmpVal += "    BEGIN TRY" + System.Environment.NewLine;
            tmpVal += "        SET @ReturnCode = 0;" + System.Environment.NewLine;
            tmpVal += "        SET @ReturnMsg = '';" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "        IF NOT EXISTS(SELECT* FROM {TableName} WHERE pkID= @pkID)" + System.Environment.NewLine;
            tmpVal += "            BEGIN" + System.Environment.NewLine;
            tmpVal += "                INSERT INTO {TableName}" + System.Environment.NewLine;
            tmpVal += "                  ({ColumnList})" + System.Environment.NewLine;
            tmpVal += "                VALUES" + System.Environment.NewLine;
            tmpVal += "                  ({InsColumnList})" + System.Environment.NewLine;
            tmpVal += "                SET @ReturnCode = 1" + System.Environment.NewLine;
            tmpVal += "                SET @ReturnMsg = 'Transaction Added Successfully !'" + System.Environment.NewLine;
            tmpVal += "            END" + System.Environment.NewLine;
            tmpVal += "            ELSE" + System.Environment.NewLine;
            tmpVal += "            BEGIN" + System.Environment.NewLine;
            tmpVal += "                UPDATE {TableName}" + System.Environment.NewLine;
            tmpVal += "                SET {UpdColumnList}" + System.Environment.NewLine;
            tmpVal += "                WHERE pkID = @pkID" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "                SET @ReturnCode = 2" + System.Environment.NewLine;
            tmpVal += "                SET @ReturnMsg = 'Transaction Updated Successfully !'" + System.Environment.NewLine;
            tmpVal += "            END" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "        -- Save New/Existing Record in Client Table" + System.Environment.NewLine;
            tmpVal += "        COMMIT TRANSACTION  ChkPoint" + System.Environment.NewLine;
            tmpVal += "    END TRY" + System.Environment.NewLine;
            tmpVal += "    BEGIN CATCH" + System.Environment.NewLine;
            tmpVal += "        ROLLBACK TRANSACTION ChkPoint" + System.Environment.NewLine;
            tmpVal += "        DECLARE @ErrorMessage NVARCHAR(4000);" + System.Environment.NewLine;
            tmpVal += "        DECLARE @ErrorSeverity INT;" + System.Environment.NewLine;
            tmpVal += "        DECLARE @ErrorState INT;" + System.Environment.NewLine;
            tmpVal += "        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();" + System.Environment.NewLine;
            tmpVal += "        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "        SET @ReturnCode = 0" + System.Environment.NewLine;
            tmpVal += "        SET @ReturnMsg = 'Some Error in occure'" + System.Environment.NewLine;
            tmpVal += "    END CATCH" + System.Environment.NewLine;
            tmpVal += "END" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            // --------------------------------------
            return tmpVal;
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Section : Del - {SPName} , {TableName} 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public string GetMasterSP_Del()
        {
            string tmpVal = "";
            tmpVal += "DROP PROCEDURE {SPName}" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += " " + System.Environment.NewLine;
            tmpVal += "SET ANSI_NULLS ON" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += "SET QUOTED_IDENTIFIER ON" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            tmpVal += "CREATE PROCEDURE[dbo].[{SPName}]" + System.Environment.NewLine;
            tmpVal += "    @pkID BIGINT = 0," + System.Environment.NewLine;
            tmpVal += "    @ReturnCode INT OUTPUT," + System.Environment.NewLine;
            tmpVal += "    @ReturnMsg NVARCHAR(255) OUTPUT" + System.Environment.NewLine;
            tmpVal += "AS" + System.Environment.NewLine;
            tmpVal += "BEGIN" + System.Environment.NewLine;
            tmpVal += "    BEGIN TRANSACTION ChkPoint" + System.Environment.NewLine;
            tmpVal += "    BEGIN TRY" + System.Environment.NewLine;
            tmpVal += "        SET @ReturnCode = 0;" + System.Environment.NewLine;
            tmpVal += "        SET @ReturnMsg = 'Some Exception Occured !';" + System.Environment.NewLine;
            tmpVal += "        IF EXISTS (SELECT * From {TableName} Where pkID = @pkID)" + System.Environment.NewLine;
            tmpVal += "        BEGIN" + System.Environment.NewLine;
            tmpVal += "            SET @ReturnCode = -1" + System.Environment.NewLine;
            tmpVal += "            SET @ReturnMsg = 'Cannot Delete, Because relevant Other Transaction is Exists !'" + System.Environment.NewLine;
            tmpVal += "        END" + System.Environment.NewLine;
            tmpVal += "        /* ====================================================== */" + System.Environment.NewLine;
            tmpVal += "        IF (@ReturnCode>=0)" + System.Environment.NewLine;
            tmpVal += "        BEGIN" + System.Environment.NewLine;
            tmpVal += "            DELETE FROM {TableName} WHERE pkID = @pkID" + System.Environment.NewLine;
            tmpVal += "            SET @ReturnCode = 1" + System.Environment.NewLine;
            tmpVal += "            SET @ReturnMsg = 'Transaction Deleted Successfully !'" + System.Environment.NewLine;
            tmpVal += "        END" + System.Environment.NewLine;
            tmpVal += "        -- Save New/Existing Record in Client Table" + System.Environment.NewLine;
            tmpVal += "        COMMIT TRANSACTION  ChkPoint" + System.Environment.NewLine;
            tmpVal += "    END TRY" + System.Environment.NewLine;
            tmpVal += "    BEGIN CATCH" + System.Environment.NewLine;
            tmpVal += "        ROLLBACK TRANSACTION ChkPoint" + System.Environment.NewLine;
            tmpVal += "        DECLARE @ErrorMessage NVARCHAR(4000);" + System.Environment.NewLine;
            tmpVal += "        DECLARE @ErrorSeverity INT;" + System.Environment.NewLine;
            tmpVal += "        DECLARE @ErrorState INT;" + System.Environment.NewLine;
            tmpVal += "        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();" + System.Environment.NewLine;
            tmpVal += "        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);" + System.Environment.NewLine;
            tmpVal += "        SET @ReturnCode = 0" + System.Environment.NewLine;
            tmpVal += "        SET @ReturnMsg = 'Some Error in occure'" + System.Environment.NewLine;
            tmpVal += "    END CATCH" + System.Environment.NewLine;
            tmpVal += "END" + System.Environment.NewLine;
            tmpVal += "GO" + System.Environment.NewLine;
            // --------------------------------------
            return tmpVal;
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Section : Entity - {ClassName} 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

        public string GenerateEntity()
        {
            string tmpVal = "";
            if (!String.IsNullOrEmpty(txtTableName.Text) && !String.IsNullOrEmpty(txtEntityName.Text))
            {
                List<Entity.ScriptGenerator> lstEntity = new List<Entity.ScriptGenerator>();
                lstEntity = BAL.ScriptGeneratorMgmt.GetTableStructure(txtTableName.Text);

                if (lstEntity.Count > 0)
                {
                    tmpVal += "public class " + txtEntityName.Text + System.Environment.NewLine;
                    tmpVal += "{" + System.Environment.NewLine;
                    for (int i = 0; i < lstEntity.Count; i++)
                    {
                        string xName = lstEntity[i].ColName;
                        string xType = lstEntity[i].ColType;
                        // ----------------------------------------------
                        xType = xType.Replace("nvarchar", "string");
                        xType = xType.Replace("varchar", "string");
                        xType = xType.Replace("bigint", "Int64");
                        xType = xType.Replace("int", "Int16");
                        xType = xType.Replace("bit", "Boolean");
                        xType = xType.Replace("float", "Decimal");
                        xType = xType.Replace("datetime", "DateTime");
                        // ----------------------------------------------
                        tmpVal += "public " + xType + " " + xName + " { get; set; };" + System.Environment.NewLine;
                    }
                    tmpVal += "public string LoginUserID { get; set; };" + System.Environment.NewLine;

                    tmpVal += "}" + System.Environment.NewLine;
                }
            }
            return tmpVal;
        }

        public string GenerateBAL()
        {
            string tmpVal = "";
            if (!String.IsNullOrEmpty(txtTableName.Text) && !String.IsNullOrEmpty(txtEntityName.Text))
            {
                tmpVal += "public class " + txtEntityName.Text + "Mgmt" + System.Environment.NewLine;
                tmpVal += "{" + System.Environment.NewLine;

                tmpVal += "public static List<Entity." + txtEntityName.Text + "> Get" + txtEntityName.Text + "List(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageColor, out int TotalRecord)" + System.Environment.NewLine;
                tmpVal += "{" + System.Environment.NewLine;
                tmpVal += "    return (new DAL." + txtEntityName.Text + "SQL().Get" + txtEntityName.Text + "List(pkID, LoginUserID, SearchKey, PageNo, PageColor, out TotalRecord));" + System.Environment.NewLine;
                tmpVal += "}" + System.Environment.NewLine;
                tmpVal += " " + System.Environment.NewLine;
                tmpVal += "public static void AddUpdate" + txtEntityName.Text + "(Entity." + txtEntityName.Text + " entity, out int ReturnCode, out string ReturnMsg)" + System.Environment.NewLine;
                tmpVal += "{" + System.Environment.NewLine;
                tmpVal += "    new DAL." + txtEntityName.Text + "SQL().AddUpdate" + txtEntityName.Text + "(entity, out ReturnCode, out ReturnMsg);" + System.Environment.NewLine;
                tmpVal += "}" + System.Environment.NewLine;
                tmpVal += " " + System.Environment.NewLine;
                tmpVal += "public static void Delete" + txtEntityName.Text + "(Int64 pkID, out int ReturnCode, out string ReturnMsg)" + System.Environment.NewLine;
                tmpVal += "{" + System.Environment.NewLine;
                tmpVal += "    new DAL." + txtEntityName.Text + "SQL().Delete" + txtEntityName.Text + "(pkID, out ReturnCode, out ReturnMsg);" + System.Environment.NewLine;
                tmpVal += "}" + System.Environment.NewLine;
                tmpVal += " " + System.Environment.NewLine;

                tmpVal += "}" + System.Environment.NewLine;

            }
            return tmpVal;
        }

        public string GenerateDAL()
        {
            string tmpVal = "";
            if (!String.IsNullOrEmpty(txtTableName.Text) && !String.IsNullOrEmpty(txtEntityName.Text))
            {
                List<Entity.ScriptGenerator> lstEntity = new List<Entity.ScriptGenerator>();
                lstEntity = BAL.ScriptGeneratorMgmt.GetTableStructure(txtTableName.Text);

                if (lstEntity.Count > 0)
                {
                    tmpVal += "public class " + txtEntityName.Text + "SQL : BaseSqlManager" + System.Environment.NewLine;
                    tmpVal += "{" + System.Environment.NewLine;

                    tmpVal += "public virtual List<Entity." + txtEntityName.Text + "> Get" + txtEntityName.Text + "List(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)" + System.Environment.NewLine;
                    tmpVal += "{" + System.Environment.NewLine;
                    tmpVal += "    List<Entity." + txtEntityName.Text + "> lstObject = new List<Entity." + txtEntityName.Text + ">();" + System.Environment.NewLine;
                    tmpVal += "    SqlCommand cmdGet = new SqlCommand();" + System.Environment.NewLine;
                    tmpVal += "    cmdGet.CommandType = CommandType.StoredProcedure;" + System.Environment.NewLine;
                    tmpVal += "    cmdGet.CommandText = '" + txtSPList + "'" + System.Environment.NewLine;
                    tmpVal += "    cmdGet.Parameters.AddWithValue('@pkID', pkID);" + System.Environment.NewLine;
                    tmpVal += "    cmdGet.Parameters.AddWithValue('@LoginUserID', LoginUserID);" + System.Environment.NewLine;
                    tmpVal += "    cmdGet.Parameters.AddWithValue('@SearchKey', SearchKey);" + System.Environment.NewLine;
                    tmpVal += "    cmdGet.Parameters.AddWithValue('@PageNo', PageNo);" + System.Environment.NewLine;
                    tmpVal += "    cmdGet.Parameters.AddWithValue('@PageSize', PageSize);" + System.Environment.NewLine;
                    tmpVal += "    SqlParameter p = new SqlParameter('@TotalCount', SqlDbType.Int);" + System.Environment.NewLine;
                    tmpVal += "    p.Direction = ParameterDirection.Output;" + System.Environment.NewLine;
                    tmpVal += "    cmdGet.Parameters.Add(p);" + System.Environment.NewLine;
                    tmpVal += "    SqlDataReader dr = ExecuteDataReader(cmdGet);" + System.Environment.NewLine;
                    tmpVal += "    while (dr.Read())" + System.Environment.NewLine;
                    tmpVal += "    {" + System.Environment.NewLine;
                    tmpVal += "        Entity." + txtEntityName.Text + " objEntity = new Entity." + txtEntityName.Text + "();" + System.Environment.NewLine;
                    for (int i = 0; i < lstEntity.Count; i++)
                    {
                        string xName = lstEntity[i].ColName;
                        string xType = lstEntity[i].ColType;
                        // ----------------------------------------------------------
                        xType = xType.Replace("nvarchar", "GetTextVale");
                        xType = xType.Replace("varchar", "GetTextVale");
                        xType = xType.Replace("bigint", "GetInt64");
                        xType = xType.Replace("int", "GetInt16");
                        xType = xType.Replace("bit", "GetBoolean");
                        xType = xType.Replace("float", "GetDecimal");
                        xType = xType.Replace("datetime", "GetDateTime");
                        // ----------------------------------------------------------
                        tmpVal += "        objEntity." + xName + " = " + xType + "(dr, \"" + xName + "\");" + System.Environment.NewLine;
                    }
                    tmpVal += "        lstObject.Add(objEntity);" + System.Environment.NewLine;
                    tmpVal += "    }" + System.Environment.NewLine;
                    tmpVal += "    dr.Close();" + System.Environment.NewLine;
                    tmpVal += "    TotalRecord = Convert.ToInt32(cmdGet.Parameters['@TotalCount'].Value.ToString());" + System.Environment.NewLine;
                    tmpVal += "    ForceCloseConncetion();" + System.Environment.NewLine;
                    tmpVal += "    return lstObject;" + System.Environment.NewLine;
                    tmpVal += "}" + System.Environment.NewLine;

                    tmpVal += "}" + System.Environment.NewLine;
                    tmpVal += " " + System.Environment.NewLine;
                    tmpVal += " " + System.Environment.NewLine;
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    tmpVal += "public virtual void AddUpdate" + txtEntityName.Text + "(Entity." + txtEntityName.Text + " objEntity, out int ReturnCode, out string ReturnMsg)" + System.Environment.NewLine;
                    tmpVal += "{" + System.Environment.NewLine;
                    tmpVal += "    SqlCommand cmdAdd = new SqlCommand();" + System.Environment.NewLine;
                    tmpVal += "    cmdAdd.CommandType = CommandType.StoredProcedure;" + System.Environment.NewLine;
                    tmpVal += "    cmdAdd.CommandText = '" + txtSPInsUpd.Text + "'" + System.Environment.NewLine;
                    
                    for (int i = 0; i < lstEntity.Count; i++)
                    {
                        string xName = lstEntity[i].ColName.ToLower();
                        // ----------------------------------------------------------
                        if (xName != "createdby" && xName != "createddate" && xName != "updatedby" && xName != "updateddate")
                            tmpVal += "    cmdAdd.Parameters.AddWithValue(\"@" + xName + "\", objEntity.pkID);" + System.Environment.NewLine;
                    }
                    tmpVal += "    cmdAdd.Parameters.AddWithValue(\"@LoginUserID\", objEntity.LoginUserID);" + System.Environment.NewLine;
                    tmpVal += "    SqlParameter p = new SqlParameter(\"@ReturnCode\", SqlDbType.Int);" + System.Environment.NewLine;
                    tmpVal += "    SqlParameter p1 = new SqlParameter(\"@ReturnMsg\", SqlDbType.NVarChar, 255);" + System.Environment.NewLine;
                    tmpVal += "    p.Direction = ParameterDirection.Output;" + System.Environment.NewLine;
                    tmpVal += "    p1.Direction = ParameterDirection.Output;" + System.Environment.NewLine;
                    tmpVal += "    cmdAdd.Parameters.Add(p);" + System.Environment.NewLine;
                    tmpVal += "    cmdAdd.Parameters.Add(p1);" + System.Environment.NewLine;
                    tmpVal += "    ExecuteNonQuery(cmdAdd);" + System.Environment.NewLine;
                    tmpVal += "    ReturnCode = Convert.ToInt32(cmdAdd.Parameters[\"@ReturnCode\"].Value.ToString());" + System.Environment.NewLine;
                    tmpVal += "    ReturnMsg = cmdAdd.Parameters[\"@ReturnMsg\"].Value.ToString();" + System.Environment.NewLine;
                    tmpVal += "    ForceCloseConncetion();" + System.Environment.NewLine;
                    tmpVal += "}" + System.Environment.NewLine;
                    tmpVal += " " + System.Environment.NewLine;
                    tmpVal += " " + System.Environment.NewLine;
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    tmpVal += "public virtual void Delete" + txtEntityName.Text + "(Int64 pkID, out int ReturnCode, out string ReturnMsg)" + System.Environment.NewLine;
                    tmpVal += "{" + System.Environment.NewLine;
                    tmpVal += "    SqlCommand cmdDel = new SqlCommand();" + System.Environment.NewLine;
                    tmpVal += "    cmdDel.CommandType = CommandType.StoredProcedure;" + System.Environment.NewLine;
                    tmpVal += "    cmdDel.CommandText = '" + txtSPDel.Text + "'" + System.Environment.NewLine;
                    tmpVal += "    cmdDel.Parameters.AddWithValue(\"@pkID\", pkID);" + System.Environment.NewLine;
                    tmpVal += "    SqlParameter p = new SqlParameter(\"@ReturnCode\", SqlDbType.Int);" + System.Environment.NewLine;
                    tmpVal += "    SqlParameter p1 = new SqlParameter(\"@ReturnMsg\", SqlDbType.NVarChar, 255);" + System.Environment.NewLine;
                    tmpVal += "    p.Direction = ParameterDirection.Output;" + System.Environment.NewLine;
                    tmpVal += "    p1.Direction = ParameterDirection.Output;" + System.Environment.NewLine;
                    tmpVal += "    cmdDel.Parameters.Add(p);" + System.Environment.NewLine;
                    tmpVal += "    cmdDel.Parameters.Add(p1);" + System.Environment.NewLine;
                    tmpVal += "    ExecuteNonQuery(cmdDel);" + System.Environment.NewLine;
                    tmpVal += "    ReturnCode = Convert.ToInt32(cmdDel.Parameters[\"@ReturnCode\"].Value.ToString());" + System.Environment.NewLine;
                    tmpVal += "    ReturnMsg = cmdDel.Parameters[\"@ReturnMsg\"].Value.ToString();" + System.Environment.NewLine;
                    tmpVal += "    ForceCloseConncetion();" + System.Environment.NewLine;
                    tmpVal += "}" + System.Environment.NewLine;

                }
            }
            return tmpVal;
        }

    }
}