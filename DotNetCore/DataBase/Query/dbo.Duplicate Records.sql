--Q:1 Write a query to retrieve duplicate Salary records from a table?

SELECT Salary, COUNT(*) AS DuplicateCount
FROM Employees
GROUP BY Salary
HAVING COUNT(*) > 1;

----
--Include FullName in the result so now result should have fullnamr, salary and 

--FullName | Salary
---------------------
--John     | 40000
--Allen    | 40000
--John     | 65000
--John     | 65000
--John     | 65000
