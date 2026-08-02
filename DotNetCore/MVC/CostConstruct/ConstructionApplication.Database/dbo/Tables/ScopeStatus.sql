CREATE TABLE [dbo].[ScopeStatus]
(
    [ScopeStatusId] INT IDENTITY(1,1) NOT NULL,
    [StatusName] NVARCHAR(50) NOT NULL,

    CONSTRAINT [PK_ScopeStatus]
        PRIMARY KEY CLUSTERED ([ScopeStatusId] ASC)
)