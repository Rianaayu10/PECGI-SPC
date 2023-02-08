/****** Object:  StoredProcedure [dbo].[sp_SPC_GetNGInput]    Script Date: 2/8/2023 4:41:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author: Malik Ilman	
-- Create date: 25-08-2022
-- Description:	Get Report NG Input Item
-- =============================================
-- =============================================
-- Editor: Malik Ilman	
-- Edit date: 16-09-2022
-- Description:	Get Report NG Input Item For Last 2 Day
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_GetNGInput]
	@User AS Varchar(10) = 'zqc',
	@FactoryCode AS Varchar(8) = 'F001',
	@TypeReport AS int = 1, -- 1 For Web, 2 For Windows Form
	@ProdDateType AS int = 1,
	@ProdDate AS DATETIME = null
As
SET NOCOUNT ON
BEGIN

	DECLARE @ProdDateYesterday AS DATETIME

	IF @ProdDateType = 1
	BEGIN
		SET @ProdDate = GETDATE()
		SET @ProdDateYesterday = (select top 1 Schedule_Date from Daily_Production where FORMAT(Schedule_Date,'yyyy-MM-dd') < FORMAT(GETDATE(),'yyyy-MM-dd') order by Schedule_Date desc)
	END
	ELSE IF @ProdDateType = 2
	BEGIN
		SET @ProdDate = @ProdDate
		SET @ProdDateYesterday = (select top 1 Schedule_Date from Daily_Production where FORMAT(Schedule_Date,'yyyy-MM-dd') < FORMAT(@ProdDate,'yyyy-MM-dd') order by Schedule_Date desc)
	END
	
	IF @TypeReport = 1
	BEGIN
		SELECT 
		('Edit||ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ Date +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1)) ) As Edit,
		CONCAT ( [Date], '|' , ItemTypeNameLink , '|' , LineName , '|' , ItemCheck, '|' , ShiftCode, '|', SequenceNo) AS Label,
		('ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ LinkDate +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1))) As Link,
			RValueSPCDashboard = 
				CASE
					WHEN CharacteristicStatus = 0 THEN ''
					WHEN CharacteristicStatus = 1 THEN Convert(varchar(10), RValue) + '||' + Convert(varchar(10), RColor) END, 
		* FROM (
				select 
					FactoryCode = CS.FactoryCode, 
					MSF.FactoryName, 
					CS.ItemTypeCode, 
					ItemTypeNameLink = IT.Description , 
					ItemTypeName = IT.Description + '||' + ICT.CharacteristicStatus + '||ProdSampleInput.aspx?FactoryCode='+ CS.FactoryCode +'&ItemTypeCode='+ CS.ItemTypeCode +'&Line='+ CS.LineCode +'&ItemCheckCode='+ 
					CS.ItemCheckCode +'&ProdDate='+ FORMAT(RS.ProdDate, 'dd MMM yy') +'&Shift='+ RS.ShiftCode  +'&Sequence='+ CAST(RS.SequenceNo AS nvarchar(1))  , 
					CS.LineCode, 
					LineName = CS.LineCode + ' - ' + MSL.LineName, 
					LineDesc = MSL.LineName,
					CS.ItemCheckCode , 
					ItemCheck = CS.ItemCheckCode + ' - ' + ICM.ItemCheck, 
					ItemCheckDesc = ICM.ItemCheck,
					Date = FORMAT(RS.ProdDate, 'dd MMM yy'), 
					LinkDate = FORMAT(RS.ProdDate, 'yyyy-MM-dd'),
					--CASE 
					--	WHEN RS.ShiftCode = 'SH001' THEN '1'
					--	WHEN RS.ShiftCode = 'SH002' THEN '2'
					--	ELSE RS.ShiftCode
					--	END ShiftCode,
					RS.ShiftCode AS ShiftCode, 
					RS.ShiftCode AS ShiftCodeHeader, 
					RS.SequenceNo, 
					USL = CS.SpecUSL, 
					LSL = CS.SpecLSL, 
					UCL = CS.CPUCL, 
					LCL = CS.CPLCL,
					MAX(RD.Value) as MaxValue, MIN(RD.Value) AS MinValue, cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) AS Average,
					RValue = MAX(RD.Value) - MIN(RD.Value),
					CASE
						WHEN (MAX(RD.Value) - MIN(RD.Value)) > CS.RUCL THEN 1
						Else 0
					END RColor,
					CASE
						WHEN 
						(
							SELECT SUM(SubCountDataOK.ValueSubRD) from (
							select 
							CASE
							WHEN subRD.VALUE < CS.SpecUSL AND subRD.VALUE > CS.SpecLSL THEN 1
							ELSE 0
							END ValueSubRD
							from spc_ResultDetail subRD where subRD.SPCResultID = RD.SPCResultID and subRD.DeleteStatus <> 1 ) SubCountDataOK
						) >= ICT.SampleSize  THEN 'OK'
						WHEN MAX(RD.Value) > CS.SpecUSL THEN 'NG'
						WHEN MIN(RD.Value) < CS.SpecLSL THEN 'NG'
						WHEN 
							cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) > CS.SpecUSL 
						THEN 'NG'
						WHEN 
							cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) < CS.SpecLSL 
						THEN 'NG'
						ELSE 'OK'
					END StatusNG,
					Operator = RS.RegisterUser, 
					MK = RS.MKVerificationUser, 
					QC = RS.QCVerificationUser, 
					UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate),
					CharacteristicStatus = ICT.CharacteristicStatus
				from spc_ChartSetup CS
				INNER JOIN spc_Result RS ON CS.FactoryCode = RS.FactoryCode AND CS.ItemTypeCode = RS.ItemTypeCode AND CS.LineCode = RS.LineCode 
				INNER JOIN spc_ResultDetail RD ON RS.SPCResultID = RD.SPCResultID
				AND CS.ItemCheckCode = RS.ItemCheckCode
				INNER JOIN MS_Factory MSF ON CS.FactoryCode = MSF.FactoryCode
				INNER JOIN MS_ItemType IT ON CS.ItemTypeCode = IT.ItemTypeCode
				INNER JOIN MS_Line MSL ON CS.LineCode = MSL.LineCode
				INNER JOIN spc_ItemCheckMaster ICM ON CS.ItemCheckCode = ICM.ItemCheckCode
				INNER JOIN spc_ItemCheckByType ICT ON CS.FactoryCode = ICT.FactoryCode AND CS.LineCode = ICT.LineCode AND CS.ItemTypeCode = ICT.ItemTypeCode AND CS.ItemCheckCode = ICT.ItemCheckCode
				WHERE 
				--RS.ProdDate IN (convert(varchar, @ProdDateYesterday, 23), convert(varchar, @ProdDate, 23)) AND 
				RS.ProdDate BETWEEN CS.StartDate and CS.EndDate
				AND 
				1 = 
				CASE
					WHEN RS.QCVerificationUser IS NULL THEN 1
					WHEN RS.QCVerificationUser = '' THEN 1
					ELSE 0
				END
				AND RD.DeleteStatus <> 1
				--convert(varchar, RS.ProdDate, 23) IN (convert(varchar, CS.StartDate, 23), convert(varchar, CS.EndDate, 23))
				GROUP BY CS.FactoryCode, MSF.FactoryName, CS.ItemTypeCode, IT.Description, CS.LineCode, MSL.LineName, CS.ItemCheckCode , ICM.ItemCheck, RS.ProdDate, RS.ShiftCode, RS.SequenceNo
				, CS.SpecUSL, CS.SpecLSL, CS.CPUCL, CS.CPLCL
				,RS.RegisterUser, RS.MKVerificationUser,RS.QCVerificationUser, RS.UpdateDate,RS.RegisterDate, RD.SPCResultID,ICT.SampleSize,ICT.CharacteristicStatus, CS.RUCL
			) tbl
			where StatusNG = 'NG'
			AND 1 = CASE 
					WHEN @FactoryCode = 'ALL' THEN 1
					WHEN @FactoryCode <> 'ALL' AND FactoryCode = @FactoryCode THEN 1
					ELSE 0 END
			AND LineCode IN (
					select distinct L.LineCode
					from MS_Line L inner join spc_ItemCheckByType I 
					on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode 
					inner join spc_UserLine P on L.LineCode = P.LineCode 
					where P.UserID = @User and P.AllowShow = 1 and 
					1 = CASE 
					WHEN @FactoryCode = 'ALL' THEN 1
					WHEN @FactoryCode <> 'ALL' AND L.FactoryCode = @FactoryCode THEN 1
					ELSE 0 END
					)
			ORDER BY Date, UpdateDate, FactoryCode, ItemTypeName, LineName, ItemCheck, ShiftCode, SequenceNo
	END
	ELSE IF @TypeReport = 2
	BEGIN
		SELECT 
		('Edit||ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ Date +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1)) ) As Edit,
		CONCAT ( [Date], '|' , ItemTypeName , '|' , LineName , '|' , ItemCheck, '|' , ShiftCode, '|', SequenceNo) AS Label,
		('ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ LinkDate +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1))) As Link,
		* FROM (
				select 
					FactoryCode = CS.FactoryCode, 
					MSF.FactoryName, 
					CS.ItemTypeCode, 
					ItemTypeName = IT.Description, 
					CS.LineCode, 
					LineName = CS.LineCode + ' - ' + MSL.LineName, 
					LineDesc = MSL.LineName,
					CS.ItemCheckCode , 
					ItemCheck = CS.ItemCheckCode + ' - ' + ICM.ItemCheck, 
					ItemCheckDesc = ICM.ItemCheck,
					Date = FORMAT(RS.ProdDate, 'dd MMM yy'), 
					LinkDate = FORMAT(RS.ProdDate, 'yyyy-MM-dd'),
					CASE 
						WHEN RS.ShiftCode = 'SH001' THEN '1'
						WHEN RS.ShiftCode = 'SH002' THEN '2'
						ELSE RS.ShiftCode
						END ShiftCode,
					RS.ShiftCode AS ShiftCodeHeader, 
					RS.SequenceNo, 
					USL = CS.SpecUSL, 
					LSL = CS.SpecLSL, 
					UCL = CS.CPUCL, 
					LCL = CS.CPLCL,
					MAX(RD.Value) as MaxValue, MIN(RD.Value) AS MinValue, cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) AS Average,
					CASE
						WHEN MAX(RD.Value) > CS.SpecUSL THEN 'NG'
						WHEN MIN(RD.Value) < CS.SpecLSL THEN 'NG'
						WHEN 
							cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) > CS.SpecUSL 
						THEN 'NG'
						WHEN 
							cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,3)) < CS.SpecLSL 
						THEN 'NG'
						ELSE 'OK'
					END StatusNG,
					Operator = RS.RegisterUser, 
					MK = RS.MKVerificationUser, 
					QC = RS.QCVerificationUser, 
					UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate)
				from spc_ChartSetup CS
				INNER JOIN spc_Result RS ON CS.FactoryCode = RS.FactoryCode AND CS.ItemTypeCode = RS.ItemTypeCode AND CS.LineCode = RS.LineCode 
				INNER JOIN spc_ResultDetail RD ON RS.SPCResultID = RD.SPCResultID
				AND CS.ItemCheckCode = RS.ItemCheckCode
				INNER JOIN MS_Factory MSF ON CS.FactoryCode = MSF.FactoryCode
				INNER JOIN MS_ItemType IT ON CS.ItemTypeCode = IT.ItemTypeCode
				INNER JOIN MS_Line MSL ON CS.LineCode = MSL.LineCode
				INNER JOIN spc_ItemCheckMaster ICM ON CS.ItemCheckCode = ICM.ItemCheckCode
				WHERE 
				--RS.ProdDate IN (convert(varchar, @ProdDateYesterday, 23), convert(varchar, @ProdDate, 23)) AND 
				RS.ProdDate BETWEEN CS.StartDate and CS.EndDate
				AND 
				1 = 
				CASE
					WHEN RS.QCVerificationUser IS NULL THEN 1
					WHEN RS.QCVerificationUser = '' THEN 1
					ELSE 0
				END
				AND RD.DeleteStatus <> 1
				--convert(varchar, RS.ProdDate, 23) IN (convert(varchar, CS.StartDate, 23), convert(varchar, CS.EndDate, 23))
				GROUP BY CS.FactoryCode, MSF.FactoryName, CS.ItemTypeCode, IT.Description, CS.LineCode, MSL.LineName, CS.ItemCheckCode , ICM.ItemCheck, RS.ProdDate, RS.ShiftCode, RS.SequenceNo
				, CS.SpecUSL, CS.SpecLSL, CS.CPUCL, CS.CPLCL
				,RS.RegisterUser, RS.MKVerificationUser,RS.QCVerificationUser, RS.UpdateDate,RS.RegisterDate
			) tbl
			where StatusNG = 'NG' AND FactoryCode = @FactoryCode
			ORDER BY Date, UpdateDate, FactoryCode, ItemTypeName, LineName, ItemCheck, ShiftCode, SequenceNo
	END

END
SET NOCOUNT OFF
