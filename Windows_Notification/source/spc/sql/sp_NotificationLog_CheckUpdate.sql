DROP PROCEDURE sp_NotificationLog_CheckUpdate
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		: Pebri
-- Create date	: 19-10-2020
-- Description	: 
-- =============================================
CREATE PROCEDURE sp_NotificationLog_CheckUpdate 
	 @NotifType	VARCHAR(5)
	,@UserID	VARCHAR(50) = 'Admin'
AS
BEGIN
	DECLARE @FactoryCode VARCHAR(25)
	SET @FactoryCode = (SELECT TOP 1 FactoryCode FROM MS_Line WHERE LineCode = (SELECT TOP 1 LineCode FROM spc_UserLine WHERE UserID = @UserID))


	SELECT * FROM spc_NotificationLog
	WHERE 
		NotificationCategory = @NotifType
	AND FactoryCode = @FactoryCode
	Order by LastUpdate ASC
END
GO
