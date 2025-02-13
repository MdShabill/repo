CREATE PROCEDURE Sp_MaterialsDetails
    @Mode NVARCHAR(50),
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON

    IF @Mode = 'GetAll'
    BEGIN
        SELECT Id, Name FROM Materials
    END
    ELSE IF @Mode = 'GetMaterialInfo' AND @Id IS NOT NULL
    BEGIN
        SELECT Id, UnitOfMeasure, UnitPrice 
        FROM Materials 
        WHERE Id = @Id
    END
END
