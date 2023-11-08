/****** Object:  StoredProcedure [dbo].[sp_SPC_MSDevice_InsUpd]    Script Date: 11/8/2023 4:56:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Fikri>
-- Create date: <2022-09-07>
-- Description:	<Master Device>
-- =============================================

ALTER   PROCEDURE [dbo].[sp_SPC_MSDevice_InsUpd]
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
	@Command			AS varchar(10) = '',
	@ActiveStatus		AS char(1),
	@EnableRTS			AS char(1),
	@EnableDTR			AS char(1),
	@FlowControl		AS char(1),
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
					Command			= @Command,
					ActiveStatus	= @ActiveStatus,
					EnableRTS		= @EnableRTS,
					EnableDTR		= @EnableDTR,
					FlowControl		= @FlowControl,
					UpdateUser		= @User,
					UpdateDate		= GETDATE()
			Where	FactoryCode		= @FactoryCode 
				AND RegistrationNo	= @RegistrationNo
		End
	Else
		Begin
			Insert Into spc_MS_Device
			(FactoryCode, RegistrationNo, Description, ToolName ,ToolFunction ,Port ,BaudRate ,DataBits ,Parity ,StopBits ,StableCondition ,PassiveActiveCls ,GetResultData ,Command ,ActiveStatus ,EnableRTS ,EnableDTR, FlowControl ,RegisterUser ,RegisterDate)
			VALUES
			(@FactoryCode, @RegistrationNo, @Description, @ToolName, @ToolFunction, @Port, @BaudRate, @DataBits, @Parity, @StopBits, @StableCondition, @PassiveActiveCls, @GetResultData, @Command, @ActiveStatus, @EnableRTS, @EnableDTR, @FlowControl, @User, GETDATE())
		End
END
