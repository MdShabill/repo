CREATE PROCEDURE Sp_Site_CRUD
    @Mode NVARCHAR(20),
    @Id INT = NULL,
    @Name NVARCHAR(100) = NULL,
    @Location NVARCHAR(200) = NULL,
    @CreatedDate DATETIME = NULL
AS
BEGIN
    IF @Mode = 'GetAll'
    BEGIN
        SELECT Id, Name, Location, CreatedDate FROM Sites
    END
    ELSE IF @Mode = 'GetById'
    BEGIN
        SELECT Id, Name, Location, CreatedDate FROM Sites WHERE Id = @Id
    END
    ELSE IF @Mode = 'Create'
    BEGIN
        INSERT INTO Sites (Name, Location, CreatedDate)
        VALUES (@Name, @Location, @CreatedDate)
    END
    ELSE IF @Mode = 'Update'
    BEGIN
        UPDATE Sites SET
            Name = @Name,
            Location = @Location,
            CreatedDate = @CreatedDate
        WHERE Id = @Id
    END
    ELSE IF @Mode = 'Delete'
    BEGIN
        DELETE FROM Sites WHERE Id = @Id
    END
END
