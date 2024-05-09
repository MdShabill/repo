-- Fetch All Employees previous and Current Department Position?

--Approach: 1 here we use CTE and Window function
WITH DepartmentChanges AS (
    SELECT 
        Id, FullName, FromDepartment,
        ToDepartment, JoiningDate,
        ROW_NUMBER() OVER (PARTITION BY FullName, JoiningDate ORDER BY Id ASC) AS FirstChangeRank,
        ROW_NUMBER() OVER (PARTITION BY FullName, JoiningDate ORDER BY Id DESC) AS LastChangeRank
    FROM
        Employees1
)
SELECT 
	FirstChange.JoiningDate,
    FirstChange.FullName,
    FirstChange.FromDepartment AS StartingDepartment,
    LastChange.ToDepartment AS EndingDepartment  
FROM 
    DepartmentChanges FirstChange
JOIN
    DepartmentChanges LastChange ON FirstChange.FullName = LastChange.FullName
    AND FirstChange.JoiningDate = LastChange.JoiningDate
WHERE
    FirstChange.FirstChangeRank = 1
    AND LastChange.LastChangeRank = 1
ORDER BY JoiningDate

--Approach: 2 Here we use SubQuery and Window function
SELECT
    FirstChange.JoiningDate,
    FirstChange.FullName,
    FirstChange.FromDepartment AS StartingDepartment,
    LastChange.ToDepartment AS EndingDepartment
FROM 
    (SELECT 
         Id, FullName, FromDepartment, ToDepartment, JoiningDate,
         ROW_NUMBER() OVER (PARTITION BY FullName, JoiningDate ORDER BY Id ASC) AS FirstChangeRank
     FROM 
         Employees1) AS FirstChange
JOIN 
    (SELECT 
         Id, FullName, FromDepartment, ToDepartment, JoiningDate,
         ROW_NUMBER() OVER (PARTITION BY FullName, JoiningDate ORDER BY Id DESC) AS LastChangeRank
     FROM 
         Employees1) AS LastChange ON FirstChange.FullName = LastChange.FullName
                        AND FirstChange.JoiningDate = LastChange.JoiningDate
WHERE 
    FirstChange.FirstChangeRank = 1
    AND LastChange.LastChangeRank = 1
ORDER BY 
    JoiningDate