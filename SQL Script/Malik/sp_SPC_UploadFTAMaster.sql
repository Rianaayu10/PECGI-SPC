/****** Object:  StoredProcedure [dbo].[sp_SPC_UploadFTAMaster]    Script Date: 4/5/2023 11:55:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author: Malik Ilman	
-- Create date: 25-Mar-2023
-- Description:	Get List Item Type For Upload FTA 
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_UploadFTAMaster]
	@ItemTypeCode	AS Varchar(25) = '',
	@ItemCheckCode	AS Varchar(25) = '',
	@FTAID			AS Varchar(25) = '',
	@ActionID		AS Varchar(25) = '',
	@ItemTypeName	AS Varchar(25) = '',
	@Type			As Integer
As
SET NOCOUNT ON
BEGIN
		
		If @Type = 1
			BEGIN
				select * from MS_ItemType where ItemTypeCode = @ItemTypeCode
			END
		Else If @Type = 2
			BEGIN
				select * from spc_ItemCheckMaster where ItemCheckCode = @ItemCheckCode
			END
		Else If @Type = 3
			BEGIN
				select FTAID from spc_MS_FTA where FTAID  = @FTAID
			END
		Else If @Type = 4
			BEGIN
				select ActionID from spc_MS_FTAAction where FTAID = @FTAID AND ActionID = @ActionID
			END
		Else If @Type = 5
			BEGIN
				select ItemTypeCode from MS_ItemType where [Description] = @ItemTypeName
			END

END
SET NOCOUNT OFF
