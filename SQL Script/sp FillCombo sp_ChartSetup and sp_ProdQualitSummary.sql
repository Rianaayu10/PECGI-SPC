/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_FillCombo]    Script Date: 12/20/2022 02:51:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_ChartSetup_FillCombo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_ChartSetup_FillCombo] AS' 
END
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-05>
-- Description:	<X Bar - R Control Chart System Setup>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_ChartSetup_FillCombo]
	@Type	As char(1),
	@Param	As varchar(100) = NULL,
	@Param2	As varchar(100) = NULL,
	@Param3	As varchar(100) = NULL
As
BEGIN
--For Factory
	If @Type = '0'
	Begin
		select	a.FactoryCode Code, FactoryName [Description] 
		from	MS_Factory a inner join spc_UserSetup b on a.FactoryCode = b.FactoryCode and b.UserID = @Param
		Order By a.FactoryCode
	End
	
--For Type
	If @Type = '1'
	Begin
		select ItemTypeCode Code, Description [Description] from MS_ItemType Order By Description
	End

--For Machine
	If @Type = '2'	
	Begin	
		Select Code, [Description] From
		(
			Select 0 Tipe,'ALL'Code, 'ALL' [Description]
			Union ALL
			select distinct 1 Tipe, a.LineCode, a.LineCode + ' - ' + LineName [Description]
			from	spc_ItemCheckByType a join MS_Line b on a.LineCode = b.LineCode and a.FactoryCode = b.FactoryCode  join spc_ItemCheckMaster c on a.ItemCheckCode = c.ItemCheckCode
			where	a.FactoryCode = @Param
				and	ISNULL(a.ActiveStatus,0) = 1				
				--and	1 = Case
				--			When @Param2 <> 'ALL' and a.ItemTypeCode = @Param2 then 1
				--			When @Param2 = 'ALL' then 1
				--			else 0
				--		end
				and	1 = Case
							When @Param2 <> 'ALL' and b.ProcessCode = @Param2 then 1
							When @Param2 = 'ALL' then 1
							else 0
						end
		) A 
		order by Tipe, Code
	End

--For Machine in Grid Editor
	If @Type = '3'
	Begin
		Select Code, [Description] From
		(
			select distinct a.LineCode Code, a.LineCode + ' - ' + LineName [Description] 
			from	spc_ItemCheckByType a join MS_Line b on a.LineCode = b.LineCode and a.FactoryCode = b.FactoryCode join spc_ItemCheckMaster c on a.ItemCheckCode = c.ItemCheckCode
			where	a.FactoryCode = @Param
				--and	ISNULL(a.ActiveStatus,0) = 1
				--and	ISNULL(c.ActiveStatus,0) = 1
				--and a.ItemTypeCode = @Param2
		) A 
		order by Code
	End

--For Machine in Grid Filter
	If @Type = '4'
	Begin
		--select LineCode Code, LineCode + ' - ' + LineName [Description] from MS_Line Order By LineCode, LineName
		Select Code, [Description] From
		(
			select distinct a.LineCode + ' - ' + LineName Code, a.LineCode + ' - ' + LineName [Description] 
			from	spc_ChartSetup a join MS_Line b on a.LineCode = b.LineCode and a.FactoryCode = b.FactoryCode join spc_ItemCheckMaster c on a.ItemCheckCode = c.ItemCheckCode
			--where	ISNULL(a.ActiveStatus,0) = 1
			--	and	ISNULL(c.ActiveStatus,0) = 1
			where	a.FactoryCode	= @Param
				AND	1 = Case
							When @Param2 <> 'ALL' and a.LineCode = @Param2 then 1
							When @Param2 = 'ALL' then 1
							else 0
						end
				AND a.ItemTypeCode	= @Param3
		) A 
		order by Code
	End

--For Item Check in Grid Editor
	If @Type = '5'
	Begin
		Select * from 
		(
			select distinct a.ItemCheckCode Code, a.ItemCheckCode + ' - ' + b.ItemCheck [Description] 
			from	spc_ItemCheckByType a join spc_ItemCheckMaster b on a.ItemCheckCode = b.ItemCheckCode
			where	a.FactoryCode = @Param
				--and	ISNULL(a.ActiveStatus,0) = 1
				--and	ISNULL(b.ActiveStatus,0) = 1
				and a.ItemTypeCode = @Param2
				and	a.LineCode = @Param3
		) a order by Code
	End

--For Item Check in Grid Filter
	If @Type = '6'
	Begin
		Select * from 
		(
			select distinct a.ItemCheckCode + ' - ' + b.ItemCheck Code, a.ItemCheckCode + ' - ' + b.ItemCheck [Description] 
			from	spc_ChartSetup a join spc_ItemCheckMaster b on a.ItemCheckCode = b.ItemCheckCode
				--where	ISNULL(a.ActiveStatus,0) = 1
				--	and	ISNULL(b.ActiveStatus,0) = 1
			where	a.FactoryCode	= @Param
				AND	1 = Case
							When @Param2 <> 'ALL' and a.LineCode = @Param2 then 1
							When @Param2 = 'ALL' then 1
							else 0
						end
				AND a.ItemTypeCode	= @Param3
		) a order by Code
	End

