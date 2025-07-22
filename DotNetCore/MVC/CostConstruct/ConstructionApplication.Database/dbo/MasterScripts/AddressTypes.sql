PRINT 'Seeding AddressTypes...';

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AddressTypes' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    SET IDENTITY_INSERT [dbo].[AddressTypes] ON;

    MERGE INTO [dbo].[AddressTypes] AS trgt
    USING (VALUES
           (1, 'Home'),
           (2, 'Office'),
           (3, 'Apartment'),
           (4, 'Factory'),
           (5, 'FarmHouse')
           ) AS src ([Id], [Name])
    ON trgt.[Id] = src.[Id]
    WHEN MATCHED THEN
        UPDATE SET [Name] = src.[Name]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Id], [Name])
        VALUES ([Id], [Name]);

    SET IDENTITY_INSERT [dbo].[AddressTypes] OFF;
END
