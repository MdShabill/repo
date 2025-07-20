CREATE PROCEDURE SP_ServiceProviderCRUD
    @Mode INT,
	@ServiceProviderId INT = NULL,
    @ServiceTypeId INT = NULL,
    @ServiceProviderName NVARCHAR(100) = NULL,
    @Gender NVARCHAR(10) = NULL,
    @DOB DATE = NULL,
    @ImageName NVARCHAR(255) = NULL,
    @MobileNumber NVARCHAR(20) = NULL,
    @ReferredBy NVARCHAR(100) = NULL,
    @FilterServiceTypeId INT = NULL
AS
BEGIN
    IF @Mode = 1
    BEGIN
        -- Add ServiceProvider
        INSERT INTO ServiceProviders (ServiceTypeId, Name, Gender, DOB, ImageName, MobileNumber, ReferredBy)
        VALUES (@ServiceTypeId, @ServiceProviderName, @Gender, @DOB, @ImageName, @MobileNumber, @ReferredBy);

        SELECT SCOPE_IDENTITY() AS ServiceProviderId;
    END
    ELSE IF @Mode = 2
    BEGIN
        -- Get ServiceProviders
        SELECT 
            ServiceProviders.Id AS ServiceProviderId, 
            ServiceProviders.ServiceTypeId, 
            ServiceTypes.Name AS JobTypes, 
            ServiceProviders.Name AS ServiceProviderName, 
            ServiceProviders.Gender, 
            ServiceProviders.DOB, 
            ServiceProviders.MobileNumber, 
            ServiceProviders.ReferredBy, 
            Addresses.AddressLine1, 
            Addresses.AddressTypeId,
            AddressTypes.Name AS AddressTypes, 
            Addresses.CountryId,
            Countries.Name AS CountryName, 
            Addresses.PinCode
        FROM ServiceProviders
        LEFT JOIN ServiceTypes ON ServiceProviders.ServiceTypeId = ServiceTypes.Id
        LEFT JOIN Addresses ON ServiceProviders.Id = Addresses.ServiceProviderId
        LEFT JOIN AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
        LEFT JOIN Countries ON Addresses.CountryId = Countries.Id
        WHERE (ISNULL(@FilterServiceTypeId, 0) = 0 OR ServiceProviders.ServiceTypeId = @FilterServiceTypeId)
		  AND (ISNULL(@ServiceProviderId, 0) = 0 OR ServiceProviders.Id = @ServiceProviderId)
    END
    ELSE IF @Mode = 3
    BEGIN
        -- Update ServiceProvider
        UPDATE ServiceProviders
        SET
            ServiceTypeId = @ServiceTypeId,
            Name = @ServiceProviderName,
            Gender = @Gender,
            DOB = @DOB,
            MobileNumber = @MobileNumber,
            ReferredBy = @ReferredBy
        WHERE Id = @ServiceProviderId;

    END
    ELSE IF @Mode = 4
    BEGIN
        -- Delete ServiceProvider
        DELETE FROM ServiceProviders 
        WHERE Id = @ServiceProviderId;
    END
END
