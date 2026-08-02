CREATE TABLE [dbo].[SiteScope]
(
    [SiteScopeMappingId] INT IDENTITY(1,1) NOT NULL,

    [SiteId] INT NOT NULL,

    [SiteScopeId] INT NOT NULL,

    [ScopeStatusId] INT NOT NULL,

    [CompletedDate] DATE NULL,

    [Remarks] NVARCHAR(500) NULL,

    CONSTRAINT [PK_SiteScope]
        PRIMARY KEY CLUSTERED ([SiteScopeMappingId] ASC),

    CONSTRAINT [FK_SiteScope_Site]
        FOREIGN KEY ([SiteId])
        REFERENCES [dbo].[Sites]([Id]),

    CONSTRAINT [FK_SiteScope_Master]
        FOREIGN KEY ([SiteScopeId])
        REFERENCES [dbo].[SiteScopeMaster]([SiteScopeId]),

    CONSTRAINT [FK_SiteScope_Status]
        FOREIGN KEY ([ScopeStatusId])
        REFERENCES [dbo].[ScopeStatus]([ScopeStatusId])
)