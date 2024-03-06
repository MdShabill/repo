--Q:2 Find the 2nd highest salary from teachers table?

SELECT TOP 1 * FROM
	(SELECT TOP 2 * FROM Employees
	ORDER BY Salary DESC) AS SecondHighestSalary
ORDER BY Salary ASC