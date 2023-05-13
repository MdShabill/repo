--Truncate Table Bookings
--DELETE FROM  BookingDetails
--DBCC CHECKIDENT ('BookingDetails', RESEED, 0);

INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate,ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(1, 2, 985764, 2500, DATEADD(DAY, 10,GETDATE()), DATEADD(DAY,30,GETDATE()), DATEADD(DAY,31,GETDATE()),
		'3 Tier','B3','33LB', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate, 
			 Class, CoachNo, SeatNo, Travelled )

VALUES(2, 1, 985764, 2200, DATEADD(DAY,15,GETDATE()), DATEADD(DAY,30,GETDATE()), DATEADD(DAY,31,GETDATE()),
		'3 Tier','B3','34MB', 0)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(3, 4, 985764, 8000, DATEADD(DAY,12,GETDATE()), DATEADD(DAY,30,GETDATE()), DATEADD(DAY,32,GETDATE()), 
		'1 Tier','B3','35UB', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(4, 6, 868794, 4500, DATEADD(DAY,-25,GETDATE()), DATEADD(DAY,65,GETDATE()), DATEADD(DAY,66,GETDATE()), 
		'2 Tier','B4','56SU', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)
VALUES(4, 6, 868794, 4500, DATEADD(DAY,-25,GETDATE()), DATEADD(DAY,65,GETDATE()), DATEADD(DAY,66,GETDATE()), 
		 '2 Tier','B4','44SU', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class,CoachNo, SeatNo, Travelled)

VALUES(4, 6, 868794, 4500, DATEADD(DAY, -25,GETDATE()), DATEADD(DAY,65,GETDATE()),DATEADD(DAY,66,GETDATE()),
		 '2 Tier','B4','46SL', 0)



INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate, 
			 Class, CoachNo, SeatNo, Travelled)

VALUES(6, 7, 856897, 6000, DATEADD(DAY, -128,GETDATE()), DATEADD(DAY,39,GETDATE()), DATEADD(DAY,40,GETDATE()),
		'1 Tier','A1','10MB', 0)



INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(7, 8, 868794, 900, DATEADD(DAY,-37,GETDATE()), DATEADD(DAY,15,GETDATE()), DATEADD(DAY, 16,GETDATE()),
		'Sleeper','S4','56MB', 1)



INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(8, 9, 254698, 3500, DATEADD(DAY,-20,GETDATE()), DATEADD(DAY,90,GETDATE()), DATEADD(DAY,91,GETDATE()),
		'2 Tier','B4','56SP', 1)



INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(10, 11, 584978, 6500, DATEADD(DAY, -30,GETDATE()), DATEADD(DAY,65,GETDATE()),DATEADD(DAY,66,GETDATE()),
		 '2 Tier','B4','67UB', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate, 
			 Class, CoachNo, SeatNo, Travelled)

VALUES(10, 11, 584978, 6500, DATEADD(DAY, -30,GETDATE()), DATEADD(DAY,65,GETDATE()), DATEADD(DAY,66,GETDATE()),
		'2 Tier','B4','69MB', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate, 
			 Class, CoachNo, SeatNo, Travelled)

VALUES(2, 3, 584978, 6000, DATEADD(DAY,-60,GETDATE()), DATEADD(DAY,65,GETDATE()), DATEADD(DAY,67,GETDATE()), 
		'2 Tier','B4','17LB', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate, 
			 Class, CoachNo, SeatNo, Travelled)

VALUES(11, 13, 854789, 7700, DATEADD(DAY,-50,GETDATE()), DATEADD(DAY,109,GETDATE()), DATEADD(DAY,110,GETDATE()),
		'1 Tier','A4','15UB', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(11, 13, 854789, 7700, DATEADD(DAY,-50,GETDATE()), DATEADD(DAY,109,GETDATE()), DATEADD(DAY,110,GETDATE()),
		'1 Tier','A4','15UB', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(13, 12, 254698, 1200, DATEADD(DAY,15,GETDATE()), DATEADD(DAY,120,GETDATE()), DATEADD(DAY,121,GETDATE()), 
		'Sleeper','S4','70MB', 0)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			  Class, CoachNo, SeatNo, Travelled)

VALUES(14, 14, 52462, 10000, DATEADD(DAY,-80,GETDATE()), DATEADD(DAY,125,GETDATE()), DATEADD(DAY,127,GETDATE()),
		'1 Tier','A4','18MB', 1)



INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(14, 14, 52462, 10000, DATEADD(DAY, -80,GETDATE()), DATEADD(DAY,125,GETDATE()), DATEADD(DAY,127,GETDATE()),
		'1 Tier','A4','18MB', 1)


INSERT INTO Bookings ( PassengerId, TrainId, PNR, Fare, BookingDate, ScheduleStartDate, ScheduleEndDate,
			 Class, CoachNo, SeatNo, Travelled)

VALUES(15, 11, 52462, 4000, DATEADD(DAY, 28,GETDATE()), DATEADD(DAY,125,GETDATE()), DATEADD(DAY,127,GETDATE()),
		'2 Tier','A4','27UB', 1)


