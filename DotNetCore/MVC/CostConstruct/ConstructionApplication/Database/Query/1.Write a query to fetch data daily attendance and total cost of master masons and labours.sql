-- Write a query to fetch data daily attendance and total costs for Master Masons and Labours

SELECT
	DailyAttendance.Date,
	DailyAttendance. TotalMasterMason AS MistryCount,
	DailyAttendance. TotalLabour AS LabourCount, 
	DailyAttendance.TotalMasterMason * CostMaster.MasterMasonCost As MasterMasonAmount,
	DailyAttendance.TotalLabour * CostMaster.LabourCost As LabourAmount,
	(DailyAttendance.TotalMasterMason * CostMaster.MasterMasonCost + DailyAttendance.TotalLabour * CostMaster.LabourCost) As TotalAmount
From
	DailyAttendance,
	CostMaster