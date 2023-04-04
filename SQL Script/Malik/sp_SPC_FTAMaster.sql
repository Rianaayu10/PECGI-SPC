/****** Object:  StoredProcedure [dbo].[sp_SPC_FTAMaster]    Script Date: 4/4/2023 10:23:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================================================
-- Author:		Malik Ilman Nafia
-- Create date: 11-Mar-2023
-- Description:	SP All Function In FTA Master
-- ==================================================

ALTER PROCEDURE [dbo].[sp_SPC_FTAMaster]
	@TypeProcess integer = 0,
	@FactoryCode varchar(50) = '',
	@ItemTypeCode varchar(50) = '',
	@ItemCheckCode varchar(50) = '',
	@FTAID varchar(50) = '',
	@Factor1 varchar(50) = '',
	@Factor2 varchar(50) = '',
	@Factor3 varchar(50) = '',
	@Factor4 varchar(50) = '',
	@CounterMeasure varchar(50) = '',
	@CheckItem varchar(50) = '',
	@CheckOrder int = 0,
	@IK image = null,
	@Remark varchar(250) = '',
	@ActiveStatus varchar(50) = '',
	@RegisterUser varchar(50) = '',
	@RegisterDate datetime = null,
	@UpdateUser varchar(50) = '' ,
	@UpdateDate datetime = null,
	@ActionID varchar(50) = '',
	@ActionName varchar(50) = ''
As
BEGIN
	
	If @TypeProcess = 1
		BEGIN
			SELECT * FROM spc_MS_FTA WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode 
			AND  ItemCheckCode = @ItemCheckCode AND FTAID = @FTAID
		END
	Else If @TypeProcess = 2 
		BEGIN
			INSERT INTO [dbo].[spc_MS_FTA] ([FactoryCode],[ItemTypeCode],[ItemCheckCode],[FTAID],[Factor1],[Factor2],[Factor3],
			[Factor4],[CounterMeasure],[CheckItem],[CheckOrder],[IK],[Remark],[ActiveStatus],[RegisterUser],[RegisterDate],[UpdateUser],[UpdateDate])

			VALUES
           (@FactoryCode, @ItemTypeCode, @ItemCheckCode, @FTAID, @Factor1, @Factor2, @Factor3, @Factor4, @CounterMeasure, @CheckItem, 
		   @CheckOrder, @IK, @Remark, @ActiveStatus, @RegisterUser, GETDATE(), @UpdateUser, @UpdateDate)

		END
	Else If @TypeProcess = 3 -- Upload IK
		BEGIN
			
			UPDATE spc_MS_FTA SET IK = @IK, UpdateDate = GETDATE(), UpdateUser = @UpdateUser WHERE  FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode 
			AND ItemCheckCode = @ItemCheckCode AND FTAID = @FTAID
		END
	Else If @TypeProcess = 4
		BEGIN
			SELECT FTAIDAction = FTAID, ActionID, ActionName, RemarkAction = Remark FROM spc_MS_FTAAction WHERE FTAID = @FTAID ORDER BY ActionID ASC
		END
	Else If @TypeProcess = 5 
		BEGIN
			UPDATE spc_MS_FTAAction SET ActionName = @ActionName, Remark = @Remark WHERE FTAID = @FTAID and ActionID = @ActionID
		END
	Else If @TypeProcess = 6
		BEGIN
			DELETE spc_MS_FTAAction WHERE FTAID = @FTAID AND ActionID = @ActionID
		END
	Else If @TypeProcess = 7
		BEGIN
			SET @ActionID = (select top 1 ActionID = ActionID + 1 from spc_MS_FTAAction where FTAID = @FTAID order by ActionID desc)

			INSERT INTO spc_MS_FTAAction VALUES (@FTAID, @ActionID, @ActionName, @Remark)
		END
	Else If @TypeProcess = 8 --Delete FTA Master ITem
		BEGIN

			DELETE spc_MS_FTA where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode AND ItemCheckCode = @ItemCheckCode AND FTAID = @FTAID
		
		END
	Else If @TypeProcess = 9 --For Update FTA Master
		BEGIN
			UPDATE [dbo].[spc_MS_FTA]
		   SET 
			   [Factor1] = @Factor1
			  ,[Factor2] = @Factor2
			  ,[Factor3] = @Factor3
			  ,[Factor4] = @Factor4
			  ,[CounterMeasure] = @CounterMeasure
			  ,[CheckItem] = @CheckItem
			  ,[CheckOrder] = @CheckOrder
			  ,[Remark] = @Remark
			  ,[ActiveStatus] = @ActiveStatus
			  ,[UpdateUser] = @UpdateUser
			  ,[UpdateDate] = GETDATE()
		 WHERE 
			   FactoryCode = @FactoryCode
			  AND ItemTypeCode = @ItemTypeCode
			  AND ItemCheckCode = @ItemCheckCode
			  AND FTAID = @FTAID
		END
	Else If @TypeProcess = 10 --Check FTA Id if has been using in C010
		BEGIN
			SELECT * FROM spc_FTAResultDetail WHERE FTAID = @FTAID
		END
	Else If @TypeProcess = 11 --GET ACTION ID FOR FTA MASTER
		BEGIN
			SELECT ActionID FROM spc_MS_FTAAction WHERE FTAID = @FTAID and ActionName = @ActionName
		END
	
END