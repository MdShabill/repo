PRINT 'Seeding [SiteStatus]...';

SET IDENTITY_INSERT [dbo].[SiteStatus] ON;

MERGE INTO [dbo].[SiteStatus] AS trgt
USING (VALUES
      (1, 'Started'),
      (2, 'Partially Completed'),
      (3, 'On Hold'),
      (4, 'Completed')
      ) AS src ([Id], [Status])
ON 
    trgt.[Id] = src.[Id]
WHEN MATCHED THEN
    UPDATE SET
        [Status] = src.[Status]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Id], [Status])
    VALUES ([Id], [Status]);

;
SET IDENTITY_INSERT [dbo].[SiteStatus] OFF;
