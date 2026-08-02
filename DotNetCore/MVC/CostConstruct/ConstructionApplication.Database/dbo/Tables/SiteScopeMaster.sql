CREATE TABLE [dbo].[SiteScopeMaster]
(
    [SiteScopeId] INT IDENTITY(1,1) NOT NULL,

    [ScopeName] NVARCHAR(100) NOT NULL,

    CONSTRAINT [PK_SiteScopeMaster]
        PRIMARY KEY CLUSTERED ([SiteScopeId] ASC)
)