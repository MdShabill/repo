-- Q:4 How Can We Delete Duplicate Records In A Table Without Id Column?
Select * from Students

WITH DuplicateCTE AS (
  SELECT 
    Name As StudentName,
    ROW_NUMBER() OVER (PARTITION BY Name ORDER BY name) AS RowNum
  FROM students
)
DELETE FROM DuplicateCTE WHERE RowNum > 1