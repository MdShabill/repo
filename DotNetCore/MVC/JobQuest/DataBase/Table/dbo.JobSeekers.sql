CREATE TABLE JobSeekers 
(
    Id INT Identity(1,1),
    FirstName NVARCHAR(200) Not Null,
    LastName NVARCHAR(200),
    Email NVARCHAR(200) UNIQUE Not Null,
	Password NVARCHAR(200) Not Null,
    Phone NVARCHAR(200) Not Null,
    DOB NVarchar(200)Not Null,

	Primary Key(Id)
)
