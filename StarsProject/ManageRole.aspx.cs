using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace StarsProject
{
    public partial class ManageRole : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        string rolerightslist = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["CurrentRole"] = "";
                Session["Access"] = "";
                Session["NoAccess"] = "";
                Session["OldUserID"] = "";
                Session["reset"] = "set";
                Session["0"] = "0";
                Session["1"] = "1";
                // --------------------------------------------------------
                // Filling Application's ... All Menus
                // --------------------------------------------------------
                DataTable dt = this.GetData("menu", 0);
                this.PopulateTreeView("menu", TreeView1, dt, 0, null);
                // --------------------------------------------------------
                // Filling Application's ... All Reports
                // --------------------------------------------------------
                DataTable dtRep = this.GetData("reports", 0);
                this.PopulateTreeView("reports", TreeView2, dtRep, 0, null);
                // --------------------------------------------------------
                // Filling Application's ... All Icons
                // --------------------------------------------------------
                DataTable dtIcon = this.GetData("icons", 0);
                this.PopulateTreeView("icons", TreeView3, dtIcon, 0, null);
                // --------------------------------------------------------
                // Filling Application's ... All General Master
                // --------------------------------------------------------
                DataTable dtGMaster = this.GetData("generalmaster", 0);
                this.PopulateTreeView("generalmaster", TreeView4, dtGMaster, 0, null);
                // --------------------------------------------------------
                TreeView1.Attributes.Add("onclick", "postBackByObject()");
                TreeView2.Attributes.Add("onclick", "postBackByObject()");
                TreeView3.Attributes.Add("onclick", "postBackByObject()");
                TreeView4.Attributes.Add("onclick", "postBackByObject()");
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                    }
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            if (Request.QueryString["mode"].ToString() == "view")
                                OnlyViewControls();
                        }
                    }
                }

            }
        }

        public void setLayout(string pMode)
        {
            ClearAllField();
            int TotalRecord = 0;

            List<Entity.Roles> lstRole = new List<Entity.Roles>();

            lstRole = BAL.RolesMgmt.GetRole(hdnpkID.Value, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalRecord);
            txtRoleID.Text = lstRole[0].RoleCode;
            txtRoleID.Enabled = false;
            txtRoleName.Text = lstRole[0].Description;
            txtComments.Text = lstRole[0].Comments;
            chkActive.Checked = lstRole[0].ActiveFlag;

            //---------For Menu ------------------
            rolerightslist = "";
            rolerightslist = BAL.RolesMgmt.GetRoleRights(hdnpkID.Value);
            if (rolerightslist.Trim() != "")
                checkAlreadySelected(TreeView1);
            //---------For Reports ------------------
            rolerightslist = "";
            rolerightslist = BAL.RolesMgmt.GetRoleReportRights(hdnpkID.Value);
            if (rolerightslist.Trim() != "")
                checkAlreadySelected(TreeView2);
            //---------For Reports ------------------
            rolerightslist = "";
            rolerightslist = BAL.RolesMgmt.GetRoleIconRights(hdnpkID.Value);
            if (rolerightslist.Trim() != "")
                checkAlreadySelected(TreeView3);

            //---------For General Master ------------------
            rolerightslist = "";
            rolerightslist = BAL.RolesMgmt.GetRoleGeneralMasterRights(hdnpkID.Value);
            if (rolerightslist.Trim() != "")
                checkAlreadySelected(TreeView4);

            //------------------------------------
            //DataSet ds = new DataSet();
            //ds.ReadXml(Server.MapPath("~/UserDetail.xml"));
            //DataTable dt = new DataTable();
            //for (int i = 0; i < ds.Tables.Count; i++)
            //{
            //    //int p = 0 ;
            //    if (ds.Tables[i].TableName.ToUpper() == lstRole[0].RoleCode.ToUpper())
            //    {
            //        dt = ds.Tables[i];
            //    }
            //}
        }

        public void OnlyViewControls()
        {
            txtRoleID.ReadOnly = true;
            txtRoleName.ReadOnly = true;
            txtComments.ReadOnly = true;

            btnSave.Enabled = false;
            btnReset.Enabled = false;

        }

        private DataTable GetData(string pModule, Int64 ParentId)
        {
            DataTable dt = new DataTable();
            List<Entity.ApplicationMenu> lstEntity = new List<Entity.ApplicationMenu>();
            lstEntity = BAL.ApplicationMenuMgmt.GetMenuByParent(pModule, ParentId);

            dt = ToDataTable(lstEntity);
            return dt;
        }

        //private DataTable GetDataReport(Int64 ParentId)
        //{
        //    DataTable dt = new DataTable();
        //    List<Entity.ApplicationMenu> lstEntity = new List<Entity.ApplicationMenu>();
        //    lstEntity = BAL.ApplicationMenuMgmt.GetMenuByParent("reports", ParentId);

        //    dt = ToDataTable(lstEntity);
        //    return dt;
        //}

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        private void PopulateTreeView(string pModule, TreeView myControl, DataTable dtParent, int parentId, TreeNode treeNode)
        {
            foreach (DataRow row in dtParent.Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = row["MenuText"].ToString(),
                    Value = row["pkId"].ToString(),
                    SelectAction = TreeNodeSelectAction.None
                };

                if (parentId == 0)
                {
                    myControl.Nodes.Add(child);
                }
                // ---------------------------------------------------------------------
                if (pModule != "icons" && pModule != "generalmaster")
                {
                    DataTable dtChild = this.GetData(pModule, Convert.ToInt64(child.Value));
                    if (dtChild.Rows.Count > 0)
                    {
                        if (parentId != 0)
                        {
                            treeNode.ChildNodes.Add(child);
                        }
                        PopulateTreeView(pModule, myControl, dtChild, int.Parse(child.Value), child);
                    }
                    else
                    {
                        if (parentId != 0)
                        {
                            treeNode.ChildNodes.Add(child);
                        }
                    }
                }
            }
        }

        private void getAllChildNodes(TreeNode node)
        {
            if (node.Checked == true && node.ChildNodes.Count > 0)
            {
                if (node.Checked == true)
                {
                    foreach (TreeNode child in node.ChildNodes)
                    {
                        child.Checked = true;
                        getAllChildNodes(child);
                    }
                }
            }
            else if (node.Checked == false && node.ChildNodes.Count > 0)
            {
                foreach (TreeNode child in node.ChildNodes)
                {
                    child.Checked = false;
                    getAllChildNodes(child);
                }
            }
        }
        private Boolean checkChiledNodeSelected(TreeNode node)
        {
            foreach (TreeNode child in node.ChildNodes)
            {
                if (child.Checked == true)
                    return true;
            }
            return false;
        }
        private void getAllParentNodes(TreeNode node)
        {
            if (node.Checked == true && node.Parent != null)
            {
                node.Parent.Checked = true;
                getAllParentNodes(node.Parent);
            }
            else if (node.Checked == false && node.Parent != null)
            {
                if (checkChiledNodeSelected(node.Parent) == false)
                    node.Parent.Checked = false;

                getAllParentNodes(node.Parent);
            }

        }
        protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.ChildNodes.Count > 0)
            {
                getAllChildNodes(e.Node);
            }
            else
            {
                getAllParentNodes(e.Node);
            }
        }
        protected void TreeView2_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.ChildNodes.Count > 0)
            {
                getAllChildNodes(e.Node);
            }
            else
            {
                getAllParentNodes(e.Node);
            }
        }
        protected void TreeView3_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.ChildNodes.Count > 0)
            {
                getAllChildNodes(e.Node);
            }
            else
            {
                getAllParentNodes(e.Node);
            }
        }

        protected void TreeView4_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.ChildNodes.Count > 0)
            {
                getAllChildNodes(e.Node);
            }
            else
            {
                getAllParentNodes(e.Node);
            }
        }

        protected void checkChildNodeSelected(TreeNode node)
        {
            if (node.ChildNodes.Count > 0)
            {
                foreach (TreeNode child in node.ChildNodes)
                {
                    if (chekNodeExist(child.Value))
                    {
                        child.Checked = true;
                        if (hdnpkID.Value.ToString() == "admin" && child.Checked == true && child.Text == "Manage Users")
                        {
                            child.Text = "<span id=disabledTreeviewNode>" + child.Text + "</span>";
                            Session["0"] = true;
                        }
                        else
                        {
                            Session["0"] = false; 
                        }
                        if (hdnpkID.Value.ToString() == "admin" && child.Checked == true && child.Text == "Manage Roles")
                        {
                            child.Text = "<span id=disabledTreeviewNode1>" + child.Text + "</span>";
                            Session["1"] = true;
                        }
                        else
                        {
                            Session["1"] = false;
                        }

                    }

                    checkChildNodeSelected(child);
                }

            }
        }
        protected void checkAlreadySelected(TreeView trvControl)
        {
            foreach (TreeNode node in trvControl.Nodes)
            {
                if (chekNodeExist(node.Value))
                    node.Checked = true;
                checkChildNodeSelected(node);
            }
        }
        protected Boolean chekNodeExist(string prolerights)
        {
            string[] values = rolerightslist.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                if (Convert.ToInt64(values[i]) == Convert.ToInt64(prolerights))
                {
                    return true;
                }
            }
            return false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtRoleID.Text == "" || txtRoleName.Text == "")
            {
                _pageValid = false;
                wowList.Style.Remove("color");
                wowList.Style.Add("color", "red");

                if (txtRoleID.Text == "")
                    wowList.Controls.Add(new LiteralControl("<li>" + "Role ID Required" + "</li>"));

                if (txtRoleName.Text == "")
                    wowList.Controls.Add(new LiteralControl("<li>" + "Role Name Required" + "</li>"));
            }


            if ((Session["Access"].ToString() != "") || (Session["NoAccess"].ToString() != ""))
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // ----------------------------------------------------------
                Entity.Roles objRole = new Entity.Roles();
                objRole.RoleCode = txtRoleID.Text.Trim();
                objRole.Description = txtRoleName.Text.Trim();
                objRole.Comments = txtComments.Text.Trim();
                objRole.ActiveFlag = chkActive.Checked;
                objRole.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.RolesMgmt.AddUpdateRoleDetail(objRole, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("~/UserDetail.xml"));

                XmlNodeList nodeList = xmlDoc.SelectNodes("/root/" + objRole.Description);
                var accesslist = Session["Access"].ToString().Split(',');
                var noaccesslist = Session["NoAccess"].ToString().Split(',');
                foreach (string value in accesslist)
                {
                    for (int i = 0; i < nodeList[0].ChildNodes.Count; i++)
                    {
                        if (nodeList[0].ChildNodes[i].Name == value)
                        {
                            nodeList[0].ChildNodes[i].InnerText = "1";
                        }
                    }
                }
                foreach (string value in noaccesslist)
                {
                    for (int i = 0; i < nodeList[0].ChildNodes.Count; i++)
                    {
                        if (nodeList[0].ChildNodes[i].Name == value)
                        {
                            nodeList[0].ChildNodes[i].InnerText = "0";
                        }
                    }
                }
                xmlDoc.Save(Server.MapPath("~/UserDetail.xml"));
                ScriptManager.RegisterStartupScript(this, typeof(string), "msg", "javascript:showmessage('" + ReturnMsg + "');", true);
                ClearAllField();
               
            }
            else
            {
                if (_pageValid)
                {
                    int ReturnCode = 0, ReturnCode1 = 0, ReturnCode2 = 0, ReturnCode11 = 0;
                    string ReturnMsg = "", ReturnMsg1 = "", ReturnMsg2 = "", ReturnMsg12 = "";

                    Entity.Roles objRole = new Entity.Roles();
                    objRole.RoleCode = txtRoleID.Text.Trim();
                    objRole.Description = txtRoleName.Text.Trim();
                    objRole.Comments = txtComments.Text.Trim();
                    objRole.ActiveFlag = chkActive.Checked;
                    objRole.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.RolesMgmt.AddUpdateRoleDetail(objRole, out ReturnCode, out ReturnMsg);
                    // --------------------------------------------------------------
                    //  Adding : Menu 
                    // --------------------------------------------------------------
                    if (ReturnCode > 0)
                    {
                        BAL.RolesMgmt.DeleteRoleRights(txtRoleID.Text.Trim(), out ReturnCode1, out ReturnMsg1);
                    }
                    if (ReturnCode1 > 0)
                    {
                        foreach (TreeNode node in TreeView1.CheckedNodes)
                        {
                            BAL.RolesMgmt.AddUpdateRoleRights(txtRoleID.Text.Trim(), node.Value, Session["LoginUserID"].ToString(), out ReturnCode2, out ReturnMsg2);
                        }
                    }

                    // --------------------------------------------------------------
                    //  Adding : Reports 
                    // --------------------------------------------------------------
                    if (ReturnCode > 0)
                    {
                        BAL.RolesMgmt.DeleteRoleReportRights(txtRoleID.Text.Trim(), out ReturnCode11, out ReturnMsg12);
                    }
                    if (ReturnCode11 > 0)
                    {
                        foreach (TreeNode node in TreeView2.CheckedNodes)
                        {
                            BAL.RolesMgmt.AddUpdateRoleReportRights(txtRoleID.Text.Trim(), node.Value, Session["LoginUserID"].ToString(), out ReturnCode11, out ReturnMsg12);
                        }
                    }

                    // --------------------------------------------------------------
                    //  Adding : Icons 
                    // --------------------------------------------------------------
                    if (ReturnCode > 0)
                    {
                        BAL.RolesMgmt.DeleteRoleIconRights(txtRoleID.Text.Trim(), out ReturnCode11, out ReturnMsg12);
                    }
                    if (ReturnCode11 > 0)
                    {
                        foreach (TreeNode node in TreeView3.CheckedNodes)
                        {
                            BAL.RolesMgmt.AddUpdateRoleIconRights(txtRoleID.Text.Trim(), node.Value, Session["LoginUserID"].ToString(), out ReturnCode11, out ReturnMsg12);
                        }
                    }
                    // --------------------------------------------------------------
                    //  Adding : General Master 
                    // --------------------------------------------------------------
                    if (ReturnCode > 0)
                    {
                        BAL.RolesMgmt.DeleteRoleGeneralMasterRights(txtRoleID.Text.Trim(), out ReturnCode11, out ReturnMsg12);
                    }
                    if (ReturnCode11 > 0)
                    {
                        foreach (TreeNode node in TreeView4.CheckedNodes)
                        {
                            BAL.RolesMgmt.AddUpdateGetRoleGeneralMasterRights(txtRoleID.Text.Trim(), node.Value, Session["LoginUserID"].ToString(), out ReturnCode11, out ReturnMsg12);
                        }
                    }

                    //---------------------------------
                    divErrorMessage.InnerHtml = ReturnMsg;
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Session["reset"] = "reset";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>window.localStorage.setItem(\"post\",\"reset\");</script>", false);
            ClearAllField();
        }

        public void ClearAllField()
        {
            Session["OldUserID"] = "";
            txtRoleID.Text = "";
            txtRoleID.Enabled = true;
            txtRoleName.Text = "";
            txtComments.Text = "";

            foreach (TreeNode node in TreeView1.Nodes)
            {
                node.Checked = false;
                getAllChildNodes(node);
            }

            foreach (TreeNode node in TreeView2.Nodes)
            {
                node.Checked = false;
                getAllChildNodes(node);
            }

            foreach (TreeNode node in TreeView3.Nodes)
            {
                node.Checked = false;
                getAllChildNodes(node);
            }
            foreach (TreeNode node in TreeView4.Nodes)
            {
                node.Checked = false;
                getAllChildNodes(node);
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteRole(string RoleCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.RolesMgmt.DeleteRoleDetail(RoleCode, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

    }
}