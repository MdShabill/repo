-- NVARCHAR Takes 2(bytes) For Each Character
-- NVARCHAR Can Contain Only 4000 Characters   
-- NVARCHAR Can Contain Multipal Language Characters

DECLARE @Shabill NVARCHAR (4000) = N'الله أكبر'
SELECT @Shabill AS Value,
DATALENGTH (@Shabill) AS MemorySize,
Len(@Shabill) AS LengthOfValue
------------------------------------------------
-- VARCHAR Takes 1(Byte) For Each Characters
-- VARCHAR Can Contain Only 8000 Characters
-- VARCHAR Is A Variable Length

DECLARE @Salman VARCHAR (8000) = 'الله أكبر'
SELECT @Salman AS Value,
DATALENGTH (@Salman) AS MemorySize,
Len(@Salman) AS LengthOfValue

-------------------------------------------------

Select Passengers.Id, Passengers.Name
From Passengers
Left Join Bookings On Bookings.PassengerId = Passengers.Id 
where Bookings.PassengerId Is Null
---------------------------------------------
Select 
	Passengers.Name,
	Passengers.Gender,
	Bookings.PassengerId,
	Bookings.TrainId,
	Bookings.PNR,
	Bookings.BookingDate,
	Bookings.ScheduleStartDate,
	Bookings.ScheduleEndDate,
	Bookings.Travelled

From Passengers

	Left Join Bookings On Bookings.PassengerId = Passengers.Id	
------------------------------------------------
------------------------------------------------
Select
	Passengers.Name,
	Passengers.Gender,
	Bookings.PNR,
	Bookings.BookingDate,
	Bookings.ScheduleStartDate,
	Bookings.ScheduleEndDate,
	Bookings.Travelled,
	Trains.Name,
	Trains.TrainNo,
	Trains.SourceCity,
	Trains.DestinationCity

From Bookings
	Inner Join Passengers On Bookings.PassengerId = Passengers.Id
	Inner Join Trains On Bookings.TrainId = Trains.Id

Where Passengers.Name In ('Shabill','Zahid Ahmed','Parveen') 


--Q:- I Want To Show As A Result Average Fare Of ticket But Only Passengers Id 3,4,10,14 ?

Select 
	Passengers.Name,
	Bookings.PassengerId,
	Bookings.TrainId,
	Avg(Bookings.Fare) As AverageFare

From Bookings

	Inner Join Passengers On Bookings.PassengerId = Passengers.Id
Where Passengers.Id In (3, 4, 10, 14)
Group By Passengers.Name,Bookings.PassengerId,Bookings.TrainId
Order By AverageFare Desc
--------------------------------


Select 
	Trains.Id as TrainId,
	Trains.Name,
	Count(TrainStoppages.TrainId) As TrainStoppageCount,
	Avg(Bookings.Fare) As AverageFare
	--, Case
	--	When Count(TrainStoppages.TrainId) = @MaxStopCount Then 'Highest Train Stoppage'
	--	When Count(TrainStoppages.TrainId)  = @MinStopCount Then 'Lowest Train Stoppage'	
	--	End As TrainStoppages

From Trains
	Inner Join TrainStoppages On TrainStoppages.TrainId = Trains.Id
	Inner Join Bookings On Bookings.TrainId = Trains.Id 
Group By Trains.Id, Trains.Name
Order By AverageFare Desc
---------------------------------------
--Q:2 count the numbner of train Whose stoppage station is 'Samastipur' 'Patna','Howra' and 
--    fare greater than 2000(2nd requirement)

SELECT 
	TrainStoppages.Station,
	Count(TrainStoppages.TrainId) As CountTrains
	--Bookings.Fare

From Trains

	Inner Join TrainStoppages On TrainStoppages.TrainId = Trains.Id
	Inner Join Bookings On Bookings.TrainId = Trains.Id

Where TrainStoppages.Station In ('Samastipur', 'Patna','Howra') and Bookings.Fare > 2000
Group By TrainStoppages.Station
-------------------------------------------
Select 
	Trains.Id, 
	Trains.Name,
	Count(TrainStoppages.Id) As StoppageCount,
	MAX(TrainStoppages.Station) AS AA

From TrainStoppages
	Inner Join Trains On TrainStoppages.TrainId = Trains.Id

Group By Trains.Id,Trains.Name
Having MIN(TrainStoppages.Station) In ('Samastipur', 'MuzaffarPur', 'Howra')
--Where TrainStoppages.Station In ('Samastipur', 'MuzaffarPur', 'Howra')
------------------------------------------

