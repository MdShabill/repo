CREATE PROCEDURE sp_Patients_CRUD
    @Mode INT,
    @Id INT = NULL,
    @FirstName NVARCHAR(200),
    @LastName NVARCHAR(200),
    @DOB DATETIME,
    @Gender NVARCHAR(100),
	@BloodGroup NVARCHAR(100),
    @ContactNumber NVARCHAR(200),
    @Email NVARCHAR(200),
    @AddressId INT
AS
BEGIN
    -- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO Patients (FirstName, LastName, DOB, Gender, BloodGroup, ContactNumber, Email, AddressId)
        VALUES (@FirstName, @LastName, @DOB, @Gender, @BloodGroup, @ContactNumber, @Email, @AddressId)
    END
    -- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE Patients
        SET FirstName = @FirstName,
            LastName = @LastName,
            DOB = @DOB,
            Gender = @Gender,
			BloodGroup = @BloodGroup,
            ContactNumber = @ContactNumber,
            Email = @Email,
            AddressId = @AddressId
        WHERE Id = @Id
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM Patients
        WHERE Id = @Id
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT * FROM Patients
        WHERE Id = @Id
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT * FROM Patients
    END
END
