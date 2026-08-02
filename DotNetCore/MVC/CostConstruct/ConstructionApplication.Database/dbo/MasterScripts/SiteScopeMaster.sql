PRINT 'Seeding [SiteScopeMaster]...';

SET IDENTITY_INSERT [dbo].[SiteScopeMaster] ON;

MERGE INTO [dbo].[SiteScopeMaster] AS trgt
USING (VALUES
      (1,  'Demolition'),
      (2,  'Excavation'),
      (3,  'Foundation'),
      (4,  'Column Casting'),
      (5,  'Beam Casting'),
      (6,  'Slab Casting'),
      (7,  'Brick Work'),
      (8,  'Plastering'),
      (9,  'Flooring'),
      (10, 'Tiles Work'),
      (11, 'Marble Work'),
      (12, 'Electrical Work'),
      (13, 'Plumbing Work'),
      (14, 'False Ceiling'),
      (15, 'Carpentry Work'),
      (16, 'Painting'),
      (17, 'Waterproofing'),
      (18, 'Finishing')
      ) AS src ([SiteScopeId], [ScopeName])

ON trgt.[SiteScopeId] = src.[SiteScopeId]

WHEN MATCHED THEN
    UPDATE SET
        [ScopeName] = src.[ScopeName]

WHEN NOT MATCHED BY TARGET THEN
    INSERT ([SiteScopeId], [ScopeName])
    VALUES (
        src.[SiteScopeId],
        src.[ScopeName]
    );

SET IDENTITY_INSERT [dbo].[SiteScopeMaster] OFF;