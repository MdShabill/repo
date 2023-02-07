CREATE TABLE Teachers
(
	Id INT IDENTITY(1,1),
	FullName NVARCHAR(100) Not Null,
	Email Nvarchar(100) Not Null,
	Age INT,
	Gender Int Not Null,
    SchoolName NVARCHAR(100),
	Department NVARCHAR(100),
	Salary INT,
)
