CREATE PROCEDURE Sp_AddressCRUD
    @Mode NVARCHAR(50),
    @ContractorId INT,
    @AddressLine1 NVARCHAR(255) = NULL,
    @AddressTypeId INT = NULL,
    @CountryId INT = NULL,
    @PinCode INT = NULL
AS
BEGIN 
    IF @Mode = 'INSERT'
    BEGIN
        INSERT INTO Addresses (ContractorId, AddressLine1, AddressTypeId, CountryId, PinCode)
        VALUES (@ContractorId, @AddressLine1, @AddressTypeId, @CountryId, @PinCode)
    END
    ELSE IF @Mode = 'UPDATE'
    BEGIN
        UPDATE Addresses 
        SET AddressLine1 = @AddressLine1,
            AddressTypeId = @AddressTypeId,
            CountryId = @CountryId,
            PinCode = @PinCode
        WHERE ContractorId = @ContractorId
    END
    ELSE IF @Mode = 'DELETE'
    BEGIN
        DELETE FROM Addresses WHERE ContractorId = @ContractorId
    END
END
