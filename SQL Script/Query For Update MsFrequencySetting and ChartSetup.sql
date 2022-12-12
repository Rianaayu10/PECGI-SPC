/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Email]    Script Date: 12/9/2022 05:24:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_ChartSetup_Email]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_ChartSetup_Email] AS' 
END
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-05>
-- Description:	<X Bar - R Control Chart System Setup>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_ChartSetup_Email]
	@Factory	As varchar(25) = 'F001',
	@ItemType	As varchar(25) = 'TPMSBR011',
	@Line		As varchar(5)  = '004',
	@ItemCheck	As varchar(15) = 'IC001',
	@Start		As varchar(10) = '2022-07-01',	
	@StartOld	As varchar(10) = '2022-07-01',	
	@End		As varchar(10) = '2022-08-01',
	@EndOld		As varchar(10) = '9999-12-31',	
	@SpecUSL	As Numeric(10,3) = '1',	 @SpecUSLOld	As Numeric(10,3) = '1',
	@SpecLSL	As Numeric(10,3) = '2',  @SpecLSLOld	As Numeric(10,3) = '2',
	--@XBarCL	As Numeric(10,3) = '3',  @XBarCLOld		As Numeric(10,3) = '3',
	@XBarUCL	As Numeric(10,3) = '3',  @XBarUCLOld	As Numeric(10,3) = '3',
	@XBarLCL	As Numeric(10,3) = '4',  @XBarLCLOld	As Numeric(10,3) = '4',
	@CPCL		As Numeric(10,3) = '5',  @CPCLOld		As Numeric(10,3) = '5',
	@CPUCL		As Numeric(10,3) = '6',  @CPUCLOld		As Numeric(10,3) = '6',
	@CPLCL		As Numeric(10,3) = '7',  @CPLCLOld		As Numeric(10,3) = '7',
	@RCL		As Numeric(10,3) = '8',  @RCLOld		As Numeric(10,3) = '8',
	@RLCL		As Numeric(10,3) = '9',  @RLCLOld		As Numeric(10,3) = '9',
	@RUCL		As Numeric(10,3) = '10', @RUCLOld		As Numeric(10,3) = '10',
	@User		As varchar(50)	 = 'F001',
	@Type		As char(1) --0 Save | 1 Update
As
BEGIN
	Declare @BodyEmail as nvarchar(Max)
	Declare @Subject as nvarchar(Max)
	Declare @To as nvarchar(Max) = 'fikri@tos.co.id'
	Declare @CC as nvarchar(Max) = 'fikri@tos.co.id'
	Declare @ScheduleTitle as nvarchar(Max)

	Declare @FactoryName	as varchar(max) = (Select Top 1 FactoryName from MS_Factory Where FactoryCode = @Factory)
	Declare @ItemTypeName	as varchar(max) = (select Top 1 Description from MS_ItemType where ItemTypeCode = @ItemType)
	Declare @ItemCheckName	as varchar(max) = (select Top 1 ItemCheckCode + ' - ' + ItemCheck from spc_ItemCheckMaster Where ItemCheckCode = @ItemCheck)
	Declare @LineName		as varchar(max) = (select Top 1 LineCode + ' - ' + LineName from MS_Line Where FactoryCode = @Factory and LineCode = @Line)

IF @Type = '0'
	Begin
		SET @Subject = 'SPC : X Bar - R Control Chart Setup Adding Data'
		SET @ScheduleTitle = 'X Bar - R Control Chart Setup Adding Data'
	End
Else
	Begin
		SET @Subject = 'SPC : X Bar - R Control Chart Setup Changed Data'
		SET @ScheduleTitle = 'X Bar - R Control Chart Setup Changed Data'
	End

SET @BodyEmail = 
'<html>
	<h2>---WARNING---</h2>

	<table>
		<tr>
			<td>SCHEDULER ISSUE TIME</td>
			<td> : </td>
			<td>'+ Format(GETDATE(),'dd MMM yyyy HH:mm:ss') +'</td>
		</tr>
		<tr>
			<td>SCHEDULER SOURCE</td>
			<td> : </td>
			<td>'+ @ScheduleTitle +'</td>
		</tr>
		<tr>
			<td>FACTORY</td>
			<td> : </td>
			<td>'+ @FactoryName +'</td>
		</tr>
		<tr>
			<td>TYPE</td>
			<td> : </td>
			<td>'+ @ItemTypeName +'</td>
		</tr>
		<tr>
			<td>MACHINE PROCESS</td>
			<td> : </td>
			<td>'+ @LineName +'</td>
		</tr>
		<tr>
			<td>ITEM CHECK</td>
			<td> : </td>
			<td>'+ @ItemCheckName +'</td>
		</tr>

	</table>
	
	 <BR>
	 
	 <P>DETAIL INFORMATION</p>'

	 IF @Type = '0'
		Begin
			SET @BodyEmail = 
