CREATE TABLE Teachers
(
	Id INT IDENTITY(1,1),
	FullName NVARCHAR(100) Not Null,
	Age INT,
	Gender Int Not Null,
	Email Nvarchar(100) Not Null,
	MobileNumber NVarchar Not Null,
    SchoolName NVARCHAR(100),
	Department NVARCHAR(100),
	Salary INT,
	PRIMARY KEY (Id),
	CONSTRAINT UQ_Teachers_Email UNIQUE (Email),
	CONSTRAINT UQ_Teachers_MobileNumber UNIQUE (MobileNumber)
)
