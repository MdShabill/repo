CREATE PROCEDURE Sp_MaterialPurchaseCRUD
    @Mode NVARCHAR(50),
    @Id INT = NULL,
    @MaterialId INT = NULL,
    @SupplierId INT = NULL,
    @PhoneNumber NVARCHAR(20) = NULL,
    @BrandId INT = NULL,
    @Quantity INT = NULL,
    @UnitOfMeasure NVARCHAR(50) = NULL,
    @Date DATE = NULL,
    @MaterialCost DECIMAL(18, 2) = NULL,
    @DeliveryCharge DECIMAL(18, 2) = NULL,
    @DateFrom DATE = NULL,
    @DateTo DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Mode = 'Get_All'
    BEGIN
        SELECT MaterialPurchase.Id, 
			   Materials.Name As MaterialName, Suppliers.Name As SupplierName, 
			   Suppliers.PhoneNumber, Brands.Name As BrandName, MaterialPurchase.Quantity, 
			   MaterialPurchase.UnitOfMeasure, Materials.UnitPrice, MaterialPurchase.Date,
			   MaterialPurchase.MaterialCost, MaterialPurchase.DeliveryCharge
			   FROM MaterialPurchase
			   Inner Join Materials On MaterialPurchase.MaterialId = Materials.Id                
			   Inner Join Suppliers On MaterialPurchase.SupplierId = Suppliers.Id                    
			   Inner Join Brands On MaterialPurchase.BrandId = Brands.Id
        Where (@DateFrom IS NULL OR MaterialPurchase.Date >= @DateFrom) 
              AND 
              (@DateTo IS NULL OR MaterialPurchase.Date <= @DateTo)
              AND 
              (@MaterialId IS NULL OR MaterialPurchase.MaterialId = @MaterialId)
              AND
              (@SupplierId IS NULL OR MaterialPurchase.SupplierId = @SupplierId)
              AND
              (@BrandId IS NULL OR MaterialPurchase.BrandId = @BrandId)
        Order By MaterialPurchase.Date DESC
    END

    ELSE IF @Mode = 'Create'
    BEGIN
        INSERT INTO MaterialPurchase
            (MaterialId, SupplierId, PhoneNumber, BrandId, Quantity, 
             UnitOfMeasure, Date, MaterialCost, DeliveryCharge)
        VALUES
            (@MaterialId, @SupplierId, @PhoneNumber, @BrandId, @Quantity, 
             @UnitOfMeasure, @Date, @MaterialCost, @DeliveryCharge)

			 SELECT @@ROWCOUNT AS AffectedRows
    END

    ELSE IF @Mode = 'Delete'
    BEGIN
        DELETE FROM MaterialPurchase WHERE Id = @Id
        SELECT @@ROWCOUNT AS AffectedRows
    END
END