@BodyEmail + 
'
	<p style="font-weight: bold;font-style: italic;">Adding New Data</p>

	<table>
		<tr>
			<td>Start</td>
			<td> : </td>
			<td>'+ Format(Cast(@Start as date),'dd MMM yyyy') +'</>
		</tr>

		<tr>
			<td>End</td>
			<td> : </td>
			<td>'+ Format(Cast(@End as date),'dd MMM yyyy') +'</>
		</tr>

		<tr>
			<td>Spec USL</td>
			<td> : </td>
			<td>'+ cast(@SpecUSL as varchar) +'</>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>
			<td>'+ cast(@SpecLSL as varchar) +'</>
		</tr>
		
		<tr>
			<td>Control Plan CL</td>
			<td> : </td>
			<td>'+ cast(@CPCL as varchar) +'</>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>
			<td>'+ cast(@CPUCL as varchar) +'</>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>
			<td>'+ cast(@CPLCL as varchar) +'</>
		</tr>		

		<tr>
			<td>X Bar UCL</td>
			<td> : </td>
			<td>'+ cast(@XBarUCL as varchar) +'</>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>
			<td>'+ cast(@XBarLCL as varchar) +'</>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>
			<td>'+ cast(@RUCL as varchar) +'</>
		</tr>
	</table>'
		End

	IF @Type = '1'
		Begin
			SET @BodyEmail = 
@BodyEmail + 
'
	<p style="font-weight: bold;font-style: italic;">Before</p>

	<table>
		<tr>
			<td>Start</td>
			<td> : </td>
			<td>'+ Format(Cast(@StartOld as date),'dd MMM yyyy') +'</>
		</tr>

		<tr>
			<td>End</td>
			<td> : </td>
			<td>'+ Format(Cast(@EndOld as date),'dd MMM yyyy') +'</>
		</tr>

		<tr>
			<td>Spec USL</td>
			<td> : </td>
			<td>'+ cast(@SpecUSLOld as varchar) +'</>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>
			<td>'+ cast(@SpecLSLOld as varchar) +'</>
		</tr>

		<tr>
			<td>Control Plan CL</td>
			<td> : </td>
			<td>'+ cast(@CPCLOld as varchar) +'</>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>
			<td>'+ cast(@CPUCLOld as varchar) +'</>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>
			<td>'+ cast(@CPLCLOld as varchar) +'</>
		</tr>
		
		<tr>
			<td>X Bar UCL</td>
			<td> : </td>
			<td>'+ cast(@XBarUCLOld as varchar) +'</>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>
			<td>'+ cast(@XBarLCLOld as varchar) +'</>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>
			<td>'+ cast(@RUCLOld as varchar) +'</>
		</tr>
	</table>
	<BR>

	<p style="font-weight: bold;font-style: italic;">After</p>

	<table>
		<tr>
			<td>Start</td>
			<td> : </td>
			<td>'+ Format(Cast(@Start as date),'dd MMM yyyy') +'</>
		</tr>

		<tr>
			<td>End</td>
			<td> : </td>
			<td>'+ Format(Cast(@End as date),'dd MMM yyyy') +'</>
		</tr>

		<tr>
			<td>Spec USL</td>
			<td> : </td>
			<td>'+ cast(@SpecUSL as varchar) +'</>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>
			<td>'+ cast(@SpecLSL as varchar) +'</>
		</tr>

		<tr>
			<td>Control Plan CL</td>
			<td> : </td>
			<td>'+ cast(@CPCL as varchar) +'</>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>
			<td>'+ cast(@CPUCL as varchar) +'</>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>
			<td>'+ cast(@CPLCL as varchar) +'</>
		</tr>	

		<tr>
			<td>X Bar UCL</td>
			<td> : </td>
			<td>'+ cast(@XBarUCL as varchar) +'</>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>
			<td>'+ cast(@XBarLCL as varchar) +'</>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>
			<td>'+ cast(@RUCL as varchar) +'</>
		</tr>
	</table>'
		End


	SET @BodyEmail = @BodyEmail + 
