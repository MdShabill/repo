PRINT 'Seeding [ServiceTypes]...';

SET IDENTITY_INSERT [dbo].[ServiceTypes] ON;

MERGE INTO [dbo].[ServiceTypes] AS trgt
USING (VALUES
      (1, 'Master Mason'),
      (2, 'Labour'),
      (3, 'Electrician'),
      (4, 'Electrician Assistant'),
      (5, 'Plumber'),
      (6, 'Plumber Assistant'),
      (7, 'Painter'),
      (8, 'Painter Helper'),
      (9, 'Carpenter'),
      (10, 'Carpenter Assistant'),
      (11, 'Tiler'),
      (12, 'Tiler Assistant')
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
SET IDENTITY_INSERT [dbo].[ServiceTypes] OFF;
