CREATE PROCEDURE sp_AddressTypes_CRUD
    @Mode INT,
    @Id INT = NULL,
    @AddressTypeName NVARCHAR(200)
AS
BEGIN
    -- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO AddressTypes (AddressTypeName)
        VALUES (@AddressTypeName)
    END
    -- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE AddressTypes
        SET AddressTypeName = @AddressTypeName
        WHERE Id = @Id
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM AddressTypes
        WHERE Id = @Id
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT * FROM AddressTypes
        WHERE Id = @Id
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT * FROM AddressTypes
    END
END
