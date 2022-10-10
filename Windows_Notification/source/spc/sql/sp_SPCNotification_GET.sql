DROP PROCEDURE sp_SPCNotification_GET
GO 

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		: Pebri
-- Create date	: 2022-09-15
-- Description	: To GET NG Input & Delay Input
-- =============================================
CREATE PROCEDURE sp_SPCNotification_GET
	 @UserID		VARCHAR(50)
	
AS
BEGIN
	DECLARE @FactoryCode VARCHAR(25)
	SET @FactoryCode = (SELECT TOP 1 FactoryCode FROM MS_Line WHERE LineCode = (SELECT TOP 1 LineCode FROM spc_UserLine WHERE UserID = @UserID))

	EXECUTE sp_SPC_GetNGInput @UserID,@FactoryCode
	EXECUTE sp_SPC_GetDelayInput @UserID,@FactoryCode

END
GO
