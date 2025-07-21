-- =============================================
-- Check if SiteStatus table exists
-- If NOT, then create and insert
-- =============================================
IF OBJECT_ID('[dbo].[SiteStatus]', 'U') IS NULL
BEGIN
    -- Create the table
    CREATE TABLE [dbo].[SiteStatus] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Status] NVARCHAR(200) NOT NULL
    );
END

-- Insert data using MERGE (only if table is empty)
IF NOT EXISTS (SELECT 1 FROM [dbo].[SiteStatus])
BEGIN
    MERGE INTO [dbo].[SiteStatus] AS Target
    USING (VALUES
        (N'Started'),
        (N'Partially Completed'),
        (N'On Hold'),
        (N'Completed')
    ) AS Source ([Status])
    ON Target.[Status] = Source.[Status]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Status]) VALUES (Source.[Status]);
END