'
	<BR>
	<BR>
	THANK YOU FOR YOUR ATTENTION
</html>
'

	Exec SP_SPC_SendEmailAlert
		@FactoryCode	= @Factory,
		@ItemTypeCode	= @ItemType,
		@LineCode		= @Line,
		@ItemCheckCode	= @ItemCheck,
		@ProdDate		= NULL,
		@ShiftCode		= NULL,
		@SequenceNo		= NULL,
		@BodyEmail		= @BodyEmail,
		@Subject		= @Subject,
		@LastUser		= @User,
		@NotificationCategory = '4'
END

GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_FillCombo]    Script Date: 12/9/2022 05:24:59 PM ******/
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
		select ItemTypeCode Code, Description [Description] from MS_ItemType Order By ItemTypeCode
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
				--and	ISNULL(a.ActiveStatus,0) = 1
				--and	ISNULL(a.ActiveStatus,0) = 1
				and	1 = Case
							When @Param2 <> 'ALL' and a.ItemTypeCode = @Param2 then 1
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
				and a.ItemTypeCode = @Param2
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
		) a order by Code
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
/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Sel]    Script Date: 12/9/2022 05:24:59 PM ******/
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
	and @Period Between Format(a.StartDate,'yyyy-MM-dd') and Format(a.EndDate,'yyyy-MM-dd')
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_MS_FrequencySetting_FillCombo]    Script Date: 12/9/2022 05:24:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_MS_FrequencySetting_FillCombo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_MS_FrequencySetting_FillCombo] AS' 
END
GO
-- =============================================
-- Author:		<Fikr>
-- Create date: <2022-08-05>
-- Description:	<Time/Frequency Setting>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_MS_FrequencySetting_FillCombo]
	@Type As Char(1)
As
BEGIN

--Frequency
IF @Type = '0'
	Begin
		Select 
			FrequencyCode Code,
			FrequencyName Description
		from spc_MS_FrequencySetting
		Order By FrequencyCode
	End

--for Grid -> Shift 
IF @Type = '1'
	Begin
		Select * from
		(
			select distinct
				Code = substring(ShiftCode,1,5),
				Description = substring(ShiftCode,1,5) 
			from MS_Shift
		) a
		Order By Code
	End
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_MS_FrequencySetting_InsUpd]    Script Date: 12/9/2022 05:24:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_MS_FrequencySetting_InsUpd]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_MS_FrequencySetting_InsUpd] AS' 
END
GO
-- =============================================
-- Author		: <Fikr>
-- Create date	: <2022-08-05>
-- Description	: <Time/Frequency Setting>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_MS_FrequencySetting_InsUpd]
	@Frequency	As Varchar(2),
	@Sequence	As Int,
	@Shift		As Varchar(25),
	@Start		As Varchar(5),
	@End		As Varchar(5),
	@Verif		As Varchar(5),
	@User		As Varchar(50),
	@Type		As Varchar(5)
As
BEGIN
	If @Type = '0'
		Begin
			If Exists(Select Top 1 * From spc_MS_Frequency Where FrequencyCode = @Frequency and SequenceNo = @Sequence)
				Begin
					Declare @msg as varchar(max)
					Select	@msg = 'Time Frequency for Frequency ' + FrequencyName + ' and Sequence ' + Cast(@Sequence As varchar(max)) +' Already Exists'
					From spc_MS_FrequencySetting Where FrequencyCode = @Frequency
					Raiserror(@msg,16,1)
				End
			Else
				Begin
					Insert Into spc_MS_Frequency
					(FrequencyCode, ShiftCode, StartTime, EndTime, VerificationTime, RegisterUser, RegisterDate, UpdateUser, UpdateDate, SequenceNo)
					VALUES
					(
						@Frequency, @Shift, @Start, @End, @Verif, @User, GETDATE(), @User, GETDATE(), @Sequence
					)
				End
		End

	If @Type = '1'
		Begin
			Update	spc_MS_Frequency
			SET		ShiftCode	  = @Shift,
					StartTime	  = @Start, EndTime = @End,
					VerificationTime = @Verif,
					UpdateDate	  = GETDATE(), UpdateUser = @User
			Where	FrequencyCode = @Frequency 
				and SequenceNo	  = @Sequence
		End
END
GO
