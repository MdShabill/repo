CREATE TABLE [dbo].[Contractors] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [JobCategoryId] INT            NOT NULL,
    [Name]          NVARCHAR (200) NOT NULL,
    [Gender]        INT            NULL,
    [DOB]           DATETIME       NULL,
    [ImageName]     NVARCHAR (200) NULL,
    [MobileNumber]  NVARCHAR (200) NULL,
    [ReferredBy]    NVARCHAR (200) NULL,
    CONSTRAINT [PK__Contract__3214EC07ABF26A2F] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__Contracto__JobCa__13F1F5EB] FOREIGN KEY ([JobCategoryId]) REFERENCES [dbo].[JobCategories] ([Id])
);

