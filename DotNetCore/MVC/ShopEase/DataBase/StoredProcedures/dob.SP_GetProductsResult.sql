CREATE PROCEDURE SP_GetProductsResult
    @title NVARCHAR(200) = NULL,
    @brandId INT = NULL,
    @minPrice DECIMAL = NULL,
    @maxPrice DECIMAL = NULL,
    @categoryId INT = NULL
AS
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