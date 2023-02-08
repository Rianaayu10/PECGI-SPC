/****** Object:  StoredProcedure [dbo].[sp_SPC_GetDelayVerification]    Script Date: 2/8/2023 4:31:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: Malik Ilman	
-- Create date: 06-10-2022
-- Description:	Get Delay Verification MK Or QC
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_GetDelayVerification]
	@User AS Varchar(10) = 'zqc',
	@FactoryCode AS Varchar(8) = 'F001',
	@TypeReport AS int = 1, -- 1 For Web, 2 For Windows Form
	@ProdDateType AS int = 1,
	@ProdDate AS DATETIME = null,
	@TypeForm As Char(1) = '1', --1 for notification, 2 for AlertDashboard
	@MenuID As varchar(8) = null,
	@FilterDate As varchar(25) = null
As
SET NOCOUNT ON
BEGIN

	DECLARE @ProdDateYesterday AS DATETIME,
			@UserPosition AS CHAR(2) = '',
			@MenuName AS varchar(100) = ''

	SELECT @UserPosition = JobPosition FROM spc_UserSetup Where UserID = @User 

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

	IF @MenuID = 'B010'
		BEGIN
			SET @MenuName = 'SPCDashboard.aspx'
		END
	ELSE IF @MenuID = 'X030'
		BEGIN
			SET @MenuName = 'AlertDelayVerification.aspx'
		END
	
	IF @TypeReport = 1
	BEGIN
		SELECT 
		CASE
			WHEN CONVERT(varchar(15), DelayVerif / 1440 ) > 0 THEN CONVERT(varchar(15), DelayVerif / 1440 ) + ' D ' + REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (DelayVerif % 1440) / 60 ))) + convert(varchar(2), (DelayVerif % 1440) / 60 ) +':' + convert(varchar(2), ((DelayVerif % 1440) % 60) ) 
			WHEN CONVERT(varchar(15), DelayVerif / 1440 ) <= 0 AND convert(varchar(2), (DelayVerif % 1440) / 60 ) > 0 THEN REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (DelayVerif % 1440) / 60 ))) + convert(varchar(2), (DelayVerif % 1440) / 60 ) +':' + convert(varchar(2), ((DelayVerif %1440) % 60) ) 
			ELSE convert(varchar(2), ((DelayVerif %1440) % 60) ) + ' Minutes'
		END AS DelayForSPCDashboard,
		CASE
			WHEN CONVERT(varchar(15), DelayVerif / 1440 ) > 0 THEN CONVERT(varchar(15), DelayVerif / 1440 ) + ' Day ' + REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (DelayVerif % 1440) / 60 ))) + convert(varchar(2), (DelayVerif % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((DelayVerif % 1440) % 60) ) + ' Minutes'
			WHEN CONVERT(varchar(15), DelayVerif / 1440 ) <= 0 AND convert(varchar(2), (DelayVerif % 1440) / 60 ) > 0 THEN REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (DelayVerif % 1440) / 60 ))) + convert(varchar(2), (DelayVerif % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((DelayVerif %1440) % 60) ) + ' Minutes'
			ELSE convert(varchar(2), ((DelayVerif %1440) % 60) ) + ' Minutes'
		END AS DelayHeader,
			('Edit||ProdSampleVerification.aspx?menu='+ @MenuName +'&FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ Date +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1)) +'&ShowVerify='+ ShowVerify + '&FilterDate='+ @FilterDate ) As Edit,
			CONCAT ( [Date],'|' , ItemTypeNameLink , '|' , LineName , '|' , ItemCheck, '|' , ShiftCode, '|', SequenceNo, '|',
						CASE
						WHEN CONVERT(varchar(15), DelayVerif / 1440 ) > 0 THEN CONVERT(varchar(15), DelayVerif / 1440 ) + ' Day ' + REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (DelayVerif % 1440) / 60 ))) + convert(varchar(2), (DelayVerif % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((DelayVerif % 1440) % 60) ) + ' Minutes'
						WHEN CONVERT(varchar(15), DelayVerif / 1440 ) <= 0 AND convert(varchar(2), (DelayVerif % 1440) / 60 ) > 0 THEN REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (DelayVerif % 1440) / 60 ))) + convert(varchar(2), (DelayVerif % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((DelayVerif %1440) % 60) ) + ' Minutes'
						ELSE convert(varchar(2), ((DelayVerif %1440) % 60) ) + ' Minutes'
						END
					) AS Label,
			('ProdSampleVerification.aspx?menu=Notification&FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ LinkDate +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1)) +'&ShowVerify='+ ShowVerify) As Link,
			RValueSPCDashboard = 
				CASE
					WHEN CharacteristicStatus = 0 THEN ''
					WHEN CharacteristicStatus = 1 THEN Convert(varchar(10), RValue) + '||' + Convert(varchar(10), RColor) END,
		* FROM (
				select 
					FactoryCode = CS.FactoryCode, 
					MF.FactoryName, 
					CS.ItemTypeCode, 
					ItemTypeNameLink = IT.Description , 
					ItemTypeName = IT.Description + '||' + ICT.CharacteristicStatus + '||ProdSampleVerification.aspx?menu='+ @MenuName +'&FactoryCode='+ CS.FactoryCode +'&ItemTypeCode='+ CS.ItemTypeCode +'&Line='+ CS.LineCode +'&ItemCheckCode='+ 
					CS.ItemCheckCode +'&ProdDate='+ FORMAT(RS.ProdDate, 'dd MMM yy') +'&Shift='+ RS.ShiftCode  +'&Sequence='+ CAST(RS.SequenceNo AS nvarchar(1)) +'&ShowVerify='+ COALESCE(RS.CompleteStatus,'0')  , 
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
					Operator = RS.RegisterUser, 
					MK = RS.MKVerificationUser, 
					QC = RS.QCVerificationUser, 
					VerifTime = CONVERT(varchar, MSF.VerificationTime, 8),
					OriginalVerifTime = FORMAT(RS.ProdDate, 'yyyy-MM-dd') + ' ' + CONVERT(varchar, MSF.VerificationTime, 8),
					CASE 
						WHEN FORMAT(RS.ProdDate, 'yyyy-MM-dd') = FORMAT(@ProdDate, 'yyyy-MM-dd') THEN DATEDIFF(MINUTE, (convert(varchar, @ProdDate, 23) + ' ' + convert(varchar, MSF.VerificationTime, 8)), convert(varchar, GETDATE(), 120))
						WHEN FORMAT(RS.ProdDate, 'yyyy-MM-dd') = FORMAT(@ProdDateYesterday, 'yyyy-MM-dd') THEN DATEDIFF(MINUTE, (convert(varchar, @ProdDateYesterday, 23) + ' ' + convert(varchar, MSF.VerificationTime, 8)), convert(varchar, GETDATE(), 120))
					END DelayVerif,
					UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate),
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
					END Status,
					ShowVerify = COALESCE(RS.CompleteStatus,'0'),
					CompleteStatus = RS.CompleteStatus,
					CharacteristicStatus = ICT.CharacteristicStatus
				from spc_ChartSetup CS
				INNER JOIN spc_Result RS ON CS.FactoryCode = RS.FactoryCode AND CS.ItemTypeCode = RS.ItemTypeCode AND CS.LineCode = RS.LineCode 
				INNER JOIN spc_ResultDetail RD ON RS.SPCResultID = RD.SPCResultID
				AND CS.ItemCheckCode = RS.ItemCheckCode
				INNER JOIN MS_Factory MF ON CS.FactoryCode = MF.FactoryCode
				INNER JOIN MS_ItemType IT ON CS.ItemTypeCode = IT.ItemTypeCode
				INNER JOIN MS_Line MSL ON CS.LineCode = MSL.LineCode
				INNER JOIN spc_ItemCheckMaster ICM ON CS.ItemCheckCode = ICM.ItemCheckCode
				INNER JOIN spc_ItemCheckByType ICT ON CS.FactoryCode = ICT.FactoryCode AND CS.ItemTypeCode = ICT.ItemTypeCode AND CS.ItemCheckCode = ICT.ItemCheckCode
				AND CS.LineCode = ICT.LineCode
				INNER JOIN spc_MS_Frequency MSF ON ICT.FrequencyCode = MSF.FrequencyCode AND RS.ShiftCode = MSF.ShiftCode AND RS.SequenceNo = MSF.SequenceNo
				WHERE RS.ProdDate IN (convert(varchar, @ProdDateYesterday, 23), convert(varchar, @ProdDate, 23)) AND RS.ProdDate BETWEEN CS.StartDate and CS.EndDate
				AND 1 = 
				CASE
					WHEN RS.QCVerificationUser IS NULL THEN 1
					WHEN RS.QCVerificationUser = '' THEN 1
					ELSE 0
				END
				AND RD.DeleteStatus <> 1
				GROUP BY CS.FactoryCode, MF.FactoryName, CS.ItemTypeCode, IT.Description, CS.LineCode, MSL.LineName, CS.ItemCheckCode , ICM.ItemCheck, RS.ProdDate, RS.ShiftCode, RS.SequenceNo
				, CS.SpecUSL, CS.SpecLSL, CS.CPUCL, CS.CPLCL, RS.CompleteStatus
				,RS.RegisterUser, RS.MKVerificationUser,RS.QCVerificationUser, RS.UpdateDate,RS.RegisterDate,MSF.VerificationTime,ICT.CharacteristicStatus,CS.RUCL
			) TBL
			where OriginalVerifTime < convert(varchar, getdate(), 120)--convert(varchar, @ProdDate, 8) > VerifTime --OriginalVerifTime < GETDATE()
			AND TBL.CompleteStatus = 1
			AND 1 = 
			CASE
				WHEN MK IS NULL THEN 1
				WHEN MK = '' THEN 1
				WHEN QC IS NULL THEN 1
				WHEN QC = '' THEN 1
				ELSE 0 END
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
			AND 1 = CASE WHEN @TypeForm = '2' THEN 1
						WHEN @TypeForm = '1' AND @UserPosition = 'MK' AND ISNULL(MK,'') = '' THEN 1
						WHEN @TypeForm = '1' AND @UserPosition = 'QC' AND ISNULL(QC,'') = '' THEN 1
						ELSE 0 END    
					)
			ORDER BY DelayVerif desc ,Date, UpdateDate, FactoryCode, ItemTypeName, LineName, ItemCheck, ShiftCode, SequenceNo
	END
	ELSE IF @TypeReport = 2
	BEGIN
		
		SELECT 
			('Edit||ProdSampleInput.aspx?FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ Date +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1)) ) As Edit,
			CONCAT ( [Date],'|' , ItemTypeName , '|' , LineName , '|' , ItemCheck, '|' , ShiftCode, '|', SequenceNo, '|',
						CASE
						WHEN CONVERT(varchar(15), DelayVerif / 1440 ) > 0 THEN CONVERT(varchar(15), DelayVerif / 1440 ) + ' Day ' + REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (DelayVerif % 1440) / 60 ))) + convert(varchar(2), (DelayVerif % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((DelayVerif % 1440) % 60) ) + ' Minutes'
						WHEN CONVERT(varchar(15), DelayVerif / 1440 ) <= 0 AND convert(varchar(2), (DelayVerif % 1440) / 60 ) > 0 THEN REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (DelayVerif % 1440) / 60 ))) + convert(varchar(2), (DelayVerif % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((DelayVerif %1440) % 60) ) + ' Minutes'
						ELSE convert(varchar(2), ((DelayVerif %1440) % 60) ) + ' Minutes'
						END
					) AS Label,
			('ProdSampleVerification.aspx?menu=ProductionSampleVerificationList.aspx&FactoryCode='+ FactoryCode +'&ItemTypeCode='+ ItemTypeCode +'&Line='+ LineCode +'&ItemCheckCode='+ ItemCheckCode +'&ProdDate='+ LinkDate +'&Shift='+ ShiftCodeHeader  +'&Sequence='+ CAST(SequenceNo AS nvarchar(1)) +'&ShowVerify='+ ShowVerify) As Link,
			* FROM (
					select 
						FactoryCode = CS.FactoryCode, 
						MF.FactoryName, 
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
						Operator = RS.RegisterUser, 
						MK = RS.MKVerificationUser, 
						QC = RS.QCVerificationUser, 
						VerifTime = CONVERT(varchar, MSF.VerificationTime, 8),
						OriginalVerifTime = FORMAT(RS.ProdDate, 'yyyy-MM-dd') + ' ' + CONVERT(varchar, MSF.VerificationTime, 8),
						DelayVerif = DATEDIFF(MINUTE, (convert(varchar, @ProdDate, 23) + ' ' + convert(varchar, MSF.VerificationTime, 8)), convert(varchar, @ProdDate, 120)),
						UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate),
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
						END Status,
						ShowVerify = COALESCE(RS.CompleteStatus,'0'),
					CompleteStatus = RS.CompleteStatus
					from spc_ChartSetup CS
					INNER JOIN spc_Result RS ON CS.FactoryCode = RS.FactoryCode AND CS.ItemTypeCode = RS.ItemTypeCode AND CS.LineCode = RS.LineCode 
					INNER JOIN spc_ResultDetail RD ON RS.SPCResultID = RD.SPCResultID
					AND CS.ItemCheckCode = RS.ItemCheckCode
					INNER JOIN MS_Factory MF ON CS.FactoryCode = MF.FactoryCode
					INNER JOIN MS_ItemType IT ON CS.ItemTypeCode = IT.ItemTypeCode
					INNER JOIN MS_Line MSL ON CS.LineCode = MSL.LineCode
					INNER JOIN spc_ItemCheckMaster ICM ON CS.ItemCheckCode = ICM.ItemCheckCode
					INNER JOIN spc_ItemCheckByType ICT ON CS.FactoryCode = ICT.FactoryCode AND CS.ItemTypeCode = ICT.ItemTypeCode AND CS.ItemCheckCode = ICT.ItemCheckCode
					AND CS.LineCode = ICT.LineCode
					INNER JOIN spc_MS_Frequency MSF ON ICT.FrequencyCode = MSF.FrequencyCode AND RS.ShiftCode = MSF.ShiftCode AND RS.SequenceNo = MSF.SequenceNo
					WHERE RS.ProdDate IN (convert(varchar, @ProdDate-1, 23), convert(varchar, @ProdDate, 23)) AND RS.ProdDate BETWEEN CS.StartDate and CS.EndDate
					AND 1 = 
					CASE
						WHEN RS.QCVerificationUser IS NULL THEN 1
						WHEN RS.QCVerificationUser = '' THEN 1
						ELSE 0
					END
					AND RD.DeleteStatus <> 1
					GROUP BY CS.FactoryCode, MF.FactoryName, CS.ItemTypeCode, IT.Description, CS.LineCode, MSL.LineName, CS.ItemCheckCode , ICM.ItemCheck, RS.ProdDate, RS.ShiftCode, RS.SequenceNo
					, CS.SpecUSL, CS.SpecLSL, CS.CPUCL, CS.CPLCL, RS.CompleteStatus
					,RS.RegisterUser, RS.MKVerificationUser,RS.QCVerificationUser, RS.UpdateDate,RS.RegisterDate,MSF.VerificationTime
				) TBL
				where OriginalVerifTime < convert(varchar, getdate(), 120)
				AND TBL.CompleteStatus = 1
				AND 1 = 
				CASE
					WHEN MK IS NULL THEN 1
					WHEN MK = '' THEN 1
					WHEN QC IS NULL THEN 1
					WHEN QC = '' THEN 1
					ELSE 0 END
				AND FactoryCode = @FactoryCode
				ORDER BY Date, UpdateDate, FactoryCode, ItemTypeName, LineName, ItemCheck, ShiftCode, SequenceNo
	END

END
SET NOCOUNT OFF
