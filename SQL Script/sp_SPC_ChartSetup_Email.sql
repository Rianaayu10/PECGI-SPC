/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Email]    Script Date: 01/26/2023 04:30:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-05>
-- Description:	<X Bar - R Control Chart System Setup>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SPC_ChartSetup_Email]
	@Factory	As varchar(25) = 'F001',
	@ItemType	As varchar(25) = 'TPMSBR011',
	@Line		As varchar(5)  = '005',
	@ItemCheck	As varchar(15) = 'IC001',
	@Start		As varchar(10) = '2023-01-01',	
	@StartOld	As varchar(10) = '2000-01-01',	
	@End		As varchar(10) = '9999-01-14',
	@EndOld		As varchar(10) = '9999-01-14',	
	@SpecUSL	As Numeric(10,3) = '1',	 @SpecUSLOld	As Numeric(10,3) = '1',
	@SpecLSL	As Numeric(10,3) = '2',  @SpecLSLOld	As Numeric(10,3) = '2',
	--@XBarCL	As Numeric(10,3) = '3',  @XBarCLOld		As Numeric(10,3) = '3',
	@XBarUCL	As Numeric(10,3) = '3',  @XBarUCLOld	As Numeric(10,3) = '3',
	@XBarLCL	As Numeric(10,3) = '4',  @XBarLCLOld	As Numeric(10,3) = '4',
	@CPCL		As Numeric(10,3) = '5',  @CPCLOld		As Numeric(10,3) = '5',
	@CPUCL		As Numeric(10,3) = '6',  @CPUCLOld		As Numeric(10,3) = '6',
	@CPLCL		As Numeric(10,3) = '7',  @CPLCLOld		As Numeric(10,3) = '7',
	@RCL		As Numeric(10,3) = '8',  @RCLOld		As Numeric(10,3) = '8', -- not Used
	@RLCL		As Numeric(10,3) = '9',  @RLCLOld		As Numeric(10,3) = '9', -- not Used
	@RUCL		As Numeric(10,3) = '10', @RUCLOld		As Numeric(10,3) = '10',
	@Remark		As varchar(50)	 = 'Ts', @RemarkOld		As varchar(50)	 = 'Tes Old',
	@User		As varchar(50)	 = 'admin',
	@To			as varchar(Max) = '',
	@CC			as varchar(Max) = '',
	@Type		As char(1) = 1 --0 Save | 1 Update
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

	--0 Not Different || 1 Different
	Declare	@diffStart	 as Char(1) = Case When Format(Cast(@StartOld	as date),'dd MMM yyyy') = Format(Cast(@Start as date),'dd MMM yyyy') then '0' Else '1' End
	Declare	@diffEnd	 as Char(1) = Case When Format(Cast(@EndOld		as date),'dd MMM yyyy') = Format(Cast(@End	 as date),'dd MMM yyyy') then '0' Else '1' End
	Declare	@diffSpecUSL as Char(1) = Case When cast(@SpecUSLOld as varchar(100))	= cast(@SpecUSL as varchar(100)) then '0' Else '1' End
	Declare	@diffSpecLSL as Char(1) = Case When cast(@SpecLSLOld as varchar(100))	= cast(@SpecLSL as varchar(100)) then '0' Else '1' End
	Declare	@diffXBarUCL as Char(1) = Case When cast(@XBarUCLOld as varchar(100))	= cast(@XBarUCL as varchar(100)) then '0' Else '1' End
	Declare	@diffXBarLCL as Char(1) = Case When cast(@XBarLCLOld as varchar(100))	= cast(@XBarLCL as varchar(100)) then '0' Else '1' End
	Declare	@diffCPCL	 as Char(1) = Case When cast(@CPCLOld	 as varchar(100))	= cast(@CPCL	as varchar(100)) then '0' Else '1' End
	Declare	@diffCPUCL	 as Char(1) = Case When cast(@CPUCLOld	 as varchar(100))	= cast(@CPUCL	as varchar(100)) then '0' Else '1' End
	Declare	@diffCPLCL	 as Char(1) = Case When cast(@CPLCLOld	 as varchar(100))	= cast(@CPLCL	as varchar(100)) then '0' Else '1' End	
	Declare	@diffRUCL	 as Char(1) = Case When cast(@RUCLOld	 as varchar(100))	= cast(@RUCL	as varchar(100)) then '0' Else '1' End
	Declare	@diffRemark	 as Char(1) = Case When cast(@RemarkOld	 as varchar(100))	= cast(@Remark	as varchar(100)) then '0' Else '1' End


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
	<head>
		<style>
			.highlight 
			{
				background-color: yellow;
				font-weight: bold;
			}
		</style>
    </head>

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
			<td>'+ Format(Cast(@Start as date),'dd MMM yyyy') +'</td>
		</tr>

		<tr>
			<td>End</td>
			<td> : </td>
			<td>'+ Format(Cast(@End as date),'dd MMM yyyy') +'</td>
		</tr>

		<tr>
			<td>Spec USL</td>
			<td> : </td>
			<td>'+ cast(@SpecUSL as varchar(100)) +'</td>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>
			<td>'+ cast(@SpecLSL as varchar(100)) +'</td>
		</tr>
		
		<tr>
			<td>Control Plan CL</td>
			<td> : </td>
			<td>'+ cast(@CPCL as varchar(100)) +'</td>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>
			<td>'+ cast(@CPUCL as varchar(100)) +'</td>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>
			<td>'+ cast(@CPLCL as varchar(100)) +'</td>
		</tr>		

		<tr>
			<td>X Bar UCL</td>
			<td> : </td>
			<td>'+ cast(@XBarUCL as varchar(100)) +'</td>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>
			<td>'+ cast(@XBarLCL as varchar(100)) +'</td>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>
			<td>'+ cast(@RUCL as varchar) +'</td>
		</tr>

		<tr>
			<td>Remark</td>
			<td> : </td>
			<td>'+ @Remark +'</td>
		</tr>

		<tr>
			<td>Create User</td>
			<td> : </td>
			<td>'+ @UserFullName +'</td>
		</tr>

		<tr>
			<td>Create Date</td>
			<td> : </td>
			<td>'+ Format(@GetDate,'dd MMM yyyy HH:mm:ss') +'</td>
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
			<td>'+
				case 
					when @diffStart = 0 then ''
					Else '<span class="highlight">'
				End+
			Format(Cast(@StartOld as date),'dd MMM yyyy') + 
				case 
					when @diffStart = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>End</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffEnd = 0 then ''
					Else '<span class="highlight">'
				End+
			Format(Cast(@EndOld as date),'dd MMM yyyy') + 
				case 
					when @diffEnd = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Spec USL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffSpecUSL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@SpecUSLOld as varchar(100)) + 
				case 
					when @diffSpecUSL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffSpecLSL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@SpecLSLOld as varchar(100)) + 
				case 
					when @diffSpecLSL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Control Plan CL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffCPCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@CPCLOld as varchar(100)) + 
				case 
					when @diffCPCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffCPUCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@CPUCLOld as varchar(100)) + 
				case 
					when @diffCPUCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffCPLCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@CPLCLOld as varchar(100)) + 
				case 
					when @diffCPLCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>
		
		<tr>
			<td>X Bar UCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffXBarUCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@XBarUCLOld as varchar(100)) + 
				case 
					when @diffXBarUCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffXBarLCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@XBarLCLOld as varchar(100)) + 
				case 
					when @diffXBarLCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffRUCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@RUCLOld as varchar(100)) + 
				case 
					when @diffRUCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Remark</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffRemark = 0 then ''
					Else '<span class="highlight">'
				End
			+ @RemarkOld + 
				case 
					when @diffRemark = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>
	</table>
	<BR>

	<p style="font-weight: bold;font-style: italic;">After</p>

	<table>
		<tr>
			<td>Start</td>
			<td> : </td>
			<td>'+
				case 
					when @diffStart = 0 then ''
					Else '<span class="highlight">'
				End+
			Format(Cast(@Start as date),'dd MMM yyyy') + 
				case 
					when @diffStart = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>End</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffEnd = 0 then ''
					Else '<span class="highlight">'
				End+
			Format(Cast(@End as date),'dd MMM yyyy') + 
				case 
					when @diffEnd = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Spec USL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffSpecUSL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@SpecUSL as varchar(100)) + 
				case 
					when @diffSpecUSL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Spec LSL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffSpecLSL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@SpecLSL as varchar(100)) + 
				case 
					when @diffSpecLSL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Control Plan CL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffCPCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@CPCL as varchar(100)) + 
				case 
					when @diffCPCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Control Plan UCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffCPUCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@CPUCL as varchar(100)) + 
				case 
					when @diffCPUCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Control Plan LCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffCPLCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@CPLCL as varchar(100)) + 
				case 
					when @diffCPLCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>
		
		<tr>
			<td>X Bar UCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffXBarUCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@XBarUCL as varchar(100)) + 
				case 
					when @diffXBarUCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>X Bar LCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffXBarLCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@XBarLCL as varchar(100)) + 
				case 
					when @diffXBarLCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>R UCL</td>
			<td> : </td>			
			<td>'+
				case 
					when @diffRUCL = 0 then ''
					Else '<span class="highlight">'
				End
			+ cast(@RUCL as varchar(100)) + 
				case 
					when @diffRUCL = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Remark</td>
			<td> : </td>
			<td>'+
				case 
					when @diffRemark = 0 then ''
					Else '<span class="highlight">'
				End
			+ @Remark + 
				case 
					when @diffRemark = 0 then ''
					Else '</span>'
				End+
			'</td>
		</tr>

		<tr>
			<td>Update User</td>
			<td> : </td>
			<td>'+ @UserFullName +'</td>
		</tr>

		<tr>
			<td>Update Date</td>
			<td> : </td>
			<td>'+ Format(@GetDate,'dd MMM yyyy HH:mm:ss') +'</td>
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
