--Q:1 Find the employee details which employee have 2nd highest salary?

--Approach: 1
SELECT TOP 1 * FROM
	(SELECT TOP 2 * FROM Employees
	ORDER BY Salary DESC) AS SecondHighestSalary
ORDER BY Salary ASC

--Approach: 2 --Using CTE
WITH SecondHighestSalary AS (
    SELECT TOP 2 * 
    FROM Employees
    ORDER BY Salary DESC
)
SELECT TOP 1 *
FROM SecondHighestSalary
ORDER BY Salary ASC

--Approach: 3 --Using Aggregate function
SELECT ID, FullName, Salary AS SecondHighestSalary
FROM Employees
WHERE Salary = (
    SELECT MAX(Salary)
    FROM Employees
    WHERE Salary < (SELECT MAX(Salary) FROM Employees)
)

--Approach: 4 --Using Window Function
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


