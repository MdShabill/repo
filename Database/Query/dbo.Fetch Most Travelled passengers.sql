--Q3: Fetch Those Passengers Who Have Booked The Most Number Of Tickets With His Total Fare, Name And Gender  ?

SELECT TOP 1
	Passengers.Id , Passengers.Name , Passengers.Gender,
	COUNT(Bookings.PassengerId) AS 'Most Travelled', 
	SUM(Bookings.Fare) AS 'Total Fare Of Most Travelled'
FROM Passengers
	INNER JOIN Bookings ON Bookings.PassengerId = Passengers.Id
	
GROUP BY Passengers.Id , Passengers.Name , Passengers.Gender
ORDER BY 'Most Travelled' DESC