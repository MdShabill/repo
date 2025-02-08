CREATE PROCEDURE Sp_CostMasterCRUD
    @Mode NVARCHAR(50),
    @Id INT = NULL,
    @JobCategoryId INT = NULL,
    @Cost DECIMAL(18, 2) = NULL,
    @Date DATETIME = NULL,
    @CurrentDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Mode = 'GET_BY_JOBCATEGORY'
    BEGIN
        SELECT CostMaster.Id, CostMaster.JobCategoryId, 
               JobCategories.Name, CostMaster.Cost, CostMaster.Date
        FROM CostMaster
        JOIN JobCategories ON CostMaster.JobCategoryId = JobCategories.Id
        WHERE CostMaster.JobCategoryId = @JobCategoryId
        ORDER BY CostMaster.Date DESC;
    END
    ELSE IF @Mode = 'GET_ACTIVE_COST'
    BEGIN
        SELECT TOP 1 CostMaster.JobCategoryId, JobCategories.Name, 
                      CostMaster.Cost, CostMaster.Date
        FROM CostMaster 
        JOIN JobCategories ON CostMaster.JobCategoryId = JobCategories.Id 
        WHERE CostMaster.JobCategoryId = @JobCategoryId 
        AND CostMaster.Date <= @CurrentDate 
        ORDER BY CostMaster.Date DESC;
    END
    ELSE IF @Mode = 'CREATE'
    BEGIN
        INSERT INTO CostMaster (JobCategoryId, Cost, Date)
        VALUES (@JobCategoryId, @Cost, @Date);
        
        -- Return the number of affected rows
        SELECT @@ROWCOUNT AS AffectedRows;
    END
END
