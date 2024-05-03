
--Q:1 Fetch all customer counts of duplicate names based on first name and last name? 
SELECT FirstName, LastName, COUNT(*) AS DuplicateCount
FROM Customers
GROUP BY FirstName, LastName
