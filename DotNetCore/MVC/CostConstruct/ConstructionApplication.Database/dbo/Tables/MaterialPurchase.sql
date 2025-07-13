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
    [SiteId]         INT             NOT NULL,

    CONSTRAINT [PK__Material__3214EC078422C0CA] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__MaterialPurchase__Brands] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brands] ([Id]),
    CONSTRAINT [FK__MaterialPurchase__Materials] FOREIGN KEY ([MaterialId]) REFERENCES [dbo].[Materials] ([Id]),
    CONSTRAINT [FK__MaterialPurchase__Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Suppliers] ([Id]),
    CONSTRAINT [FK__MaterialPurchase__Sites] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[Sites] ([Id])
);

