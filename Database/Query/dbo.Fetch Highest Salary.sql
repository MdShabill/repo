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

--Approach: 3
-- Find the 2nd highest salary from the employee table by Using Aggregate function instead of CTE?

SELECT ID, FullName, Salary AS SecondHighestSalary
FROM Employees
WHERE Salary = (
    SELECT MAX(Salary)
    FROM Employees
    WHERE Salary < (SELECT MAX(Salary) FROM Employees)
)

--Approach: 4
-- Find the 2nd highest salary from the employee table by Using Window Function instead of Aggregate function?

WITH SecondHighestSalary AS (
    SELECT 
        ID, 
        FullName, 
        Salary,
        DENSE_RANK() OVER (ORDER BY Salary DESC) AS RankSalary
    FROM Employees
)
SELECT ID, FullName, Salary
FROM SecondHighestSalary
WHERE Ranksalary = 5


