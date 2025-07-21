-- =============================================
-- Check if Countries table exists
-- If NOT, then create and insert
-- =============================================
IF OBJECT_ID('[dbo].[Countries]', 'U') IS NULL
BEGIN
    -- Create the table
    CREATE TABLE [dbo].[Countries] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL
    );
END

-- Insert data using MERGE (only if table is empty)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Countries])
BEGIN
    MERGE INTO [dbo].[Countries] AS Target
    USING (VALUES
        (N'US'),
        (N'UAE'),
        (N'India'),
        (N'England'),
        (N'Pakistan'),
        (N'Qatar')
    ) AS Source ([Name])
    ON Target.[Name] = Source.[Name]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Name]) VALUES (Source.[Name]);
END