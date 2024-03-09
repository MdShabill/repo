--Q1: Fetch Those Passengers Who has to spend highest fare on booking tickets?
		--Display PassengerId, Name, Gender, Highest Number Booking, TotalFare ?

--Approach: 1
SELECT TOP 1
    Passengers.Id,
    Passengers.Name,
    Passengers.Gender,
	COUNT(Bookings.PassengerId) AS HighestBooking,
    SUM(Bookings.Fare) AS TotalFare
FROM Passengers
INNER JOIN Bookings ON Bookings.PassengerId = Passengers.Id
GROUP BY Passengers.Id, Passengers.Name, Passengers.Gender
ORDER BY SUM(Bookings.Fare) DESC

-- Using CTE

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
ORDER BY TotalFare DESC
