CREATE TABLE [dbo].[DailyAttendance] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [Date]            DATE            NOT NULL,
    [JobCategoryId]   INT             NOT NULL,
    [ContractorId]    INT             NULL,
    [TotalWorker]     INT             NOT NULL,
    [AmountPerWorker] DECIMAL (10, 2) NOT NULL,
    [TotalAmount]     DECIMAL (10, 2) NULL,
    [Notes] NVARCHAR(200) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([JobCategoryId]) REFERENCES [dbo].[JobCategories] ([Id]),
    CONSTRAINT [FK__DailyAtte__Contr__1C873BEC] FOREIGN KEY ([ContractorId]) REFERENCES [dbo].[Contractors] ([Id])
);

