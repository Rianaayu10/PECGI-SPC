/****** Object:  StoredProcedure [dbo].[sp_SPC_MSDevice_Sel]    Script Date: 01/26/2023 04:32:17 PM ******/
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
		RegNo		= RegistrationNo,
		Description = Description,
		ToolName	= ToolName,
		ToolFunction= ToolFunction,
		Port		= ISNULL(Port,''),
		BaudRate	= ISNULL(Cast(BaudRate As Varchar(Max)),''),
		DataBits	= ISNULL(Cast(DataBits As Varchar(Max)),''),
		Parity		= ISNULL(Cast(Parity As Varchar(Max)),''),
		StopBits	= ISNULL(Cast(StopBits As Varchar(Max)),''),
		Stable		= ISNULL(Cast(StableCondition As Varchar(Max)),''),
		Passive		= ISNULL(Cast(PassiveActiveCls As Varchar(Max)),''),
		GetResult	= ISNULL(Cast(GetResultData As Varchar(Max)),''),
		ActiveStatus= ActiveStatus,
		LastUser	= ISNULL(a.UpdateUser, a.RegisterUser),
		LastUpdate	= Format(ISNULL(a.UpdateDate, a.RegisterDate),'dd MMM yyyy HH:mm:ss')
	from 
		spc_MS_Device a Left Join
		MS_Factory b on a.FactoryCode = b.FactoryCode
	Where
		a.FactoryCode = @Factory
END
GO