Select 
	Bookings.PassengerId,
	Passengers.Name,
	Count(Bookings.TicketId) As 'Count Passenger Ticket',
	Sum(Bookings.Fare) As 'Totl Fare',
	Min(Bookings.Fare) As 'Minimum Fare',
	Max(Bookings.Fare) As 'Maximum Fare'
	
From Bookings

	Inner Join Passengers On Bookings.PassengerId = Passengers.Id

Group By Bookings.PassengerId,Passengers.Name

-----------------------------------------------------
--Q: we need passenger id, Passenger gender and in gender where 'M' we want as a result 'Mr' and where 'F' = 'Mrs' and 
--add with passenger name In Upper Case ?

SELECT Bookings.PassengerId ,

CASE
	WHEN Gender = 'M' THEN 'MR, ' + Upper(Name)

	WHEN Gender = 'F' THEN 'MRS, ' + Upper(Name)

END AS 'Full Name',
Passengers.Gender


FROM Bookings

	INNER JOIN Passengers ON Bookings.PassengerId = Passengers.Id
-------------------------------------------------------------------

Select 
	TicketId, PassengerId, TrainId, PNR, 
	BookingDate, Fare, ScheduleStartDate, 
	ScheduleEndDate, Class, CoachNo, SeatNo,
    Travelled, ActualStartDate, ActualEndDate,

	Cast(TicketId As Varchar(50)) + '- ' + Cast (PassengerId As Varchar(50)) + '- ' + Cast(TrainId As VArchar(50)) + '- ' +
	Cast(PNR As Varchar(50)) + '- ' + Cast(BookingDate As Varchar(50)) + '- ' + Cast(Fare as Varchar (50)) + '- ' +
	Cast(ScheduleStartDate As Varchar (50)) + '- ' + Cast(ScheduleEndDate As Varchar (50)) + '- ' + REVERSE(Class) + '- ' + 
	CoachNo + '- ' + Substring(SeatNo, 1, 2) + '- ' + Cast(Travelled As VArchar (50)) + '- ' + Cast(IsNull(ActualStartDate, 0) As Varchar(50)) + 
	'- ' + Cast(IsNull(ActualEndDate, 0) As Varchar(50)) As 'Booking Detail'

From Passengers

	left Join Bookings On Bookings.PassengerId = Passengers.Id

---------------------------------------------
-- Fetch Those Passengers Whose 1 condition (Age Under 35 And Booking Date At Least Under 5 Month back) OR
-- 2 Condition (Train should Rajdhani or speical and Class should 1st or 2nd)
Select 
    Passengers.Name ,
	Passengers.DOB,
    Trains.TrainTypes,
	Bookings.BookingDate,
	Bookings.Compartment,
	Count(Passengers.Id) As TotalPassenger
From Bookings

	Inner Join Passengers on Bookings.PassengerId = Passengers.Id
	Inner Join Trains On Bookings.TrainId = Trains.Id
Where 
	(Passengers.DOB >= DateAdd(Year, -35, Getdate())) 
	And (Bookings.BookingDate >= DateAdd(MONTH, -5, GetDate())) 
	Or (Trains.TrainTypes In ('Rajdhani Exp', 'Special') And Bookings.Compartment In ('1 Tier', '2 Tier')) 
Group By Passengers.Name , Passengers.DOB, Trains.TrainTypes,
		 Bookings.BookingDate,Bookings.Compartment

-------------------------------------------
Update Teachers
Set Salary = IsNull(Salary,0)+500

Select Id, FullName, Department, Salary
 
From Teachers

-----------------------------
--Q.Write sql query by using variable
--.I want to give 5% Extra bonus of Salary to each teachers Whose department is Computer and Mathematics
--and Show it in a seprate column by the name Of 'Bonus Amount'

Alter Table Teachers
Alter Column salary INT Null

Update Teachers
	Set Salary = Null
Where Id In (3,4,6)
-----------------------------------------------
-------------Local VAriable-------------------
Declare @Bonus Int
	Set @Bonus = 5

	Select Id, FullName, Department, Salary,
Case
	When Salary Is Null  Then '1000'
	Else @Bonus * Salary / 100
End As 'Bonus Salary'

From Teachers

Where Department In ('Computer', 'Mathematics')

-----------------------------------------
---------Information Schema----------------------

