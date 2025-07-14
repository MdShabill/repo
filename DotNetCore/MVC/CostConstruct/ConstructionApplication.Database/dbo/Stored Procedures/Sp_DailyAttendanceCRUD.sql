CREATE PROCEDURE Sp_DailyAttendanceCRUD
    @Mode NVARCHAR(50),
    @Id INT = NULL,
    @Date DATETIME = NULL,
    @JobCategoryId INT = NULL,
    @ServiceProviderId INT = NULL,
    @TotalWorker INT = NULL,
    @AmountPerWorker DECIMAL(18,2) = NULL,
    @TotalAmount DECIMAL(18,2) = NULL,
    @DateFrom DATETIME = NULL,
    @DateTo DATETIME = NULL,
    @Notes NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Get all records (filtered by Date range)
    IF @Mode = 'GET_ALL'
    BEGIN
        SELECT 
            DailyAttendance.Id,
            DailyAttendance.Date,
            JobCategories.Name, 
            ServiceProviders.Name AS ServiceProviderName,
            DailyAttendance.TotalWorker, 
            DailyAttendance.AmountPerWorker,
            DailyAttendance.TotalAmount
        FROM DailyAttendance
        JOIN JobCategories ON DailyAttendance.JobCategoryId = JobCategories.Id
        JOIN ServiceProviders ON DailyAttendance.ServiceProviderId = ServiceProviders.Id
        WHERE 
            (@DateFrom IS NULL OR DailyAttendance.Date >= @DateFrom) 
            AND 
            (@DateTo IS NULL OR DailyAttendance.Date <= @DateTo)
        ORDER BY DailyAttendance.Date DESC;
    END

    -- Insert a new record
    IF @Mode = 'CREATE'
    BEGIN
        INSERT INTO DailyAttendance
        (Date, JobCategoryId, ServiceProviderId, TotalWorker, AmountPerWorker, TotalAmount, Notes)
        VALUES
        (@Date, @JobCategoryId, @ServiceProviderId, @TotalWorker, @AmountPerWorker, @TotalAmount, @Notes);
        
        SELECT SCOPE_IDENTITY();  -- Return the newly inserted ID
    END

    -- Delete records by ServiceProviderId
    IF @Mode = 'DELETE'
    BEGIN
        DELETE FROM DailyAttendance WHERE ServiceProviderId = @ServiceProviderId;
    END
END
