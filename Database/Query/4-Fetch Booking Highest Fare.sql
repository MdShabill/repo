--Approach: 1
--Q3: Fetch Those Passengers Who has to spend highest fare on booking tickets?
		--Display PassengerId, Name, Gender, HighestTravelled, TotalFare ?

SELECT TOP 1
    Passengers.Id,
    Passengers.Name,
    Passengers.Gender,
	COUNT(Bookings.PassengerId) AS HighestTravelled,
    SUM(Bookings.Fare) AS TotalFare
FROM Passengers
INNER JOIN Bookings ON Bookings.PassengerId = Passengers.Id
GROUP BY Passengers.Id, Passengers.Name, Passengers.Gender
ORDER BY SUM(Bookings.Fare) DESC