-- List All Columns Where datatype is Int
Select TABLE_NAME, COLUMN_NAME, DATA_TYPE  from INFORMATION_SCHEMA.Columns
Where DATA_TYPE = 'Int'

--List All Columns Where DataType Is DateTime 
Select TABLE_NAME, COLUMN_NAME, DATA_TYPE  from INFORMATION_SCHEMA.Columns
Where DATA_TYPE = 'DateTime'
 
-- List All Columns Where Column Name Has 'Name'
Select TABLE_NAME, COLUMN_NAME  from INFORMATION_SCHEMA.Columns
Where COLUMN_NAME Like '%Name%'
--------------------------------------------
Select Count(Passengers.Id) As NullPassenger

From Passengers

Left Join Bookings On Bookings.PassengerId = Passengers.Id

Where Bookings.PassengerId Is Null

-------------------------------------
--Q.Find the Percentage of those Passengers who have not booked any ticket

DECLARE @TotalPassengers INT
DECLARE @TotalNotBookedPassengers INT


SELECT  @TotalPassengers = COUNT(Passengers.Id)  
FROM Passengers

SELECT  @TotalNotBookedPassengers= count(Passengers.Id)
 FROM Passengers
LEFT JOIN Bookings ON Passengers.Id = Bookings.PassengerId
WHERE Bookings.PassengerId IS NULL

SELECT  @TotalPassengers  'Total Passengers'
SELECT  @TotalNotBookedPassengers  'Passengers not booked'
SELECT  @TotalNotBookedPassengers  * 100 / @TotalPassengers  'Passengers not booked %'
-----------------------------------------
---Q Find The Total Marks And Rank Of Each Student From Result Table ?

Select FullName, Roll, Mathematics, Science, English, Hindi, Computer, SocialScience,
(Mathematics + Science + English + Hindi + Computer + SocialScience) As 'Total Marks',

Rank() Over ( Order By (Mathematics + Science + English + Hindi + Computer + SocialScience) DESC) As 'Marks Rank',

DENSE_RANK() Over (Order By (Mathematics + Science + English + Hindi + Computer + SocialScience) DESC) As 'Dence Rank'

From Results
-------------------------------------------

---Q Find The Total Marks And Rank Of Each Student From Result Table ?

SELECT FullName, Roll, Mathematics, Science, English, Hindi, Computer, SocialScience,
(Mathematics + Science + English + Hindi + Computer + SocialScience) AS 'Total Marks',

Rank() OVER ( ORDER BY (Mathematics + Science + English + Hindi + Computer + SocialScience) DESC) AS 'Rank',

DENSE_RANK() OVER (ORDER BY (Mathematics + Science + English + Hindi + Computer + SocialScience) DESC) AS 'Dence Rank',

ROW_NUMBER() OVER (ORDER BY (Mathematics + Science + English + Hindi + Computer + SocialScience) DESC) AS 'Row Number'

FROM Results
-----------------------------------------------
Select Passengers.Id, Passengers.Name, Passengers.Gender, Bookings.Fare,

RANK() Over (Order By (Bookings.Fare)DESC) As Rank,
DENSE_RANK() OVER (ORDER BY (Bookings.Fare)DESC) AS DenseRank,
ROW_NUMBER() Over (Order By (Bookings.Fare)DESC) As RowNumber

From Bookings
Inner Join Passengers On Bookings.PassengerId = Passengers.Id

------------------------------------------------------
-- Use Coalesce With Multipal Columns To Get Only Not Null Values ?

-- Select * From Students
SELECT 
	Id, FullName,
	Coalesce(MobileNo, HomeMobileNo, ParentMobileNo, 'NA') AS ContactDetail
FROM Students
---------------------------------------------------------
-- How React Null = Null With IF And ELSE 
If(Null = Null)
Begin
	Print 'Hello'
End
Else
Begin
	Print 'Welcome'
End

-------------------------------------------------
-- CREATE VIEW
-- SELECT * FROM Customers1
CREATE VIEW CUSTOMERS_VIEW
AS
SELECT * FROM Customers1

-- HOW TO UPDATE THE METADATA OF VIEW IN SQL
SELECT * FROM CUSTOMERS_VIEW
ALTER TABLE Customers1 ADD Country VARCHAR(100)

EXEC sp_refreshview CUSTOMERS_VIEW

-- HOW TO CREATE SCHEMA BINDING IN VIEW
CREATE VIEW CUSTOMERS_VIEW
AS
SELECT * FROM Customers1

















































































