-- =============================================
-- Check if MaterialTypes table exists
-- If NOT, then create and insert
-- =============================================
IF OBJECT_ID('[dbo].[MaterialTypes]', 'U') IS NULL
BEGIN
    -- Create the table
    CREATE TABLE [dbo].[MaterialTypes] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] VARCHAR(100) NOT NULL
    );
END

-- Insert data using MERGE (only if table is empty)
IF NOT EXISTS (SELECT 1 FROM [dbo].[MaterialTypes])
BEGIN
    MERGE INTO [dbo].[MaterialTypes] AS Target
    USING (VALUES
        ('Paint'),
        ('Cement'),
        ('Steel'),
        ('Bricks'),
        ('Electrical'),
        ('Plumbing'),
        ('Tiles'),
        ('Sand'),
        ('Crushed Stone'),
        ('Sanitary')
    ) AS Source ([Name])
    ON Target.[Name] = Source.[Name]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Name]) VALUES (Source.[Name]);
END