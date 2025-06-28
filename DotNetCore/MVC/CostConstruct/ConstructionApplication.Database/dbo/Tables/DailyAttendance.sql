CREATE TABLE [dbo].[DailyAttendance] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [Date]            DATE            NOT NULL,
    [JobCategoryId]   INT             NOT NULL,
    [ContractorId]    INT             NULL,
    [TotalWorker]     INT             NOT NULL,
    [AmountPerWorker] DECIMAL (10, 2) NOT NULL,
    [TotalAmount]     DECIMAL (10, 2) NULL,
    [Notes]           NVARCHAR (200)  NULL, 
    [SiteId]          INT             NOT NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([JobCategoryId]) REFERENCES [dbo].[JobCategories] ([Id]),
    CONSTRAINT [FK__DailyAttendance__Contractors] FOREIGN KEY ([ContractorId]) REFERENCES [dbo].[Contractors] ([Id]),
    CONSTRAINT [FK__DailyAttendance__Sites] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[Sites] ([Id])
);

