Create table Students
(
	Id Int identity (1,1),
	FullName Nvarchar (100),
	Gender Int,
	Email Nvarchar (100),
	Password NVarchar (100),
	RegistrationDate DateTime,
	PRIMARY KEY (Id),
	CONSTRAINT UQ_Students_Email UNIQUE (Email),
)
