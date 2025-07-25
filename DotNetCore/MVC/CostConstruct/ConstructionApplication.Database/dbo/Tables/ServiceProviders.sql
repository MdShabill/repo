﻿CREATE TABLE [dbo].[ServiceProviders] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ServiceTypeId] INT            NOT NULL,
    [Name]          NVARCHAR (200) NOT NULL,
    [Gender]        INT            NULL,
    [DOB]           DATETIME       NULL,
    [ImageName]     NVARCHAR (200) NULL,
    [MobileNumber]  NVARCHAR (200) NULL,
    [ReferredBy]    NVARCHAR (200) NULL,
    CONSTRAINT [PK__ServiceProvider__3214EC07ABF26A2F] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__ServiceProvider__ServiceTypes] FOREIGN KEY ([ServiceTypeId]) REFERENCES [dbo].[ServiceTypes] ([Id])
);

