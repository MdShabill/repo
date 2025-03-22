CREATE TABLE [dbo].[Addresses] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [AddressLine1]  NVARCHAR (200) NULL,
    [ContractorId]  INT            NULL,
    [AddressTypeId] INT            NULL,
    [CountryId]     INT            NULL,
    [PinCode]       INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([AddressTypeId]) REFERENCES [dbo].[AddressTypes] ([Id]),
    FOREIGN KEY ([ContractorId]) REFERENCES [dbo].[Contractors] ([Id]),
    FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([Id])
);

