PRINT 'Seeding [MaterialTypes]...';

SET IDENTITY_INSERT [dbo].[MaterialTypes] ON;

MERGE INTO [dbo].[MaterialTypes] AS trgt
USING (VALUES
      (1, 'Paint'),
      (2, 'Cement'),
      (3, 'Steel'),
      (4, 'Bricks'),
      (5, 'Electrical'),
      (6, 'Plumbing'),
      (7, 'Tiles'),
      (8, 'Sand'),
      (9, 'Crushed Stone'),
      (10, 'Sanitary')
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
SET IDENTITY_INSERT [dbo].[MaterialTypes] OFF;
