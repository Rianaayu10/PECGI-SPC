/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Sel]    Script Date: 12/27/2022 10:31:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_ChartSetup_Sel]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_ChartSetup_Sel] AS' 
END
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
		SpecUSL = ISNULL(a.SpecUSL,0),
		SpecLSL = ISNULL(a.SpecLSL,0),
		--XCL	= ISNULL(a._XBarCL,0),
		XUCL	= ISNULL(a.XBarUCL,0),
		XLCL	= ISNULL(a.XBarLCL,0),
		CPCL	= ISNULL(a.CPCL,0),
		CPUCL	= ISNULL(a.CPUCL,0),
		CPLCL	= ISNULL(a.CPLCL,0),
		RCL		= ISNULL(a.RCL,0),
		RUCL	= ISNULL(a.RUCL,0),
		RLCL	= ISNULL(a.RLCL,0),
		Remark	= ISNULL(a.Remark,''),
		LastUser	= a.UpdateUser,
		LastUpdate	= Format(a.UpdateDate,'dd MMM yyyy HH:mm:ss')
	From 
		spc_ChartSetup a Inner Join
		MS_ItemType b on a.ItemTypeCode = b.ItemTypeCode inner join
		MS_Line c on a.FactoryCode = c.FactoryCode and a.LineCode = c.LineCode inner join
		spc_ItemCheckMaster d on a.ItemCheckCode = d.ItemCheckCode inner join
		spc_ItemCheckByType e on a.FactoryCode = e.FactoryCode and a.ItemTypeCode = e.ItemTypeCode and a.LineCode = e.LineCode and a.ItemCheckCode = e.ItemCheckCode
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
