-- Write a SQL query to retrieve the top 3 products from the Products table 

Select * 
From (
Select ROW_NUMBER() OVER (ORDER BY Products.Id) AS RowNum, 
Products.Id, Products.ImageName, 
Products.BrandId, Products.Title, 
Brands.BrandName, Products.Price,
(Products.Price - Products.Discount) AS ActualPrice,
Categories.CategoryName, 
Products.Quantity
From Products 
Inner Join 
	Brands On Products.BrandId = Brands.Id                
Inner Join 
	Categories On Products.CategoryId = Categories.Id
Where Products.Quantity > 0  And 
	  Products.Price <= 1000000 
) As RowNum
WHERE RowNum >= 1 AND 
	  RowNum <= 3
ORDER BY RowNum


