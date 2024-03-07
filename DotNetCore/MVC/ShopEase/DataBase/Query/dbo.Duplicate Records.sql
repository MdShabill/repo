--Approach: 1
--Q:1 Write a query to retrieve duplicate Salary records from a table?

SELECT Salary, COUNT(*) AS DuplicateCount
FROM Employees
GROUP BY Salary
HAVING COUNT(*) > 1;

--Approach: 2
--Include FullName in the result so now result should have fullnamr, salary and 

--FullName | Salary
---------------------
--John     | 40000
--Allen    | 40000
--Henry    | 65000
--Marry    | 65000
--Tom      | 65000

Select Employees.FullName, Employees.Salary
From Employees
Inner Join(
	Select Salary
	From Employees
	Group By Salary
	Having Count(*) > 1
) Duplicates On Employees.Salary = Duplicates.Salary
Order By Employees.Salary, Employees.FullName

--Approach: 3
--Fetched the same result but instead of using SubQuery we used CTE in this query

With SalaryCount As(
	Select Id, FullName, Salary,
	Count(*) Over (Partition By Salary) As SalaryCount
	From Employees
)
Select Distinct Id, FullName, Salary
From SalaryCount
Where SalaryCount > 1
Order By Id, FullName, Salary

--Approach: 4
--Fetched the same result but instead of using Over and Partition we used Inner Join in this query

WITH SalaryCounts AS (
    SELECT Salary, COUNT(*) AS DuplicateCount
    FROM Employees
    GROUP BY Salary
    HAVING COUNT(*) > 1
)
SELECT Employees.FullName, Employees.Salary
FROM Employees
INNER JOIN SalaryCounts ON Employees.Salary = SalaryCounts.Salary
ORDER BY Employees.Salary, Employees.FullName
