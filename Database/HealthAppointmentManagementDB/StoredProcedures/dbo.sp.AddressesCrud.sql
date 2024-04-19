CREATE PROCEDURE sp_Addresses_CRUD
    @Mode INT,
    @Id INT = NULL,
    @AddressLine1 NVARCHAR(200),
    @AddressLine2 NVARCHAR(200) = NULL,
    @PinCode INT,
    @Country NVARCHAR(100),
    @AddressTypeId INT
AS
BEGIN
    -- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO Addresses (AddressLine1, AddressLine2, PinCode, Country, AddressTypeId)
        VALUES (@AddressLine1, @AddressLine2, @PinCode, @Country, @AddressTypeId)
    END
    -- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE Addresses
        SET AddressLine1 = @AddressLine1,
            AddressLine2 = @AddressLine2,
            PinCode = @PinCode,
            Country = @Country,
            AddressTypeId = @AddressTypeId
        WHERE Id = @Id
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM Addresses
        WHERE Id = @Id
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT * FROM Addresses
        WHERE Id = @Id
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT * FROM Addresses
    END
END
