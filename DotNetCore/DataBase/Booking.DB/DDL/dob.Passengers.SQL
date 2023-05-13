--Truncate Table Passengers

CREATE TABLE Passengers
(
	Id Int Identity (1,1), 
	Name VARCHAR(100) Not Null,
	DOB DATETIME Not Null,
	Mobile Varchar (100) Not Null,
	Email Varchar (100) Not Null,
	Gender VARCHAR(100) Not Null,
	Primary Key (Id)	
)

CREATE UNIQUE NONCLUSTERED INDEX [IX _Passengers_Mobile] ON [dbo].[Passengers]
	(
		[Mobile] ASC
	)

CREATE UNIQUE NONCLUSTERED INDEX [IX _Passengers_Email] ON [dbo].[Passengers]
	(
		[Email] ASC
	)










