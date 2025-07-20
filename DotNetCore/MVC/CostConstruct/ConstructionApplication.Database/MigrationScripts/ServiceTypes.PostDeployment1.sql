-- =============================================
-- Check if ServiceTypes table exists
-- If NOT, then create and insert
-- =============================================
IF OBJECT_ID('[dbo].[ServiceTypes]', 'U') IS NULL
BEGIN
    -- Create the table
    CREATE TABLE [dbo].[ServiceTypes] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(200) NOT NULL
    );

    -- Insert data using MERGE (only if table is empty)
    IF NOT EXISTS (SELECT 1 FROM [dbo].[ServiceTypes])
    BEGIN
        MERGE INTO [dbo].[ServiceTypes] AS Target
        USING (VALUES
            (N'Master Mason'),
            (N'Labour'),
            (N'Electrician'),
            (N'Electrician Assistant'),
            (N'Plumber'),
            (N'Plumber Assistant'),
            (N'Painter'),
            (N'Painter Helper'),
            (N'Carpenter'),
            (N'Carpenter Assistant'),
            (N'Tiler'),
            (N'Tiler Assistant')
        ) AS Source ([Name])
        ON Target.[Name] = Source.[Name]
        WHEN NOT MATCHED BY TARGET THEN
            INSERT ([Name]) VALUES (Source.[Name]);
    END
END
