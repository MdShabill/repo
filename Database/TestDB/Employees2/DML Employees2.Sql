Create Table Employees2
(
	Id Int Identity(1,1),
	FullName NVarchar (200) Not Null,
	Department NVarchar (200) Not Null,
	JoiningDate DateTime Not Null,

	Primary Key (Id)
)