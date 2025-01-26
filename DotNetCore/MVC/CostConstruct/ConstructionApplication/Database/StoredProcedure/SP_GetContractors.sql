CREATE PROCEDURE GetContractors
    @jobCategoryId INT = NULL,
    @id INT = NULL
AS
BEGIN
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
    WHERE (@jobCategoryId IS NULL OR JobCategoryId = @jobCategoryId)
    AND (@id IS NULL OR Contractors.Id = @id)
END

