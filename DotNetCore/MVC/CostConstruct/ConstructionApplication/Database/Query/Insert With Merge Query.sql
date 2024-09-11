MERGE INTO Materials AS Target
USING (VALUES 
    ('Cement', 2, 3, 2, 'Bag', 350.00, GETDATE()),
    ('Steel', 3, 6, 2, 'Kg', 70, GETDATE()),
    ('Paint', 1, 11, 4, 'L', 50, GETDATE())
) AS Source (Name, MaterialTypeId, BrandId, SupplierId, UnitOfMeasure, UnitPrice, Date)
ON Target.Name = Source.Name
    AND Target.MaterialTypeId = Source.MaterialTypeId
    AND Target.BrandId = Source.BrandId
    AND Target.SupplierId = Source.SupplierId
    AND Target.UnitOfMeasure = Source.UnitOfMeasure
WHEN NOT MATCHED BY TARGET
    THEN INSERT (Name, MaterialTypeId, BrandId, SupplierId, UnitOfMeasure, UnitPrice, Date)
         VALUES (Source.Name, Source.MaterialTypeId, Source.BrandId, Source.SupplierId, Source.UnitOfMeasure, Source.UnitPrice, Source.Date);
