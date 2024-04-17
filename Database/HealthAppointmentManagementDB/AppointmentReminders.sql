CREATE TABLE AppointmentReminders 
(
    ID INT IDENTITY(1,1),
    AppointmentID INT NOT NULL,
    ReminderDateTime DATETIME NOT NULL,
    ReminderType NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
	PRIMARY KEY (ID),
    FOREIGN KEY (AppointmentID) REFERENCES Appointments(ID)
)
