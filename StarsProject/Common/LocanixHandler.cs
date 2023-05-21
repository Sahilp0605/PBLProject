using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LocanixHandler
/// </summary>
public class LocanixHandler : IHttpHandler
{
    public LocanixHandler()
    {
    }

    #region IhttpHandler Members
    public virtual void ProcessRequest(HttpContext context)
    {
        this.Context = context;

        GpsPacket packet = new GpsPacket();

        packet.DeviceId = this.DeviceId;
        packet.PositionTimeStamp = this.PositionTimeStamp;
        packet.Longitude = this.Longitude;
        packet.Latitude = this.Latitude;
        packet.Heading = this.Heading;
        packet.Speed = this.Speed;

        packet.Save();
    }

    public bool IsReusable
    {
        get { return true; }
    }

    protected HttpContext Context { get; set; }
    #endregion

    #region GPS Data Members

    protected string DeviceId
    {
        get
        {
            return this.Context.Request.QueryString["DeviceId"];
        }
    }
  
    protected DateTime PositionTimeStamp
    {
        get
        {
            string eventTime = this.Context.Request.QueryString["ts"].Replace("T", " ");

            return DateTime.ParseExact(eventTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).AddHours(5).AddMinutes(30);
        }
    }

    protected double Longitude
    {
        get
        {
            try
            {
                return Convert.ToDouble(this.Context.Request.QueryString["Longitude"]);
            }
            catch { return 0; }
        }
    }

    protected double Latitude
    {
        get
        {
            try
            {
                return Convert.ToDouble(this.Context.Request.QueryString["Latitude"]);
            }
            catch { return 0; }
        }
    }

    protected double Speed
    {
        get
        {
            try
            {
                return Convert.ToDouble(this.Context.Request.QueryString["Speed"]);
            }
            catch { return 0; }
        }
    }

    protected double Heading
    {
        get
        {
            try
            {
                return Convert.ToDouble(this.Context.Request.QueryString["Heading"]);
            }
            catch { return 0; }
        }
    }

    #endregion
}