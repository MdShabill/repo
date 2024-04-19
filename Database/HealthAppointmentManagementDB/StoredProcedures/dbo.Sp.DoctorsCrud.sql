CREATE PROCEDURE sp_Doctors_CRUD
    @Mode INT,
    @Id INT = NULL,
    @FirstName NVARCHAR(200),
    @LastName NVARCHAR(200),
    @ProfileImage NVARCHAR (200) = NULL,
    @Specialty NVARCHAR(100),
    @Qualification NVARCHAR(200),
    @WorkExperience NVARCHAR(200),
	@LicenceNumber NVARCHAR(200),
    @ContactNumber NVARCHAR(200),
    @Email NVARCHAR(200),
    @AddressId INT
AS
BEGIN
	-- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO Doctors (FirstName, LastName, ProfileImage, Specialty, Qualification, WorkExperience, LicenceNumber, ContactNumber, Email, AddressId)
        VALUES (@FirstName, @LastName, @ProfileImage, @Specialty, @Qualification, @WorkExperience, @LicenceNumber, @ContactNumber, @Email, @AddressId)
    END
	-- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE Doctors
        SET FirstName = @FirstName,
            LastName = @LastName,
            ProfileImage = @ProfileImage,
            Specialty = @Specialty,
            Qualification = @Qualification,
            WorkExperience = @WorkExperience,
			LicenceNumber = @LicenceNumber,
            ContactNumber = @ContactNumber,
            Email = @Email,
            AddressId = @AddressId
        WHERE Id = @Id
    END
	-- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM Doctors
        WHERE Id = @Id
    END
	-- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT * FROM Doctors
        WHERE Id = @Id
    END
	-- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT * FROM Doctors
    END
END
GO

