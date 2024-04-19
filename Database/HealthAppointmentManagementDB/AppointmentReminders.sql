CREATE TABLE AppointmentReminders 
(
    Id INT IDENTITY(1,1),
    AppointmentId INT NOT NULL,
    ReminderDateTime DATETIME NOT NULL,
    ReminderType NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
	PRIMARY KEY (Id),
    FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id)
)
