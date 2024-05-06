--Q:3 Delete all duplicate records from customers table except first insert records
--Approach:1

Delete From Customers
Where Id NOT IN (
    Select MIN(Id)
    From Customers
    Group By FirstName, LastName
)

--Approach:2
With RankedCustomers As (
    Select Id, 
        ROW_NUMBER() Over (Partition By FirstName, LastName Order By Id) As RowNum
    FROM 
        Customers
)
Delete From RankedCustomers
Where RowNum > 1