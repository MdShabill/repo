CREATE TABLE Bookings
(
	TicketId INT IDENTITY(1,1),
	PassengerId Int Not Null,
	TrainId Int Not Null,
	PNR INT NOt Null,
	BookingDate DateTime NOt Null,
	Fare Int Not Null,
	ScheduleStartDate DATETIME Null,
	ScheduleEndDate DATETIME Null,
	Class VARCHAR(100) Not Null,
	CoachNo VARCHAR(100) Not Null,
	SeatNo VARCHAR(100) Not Null,
	Travelled Bit Not Null,
	ActualStartDate DATETIME Null,
	ActualEndDate DATETIME Null,
	Primary Key (TicketId)
)

ALTER TABLE Bookings
ADD FOREIGN KEY (PassengerId) REFERENCES Passengers (Id)

ALTER TABLE Bookings
ADD FOREIGN KEY (TrainId) REFERENCES Trains (Id)

