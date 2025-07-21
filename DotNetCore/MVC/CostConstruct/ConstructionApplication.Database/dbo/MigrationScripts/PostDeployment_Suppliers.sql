-- =============================================
-- Check if Suppliers table exists
-- If NOT, then create and insert
-- =============================================
IF OBJECT_ID('[dbo].[Suppliers]', 'U') IS NULL
BEGIN
    -- Create the table
    CREATE TABLE [dbo].[Suppliers] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(255) NOT NULL,
        [PhoneNumber] NVARCHAR(200) NULL,
        [Email] NVARCHAR(100) NULL,
        [Address] NVARCHAR(255) NULL
    );
END

-- Insert data using MERGE (only if table is empty)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Suppliers])
BEGIN
    MERGE INTO [dbo].[Suppliers] AS Target
    USING (VALUES
        (N'ZamZam', N'9845665640', N'zamzam@gmail.com', N'Dholi Sakra, Railway Sattion, Muzaffarpur, Bihar'),
        (N'Aashiyana Hardware', N'8899227710', N'aashiyana@gmail.com', N'Sujawalpur Chowk, Dholi, Muzaffarpur, Bihar'),
        (N'Sankar Electronic', N'9334391268', N'sankar@gmail.com', N'Tilak Maidan, MuzaffarPur, Bihar'),
        (N'Maruf Hindustan Hardware', N'7766889400', N'mukhiya@gmail.com', N'SujawalPur, Dholi Skara, Muzaffarpur, Bihar')
    ) AS Source ([Name], [PhoneNumber], [Email], [Address])
    ON Target.[Name] = Source.[Name] -- matching only on Name
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Name], [PhoneNumber], [Email], [Address])
        VALUES (Source.[Name], Source.[PhoneNumber], Source.[Email], Source.[Address]);
END