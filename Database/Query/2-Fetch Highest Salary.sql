--Q:1 Find the employee details which employee have 2nd highest salary?

--Approach: 1
SELECT TOP 1 * FROM
	(SELECT TOP 2 * FROM Employees
	ORDER BY Salary DESC) AS SecondHighestSalary
ORDER BY Salary ASC


-- Find the 2nd highest salary from the employee table by Using CTE instead of Subquery?

--Approach: 2
WITH SecondHighestSalary AS (
    SELECT TOP 2 * 
    FROM Employees
    ORDER BY Salary DESC
)
SELECT TOP 1 *
FROM SecondHighestSalary
ORDER BY Salary ASC


-- Find the 2nd highest salary from the employee table by Using Aggregate function instead of CTE?

--Approach: 3
SELECT ID, FullName, Salary AS SecondHighestSalary
FROM Employees
WHERE Salary = (
    SELECT MAX(Salary)
    FROM Employees
    WHERE Salary < (SELECT MAX(Salary) FROM Employees)
)

-- Find the 2nd highest salary from the employee table by Using Window Function instead of Aggregate function?

--Approach: 4
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
WHERE Ranksalary = 2


