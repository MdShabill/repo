Create Table Employees1
(
	Id Int Identity(1,1),
	FullName NVarchar(200) Not Null, 
	FromDepartment NVarchar(200) Not Null,
	ToDepartment NVarchar(200) Not Null,
	JoiningDate DateTime Not Null,
	
	Primary Key(Id)
)
