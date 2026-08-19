CREATE TABLE [dbo].[SiteScopeMaster]
(
    [Id] INT IDENTITY(1,1) NOT NULL,

    [ScopeName] NVARCHAR(100) NOT NULL,

    CONSTRAINT [PK_SiteScopeMaster]
        PRIMARY KEY CLUSTERED ([Id] ASC)
)