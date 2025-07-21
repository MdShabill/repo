-- =============================================
-- Check if Brands table exists
-- If NOT, then create and insert
-- =============================================
IF OBJECT_ID('[dbo].[Brands]', 'U') IS NULL
BEGIN
    -- Create the table
    CREATE TABLE [dbo].[Brands] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] VARCHAR(255) NOT NULL
    );
END

-- Insert data using MERGE (only if table is empty)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Brands])
BEGIN
    MERGE INTO [dbo].[Brands] AS Target
    USING (VALUES
        ('Ambuja'),
        ('ACC'),
        ('JSW'),
        ('UltraTech'),
        ('Jindal Pantherl'),
        ('Shyam TMT Bar'),
        ('Kamdhenu'),
        ('Tata Tiscon'),
        ('Berger Paints'),
        ('Indigo Paints'),
        ('Dulux'),
        ('Nerolac Paints'),
        ('Asian Paints'),
        ('Kajaria Tiles'),
        ('Johnson Tiles'),
        ('Orientbell Tiles'),
        ('Nitco Tiles'),
        ('AGL Tiles'),
        ('Hindware'),
        ('CERA'),
        ('Jaquar'),
        ('Parryware'),
        ('Babar Enterprises')
    ) AS Source ([Name])
    ON Target.[Name] = Source.[Name]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Name]) VALUES (Source.[Name]);
END