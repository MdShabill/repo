-- Fetch All Employees previous and Current Department Position?
--Approach: 1 here we used CTE in this query and Window function

WITH DepartmentChanges AS (
    SELECT 
        Id, FullName, 
        Department,
        JoiningDate,
        ROW_NUMBER() OVER (PARTITION BY FullName, JoiningDate ORDER BY Id ASC) AS FirstChangeRank,
        ROW_NUMBER() OVER (PARTITION BY FullName, JoiningDate ORDER BY Id DESC) AS LastChangeRank
    FROM
        Employees2
)
SELECT 
    FirstChange.JoiningDate As Date,
    FirstChange.FullName,
    FirstChange.Department AS StartingDepartment,
    LastChange.Department AS EndingDepartment  
FROM 
    DepartmentChanges FirstChange
JOIN
    DepartmentChanges LastChange ON FirstChange.FullName = LastChange.FullName
    AND FirstChange.JoiningDate = LastChange.JoiningDate
WHERE
    FirstChange.FirstChangeRank = 1
    AND LastChange.LastChangeRank = 1
ORDER BY Date

-- Approach:2 Here we used Top function and SubQuery

SELECT DISTINCT
    EmployeeRecord.JoiningDate AS Date,
    EmployeeRecord.FullName,
    
    (SELECT TOP 1 Department
     FROM Employees2 
     WHERE Employees2.FullName = EmployeeRecord.FullName AND Employees2.JoiningDate = EmployeeRecord.JoiningDate
     ORDER BY Id)
    AS FromDepartment,
    
    (SELECT TOP 1 Department
     FROM Employees2
     WHERE Employees2.FullName = EmployeeRecord.FullName AND Employees2.JoiningDate = EmployeeRecord.JoiningDate
     ORDER BY Id DESC)
    AS ToDepartment

FROM (
    SELECT * FROM Employees2
) EmployeeRecord
ORDER BY EmployeeRecord.JoiningDate


-- Approach:5 Here we used Aggregate function and SubQuery and inner join
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
ORDER BY EmployeeRecord.JoiningDate


-- Approach:4 Here we used Aggregate function and SubQuery
SELECT DISTINCT
    EmployeeRecord.JoiningDate AS Date,
    EmployeeRecord.FullName,
    (SELECT Department
     FROM Employees2 
     WHERE Employees2.FullName = EmployeeRecord.FullName AND 
           Employees2.JoiningDate = EmployeeRecord.JoiningDate AND 
           Employees2.Id = (SELECT MIN(Id) 
                            FROM Employees2 
                            WHERE FullName = EmployeeRecord.FullName AND 
                                  JoiningDate = EmployeeRecord.JoiningDate)
    ) AS FromDepartment,
    (SELECT Department
     FROM Employees2
     WHERE Employees2.FullName = EmployeeRecord.FullName AND 
           Employees2.JoiningDate = EmployeeRecord.JoiningDate AND 
           Employees2.Id = (SELECT MAX(Id) 
                            FROM Employees2 
                            WHERE FullName = EmployeeRecord.FullName AND 
                                  JoiningDate = EmployeeRecord.JoiningDate)
    ) AS ToDepartment
FROM Employees2 AS EmployeeRecord
ORDER BY EmployeeRecord.JoiningDate
