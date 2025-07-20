CREATE PROCEDURE Sp_CostMasterCRUD
    @Mode NVARCHAR(50),
    @Id INT = NULL,
    @ServiceTypeId INT = NULL,
    @Cost DECIMAL(18, 2) = NULL,
    @Date DATETIME = NULL,
    @CurrentDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Mode = 'GET_BY_SERVICETYPE'
    BEGIN
        SELECT CostMaster.Id, CostMaster.ServiceTypeId, 
               ServiceTypes.Name, CostMaster.Cost, CostMaster.Date
        FROM CostMaster
        JOIN ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id
        WHERE CostMaster.ServiceTypeId = @ServiceTypeId
        ORDER BY CostMaster.Date DESC;
    END
    ELSE IF @Mode = 'GET_ACTIVE_COST'
    BEGIN
        SELECT TOP 1 CostMaster.ServiceTypeId, ServiceTypes.Name, 
                      CostMaster.Cost, CostMaster.Date
        FROM CostMaster 
        JOIN ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id 
        WHERE CostMaster.ServiceTypeId = @ServiceTypeId 
        AND CostMaster.Date <= @CurrentDate 
        ORDER BY CostMaster.Date DESC;
    END
    ELSE IF @Mode = 'CREATE'
    BEGIN
        INSERT INTO CostMaster (ServiceTypeId, Cost, Date)
        VALUES (@ServiceTypeId, @Cost, @Date);
        
        -- Return the number of affected rows
        SELECT @@ROWCOUNT AS AffectedRows;
    END
END
