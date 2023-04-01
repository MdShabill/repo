Create Table Movies
(
	Id Int Identity(1,1),
	ActorName NVarchar(100),
	ActressName NVarchar(100),
	Title NVarchar(100),
	MovieType int,
	ReleaseDate DateTime,
	Primary Key (Id)
)