
CREATE TABLE Movies
(
	Id int IDENTITY(1,1) NOT NULL,
	MovieName nvarchar (200) NOT NULL,
	DirectorName nvarchar (200) NOT NULL,
	ActorId int NOT NULL,
	ActressId int Not Null,
    PRIMARY KEY (Id),
	
)

Alter Table Movies
Add FOREIGN KEY (ActorId) REFERENCES Actors (Id)

Alter Table Movies
Add FOREIGN KEY (ActressId) REFERENCES Actresses (Id)