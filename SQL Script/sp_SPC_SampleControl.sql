/****** Object:  StoredProcedure [dbo].[sp_SPC_SampleControl]    Script Date: 4/7/2023 4:12:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ari
-- Create date: 08-Sep-2022
-- Description:	
-- =============================================
ALTER   PROCEDURE [dbo].[sp_SPC_SampleControl]
	@FactoryCode varchar(15) = 'F001',
	@ItemTypeCode varchar(15) = 'TPMSBR011',
	@Line varchar(15) = '015',
	@ItemCheckCode varchar(15) = 'IC021',
	@ProdDate date = '2023-03-31',
	@ProdDate2 date = '2023-03-31',
	@VerifiedOnly int = 0,
	@ShowVerifier int = 1,
	@CompleteStatus int = 1
AS
BEGIN
	declare @Digit varchar(2) = (select top 1 DecimalDigit from spc_ItemCheckMaster where ItemCheckCode = @ItemCheckCode)
	if @Digit is Null set @Digit = '3'

	declare @CharacteristicStatus char(1) = (
		select top 1 CharacteristicStatus from spc_ItemCheckByType 
		where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @Line and ItemCheckCode = @ItemCheckCode
	)
	declare @NoJudgement varchar(50) = ''
	if @CharacteristicStatus <> '1' and @CharacteristicStatus <> '2'
		set @NoJudgement = '(No Judgement)'		

	if @ProdDate is Null
	set @ProdDate = (
		select max(ProdDate) from spc_Result R inner join spc_ResultDetail D on R.SPCResultID = D.SPCResultID
		where R.FactoryCode = @FactoryCode and R.ItemTypeCode = @ItemTypeCode and R.LineCode = @Line and R.ItemCheckCode = @ItemCheckCode and R.ProdDate < @ProdDate2 
		and D.DeleteStatus = 0
		and isnull(R.CompleteStatus, 0) = case when @CompleteStatus = 1 then 1 else isnull(R.CompleteStatus, 0) end
	)	

	declare @que nvarchar(max)

	select R.ProdDate, right(R.ShiftCode, 1) ShiftCode, R.SequenceNo SeqNo, R.ShiftCode ShiftCode2,
	convert(char(6), R.ProdDate, 12) + '_' + right(R.ShiftCode, 1) + '_' + cast(R.SequenceNo as varchar(2)) ColName, 
	convert(char(5), R.RegisterDate, 114) RegisterDate, R.SubLotNo, R.Remark,
	U.FullName Operator, 
	UMK.FullName MKVerificationUser, 
	UQC.FullName QCVerificationUser,
	D.SequenceNo, D.Value, S.SpecLSL, S.SpecUSL, S.CPLCL, S.CPCL, S.CPUCL, S.RUCL, S.RLCL, S.XBarLCL, S.XBarUCL,
	isnull(D.DeleteStatus, '0') DeleteStatus
	into #tempInput
	from spc_Result R inner join uf_SPCResult_Detail(@VerifiedOnly) D on R.SPCResultID = D.SPCResultID
	left join spc_ChartSetup S on R.FactoryCode = S.FactoryCode and R.ItemTypeCode = S.ItemTypeCode and R.LineCode = S.LineCode and R.ItemCheckCode = S.ItemCheckCode
	and R.ProdDate between S.StartDate and S.EndDate
	left join spc_UserSetup U on R.RegisterUser = U.UserID
	left join spc_UserSetup UMK on R.MKVerificationUser = UMK.UserID
	left join spc_UserSetup UQC on R.QCVerificationUser = UQC.UserID
	where R.ProdDate between @ProdDate and @ProdDate2
	and R.FactoryCode = @FactoryCode and R.ItemTypeCode = @ItemTypeCode and R.LineCode = @Line and R.ItemCheckCode = @ItemCheckCode
	and isnull(R.CompleteStatus, 0) = case when @CompleteStatus = 1 then 1 else isnull(R.CompleteStatus, 0) end

	select R.ProdDate, right(R.ShiftCode, 1) ShiftCode, R.SequenceNo SeqNo, R.ShiftCode ShiftCode2,
	convert(char(6), R.ProdDate, 12) + '_' + right(R.ShiftCode, 1) + '_' + cast(R.SequenceNo as varchar(2)) ColName, 
	convert(char(5), R.RegisterDate, 114) RegisterDate, R.SubLotNo, R.Remark,
	U.FullName Operator, 
	case when isnull(R.MKVerificationUser, '') = '-' then '-' else UMK.FullName end MKVerificationUser, 
	case when isnull(R.QCVerificationUser, '') = '-' then '-' else UQC.FullName end QCVerificationUser,
	D.SequenceNo, D.Value, S.SpecLSL, S.SpecUSL, S.CPLCL, S.CPCL, S.CPUCL, S.RUCL, S.RLCL, S.XBarLCL, S.XBarUCL,
	case when V.CountOK >= V.SampleSize or V.RatioNG < V.FTARatio then 'OK' else 'NG' end Judgement
	--case when D.Value is Null then NULL when D.Value between S.SpecLSL and S.SpecUSL then 'OK' else 'NG' end Judgement	
	into #tempXR
	from spc_Result R inner join uf_SPCResult_Detail(@VerifiedOnly) D on R.SPCResultID = D.SPCResultID
	left join vw_SPCResultRatioNG V on R.SPCResultID = V.SPCResultID
	left join spc_ChartSetup S on R.FactoryCode = S.FactoryCode and R.ItemTypeCode = S.ItemTypeCode and R.LineCode = S.LineCode and R.ItemCheckCode = S.ItemCheckCode
	and R.ProdDate between S.StartDate and S.EndDate
	left join spc_UserSetup U on R.RegisterUser = U.UserID
	left join spc_UserSetup UMK on R.MKVerificationUser = UMK.UserID
	left join spc_UserSetup UQC on R.QCVerificationUser = UQC.UserID

	where R.ProdDate between @ProdDate and @ProdDate2
	and R.FactoryCode = @FactoryCode and R.ItemTypeCode = @ItemTypeCode and R.LineCode = @Line and R.ItemCheckCode = @ItemCheckCode
	and isnull(D.DeleteStatus, 0) = 0
	and isnull(R.CompleteStatus, 0) = case when @CompleteStatus = 1 then 1 else isnull(R.CompleteStatus, 0) end

	select A.ProdDate, ShiftCode, ShiftCode2, SeqNo, convert(char(6), A.ProdDate, 12) + '_' + ShiftCode + '_' + cast(SeqNo as varchar(2)) ColName,
	RegisterDate, SubLotNo, Operator, MKVerificationUser, QCVerificationUser
	from #tempXR A 
	group by A.ProdDate, ShiftCode, SeqNo, RegisterDate, SubLotNo, Operator, MKVerificationUser, QCVerificationUser, ShiftCode2

	DECLARE @ListCol nvarchar(max)
	SELECT @ListCol = COALESCE(@ListCol + ',','') + '[' + A.ColName + ']' from (select distinct ColName from #tempXR) A

	if isnull(@ListCol, '') = '' set @ListCol = '[000000_1_1]'

	DECLARE @ListColNull nvarchar(max)
	SELECT @ListColNull = COALESCE(@ListColNull + ',','') + 'NULL [' + A.ColName + ']' from (select distinct ColName from #tempXR) A

	if isnull(@ListColNull, '') = '' set @ListColNull = '[000000_1_1]'

	DECLARE @ListColString nvarchar(max)
	SELECT @ListColString = COALESCE(@ListColString + ',','') + CHAR(10) + 'CAST([' + A.ColName + '] as varchar(20)) [' + A.ColName + ']' from (select distinct ColName from #tempXR) A
	if isnull(@ListColString, '') = '' set @ListColString = '[000000_1_1]'

	DECLARE @ListColNum nvarchar(max)
	SELECT @ListColNum = COALESCE(@ListColNum + ',','') + CHAR(10) + 'CAST(CAST([' + A.ColName + '] as numeric(10,4)) as varchar(20)) [' + A.ColName + ']' from (select distinct ColName from #tempXR) A
	if isnull(@ListColNum, '') = '' set @ListColNum = '[000000_1_1]'
	
	declare @qval nvarchar(max) = 'cast(Value as numeric(18, ' + @Digit + ')) as Value'

set @que = 
		'
		select A.* from (
			select 1 Seq, SequenceNo, cast(SequenceNo as varchar(2)) Des, ' + @ListColString + '
			from (select ColName, SequenceNo, ' + @qval + ' from #tempInput) Tb pivot (sum(Value) for ColName in (' + @ListCol + ')) Pv 
			union 
			select 2 Seq, 0 SequenceNo, ''-'' Des, ' + @ListColNull + '
			from (select ColName, 0 Value from #tempXR) Tb pivot (min(Value) for ColName in (' + @ListCol + ')) Pv 
			union 
			select 3 Seq, 0 SequenceNo, ''Min'' Des, ' + @ListColString + '
			from (select ColName, ' + @qval + ' from #tempXR) Tb pivot (min(Value) for ColName in (' + @ListCol + ')) Pv 
			union
			select 4 Seq, 0 SequenceNo, ''Max'' Des, ' + @ListColString + '
			from (select ColName, ' + @qval + ' from #tempXR) Tb pivot (max(Value) for ColName in (' + @ListCol + ')) Pv 
			union
			select 5 Seq, 0 SequenceNo, ''Xbar ' + @NoJudgement + ''' Des, ' + @ListColNum + '
			from (select ColName, ' + @qval + ' from #tempXR) Tb pivot (avg(Value) for ColName in (' + @ListCol + ')) Pv 
			union
			select 6 Seq, 0 SequenceNo, ''R '  + @NoJudgement +  ''' Des, ' + @ListColString + '
			from (select ColName, cast(max(Value) - min(Value) as numeric(18, ' + @Digit + ')) RValue from #tempXR group by ColName) Tb pivot (min(RValue) for ColName in (' + @ListCol + ')) Pv
			union 
			select 7 Seq, 0 SequenceNo, ''--'' Des, ' + @ListColNull + '
			from (select ColName, 0 Value from #tempXR) Tb pivot (min(Value) for ColName in (' + @ListCol + ')) Pv 
			union
			select 8 Seq, 0 SequenceNo, ''Judgement'' Des, ' + @ListCol + '
			from (select ColName, Judgement from #tempXR) Tb pivot (min(Judgement) for ColName in (' + @ListCol + ')) Pv 
			union
			select 9 Seq, 0 SequenceNo, ''Lot No'' Des, ' + @ListCol + '
			from (select ColName, SubLotNo from #tempXR) Tb pivot (max(SubLotNo) for ColName in (' + @ListCol + ')) Pv 
			union
			select 10 Seq, 0 SequenceNo, ''Remarks'' Des, ' + @ListCol + '
			from (select ColName, Remark from #tempXR) Tb pivot (max(Remark) for ColName in (' + @ListCol + ')) Pv 
			union
			select 11 Seq, 0 SequenceNo, ''Operator'' Des, ' + @ListCol + '
			from (select ColName, Operator from #tempXR) Tb pivot (max(Operator) for ColName in (' + @ListCol + ')) Pv 
			union
			select 12 Seq, 0 SequenceNo, ''MK/GC/SC'' Des, ' + @ListCol + '
			from (select ColName, MKVerificationUser from #tempXR) Tb pivot (max(MKVerificationUser) for ColName in (' + @ListCol + ')) Pv 
			union
			select 13 Seq, 0 SequenceNo, ''QC'' Des, ' + @ListCol + '
			from (select ColName, QCVerificationUser from #tempXR) Tb pivot (max(QCVerificationUser) for ColName in (' + @ListCol + ')) Pv
		) A order by Seq, SequenceNo '		

	exec sp_executesql @que	

	set @que = '
		select 2 seq, ''SpecLSL'' Des, ' + @ListCol + '
		from (select ColName, SpecLSL from #tempXR) Tb pivot (max(SpecLSL) for ColName in (' + @ListCol + ')) Pv'
	exec sp_executesql @que

	set @que = '
		select 3 seq, ''SpecUSL'' Des, ' + @ListCol + '
		from (select ColName, SpecUSL from #tempXR) Tb pivot (max(SpecUSL) for ColName in (' + @ListCol + ')) Pv'
	exec sp_executesql @que

	set @que = 'select 4 seq, ''CPLCL'' Des, ' + @ListCol + '
		from (select ColName, CPLCL from #tempXR) Tb pivot (max(CPLCL) for ColName in (' + @ListCol + ')) Pv'
	exec sp_executesql @que

	set @que = 'select 5 seq, ''CPUCL'' Des, ' + @ListCol + '
		from (select ColName, CPUCL from #tempXR) Tb pivot (max(CPUCL) for ColName in (' + @ListCol + ')) Pv'
	exec sp_executesql @que


	--select '#' + cast(A.SeqNo as varchar(2)) [Description], A.ProdDate, A.ShiftCode, A.RegisterDate,
	--A.RegisterDate + char(10) + 'Shift ' + cast(A.ShiftCode as char(1)) + char(10) + format(A.ProdDate, 'dd MMM yyyy') Seq, A.[Value]
	--,NULL AvgValue, NULL RValue, NULL [RuleValue], SpecLSL, SpecUSL, CPLCL, CPUCL
	--from #tempXR A 

	declare @Min numeric(9, 4) = (select MIN(Value) from #tempXR)
	declare @Max numeric(9, 4) = (select MAX(Value) from #tempXR)
	declare @Avg float = (select AVG(Value) from #tempXR)
	declare @STD float = (select STDEV(Value) from #tempXR)
	declare @SpecLSL float = (select top 1 SpecLSL from #tempXR)
	declare @SpecUSL float = (select top 1 SpecUSL from #tempXR)
	declare @LCL numeric(9, 4) = (select top 1 CPLCL from #tempXR)
	declare @UCL numeric(9, 4) = (select top 1 CPUCL from #tempXR)	
	declare @CL numeric(9, 4) = (select top 1 CPCL from #tempXR)
	declare @XBarLCL numeric(9, 4) = (select top 1 XbarLCL from #tempXR)
	declare @XBarUCL numeric(9, 4) = (select top 1 XbarUCL from #tempXR)
	declare @RUCL numeric(9, 4) = (select top 1 RUCL from #tempXR)	

	declare @nSample as integer = (
		select top 1 I.SampleSize from spc_ItemCheckByType I inner join spc_Result R 
		on I.FactoryCode = R.FactoryCode and I.ItemTypeCode = R.ItemTypeCode and I.LineCode = R.LineCode and I.ItemCheckCode = R.ItemCheckCode		
		where R.FactoryCode = @FactoryCode and R.ItemTypeCode = @ItemTypeCode and R.LineCode = @Line and R.ItemCheckCode = @ItemCheckCode
		and R.ProdDate >= @ProdDate
		order by R.ProdDate
	)

	declare @ItemTypeName varchar(50) = (
		select top 1 Description from MS_ItemType where ItemTypeCode = @ItemTypeCode 
	)
	declare @LineName varchar(50) = (select top 1 LineName from MS_Line where LineCode = @Line and FactoryCode = @FactoryCode)
	declare @ItemCheckName varchar(50) = (select top 1 ItemCheck from spc_ItemCheckMaster where ItemCheckCode = @ItemCheckCode)
	declare @Unit varchar(50) = (select top 1 UnitMeasurement from spc_ItemCheckMaster where ItemCheckCode = @ItemCheckCode)
	declare @FactoryName varchar(50) = (select top 1 FactoryName from MS_Factory where FactoryCode = @FactoryCode)

	declare @d2 numeric(9, 4) = (select top 1 D2 from spc_D2Value where SampleSize = @nSample)

	if @d2 is null
	begin
		if @nSample = 2
			set @d2 = 1.128
		if @nSample = 3
			set @d2 = 1.693
		if @nSample = 4
			set @d2 = 2.059
		if @nSample = 5
			set @d2 = 2.326
		if @nSample = 6
			set @d2 = 2.534
	end
		
	declare @RBar numeric(18, 4) = (select avg(RValue) from (select ColName, max(cast(Value as float)) - min(cast(Value as float)) RValue from #tempXR group by ColName) T)	
	declare @XBarBar numeric(18, 4) = (select Avg(AvgValue) from (select ColName, cast(AVG(Value) as numeric(9, 4)) AvgValue from #tempXR group by ColName) T)	

	
	--set @RBar = 0.009
	--set @XBarBar = 2.694

	declare @CP numeric(9, 4)
	declare @CPK1 numeric(9, 4)
	declare @CPK2 numeric(9, 4) 
	declare @CPKMin numeric(9, 4)
	declare @SpceUSL_6 float = (@SpecUSL - @SpecLSL) / 6
	declare @RBarD2 numeric(28, 18) = @RBar / @D2

	if @RBar <> 0 and @d2 <> 0
	begin
		set @CP = @SpceUSL_6 / @RBarD2	
		set @CPK1 = (@SpecUSL - @XBarBar) / 3 / (@RBar / @d2)	
		set @CPK2 = (@XBarBar - @SpecLSL) / 3 / (@RBar / @d2)	
		if @CPK1 <= @CPK2 
			set @CPKMin = @CPK1
		else
			set @CPKMin = @CPK2
	end
		
	
	select @SpecUSL - @SpecLSL [SpecUSL - SpecLSL], @SpceUSL_6 [(SpecUSL - SpecLSL) / 6],
	(@SpecUSL - @SpecLSL) / 6 [(SpecUSL - SpecLSL) / 6] , 
	@RBar R_Bar,  @RBarD2 RBar_D2,
	
	@Min [Min], @Max [Max], 
	cast(@Avg as numeric(9, 4)) [Avg], 
	cast(@STD as numeric(9, 4)) STD, 
	@SpecLSL LSL, @SpecUSL USL, 
	@LCL LCL, @CL CL, @UCL UCL, @RUCL RUCL, @XBarLCL XBarLCL, @XBarUCL XBarUCL,
	@CP CP, @CPK1 CPK1, @CPK2 CPK2, 
	cast(@XBarBar as numeric(9, 4)) XBarBar, 
	cast(@RBar as numeric(9, 4)) RBar, 
	@CPKMin CPKMin,
	@nSample nSample, @d2 d2, 
	@ItemTypeName ItemTypeName, 
	@Line + ' - ' + @LineName LineName, 
	@ItemCheckCode + ' - ' + @ItemCheckName ItemCheckName,
	@Unit Unit, @FactoryName FactoryName,
	Case when @XBarBar < @LCL or @XBarBar > @UCL then 1 else 0 end XBarColor

	set @que = 'select 6 seq, ''RUCL'' Des, ' + @ListCol + '
		from (select ColName, RUCL from #tempXR) Tb pivot (max(RUCL) for ColName in (' + @ListCol + ')) Pv'
	exec sp_executesql @que
	set @que = 'select 7 seq, ''RLCL'' Des, ' + @ListCol + '
		from (select ColName, RLCL from #tempXR) Tb pivot (max(RLCL) for ColName in (' + @ListCol + ')) Pv'
	exec sp_executesql @que

	set @que = 'select 8 Seq, SequenceNo, cast(SequenceNo as varchar(2)) Des, ' + @ListColString + '
		from (select ColName, SequenceNo, DeleteStatus from #tempInput) Tb pivot (max(DeleteStatus) for ColName in (' + @ListCol + ')) Pv'

	exec sp_executesql @que	
END
