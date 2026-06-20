PRINT 'Seeding [Materials]...';

SET IDENTITY_INSERT [dbo].[Materials] ON;

MERGE INTO [dbo].[Materials] AS trgt
USING
(
    VALUES

    -- Cement
    (
        1,
        'UltraTech Cement',
        2,
        4,
        1,
        'Bag',
        420.00,
        GETDATE()
    ),

    -- Steel
    (
        2,
        'Tata Tiscon Steel',
        3,
        8,
        2,
        'Kg',
        150.00,
        GETDATE()
    ),

    -- Paint
    (
        3,
        'Asian Paint Premium',
        1,
        13,
        4,
        'Litre',
        350.00,
        GETDATE()
    ),

    -- Tiles
    (
        4,
        'Kajaria Floor Tile',
        7,
        14,
        1,
        'Box',
        950.00,
        GETDATE()
    ),

    -- Sanitary
    (
        5,
        'Jaquar Wash Basin',
        10,
        21,
        3,
        'Piece',
        4500.00,
        GETDATE()
    ),

    (
        6,
        'Sand',
        8,
        23,
        1,
        'CFT',
        7000.00,
        GETDATE()
    ),
    (
        7,
        'JSW Cement',
        2,
        3,
        2,
        'Bag',
        400.00,
        GETDATE()
    ),
    (
        8,
        'Shyam Steel',
        3,
        6,
        2,
        'Kg',
        120.00,
        GETDATE()
    )
)
AS src
(
    [Id],
    [Name],
    [MaterialTypeId],
    [BrandId],
    [SupplierId],
    [UnitOfMeasure],
    [UnitPrice],
    [Date]
)

ON trgt.[Id] = src.[Id]

WHEN MATCHED THEN
UPDATE SET
    [Name] = src.[Name],
    [MaterialTypeId] = src.[MaterialTypeId],
    [BrandId] = src.[BrandId],
    [SupplierId] = src.[SupplierId],
    [UnitOfMeasure] = src.[UnitOfMeasure],
    [UnitPrice] = src.[UnitPrice],
    [Date] = src.[Date]

WHEN NOT MATCHED BY TARGET THEN
INSERT
(
    [Id],
    [Name],
    [MaterialTypeId],
    [BrandId],
    [SupplierId],
    [UnitOfMeasure],
    [UnitPrice],
    [Date]
)
VALUES
(
    src.[Id],
    src.[Name],
    src.[MaterialTypeId],
    src.[BrandId],
    src.[SupplierId],
    src.[UnitOfMeasure],
    src.[UnitPrice],
    src.[Date]
);

SET IDENTITY_INSERT [dbo].[Materials] OFF;