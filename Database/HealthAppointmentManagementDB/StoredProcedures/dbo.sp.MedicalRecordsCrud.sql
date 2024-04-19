CREATE PROCEDURE sp_MedicalRecords_CRUD
    @Mode INT,
    @Id INT = NULL,
    @PatientId INT,
    @DoctorId INT,
    @AppointmentId INT,
    @Height DECIMAL(5, 2),
    @Weight DECIMAL(5, 2),
    @VisitDate DATETIME,
    @Treatment NVARCHAR(255)
AS
BEGIN
    -- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO MedicalRecords (PatientId, DoctorId, AppointmentId, Height, Weight, VisitDate, Treatment)
        VALUES (@PatientId, @DoctorId, @AppointmentId, @Height, @Weight, @VisitDate, @Treatment)
    END
    -- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE MedicalRecords
        SET PatientId = @PatientId,
            DoctorId = @DoctorId,
            AppointmentId = @AppointmentId,
            Height = @Height,
            Weight = @Weight,
            VisitDate = @VisitDate,
            Treatment = @Treatment
        WHERE Id = @Id
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM MedicalRecords
        WHERE Id = @Id
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT * FROM MedicalRecords
        WHERE Id = @Id
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT * FROM MedicalRecords
    END
END
