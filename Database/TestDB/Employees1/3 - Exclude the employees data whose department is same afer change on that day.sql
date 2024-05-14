-- Exclude the employees data whose department is same afer change on that day

-- Approach:1 Here we used Aggregate function and SubQuery and inner join

SELECT 
    EmployeeRecord.JoiningDate AS Date,
    EmployeeRecord.FullName,
    MinDept.Department AS FromDepartment,
    MaxDept.Department AS ToDepartment
FROM (
    SELECT 
        JoiningDate,
        FullName,
        MIN(Id) AS MinId,
        MAX(Id) AS MaxId
    FROM Employees2
    GROUP BY JoiningDate, FullName
) AS EmployeeRecord
INNER JOIN Employees2 AS MinDept ON EmployeeRecord.MinId = MinDept.Id
INNER JOIN Employees2 AS MaxDept ON EmployeeRecord.MaxId = MaxDept.Id
WHERE MinDept.Department != MaxDept.Department
ORDER BY EmployeeRecord.JoiningDate

-- Approach:2 Here we used Aggregate function and SubQuery

SELECT Date, FullName, FromDepartment, ToDepartment
FROM (
    SELECT DISTINCT
        EmployeeRecord.JoiningDate AS Date,
        EmployeeRecord.FullName,
        (SELECT Department
         FROM Employees2 
         WHERE Employees2.FullName = EmployeeRecord.FullName AND 
               Employees2.JoiningDate = EmployeeRecord.JoiningDate AND 
               Employees2.Id = MIN(EmployeeRecord.Id) 
        ) AS FromDepartment,
        (SELECT Department
         FROM Employees2
         WHERE Employees2.FullName = EmployeeRecord.FullName AND 
               Employees2.JoiningDate = EmployeeRecord.JoiningDate AND 
               Employees2.Id =  MAX(EmployeeRecord.Id) 
        ) AS ToDepartment
    FROM Employees2 AS EmployeeRecord
    GROUP BY EmployeeRecord.JoiningDate, EmployeeRecord.FullName 
) AS Departments
WHERE FromDepartment != ToDepartment
ORDER BY Date

-- Approach:3 Here we used Aggregate function and CTE and inner join

WITH EmployeeCTE AS (
    SELECT 
        JoiningDate,
        FullName,
        MIN(Id) AS MinId,
        MAX(Id) AS MaxId
    FROM Employees2
    GROUP BY JoiningDate, FullName
)
SELECT 
    EmployeeRecord.JoiningDate AS Date,
    EmployeeRecord.FullName,
    MinDept.Department AS FromDepartment,
    MaxDept.Department AS ToDepartment
FROM EmployeeCTE AS EmployeeRecord
INNER JOIN Employees2 AS MinDept ON EmployeeRecord.MinId = MinDept.Id
INNER JOIN Employees2 AS MaxDept ON EmployeeRecord.MaxId = MaxDept.Id
WHERE MinDept.Department != MaxDept.Department
ORDER BY EmployeeRecord.JoiningDate
