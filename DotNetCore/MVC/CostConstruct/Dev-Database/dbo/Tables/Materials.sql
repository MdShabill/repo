CREATE TABLE [dbo].[Materials] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (200)  NOT NULL,
    [MaterialTypeId] INT             NOT NULL,
    [BrandId]        INT             NOT NULL,
    [SupplierId]     INT             NOT NULL,
    [UnitOfMeasure]  VARCHAR (200)   NOT NULL,
    [UnitPrice]      DECIMAL (10, 2) NOT NULL,
    [Date]           DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brands] ([Id]),
    FOREIGN KEY ([MaterialTypeId]) REFERENCES [dbo].[MaterialTypes] ([Id]),
    FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers] ([Id])
);

