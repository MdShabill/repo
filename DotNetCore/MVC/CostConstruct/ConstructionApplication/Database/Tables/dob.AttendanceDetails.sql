Create Table AttendanceDetails 
(
    Id INT Identity(1,1),
    AttendanceId Int Not Null,
    Name NVarchar(200) Not Null,
    Role NVarchar(200) Null,

	Primary Key(Id),
    Foreign Key (AttendanceId) References DailyAttendance(Id)
)
