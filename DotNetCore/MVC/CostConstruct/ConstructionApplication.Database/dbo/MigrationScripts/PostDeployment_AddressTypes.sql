-- =============================================
-- Check if AddressTypes table exists
-- If NOT, then create and insert
-- =============================================
IF OBJECT_ID('[dbo].[AddressTypes]', 'U') IS NULL
BEGIN
    -- Create the table
    CREATE TABLE [dbo].[AddressTypes] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL
    );
END

-- Insert data using MERGE (only if table is empty)
IF NOT EXISTS (SELECT 1 FROM [dbo].[AddressTypes])
BEGIN
    MERGE INTO [dbo].[AddressTypes] AS Target
    USING (VALUES
        (N'Home'),
        (N'Office'),
        (N'Apartment'),
        (N'Factory'),
        (N'FarmHouse')
    ) AS Source ([Name])
    ON Target.[Name] = Source.[Name]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Name]) VALUES (Source.[Name]);
END