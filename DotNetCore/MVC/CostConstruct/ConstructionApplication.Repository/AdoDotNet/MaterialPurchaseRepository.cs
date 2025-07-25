﻿using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.MaterialPurchase;
using ConstructionApplication.Repository.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class MaterialPurchaseRepository : IMaterialPurchaseRepository
    {
        private readonly string _connectionString;

        public MaterialPurchaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<MaterialPurchase> GetAll(int siteId, DateTime? DateFrom, DateTime? DateTo, int? MaterialId, int? SupplierId, int? BrandId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT MaterialPurchase.Id,
                       Materials.Name As MaterialName, Suppliers.Name As SupplierName, 
                       Suppliers.PhoneNumber, Brands.Name As BrandName, MaterialPurchase.Quantity, 
                       MaterialPurchase.UnitOfMeasure, Materials.UnitPrice, MaterialPurchase.Date,
                       MaterialPurchase.MaterialCost, MaterialPurchase.DeliveryCharge
                       FROM MaterialPurchase
                       Inner Join Materials On MaterialPurchase.MaterialId = Materials.Id                
                       Inner Join Suppliers On MaterialPurchase.SupplierId = Suppliers.Id                    
                       Inner Join Brands On MaterialPurchase.BrandId = Brands.Id
                       Where MaterialPurchase.SiteId = @SiteId
                             And
                             (@DateFrom IS NULL OR MaterialPurchase.Date >= @DateFrom) 
                             AND 
                             (@DateTo IS NULL OR MaterialPurchase.Date <= @DateTo)
                             AND 
                             (@MaterialId IS NULL OR MaterialPurchase.MaterialId = @MaterialId)
                             AND
                             (@SupplierId IS NULL OR MaterialPurchase.SupplierId = @SupplierId)
                             AND
                             (@BrandId IS NULL OR MaterialPurchase.BrandId = @BrandId)
                       Order By MaterialPurchase.Date DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@SiteId", siteId);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@DateFrom", (object)DateFrom ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@DateTo", (object)DateTo ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@MaterialId", (object)MaterialId ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@SupplierId", (object)SupplierId ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@BrandId", (object)BrandId ?? DBNull.Value);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<MaterialPurchase> materialPurchases = new();
                //Approach: 1 = With For Loop
                //for (int i = 0; i < dataTable.Rows.Count; i++)
                //{
                //    MaterialPurchase materialPurchase = new()
                //    {
                //        Id = (int)dataTable.Rows[i]["Id"],
                //        MaterialName = (string)dataTable.Rows[i]["MaterialName"],
                //        SupplierName = (string)dataTable.Rows[i]["SupplierName"],
                //        PhoneNumber = (string)dataTable.Rows[i]["PhoneNumber"],
                //        BrandName = (string)dataTable.Rows[i]["BrandName"],
                //        Quantity = (int)dataTable.Rows[i]["Quantity"],
                //        UnitOfMeasure = (string)dataTable.Rows[i]["UnitOfMeasure"],
                //        UnitPrice = (decimal)dataTable.Rows[i]["UnitPrice"],
                //        Date = (DateTime)dataTable.Rows[i]["Date"],
                //        MaterialCost = (decimal)dataTable.Rows[i]["MaterialCost"],
                //        DeliveryCharge = (decimal)dataTable.Rows[i]["DeliveryCharge"],
                //    };
                //    materialPurchases.Add(materialPurchase);
                //}

                //Approach: 2 = With For Each Loop
                foreach (DataRow row in dataTable.Rows)
                {
                    MaterialPurchase materialPurchase = new()
                    {
                        Id = (int)row["Id"],
                        MaterialName = (string)row["MaterialName"],
                        SupplierName = (string)row["SupplierName"],
                        PhoneNumber = (string)row["PhoneNumber"],
                        BrandName = (string)row["BrandName"],
                        Quantity = (int)row["Quantity"],
                        UnitOfMeasure = (string)row["UnitOfMeasure"],
                        UnitPrice = (decimal)row["UnitPrice"],
                        Date = (DateTime)row["Date"],
                        MaterialCost = (decimal)row["MaterialCost"],
                        DeliveryCharge = (decimal)row["DeliveryCharge"],
                    };
                    materialPurchases.Add(materialPurchase);
                }
                return materialPurchases;
            }
        }

        public int Create(MaterialPurchase materialPurchase)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into MaterialPurchase
                       (MaterialId, SupplierId, SiteId, PhoneNumber, BrandId, Quantity, UnitOfMeasure, Date, MaterialCost, DeliveryCharge)
                       Values
                       (@materialId, @supplierId, @siteId, @phoneNumber, @brandId, @quantity, @unitOfMeasure, @date, @materialCost, @deliveryCharge) ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@materialId", materialPurchase.MaterialId);
                sqlCommand.Parameters.AddWithValue("@supplierId", materialPurchase.SupplierId);
                sqlCommand.Parameters.AddWithValue("@siteId", materialPurchase.SiteId);
                sqlCommand.Parameters.AddWithValue("@phoneNumber", materialPurchase.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("@brandId", materialPurchase.BrandId);
                sqlCommand.Parameters.AddWithValue("@quantity", materialPurchase.Quantity);
                sqlCommand.Parameters.AddWithValue("@unitOfMeasure", materialPurchase.UnitOfMeasure);
                sqlCommand.Parameters.AddWithValue("@date", materialPurchase.Date);
                sqlCommand.Parameters.AddWithValue("@materialCost", materialPurchase.MaterialCost);
                sqlCommand.Parameters.AddWithValue("@deliveryCharge", materialPurchase.DeliveryCharge);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteServiceProviderQuery = "DELETE FROM MaterialPurchase WHERE Id = @Id";
                SqlCommand deleteMaterialPurchaseCommand = new(deleteServiceProviderQuery, sqlConnection);
                deleteMaterialPurchaseCommand.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                deleteMaterialPurchaseCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
