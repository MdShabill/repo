CREATE PROCEDURE [dbo].[GetAllCustomers]
AS
BEGIN
    SELECT * FROM [dbo].[Customers];
END