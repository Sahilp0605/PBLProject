using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Sharvaya_Authorization : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }
        }

        string provider = "RSAProtectedConfigurationProvider";
        string section = "connectionStrings";

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration confg = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                ConfigurationSection confStrSect = confg.GetSection(section);
                if (confStrSect != null)
                {
                    confStrSect.SectionInformation.ProtectSection(provider);
                    confg.Save();
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Encrypted Successfully...!','toast-success');", true);
                divAuthorization.Visible = false;
                divLogin.Visible = true;
            }
            catch (Exception ex)
            {
                string Errormsg = ex.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Some Error In process...!','toast-danger');", true);
            }
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration confg = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                ConfigurationSection confStrSect = confg.GetSection(section);
                if (confStrSect != null && confStrSect.SectionInformation.IsProtected)
                {
                    confStrSect.SectionInformation.UnprotectSection();
                    confg.Save();
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Decrypted Successfully...!','toast-success');", true);
                divAuthorization.Visible = false;
                divLogin.Visible = true;
            }
            catch (Exception ex)
            {
                string Errormsg = ex.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Some Error In process...!','toast-danger');", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if(txtpassword.Text.Trim() == "Sharvaya123!@#")
            {
                divAuthorization.Visible = true;
                divLogin.Visible = false;
            }
            else
            {
                divAuthorization.Visible = false;
                divLogin.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Password Is Invalid...!!!!','toast-danger');", true);

            }
        }
    }
}