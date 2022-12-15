/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Email]    Script Date: 12/15/2022 02:31:23 PM ******/
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
	@Remark		As varchar(50)	 = 'Tes',@RemarkOld		As varchar(50)	 = 'Tes Old',
	@User		As varchar(50)	 = 'F001',
	@To			as varchar(Max) = '',
	@CC			as varchar(Max) = '',
	@Type		As char(1) --0 Save | 1 Update
As
BEGIN
	Declare @BodyEmail as nvarchar(Max)
	Declare @Subject as nvarchar(Max)
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
			<td>'+ cast(@SpecUSL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>
			<td>'+ cast(@SpecLSL as varchar(100)) +'</>
		</tr>
		
		<tr>
			<td>Control Plan CL</td>
			<td> : </td>
			<td>'+ cast(@CPCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>
			<td>'+ cast(@CPUCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>
			<td>'+ cast(@CPLCL as varchar(100)) +'</>
		</tr>		

		<tr>
			<td>X Bar UCL</td>
			<td> : </td>
			<td>'+ cast(@XBarUCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>
			<td>'+ cast(@XBarLCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>
			<td>'+ cast(@RUCL as varchar) +'</>
		</tr>

		<tr>
			<td>Remark</td>
			<td> : </td>
			<td>'+ @Remark +'</>
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
			<td>'+ cast(@SpecUSLOld as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>
			<td>'+ cast(@SpecLSLOld as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Control Plan CL</td>
			<td> : </td>
			<td>'+ cast(@CPCLOld as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>
			<td>'+ cast(@CPUCLOld as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>
			<td>'+ cast(@CPLCLOld as varchar(100)) +'</>
		</tr>
		
		<tr>
			<td>X Bar UCL</td>
			<td> : </td>
			<td>'+ cast(@XBarUCLOld as varchar(100)) +'</>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>
			<td>'+ cast(@XBarLCLOld as varchar(100)) +'</>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>
			<td>'+ cast(@RUCLOld as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Remark</td>
			<td> : </td>
			<td>'+ @RemarkOld +'</>
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
			<td>'+ cast(@SpecUSL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>
			<td>'+ cast(@SpecLSL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Control Plan CL</td>
			<td> : </td>
			<td>'+ cast(@CPCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>
			<td>'+ cast(@CPUCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>
			<td>'+ cast(@CPLCL as varchar(100)) +'</>
		</tr>	

		<tr>
			<td>X Bar UCL</td>
			<td> : </td>
			<td>'+ cast(@XBarUCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>
			<td>'+ cast(@XBarLCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>
			<td>'+ cast(@RUCL as varchar(100)) +'</>
		</tr>

		<tr>
			<td>Remark</td>
			<td> : </td>
			<td>'+ @Remark +'</>
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
		@To				= @To,
		@CC				= @CC,
		@Subject		= @Subject,
		@LastUser		= @User,
		@NotificationCategory = '4'
END

GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_InsUpd]    Script Date: 12/15/2022 02:31:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SPC_ChartSetup_InsUpd]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SPC_ChartSetup_InsUpd] AS' 
END
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-05>
-- Description:	<X Bar - R Control Chart System Setup>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SPC_ChartSetup_InsUpd]
	@Factory	As varchar(25) = 'F001',
	@ItemType	As varchar(25) = 'TPMSBR011',
	@Line		As varchar(5) = '015',
	@ItemCheck	As varchar(15) = 'IC021',
	@Start		As varchar(10) = '2022-12-01',	
	@StartOld	As varchar(10) = '2022-12-01',	
	@End		As varchar(10) = '9999-12-29',
	@EndOld		As varchar(10) = '9999-12-30',	
	@SpecUSL	As Numeric(10,3) = '1',
	@SpecLSL	As Numeric(10,3) = '2',
	--@XBarCL	As Numeric(10,3) = '3',
	@XBarUCL	As Numeric(10,3) = '4',
	@XBarLCL	As Numeric(10,3) = '5',
	@CPCL		As Numeric(10,3) = '6',
	@CPUCL		As Numeric(10,3) = '7',
	@CPLCL		As Numeric(10,3) = '8',
	@RCL		As Numeric(10,3) = '9',
	@RLCL		As Numeric(10,3) = '10',
	@RUCL		As Numeric(10,3) = '11',
	@Remark		As varchar(50) = 'Test Note',
	@User		As varchar(50) = 'F001',
	@Type		As char(1)--0 Save | 1 Update
