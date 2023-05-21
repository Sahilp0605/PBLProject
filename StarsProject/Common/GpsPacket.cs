using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GpsPacket
/// </summary>
public class GpsPacket
{
    public GpsPacket() { }

    public string DeviceId { get; internal set; }
    public DateTime PositionTimeStamp { get; internal set; }
    public double Latitude { get; internal set; }
    public double Longitude { get; internal set; }
    public double Speed { get; internal set; }
    public double Heading { get; internal set; }

    public void Save()
    {
        try
        {
            //Add your code to save the Gps Packet in the Database here.
        }
        catch (Exception ex)
        {            
            throw;
        }
    }
}