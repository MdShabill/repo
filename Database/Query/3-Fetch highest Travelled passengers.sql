--Approach: 1
--Q3: Fetch Those Passengers Who has highest Travelled. 
		--Display PassengerId, Name, Gender, HighestTravelled, TotalFare ?  

SELECT TOP 1
	Passengers.Id , Passengers.Name , Passengers.Gender,
	COUNT(Bookings.PassengerId) AS 'Most Travelled', 
	SUM(Bookings.Fare) AS 'Total Fare Of Most Travelled'
FROM Passengers
	INNER JOIN Bookings ON Bookings.PassengerId = Passengers.Id
	
GROUP BY Passengers.Id , Passengers.Name , Passengers.Gender
ORDER BY 'Most Travelled' DESC

--Approach: 2
--Q3: Fetch Those Passengers Who has highest Travelled by using CTE. 
		--Display PassengerId, Name, Gender, HighestTravelled, TotalFare ?

WITH PassengerBookings AS (
    SELECT
    Passengers.Id,
    Passengers.Name,
    Passengers.Gender,
    COUNT(Bookings.PassengerId) AS MostTravelled,
    SUM(Bookings.Fare) AS TotalFareOfMostTravelled
    FROM Passengers
    INNER JOIN Bookings ON Bookings.PassengerId = Passengers.Id
    GROUP BY Passengers.Id, Passengers.Name, Passengers.Gender
)
SELECT TOP 1
    Id,
    Name,
    Gender,
    MostTravelled,
    TotalFareOfMostTravelled
FROM PassengerBookings
ORDER BY MostTravelled DESC
