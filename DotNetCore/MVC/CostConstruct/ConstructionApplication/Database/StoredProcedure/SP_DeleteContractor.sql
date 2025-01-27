CREATE PROCEDURE sp_DeleteContractor
    @ContractorId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Contractors 
    WHERE Id = @ContractorId;
END
