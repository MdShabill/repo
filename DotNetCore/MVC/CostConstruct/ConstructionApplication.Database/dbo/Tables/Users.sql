CREATE TABLE [dbo].[Users]
(
	[Id] INT Identity(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Gender] INT NOT NULL, 
    [Email] NVARCHAR(200) NOT NULL, 
    [Password] NVARCHAR(200) NULL, 
    [MobileNumber] NVARCHAR(200) NULL, 
    [LastFailedLoginDate] DATETIME NULL, 
    [LastSuccessFulLoginDate] DATETIME NULL, 
    [LoginFailedCount] INT NULL, 
    [IsLocked] BIT NULL
)
