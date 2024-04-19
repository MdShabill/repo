CREATE PROCEDURE sp_PrescriptionDetails_CRUD
    @Mode INT,
    @Id INT = NULL,
    @PrescriptionId INT,
    @Medicine NVARCHAR(200),
    @Dosage NVARCHAR(200),
    @Frequency NVARCHAR(200)
AS
BEGIN
    -- INSERT
    IF (@Mode = 1)
    BEGIN
        INSERT INTO PrescriptionDetails (PrescriptionId, Medicine, Dosage, Frequency)
        VALUES (@PrescriptionId, @Medicine, @Dosage, @Frequency);
    END
    -- UPDATE
    ELSE IF (@Mode = 2)
    BEGIN
        UPDATE PrescriptionDetails
        SET PrescriptionId = @PrescriptionId,
            Medicine = @Medicine,
            Dosage = @Dosage,
            Frequency = @Frequency
        WHERE Id = @Id;
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM PrescriptionDetails
        WHERE Id = @Id;
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT Id, PrescriptionId, Medicine, Dosage, Frequency
        FROM PrescriptionDetails
        WHERE Id = @Id;
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT Id, PrescriptionId, Medicine, Dosage, Frequency
        FROM PrescriptionDetails;
    END
END;
GO
