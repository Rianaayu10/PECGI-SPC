/****** Object:  StoredProcedure [dbo].[sp_SPC_ItemCheckByBattery_FillCombo]    Script Date: 5/5/2023 8:20:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Malik
-- Create date: 15-08-2022
-- Description:	Fill ComboBox Item Check By Battery
-- =============================================

ALTER   PROCEDURE [dbo].[sp_SPC_ItemCheckByBattery_FillCombo]
	@Type			As char(1),
	@FactoryCode	As Varchar(50) = NULL,
	@User			As Varchar(10) = NULL
As
BEGIN
	
	IF @Type = '1'
	BEGIN
		--SELECT FactoryCode, FactoryName from MS_Factory
		SELECT	MSF.FactoryCode, MSF.FactoryName FROM MS_Factory MSF 
		INNER JOIN spc_UserSetup US on MSF.FactoryCode = US.FactoryCode and US.UserID = @User
		Order By Description
	END
	ELSE IF @Type = '2'
	BEGIN
		SELECT ItemTypeCode, ItemTypeName = Description, ItemTypeNameGrid = ItemTypeCode + ' - ' + Description from MS_ItemType
	END
	ELSE IF @Type = '3'
	BEGIN
		SELECT LineCode, LineName = LineCode + ' - ' + LineName FROM MS_Line
	END
	ELSE IF @Type = '4'
	BEGIN
		SELECT FrequencyCode, FrequencyName from spc_MS_FrequencySetting
	END
	ELSE IF @Type = '5'
	BEGIN
		SELECT  ItemCheckCode, ItemCheck = ItemCheckCode + ' - ' + ItemCheck from spc_ItemCheckMaster
	END
	ELSE IF @Type = '6'
	BEGIN
		SELECT distinct RegistrationNo, RegistrationName = RegistrationNo + ' - ' + Description FROM spc_MS_Device --WHERE FactoryCode = @FactoryCode
	END
	ELSE IF @Type = '7'
	BEGIN
		SELECT DISTINCT 
			CASE
				WHEN CharacteristicStatus = 0 THEN '0 - NonSpecial'
				WHEN CharacteristicStatus = 1 THEN '1 - Special Characteristics'
				--WHEN CharacteristicStatus = 2 THEN '2 - Special Characteristic 2'
				ELSE CharacteristicStatus
			END CharacteristicStatus
		FROM spc_ItemCheckByType
	END
	ELSE IF @Type = '8'
	BEGIN
		SELECT DISTINCT Evaluation FROM spc_ItemCheckByType WHERE Evaluation <> ''
	END
	ELSE IF @Type = '9'
	BEGIN
		SELECT '1' PrevValue
		UNION
		SELECT '2' PrevValue
	END
END
