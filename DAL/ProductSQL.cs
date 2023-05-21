using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ProductSQL:BaseSqlManager
    {
        public virtual List<Entity.ProductAssemblyStock> GetAssemblyStockSummary(string pStatus, string pOrderNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductAssemblyStockSummary";
            cmdGet.Parameters.AddWithValue("@Status", pStatus);
            cmdGet.Parameters.AddWithValue("@OrderNo", pOrderNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductAssemblyStock> lstObject = new List<Entity.ProductAssemblyStock>();
            while (dr.Read())
            {
                Entity.ProductAssemblyStock objEntity = new Entity.ProductAssemblyStock();
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.AssemblyID = GetInt64(dr, "AssemblyID");
                objEntity.AssemblyName = GetTextVale(dr, "AssemblyName");
                objEntity.AssemblyQty = GetDecimal(dr, "AssemblyQty");
                objEntity.RequiredQty = GetDecimal(dr, "RequiredQty");
                objEntity.ClosingSTK = GetDecimal(dr, "ClosingSTK");
                objEntity.BalanceQty = GetDecimal(dr, "BalanceQty");
                objEntity.BalanceStatus = GetTextVale(dr, "BalanceStatus");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        public virtual List<Entity.ProductAssemblyStock> GetAssemblyStockSummaryProductWise(string pStatus, Int64 ProductID, double Quantity)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductAssemblyStockSummaryProductWise";
            cmdGet.Parameters.AddWithValue("@Status", pStatus);
            cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
            cmdGet.Parameters.AddWithValue("@Quantity", Quantity);

            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductAssemblyStock> lstObject = new List<Entity.ProductAssemblyStock>();
            while (dr.Read())
            {
                Entity.ProductAssemblyStock objEntity = new Entity.ProductAssemblyStock();
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.AssemblyID = GetInt64(dr, "AssemblyID");
                objEntity.AssemblyName = GetTextVale(dr, "AssemblyName");
                objEntity.AssemblyQty = GetDecimal(dr, "AssemblyQty");
                objEntity.RequiredQty = GetDecimal(dr, "RequiredQty");
                objEntity.ClosingSTK = GetDecimal(dr, "ClosingSTK");
                objEntity.BalanceQty = GetDecimal(dr, "BalanceQty");
                objEntity.BalanceStatus = GetTextVale(dr, "BalanceStatus");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        
        public virtual List<Entity.Products> GetProductListForDropdown(string pProductName)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductListForDropdown";
            cmdGet.Parameters.AddWithValue("@ProductName", pProductName);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstObject = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductNameLongStk = GetTextVale(dr, "ProductNameLongStk");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Products> GetProductListForDropdown(string pProductName, string pSearchModule)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductListForDropdown";
            cmdGet.Parameters.AddWithValue("@ProductName", pProductName);
            cmdGet.Parameters.AddWithValue("@SearchModule", pSearchModule);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstObject = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductNameLongStk = GetTextVale(dr, "ProductNameLongStk");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Products> GetProductListForDropdownForMaterialIndent(string pProductName, string pSearchModule)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductListForDropdownForMaterialIndent";
            cmdGet.Parameters.AddWithValue("@ProductName", pProductName);
            cmdGet.Parameters.AddWithValue("@SearchModule", pSearchModule);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstObject = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductNameLongStk = GetTextVale(dr, "ProductNameLongStk");
                objEntity.BrandName = GetTextVale(dr, "BrandName");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Products> GetProductListForDropdown(string SerialKey,string pProductName, string pSearchModule, Int64 CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = (SerialKey == "HEXA-09PM-56JK-KG88") ? "ProductListForDropdownNew" : "ProductListForDropdown";
            cmdGet.Parameters.AddWithValue("@SerialKey", SerialKey);
            cmdGet.Parameters.AddWithValue("@ProductName", pProductName);
            cmdGet.Parameters.AddWithValue("@SearchModule", pSearchModule);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstObject = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductNameLongStk = GetTextVale(dr, "ProductNameLongStk");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Products> GetProductList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstObject = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.ProductType = GetTextVale(dr, "ProductType");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.UnitPrice = GetDecimal(dr, "UnitPrice");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductGroupName = GetTextVale(dr, "ProductGroupName");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.AddTaxPer = GetInt32(dr, "AddTaxPer");
                objEntity.ProductImage = (!String.IsNullOrEmpty(GetTextVale(dr, "ProductImage"))) ? GetTextVale(dr, "ProductImage") : "~/images/no-figure.png";
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.HSNCode = GetTextVale(dr, "HSNCode");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                
                objEntity.ManPower = GetDecimal(dr, "ManPower");
                objEntity.HorsePower = GetDecimal(dr, "HorsePower");
                objEntity.Min_UnitPrice = GetDecimal(dr, "Min_UnitPrice");
                objEntity.Max_UnitPrice = GetDecimal(dr, "Max_UnitPrice");
                objEntity.ProfitRatio = GetDecimal(dr, "ProfitRatio");
                objEntity.MinQuantity = GetDecimal(dr, "MinQuantity");

                objEntity.UnitQuantity = GetInt64(dr, "UnitQuantity");
                objEntity.UnitSize = GetTextVale(dr, "UnitSize");
                objEntity.UnitSurface = GetTextVale(dr, "UnitSurface");

                objEntity.UnitGrade = GetTextVale(dr, "UnitGrade");
                objEntity.Box_Weight = GetDecimal(dr, "Box_Weight");
                objEntity.Box_SQFT = GetDecimal(dr, "Box_SQFT");
                objEntity.Box_SQMT = GetDecimal(dr, "Box_SQMT");

                objEntity.OpeningSTK = GetDecimal(dr, "OpeningSTK");
                objEntity.InwardSTK = GetDecimal(dr, "InwardSTK");
                objEntity.OutwardSTK = GetDecimal(dr, "OutwardSTK");
                objEntity.ClosingSTK = GetDecimal(dr, "ClosingSTK");
                objEntity.OpeningValuation = GetDecimal(dr, "OpeningValuation");
                objEntity.OpeningWeightRate = GetDecimal(dr, "OpeningWeightRate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Products> GetProductList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Products> lstLocation = new List<Entity.Products>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.ProductType = GetTextVale(dr, "ProductType");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.UnitPrice = GetDecimal(dr, "UnitPrice");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductGroupName = GetTextVale(dr, "ProductGroupName");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.AddTaxPer = GetInt32(dr, "AddTaxPer");
                
                objEntity.ProductImage = (!String.IsNullOrEmpty(GetTextVale(dr, "ProductImage"))) ? GetTextVale(dr, "ProductImage") : "images/no-figure.png";
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.HSNCode = GetTextVale(dr, "HSNCode");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");

                objEntity.ManPower = GetDecimal(dr, "ManPower");
                objEntity.HorsePower = GetDecimal(dr, "HorsePower");
                objEntity.Min_UnitPrice = GetDecimal(dr, "Min_UnitPrice");
                objEntity.Max_UnitPrice = GetDecimal(dr, "Max_UnitPrice");
                objEntity.ProfitRatio = GetDecimal(dr, "ProfitRatio");
                objEntity.MinQuantity = GetDecimal(dr, "MinQuantity");

                objEntity.UnitQuantity = GetInt64(dr, "UnitQuantity");
                objEntity.UnitSize = GetTextVale(dr, "UnitSize");
                objEntity.UnitSurface = GetTextVale(dr, "UnitSurface");

                objEntity.UnitGrade = GetTextVale(dr, "UnitGrade");
                objEntity.Box_Weight = GetDecimal(dr, "Box_Weight");
                objEntity.Box_SQFT = GetDecimal(dr, "Box_SQFT");
                objEntity.Box_SQMT = GetDecimal(dr, "Box_SQMT");
                
                objEntity.OpeningSTK = GetDecimal(dr, "OpeningSTK");
                objEntity.InwardSTK = GetDecimal(dr, "InwardSTK");
                objEntity.OutwardSTK = GetDecimal(dr, "OutwardSTK");
                objEntity.ClosingSTK = GetDecimal(dr, "ClosingSTK");
                objEntity.OpeningValuation = GetDecimal(dr, "OpeningValuation");
                objEntity.OpeningWeightRate = GetDecimal(dr, "OpeningWeightRate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Products> GetProductList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Products> lstLocation = new List<Entity.Products>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.ProductType = GetTextVale(dr, "ProductType");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.UnitPrice = GetDecimal(dr, "UnitPrice");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductGroupName = GetTextVale(dr, "ProductGroupName");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.AddTaxPer = GetInt32(dr, "AddTaxPer");

                objEntity.ProductImage = (!String.IsNullOrEmpty(GetTextVale(dr, "ProductImage"))) ? GetTextVale(dr, "ProductImage") : "~/images/no-figure.png";
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.HSNCode = GetTextVale(dr, "HSNCode");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");

                objEntity.ManPower = GetDecimal(dr, "ManPower");
                objEntity.HorsePower = GetDecimal(dr, "HorsePower");
                objEntity.Min_UnitPrice = GetDecimal(dr, "Min_UnitPrice");
                objEntity.Max_UnitPrice = GetDecimal(dr, "Max_UnitPrice");
                objEntity.ProfitRatio = GetDecimal(dr, "ProfitRatio");
                objEntity.MinQuantity = GetDecimal(dr, "MinQuantity");

                objEntity.UnitQuantity = GetInt64(dr, "UnitQuantity");
                objEntity.UnitSize = GetTextVale(dr, "UnitSize");
                objEntity.UnitSurface = GetTextVale(dr, "UnitSurface");

                objEntity.UnitGrade = GetTextVale(dr, "UnitGrade");
                objEntity.Box_Weight = GetDecimal(dr, "Box_Weight");
                objEntity.Box_SQFT = GetDecimal(dr, "Box_SQFT");
                objEntity.Box_SQMT = GetDecimal(dr, "Box_SQMT");

                objEntity.OpeningSTK = GetDecimal(dr, "OpeningSTK");
                objEntity.InwardSTK = GetDecimal(dr, "InwardSTK");
                objEntity.OutwardSTK = GetDecimal(dr, "OutwardSTK");
                objEntity.ClosingSTK = GetDecimal(dr, "ClosingSTK");
                objEntity.OpeningValuation = GetDecimal(dr, "OpeningValuation");
                objEntity.OpeningWeightRate = GetDecimal(dr, "OpeningWeightRate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Products> GetProductList(string pProductName)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductListByName";
            cmdGet.Parameters.AddWithValue("@ProductName", pProductName);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstObject = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.ProductType = GetTextVale(dr, "ProductType");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.UnitPrice = GetDecimal(dr, "UnitPrice");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.AddTaxPer = GetInt32(dr, "AddTaxPer");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductGroupName = GetTextVale(dr, "ProductGroupName");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.HSNCode = GetTextVale(dr, "HSNCode");
                objEntity.ProductImage = (!String.IsNullOrEmpty(GetTextVale(dr, "ProductImage"))) ? GetTextVale(dr, "ProductImage") : "~/images/no-figure.png";
                objEntity.ManPower = GetDecimal(dr, "ManPower");
                objEntity.HorsePower = GetDecimal(dr, "HorsePower");
                objEntity.Min_UnitPrice = GetDecimal(dr, "Min_UnitPrice");
                objEntity.Max_UnitPrice = GetDecimal(dr, "Max_UnitPrice");
                objEntity.ProfitRatio = GetDecimal(dr, "ProfitRatio");
                objEntity.MinQuantity = GetDecimal(dr, "MinQuantity");

                objEntity.UnitQuantity = GetInt64(dr, "UnitQuantity");
                objEntity.UnitSize = GetTextVale(dr, "UnitSize");
                objEntity.UnitSurface = GetTextVale(dr, "UnitSurface");

                objEntity.UnitGrade = GetTextVale(dr, "UnitGrade");
                objEntity.Box_Weight = GetDecimal(dr, "Box_Weight");
                objEntity.Box_SQFT = GetDecimal(dr, "Box_SQFT");
                objEntity.Box_SQMT = GetDecimal(dr, "Box_SQMT");

                objEntity.OpeningSTK = GetDecimal(dr, "OpeningSTK");
                objEntity.InwardSTK = GetDecimal(dr, "InwardSTK");
                objEntity.OutwardSTK = GetDecimal(dr, "OutwardSTK");
                objEntity.ClosingSTK = GetDecimal(dr, "ClosingSTK");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Products> GetProductCategoryList(Int64 pkID)
        {
            SqlCommand cmdGet = new SqlCommand(); 
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select pd.pkID, pd.ProductID, pg.ProductGroupName from Quotation_Detail pd Left Join MST_Product pro on pro.pkID = pd.ProductID Left Join MST_ProductGroup pg on Pro.ProductGroupID = pg.pkID where pd.pkID = '" + pkID + "'";
           
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstObject = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductGroupName = GetTextVale(dr, "ProductGroupName");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateProduct(Entity.Products objEntity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnProductId)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Product_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ProductName", objEntity.ProductName);
            cmdAdd.Parameters.AddWithValue("@ProductAlias", objEntity.ProductAlias);
            cmdAdd.Parameters.AddWithValue("@ProductType", objEntity.ProductType);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@UnitPrice", objEntity.UnitPrice);
            cmdAdd.Parameters.AddWithValue("@TaxRate", objEntity.TaxRate);
            cmdAdd.Parameters.AddWithValue("@ProductSpecification", objEntity.ProductSpecification);
            cmdAdd.Parameters.AddWithValue("@ProductGroupID", objEntity.ProductGroupID);
            cmdAdd.Parameters.AddWithValue("@BrandID", objEntity.BrandID);
            cmdAdd.Parameters.AddWithValue("@TaxType", objEntity.TaxType);
            cmdAdd.Parameters.AddWithValue("@AddTaxPer", objEntity.AddTaxPer);
            cmdAdd.Parameters.AddWithValue("@UnitQuantity", objEntity.UnitQuantity);
            cmdAdd.Parameters.AddWithValue("@UnitSize", objEntity.UnitSize);
            cmdAdd.Parameters.AddWithValue("@UnitSurface", objEntity.UnitSurface);
            cmdAdd.Parameters.AddWithValue("@LRDate", objEntity.LRDate);
            cmdAdd.Parameters.AddWithValue("@UnitGrade", objEntity.UnitGrade);
            cmdAdd.Parameters.AddWithValue("@Box_Weight", objEntity.Box_Weight);
            cmdAdd.Parameters.AddWithValue("@Box_SQFT", objEntity.Box_SQFT);
            cmdAdd.Parameters.AddWithValue("@Box_SQMT", objEntity.Box_SQMT);

            if (!String.IsNullOrEmpty(objEntity.ProductImage))
                cmdAdd.Parameters.AddWithValue("@ProductImage", objEntity.ProductImage);
            cmdAdd.Parameters.AddWithValue("@ActiveFlag", objEntity.ActiveFlag);
            cmdAdd.Parameters.AddWithValue("@HSNCode", objEntity.HSNCode);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            cmdAdd.Parameters.AddWithValue("@ManPower",objEntity.ManPower);
            cmdAdd.Parameters.AddWithValue("@HorsePower",objEntity.HorsePower);
            cmdAdd.Parameters.AddWithValue("@Min_UnitPrice", objEntity.Min_UnitPrice);
            cmdAdd.Parameters.AddWithValue("@Max_UnitPrice", objEntity.Max_UnitPrice);
            cmdAdd.Parameters.AddWithValue("@ProfitRatio", objEntity.ProfitRatio);
            cmdAdd.Parameters.AddWithValue("@MinQuantity", objEntity.MinQuantity);
            cmdAdd.Parameters.AddWithValue("@OpeningSTK", objEntity.OpeningSTK);
            cmdAdd.Parameters.AddWithValue("@OpeningValuation", objEntity.OpeningValuation);
            cmdAdd.Parameters.AddWithValue("@OpeningWeightRate", objEntity.OpeningWeightRate);

            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnProductId", SqlDbType.BigInt);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnProductId = Convert.ToInt32(cmdAdd.Parameters["@ReturnProductId"].Value.ToString());
            ForceCloseConncetion();
        }

        // ============================= Insert & Update
        public virtual void AddUpdateProductUPDOWN(Entity.Products objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Product_INS_UPD_UPDOWN";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ProductName", objEntity.ProductName);
            cmdAdd.Parameters.AddWithValue("@ProductAlias", objEntity.ProductAlias);
            cmdAdd.Parameters.AddWithValue("@ProductGroupName", objEntity.ProductGroupName);
            cmdAdd.Parameters.AddWithValue("@BrandName", objEntity.BrandName);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@UnitPrice", objEntity.UnitPrice);
            cmdAdd.Parameters.AddWithValue("@TaxRate", objEntity.TaxRate);
            cmdAdd.Parameters.AddWithValue("@ProductSpecification", objEntity.ProductSpecification);
            cmdAdd.Parameters.AddWithValue("@AddTaxPer", objEntity.AddTaxPer);
            cmdAdd.Parameters.AddWithValue("@TaxType", objEntity.TaxType);
            cmdAdd.Parameters.AddWithValue("@ActiveFlag", objEntity.ActiveFlag);
            cmdAdd.Parameters.AddWithValue("@HSNCode", objEntity.HSNCode);
            cmdAdd.Parameters.AddWithValue("@ManPower", objEntity.ManPower);
            cmdAdd.Parameters.AddWithValue("@HorsePower", objEntity.HorsePower);
            cmdAdd.Parameters.AddWithValue("@ProductType", objEntity.ProductType);
            cmdAdd.Parameters.AddWithValue("@OpeningSTK", objEntity.OpeningSTK);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteProduct(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Product_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();           
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Section : Product Detail 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual List<Entity.ProductDetailCard> GetProductDetailList(Int64 pkID, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.ProductDetailCard> lstLocation = new List<Entity.ProductDetailCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.FinishProductNameLong = GetTextVale(dr, "FinishProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.AssQty = GetDecimal(dr, "AssQty");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.UnitPrice = GetDecimal(dr, "UnitPrice");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.ProductDetailCard> GetProductDetailListForProduction(Int64 pkID, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.ProductDetailCard> lstLocation = new List<Entity.ProductDetailCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select pkID,FinishProductID,ProductID,dbo.fnGetProductName(ProductID) as ProductName,dbo.fnGetProductNameLong(ProductID) as ProductNameLong,Quantity,Unit,Cast('' as nvarchar(2000)) as Remarks from MST_Product_Detail Where FinishProductID = " + FinishProductID;
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.Quantity = GetInt64(dr, "Quantity");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.ProductDetailCard> GetAssembly(Int64 FinishProductID)
        {
            List<Entity.ProductDetailCard> lstLocation = new List<Entity.ProductDetailCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductAssemblyList";
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.FinishProductNameLong = GetTextVale(dr, "FinishProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.Quantity = GetInt64(dr, "Quantity");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.UnitPrice = GetDecimal(dr, "UnitPrice");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateProductDetail(Entity.ProductDetailCard objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ProductDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }
        
        public virtual void DeleteProductDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductDetail_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteProductDetailByFinishProductID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductDetailByFinishProductID_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Section : Product Accessories
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual List<Entity.ProductDetailCard> GetProductAccessoriesList(Int64 pkID, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.ProductDetailCard> lstLocation = new List<Entity.ProductDetailCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductAccessoriesList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.FinishProductNameLong = GetTextVale(dr, "FinishProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.Quantity = GetInt64(dr, "Quantity");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateProductAccessories(Entity.ProductDetailCard objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ProductAccessories_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteProductAccessories(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductAccessories_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteProductAccessoriesByFinishProductID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductAccessoriesByFinishProductID_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Section : Product Specification For Quotation
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public virtual List<Entity.ProductDetailCard> GetQuotationProductSpecList(String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.ProductDetailCard> lstLocation = new List<Entity.ProductDetailCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationProductSpecList";
            cmdGet.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.FinishProductNameLong = GetTextVale(dr, "FinishProductNameLong");
                objEntity.GroupHead = GetTextVale(dr, "GroupHead");
                objEntity.MaterialHead = GetTextVale(dr, "MaterialHead");
                objEntity.MaterialRemarks = GetTextVale(dr, "MaterialRemarks");
                objEntity.MaterialSpec = GetTextVale(dr, "MaterialSpec");
                objEntity.ItemOrder = GetTextVale(dr, "ItemOrder");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateQuotationProductSpec(Entity.ProductDetailCard objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "QuotationProductSpec_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@GroupHead", objEntity.GroupHead);
            cmdAdd.Parameters.AddWithValue("@MaterialHead", objEntity.MaterialHead);
            cmdAdd.Parameters.AddWithValue("@MaterialSpec", objEntity.MaterialSpec);
            cmdAdd.Parameters.AddWithValue("@MaterialRemarks", objEntity.MaterialRemarks);
            cmdAdd.Parameters.AddWithValue("@ItemOrder", objEntity.ItemOrder);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteQuotationProductSpec(String QuotationNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationProductSpec_DEL";
            cmdDel.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            cmdDel.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual string DeleteUnwantedSpec(string pQuotationNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Delete From Quotation_ProductSpec Where QuotationNo = '" + pQuotationNo + "' And FinishProductID NOT IN (Select ProductID From Quotation_Detail qd Where QuotationNo = Quotation_ProductSpec.QuotationNo)";
            ExecuteNonQuery(myCommand);
            ForceCloseConncetion();
            return "";
        }

        public virtual string DeleteUnwantedSubsidy(string pQuotationNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Delete From Quotation_Subsidy Where QuotationNo = '" + pQuotationNo + "' And ProductID NOT IN (Select ProductID From Quotation_Detail qd Where QuotationNo = Quotation_Subsidy.QuotationNo)";
            ExecuteNonQuery(myCommand);
            ForceCloseConncetion();
            return "";

        }

        public virtual List<Entity.Products> GetProdSpecList(String Module,String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.Products> lstLocation = new List<Entity.Products>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductSpecsList";
            cmdGet.Parameters.AddWithValue("@Module", Module);
            cmdGet.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "ProductID");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Products> GetQuotProdSpecList(String QuotationNo, Int64 FinishProductID,  string LoginUserID)
        {
            List<Entity.Products> lstLocation = new List<Entity.Products>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationShortRemarkList";
            cmdGet.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "ProductID");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Products> GetQuotProdSpecList(String QuotationNo, Int64 FinishProductID,Int64 pkID, string LoginUserID)
        {
            List<Entity.Products> lstLocation = new List<Entity.Products>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationShortRemarkList";
            cmdGet.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "ProductID");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Products> GetSOQuotProdSpecList(String OrderNo,String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.Products> lstLocation = new List<Entity.Products>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderShortRemarkList";
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "ProductID");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Section : Product Specification For SalesOrder
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public virtual List<Entity.ProductDetailCard> GetSalesOrderProductSpecList(String OrderNo, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.ProductDetailCard> lstLocation = new List<Entity.ProductDetailCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderProductSpecList";
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.FinishProductNameLong = GetTextVale(dr, "FinishProductNameLong");
                objEntity.GroupHead = GetTextVale(dr, "GroupHead");
                objEntity.MaterialHead = GetTextVale(dr, "MaterialHead");
                objEntity.MaterialSpec = GetTextVale(dr, "MaterialSpec");
                objEntity.ItemOrder = GetTextVale(dr, "ItemOrder");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateSalesOrderProductSpec(Entity.ProductDetailCard objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderProductSpec_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@GroupHead", objEntity.GroupHead);
            cmdAdd.Parameters.AddWithValue("@MaterialHead", objEntity.MaterialHead);
            cmdAdd.Parameters.AddWithValue("@MaterialSpec", objEntity.MaterialSpec);
            cmdAdd.Parameters.AddWithValue("@ItemOrder", objEntity.ItemOrder);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteSalesOrderProductSpec(String OrderNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderProductSpec_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdDel.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual string DeleteUnwantedSalesOrderSpec(string pOrderNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Delete From SalesOrder_ProductSpec Where OrderNo = '" + pOrderNo + "' And FinishProductID NOT IN (Select ProductID From SalesOrder_Detail qd Where OrderNo = SalesOrder_ProductSpec.OrderNo)";
            ExecuteNonQuery(myCommand);
            ForceCloseConncetion();
            return "";
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Section : Product Specification For Dealer Sales Order
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public virtual List<Entity.ProductDetailCard> GetSalesOrderDealerProductSpecList(String OrderNo, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.ProductDetailCard> lstLocation = new List<Entity.ProductDetailCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerProductSpecList";
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.FinishProductNameLong = GetTextVale(dr, "FinishProductNameLong");
                objEntity.GroupHead = GetTextVale(dr, "GroupHead");
                objEntity.MaterialHead = GetTextVale(dr, "MaterialHead");
                objEntity.MaterialSpec = GetTextVale(dr, "MaterialSpec");
                objEntity.ItemOrder = GetTextVale(dr, "ItemOrder");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateSalesOrderDealerProductSpec(Entity.ProductDetailCard objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderDealerProductSpec_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@GroupHead", objEntity.GroupHead);
            cmdAdd.Parameters.AddWithValue("@MaterialHead", objEntity.MaterialHead);
            cmdAdd.Parameters.AddWithValue("@MaterialSpec", objEntity.MaterialSpec);
            cmdAdd.Parameters.AddWithValue("@ItemOrder", objEntity.ItemOrder);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteSalesOrderDealerProductSpec(String OrderNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderDealerProductSpec_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdDel.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual string DeleteUnwantedSalesOrderDealerSpec(string pOrderNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Delete From SalesOrderDealer_ProductSpec Where OrderNo = '" + pOrderNo + "' And FinishProductID NOT IN (Select ProductID From SalesOrderDealer_Detail qd Where OrderNo = SalesOrderDealer_ProductSpec.OrderNo)";
            ExecuteNonQuery(myCommand);
            ForceCloseConncetion();
            return "";
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Section : Product Specification For PurchaseOrder
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public virtual List<Entity.ProductDetailCard> GetPurchaseOrderProductSpecList(String OrderNo, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.ProductDetailCard> lstLocation = new List<Entity.ProductDetailCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderProductSpecList";
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.FinishProductNameLong = GetTextVale(dr, "FinishProductNameLong");
                objEntity.GroupHead = GetTextVale(dr, "GroupHead");
                objEntity.MaterialHead = GetTextVale(dr, "MaterialHead");
                objEntity.MaterialSpec = GetTextVale(dr, "MaterialSpec");
                objEntity.ItemOrder = GetTextVale(dr, "ItemOrder");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdatePurchaseOrderProductSpec(Entity.ProductDetailCard objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PurchaseOrderProductSpec_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@GroupHead", objEntity.GroupHead);
            cmdAdd.Parameters.AddWithValue("@MaterialHead", objEntity.MaterialHead);
            cmdAdd.Parameters.AddWithValue("@MaterialSpec", objEntity.MaterialSpec);
            cmdAdd.Parameters.AddWithValue("@ItemOrder", objEntity.ItemOrder);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeletePurchaseOrderProductSpec(String OrderNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PurchaseOrderProductSpec_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdDel.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual string DeleteUnwantedPurchaseOrderSpec(string pOrderNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Delete From PurchaseOrder_ProductSpec Where OrderNo = '" + pOrderNo + "' And FinishProductID NOT IN (Select ProductID From PurchaseOrder_Detail qd Where OrderNo = PurchaseOrder_ProductSpec.OrderNo)";
            ExecuteNonQuery(myCommand);
            ForceCloseConncetion();
            return "";
        }

        public virtual List<Entity.InwardOutwardLedger> GetInwardOutwardLedger(Int64 ProjectID, Int64 LocationID, Int64 pkID, string pLedgerType)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InwardOutwardLedger";
            cmdGet.Parameters.AddWithValue("@ProjectID", ProjectID);
            cmdGet.Parameters.AddWithValue("@LocationID", LocationID);
            cmdGet.Parameters.AddWithValue("@ProductID", pkID);
            cmdGet.Parameters.AddWithValue("@LedgerType", pLedgerType);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InwardOutwardLedger> lstObject = new List<Entity.InwardOutwardLedger>();
            while (dr.Read())
            {
                Entity.InwardOutwardLedger objEntity = new Entity.InwardOutwardLedger();
                objEntity.ProjectID = GetInt64(dr, "ProjectID");
                objEntity.LocationID = GetInt64(dr, "LocationID");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.TransType = GetTextVale(dr, "TransType");
                objEntity.Module = GetTextVale(dr, "Module");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.Amount = GetDecimal(dr, "Amount");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Section : Product Stock
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual List<Entity.ProductOpeningStk> GetProductOpeningStockList(Int64 pkID, string LoginUserID,string FinancialYear, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {            
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductOpeningStockList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@FinancialYear", FinancialYear);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductOpeningStk> lstObject = new List<Entity.ProductOpeningStk>();
            while (dr.Read())
            {
                Entity.ProductOpeningStk objEntity = new Entity.ProductOpeningStk();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.Quantity = GetDecimal(dr, "OpeningStock");
                objEntity.ProductGroupName = GetTextVale(dr, "ProductGroupName");
                objEntity.BrandName = GetTextVale(dr, "BrandName");


                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        /*-------------------------------------------------------------------------*/
        public virtual List<Entity.ProductImages> GetProductImageList(Int64 pkID, Int64 ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductImageList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductImages> lstLocation = new List<Entity.ProductImages>();
            while (dr.Read())
            {
                Entity.ProductImages objLocation = new Entity.ProductImages();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.ProductID = GetInt64(dr, "ProductID");
                objLocation.Name = GetTextVale(dr, "Name");
                objLocation.Type = GetTextVale(dr, "Type");
                objLocation.ContentData = null;
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateProductImages(Entity.ProductImages objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ProductImages_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Name", objEntity.Name);
            cmdAdd.Parameters.AddWithValue("@Type", objEntity.Type);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteProductImage(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductImages_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }
        public virtual void DeleteProductImageByProductID(Int64 ProductID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductImagesByProductID_DEL";
            cmdDel.Parameters.AddWithValue("@ProductID", ProductID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        //-----------------------------Serial No Filteration ----------------------------
        public virtual List<Entity.ProductDetailCard> GetSerialNoForDropdown(Int64 ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "FilterSerialNo";
            cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
            cmdGet.Parameters.AddWithValue("@Module", "");
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductDetailCard> lstObject = new List<Entity.ProductDetailCard>();
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.SerialNo = GetTextVale(dr, "SerialNo");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.ProductDetailCard> GetSerialNoForDropdown(Int64 ProductID, String Module)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "FilterSerialNo";
            cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
            cmdGet.Parameters.AddWithValue("@Module", Module);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductDetailCard> lstObject = new List<Entity.ProductDetailCard>();
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.SerialNo = GetTextVale(dr, "SerialNo");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        //public virtual List<Entity.ProductSerialNoHistory> GetSerialNoHistory(Int64 ProductID, String SerialNo, String FilterType)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "FilterSerialNoHistory";
        //    cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
        //    cmdGet.Parameters.AddWithValue("@SerialNo", SerialNo);
        //    cmdGet.Parameters.AddWithValue("@FilterType", FilterType);
        //    SqlDataReader dr = ExecuteDataReader(cmdGet);
        //    List<Entity.ProductSerialNoHistory> lstObject = new List<Entity.ProductSerialNoHistory>();
        //    while (dr.Read())
        //    {
        //        Entity.ProductSerialNoHistory objEntity = new Entity.ProductSerialNoHistory();
        //        objEntity.SerialNo = GetTextVale(dr, "SerialNo");
        //        objEntity.ProductID = GetInt64(dr, "ProductID");
        //        objEntity.ProductName = GetTextVale(dr, "ProductName");
        //        objEntity.PurcpkID = GetInt64(dr, "PurcpkID");
        //        objEntity.PurchaseBillNo = GetTextVale(dr, "PurchaseBillNo");
        //        objEntity.PurchaseBillDate = GetDateTime(dr, "PurchaseBillDate");
        //        if (FilterType != "inout")
        //        {
        //            objEntity.PurchaseExpiryDate = GetDateTime(dr, "PurchaseExpiryDate");
        //            if (objEntity.PurchaseExpiryDate == null || Convert.ToDateTime(objEntity.PurchaseExpiryDate).Year <= 1990)
        //                objEntity.PurchaseExpiryDays = 0;
        //            else
        //                objEntity.PurchaseExpiryDays = Convert.ToInt64((Convert.ToDateTime(GetDateTime(dr, "PurchaseExpiryDate")) - DateTime.Now).TotalDays);
        //        }
        //        objEntity.PurchaseCustomer = GetTextVale(dr, "PurchaseCustomer");
        //        objEntity.SalepkID = GetInt64(dr, "SalepkID");
        //        objEntity.SalesBillNo = GetTextVale(dr, "SalesBillNo");
        //        objEntity.SalesBillDate = GetDateTime(dr, "SalesBillDate");
        //        objEntity.SalesCustomer = GetTextVale(dr, "SalesCustomer");

        //        lstObject.Add(objEntity);
        //    }
        //    dr.Close();
        //    ForceCloseConncetion();
        //    return lstObject;
        //}
        public virtual List<Entity.ProductDetailCard> GetProductSerialNoListByInvoiceNo(String Module, String InvoiceNo, Int64 ProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductSerialNoListByInvoiceNo";

            cmdGet.Parameters.AddWithValue("@Module", Module);
            cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductDetailCard> lstObject = new List<Entity.ProductDetailCard>();
            while (dr.Read())
            {
                Entity.ProductDetailCard objEntity = new Entity.ProductDetailCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Module = GetTextVale(dr, "Module");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.SerialNo = GetTextVale(dr, "SerialNo");
                objEntity.RefNo = GetTextVale(dr, "RefNo");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        //------------------------------------Product Hexagon------------------------------------------------------

        public virtual List<Entity.Products> GetProductHexagonList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstObject = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Products> GetProductHexagonList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Products> lstLocation = new List<Entity.Products>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductAlias = GetTextVale(dr, "ProductAlias");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateProductHexagon(Entity.Products objEntity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnProductId)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Product_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ProductName", objEntity.ProductName);
            cmdAdd.Parameters.AddWithValue("@ProductAlias", objEntity.ProductAlias);
            cmdAdd.Parameters.AddWithValue("@BrandID", objEntity.BrandID);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);

            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnProductId", SqlDbType.BigInt);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnProductId = Convert.ToInt32(cmdAdd.Parameters["@ReturnProductId"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteProductHexagon(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Product_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

    }
}
