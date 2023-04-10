/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Sel]    Script Date: 01/17/2023 09:42:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-05>
-- Description:	<X Bar - R Control Chart System Setup>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SPC_ChartSetup_Sel]
	@Factory	As varchar(25),
	@Machine	As varchar(5),
	@MachineIOT	As Varchar(MAX),
	@Type		As varchar(25),
	@Period		As varchar(10)
As
BEGIN
	Select 
		Factory			= a.FactoryCode,
		Type			= b.Description,
		TypeEditGrid	= a.ItemTypeCode,
		Machine			= a.LineCode + ' - ' + c.LineName,
		MachineEditGrid	= a.LineCode,
		ItemCheck		= a.ItemCheckCode + ' - ' + d.ItemCheck,
		ItemCheckEditGrid= a.ItemCheckCode,
		[Start]			= a.StartDate, --FORMAT(a.StartDate,'dd MMM yyyy'),
		[End]			= a.EndDate, --FORMAT(a.EndDate,'dd MMM yyyy'),
		Characteristic	= 
			CASE
				WHEN e.CharacteristicStatus = 0 THEN '0 - Non Special'
				WHEN e.CharacteristicStatus = 1 THEN '1 - Special Characteristics'
				ELSE e.CharacteristicStatus
			End,
		MeasuringUnit	= d.UnitMeasurement,
		SpecUSL = ISNULL(a.SpecUSL,0),	view_SpecUSL = Case When ISNULL(a.SpecUSL,0) = 0.000 Then '#NA' Else Cast(ISNULL(a.SpecUSL,0)	as varchar(max)) End,
		SpecLSL = ISNULL(a.SpecLSL,0),	view_SpecLSL = Case When ISNULL(a.SpecLSL,0) = 0.000 Then '#NA' Else Cast(ISNULL(a.SpecLSL,0)	as varchar(max)) End,
		--XCL	= ISNULL(a._XBarCL,0),
		XUCL	= ISNULL(a.XBarUCL,0),	view_XUCL	 = Case When ISNULL(a.XBarUCL,0) = 0.000 Then '#NA' Else Cast(ISNULL(a.XBarUCL,0)	as varchar(max)) End,
		XLCL	= ISNULL(a.XBarLCL,0),	view_XLCL	 = Case When ISNULL(a.XBarLCL,0) = 0.000 Then '#NA' Else Cast(ISNULL(a.XBarLCL,0)	as varchar(max)) End,
		CPCL	= ISNULL(a.CPCL,0),		view_CPCL	 = Case When ISNULL(a.CPCL,0)	 = 0.000 Then '#NA' Else Cast(ISNULL(a.CPCL,0)		as varchar(max)) End,
		CPUCL	= ISNULL(a.CPUCL,0),	view_CPUCL	 = Case When ISNULL(a.CPUCL,0)	 = 0.000 Then '#NA' Else Cast(ISNULL(a.CPUCL,0)	as varchar(max)) End,
		CPLCL	= ISNULL(a.CPLCL,0),	view_CPLCL	 = Case When ISNULL(a.CPLCL,0)	 = 0.000 Then '#NA' Else Cast(ISNULL(a.CPLCL,0)	as varchar(max)) End,
		RCL		= ISNULL(a.RCL,0),		view_RCL	 = Case When ISNULL(a.RCL,0)	 = 0.000 Then '#NA' Else Cast(ISNULL(a.RCL,0)		as varchar(max)) End,
		RUCL	= ISNULL(a.RUCL,0),		view_RUCL	 = Case When ISNULL(a.RUCL,0)	 = 0.000 Then '#NA' Else Cast(ISNULL(a.RUCL,0)		as varchar(max)) End,
		RLCL	= ISNULL(a.RLCL,0),		view_RLCL	 = Case When ISNULL(a.RLCL,0)	 = 0.000 Then '#NA' Else Cast(ISNULL(a.RLCL,0)		as varchar(max)) End,
		Remark	= ISNULL(a.Remark,''),
		LastUser	= ISNULL(f.FullName,a.UpdateUser),
		LastUpdate	= Format(a.UpdateDate,'dd MMM yyyy HH:mm:ss')
	From 
		spc_ChartSetup a Inner Join
		MS_ItemType b on a.ItemTypeCode = b.ItemTypeCode inner join
		MS_Line c on a.FactoryCode = c.FactoryCode and a.LineCode = c.LineCode inner join
		spc_ItemCheckMaster d on a.ItemCheckCode = d.ItemCheckCode inner join
		spc_ItemCheckByType e on a.FactoryCode = e.FactoryCode and a.ItemTypeCode = e.ItemTypeCode and a.LineCode = e.LineCode and a.ItemCheckCode = e.ItemCheckCode Left Join
		spc_UserSetup f on a.UpdateUser = f.UserID
	Where
		a.FactoryCode = @Factory 
	and a.ItemTypeCode = @Type
	and 1 = Case 
				When @Machine = 'ALL' Then 1
				When @Machine <> 'ALL' and a.LineCode = @Machine Then 1
				Else 0
			End
	and 1 = Case 
				When @MachineIOT = 'ALL' Then 1
				When @MachineIOT <> 'ALL' and c.ProcessCode = @MachineIOT Then 1
				Else 0
			End
	and 1 = Case 
				When @Period = 'ALL' Then 1
				When @Period <> 'ALL' and @Period Between Format(a.StartDate,'yyyy-MM-dd') and Format(a.EndDate,'yyyy-MM-dd') Then 1
				Else 0
			End
	Order By
		a.ItemCheckCode, a.StartDate Desc
END
GO
