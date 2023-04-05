/****** Object:  StoredProcedure [dbo].[sp_SPC_GetNGInput]    Script Date: 4/5/2023 10:10:22 AM ******/
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
		--SELECT 
		--('Edit||ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ Date +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1)) ) As Edit,
		--CONCAT ( [Date], '|' , ItemTypeNameLink , '|' , LineName , '|' , ItemCheck, '|' , ShiftCode, '|', SequenceNo) AS Label,
		--('ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ LinkDate +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1))) As Link,
		--	RValueSPCDashboard = 
		--		CASE
		--			WHEN CharacteristicStatus = 0 THEN '||0'
		--			WHEN CharacteristicStatus = 1 THEN Convert(varchar(10), RValue) + '||' + Convert(varchar(10), RColor) END, 
		--ActionFTA = (select top 1 ftd.[Action] from spc_FTAResultDetail ftd where ftd.FTAResultID = tbl.FTAResultID and ftd.FTAResult = '2'),
		--* FROM (
		--		select 
		--			FTAResultID = FTA.FTAResultID,
		--			FactoryCode = CS.FactoryCode, 
		--			MSF.FactoryName, 
		--			CS.ItemTypeCode, 
		--			ItemTypeNameLink = IT.Description , 
		--			ItemTypeName = IT.Description + '||' + ICT.CharacteristicStatus + '||ProdSampleInput.aspx?FactoryCode='+ CS.FactoryCode +'&ItemTypeCode='+ CS.ItemTypeCode +'&Line='+ CS.LineCode +'&ItemCheckCode='+ 
		--			CS.ItemCheckCode +'&ProdDate='+ FORMAT(RS.ProdDate, 'dd MMM yy') +'&Shift='+ RS.ShiftCode  +'&Sequence='+ CAST(RS.SequenceNo AS nvarchar(1)) + '||FTACorrectiveAction.aspx?FactoryCode='+ CS.FactoryCode +'&ItemTypeCode='+ CS.ItemTypeCode +'&Line='+ CS.LineCode +'&ItemCheckCode='+ 
		--			CS.ItemCheckCode +'&ProdDate='+ FORMAT(RS.ProdDate, 'dd MMM yy') +'&Shift='+ RS.ShiftCode  +'&Sequence='+ CAST(RS.SequenceNo AS nvarchar(1))  , 
		--			CS.LineCode, 
		--			LineName = CS.LineCode + ' - ' + MSL.LineName, 
		--			LineDesc = MSL.LineName,
		--			CS.ItemCheckCode , 
		--			ItemCheck = CS.ItemCheckCode + ' - ' + ICM.ItemCheck, 
		--			ItemCheckDesc = ICM.ItemCheck,
		--			Date = FORMAT(RS.ProdDate, 'dd MMM yy'), 
		--			LinkDate = FORMAT(RS.ProdDate, 'yyyy-MM-dd'),
		--			--CASE 
		--			--	WHEN RS.ShiftCode = 'SH001' THEN '1'
		--			--	WHEN RS.ShiftCode = 'SH002' THEN '2'
		--			--	ELSE RS.ShiftCode
		--			--	END ShiftCode,
		--			RS.ShiftCode AS ShiftCode, 
		--			RS.ShiftCode AS ShiftCodeHeader, 
		--			RS.SequenceNo, 
		--			USL = CS.SpecUSL, 
		--			LSL = CS.SpecLSL, 
		--			UCL = CS.CPUCL, 
		--			LCL = CS.CPLCL,
		--			MAX(RD.Value) as MaxValue, MIN(RD.Value) AS MinValue, cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,4)) AS Average,
		--			RValue = MAX(RD.Value) - MIN(RD.Value),
		--			CASE
		--				WHEN (MAX(RD.Value) - MIN(RD.Value)) > CS.RUCL THEN 1
		--				Else 0
		--			END RColor,
		--			CASE
		--				WHEN 
		--				(
		--					SELECT SUM(SubCountDataOK.ValueSubRD) from (
		--					select 
		--					CASE
		--					WHEN subRD.VALUE < CS.SpecUSL AND subRD.VALUE > CS.SpecLSL THEN 1
		--					ELSE 0
		--					END ValueSubRD
		--					from spc_ResultDetail subRD where subRD.SPCResultID = RD.SPCResultID and subRD.DeleteStatus <> 1 
		--					and (subRD.Value1 is Null or subRD.Value1 + subRD.Value2 is not Null)
		--					) SubCountDataOK
		--				) >= ICT.SampleSize  THEN 'OK'
		--				WHEN MAX(RD.Value) > CS.SpecUSL THEN 'NG'
		--				WHEN MIN(RD.Value) < CS.SpecLSL THEN 'NG'
		--				WHEN 
		--					cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,4)) > CS.SpecUSL 
		--				THEN 'NG'
		--				WHEN 
		--					cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,4)) < CS.SpecLSL 
		--				THEN 'NG'
		--				ELSE 'OK'
		--			END StatusNG,
		--			----Operator = RS.RegisterUser,
		--			--MK = RS.MKVerificationUser, 
		--			--QC = RS.QCVerificationUser, 
		--			Operator = ISNULL(US.FullName, ''), 
		--			MK = ISNULL(US3.FullName, ''),
		--			QC = ISNULL(US2.FullName, ''),
		--			UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate),
		--			CharacteristicStatus = ICT.CharacteristicStatus,
		--			CASE
		--				WHEN FTA.FTAResult = 2 THEN 'NG'
		--				WHEN FTA.FTAResult = 1 THEN 'OK'
		--				ELSE ''
		--			END StatusFTA,
		--			--ActionFTA = ISNULL(FTARD.Action, ''),
		--			MKFTA = ISNULL(FTA.MKVerificationUser, ''),
		--			QCFTA = ISNULL(FTA.QCVerificationUser, '')
		--		from spc_ChartSetup CS
		--		INNER JOIN spc_Result RS ON CS.FactoryCode = RS.FactoryCode AND CS.ItemTypeCode = RS.ItemTypeCode AND CS.LineCode = RS.LineCode 
		--		INNER JOIN spc_ResultDetail RD ON RS.SPCResultID = RD.SPCResultID
		--		AND CS.ItemCheckCode = RS.ItemCheckCode
		--		INNER JOIN MS_Factory MSF ON CS.FactoryCode = MSF.FactoryCode
		--		INNER JOIN MS_ItemType IT ON CS.ItemTypeCode = IT.ItemTypeCode
		--		INNER JOIN MS_Line MSL ON CS.LineCode = MSL.LineCode
		--		INNER JOIN spc_ItemCheckMaster ICM ON CS.ItemCheckCode = ICM.ItemCheckCode
		--		INNER JOIN spc_ItemCheckByType ICT ON CS.FactoryCode = ICT.FactoryCode AND CS.LineCode = ICT.LineCode AND CS.ItemTypeCode = ICT.ItemTypeCode AND CS.ItemCheckCode = ICT.ItemCheckCode
		--		LEFT JOIN spc_UserSetup US ON RS.RegisterUser = US.UserID 
		--		LEFT JOIN spc_UserSetup US2 ON RS.QCVerificationUser = US2.UserID 
		--		LEFT JOIN spc_UserSetup US3 ON RS.MKVerificationUser = US3.UserID 
		--		LEFT JOIN spc_FTAResult FTA ON RS.SPCResultID = FTA.SPCResultID
		--		LEFT JOIN spc_FTAResultDetail FTARD ON FTA.FTAResultID = FTARD.FTAResultID AND FTA.FTAResult = FTARD.FTAResult
		--		WHERE 
		--		--RS.ProdDate IN (convert(varchar, @ProdDateYesterday, 23), convert(varchar, @ProdDate, 23)) AND 
		--		RS.ProdDate BETWEEN CS.StartDate and CS.EndDate
		--		AND 
		--		1 = 
		--		CASE
		--			WHEN RS.QCVerificationUser IS NULL THEN 1
		--			WHEN RS.QCVerificationUser = '' THEN 1
		--			ELSE 0
		--		END
		--		AND RD.DeleteStatus <> 1
		--		and (RD.Value1 is Null or RD.Value1 + RD.Value2 is not Null)
		--		--and 1 = 
		--		--		CASE 
		--		--			WHEN ISNULL(FTA.MKVerificationUser, '') = '' THEN 1
		--		--			WHEN ISNULL(FTA.QCVerificationUser, '') = '' THEN 1
		--		--		ELSE 0
		--		--END
		--		--convert(varchar, RS.ProdDate, 23) IN (convert(varchar, CS.StartDate, 23), convert(varchar, CS.EndDate, 23))
		--		GROUP BY CS.FactoryCode, MSF.FactoryName, CS.ItemTypeCode, IT.Description, CS.LineCode, MSL.LineName, CS.ItemCheckCode , ICM.ItemCheck, RS.ProdDate, RS.ShiftCode, RS.SequenceNo
		--		, CS.SpecUSL, CS.SpecLSL, CS.CPUCL, CS.CPLCL
		--		,RS.RegisterUser, RS.MKVerificationUser,RS.QCVerificationUser, RS.UpdateDate,RS.RegisterDate, RD.SPCResultID,ICT.SampleSize,ICT.CharacteristicStatus, CS.RUCL, US.FullName, US2.FullName, US3.FullName
		--		,FTA.MKVerificationUser,FTA.QCVerificationUser, FTA.FTAResult,  FTA.FTAResultID
		--	) tbl
		--	where StatusFTA = 'NG' --AND StatusFTA = 'OK'
		--	AND 1 = 
		--		CASE
		--			WHEN tbl.MK = '' THEN 1 
		--			WHEN tbl.QC = '' THEN 1 
		--			WHEN tbl.MKFTA = '' THEN 1
		--			WHEN tbl.QCFTA = '' THEN 1
		--		Else 0
		--	END
		--	AND 1 = CASE 
		--			WHEN @FactoryCode = 'ALL' THEN 1
		--			WHEN @FactoryCode <> 'ALL' AND FactoryCode = @FactoryCode THEN 1
		--			ELSE 0 END
		--	AND LineCode IN (
		--			select distinct L.LineCode
		--			from MS_Line L inner join spc_ItemCheckByType I 
		--			on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode 
		--			inner join spc_UserLine P on L.LineCode = P.LineCode 
		--			where P.UserID = @User and P.AllowShow = 1 and 
		--			1 = CASE 
		--			WHEN @FactoryCode = 'ALL' THEN 1
		--			WHEN @FactoryCode <> 'ALL' AND L.FactoryCode = @FactoryCode THEN 1
		--			ELSE 0 END
		--			)
		--	ORDER BY Date, UpdateDate, FactoryCode, ItemTypeName, LineName, ItemCheck, ShiftCode, SequenceNo
		SELECT 
			('Edit||ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ Date +'&Shift='+ ShiftCode  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1)) ) As Edit,
					CONCAT ( [Date], '|' , ItemTypeNameLink , '|' , LineName , '|' , ItemCheck, '|' , ShiftCode, '|', SequenceNo) AS Label,
					('ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ LinkDate +'&Shift='+ ShiftCode  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1))) As Link,
			RValueSPCDashboard = 
				CASE
					WHEN CharacteristicStatus = 0 THEN '||0'
					WHEN CharacteristicStatus = 1 THEN Convert(varchar(10), RValue) + '||' + Convert(varchar(10), RColor) 
				END,
			* FROM (
				SELECT 
				spcRs.FactoryCode,
				spcRs.ItemTypeCode,
				ItemTypeName = MSI.Description + '||' + spcICT.CharacteristicStatus + '||ProdSampleInput.aspx?FactoryCode='+ spcCS.FactoryCode +'&ItemTypeCode='+ spcCS.ItemTypeCode +'&Line='+ spcCS.LineCode +'&ItemCheckCode='+ 
									spcCS.ItemCheckCode +'&ProdDate='+ FORMAT(spcRs.ProdDate, 'dd MMM yy') +'&Shift='+ spcRs.ShiftCode  +'&Sequence='+ CAST(spcRs.SequenceNo AS nvarchar(1)) + '||FTACorrectiveAction.aspx?FactoryCode='+ spcCS.FactoryCode +'&ItemTypeCode='+ spcCS.ItemTypeCode +'&Line='+ spcCS.LineCode +'&ItemCheckCode='+ 
									spcCS.ItemCheckCode +'&ProdDate='+ FORMAT(spcRs.ProdDate, 'dd MMM yy') +'&Shift='+ spcRs.ShiftCode  +'&Sequence='+ CAST(spcRs.SequenceNo AS nvarchar(1))  ,
				spcRs.LineCode,
				LineName = spcRs.LineCode + ' - '+ MSL.LineName,
				spcRs.ItemCheckCode,
				ItemCheck = spcRs.ItemCheckCode + ' - ' + spcICM.ItemCheck,
				[Date] = FORMAT(spcRs.ProdDate, 'dd MMM yy'),
				spcRs.ShiftCode,
				spcRs.SequenceNo,
				USL = spcCS.SpecUSL,
				LSL = spcCS.SpecLSL,
				UCL = spcCS.CPUCL,
				LCL = spcCS.CPLCL,
				spcRD.MaxValue,
				spcRD.MinValue,
				spcRD.Average,
				spcRD.RValue,
				CASE
					WHEN spcRD.RValue > spcCS.RUCL THEN 1
					Else 0
				END RColor,
				Operator = ISNULL(spcUS.FullName, ''),
				MK = ISNULL(spcUS2.FullName, ''),
				QC = ISNULL(spcUS3.FullName, ''),
				UpdateDate = COALESCE(spcRs.UpdateDate, spcRs.RegisterDate),
				spcICT.CharacteristicStatus,
				CASE
					WHEN spcFTAR.FTAResult = 2 THEN 'NG'
					WHEN spcFTAR.FTAResult = 1 THEN 'OK'
					ELSE ''
				END StatusFTA,
				MKFTA = ISNULL(spcFTAR.MKVerificationUser, ''),
				QCFTA = ISNULL(spcFTAR.QCVerificationUser, ''),
				ActionFTA = ISNULL(spcFTAD.Action, ''),
				ItemTypeNameLink = MSI.Description,
				LinkDate = FORMAT(spcRs.ProdDate, 'yyyy-MM-dd')
	
				from spc_Result spcRs
				INNER JOIN (select SPCResultID, MinValue = MIN(Value), MaxValue = MAX(Value), Average = cast((SUM(Value) / COUNT(SPCResultID)) as decimal(10,4)), RValue = MAX(Value) - MIN(Value)
				from spc_ResultDetail GROUP BY SPCResultID) spcRD ON spcRs.SPCResultID = spcRD.SPCResultID
				INNER JOIN MS_ItemType MSI ON spcRs.ItemTypeCode = MSI.ItemTypeCode
				INNER JOIN MS_Line MSL ON spcRs.LineCode = MSL.LineCode
				INNER JOIN spc_ItemCheckMaster spcICM ON spcRs.ItemCheckCode = spcICM.ItemCheckCode
				INNER JOIN spc_ChartSetup spcCS ON spcRs.FactoryCode = spcCS.FactoryCode AND spcRs.ItemTypeCode = spcCS.ItemTypeCode AND spcRs.LineCode = spcCS.LineCode AND spcRs.ItemCheckCode = spcCS.ItemCheckCode 
				LEFT JOIN spc_UserSetup spcUS ON spcRs.RegisterUser = spcUS.UserID
				LEFT JOIN spc_UserSetup spcUS2 ON spcRs.MKVerificationUser = spcUS2.UserID
				LEFT JOIN spc_UserSetup spcUS3 ON spcRs.MKVerificationUser = spcUS3.UserID
				INNER JOIN spc_ItemCheckByType spcICT ON spcRs.FactoryCode = spcICT.FactoryCode AND spcRs.ItemTypeCode = spcICT.ItemTypeCode AND spcRs.LineCode = spcICT.LineCode AND spcRs.ItemCheckCode = spcICT.ItemCheckCode
				LEFT JOIN spc_FTAResult spcFTAR ON spcRs.SPCResultID = spcFTAR.SPCResultID
				LEFT JOIN spc_FTAResultDetail spcFTAD ON spcFTAR.FTAResultID = spcFTAD.FTAResultID AND spcFTAD.FTAResult = 2
				WHERE FORMAT(spcRs.ProdDate, 'dd MMM yy') BETWEEN FORMAT(spcCS.StartDate, 'dd MMM yy') AND FORMAT(spcCS.EndDate, 'dd MMM yy')
				AND spcRs.FTAStatus = 1
				AND 
				1 = 
					CASE
						WHEN ISNULL(spcUS2.FullName, '') = '' THEN 1
						WHEN ISNULL(spcUS3.FullName, '') = '' THEN 1
						WHEN ISNULL(spcFTAR.MKVerificationUser, '') = '' THEN 1
						WHEN ISNULL(spcFTAR.QCVerificationUser, '') = '' THEN 1
						ELSE 0
					END
			) TBL
			where 
				1 = 
				CASE 
					WHEN tbl.MK = '' THEN 1
					WHEN tbl.QC = '' THEN 1
					WHEN tbl.MKFTA = '' THEN 1
					WHEN tbl.QCFTA = '' THEN 1 
				END 
				AND
				1 = CASE 
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
			ORDER BY FactoryCode, ItemTypeCode, LineCode, ItemCheckCode, [Date], ShiftCode, SequenceNo
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
					MAX(RD.Value) as MaxValue, MIN(RD.Value) AS MinValue, cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,4)) AS Average,
					CASE
						WHEN MAX(RD.Value) > CS.SpecUSL THEN 'NG'
						WHEN MIN(RD.Value) < CS.SpecLSL THEN 'NG'
						WHEN 
							cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,4)) > CS.SpecUSL 
						THEN 'NG'
						WHEN 
							cast((SUM(RD.Value) / COUNT(RD.SPCResultID)) as decimal(10,4)) < CS.SpecLSL 
						THEN 'NG'
						ELSE 'OK'
					END StatusNG,
					--Operator = RS.RegisterUser,
					Operator = ISNULL(US.FullName, ''), 
					--MK = RS.MKVerificationUser, 
					--QC = RS.QCVerificationUser,
					MK = ISNULL(US3.FullName, ''),
					QC = ISNULL(US2.FullName, ''), 
					UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate)
				from spc_ChartSetup CS
				INNER JOIN spc_Result RS ON CS.FactoryCode = RS.FactoryCode AND CS.ItemTypeCode = RS.ItemTypeCode AND CS.LineCode = RS.LineCode 
				INNER JOIN spc_ResultDetail RD ON RS.SPCResultID = RD.SPCResultID
				AND CS.ItemCheckCode = RS.ItemCheckCode
				INNER JOIN MS_Factory MSF ON CS.FactoryCode = MSF.FactoryCode
				INNER JOIN MS_ItemType IT ON CS.ItemTypeCode = IT.ItemTypeCode
				INNER JOIN MS_Line MSL ON CS.LineCode = MSL.LineCode
				INNER JOIN spc_ItemCheckMaster ICM ON CS.ItemCheckCode = ICM.ItemCheckCode
				LEFT JOIN spc_UserSetup US ON RS.RegisterUser = US.UserID 
				LEFT JOIN spc_UserSetup US2 ON RS.QCVerificationUser = US2.UserID 
				LEFT JOIN spc_UserSetup US3 ON RS.MKVerificationUser = US3.UserID 
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
				and (RD.Value1 is Null or RD.Value1 + RD.Value2 is not Null)
				--convert(varchar, RS.ProdDate, 23) IN (convert(varchar, CS.StartDate, 23), convert(varchar, CS.EndDate, 23))
				GROUP BY CS.FactoryCode, MSF.FactoryName, CS.ItemTypeCode, IT.Description, CS.LineCode, MSL.LineName, CS.ItemCheckCode , ICM.ItemCheck, RS.ProdDate, RS.ShiftCode, RS.SequenceNo
				, CS.SpecUSL, CS.SpecLSL, CS.CPUCL, CS.CPLCL
				,RS.RegisterUser, RS.MKVerificationUser,RS.QCVerificationUser, RS.UpdateDate,RS.RegisterDate, US.FullName, US2.FullName, US3.FullName
			) tbl
			where StatusNG = 'NG' AND FactoryCode = @FactoryCode
			ORDER BY Date, UpdateDate, FactoryCode, ItemTypeName, LineName, ItemCheck, ShiftCode, SequenceNo
	END

END
SET NOCOUNT OFF
