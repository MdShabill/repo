--- Write a query to fetch the records of highest 1,2,3 employees salary
--Approach: 1

SELECT  FullName, Age, Gender, JobTitle, Salary
FROM    ( SELECT ROW_NUMBER() OVER ( ORDER BY salary DESC ) AS RowNum, *
          FROM      Employees
        ) AS RowConstrainedResult
WHERE   RowNum >= 1
    AND RowNum <= 3
ORDER BY RowNum

--Approach: 2

WITH RankedSalaries AS (
    SELECT Salary, ROW_NUMBER() OVER (ORDER BY Salary DESC) AS SalaryRank
    FROM Employees
)
SELECT Salary
FROM RankedSalaries
WHERE SalaryRank >= 1 And SalaryRank <= 3



