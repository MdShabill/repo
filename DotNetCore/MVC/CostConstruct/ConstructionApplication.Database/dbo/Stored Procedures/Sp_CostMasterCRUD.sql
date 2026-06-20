CREATE PROCEDURE Sp_CostMasterCRUD
    @Mode NVARCHAR(50),
    @Id INT = NULL,
    @ServiceTypeId INT = NULL,
    @SiteId INT = NULL,
    @Cost DECIMAL(18,2) = NULL,
    @Date DATETIME = NULL,
    @CurrentDate DATETIME = NULL
AS
BEGIN
SET NOCOUNT ON;
IF @Mode='GET_BY_SERVICETYPE'
BEGIN
    SELECT
        CM.Id,
        CM.ServiceTypeId,
        CM.SiteId,
        ST.Name,
        CM.Cost,
        CM.Date
    FROM CostMaster CM
    INNER JOIN ServiceTypes ST
        ON CM.ServiceTypeId=ST.Id
    WHERE
        CM.ServiceTypeId=@ServiceTypeId
        AND CM.SiteId=@SiteId
    ORDER BY CM.Date DESC;
END
ELSE IF @Mode='GET_ACTIVE_COST'
BEGIN
    SELECT TOP 1
        CM.ServiceTypeId,
        CM.SiteId,
        ST.Name,
        CM.Cost,
        CM.Date
    FROM CostMaster CM
    INNER JOIN ServiceTypes ST
        ON CM.ServiceTypeId=ST.Id
    WHERE
        CM.ServiceTypeId=@ServiceTypeId
        AND CM.SiteId=@SiteId
        AND CM.Date<=@CurrentDate
    ORDER BY CM.Date DESC;
END
ELSE IF @Mode='GET_BY_ID'
BEGIN
    SELECT
        CM.Id,
        CM.ServiceTypeId,
        CM.SiteId,
        ST.Name,
        CM.Cost,
        CM.Date
    FROM CostMaster CM
    INNER JOIN ServiceTypes ST
        ON CM.ServiceTypeId=ST.Id
    WHERE
        CM.Id=@Id
        AND CM.SiteId=@SiteId;
END
ELSE IF @Mode='CREATE'
BEGIN
    INSERT INTO CostMaster
    (
        ServiceTypeId,
        SiteId,
        Cost,
        Date
    )
    VALUES
    (
        @ServiceTypeId,
        @SiteId,
        @Cost,
        @Date
    );
    SELECT @@ROWCOUNT;
END
ELSE IF @Mode='UPDATE'
BEGIN
    UPDATE CostMaster
    SET
        ServiceTypeId=@ServiceTypeId,
        Cost=@Cost,
        Date=@Date
    WHERE
        Id=@Id
        AND SiteId=@SiteId;
    SELECT @@ROWCOUNT;
END
ELSE IF @Mode='DELETE'
BEGIN
    DELETE FROM CostMaster
    WHERE
        Id=@Id
        AND SiteId=@SiteId;
END
END