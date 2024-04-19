CREATE PROCEDURE sp_Appointments_CRUD
    @Mode INT,
    @Id INT = NULL,
    @PatientId INT,
    @DoctorId INT,
    @AppointmentDate DATETIME,
    @Purpose NVARCHAR(255),
    @Status NVARCHAR(200)
AS
BEGIN
    -- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, Purpose, Status)
        VALUES (@PatientId, @DoctorId, @AppointmentDate, @Purpose, @Status)
    END
    -- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE Appointments
        SET PatientId = @PatientId,
            DoctorId = @DoctorId,
            AppointmentDate = @AppointmentDate,
            Purpose = @Purpose,
            Status = @Status
        WHERE Id = @Id
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM Appointments
        WHERE Id = @Id
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT * FROM Appointments
        WHERE Id = @Id
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT * FROM Appointments
    END
END
