CREATE TABLE WorkExperiences 
(
    Id INT Identity(1,1),
    JobSeekerId INT Not Null,
    Position NVARCHAR(200) Not Null,
    Company NVARCHAR(200) Not Null,
    StartYear INT,
    EndYear INT

	PRIMARY KEY(Id),
	FOREIGN KEY (JobSeekerId) REFERENCES JobSeekers (Id)
)