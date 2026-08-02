PRINT 'Seeding [ScopeStatus]...';

SET IDENTITY_INSERT [dbo].[ScopeStatus] ON;

MERGE INTO [dbo].[ScopeStatus] AS trgt
USING (VALUES
      (1, 'Pending'),
      (2, 'In Progress'),
      (3, 'Completed'),
      (4, 'On Hold')
      ) AS src ([ScopeStatusId], [StatusName])

ON trgt.[ScopeStatusId] = src.[ScopeStatusId]

WHEN MATCHED THEN
    UPDATE SET
        [StatusName] = src.[StatusName]

WHEN NOT MATCHED BY TARGET THEN
    INSERT ([ScopeStatusId], [StatusName])
    VALUES (
        src.[ScopeStatusId],
        src.[StatusName]
    );

SET IDENTITY_INSERT [dbo].[ScopeStatus] OFF;