PRINT 'Seeding [Brands]...';

SET IDENTITY_INSERT [dbo].[Brands] ON;

MERGE INTO [dbo].[Brands] AS trgt
USING (VALUES
      (1, 'Ambuja'),
      (2, 'ACC'),
      (3, 'JSW'),
      (4, 'UltraTech'),
      (5, 'Jindal Pantherl'),
      (6, 'Shyam TMT Bar'),
      (7, 'Kamdhenu'),
      (8, 'Tata Tiscon'),
      (9, 'Berger Paints'),
      (10, 'Indigo Paints'),
      (11, 'Dulux'),
      (12, 'Nerolac Paints'),
      (13, 'Asian Paints'),
      (14, 'Kajaria Tiles'),
      (15, 'Johnson Tiles'),
      (16, 'Orientbell Tiles'),
      (17, 'Nitco Tiles'),
      (18, 'AGL Tiles'),
      (19, 'Hindware'),
      (20, 'CERA'),
      (21, 'Jaquar'),
      (22, 'Parryware'),
      (23, 'Babar Enterprises')
      ) AS src ([Id], [Name])
ON 
    trgt.[Id] = src.[Id]
WHEN MATCHED THEN
    UPDATE SET
        [Name] = src.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Id], [Name])
    VALUES ([Id], [Name]);

;
SET IDENTITY_INSERT [dbo].[Brands] OFF;