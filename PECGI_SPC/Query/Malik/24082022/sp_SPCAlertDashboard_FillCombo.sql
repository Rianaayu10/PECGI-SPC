SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Malik
-- Create date: 24-08-2022
-- Description:	Fill ComboBox Alert Dashboard
-- =============================================

CREATE PROCEDURE [dbo].[sp_SPCAlertDashboard_FillCombo]
	@Type	As char(1),
	@Param	As varchar(100) = NULL
As
BEGIN
	
	IF @Type = '1'
	BEGIN
		SELECT FactoryCode, FactoryName = FactoryCode + ' - ' + FactoryName from MS_Factory
	END
	ELSE IF @Type = '2'
	BEGIN
		SELECT ItemTypeCode, ItemTypeName = Description from MS_ItemType
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
		SELECT  ItemCheckCode, ItemCheck from spc_ItemCheckMaster --= ItemCheckCode + ' - ' + ItemCheck from spc_ItemCheckMaster
	END
	ELSE IF @Type = '6'
	BEGIN
		SELECT DISTINCT ShiftCode FROM MS_Shift
	END
	ELSE IF @Type = '7'
	BEGIN
		SELECT DISTINCT SequenceNo FROM spc_MS_Frequency
	END
END
GO


