CREATE PROCEDURE sp_Departments_CRUD
    @Mode INT,
    @Id INT = NULL,
    @DepartmentName NVARCHAR(100),
    @Location NVARCHAR(255),
    @Phone NVARCHAR(200)
AS
BEGIN
    -- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO Departments (DepartmentName, Location, Phone)
        VALUES (@DepartmentName, @Location, @Phone)
    END
    -- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE Departments
        SET DepartmentName = @DepartmentName,
            Location = @Location,
            Phone = @Phone
        WHERE Id = @Id
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM Departments
        WHERE Id = @Id
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT * FROM Departments
        WHERE Id = @Id
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT * FROM Departments
    END
END
