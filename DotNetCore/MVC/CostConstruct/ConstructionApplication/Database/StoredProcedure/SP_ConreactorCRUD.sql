CREATE PROCEDURE SP_ConreactorCRUD
    @Mode INT,
    @Id INT = NULL,
    @JobCategoryId INT = NULL,
    @Name NVARCHAR(100) = NULL,
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
        VALUES (@JobCategoryId, @Name, @Gender, @DOB, @ImageName, @MobileNumber, @ReferredBy);

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
        WHERE (@FilterJobCategoryId IS NULL OR JobCategoryId = @FilterJobCategoryId)
          AND (@Id IS NULL OR Contractors.Id = @Id);
    END
    ELSE IF @Mode = 3
    BEGIN
        -- Update Contractor
        UPDATE Contractors
        SET
            JobCategoryId = @JobCategoryId,
            Name = @Name,
            Gender = @Gender,
            DOB = @DOB,
            MobileNumber = @MobileNumber,
            ReferredBy = @ReferredBy
        WHERE Id = @Id;

    END
    ELSE IF @Mode = 4
    BEGIN
        -- Delete Contractor
        DELETE FROM Contractors 
        WHERE Id = @Id;
    END
END
GO
