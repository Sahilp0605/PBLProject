using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class VehicleMgmt
    {
        public static List<Entity.Vehicle> GetVehicleList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VehicleSQL().GetVehicleList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Vehicle> GetVehicleList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VehicleSQL().GetVehicleList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateVehicle(Entity.Vehicle entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VehicleSQL().AddUpdateVehicle(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteVehicle(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VehicleSQL().DeleteVehicle(pkID, out ReturnCode, out ReturnMsg);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.VehicleTrip> GetVehicleTripList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VehicleSQL().GetVehicleTripList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.VehicleTrip> GetVehicleTripList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VehicleSQL().GetVehicleTripList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateVehicleTrip(Entity.VehicleTrip entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VehicleSQL().AddUpdateVehicleTrip(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteVehicleTrip(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VehicleSQL().DeleteVehicleTrip(pkID, out ReturnCode, out ReturnMsg);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.Vehicle> GetTransporterList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VehicleSQL().GetTransporterList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Vehicle> GetTransporterList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VehicleSQL().GetTransporterList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateTransporter(Entity.Vehicle entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VehicleSQL().AddUpdateTransporter(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteTransporter(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VehicleSQL().DeleteTransporter(pkID, out ReturnCode, out ReturnMsg);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
    }
}
