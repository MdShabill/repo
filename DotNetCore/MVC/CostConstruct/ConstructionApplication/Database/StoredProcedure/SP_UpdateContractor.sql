CREATE PROCEDURE sp_UpdateContractor
    @Id INT,
    @JobCategoryId INT,
    @Name NVARCHAR(100),
    @Gender NVARCHAR(10),
    @DOB DATE,
    @MobileNumber NVARCHAR(20),
    @ReferredBy NVARCHAR(100)
AS
BEGIN
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
