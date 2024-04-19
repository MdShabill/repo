CREATE PROCEDURE sp_Prescriptions_CRUD
    @Mode INT,
    @Id INT = NULL,
    @MedicalRecordId INT,
    @Advice NVARCHAR(200)
AS
BEGIN
    -- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO Prescriptions (MedicalRecordId, Advice)
        VALUES (@MedicalRecordId, @Advice);
    END
    -- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE Prescriptions
        SET MedicalRecordId = @MedicalRecordId,
            Advice = @Advice
        WHERE Id = @Id;
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM Prescriptions
        WHERE Id = @Id;
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT Id, MedicalRecordId, Advice
        FROM Prescriptions
        WHERE Id = @Id;
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT Id, MedicalRecordId, Advice
        FROM Prescriptions;
    END
END;
GO
