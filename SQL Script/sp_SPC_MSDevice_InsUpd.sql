/****** Object:  StoredProcedure [dbo].[sp_SPC_MSDevice_InsUpd]    Script Date: 02/03/2023 04:19:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-09-07>
-- Description:	<Master Device>
-- =============================================

CREATE PROCEDURE [dbo].[sp_SPC_MSDevice_InsUpd]
	@FactoryCode		AS varchar(25),
	@RegistrationNo		AS varchar(25),
	@Description		AS varchar(50),
	@ToolName			AS varchar(50),
	@ToolFunction		AS varchar(50),
	@Port				AS varchar(10),
	@BaudRate			AS int		= NULL,
	@DataBits			AS int		= NULL,
	@Parity				AS char(1)	= NULL,
	@StopBits			AS int		= NULL,
	@StableCondition	AS int		= NULL,
	@PassiveActiveCls	AS char(1)	= NULL,
	@GetResultData		AS int		= NULL,
	@ActiveStatus		AS char(1),
	@EnableRTS			AS char(1),
	@User				As varchar(50)
As
BEGIN
	If Exists(Select Top 1 RegistrationNo From spc_MS_Device Where FactoryCode = @FactoryCode and RegistrationNo = @RegistrationNo)
		Begin
			Update	spc_MS_Device
			SET		Description		= @Description,
					ToolName		= @ToolName,
					ToolFunction	= @ToolFunction,
					Port			= @Port,
					BaudRate		= @BaudRate,
					DataBits		= @DataBits,
					Parity			= @Parity,
					StopBits		= @StopBits,
					StableCondition = @StableCondition,
					PassiveActiveCls= @PassiveActiveCls,
					GetResultData	= @GetResultData,
					ActiveStatus	= @ActiveStatus,
					EnableRTS		= @EnableRTS,
					UpdateUser		= @User,
					UpdateDate		= GETDATE()
			Where	FactoryCode		= @FactoryCode 
				AND RegistrationNo	= @RegistrationNo
		End
	Else
		Begin
			Insert Into spc_MS_Device
			(FactoryCode, RegistrationNo, Description, ToolName ,ToolFunction ,Port ,BaudRate ,DataBits ,Parity ,StopBits ,StableCondition ,PassiveActiveCls ,GetResultData ,ActiveStatus ,EnableRTS ,RegisterUser ,RegisterDate)
			VALUES
			(@FactoryCode, @RegistrationNo, @Description, @ToolName, @ToolFunction, @Port, @BaudRate, @DataBits, @Parity, @StopBits, @StableCondition, @PassiveActiveCls, @GetResultData, @ActiveStatus, @EnableRTS, @User, GETDATE())
		End
END
GO
