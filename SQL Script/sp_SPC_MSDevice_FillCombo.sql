/****** Object:  StoredProcedure [dbo].[sp_SPC_MSDevice_FillCombo]    Script Date: 01/26/2023 04:32:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-09-07>
-- Description:	<Master Device>
-- =============================================

CREATE PROCEDURE [dbo].[sp_SPC_MSDevice_FillCombo]
	@Type	As char(1)
As
BEGIN
--For Factory
	If @Type = '0'
	Begin
		select FactoryCode Code, FactoryName [Description] from MS_Factory Order By Description
	End
	
--For Baud Rate
	If @Type = '1'
	Begin
		select '150' [Code], '150' [Description]
		UNION ALL
		select '300' [Code], '300' [Description]
		UNION ALL
		select '600' [Code], '600' [Description]
		UNION ALL
		select '1200' [Code], '1200' [Description]
		UNION ALL
		select '2400' [Code], '2400' [Description]
		UNION ALL
		select '4800' [Code], '4800' [Description]
		UNION ALL
		select '9600' [Code], '9600' [Description]
		UNION ALL
		select '19200' [Code], '19200' [Description]
		UNION ALL
		select '38400' [Code], '38400' [Description]
		UNION ALL
		select '57600' [Code], '57600' [Description]
		UNION ALL
		select '115200' [Code], '115200' [Description]
	End

--For Data Bits
	If @Type = '2'
	Begin
		select '7' [Code], '7' [Description]
		UNION ALL
		select '8' [Code], '8' [Description]
		UNION ALL
		select '9' [Code], '9' [Description]
	End

--For Parity
	If @Type = '3'
	Begin
		select '0' [Code], 'None' [Description]
		UNION ALL
		select '1' [Code], 'Odd' [Description]
		UNION ALL
		select '2' [Code], 'Even' [Description]
		--UNION ALL
		--select '3' [Code], 'Both' [Description]
	End

--For Stop Bits
	If @Type = '4'
	Begin
		select '1' [Code], '1' [Description]
		UNION ALL
		select '2' [Code], '2' [Description]
		UNION ALL
		select '3' [Code], '3' [Description]
	End

--For PassiveActive
	If @Type = '5'
	Begin
		select '0' [Code], 'Passive' [Description]
		UNION ALL
		select '1' [Code], 'Active' [Description]
	End

--For Port
	If @Type = '6'
	Begin
		Select 'USB' [Code], 'USB' [Description]
		Union
		Select Distinct Port [Code], Port [Description] From spc_MS_Device Where ISNULL(Port,'') <> ''
	End
END
GO
