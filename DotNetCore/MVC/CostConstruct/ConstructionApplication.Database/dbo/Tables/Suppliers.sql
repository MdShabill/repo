CREATE TABLE [dbo].[Suppliers] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [PhoneNumber] NVARCHAR (200) NULL,
    [Email]       NVARCHAR (100) NULL,
    [Address]     NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

