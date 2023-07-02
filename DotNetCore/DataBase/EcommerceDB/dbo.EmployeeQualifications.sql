Create Table EmployeeQualifications
(
	Id Int Identity(1,1),
	Qualification NVarchar (200) Not Null,
	Primary Key (Id)
)


Insert Into EmployeeQualifications (Qualification)
	Values ('B.Tech')

Insert Into EmployeeQualifications (Qualification)
	Values ('M.Tech')

Insert Into EmployeeQualifications (Qualification)
	Values ('Civil Engineer')

Insert Into EmployeeQualifications (Qualification)
	Values ('Mechanical Engineer')

Insert Into EmployeeQualifications (Qualification)
	Values ('Electrical Engineer')





