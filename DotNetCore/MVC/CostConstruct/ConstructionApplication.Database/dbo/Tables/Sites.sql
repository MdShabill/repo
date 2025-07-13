﻿CREATE TABLE [dbo].[Sites]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [Location] NVARCHAR(150) NULL,
    [CreatedDate] DATETIME NULL
);
