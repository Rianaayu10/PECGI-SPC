SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author: Malik Ilman	
-- Create date: 24-08-2022
-- Description:	Get Report Delay Input Item
-- =============================================

CREATE PROCEDURE [dbo].[sp_SPC_GetDelayInput]
	@FactoryCode AS Varchar(MAX)
As
SET NOCOUNT ON
BEGIN
		SELECT * FROM
			(
				select ICT.FactoryCode, ItemTypeName = MSI.Description, LineName = MSL.LineCode + ' - ' + MSL.LineName, ICM.ItemCheck, Date = FORMAT(GETDATE(), 'dd MMM yy'), 
				MSF.ShiftCode, MSF.SequenceNo, convert(varchar, MSF.StartTime, 8) as StartTime, convert(varchar, MSF.EndTime, 8) as EndTime, 
				Delay = DATEDIFF(MINUTE, convert(varchar, MSF.EndTime, 8), convert(varchar, getdate(), 8))
				, ICT.ItemTypeCode, ICT.LineCode, ICT.ItemCheckCode, ICT.FrequencyCode
				from spc_ItemCheckByType ICT
				INNER JOIN spc_MS_Frequency MSF ON ICT.FrequencyCode = MSF.FrequencyCode	
				INNER JOIN MS_ItemType MSI ON ICT.ItemTypeCode = MSI.ItemTypeCode
				INNER JOIN MS_Line MSL ON ICT.LineCode = MSL.LineCode
				INNER JOIN spc_ItemCheckMaster ICM ON ICT.ItemCheckCode = ICM.ItemCheckCode
			) TBL
		WHERE NOT EXISTS (
				SELECT * FROM (
				select RS.ShiftCode, RS.ItemTypeCode, RS.LineCode, RS.ItemCheckCode, RD.* from spc_ResultDetail RD
				INNER JOIN spc_Result RS ON RD.SPCResultID = RS.SPCResultID WHERE FORMAT(RD.RegisterDate, 'dd MMM yy') = FORMAT(GETDATE(), 'dd MMM yy')) TBL2
				WHERE TBL.ShiftCode = TBL2.ShiftCode AND TBL.ItemTypeCode = TBL2.ItemTypeCode AND TBL.LineCode = TBL2.LineCode AND TBL.ItemCheckCode = TBL2.ItemCheckCode
				AND TBL.SequenceNo = TBL2.SequenceNo
			) 
		AND EndTime < convert(varchar, getdate(), 8)
		AND FactoryCode = @FactoryCode

END
SET NOCOUNT OFF
GO


