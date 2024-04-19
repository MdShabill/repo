CREATE PROCEDURE sp_AppointmentReminders_CRUD
    @Mode INT,
    @Id INT = NULL,
    @AppointmentId INT,
    @ReminderDateTime DATETIME,
    @ReminderType NVARCHAR(100),
    @Description NVARCHAR(255)
AS
BEGIN
    -- INSERT
    IF (@Mode = 1) 
    BEGIN
        INSERT INTO AppointmentReminders (AppointmentId, ReminderDateTime, ReminderType, Description)
        VALUES (@AppointmentId, @ReminderDateTime, @ReminderType, @Description)
    END
    -- UPDATE
    ELSE IF (@Mode = 2) 
    BEGIN
        UPDATE AppointmentReminders
        SET AppointmentId = @AppointmentId,
            ReminderDateTime = @ReminderDateTime,
            ReminderType = @ReminderType,
            Description = @Description
        WHERE Id = @Id
    END
    -- DELETE
    ELSE IF (@Mode = 3)
    BEGIN
        DELETE FROM AppointmentReminders
        WHERE Id = @Id
    END
    -- SELECT a single record
    ELSE IF (@Mode = 4)
    BEGIN
        SELECT * FROM AppointmentReminders
        WHERE Id = @Id
    END
    -- SELECT all records
    ELSE IF (@Mode = 5)
    BEGIN
        SELECT * FROM AppointmentReminders
    END
END
