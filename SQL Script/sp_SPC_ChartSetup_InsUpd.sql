/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_InsUpd]    Script Date: 12/14/2022 08:52:26 AM ******/
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
	@ItemType	As varchar(25) = 'TPMSBR009',
	@Line		As varchar(5) = '061',
	@ItemCheck	As varchar(15) = 'IC006',
	@Start		As varchar(10) = '2022-09-02',	
	@StartOld	As varchar(10) = '2022-09-26',	
	@End		As varchar(10) = '9999-12-31',
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
	@User		As varchar(50) = 'F001',
	@Type		As char(1) --0 Save | 1 Update
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
							UpdateDate	= GETDATE(),UpdateUser = @User
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
						@User,	  GETDATE(), @User, GETDATE()
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
