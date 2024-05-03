--Q:4 Delete all duplicate records from customers table except Last insert records
-- Approach-1
With RankedCustomers As (
    Select Id, 
        ROW_NUMBER() Over (Partition By FirstName, LastName Order By Id Desc) As RowNum
    From 
        Customers
)
Delete From RankedCustomers
Where RowNum > 1

-- Approach-2
Delete From Customers
Where Id NOT IN (
    Select MAX(Id)
    From Customers
    Group By FirstName, LastName
)









Truncate Table Customers
