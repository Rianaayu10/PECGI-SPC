GO
/****** Object:  StoredProcedure [dbo].[sp_SPC_MS_FrequencySetting_Check]    Script Date: 02/02/2023 03:04:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		: <Fikr>
-- Create date	: <2022-08-05>
-- Description	: <Time/Frequency Setting>
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_MS_FrequencySetting_Check]
	@Frequency	As Varchar(2),
	@Shift		As Varchar(25),
	@Start		As Varchar(5),
	@End		As Varchar(5),
	@SequenceNo As Int
As
BEGIN
	--Declare @date as varchar(10) = '2000-01-01'
	--Declare @startTime as varchar(25) = @date + ' ' + @start
	--Declare @endTime as varchar(25) = @date + ' ' + @End

	--if exists
	--(
	--	select Top 1 FrequencyCode from spc_MS_Frequency where FrequencyCode = @Frequency and SequenceNo <> @SequenceNo --and ShiftCode = @Shift 
	--	and 
	--	(
	--		@startTime between StartTime and EndTime or 
	--		@endTime between StartTime and EndTime or
	--		(
	--			cast(@date + ' ' + Format(cast(StartTime as datetime), 'HH:mm') as datetime) >= @startTime 
	--			And 
	--			Case 
	--				When StartTime > EndTime And ShiftCode in ('SH002','SH003') Then dateadd(day,1,cast(@date + ' ' + Format(cast(EndTime as datetime), 'HH:mm') as datetime))
	--				Else cast(@date + ' ' + Format(cast(EndTime as datetime), 'HH:mm') as datetime)
	--			End
	--			<= @endTime
	--			--cast(@date + ' ' + Format(cast(EndTime as datetime), 'HH:mm') as datetime) <= @endTime
	--		)
	--		--(cast(@date + ' ' + Format(cast(StartTime as datetime), 'HH:mm') as datetime) >= @startTime And 
	--		-- cast(@date + ' ' + Format(cast(EndTime as datetime), 'HH:mm') as datetime) <= @endTime)
	--	)
	--) 
	--Begin
	--	Declare @msg as varchar(max) = 'Time Frequency for ' + @Start + ' - ' + @End + ' Already Exists'
	--	Raiserror(@msg,16,1) 
	--End
	select 0
END

