--Q:2 Fetch all customers records who have more than one duplicate records based on 
-- first name and last name?
SELECT FirstName, LastName, COUNT(*) AS DuplicateCount
FROM Customers
GROUP BY FirstName, LastName
HAVING COUNT(*) > 1
