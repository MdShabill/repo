CREATE TABLE [dbo].[Sites]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [StartedDate] DATETIME NULL,
    ExpectedCompletionDate DATE NULL,
    ExpectedBudget DECIMAL(18,2) NULL,
    [SiteStatusId] INT NULL,
    [Note] NVARCHAR(100) NULL,

    CONSTRAINT [FK_Sites_SiteStatus] FOREIGN KEY ([SiteStatusId]) REFERENCES [dbo].[SiteStatus] ([Id])
);
