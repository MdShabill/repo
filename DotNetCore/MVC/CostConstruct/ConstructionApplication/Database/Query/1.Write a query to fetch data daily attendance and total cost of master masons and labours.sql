-- Write a query to fetch data daily attendance and total costs for Master Masons and Labours

SELECT
    DailyAttendance.Date,
	DailyAttendance. TotalMasterMason AS MasterMasonCount,
	DailyAttendance. TotalLabour AS LabourCount,
	DailyAttendance.TotalMasterMason * CostMaster.MasterMasonCost As MasterMasonAmount,
	DailyAttendance.TotalLabour * CostMaster.LabourCost As LabourAmount,
	(DailyAttendance.TotalMasterMason * CostMaster.MasterMasonCost + DailyAttendance.TotalLabour * CostMaster.LabourCost) As TotalAmount
FROM
    DailyAttendance 
JOIN
    CostMaster ON CostMaster.Date = (
        SELECT MAX(Date)
        FROM CostMaster
        WHERE CostMaster.Date <= DailyAttendance.Date
    )
ORDER BY
    DailyAttendance.Date