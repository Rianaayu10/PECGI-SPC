USE [Panasonic_Dashboard_CR_UAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Email]    Script Date: 12/29/2022 12:49:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	Declare @GetDate		as datetime		= (select Top 1 UpdateDate from spc_ChartSetup Where FactoryCode = @Factory and LineCode = @Line and ItemTypeCode = @ItemType and ItemCheckCode = @ItemCheck and StartDate = @Start)
	Declare @UserFullName	as Varchar(max) = (select Top 1 Trim(FullName) from spc_UserSetup Where UserID = @User)

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

		<tr>
			<td>Create User</td>
			<td> : </td>
			<td>'+ @UserFullName +'</>
		</tr>

		<tr>
			<td>Create Date</td>
			<td> : </td>
			<td>'+ Format(@GetDate,'dd MMM yyyy HH:mm:ss') +'</>
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

		<tr>
			<td>Update User</td>
			<td> : </td>
			<td>'+ @UserFullName +'</>
		</tr>

		<tr>
			<td>Update Date</td>
			<td> : </td>
			<td>'+ Format(@GetDate,'dd MMM yyyy HH:mm:ss') +'</>
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

