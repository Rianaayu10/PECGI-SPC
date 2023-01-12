/****** Object:  StoredProcedure [dbo].[sp_SPC_SampleControlChart]    Script Date: 12/01/2023 08:57:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ari
-- Create date: 08-Sep-2022
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[sp_SPC_SampleControlChart]
	@FactoryCode varchar(15) = 'F001',
	@ItemTypeCode varchar(15) = 'TPMSBR011',
	@Line varchar(15) = '015',
	@ItemCheckCode varchar(15) = 'IC021',
	@ProdDate date = NULL,
	@ProdDate2 date = '26 Oct 2022',
	@VerifiedOnly int = 1,
	@SeqNo int = 99,
	@Shift varchar(10) = '',
	@CompleteStatus int = 1
AS
BEGIN

	declare @PointCount int = 4	
	
	if @ProdDate is Null
	set @ProdDate = (
		select max(ProdDate) from spc_Result R inner join spc_ResultDetail D on R.SPCResultID = D.SPCResultID
		where R.FactoryCode = @FactoryCode and R.ItemTypeCode = @ItemTypeCode and R.LineCode = @Line and R.ItemCheckCode = @ItemCheckCode and R.ProdDate < @ProdDate2 
		and D.DeleteStatus = 0 
		and isnull(R.CompleteStatus, 0) = case when @CompleteStatus = 1 then 1 else isnull(R.CompleteStatus, 0) end
	)	

	if @ProdDate is Null
	set @ProdDate = @ProdDate2

	--if exists (select * from tempdb.sys.tables where name like '#tempXR%')
	--drop table #tempXR


	select R.ProdDate, case R.ShiftCode when 'SH001' then '1' else '2' end ShiftCode, R.SequenceNo,
	convert(char(6), R.ProdDate, 12) + '_' + case R.ShiftCode when 'SH001' then '1' else '2' end + '_' + cast(R.SequenceNo as varchar(2)) ColName, 
	convert(char(8), R.RegisterDate, 114) RegisterDate,
	D.SequenceNo ItemSeqNo, D.Value, S.SpecLSL, S.SpecUSL, S.CPLCL, S.CPUCL, S.CPCL, NULL RuleValue, NULL RuleYellow, 0 AllTop, 0 AllBottom
	into #tempXR
	from spc_Result R inner join uf_SPCResult_Detail(@VerifiedOnly) D on R.SPCResultID = D.SPCResultID
	left join spc_ChartSetup S on R.FactoryCode = S.FactoryCode and R.ItemTypeCode = S.ItemTypeCode and R.LineCode = S.LineCode and R.ItemCheckCode = S.ItemCheckCode
	and R.ProdDate between S.StartDate and S.EndDate
	where R.ProdDate between @ProdDate and @ProdDate2
	and R.FactoryCode = @FactoryCode and R.ItemTypeCode = @ItemTypeCode and R.LineCode = @Line and R.ItemCheckCode = @ItemCheckCode	
	and R.SequenceNo <= case when R.ProdDate = @ProdDate2 then @SeqNo else 99 end
	and isnull(R.CompleteStatus, 0) = case when @CompleteStatus = 1 then 1 else isnull(R.CompleteStatus, 0) end
	declare @SeqCount int = (select count(Distinct ColName) CountSeq from #tempXR)

	------------- Update Rule ------------------

	update #tempXR set RuleValue = 1 where ColName in (
		select ColName
		from #tempXR group by ColName, SpecUSL, SpecLSL
		having Avg(Value) > SpecUSL or Avg(Value) < SpecLSL
	)	
	update #tempXR set RuleValue = 1 where ColName in (select ColName from #tempXR where RuleValue = 1)
	update #tempXR set RuleValue = NULL where ColName in (select ColName from #tempXR where RuleValue = 1) and ItemSeqNo <> 1

	update #tempXR set RuleYellow = 1 where ColName in (
		select ColName
		from #tempXR group by ColName, CPUCL, CPLCL
		having Avg(Value) > CPUCL or Avg(Value) < CPLCL
	) and RuleValue is Null
	update #tempXR set RuleYellow = 1 where ColName in (select ColName from #tempXR where RuleYellow = 1) and RuleValue is Null
	update #tempXR set RuleYellow = NULL where ColName in (select ColName from #tempXR where RuleYellow = 1) and ItemSeqNo <> 1 

	--declare @ShiftCode int, @SequenceNo int, @AvgValue numeric(9, 3), @PrevValue numeric(9, 3), @Decreasing int = 0, @Increasing int = 0, @AllTop int, @AllBottom int,
	--@ColName varchar(20), @MinValue numeric(9, 3), @MaxValue numeric(9, 3), @UCL numeric(9, 3), @LCL numeric(9, 3), @CL numeric(9, 3)

	--declare cr cursor for 
	--select ColName, Avg(Value) AvgValue, Min(Value) MinValue, Max(Value) MaxValue, min(CPLCL) CPLCL, max(CPUCL) CPUCL, max(CPCL) CPCL 
	--from #tempXR group by ColName

	--open cr
	--fetch next from cr into @ColName, @AvgValue, @MinValue, @MaxValue, @UCL, @LCL, @CL
	--while @@FETCH_STATUS = 0
	--begin		
	--	if @PrevValue is not Null 
	--	begin
	--		if @AvgValue < @PrevValue 
	--		begin
	--			set @Decreasing = @Decreasing + 1 				
	--		end else 
	--			set @Decreasing = 0
	--		if @AvgValue > @PrevValue set @Increasing = @Increasing + 1 else set @Increasing = 0
	--	end

	--	if @AvgValue > @CL 
	--	begin
	--		set @AllTop    = isnull(@AllTop, 0) + 1    
	--		update #tempXR set AllTop = @AllTop where ColName = @ColName and ItemSeqNo = 1 
	--	end
	--	else 
	--		set @AllTop = 0

	--	if @AvgValue < @CL 
	--	begin
	--		set @AllBottom = isnull(@AllBottom, 0) + 1 
	--		update #tempXR set AllBottom = @AllBottom where ColName = @ColName and ItemSeqNo = 1 
	--	end
	--	else 
	--		set @AllBottom = 0

	--	if @Decreasing >= @PointCount - 1
	--	begin
	--		update #tempXR set RuleValue = 1 where ColName = @ColName and ItemSeqNo = 1 and RuleValue is Null
	--		update #tempXR set RuleYellow = 1 where ItemSeqNo = 1 and RuleValue is Null and RuleYellow is Null
	--		and ColName in (
	--			select top (@PointCount - 1) ColName from #tempXR where ColName < @ColName and ItemSeqNo = 1
	--			order by ColName desc
	--		)
	--		--set @Decreasing = 0
	--	end

	--	if @Increasing >= @PointCount - 1
	--	begin			
	--		update #tempXR set RuleValue = 1 where ColName = @ColName and ItemSeqNo = 1 and RuleValue is Null
	--		update #tempXR set RuleYellow = 1 where ItemSeqNo = 1 and RuleValue is Null and RuleYellow is Null
	--		and ColName in (
	--			select top (@PointCount - 1) ColName from #tempXR where ColName < @ColName and ItemSeqNo = 1
	--			order by ColName desc
	--		)
	--		--set @Increasing = 0
	--	end

	--	if @AllTop >= @PointCount 
	--	begin
	--		update #tempXR set RuleValue = 1 where ColName = @ColName and ItemSeqNo = 1 and RuleValue is Null
	--		update #tempXR set RuleYellow = 1 where ItemSeqNo = 1 and RuleValue is Null and RuleYellow is Null
	--		and ColName in (
	--			select top (@PointCount - 1) ColName from #tempXR where ColName < @ColName and ItemSeqNo = 1
	--			order by ColName desc
	--		)
	--		--set @AllTop = 0
	--	end

	--	if @AllBottom >= @PointCount
	--	begin
	--		update #tempXR set RuleValue = 1 where ColName = @ColName and ItemSeqNo = 1 and RuleValue is Null
	--		update #tempXR set RuleYellow = 1 where ItemSeqNo = 1 and RuleValue is Null and RuleYellow is Null
	--		and ColName in (
	--			select top (@PointCount - 1) ColName from #tempXR where ColName < @ColName and ItemSeqNo = 1
	--			order by ColName desc
	--		)
	--		--set @AllBottom = 0
	--	end

	--	set @PrevValue = @AvgValue
	--	fetch next from cr into @ColName, @AvgValue, @MinValue, @MaxValue, @UCL, @LCL, @CL
	--end
	--close cr
	--deallocate cr

	--------------------------------------------

	declare @ChartType char(1) = (
		select top 1 CharacteristicStatus from spc_ItemCheckByType where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @Line and ItemCheckCode = @ItemCheckCode 
	)

	select A.ColName, '#' + cast(A.ItemSeqNo as varchar(2)) [Description], 
	A.ProdDate, A.ShiftCode, A.SequenceNo, A.RegisterDate, 
	format(A.ProdDate, 'dd MMM yyyy') + char(10) + 'Shift ' + cast(A.ShiftCode as char(1)) + char(10) + 'Seq ' + cast(A.SequenceNo as char(1)) Seq, A.[Value]
	,B.AvgValue, B.RValue, A.[RuleValue], A.RuleYellow, A.CPUCL, A.CPLCL, A.CPCL, C.MinValue, C.MaxValue, A.SpecLSL, A.SpecUSL, @SeqCount SeqCount, 
	A.AllTop, A.AllBottom
	from #tempXR A 
	inner join (
		select ColName, cast(avg(Value) as numeric(9, 3)) AvgValue, max(Value) - min(Value) RValue
		from #tempXR
		group by ColName
	) B on A.ColName = B.ColName inner join (
		select Min(Value) MinValue, Max(Value) MaxValue from #tempXR
	) C on 1 = 1	

	order by B.ColName, ShiftCode, SequenceNo

	--drop table #tempXR
END
