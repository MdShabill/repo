CREATE TABLE [dbo].[AttendanceDetails] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [AttendanceId] INT            NOT NULL,
    [Name]         NVARCHAR (200) NOT NULL,
    [Role]         NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

