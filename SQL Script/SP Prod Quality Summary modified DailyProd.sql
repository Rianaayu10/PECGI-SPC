/****** Object:  StoredProcedure [dbo].[sp_SPC_ProdQualitySummary_Sel]    Script Date: 12/16/2022 01:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_ProdQualitySummary_Sel]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_ProdQualitySummary_Sel] AS' 
END
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-19>
-- Description:	<Production Sample Control Quality Summary>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_ProdQualitySummary_Sel]
	@Factory		As Varchar(MAX) = 'F001',
	@ItemType		As Varchar(MAX) = 'ALL',
	@LineCode		As Varchar(MAX) = 'ALL',
	@Frequency		As Varchar(MAX) = 'ALL',
	@Sequence		As Varchar(MAX) = 'ALL',
	@Period			As Varchar(10)  = '2022-11-01', --yyyy-MM-dd
	@UserID			As Varchar(50)  = 'admin'
As
BEGIN
	DECLARE	@Cols		As Varchar(MAX), 
			@ColsName	As Varchar(MAX), 
			@query		As Varchar(MAX),
			@query2		As Varchar(MAX),
			@dTime		As datetime

	SET @dTime = cast(@Period + ' ' + convert(varchar, GETDATE(), 8) as datetime)
	IF convert(varchar, GETDATE(), 8) >= '00:00' and convert(varchar, GETDATE(), 8) <= '07:00' 
	Begin
		SET @dTime = dateadd(day,1,@dTime)
	End

		SET @Cols = ISNULL(STUFF((SELECT distinct ',' + QUOTENAME(c.ItemCheck) 
					FROM ((
							--select	ItemCheck = a.LineCode + '||' + b.ItemCheck
							select	ItemCheck = a.ItemCheckCode + ' - ' + b.ItemCheck
							from	spc_ItemCheckByType a inner join spc_ItemCheckMaster b on a.ItemCheckCode = b.ItemCheckCode and a.ActiveStatus = 1 and b.ActiveStatus = 1
									inner join (Select L.LineCode from MS_Line L join spc_UserLine U on L.LineCode = U.LineCode and U.UserID = @UserID and AppID = 'SPC' and ISNULL(AllowShow,0) = 1) c on a.LineCode = c.LineCode
							where	a.FactoryCode = @Factory
								and	1 = Case 
											When @Frequency = 'ALL' Then 1
											When @Frequency <> 'ALL' and a.FrequencyCode = @Frequency Then 1
											Else 0
										End
								and	1 = Case 
											When @ItemType = 'ALL' Then 1
											When @ItemType <> 'ALL' and a.ItemTypeCode = @ItemType Then 1
											Else 0
										End
								and 1 = Case 
											When @LineCode = 'ALL' Then 1
											When @LineCode <> 'ALL' and a.LineCode = @LineCode Then 1
											Else 0
										End
								
						 ))c
					FOR XML PATH(''), TYPE
						 ).value('.', 'NVARCHAR(MAX)') 
					,1,1,''),'')

		SET @ColsName = ISNULL(STUFF((SELECT distinct ', ISNULL(' + QUOTENAME(c.ItemCheck) + ', ''NoProd||#515151||1'') ' + QUOTENAME(c.ItemCheck)
					FROM ((
							--select	ItemCheck = a.LineCode + '||' + b.ItemCheck
							select	ItemCheck = a.ItemCheckCode + ' - ' + b.ItemCheck
							from	spc_ItemCheckByType a inner join spc_ItemCheckMaster b on a.ItemCheckCode = b.ItemCheckCode and a.ActiveStatus = 1 and b.ActiveStatus = 1
									inner join (Select L.LineCode from MS_Line L join spc_UserLine U on L.LineCode = U.LineCode and U.UserID = @UserID and AppID = 'SPC' and ISNULL(AllowShow,0) = 1) c on a.LineCode = c.LineCode
							where	a.FactoryCode = @Factory
								and	1 = Case 
											When @Frequency = 'ALL' Then 1
											When @Frequency <> 'ALL' and a.FrequencyCode = @Frequency Then 1
											Else 0
										End
								and	1 = Case 
											When @ItemType = 'ALL' Then 1
											When @ItemType <> 'ALL' and a.ItemTypeCode = @ItemType  Then 1
											Else 0
										End
								and 1 = Case 
											When @LineCode = 'ALL' Then 1
											When @LineCode <> 'ALL' and a.LineCode = @LineCode  Then 1
											Else 0
										End
								
						 ))c
					FOR XML PATH(''), TYPE
						 ).value('.', 'NVARCHAR(MAX)') 
					,1,1,''),'')
		
		IF @Cols = '' or @ColsName = ''
			Begin
				Select * from(Select '' Type) a where Type <> ''
			End

		IF @Cols <> '' and @ColsName <> ''
		Begin

		Set @query = '
		declare @Date as datetime = '''+ Format(@dTime,'yyyy-MM-dd HH:mm:ss') +'''

		select
			[Description] Type,
		   '+@ColsName+'
		from 
		(
			SELECT ItemCheck,Description,STRING_AGG(Result+''||''+cast(Jumlah as varchar(max)),''|,|'') Result 
			FROM 
			(
				Select ItemCheck, Description, Result, Count(Result) Jumlah From
				(
					select	ItemCheck = a.ItemCheckCode + '' - '' + b.ItemCheck, item.Description, --freq.SequenceNo, --a.ItemTypeCode, --d.SPCResultID,
							Result = 
							Case 								
								When daily.ItemTypeCode IS NULL then ''NoProd||#515151'' --Abu-Abu
								When isnull(a.ActiveStatus,''0'') = ''0'' or isnull(b.ActiveStatus,''0'') = ''0'' then ''NoActive||#EF8E19'' --Orange
								When '''+@Period+''' > Format(getdate(),''yyyy-MM-dd'') --then ''NOK''
									then ''NOK||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
										+''&FactoryCode=''+a.FactoryCode+''&ItemTypeCode=''+a.ItemTypeCode+''&ItemCheckCode=''+a.ItemCheckCode --Kuning
										+''&ProdDate='+@Period+'&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
								When Format(cast('''+@Period+''' + '' '' + Format(cast(Freq.EndTime as datetime), ''HH:mm'') as datetime) ,''yyyy-MM-dd HH:mm'') > Format(getdate(),''yyyy-MM-dd HH:mm'') --then ''NOK''
									then ''NOK||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
										+''&FactoryCode=''+a.FactoryCode+''&ItemTypeCode=''+a.ItemTypeCode+''&ItemCheckCode=''+a.ItemCheckCode --Kuning
										+''&ProdDate='+@Period+'&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
								When ISNULL(Minim,-1) = -1 then 
									Case 
										When '''+@Period+''' < Format(getdate(),''yyyy-MM-dd'') --then ''NoResult||#FFFB00'' --Kuning
											then ''NoResult||#FFFB00||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
												+''&FactoryCode=''+a.FactoryCode+''&ItemTypeCode=''+a.ItemTypeCode+''&ItemCheckCode=''+a.ItemCheckCode --Kuning
												+''&ProdDate='+@Period+'&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
										when
											Case 
												--=--When Freq.ShiftCode = ''SH002'' and convert(varchar, freq.EndTime, 8) >= ''00:00'' and convert(varchar, freq.EndTime, 8) <= ''12:00'' 
													When convert(varchar, freq.EndTime, 8) >= ''00:00'' and convert(varchar, freq.EndTime, 8) <= ''07:00'' 
													then convert(varchar, cast(Format(dateadd(day,1,cast('''+@Period+''' as datetime)),''yyyy-MM-dd'') + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
												Else convert(varchar, cast('''+@Period+''' + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
											End > convert(varchar, @Date, 120)
										--Then ''NOK''
										Then ''NOK||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
											+''&FactoryCode=''+a.FactoryCode+''&ItemTypeCode=''+a.ItemTypeCode+''&ItemCheckCode=''+a.ItemCheckCode
											+''&ProdDate='+@Period+'&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
										--Else ''NoResult||#FFFB00'' --Kuning
										Else ''NoResult||#FF0000||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
											+''&FactoryCode=''+a.FactoryCode+''&ItemTypeCode=''+a.ItemTypeCode+''&ItemCheckCode=''+a.ItemCheckCode --Kuning
											+''&ProdDate='+@Period+'&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
									End
								When (Maxim > setup.SpecUSL or Minim < setup.SpecLSL) AND de.nValue >= a.SampleSize
									Then ''OK||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
									+''&FactoryCode=''+c.FactoryCode+''&ItemTypeCode=''+c.ItemTypeCode+''&ItemCheckCode=''+c.ItemCheckCode --OK
									+''&ProdDate=''+cast(c.ProdDate as varchar(MAX))+''&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
								When Maxim > setup.SpecUSL or Minim < setup.SpecLSL 
									Then ''NG||#FF0000||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
									+''&FactoryCode=''+c.FactoryCode+''&ItemTypeCode=''+c.ItemTypeCode+''&ItemCheckCode=''+c.ItemCheckCode --Merah
									+''&ProdDate=''+cast(c.ProdDate as varchar(MAX))+''&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
								--When (Maxim > setup.XBarUCL or Minim < setup.XBarLCL) and ISNULL(CharacteristicStatus,''0'') = 1 --<> 2
								--	Then ''NG||#FFFB00||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
								--	+''&FactoryCode=''+c.FactoryCode+''&ItemTypeCode=''+c.ItemTypeCode+''&ItemCheckCode=''+c.ItemCheckCode --Kuning
								--	+''&ProdDate=''+cast(c.ProdDate as varchar(MAX))+''&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
								--When (Maxim > setup.XBarUCL or Minim < setup.XBarLCL) and ISNULL(CharacteristicStatus,''0'') <> 1 --= 2
								--	--Then ''NG||#FFC0CB||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
								--	Then ''OK||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
								--	+''&FactoryCode=''+c.FactoryCode+''&ItemTypeCode=''+c.ItemTypeCode+''&ItemCheckCode=''+c.ItemCheckCode --Pink||Jadikan OK
								--	+''&ProdDate=''+cast(c.ProdDate as varchar(MAX))+''&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
								Else ''OK||ProdSampleQCSummaryDetail.aspx?menu=ProdIDSummary''
									+''&FactoryCode=''+c.FactoryCode+''&ItemTypeCode=''+c.ItemTypeCode+''&ItemCheckCode=''+c.ItemCheckCode --OK
									+''&ProdDate=''+cast(c.ProdDate as varchar(MAX))+''&Frequency='+@Frequency+'&Sequence='+@Sequence+''+'&Line='+@LineCode+'''
							End
					from	spc_ItemCheckByType a inner join spc_ItemCheckMaster b on a.ItemCheckCode = b.ItemCheckCode and a.ActiveStatus = 1 and b.ActiveStatus = 1
							inner join (Select L.LineCode from MS_Line L join spc_UserLine U on L.LineCode = U.LineCode and U.UserID = '''+@UserID+''' and AppID = ''SPC'' and ISNULL(AllowShow,0) = 1) Line on a.LineCode = Line.LineCode
							left join MS_ItemType item on a.ItemTypeCode = item.ItemTypeCode
							left Join spc_MS_Frequency Freq on a.FrequencyCode = Freq.FrequencyCode
								and 1 = case 
											when '''+@Sequence+''' <> ''ALL'' and Freq.SequenceNo = '''+@Sequence+''' then 1
											when '''+@Sequence+''' = ''ALL'' then 1
											else 0
										End
							left join spc_Result c on a.FactoryCode = c.FactoryCode and a.ItemTypeCode = c.ItemTypeCode and a.LineCode = c.LineCode and a.ItemCheckCode = c.ItemCheckCode and CompleteStatus = 1 and Freq.SequenceNo = c.SequenceNo and Freq.ShiftCode = c.ShiftCode and ProdDate = '''+@Period+'''
								and 1 = case 
											when '''+@Sequence+''' <> ''ALL'' and c.SequenceNo = '''+@Sequence+''' then 1
											when '''+@Sequence+''' = ''ALL'' then 1
											else 0
										End
							left join 
							(
								Select 
									SPCResultID, 
									Minim = min([value]), Maxim = max([value]) 
								From  spc_ResultDetail
								Where DeleteStatus = ''0''
								group by SPCResultID
							) d on c.SPCResultID = d.SPCResultID
							left join 
							(
								 SELECT 
									A.SPCResultID,
									COUNT(C.SPCResultID) AS nValue
								 FROM spc_Result A
								 JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode 
									AND A.ItemTypeCode = B.ItemTypeCode
									AND A.LineCode = B.LineCode AND A.ItemCheckCode = B.ItemCheckCode 
									AND (A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
								 JOIN spc_ResultDetail C ON A.SPCResultID = C.SPCResultID 
									AND ISNULL(C.DeleteStatus,''0'') <> ''1''
								 WHERE C.VALUE <= B.SpecUSL AND C.Value >= B.SpecLSL
								 GROUP BY A.SPCResultID
							) de on c.SPCResultID = de.SPCResultID
							left join spc_ChartSetup setup on a.FactoryCode = setup.FactoryCode and a.ItemTypeCode = setup.ItemTypeCode and a.LineCode = setup.LineCode and a.ItemCheckCode = setup.ItemCheckCode and c.ProdDate between StartDate and EndDate
							Left Join 
							(
								select	distinct Factory_code, Line_Code, c.ItemTypeCode, Schedule_Date, Shift 
								from	Daily_Production a inner join
										MS_Item b on a.Item_code = b.Item_Code inner join
										MS_ItemType c on b.Item_Type = c.ItemTypeCode
							)'							
		SET @query2 ='
							daily on a.FactoryCode = daily.Factory_code and a.LineCode = daily.Line_Code and a.ItemTypeCode = daily.ItemTypeCode and Format(daily.Schedule_Date,''yyyy-MM-dd'') = '''+@Period+''' and daily.Shift = freq.ShiftCode
				where	a.FactoryCode = '''+@Factory+''' '
		IF @Period = Format(GETDATE(),'yyyy-MM-dd')
		Begin
		SET @query2 = @query2 + '
						--and Case 
						--		When Freq.ShiftCode = ''SH002'' and convert(varchar, freq.EndTime, 8) >= ''00:00'' and convert(varchar, freq.EndTime, 8) <= ''12:00'' 
						--			then convert(varchar, cast(Format(dateadd(day,1,cast('''+@Period+''' as datetime)),''yyyy-MM-dd'') + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
						--		Else convert(varchar, cast('''+@Period+''' + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
						--	End < convert(varchar, @Date, 120)
						and 
						(
							Case 
								When convert(varchar, freq.EndTime, 8) >= ''00:00'' and convert(varchar, freq.EndTime, 8) <= ''07:00'' 
									then convert(varchar, cast(Format(dateadd(day,1,cast('''+@Period+''' as datetime)),''yyyy-MM-dd'') + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
								Else convert(varchar, cast('''+@Period+''' + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
							End < convert(varchar, @Date, 120)
						or
							Case 
								When convert(varchar, freq.EndTime, 8) >= ''00:00'' and convert(varchar, freq.EndTime, 8) <= ''17:00'' 
									then convert(varchar, cast(Format(dateadd(day,1,cast('''+@Period+''' as datetime)),''yyyy-MM-dd'') + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
								Else convert(varchar, cast('''+@Period+''' + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
							End > convert(varchar, @Date, 120)
						)'
		End
						
		SET @query2 = @query2 + '
						and	1 = Case 
									When '''+@Frequency+''' = ''ALL'' Then 1
									When '''+@Frequency+''' <> ''ALL'' and a.FrequencyCode = '''+@Frequency+''' Then 1
									Else 0
								End
						and	1 = Case 
									When '''+@ItemType+''' = ''ALL'' Then 1
									When '''+@ItemType+''' <> ''ALL'' and a.ItemTypeCode = '''+@ItemType+''' Then 1
									Else 0
								End
						and 1 = Case 
									When '''+@LineCode+''' = ''ALL'' Then 1
									When '''+@LineCode+''' <> ''ALL'' and a.LineCode = '''+@LineCode+''' Then 1
									Else 0
						End --order by a.ItemCheckCode
				) a Group by ItemCheck, Description, a.Result
			)qtbl group by ItemCheck,Description
		) src
		pivot
		(
		  max(Result)
		  for ItemCheck in ('+@Cols+')
		) piv;
		'
		print @query
		print @query2
		
		exec (@query+@query2)

		End

		--Untuk Get Info Legend : Sample Time
		Set @query = '
		select	distinct Timing = Format(cast(Freq.StartTime as Datetime), ''HH:mm'') + '' - '' + Format(cast(Freq.EndTime as Datetime), ''HH:mm'')
		from	spc_MS_Frequency Freq
		where 1 = Case 
					When '''+@Frequency+''' = ''ALL'' Then 1
					When '''+@Frequency+''' <> ''ALL'' and Freq.FrequencyCode = '''+@Frequency+''' Then 1
					Else 0
				End
			and 1 = case 
					when '''+@Sequence+''' <> ''ALL'' and Freq.SequenceNo = '''+@Sequence+''' then 1
					when '''+@Sequence+''' = ''ALL'' then 1
					else 0
				End
		'
		exec(@query)

		--Untuk Get Info Legend
		exec sp_SPC_ProdQualitySummary_Sel_Legend @Factory, @ItemType, @LineCode, @Frequency, @Sequence, @Period, @UserID
		
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_ProdQualitySummary_Sel_Detail]    Script Date: 12/16/2022 01:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_ProdQualitySummary_Sel_Detail]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_ProdQualitySummary_Sel_Detail] AS' 
END
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-19>
-- Description:	<Production Sample Control Quality Summary>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_ProdQualitySummary_Sel_Detail]
	@Factory		As Varchar(MAX) = 'F001',
	@ItemType		As Varchar(MAX) = 'TPMSBR011',
	@ItemCheck		As Varchar(MAX) = 'IC009',
	@LineCode		As Varchar(MAX) = 'ALL',
	@Frequency		As Varchar(MAX) = 'ALL',
	@Sequence		As Varchar(MAX) = 'ALL',
	@Period			As Varchar(10)  = '2022-11-01', --yyyy-MM-dd
	@UserID			As Varchar(50)  = 'admin'
As
BEGIN
	declare	@dTime	As datetime	
	SET @dTime = cast(@Period + ' ' + convert(varchar, GETDATE(), 8) as datetime)
	IF convert(varchar, GETDATE(), 8) >= '00:00' and convert(varchar, GETDATE(), 8) <= '07:00' 
	Begin
		SET @dTime = dateadd(day,1,@dTime)
	End

	Select 
		 Link
		,TextLink 
		,Factory = tbl.Factory -- + tbl.color
		,Type = tbl.Type --+ tbl.color
		,Line = tbl.Line --+ tbl.color
		,ItemCheck = tbl.ItemCheck --+ tbl.color
		,Prod =  Format(tbl.Prod,'dd MMM yyyy') --+ tbl.color
		,Shift =  tbl.Shift --+ tbl.color
		,Seq =  cast(tbl.Seq as varchar(max)) --+ tbl.color 
		,Result
	From (
		select
			 Link = 'ProdSampleInput.aspx?FactoryCode='+a.FactoryCode+'&ItemTypeCode='+a.ItemTypeCode+'&Line='+a.LineCode+'&ItemCheckCode='+a.ItemCheckCode+'&ProdDate='+@Period+'&Shift='+Freq.ShiftCode+'&Sequence='+cast(freq.SequenceNo as varchar)+''
			,TextLink = 'View'
			,Factory = e.FactoryName
			,Type = item.Description
			,Line = a.LineCode + ' - ' + f.LineName
			,ItemCheck = a.ItemCheckCode + ' - ' + b.ItemCheck
			,Prod = Cast(@Period as date)
			,Shift = ISNULL(c.ShiftCode,Freq.ShiftCode)
			,Seq = Freq.SequenceNo
			,Result = 
				Case
					When daily.ItemTypeCode IS NULL then 'NoProd||#515151' --Abu-Abu
					When isnull(a.ActiveStatus,'0') = '0' or isnull(b.ActiveStatus,'0') = '0' then 'NoActive||#EF8E19' --Orange
					When @Period > Format(getdate(),'yyyy-MM-dd') then 'NOK||#FFFFFF'
					When Format(cast(@Period + ' ' + Format(cast(Freq.EndTime as datetime), 'HH:mm') as datetime) ,'yyyy-MM-dd HH:mm') > Format(getdate(),'yyyy-MM-dd HH:mm') then 'NOK||#FFFFFF'
					When ISNULL(Minim,-1) = -1 then 
						Case 
							When @Period < Format(getdate(),'yyyy-MM-dd') then 'NoResult||#FFFB00' --Kuning
							when
								Case 
									--When Freq.ShiftCode = 'SH002' and convert(varchar, freq.EndTime, 8) >= '00:00' and convert(varchar, freq.EndTime, 8) <= '12:00' 
									--When convert(varchar, freq.EndTime, 8) >= '00:00' and convert(varchar, freq.EndTime, 8) <= '07:00' 
									When convert(varchar, Freq.EndTime, 8) < (select Top 1 convert(varchar, StartTime, 8) from spc_MS_Frequency where FrequencyCode = Freq.FrequencyCode Order By Freq.SequenceNo)
										then convert(varchar, cast(Format(dateadd(day,1,cast(@Period as datetime)),'yyyy-MM-dd') + ' ' + convert(varchar, freq.EndTime, 8) as datetime), 120)
									Else convert(varchar, cast(@Period + ' ' + convert(varchar, freq.EndTime, 8) as datetime), 120)
								End > convert(varchar, @dTime, 120)
							Then 'NOK||#FFFFFF'
							Else 'NoResult||#FFFB00' --Kuning
						End
					When (Maxim > setup.SpecUSL or Minim < setup.SpecLSL) AND de.nValue >= a.SampleSize THEN 
						'OK'
					When Maxim > setup.SpecUSL or Minim < setup.SpecLSL 
						Then 'NG||#FF0000'
					--When (Maxim > setup.XBarUCL or Minim < setup.XBarLCL) and ISNULL(CharacteristicStatus,'0') =  1 
					--	Then 'NG||#FFFB00'
					--When (Maxim > setup.XBarUCL or Minim < setup.XBarLCL) and ISNULL(CharacteristicStatus,'0') <> 1 
					--	Then 'OK'--'NG||#FFC0CB'
					Else 'OK'
				End, c.SPCResultID, de.nValue, a.SampleSize
		from	spc_ItemCheckByType a inner join spc_ItemCheckMaster b on a.ItemCheckCode = b.ItemCheckCode --and a.ActiveStatus = 1 and b.ActiveStatus = 1
				inner join (Select L.LineCode from MS_Line L join spc_UserLine U on L.LineCode = U.LineCode and U.UserID = @UserID and AppID = 'SPC' and ISNULL(AllowShow,0) = 1) Line on a.LineCode = Line.LineCode
				left join MS_ItemType item on a.ItemTypeCode = item.ItemTypeCode
				left Join spc_MS_Frequency Freq on a.FrequencyCode = Freq.FrequencyCode
				left join spc_Result c on a.FactoryCode = c.FactoryCode and a.ItemTypeCode = c.ItemTypeCode and a.LineCode = c.LineCode and a.ItemCheckCode = c.ItemCheckCode and CompleteStatus = 1 and Freq.SequenceNo = c.SequenceNo and Freq.ShiftCode = c.ShiftCode and ProdDate = @Period
				left join 
				(
					Select 
						SPCResultID, 
						Minim = min([value]), Maxim = max([value]) 
					From  spc_ResultDetail
					Where DeleteStatus = '0'
					group by SPCResultID
				) d on c.SPCResultID = d.SPCResultID
				left join 
				(
					 SELECT 
						A.SPCResultID,
						COUNT(C.SPCResultID) AS nValue
					 FROM spc_Result A
					 JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode 
						AND A.ItemTypeCode = B.ItemTypeCode
						AND A.LineCode = B.LineCode AND A.ItemCheckCode = B.ItemCheckCode 
						AND (A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
					 JOIN spc_ResultDetail C ON A.SPCResultID = C.SPCResultID 
						AND ISNULL(C.DeleteStatus,'0') <> '1'
					 WHERE C.VALUE <= B.SpecUSL AND C.Value >= B.SpecLSL
					 GROUP BY A.SPCResultID
				) de on c.SPCResultID = de.SPCResultID
				left join spc_ChartSetup setup on a.FactoryCode = setup.FactoryCode and a.ItemTypeCode = setup.ItemTypeCode and a.LineCode = setup.LineCode and a.ItemCheckCode = setup.ItemCheckCode and c.ProdDate between StartDate and EndDate
				Left Join 
				(
					select	distinct Factory_code, Line_Code, c.ItemTypeCode, Schedule_Date, Shift 
					from	Daily_Production a inner join
							MS_Item b on a.Item_code = b.Item_Code inner join
							MS_ItemType c on b.Item_Type = c.ItemTypeCode
				)
				daily on a.FactoryCode = daily.Factory_code and a.LineCode = daily.Line_Code and a.ItemTypeCode = daily.ItemTypeCode and Format(daily.Schedule_Date,'yyyy-MM-dd') = @Period and daily.Shift = ISNULL(c.ShiftCode,Freq.ShiftCode)
				left join MS_Factory e on a.FactoryCode = e.FactoryCode
				left join MS_Line f on a.FactoryCode = f.FactoryCode and a.LineCode = f.LineCode
		where	a.FactoryCode = @Factory
			and	a.ItemTypeCode = @ItemType
			and	a.ItemCheckCode = @ItemCheck
			and 1 = Case
						When @LineCode = 'ALL' Then 1
						When @LineCode <> 'ALL' and c.LineCode = @LineCode Then 1
						Else 0
					End
			and 1 = Case
						When @Frequency = 'ALL' Then 1
						When @Frequency <> 'ALL' and a.FrequencyCode = @Frequency Then 1
						Else 0
					End
			and 1 = Case
						When @Sequence = 'ALL' Then 1
						When @Sequence <> 'ALL' and c.SequenceNo = @Sequence Then 1
						Else 0
					End
	) tbl
	Where Result not like '%NOK%' and Result not like '%NoProd%'
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_ProdQualitySummary_Sel_Legend]    Script Date: 12/16/2022 01:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_ProdQualitySummary_Sel_Legend]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_ProdQualitySummary_Sel_Legend] AS' 
END
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-19>
-- Description:	<Production Sample Control Quality Summary>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_ProdQualitySummary_Sel_Legend]
	@Factory		As Varchar(MAX) = 'F001',
	@ItemType		As Varchar(MAX) = 'ALL',
	@LineCode		As Varchar(MAX) = 'ALL',
	@Frequency		As Varchar(MAX) = 'ALL',
	@Sequence		As Varchar(MAX) = 'ALL',
	@Period			As Varchar(10)  = '2022-07-04', --yyyy-MM-dd
	@UserID			As Varchar(50)  = 'admin'
As
BEGIN
	DECLARE	@query		As Varchar(MAX),
			@query2		As Varchar(MAX),
			@dTime		As datetime

	SET @dTime = cast(@Period + ' ' + convert(varchar, GETDATE(), 8) as datetime)
	IF convert(varchar, GETDATE(), 8) >= '00:00' and convert(varchar, GETDATE(), 8) <= '07:00' 
	Begin
		SET @dTime = dateadd(day,1,@dTime)
	End

	Set @query = '
		declare @Date as datetime = '''+ Format(@dTime,'yyyy-MM-dd HH:mm:ss') +'''

		Select Result, SUM(Jumlah) Jumlah From 
		(
			Select Result, Count(Result) Jumlah From
				(
					select	ItemCheck = a.ItemCheckCode + '' - '' + b.ItemCheck, item.Description, --a.ItemTypeCode, --d.SPCResultID,
						Result = 
							Case 								
								When daily.ItemTypeCode IS NULL then ''NoProd||#515151'' --Abu-Abu
								When isnull(a.ActiveStatus,''0'') = ''0'' or isnull(b.ActiveStatus,''0'') = ''0'' then ''NoActive||#EF8E19'' --Orange
								When '''+@Period+''' > Format(getdate(),''yyyy-MM-dd'') then ''NIK''
								When Format(cast('''+@Period+''' + '' '' + Format(cast(Freq.EndTime as datetime), ''HH:mm'') as datetime) ,''yyyy-MM-dd HH:mm'') > Format(getdate(),''yyyy-MM-dd HH:mm'') then ''NIK''
								When ISNULL(Minim,-1) = -1 then 
									Case 
										When '''+@Period+''' < Format(getdate(),''yyyy-MM-dd'') then ''Delay||#FFFB00'' --Kuning
										when
											Case 
												--When Freq.ShiftCode = ''SH002'' and convert(varchar, freq.EndTime, 8) >= ''00:00'' and convert(varchar, freq.EndTime, 8) <= ''12:00'' 
												When convert(varchar, Freq.EndTime, 8) < (select Top 1 convert(varchar, StartTime, 8) from spc_MS_Frequency where FrequencyCode = Freq.FrequencyCode Order By Freq.SequenceNo)
													then convert(varchar, cast(Format(dateadd(day,1,cast('''+@Period+''' as datetime)),''yyyy-MM-dd'') + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
												Else convert(varchar, cast('''+@Period+''' + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
											End > convert(varchar, @Date, 120)
										Then ''NIK''
										Else ''Delay||#FFFB00'' --Kuning
									End
								When (Maxim > setup.SpecUSL or Minim < setup.SpecLSL) AND de.nValue >= a.SampleSize
									then ''OK''
								When Maxim > setup.SpecUSL or Minim < setup.SpecLSL
									Then ''NG''
								--When (Maxim > setup.XBarUCL or Minim < setup.XBarLCL) And ISNULL(CharacteristicStatus,''0'') =  1
								--	Then ''NG''
								--When (Maxim > setup.XBarUCL or Minim < setup.XBarLCL) And ISNULL(CharacteristicStatus,''0'') <>  1
								--	Then ''OK''
								Else ''OK''
							End
					from	spc_ItemCheckByType a inner join spc_ItemCheckMaster b on a.ItemCheckCode = b.ItemCheckCode and a.ActiveStatus = 1 and b.ActiveStatus = 1
							inner join (Select L.LineCode from MS_Line L join spc_UserLine U on L.LineCode = U.LineCode and U.UserID = '''+@UserID+''' and AppID = ''SPC'' and ISNULL(AllowShow,0) = 1) Line on a.LineCode = Line.LineCode
							left join MS_ItemType item on a.ItemTypeCode = item.ItemTypeCode
							left Join spc_MS_Frequency Freq on a.FrequencyCode = Freq.FrequencyCode
								and 1 = case 
											when '''+@Sequence+''' <> ''ALL'' and Freq.SequenceNo = '''+@Sequence+''' then 1
											when '''+@Sequence+''' = ''ALL'' then 1
											else 0
										End
							left join spc_Result c on a.FactoryCode = c.FactoryCode and a.ItemTypeCode = c.ItemTypeCode and a.LineCode = c.LineCode and a.ItemCheckCode = c.ItemCheckCode and CompleteStatus = 1 and Freq.SequenceNo = c.SequenceNo and Freq.ShiftCode = c.ShiftCode and ProdDate = '''+@Period+'''
								and 1 = case 
											when '''+@Sequence+''' <> ''ALL'' and c.SequenceNo = '''+@Sequence+''' then 1
											when '''+@Sequence+''' = ''ALL'' then 1
											else 0
										End
							left join 
							(
								Select 
									SPCResultID, 
									Minim = min([value]), Maxim = max([value]) 
								From  spc_ResultDetail
								Where DeleteStatus = ''0''
								group by SPCResultID
							) d on c.SPCResultID = d.SPCResultID
							left join 
							(
								 SELECT 
									A.SPCResultID,
									COUNT(C.SPCResultID) AS nValue
								 FROM spc_Result A
								 JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode 
									AND A.ItemTypeCode = B.ItemTypeCode
									AND A.LineCode = B.LineCode AND A.ItemCheckCode = B.ItemCheckCode 
									AND (A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
								 JOIN spc_ResultDetail C ON A.SPCResultID = C.SPCResultID 
									AND ISNULL(C.DeleteStatus,''0'') <> ''1''
								 WHERE C.VALUE <= B.SpecUSL AND C.Value >= B.SpecLSL
								 GROUP BY A.SPCResultID
							) de on c.SPCResultID = de.SPCResultID
							left join spc_ChartSetup setup on a.FactoryCode = setup.FactoryCode and a.ItemTypeCode = setup.ItemTypeCode and a.LineCode = setup.LineCode and a.ItemCheckCode = setup.ItemCheckCode and c.ProdDate between StartDate and EndDate
							Left Join 
							(
								select	distinct Factory_code, Line_Code, c.ItemTypeCode, Schedule_Date, Shift 
								from	Daily_Production a inner join
										MS_Item b on a.Item_code = b.Item_Code inner join
										MS_ItemType c on b.Item_Type = c.ItemTypeCode
							)
							daily on a.FactoryCode = daily.Factory_code and a.LineCode = daily.Line_Code and a.ItemTypeCode = daily.ItemTypeCode and Format(daily.Schedule_Date,''yyyy-MM-dd'') = '''+@Period+''' and daily.Shift = freq.ShiftCode'
		SET @query2 ='
					where	a.FactoryCode = '''+@Factory+''' '
		IF @Period = Format(GETDATE(),'yyyy-MM-dd')
		Begin
		SET @query2 = @query2 + '
						and 
						(
							Case 
								--When Freq.ShiftCode = ''SH002'' and convert(varchar, freq.EndTime, 8) >= ''00:00'' and convert(varchar, freq.EndTime, 8) <= ''12:00'' 
								When convert(varchar, Freq.EndTime, 8) < (select Top 1 convert(varchar, StartTime, 8) from spc_MS_Frequency where FrequencyCode = Freq.FrequencyCode Order By Freq.SequenceNo)
									then convert(varchar, cast(Format(dateadd(day,1,cast('''+@Period+''' as datetime)),''yyyy-MM-dd'') + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
								Else convert(varchar, cast('''+@Period+''' + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
							End < convert(varchar, @Date, 120)
						or
							Case 
								--When Freq.ShiftCode = ''SH002'' and convert(varchar, freq.EndTime, 8) >= ''00:00'' and convert(varchar, freq.EndTime, 8) <= ''12:00'' 
								When convert(varchar, Freq.EndTime, 8) < (select Top 1 convert(varchar, StartTime, 8) from spc_MS_Frequency where FrequencyCode = Freq.FrequencyCode Order By Freq.SequenceNo)
									then convert(varchar, cast(Format(dateadd(day,1,cast('''+@Period+''' as datetime)),''yyyy-MM-dd'') + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
								Else convert(varchar, cast('''+@Period+''' + '' '' + convert(varchar, freq.EndTime, 8) as datetime), 120)
							End > convert(varchar, @Date, 120)
						)'
		End
						
		SET @query2 = @query2 + '
						and	1 = Case 
									When '''+@Frequency+''' = ''ALL'' Then 1
									When '''+@Frequency+''' <> ''ALL'' and a.FrequencyCode = '''+@Frequency+''' Then 1
									Else 0
								End
						and	1 = Case 
									When '''+@ItemType+''' = ''ALL'' Then 1
									When '''+@ItemType+''' <> ''ALL'' and a.ItemTypeCode = '''+@ItemType+''' Then 1
									Else 0
								End
						and 1 = Case 
									When '''+@LineCode+''' = ''ALL'' Then 1
									When '''+@LineCode+''' <> ''ALL'' and a.LineCode = '''+@LineCode+''' Then 1
									Else 0
						End
			) a Group by ItemCheck, Description, a.Result
		)ab group by Result'
		print @query
		print @query2
		
		exec (@query+@query2)
		
END
GO
