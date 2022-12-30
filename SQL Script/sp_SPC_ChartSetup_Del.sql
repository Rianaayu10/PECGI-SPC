USE [Panasonic_Dashboard_CR_UAT]
GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_ChartSetup_Del]    Script Date: 12/29/2022 12:50:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-08-05>
-- Description:	<X Bar - R Control Chart System Setup>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_ChartSetup_Del]
	@Factory	As varchar(25),
	@ItemType	As varchar(25),
	@Line		As varchar(5),
	@ItemCheck	As varchar(15),
	@Start		As varchar(10)
As
BEGIN
	Declare @StartTmp	As date = (select Top 1 StartDate from spc_ChartSetup Where FactoryCode = @Factory and LineCode = @Line and ItemTypeCode = @ItemType and ItemCheckCode = @ItemCheck and StartDate = @Start)
	Declare @EndTmp		As date = (select Top 1 EndDate   from spc_ChartSetup Where FactoryCode = @Factory and LineCode = @Line and ItemTypeCode = @ItemType and ItemCheckCode = @ItemCheck and StartDate = @Start)
	If Exists
	(
	 --Select Top 1 * from spc_Result a inner join spc_ChartSetup b on a.FactoryCode = b.FactoryCode and a.ItemCheckCode = b.ItemCheckCode and a.ItemTypeCode = b.ItemTypeCode and a.ProdDate between b.StartDate and b.EndDate
	 --where a.FactoryCode = @Factory and a.ItemTypeCode = @ItemType and a.LineCode = @Line and a.ItemCheckCode = @ItemCheck
	 Select Top 1 * from spc_Result a 
	 where a.FactoryCode = @Factory and a.ItemTypeCode = @ItemType and a.LineCode = @Line and a.ItemCheckCode = @ItemCheck and a.ProdDate between @StartTmp and @EndTmp
	)
		Begin
			Declare @msg as varchar(max)
			Declare @FacDesc as varchar(max)
			Declare @TypeDesc as varchar(max)
			Declare @LineDesc as varchar(max)

			Select Top 1 @FacDesc = FactoryName From MS_Factory Where FactoryCode = @Factory
			Select Top 1 @LineDesc = LineName From MS_Line Where LineCode = @Line
			Select Top 1 @TypeDesc = Description From MS_ItemType Where ItemTypeCode = @ItemType

			Set @msg = 'Cannot Delete Setup of Factory ' + @FacDesc + ' Type ' + @TypeDesc + ' Machine ' + @LineDesc + ' Because There is Already Result Data of This Setup'
			Raiserror(@msg,16,1)
		End
	Else
		Begin
			Delete	spc_ChartSetup
			Where	FactoryCode = @Factory and ItemTypeCode = @ItemType and 
					LineCode = @Line and ItemCheckCode = @ItemCheck and Format(StartDate, 'yyyy-MM-dd') = @Start
		End
END

