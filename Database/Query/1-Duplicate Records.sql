--Q:1 Write a query to retrieve duplicate Salary records from the Employees table?

--Approach: 1
SELECT Salary, COUNT(*) AS DuplicateCount
FROM Employees
GROUP BY Salary
HAVING COUNT(*) > 1;

--Approach: 2 --SubQuery
Select Employees.FullName, Employees.Salary
From Employees
Inner Join(
	Select Salary
	From Employees
	Group By Salary
	Having Count(*) > 1
) Duplicates On Employees.Salary = Duplicates.Salary
Order By Employees.Salary, Employees.FullName

--Approach: 3 -- Using CTE
With SalaryCount As(
	Select Id, FullName, Salary,
	Count(*) Over (Partition By Salary) As SalaryCount
	From Employees
)
Select Distinct Id, FullName, Salary
From SalaryCount
Where SalaryCount > 1
Order By Id, FullName, Salary

--Approach: 4 --using Window Function
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
