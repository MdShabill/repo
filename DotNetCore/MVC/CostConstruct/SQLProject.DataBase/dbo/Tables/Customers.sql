CREATE TABLE [dbo].[Customers] (
    [CustomerId] INT PRIMARY KEY IDENTITY(1,1),
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE()
);