--For Type in Grid Filter
	If @Type = '7'
	Begin
		Select * from 
		(
			select distinct c.Description Code, c.Description [Description]
			from	spc_ChartSetup a join spc_ItemCheckMaster b on a.ItemCheckCode = b.ItemCheckCode left join MS_ItemType c on a.ItemTypeCode = c.ItemTypeCode
			--where	ISNULL(a.ActiveStatus,0) = 1
			--	and	ISNULL(b.ActiveStatus,0) = 1
			where	a.FactoryCode	= @Param
				AND	1 = Case
							When @Param2 <> 'ALL' and a.LineCode = @Param2 then 1
							When @Param2 = 'ALL' then 1
							else 0
						end
				AND a.ItemTypeCode	= @Param3
		) a order by Description
	End

--For Characteristics Status in Grid Filter
	If @Type = '8'
	Begin
		Select '0 - Non Special' Code, '0 - Non Special' Description
		Union All
		Select '1 - Special Characteristics', '1 - Special Characteristics'
	End
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_ProdQualitySummary_FillCombo]    Script Date: 12/20/2022 02:51:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_ProdQualitySummary_FillCombo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_ProdQualitySummary_FillCombo] AS' 
END
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-19>
-- Description:	<Production Sample Control Quality Summary>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_ProdQualitySummary_FillCombo]
	@Type	As char(1),
	@Param	As varchar(100) = NULL,
	@Param2	As varchar(100) = NULL,
	@Param3	As varchar(100) = NULL,
	@Param4	As varchar(100) = NULL,
	@Param5	As varchar(100) = NULL
As
BEGIN
--For Factory
	If @Type = '0'
	Begin
		select	a.FactoryCode Code, FactoryName [Description] 
		from	MS_Factory a inner join spc_UserSetup b on a.FactoryCode = b.FactoryCode and b.UserID = @Param
		Order By Description
	End
	
--For Type
	If @Type = '1'
	Begin
		Select Code, [Description] From
		(
			Select 0 Tipe,'ALL'Code, 'ALL' [Description]
			Union ALL
			select 1 Tipe, ItemTypeCode Code, Description [Description] from MS_ItemType
		) A 
		order by Tipe, Code, Description
	End

--For Machine
	If @Type = '2'
	Begin
		Select Code, [Description] From
		(
			Select 0 Tipe,'ALL'Code, 'ALL' [Description]
			Union ALL
			select	distinct 1 Tipe, A.LineCode, A.LineCode + ' - ' + B.LineName [Description] 
			from	spc_ItemCheckByType a 
			   JOIN MS_Line b on a.LineCode = b.LineCode and a.FactoryCode = b.FactoryCode 
			   JOIN spc_UserLine c ON b.LineCode = c.LineCode and c.AppID = 'SPC' and ISNULL(AllowShow,0) = 1
			Where	ISNULL(a.ActiveStatus,0) = 1
				and	a.FactoryCode = @Param 
				and	1 = Case
							When @Param2 <> 'ALL' and b.ProcessCode = @Param2 then 1
							When @Param2 = 'ALL' then 1
							else 0
						end
				and c.UserID = @Param3
				--and	1 = Case
				--			When @Param2 <> 'ALL' and a.ItemTypeCode = @Param2 then 1
				--			When @Param2 = 'ALL' then 1
				--			else 0
				--		end
		) A 
		order by Tipe, Code, Description
	End

--For Frequency
	If @Type = '3'
	Begin
		Select 'ALL' Code, 'ALL' [Description]
		Union ALL
		select distinct
			Code = a.FrequencyCode,
			[Description] = b.FrequencyName
		from 
			spc_ItemCheckByType a inner join 
			spc_MS_FrequencySetting b on a.FrequencyCode = b.FrequencyCode
			inner join (Select L.LineCode from MS_Line L join spc_UserLine U on L.LineCode = U.LineCode and U.UserID = @Param4) c on a.LineCode = c.LineCode
		where
			a.FactoryCode	= @Param
		and	1 = Case 
					When @Param2 = 'ALL' Then 1 
					When @Param2 <> 'ALL' and a.ItemTypeCode = @Param2 Then 1
					Else 0
				End
		and	1 = Case 
					When @Param3 = 'ALL' Then 1 
					When @Param3 <> 'ALL' and a.LineCode = @Param3 Then 1
					Else 0
				End
	End

--For Sequence
	If @Type = '4'
	Begin
		Select 'ALL' Code, 'ALL' [Description]
		Union ALL
		select Distinct
			Code = cast(b.SequenceNo as varchar(max)),
			[Description] = cast(b.SequenceNo as varchar(max))
		from 
			spc_ItemCheckByType a inner join 
			spc_MS_Frequency b on a.FrequencyCode = b.FrequencyCode
			inner join (Select L.LineCode from MS_Line L join spc_UserLine U on L.LineCode = U.LineCode and U.UserID = @Param5) c on a.LineCode = c.LineCode
		where
			a.FactoryCode	= @Param
		and	1 = Case 
					When @Param2 = 'ALL' Then 1 
					When @Param2 <> 'ALL' and a.FrequencyCode = @Param2 Then 1
					Else 0
				End
		and	1 = Case 
					When @Param3 = 'ALL' Then 1 
					When @Param3 <> 'ALL' and a.ItemTypeCode = @Param3 Then 1
					Else 0
				End
		and	1 = Case 
					When @Param4 = 'ALL' Then 1 
					When @Param4 <> 'ALL' and a.LineCode = @Param4 Then 1
					Else 0
				End
	End
END
GO
