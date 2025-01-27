CREATE PROCEDURE sp_AddContractor
    @JobCategoryId INT,
    @Name NVARCHAR(100),
    @Gender NVARCHAR(10),
    @DOB DATE,
    @ImageName NVARCHAR(255),
    @MobileNumber NVARCHAR(20),
    @ReferredBy NVARCHAR(100)
AS
BEGIN
    INSERT INTO Contractors (JobCategoryId, Name, Gender, DOB, ImageName, MobileNumber, ReferredBy)
    VALUES (@JobCategoryId, @Name, @Gender, @DOB, @ImageName, @MobileNumber, @ReferredBy);

    SELECT SCOPE_IDENTITY() AS ContractorId;
END
