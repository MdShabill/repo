PRINT 'Seeding Users...';

IF EXISTS
(
    SELECT 1
    FROM sys.tables
    WHERE name = 'Users'
    AND schema_id = SCHEMA_ID('dbo')
)
BEGIN

    SET IDENTITY_INSERT [dbo].[Users] ON;

    MERGE [dbo].[Users] AS trgt
    USING
    (
        VALUES
        (
            1,
            'Zishan',
            1,
            'zishan123@gmail.com',
            'sujawalpur123',
            '9878642020',
            NULL,
            GETDATE(),
            0,
            0
        ),

        (
            2,
            'Aman',
            1,
            'aman@gmail.com',
            'Aman@123',
            '9876543210',
            NULL,
            GETDATE(),
            0,
            0
        ),

        (
            3,
            'Sara',
            2,
            'sara@gmail.com',
            'Sara@123',
            '9876501234',
            NULL,
            GETDATE(),
            0,
            0
        )

    ) AS src
    (
        [Id],
        [Name],
        [Gender],
        [Email],
        [Password],
        [Mobile],
        [LastFailedLoginDate],
        [LastSuccessfulLoginDate],
        [LoginFailedCount],
        [IsLocked]
    )

    ON trgt.[Id] = src.[Id]

    WHEN MATCHED THEN
        UPDATE SET
            [Name] = src.[Name],
            [Gender] = src.[Gender],
            [Email] = src.[Email],
            [Password] = src.[Password],
            [Mobile] = src.[Mobile],
            [LastFailedLoginDate] = src.[LastFailedLoginDate],
            [LastSuccessfulLoginDate] = src.[LastSuccessfulLoginDate],
            [LoginFailedCount] = src.[LoginFailedCount],
            [IsLocked] = src.[IsLocked]

    WHEN NOT MATCHED BY TARGET THEN
        INSERT
        (
            [Id],
            [Name],
            [Gender],
            [Email],
            [Password],
            [Mobile],
            [LastFailedLoginDate],
            [LastSuccessfulLoginDate],
            [LoginFailedCount],
            [IsLocked]
        )
        VALUES
        (
            src.[Id],
            src.[Name],
            src.[Gender],
            src.[Email],
            src.[Password],
            src.[Mobile],
            src.[LastFailedLoginDate],
            src.[LastSuccessfulLoginDate],
            src.[LoginFailedCount],
            src.[IsLocked]
        );

    SET IDENTITY_INSERT [dbo].[Users] OFF;

END