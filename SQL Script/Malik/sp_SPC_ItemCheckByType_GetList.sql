/****** Object:  StoredProcedure [dbo].[sp_SPC_ItemCheckByType_GetList]    Script Date: 5/9/2023 1:37:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author: Malik Ilman	
-- Create date: 12-08-2022
-- Description:	Get List Item Check By Battery Type
-- =============================================

ALTER   PROCEDURE [dbo].[sp_SPC_ItemCheckByType_GetList]
	@User					As Varchar(25),
	@FactoryCode			AS Varchar(25),
	@ItemTypeDescription	AS Varchar(200),
	@MachineProccess		AS Varchar(MAX),
	@ItemTypeCode			AS Varchar(50) = NULL
As
SET NOCOUNT ON
BEGIN
		--DECLARE @NewQuery  VARCHAR(MAX)   
		
		IF @MachineProccess <> 'ALL'
			BEGIN
				SET @MachineProccess = ' LineCode = ''' + @MachineProccess+ ''' '
			END
		ELSE
			BEGIN
				--SET @MachineProccess = ' 
				--LineCode IN (
				--	select distinct L.LineCode
				--	from MS_Line L inner join spc_ItemCheckByType I 
				--	on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode 
				--	inner join spc_UserLine P on L.LineCode = P.LineCode 
				--	where P.UserID = ''' + @User + ''' and P.AllowShow = 1 and L.FactoryCode = ''' + @FactoryCode + ''' and I.ItemTypeCode = ''' + @ItemTypeCode + ''' 
				--)'
				SET @MachineProccess = '1=1'
			END
		--SET @NewQuery = @FactoryCode + @ItemTypeDescription + @MachineProccess

		BEGIN
			EXEC ('
					SELECT * FROM (
						SELECT MSF.FactoryCode, MSF.FactoryName, ItemTypeCode = MSI.ItemTypeCode , ItemTypeName = MSI.Description, ItemTypeNameGrid = MSI.ItemTypeCode + '' - '' + MSI.Description, MSL.LineCode, LineName = MSL.LineCode + '' - '' + MSL.LineName, 
						ItemCheck = ICM.ItemCheckCode + '' - '' + ICM.ItemCheck, ICT.FrequencyCode, FS.FrequencyName, ICT.RegistrationNo, ICT.SampleSize, ICT.Remark, ICT.Evaluation, 
						CASE
							WHEN ICT.CharacteristicStatus = 0 THEN ''0 - Non-Special''
							WHEN ICT.CharacteristicStatus = 1 THEN ''1 - Special Characteristic 1''
							WHEN ICT.CharacteristicStatus = 2 THEN ''2 - Special Characteristic 2''
							ELSE ICT.CharacteristicStatus
							END CharacteristicStatus
						, ProcessTableLineCode = ICT.ProcessTableLineCode + '' - '' + MSL2.LineName, FTARatio = CONVERT(varchar(5), ICT.FTARatio), ICT.StationID, ICT.ActiveStatus,
						ISNULL(TRIM(ICT.PrevItemCheck) + '' - '' + prevICM.ItemCheck, ''N/A'') PrevItemCheck, ICT.PrevValue, 
						ISNULL(SU.FullName, ICT.UpdateUser) UpdateUser, FORMAT(ICT.UpdateDate, ''dd MMM yy hh:mm:ss'') UpdateDate
						FROM spc_ItemCheckByType ICT 
						JOIN MS_Line MSL ON ICT.FactoryCode = MSL.FactoryCode AND ICT.LineCode = MSL.LineCode 
						JOIN spc_ItemCheckMaster ICM ON ICT.ItemCheckCode = ICM.ItemCheckCode 
						JOIN spc_MS_FrequencySetting FS ON ICT.FrequencyCode = FS.FrequencyCode
						JOIN MS_ItemType MSI ON ICT.ItemTypeCode = MSI.ItemTypeCode 
						JOIN MS_Factory MSF ON ICT.FactoryCode = MSF.FactoryCode
						LEFT JOIN MS_LINE MSL2 ON ICT.ProcessTableLineCode = MSL2.LineCode 
						LEFT JOIN spc_UserSetup SU ON ICT.UpdateUser = SU.UserID
						LEFT JOIN spc_ItemCheckMaster prevICM ON ICT.PrevItemCheck = prevICM.ItemCheckCode 
					) tbl
					WHERE  FactoryCode = '''+@FactoryCode+''' AND ItemTypeName = '''+ @ItemTypeDescription+''' AND  '+ @MachineProccess+'
				')
		END

END
SET NOCOUNT OFF
