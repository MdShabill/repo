SET IDENTITY_INSERT [dbo].[Suppliers] ON;

MERGE INTO [dbo].[Suppliers] AS trgt
USING (VALUES
      (1, 'ZamZam', '9845665640', 'zamzam@gmail.com', 'Dholi Sakra, Railway Sattion, Muzaffarpur, Bihar'),
      (2, 'Aashiyana Hardware', '8899227710', 'aashiyana@gmail.com', 'Sujawalpur Chowk, Dholi, Muzaffarpur, Bihar'),
      (3, 'Sankar Electronic', '9334391268', 'sankar@gmail.com', 'Tilak Maidan, MuzaffarPur, Bihar'),
      (4, 'Maruf Hindustan Hardware', '7766889400', 'mukhiya@gmail.com', 'SujawalPur, Dholi Skara, Muzaffarpur, Bihar')
      ) AS src ([Id], [Name], [PhoneNumber], [Email], [Address])
ON 
    TARGET.[Id] = src.[Id]
WHEN MATCHED THEN
    UPDATE SET
        [Name] = src.[Name],
        [PhoneNumber] = src.[PhoneNumber],
        [Email] = src.[Email],
        [Address] = src.[Address]
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([Id], [Name], [PhoneNumber], [Email], [Address])
    VALUES ([Id], [Name], [PhoneNumber], [Email], [Address]);

;
SET IDENTITY_INSERT [dbo].[Suppliers] OFF;