As
BEGIN
	Declare @msg as varchar(max)
	Declare @FactoryName	as varchar(max) = (Select Top 1 FactoryName from MS_Factory Where FactoryCode = @Factory)
	Declare @ItemTypeName	as varchar(max) = (select Top 1 Description from MS_ItemType where ItemTypeCode = @ItemType)
	Declare @ItemCheckName	as varchar(max) = (select Top 1 ItemCheckCode + ' - ' + ItemCheck from spc_ItemCheckMaster Where ItemCheckCode = @ItemCheck)
	Declare @LineName		as varchar(max) = (select Top 1 LineCode + ' - ' + LineName from MS_Line Where FactoryCode = @Factory and LineCode = @Line)
	
	IF @Type = 1
		Begin
			If Exists
			(
				Select	Top 1 FactoryCode From spc_ChartSetup 
				Where	FactoryCode = @Factory and ItemTypeCode = @ItemType 
					and LineCode = @Line and ItemCheckCode = @ItemCheck
					and	(
						 @Start between StartDate and EndDate or 
						 @end between StartDate and EndDate or
						 (Format(StartDate, 'yyyy-MM-dd') >= @Start And Format(EndDate, 'yyyy-MM-dd') <= @End)
						)
					and StartDate <> @StartOld and EndDate <> @EndOld
			)
				Begin					
					Set @msg = 'Cannot Update Start ' + @Start + ' - ' +@End + ' in Factory ' + @FactoryName + ' Machine ' + @LineName + ' Item Type ' + @ItemTypeName + ' , Because it is Overlapping with Existing Data. Please Check Again'
					Raiserror(@msg,16,1)
				End
			Else
				Begin
					Update	spc_ChartSetup
					SET		StartDate = @Start,	EndDate = @End,
							SpecUSL = @SpecUSL, SpecLSL = @SpecLSL, 
							XBarUCL = @XBarUCL, XBarLCL = @XBarLCL, --_XBarCL	= @XBarCL,	
							CPCL	= @CPCL,	CPUCL	= @CPUCL,	CPLCL	= @CPLCL,
							RCL		= @RCL,		RLCL	= @RLCL,	RUCL	= @RUCL,
							UpdateDate	= GETDATE(),UpdateUser = @User, Remark = @Remark
					Where	FactoryCode = @Factory	and ItemTypeCode	= @ItemType and 
							LineCode	= @Line		and ItemCheckCode	= @ItemCheck and 
							Format(StartDate, 'yyyy-MM-dd') = @StartOld and Format(EndDate, 'yyyy-MM-dd') = @EndOld
				End
		End

	IF @Type = 0
		Begin
			IF Not Exists(Select Top 1 FactoryCode From spc_ItemCheckByType Where FactoryCode = @Factory and ItemTypeCode = @ItemType and LineCode = @Line and ItemCheckCode = @ItemCheck)
				Begin
					Set @msg ='Item type ' + @ItemTypeName + ' and Item Check ' + @ItemCheckName + ' on Machine ' + @LineName + ' at ' + @FactoryName + ' Not Registered in Screen A02-Item Check by Battery Type'
					Raiserror(@msg,16,1)
				End
			Else If Exists
			(
				Select	Top 1 FactoryCode From spc_ChartSetup 
				Where	FactoryCode = @Factory and ItemTypeCode = @ItemType 
					and LineCode = @Line and ItemCheckCode = @ItemCheck
					and	(
						 @Start between StartDate and EndDate or 
						 @end between StartDate and EndDate or
						 (Format(StartDate, 'yyyy-MM-dd') >= @Start And Format(EndDate, 'yyyy-MM-dd') <= @End)
						)
					and (Format(StartDate, 'yyyy-MM-dd') >= @Start)
			)
				Begin
					Set @msg = 'Cannot Insert Start ' + @Start + ' - ' +@End + ' in Factory ' + @FactoryName + ' Machine ' + @LineName + ' Item Type ' + @ItemTypeName + ' , Because it is Overlapping with Existing Data. Please Check Again'
					Raiserror(@msg,16,1)
				End
			Else
				Begin
					Declare @StartDateEnd As Varchar(10)= 
					(Select Top 1 Format(StartDate,'yyyy-MM-dd') 
					 From spc_ChartSetup 
					 Where FactoryCode = @Factory and ItemTypeCode = @ItemType and LineCode	= @Line	and ItemCheckCode = @ItemCheck and Format(StartDate,'yyyy-MM-dd') >= @Start --and Format(EndDate,'yyyy-MM-dd') = '9999-12-31'
					 Order By StartDate Desc)

					 IF @StartDateEnd IS NULL
						Begin
							SET @StartDateEnd = 
							(Select Top 1 Format(StartDate,'yyyy-MM-dd') 
							 From spc_ChartSetup 
							 Where FactoryCode = @Factory and ItemTypeCode = @ItemType and LineCode	= @Line	and ItemCheckCode = @ItemCheck --and Format(StartDate,'yyyy-MM-dd') >= @Start --and Format(EndDate,'yyyy-MM-dd') = '9999-12-31'
							 Order By StartDate Desc)
						End
					
					Insert Into spc_ChartSetup Values
					(
						@Factory, @ItemType,@Line, @ItemCheck, @Start, @End, 
						@SpecUSL, @SpecLSL,
						0,  @XBarUCL,	@XBarLCL, --@XBarCL
						@CPCL,	  @CPUCL,	@CPLCL,
						@RCL,	  @RLCL,	@RUCL,
						@User,	  GETDATE(), @User, GETDATE(), @Remark
					)
					
					IF @StartDateEnd IS Not NULL
						Begin
							Update	spc_ChartSetup
							Set		EndDate = DateAdd(Day,-1,Cast(@Start As datetime))
							Where	FactoryCode = @Factory and ItemTypeCode = @ItemType and LineCode = @Line and ItemCheckCode = @ItemCheck
								And	Format(StartDate, 'yyyy-MM-dd') = @StartDateEnd
						End
				End
		End
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Sel]    Script Date: 12/15/2022 02:31:23 PM ******/
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
	and @Period Between Format(a.StartDate,'yyyy-MM-dd') and Format(a.EndDate,'yyyy-MM-dd')
END
GO
