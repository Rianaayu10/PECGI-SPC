/****** Object:  StoredProcedure [dbo].[sp_SPC_MSDevice_Sel]    Script Date: 02/03/2023 04:21:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-09-07>
-- Description:	<Master Device>
-- =============================================

CREATE PROCEDURE [dbo].[sp_SPC_MSDevice_Sel]
	@Factory	As varchar(25)
As
BEGIN
	select 
		FactoryCode = a.FactoryCode,
		FactoryDesc = b.FactoryName,
		RegNo		= a.RegistrationNo,
		Description = a.Description,
		ToolName	= a.ToolName,
		ToolFunction= a.ToolFunction,
		Port		= ISNULL(a.Port,''),
		BaudRate	= ISNULL(Cast(a.BaudRate As Varchar(Max)),''),
		DataBits	= ISNULL(Cast(a.DataBits As Varchar(Max)),''),
		Parity		= ISNULL(Cast(a.Parity As Varchar(Max)),''),
		StopBits	= ISNULL(Cast(a.StopBits As Varchar(Max)),''),
		Stable		= ISNULL(Cast(a.StableCondition As Varchar(Max)),''),
		Passive		= ISNULL(Cast(a.PassiveActiveCls As Varchar(Max)),''),
		GetResult	= ISNULL(Cast(a.GetResultData As Varchar(Max)),''),
		ActiveStatus= a.ActiveStatus,
		EnableRTS	= ISNULL(a.EnableRTS,0),
		LastUser	= c.FullName,
		LastUpdate	= Format(ISNULL(a.UpdateDate, a.RegisterDate),'dd MMM yyyy HH:mm:ss')
	from 
		spc_MS_Device a Left Join
		MS_Factory b on a.FactoryCode = b.FactoryCode Left Join
		spc_UserSetup c on c.UserID = ISNULL(a.UpdateUser, a.RegisterUser)
	Where
		a.FactoryCode = @Factory
END
GO
