USE [ShopEaseDB]
GO

/****** Object: StoredProcedure [dbo].[SP_GetProductResult] Script Date: 21-01-2024 21:38:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetProductResult]
    @Mode INT,
    @ProductId INT = NULL,
	@title NVARCHAR(200) = NULL,
    @brandId INT = NULL,
    @minPrice DECIMAL = NULL,
    @maxPrice DECIMAL = NULL,
    @categoryId INT = NULL
AS
BEGIN
    IF @Mode = 1
    BEGIN
        SELECT
            Products.Id,
            Products.Title,
            Categories.CategoryName,
            Products.BrandId,
            Brands.BrandName,
            Products.Price,
            Products.Discount,
            (Products.Price - Products.Discount) AS ActualPrice,
            Suppliers.SupplierName,
            Products.Quantity,
            Products.ImageName
        FROM
            Products
            INNER JOIN Brands ON Products.BrandId = Brands.Id
            INNER JOIN Categories ON Products.CategoryId = Categories.Id
            INNER JOIN Suppliers ON Products.SupplierId = Suppliers.Id
        WHERE
            Products.Id = @ProductId;
    END
    ELSE IF @Mode = 2
    BEGIN
        SELECT 
            Products.Id,
            Products.ImageName,
            Products.BrandId,
            Products.Title,
            Brands.BrandName,
            Products.Price,
            (Products.Price - Products.Discount) AS ActualPrice,
            Categories.CategoryName,
            Products.Quantity
        FROM 
            Products 
        INNER JOIN 
            Brands ON Products.BrandId = Brands.Id                
        INNER JOIN 
            Categories ON Products.CategoryId = Categories.Id
        WHERE 
            Products.Quantity > 0
            AND (@title IS NULL OR Products.Title LIKE '%' + @title + '%')
            AND (@brandId IS NULL OR Products.BrandId = @brandId)
            AND (@minPrice IS NULL OR Products.Price >= @minPrice)
            AND (@maxPrice IS NULL OR Products.Price <= @maxPrice)
            AND (@categoryId IS NULL OR Products.CategoryId = @categoryId);
    END
END
GO