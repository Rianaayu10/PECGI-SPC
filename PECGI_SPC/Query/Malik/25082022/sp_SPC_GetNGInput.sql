SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author: Malik Ilman	
-- Create date: 25-08-2022
-- Description:	Get Report NG Input Item
-- =============================================

CREATE PROCEDURE [dbo].[sp_SPC_GetNGInput]
	@FactoryCode AS Varchar(MAX)
As
SET NOCOUNT ON
BEGIN
		SELECT * FROM (
			SELECT 
				FactoryCode = CS.FactoryCode, 
				ItemTypeCode = CS.ItemTypeCode, 
				ItemTypeName = IT.Description, 
				LineCode = CS.LineCode, 
				LineName = CS.LineCode + ' - ' + MSL.LineName, 
				ItemCheckCode = CS.ItemCheckCode, 
				ItemCheck = ICT.ItemCheck, 
				Date = FORMAT(GETDATE(), 'dd MMM yy'),
				ShiftCode = RS.ShiftCode, 
				SequenceNo = RS.SequenceNo , 
				USL = CS.SpecUSL, 
				LSL = CS.SpecLSL, 
				UCL = CS.XBarUCL, 
				LCL = CS.XBarLCL,
				MAX(RD.Value) as MaxValue, MIN(RD.Value) AS MinValue, cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) AS Average,
				Operator = convert(varchar, RS.RegisterDate, 8),
				MK = convert(varchar, RS.MKVerificationDate, 8),
				QC = convert(varchar, RS.QCVerificationDate, 8),
				CASE
					WHEN MAX(RD.Value) > CS.SpecUSL OR MAX(RD.Value) > CS.XBarUCL THEN 'NG'
					WHEN MIN(RD.Value) < CS.SpecLSL OR MIN(RD.Value) < CS.XBarLCL THEN 'NG'
					WHEN 
						cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) > CS.SpecLSL 
						OR cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) > CS.XBarLCL 
					THEN 'NG'
					WHEN 
						cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) < CS.SpecLSL 
						OR cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) < CS.XBarLCL 
					THEN 'NG'
					ELSE 'OK'
				END StatusNG
			FROM spc_ChartSetup CS
				INNER JOIN
				(
					SELECT * FROM spc_Result where Format(ProdDate,'yyyy-MM-dd') = Format(GETDATE(),'yyyy-MM-dd')
				) RS ON CS.FactoryCode = RS.FactoryCode AND CS.ItemTypeCode = RS.ItemTypeCode AND CS.LineCode = RS.LineCode AND CS.ItemCheckCode = RS.ItemCheckCode
				INNER JOIN 
				(
					SELECT RD.* FROM spc_ResultDetail RD WHERE RD.SPCResultID IN 
					( SELECT SPCResultID FROM spc_Result where Format(ProdDate,'yyyy-MM-dd') = Format(GETDATE(),'yyyy-MM-dd'))
				) RD ON RS.SPCResultID = RD.SPCResultID
				INNER JOIN MS_ItemType IT ON CS.ItemTypeCode = IT.ItemTypeCode
				INNER JOIN MS_Line MSL ON CS.LineCode = MSL.LineCode
				INNER JOIN spc_ItemCheckMaster ICT ON CS.ItemCheckCode = ICT.ItemCheckCode
			WHERE Format(GETDATE(),'yyyy-MM-dd') Between Format(CS.StartDate,'yyyy-MM-dd') and Format(CS.EndDate,'yyyy-MM-dd')
			GROUP BY CS.FactoryCode, CS.ItemTypeCode, IT.Description, CS.LineCode, LineName, CS.ItemCheckCode, ItemCheck, ShiftCode,
			RS.SequenceNo, CS.SpecUSL, CS.SpecLSL, CS.XBarUCL, CS.XBarLCL, RS.RegisterDate, RS.MKVerificationDate, RS.QCVerificationDate
		) Query
		WHERE StatusNG = 'NG'
		AND FactoryCode = @FactoryCode

END
SET NOCOUNT OFF
GO


