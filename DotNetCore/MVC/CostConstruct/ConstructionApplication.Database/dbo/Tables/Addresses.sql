CREATE TABLE [dbo].[Addresses] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [AddressLine1]  NVARCHAR (200) NULL,
    [ServiceProviderId]  INT            NULL,
    [AddressTypeId] INT            NULL,
    [CountryId]     INT            NULL,
    [PinCode]       INT            NULL,
    [SiteId]       INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([AddressTypeId]) REFERENCES [dbo].[AddressTypes] ([Id]),
    FOREIGN KEY ([ServiceProviderId]) REFERENCES [dbo].[ServiceProviders] ([Id]),
    FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([Id]),
    CONSTRAINT [FK_Addresses_Sites] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[Sites] ([Id])
);

