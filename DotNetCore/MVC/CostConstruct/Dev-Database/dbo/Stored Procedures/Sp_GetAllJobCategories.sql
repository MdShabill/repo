﻿CREATE PROCEDURE Sp_GetAllJobCategories
AS
BEGIN
    SET NOCOUNT ON
    SELECT Id, Name FROM JobCategories
END
