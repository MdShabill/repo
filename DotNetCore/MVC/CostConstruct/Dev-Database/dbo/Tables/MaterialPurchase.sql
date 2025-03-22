CREATE TABLE [dbo].[MaterialPurchase] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [MaterialId]     INT             NOT NULL,
    [SupplierId]     INT             NULL,
    [PhoneNumber]    NVARCHAR (200)  NULL,
    [BrandId]        INT             NULL,
    [Quantity]       INT             NOT NULL,
    [UnitOfMeasure]  NVARCHAR (200)  NOT NULL,
    [Date]           DATETIME        NOT NULL,
    [MaterialCost]   DECIMAL (10, 2) NOT NULL,
    [DeliveryCharge] DECIMAL (10, 2) NULL,
    [PaymentStatus]  NVARCHAR (200)  NULL,
    CONSTRAINT [PK__Material__3214EC078422C0CA] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__MaterialP__Brand__70DDC3D8] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brands] ([Id]),
    CONSTRAINT [FK__MaterialP__Mater__71D1E811] FOREIGN KEY ([MaterialId]) REFERENCES [dbo].[Materials] ([Id]),
    CONSTRAINT [FK__MaterialP__Suppl__6FE99F9F] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers] ([Id])
);

