--Q:1 How can you use a subquery to find employees whose salary is above the average salary?

SELECT 
    Id AS employeeid,
    FullName AS employeeName,
    Salary
FROM 
    Employees
WHERE 
    Salary > (SELECT AVG(Salary) FROM Employees)