/****** Object:  StoredProcedure [dbo].[sp_SPC_GetDelayInput]    Script Date: 12/20/2022 10:11:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author: Malik Ilman	
-- Create date: 24-08-2022
-- Description:	Get Report Delay Input Item
-- =============================================
-- =============================================
-- Editor : Malik Ilman	
-- Edit date: 19-10-2022
-- Description:	Change Filter Based On Production Date
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_GetDelayInput]
	@User AS Varchar(10) = 'zqc',
	@FactoryCode AS Varchar(8) = 'F001',
	@TypeReport AS int = 1, -- 1 For Web, 2 For Scheduler
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
		SELECT DISTINCT 
		CASE
			WHEN CONVERT(varchar(15), Delay / 1440 ) > 0 THEN CONVERT(varchar(15), Delay / 1440 ) + ' Day ' + REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (Delay % 1440) / 60 ))) + convert(varchar(2), (Delay % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((Delay % 1440) % 60) ) + ' Minutes'
			WHEN CONVERT(varchar(15), Delay / 1440 ) <= 0 AND convert(varchar(2), (Delay % 1440) / 60 ) > 0 THEN REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (Delay % 1440) / 60 ))) + convert(varchar(2), (Delay % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((Delay %1440) % 60) ) + ' Minutes'
			ELSE convert(varchar(2), ((Delay %1440) % 60) ) + ' Minutes'
		END AS DelayHeader, 
		CONCAT([Date],'|', ItemTypeName , '|' , LineName , '|' , ItemCheck , '|', ShiftCode ,'|', SequenceNo ,'|' , 
				CASE
				WHEN CONVERT(varchar(15), Delay / 1440 ) > 0 THEN CONVERT(varchar(15), Delay / 1440 ) + ' Day ' + REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (Delay % 1440) / 60 ))) + convert(varchar(2), (Delay % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((Delay % 1440) % 60) ) + ' Minutes'
				WHEN CONVERT(varchar(15), Delay / 1440 ) <= 0 AND convert(varchar(2), (Delay % 1440) / 60 ) > 0 THEN REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (Delay % 1440) / 60 ))) + convert(varchar(2), (Delay % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((Delay %1440) % 60) ) + ' Minutes'
				ELSE convert(varchar(2), ((Delay %1440) % 60) ) + ' Minutes'
				END
			) AS Label,
		* FROM (

			select ('Edit||ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
					TBL.ItemCheckCode +'&ProdDate='+ TBL.Date +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1)) ) As Edit,					
					('ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
					TBL.ItemCheckCode +'&ProdDate='+ FORMAT(@ProdDateYesterday,'yyyy-MM-dd') +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1))) As Link,
					(TBL.ItemTypeName + '||ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
					TBL.ItemCheckCode +'&ProdDate='+ TBL.Date +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1)) ) As TypeHeader,
			* from (
					select ICT.FactoryCode, 
									MsFtr.FactoryName,
									ItemTypeName = MSI.Description, 
									LineName = MSL.LineCode + ' - ' + MSL.LineName, 
									--LineDesc = MSL.LineName,
									ItemCheck = ICT.ItemCheckCode + ' - ' + ICM.ItemCheck, 
									--ItemCheckDesc = ICM.ItemCheck,
									Date = FORMAT(@ProdDateYesterday, 'dd MMM yy'),
									LinkDate = FORMAT(@ProdDateYesterday, 'yyyy-MM-dd'),
									--CASE
									--	WHEN MSF.ShiftCode = 'SH001' THEN '1'
									--	WHEN MSF.ShiftCode = 'SH002' THEN '2'
									--	ELSE
									--	MSF.ShiftCode
									--END ShiftCode,
									MSF.ShiftCode AS ShiftCode,
									MSF.ShiftCode AS ShiftCodeHeader,
									MSF.SequenceNo, 
									convert(varchar, MSF.StartTime, 8) as StartTime, convert(varchar, MSF.EndTime, 8) as EndTime, 
									--CASE 
									--WHEN MSF.SequenceNo = 5 THEN DATEDIFF(MINUTE, (convert(varchar, @ProdDateYesterday + 1, 23) + ' ' + convert(varchar, MSF.EndTime, 8)), convert(varchar, GETDATE(), 120))
									--ELSE DATEDIFF(MINUTE, (convert(varchar, @ProdDateYesterday, 23) + ' ' + convert(varchar, MSF.EndTime, 8)), convert(varchar, GETDATE(), 120))
									--END Delay,
									CASE 
									WHEN convert(varchar, MSF.EndTime, 8) < (select convert(varchar, StartTime, 8) from spc_MS_Frequency where FrequencyCode = ICT.FrequencyCode AND SequenceNo = 1)  
									THEN 
										DATEDIFF(MINUTE, (convert(varchar, @ProdDateYesterday + 1, 23) + ' ' + convert(varchar, MSF.EndTime, 8)), convert(varchar, GETDATE(), 120))
									ELSE 
										DATEDIFF(MINUTE, (convert(varchar, @ProdDateYesterday, 23) + ' ' + convert(varchar, MSF.EndTime, 8)), convert(varchar, GETDATE(), 120))
									END Delay,
									ICT.ItemTypeCode, ICT.LineCode, ICT.ItemCheckCode, ICT.FrequencyCode --,UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate)
									, UpdateTime = FORMAT(@ProdDateYesterday, 'yyyy-MM-dd') + ' ' + convert(varchar, MSF.EndTime, 8)
									from spc_ItemCheckByType ICT
									INNER JOIN spc_MS_Frequency MSF ON ICT.FrequencyCode = MSF.FrequencyCode	
									INNER JOIN MS_ItemType MSI ON ICT.ItemTypeCode = MSI.ItemTypeCode
									INNER JOIN MS_Line MSL ON ICT.LineCode = MSL.LineCode
									INNER JOIN spc_ItemCheckMaster ICM ON ICT.ItemCheckCode = ICM.ItemCheckCode
									LEFT JOIN MS_Factory MsFtr ON ICT.FactoryCode = MsFtr.FactoryCode
									--INNER JOIN spc_Result RS ON ICT.FactoryCode = RS.FactoryCode AND ICT.ItemTypeCode = RS.ItemTypeCode AND ICT.LineCode = RS.LineCode AND ICT.ItemCheckCode = RS.ItemCheckCode
									WHERE ICT.ActiveStatus = 1
					) tbl
					WHERE NOT EXISTS ( select * from spc_Result RS WHERE TBL.FactoryCode = RS.FactoryCode AND TBL.ItemTypeCode = RS.ItemTypeCode AND TBL.LineCode = RS.LineCode 
							AND TBL.ItemCheckCode = RS.ItemCheckCode AND TBL.ShiftCodeHeader = RS.ShiftCode AND TBL.SequenceNo = RS.SequenceNo
							AND RS.ProdDate = FORMAT(@ProdDateYesterday, 'dd MMM yy') AND RS.CompleteStatus = 1 )
			UNION ALL

			select ('Edit||ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
					TBL.ItemCheckCode +'&ProdDate='+ TBL.Date +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1)) ) As Edit,
					('ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
					TBL.ItemCheckCode +'&ProdDate='+ FORMAT(@ProdDate,'yyyy-MM-dd') +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1))) As Link,
					(TBL.ItemTypeName + '||ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
					TBL.ItemCheckCode +'&ProdDate='+ TBL.Date +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1)) ) As TypeHeader,
					* from (
							select ICT.FactoryCode, 
											MsFtr.FactoryName,
											ItemTypeName = MSI.Description, 
											LineName = MSL.LineCode + ' - ' + MSL.LineName, 
											--LineDesc = MSL.LineName,
											ItemCheck = ICT.ItemCheckCode + ' - ' + ICM.ItemCheck, 
											--ItemCheckDesc = ICM.ItemCheck,
											Date = FORMAT(@ProdDate, 'dd MMM yy'), 
											LinkDate = FORMAT(@ProdDate, 'yyyy-MM-dd'),
											--CASE
											--	WHEN MSF.ShiftCode = 'SH001' THEN '1'
											--	WHEN MSF.ShiftCode = 'SH002' THEN '2'
											--	ELSE
											--	MSF.ShiftCode
											--END ShiftCode,
											MSF.ShiftCode AS ShiftCode,
											MSF.ShiftCode AS ShiftCodeHeader,
											MSF.SequenceNo, 
											convert(varchar, MSF.StartTime, 8) as StartTime, convert(varchar, MSF.EndTime, 8) as EndTime, 
											Delay = DATEDIFF(MINUTE, (convert(varchar, @ProdDate, 23) + ' ' + convert(varchar, MSF.EndTime, 8)), convert(varchar, GETDATE(), 120)),
												ICT.ItemTypeCode, ICT.LineCode, ICT.ItemCheckCode, ICT.FrequencyCode --,UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate)
											, UpdateTime = FORMAT(@ProdDate, 'yyyy-MM-dd') + ' ' + convert(varchar, MSF.EndTime, 8)
											from spc_ItemCheckByType ICT
											INNER JOIN spc_MS_Frequency MSF ON ICT.FrequencyCode = MSF.FrequencyCode	
											INNER JOIN MS_ItemType MSI ON ICT.ItemTypeCode = MSI.ItemTypeCode
											INNER JOIN MS_Line MSL ON ICT.LineCode = MSL.LineCode
											INNER JOIN spc_ItemCheckMaster ICM ON ICT.ItemCheckCode = ICM.ItemCheckCode
											LEFT JOIN MS_Factory MsFtr ON ICT.FactoryCode = MsFtr.FactoryCode
											--INNER JOIN spc_Result RS ON ICT.FactoryCode = RS.FactoryCode AND ICT.ItemTypeCode = RS.ItemTypeCode AND ICT.LineCode = RS.LineCode AND ICT.ItemCheckCode = RS.ItemCheckCode
											WHERE ICT.ActiveStatus = 1
							) tbl
							WHERE NOT EXISTS ( select * from spc_Result RS WHERE TBL.FactoryCode = RS.FactoryCode AND TBL.ItemTypeCode = RS.ItemTypeCode AND TBL.LineCode = RS.LineCode 
												AND TBL.ItemCheckCode = RS.ItemCheckCode AND TBL.ShiftCodeHeader = RS.ShiftCode AND TBL.SequenceNo = RS.SequenceNo --AND TBL.DATE = RS.ProdDate
												AND RS.ProdDate = FORMAT(@ProdDate, 'dd MMM yy') AND RS.CompleteStatus = 1 )
												AND EndTime < convert(varchar, @ProdDate, 8)
							AND SequenceNo <> 5
			) TBL2 

		WHERE EXISTS (

				SELECT * FROM Daily_production DP WHERE TBL2.FactoryCode = DP.Factory_code AND TBL2.LineCode = DP.Line_Code 
				AND TBL2.ShiftCodeHeader = DP.Shift AND TBL2.ItemTypeName =  DP.Item_code AND TBL2.Date = FORMAT(DP.Schedule_Date, 'dd MMM yy')
			)
		AND
		1 = CASE WHEN @FactoryCode = 'ALL' THEN 1
								WHEN @FactoryCode <> 'ALL' AND TBL2.FactoryCode = @FactoryCode THEN 1
								ELSE 0 END
		AND TBL2.LineCode IN (
				select distinct L.LineCode
				from MS_Line L inner join spc_ItemCheckByType I 
				on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode 
				inner join spc_UserLine P on L.LineCode = P.LineCode 
				where P.UserID = @User and P.AllowShow = 1 
				AND 1 = CASE WHEN @FactoryCode = 'ALL' THEN 1
					WHEN @FactoryCode <> 'ALL' AND L.FactoryCode = @FactoryCode THEN 1
					ELSE 0 END
		) 
		ORDER BY Delay DESC, ItemTypeName ASC, LineName ASC 
	END
	ELSE IF @TypeReport = 2
	BEGIN
		
		SELECT DISTINCT 
		CASE
			WHEN CONVERT(varchar(15), Delay / 1440 ) > 0 THEN CONVERT(varchar(15), Delay / 1440 ) + ' Day ' + REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (Delay % 1440) / 60 ))) + convert(varchar(2), (Delay % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((Delay % 1440) % 60) ) + ' Minutes'
			WHEN CONVERT(varchar(15), Delay / 1440 ) <= 0 AND convert(varchar(2), (Delay % 1440) / 60 ) > 0 THEN REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (Delay % 1440) / 60 ))) + convert(varchar(2), (Delay % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((Delay %1440) % 60) ) + ' Minutes'
			ELSE convert(varchar(2), ((Delay %1440) % 60) ) + ' Minutes'
		END AS DelayHeader, 
		CONCAT([Date],'|', ItemTypeName , '|' , LineName , '|' , ItemCheck , '|', ShiftCode ,'|', SequenceNo ,'|' , 
				CASE
				WHEN CONVERT(varchar(15), Delay / 1440 ) > 0 THEN CONVERT(varchar(15), Delay / 1440 ) + ' Day ' + REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (Delay % 1440) / 60 ))) + convert(varchar(2), (Delay % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((Delay % 1440) % 60) ) + ' Minutes'
				WHEN CONVERT(varchar(15), Delay / 1440 ) <= 0 AND convert(varchar(2), (Delay % 1440) / 60 ) > 0 THEN REPLICATE('0', 2 - DATALENGTH(convert(varchar(2), (Delay % 1440) / 60 ))) + convert(varchar(2), (Delay % 1440) / 60 ) +' Hours ' + convert(varchar(2), ((Delay %1440) % 60) ) + ' Minutes'
				ELSE convert(varchar(2), ((Delay %1440) % 60) ) + ' Minutes'
				END
			) AS Label,
			* FROM (

				select ('Edit||ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
						TBL.ItemCheckCode +'&ProdDate='+ TBL.Date +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1)) ) As Edit,					
						('ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
						TBL.ItemCheckCode +'&ProdDate='+ FORMAT(@ProdDate- 1,'yyyy-MM-dd') +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1))) As Link,
				* from (
						select ICT.FactoryCode, 
										MsFtr.FactoryName,
										ItemTypeName = MSI.Description, 
										LineName = MSL.LineCode + ' - ' + MSL.LineName, 
										--LineDesc = MSL.LineName,
										ItemCheck = ICT.ItemCheckCode + ' - ' + ICM.ItemCheck, 
										--ItemCheckDesc = ICM.ItemCheck,
										Date = FORMAT(@ProdDate - 1, 'dd MMM yy'),
										LinkDate = FORMAT(@ProdDate - 1, 'yyyy-MM-dd'),
										CASE
											WHEN MSF.ShiftCode = 'SH001' THEN '1'
											WHEN MSF.ShiftCode = 'SH002' THEN '2'
											ELSE
											MSF.ShiftCode
										END ShiftCode,
										MSF.ShiftCode AS ShiftCodeHeader,
										MSF.SequenceNo, 
										convert(varchar, MSF.StartTime, 8) as StartTime, convert(varchar, MSF.EndTime, 8) as EndTime, 
										--Delay = DATEDIFF(MINUTE, convert(varchar, MSF.EndTime, 8), convert(varchar, @ProdDate, 8)),
										--Delay = DATEDIFF(MINUTE, (convert(varchar, @ProdDate - 1, 23) + ' ' + convert(varchar, MSF.EndTime, 8)), convert(varchar, @ProdDate, 120)),
										CASE 
										WHEN MSF.SequenceNo = 5 THEN DATEDIFF(MINUTE, (convert(varchar, @ProdDate, 23) + ' ' + convert(varchar, MSF.EndTime, 8)), convert(varchar, @ProdDate, 120))
										ELSE DATEDIFF(MINUTE, (convert(varchar, @ProdDate - 1, 23) + ' ' + convert(varchar, MSF.EndTime, 8)), convert(varchar, @ProdDate, 120))
										END Delay,
										ICT.ItemTypeCode, ICT.LineCode, ICT.ItemCheckCode, ICT.FrequencyCode --,UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate)
										, UpdateTime = FORMAT(@ProdDate -1, 'yyyy-MM-dd') + ' ' + convert(varchar, MSF.EndTime, 8)
										from spc_ItemCheckByType ICT
										INNER JOIN spc_MS_Frequency MSF ON ICT.FrequencyCode = MSF.FrequencyCode	
										INNER JOIN MS_ItemType MSI ON ICT.ItemTypeCode = MSI.ItemTypeCode
										INNER JOIN MS_Line MSL ON ICT.LineCode = MSL.LineCode
										INNER JOIN spc_ItemCheckMaster ICM ON ICT.ItemCheckCode = ICM.ItemCheckCode
										LEFT JOIN MS_Factory MsFtr ON ICT.FactoryCode = MsFtr.FactoryCode
										--INNER JOIN spc_Result RS ON ICT.FactoryCode = RS.FactoryCode AND ICT.ItemTypeCode = RS.ItemTypeCode AND ICT.LineCode = RS.LineCode AND ICT.ItemCheckCode = RS.ItemCheckCode
										WHERE ICT.ActiveStatus = 1
						) tbl
						WHERE NOT EXISTS ( select * from spc_Result RS WHERE TBL.FactoryCode = RS.FactoryCode AND TBL.ItemTypeCode = RS.ItemTypeCode AND TBL.LineCode = RS.LineCode 
								AND TBL.ItemCheckCode = RS.ItemCheckCode AND TBL.ShiftCodeHeader = RS.ShiftCode AND TBL.SequenceNo = RS.SequenceNo
								AND RS.ProdDate = FORMAT(@ProdDate - 1, 'dd MMM yy') AND RS.CompleteStatus = 1 )
				UNION ALL

				select ('Edit||ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
						TBL.ItemCheckCode +'&ProdDate='+ TBL.Date +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1)) ) As Edit,
						('ProdSampleInput.aspx?FactoryCode='+ TBL.FactoryCode +'&ItemTypeCode='+ TBL.ItemTypeCode +'&Line='+ TBL.LineCode +'&ItemCheckCode='+ 
						TBL.ItemCheckCode +'&ProdDate='+ FORMAT(@ProdDate,'yyyy-MM-dd') +'&Shift='+ TBL.ShiftCodeHeader  +'&Sequence='+ CAST(TBL.SequenceNo AS nvarchar(1))) As Link,
						* from (
								select ICT.FactoryCode, 
												MsFtr.FactoryName,
												ItemTypeName = MSI.Description, 
												LineName = MSL.LineCode + ' - ' + MSL.LineName, 
												--LineDesc = MSL.LineName,
												ItemCheck = ICT.ItemCheckCode + ' - ' + ICM.ItemCheck, 
												--ItemCheckDesc = ICM.ItemCheck,
												Date = FORMAT(@ProdDate, 'dd MMM yy'), 
												LinkDate = FORMAT(@ProdDate, 'yyyy-MM-dd'),
												CASE
													WHEN MSF.ShiftCode = 'SH001' THEN '1'
													WHEN MSF.ShiftCode = 'SH002' THEN '2'
													ELSE
													MSF.ShiftCode
												END ShiftCode,
												MSF.ShiftCode AS ShiftCodeHeader,
												MSF.SequenceNo, 
												convert(varchar, MSF.StartTime, 8) as StartTime, convert(varchar, MSF.EndTime, 8) as EndTime, 
												Delay = DATEDIFF(MINUTE, convert(varchar, MSF.EndTime, 8), convert(varchar, @ProdDate, 8)),
													ICT.ItemTypeCode, ICT.LineCode, ICT.ItemCheckCode, ICT.FrequencyCode --,UpdateDate = COALESCE(RS.UpdateDate, RS.RegisterDate)
												, UpdateTime = FORMAT(@ProdDate, 'yyyy-MM-dd') + ' ' + convert(varchar, MSF.EndTime, 8)
												from spc_ItemCheckByType ICT
												INNER JOIN spc_MS_Frequency MSF ON ICT.FrequencyCode = MSF.FrequencyCode	
												INNER JOIN MS_ItemType MSI ON ICT.ItemTypeCode = MSI.ItemTypeCode
												INNER JOIN MS_Line MSL ON ICT.LineCode = MSL.LineCode
												INNER JOIN spc_ItemCheckMaster ICM ON ICT.ItemCheckCode = ICM.ItemCheckCode
												LEFT JOIN MS_Factory MsFtr ON ICT.FactoryCode = MsFtr.FactoryCode
												--INNER JOIN spc_Result RS ON ICT.FactoryCode = RS.FactoryCode AND ICT.ItemTypeCode = RS.ItemTypeCode AND ICT.LineCode = RS.LineCode AND ICT.ItemCheckCode = RS.ItemCheckCode
												WHERE ICT.ActiveStatus = 1
								) tbl
								WHERE NOT EXISTS ( select * from spc_Result RS WHERE TBL.FactoryCode = RS.FactoryCode AND TBL.ItemTypeCode = RS.ItemTypeCode AND TBL.LineCode = RS.LineCode 
													AND TBL.ItemCheckCode = RS.ItemCheckCode AND TBL.ShiftCodeHeader = RS.ShiftCode AND TBL.SequenceNo = RS.SequenceNo --AND TBL.DATE = RS.ProdDate
													AND RS.ProdDate = FORMAT(@ProdDate, 'dd MMM yy') AND RS.CompleteStatus = 1 )
													AND EndTime < convert(varchar, @ProdDate, 8)
								AND SequenceNo <> 5
				) TBL2 

			WHERE EXISTS (

					SELECT * FROM Daily_production DP WHERE TBL2.FactoryCode = DP.Factory_code AND TBL2.LineCode = DP.Line_Code 
					AND TBL2.ShiftCodeHeader = DP.Shift AND TBL2.ItemTypeName =  DP.Item_code AND TBL2.Date = FORMAT(DP.Schedule_Date, 'dd MMM yy')
				)
			AND FactoryCode = @FactoryCode
			ORDER BY Delay DESC, ItemTypeName ASC, LineName ASC 
	END

END
SET NOCOUNT OFF
