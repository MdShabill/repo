CREATE TABLE Educations 
(
    Id INT Identity(1,1),
    JobSeekerId INT Not Null,
    Degree NVARCHAR(200) Not Null,
    School NVARCHAR(200) Not Null,
    GraduationYear INT Not Null,

	PRIMARY KEY(Id),
	FOREIGN KEY (JobSeekerId) REFERENCES JobSeekers (Id)
)