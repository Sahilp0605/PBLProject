using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace StarsProject
{
    public partial class ShowMapInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["PkID"].ToString()))
            {
                hdnPkID.Value = Request.QueryString["PkID"].ToString();
                // ----------------------------------------------------------------------------------------
                //List<Entity.HelpLog> lstEntity = new List<Entity.HelpLog>();
                 
                // -------------------------------------------------------------------------
                //lstEntity = BAL.HelpLogMgmt.GetHelpLogListByPkID(Convert.ToInt64(hdnPkID.Value));
                //hdnRegistrationNo.Value = lstEntity[0].RegistrationNo;
                //txtRegistrationNo.Text = lstEntity[0].RegistrationNo;

                //hdnLatitude.Value = lstEntity[0].Latitude.ToString();
                //hdnLatitudeDirection.Value = lstEntity[0].LatitudeDirection;
                //hdnLongitude.Value = lstEntity[0].Longitude.ToString();
                //hdnLongitudeDirection.Value = lstEntity[0].LongitudeDirection;
                //DataTable tblObject = lstEntity.ToDataTable(lstEntity);
                // ----------------------------------------------------------------------------------------
//                String Locations = "";
//                Entity.HelpLog objHelpLog = new Entity.HelpLog();
//                foreach (var item in lstEntity)
//                {
//                    if (item.Longitude.ToString().Trim().Length == 0)
//                        continue;

//                    string Latitude = item.Latitude.ToString();
//                    string Longitude = item.Longitude.ToString();

//                    // create a line of JavaScript for marker on map for this record 
//                    Locations += Environment.NewLine + " map.addOverlay(new GMarker(new GLatLng(" + Latitude + "," + Longitude + ")));";
//                }

//                // construct the final script
//                js.Text = @"<script type='text/javascript'>
//                            function initialize() {
//                            if (GBrowserIsCompatible()) {
//                                var map = new GMap2(document.getElementById('map_canvas'));
//                            map.setCenter(new GLatLng(51.5,-0.1167), 2); 
//                            " + Locations + @"
//                            map.setUIToDefault();
//                            }
//                        }
//                    </script> ";
            }
        }

        // ----------------------------------------------------------------------------

        private void BuildScript(DataTable tbl)
        {
            String Locations = "";
            foreach (DataRow r in tbl.Rows)
            {
                // bypass empty rows 
                if (r["Latitude"].ToString().Trim().Length == 0)
                    continue;

                    string Latitude = r["Latitude"].ToString();
                    string Longitude = r["Longitude"].ToString();

                // create a line of JavaScript for marker on map for this record 
                Locations += Environment.NewLine + " map.addOverlay(new GMarker(new GLatLng(" + Latitude + "," + Longitude + ")));";
            }

            // construct the final script
            js.Text = @"<script type='text/javascript'>
                            function initialize() {
                            if (GBrowserIsCompatible()) {
                                var map = new GMap2(document.getElementById('map_canvas'));
                            map.setCenter(new GLatLng(51.5,-0.1167), 2); 
                            " + Locations + @"
                            map.setUIToDefault();
                            }
                        }
                    </script> ";
        }
    }
}