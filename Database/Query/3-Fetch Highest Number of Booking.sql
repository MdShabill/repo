--Q3: Fetch Those Passengers Who has Highest Number OF Booking. 
		--Display PassengerId, Name, Gender, Highest Number OF Booking, TotalFare ?  

--Approach: 1
SELECT TOP 1
	Passengers.Id , Passengers.Name , Passengers.Gender,
	COUNT(Bookings.PassengerId) AS HighestBooking, 
	SUM(Bookings.Fare) AS TotalFare
FROM Passengers
	INNER JOIN Bookings ON Bookings.PassengerId = Passengers.Id
	
GROUP BY Passengers.Id , Passengers.Name , Passengers.Gender
ORDER BY HighestBooking DESC


--Using CTE
--Approach: 2
WITH PassengerBookings AS (
    SELECT
    Passengers.Id,
    Passengers.Name,
    Passengers.Gender,
    COUNT(Bookings.PassengerId) AS HighestBooking,
    SUM(Bookings.Fare) AS TotalFare
    FROM Passengers
    INNER JOIN Bookings ON Bookings.PassengerId = Passengers.Id
    GROUP BY Passengers.Id, Passengers.Name, Passengers.Gender
)
SELECT TOP 1
    Id,
    Name,
    Gender,
    HighestBooking,
    TotalFare
FROM PassengerBookings
ORDER BY HighestBooking DESC
