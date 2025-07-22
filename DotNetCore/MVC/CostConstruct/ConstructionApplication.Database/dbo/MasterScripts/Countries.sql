PRINT 'Seeding [Countries]...';

SET IDENTITY_INSERT [dbo].[Countries] ON;

MERGE INTO [dbo].[Countries] AS trgt
USING (VALUES
      (1, 'US'),
      (2, 'UAE'),
      (3, 'India'),
      (4, 'England'),
      (5, 'Pakistan'),
      (6, 'Qatar')
      ) AS src ([Id], [Name])
ON 
    trgt.[Id] = src.[Id]
WHEN MATCHED THEN
    UPDATE SET
        [Name] = src.[Name]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Id], [Name])
    VALUES ([Id], [Name]);


SET IDENTITY_INSERT [dbo].[Countries] OFF;
