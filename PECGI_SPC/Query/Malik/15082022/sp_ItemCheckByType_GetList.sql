SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Malik Ilman	
-- Create date: 12-08-2022
-- Description:	Get List Item Check By Battery Type
-- =============================================

ALTER PROCEDURE [dbo].[sp_ItemCheckByType_GetList]
	@FactoryName AS Varchar(MAX),
	@ItemTypeDescription AS Varchar(MAX),
	@MachineProccess AS Varchar(MAX)
As
SET NOCOUNT ON
BEGIN
		DECLARE @NewQuery  VARCHAR(MAX)   

		IF @FactoryName <> 'ALL'
			BEGIN
				SET @FactoryName = ' MSF.FactoryCode = ''' + @FactoryName +''''
			END
		ELSE
			BEGIN
				SET @FactoryName = ' 1=1'
			END 

		IF @ItemTypeDescription <> 'ALL'
			BEGIN
				SET @ItemTypeDescription = ' AND MSI.Description = ''' + @ItemTypeDescription+''''
			END
		ELSE
			BEGIN
				SET @ItemTypeDescription = ' AND 1=1'
			END 

		IF @MachineProccess <> 'ALL'
			BEGIN
				SET @MachineProccess = ' AND MSL.LineCode = ''' + @MachineProccess+ ''''
			END
		ELSE
			BEGIN
				SET @MachineProccess = ' AND 1=1'
			END 
		SET @NewQuery = @FactoryName + @ItemTypeDescription + @MachineProccess

	If @NewQuery <> ''
		BEGIN
			EXEC ('
					SELECT MSF.FactoryCode, MSF.FactoryName, ItemTypeCode = MSI.ItemTypeCode , ItemTypeName = MSI.Description, ItemTypeNameGrid = MSI.ItemTypeCode + '' - '' + MSI.Description, LineCode = MSL.LineCode + '' - '' + MSL.LineName, 
					ItemCheck = ICM.ItemCheckCode + '' - '' + ICM.ItemCheck, ICT.FrequencyCode, FS.FrequencyName, ICT.RegistrationNo, ICT.SampleSize, ICT.Remark, ICT.Evaluation, 
					ICT.CharacteristicStatus, ICT.ActiveStatus,ICT.UpdateUser, FORMAT(ICT.UpdateDate, ''dd MMM yy hh:mm:ss'') UpdateDate
					FROM spc_ItemCheckByType ICT 
					JOIN MS_Line MSL ON ICT.FactoryCode = MSL.FactoryCode AND ICT.LineCode = MSL.LineCode 
					JOIN spc_ItemCheckMaster ICM ON ICT.ItemCheckCode = ICM.ItemCheckCode 
					JOIN spc_MS_FrequencySetting FS ON ICT.FrequencyCode = FS.FrequencyCode
					JOIN MS_ItemType MSI ON ICT.ItemTypeCode = MSI.ItemTypeCode 
					JOIN MS_Factory MSF ON ICT.FactoryCode = MSF.FactoryCode
					WHERE '+@FactoryName+' '+ @ItemTypeDescription+' '+ @MachineProccess+'
				')
		END

END
SET NOCOUNT OFF
