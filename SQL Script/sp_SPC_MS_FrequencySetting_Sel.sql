/****** Object:  StoredProcedure [dbo].[sp_SPC_MS_FrequencySetting_Sel]    Script Date: 01/17/2023 09:43:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		: <Fikr>
-- Create date	: <2022-08-05>
-- Description	: <Time/Frequency Setting>
-- Example		: exec sp_MS_FrequencySetting_Sel '01'
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_MS_FrequencySetting_Sel]
	@Frequency As Varchar(2)
As
BEGIN
	Select 
		Frequency	= a.FrequencyCode ,
		[No]		= a.SequenceNo,
		[Shift]		= a.ShiftCode,
		[Start]		= cast(a.StartTime as Datetime),
		[End]		= cast(a.EndTime as Datetime),
		[Verif]		= cast(a.VerificationTime as Datetime),
		[Status]	= a.ActiveStatus,
		LastUser	= ISNULL(b.FullName,a.UpdateUser),
		LastUpdate	= Format(a.UpdateDate,'dd MMM yyyy HH:mm:ss')
	from 
		spc_MS_Frequency a Left Join
		spc_UserSetup b on a.UpdateUser = b.UserID
	Where
		a.FrequencyCode = @Frequency
END
GO
