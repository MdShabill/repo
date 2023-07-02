CREATE TABLE Employees1
(
	Id int IDENTITY(1,1) NOT NULL,
	FullName nvarchar(200) NOT NULL,
	FatherName nvarchar(200) NOT NULL,
	Email nvarchar(200) NULL,
	CountryId int NULL,
    PRIMARY KEY (Id)
)

Alter Table Employees1
Add QualificationId int


ALTER TABLE Employees1
ADD FOREIGN KEY(QualificationId) REFERENCES Qualifications(Id)








