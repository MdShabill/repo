CREATE TABLE Employees1
(
	Id int IDENTITY(1,1) NOT NULL,
	FullName NVarchar(200) NOT NULL,
	FatherName NVarchar(200) NOT NULL,
	Email NVarchar(200) Not NULL,
    PRIMARY KEY (Id)
)