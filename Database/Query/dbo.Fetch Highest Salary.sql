--Approach: 1
--Q:2 Find the employee details which employee have 2nd highest salary?

SELECT TOP 1 * FROM
	(SELECT TOP 2 * FROM Employees
	ORDER BY Salary DESC) AS SecondHighestSalary
ORDER BY Salary ASC

--Approach: 2
-- Find the 2nd highest salary from the employee table by Using CTE instead of Subquery?

WITH SecondHighestSalary AS (
    SELECT TOP 2 * 
    FROM Employees
    ORDER BY Salary DESC
)
SELECT TOP 1 *
FROM SecondHighestSalary
ORDER BY Salary ASC
