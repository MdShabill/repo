﻿CREATE TABLE [dbo].[CostMaster] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [ServiceTypeId] INT             NOT NULL,
    [Cost]          DECIMAL (10, 2) NOT NULL,
    [Date]          DATE            NOT NULL,
    CONSTRAINT [PK__CostMast__3214EC07252A5A5A] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__CostMaste__ServiceType] FOREIGN KEY ([ServiceTypeId]) REFERENCES [dbo].[ServiceTypes] ([Id])
);

