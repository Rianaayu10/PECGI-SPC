
/****** Object:  StoredProcedure [dbo].[sp_spc_FTA_ReportYearlyByType_LoadDetail]    Script Date: 4/4/2023 3:03:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Riana>
-- Create date: <2023-03-09>
-- Description:	<>
-- =============================================
ALTER PROCEDURE [dbo].[sp_spc_FTA_ReportYearlyByType_LoadDetail]
	@User			AS VARCHAR(50),
	@LineGroup		AS VARCHAR(25),
	@ProcessCode	AS VARCHAR(25),
	@LineCode		AS VARCHAR(25),
	@QtyFTA			AS VARCHAR(25),
	@Periode		AS VARCHAR(15)
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ROW_NUMBER() OVER (Order by QtyFTA_Detail Desc) AS Number, * FROM (
			SELECT ItemCheckName, QtyFTA_Detail = COUNT(Periode), QtyFTA_DetailPercentage = CAST((COUNT(Periode) * 100) / CAST(@QtyFTA AS DECIMAL(18,2)) AS DECIMAL(18,2))
			FROM (		
					SELECT  FactoryCode		= SR.FactoryCode,
							LineGroup		= MLG.LineGroup , 
							LineGroupName	= MLG.LineGroupName ,
							LineCode		= SR.LineCode,
							ItemType		= SR.ItemTypeCode,
							ItemCheckCode	= SR.ItemCheckCode,
							ItemCheckName	= ICM.ItemCheck,
							Periode			= FORMAT(SR.ProdDate,'MMM-yy')
					FROM spc_Result SR
					INNER JOIN spc_UserLine UL ON SR.LineCode = UL.LineCode AND UL.UserID = 'AdminTos'
					INNER JOIN MS_Line ML ON UL.LineCode = ML.LineCode 
					INNER JOIN MS_Process MP ON ML.ProcessCode = MP.ProcessCode
					INNER JOIN MS_LineGroup MLG ON MLG.LineGroup = MP.LineGroup AND MLG.ProcessGroup = MP.ProcessGroup
					INNER JOIN spc_ItemCheckMaster ICM ON SR.ItemCheckCode = ICM.ItemCheckCode
					WHERE MLG.LineGroup = @LineGroup 
					 AND FORMAT(SR.ProdDate, 'MMM-yy') = @Periode AND SR.FTAStatus = '1'
					 AND 1 = CASE WHEN @ProcessCode = 'ALL' THEN 1 WHEN  @ProcessCode  <> '' AND MP.ProcessCode  = @ProcessCode   THEN 1 ELSE 0 END
					 AND 1 = CASE WHEN @LineCode = 'ALL' THEN 1 WHEN  @LineCode <> '' AND SR.LineCode = @LineCode  THEN 1 ELSE 0 END
				) A GROUP BY A.FactoryCode, A.LineGroup, A.LineGroupName, A.LineCode, A.ItemType, A.ItemCheckCode, A.ItemCheckName, A.Periode
			) A
		
	
END
GO


