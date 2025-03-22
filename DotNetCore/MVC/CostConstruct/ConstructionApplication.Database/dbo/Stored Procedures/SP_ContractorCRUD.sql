CREATE PROCEDURE SP_ContractorCRUD
    @Mode INT,
	@ContractorId INT = NULL,
    @JobCategoryId INT = NULL,
    @ContractorName NVARCHAR(100) = NULL,
    @Gender NVARCHAR(10) = NULL,
    @DOB DATE = NULL,
    @ImageName NVARCHAR(255) = NULL,
    @MobileNumber NVARCHAR(20) = NULL,
    @ReferredBy NVARCHAR(100) = NULL,
    @FilterJobCategoryId INT = NULL
AS
BEGIN
    IF @Mode = 1
    BEGIN
        -- Add Contractor
        INSERT INTO Contractors (JobCategoryId, Name, Gender, DOB, ImageName, MobileNumber, ReferredBy)
        VALUES (@JobCategoryId, @ContractorName, @Gender, @DOB, @ImageName, @MobileNumber, @ReferredBy);

        SELECT SCOPE_IDENTITY() AS ContractorId;
    END
    ELSE IF @Mode = 2
    BEGIN
        -- Get Contractors
        SELECT 
            Contractors.Id AS ContractorId, 
            Contractors.JobCategoryId, 
            JobCategories.Name AS JobTypes, 
            Contractors.Name AS ContractorName, 
            Contractors.Gender, 
            Contractors.DOB, 
            Contractors.MobileNumber, 
            Contractors.ReferredBy, 
            Addresses.AddressLine1, 
            Addresses.AddressTypeId,
            AddressTypes.Name AS AddressTypes, 
            Addresses.CountryId,
            Countries.Name AS CountryName, 
            Addresses.PinCode
        FROM Contractors
        LEFT JOIN JobCategories ON Contractors.JobCategoryId = JobCategories.Id
        LEFT JOIN Addresses ON Contractors.Id = Addresses.ContractorId
        LEFT JOIN AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
        LEFT JOIN Countries ON Addresses.CountryId = Countries.Id
        WHERE (ISNULL(@FilterJobCategoryId, 0) = 0 OR Contractors.JobCategoryId = @FilterJobCategoryId)
		  AND (ISNULL(@ContractorId, 0) = 0 OR Contractors.Id = @ContractorId)
    END
    ELSE IF @Mode = 3
    BEGIN
        -- Update Contractor
        UPDATE Contractors
        SET
            JobCategoryId = @JobCategoryId,
            Name = @ContractorName,
            Gender = @Gender,
            DOB = @DOB,
            MobileNumber = @MobileNumber,
            ReferredBy = @ReferredBy
        WHERE Id = @ContractorId;

    END
    ELSE IF @Mode = 4
    BEGIN
        -- Delete Contractor
        DELETE FROM Contractors 
        WHERE Id = @ContractorId;
    END
END
