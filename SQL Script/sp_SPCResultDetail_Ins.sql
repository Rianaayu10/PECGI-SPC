/****** Object:  StoredProcedure [dbo].[sp_SPCResultDetail_Ins]    Script Date: 12/01/2023 08:54:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ari
-- Create date: 18-Aug-2022
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[sp_SPCResultDetail_Ins]
	@SPCResultID int, 
	@SequenceNo numeric(2, 0), 
	@Value numeric(10, 3),
	@Remark varchar(50) = '', 
	@DeleteStatus char(1) = 0, 
	@RegisterUser varchar(50) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	delete spc_ResultDetail where SPCResultID = @SPCResultID and SequenceNo = @SequenceNo
	insert spc_ResultDetail (
		SPCResultID, SequenceNo, Value, Remark, DeleteStatus, RegisterUser, RegisterDate
	) values (
		@SPCResultID, @SequenceNo, @Value, @Remark, @DeleteStatus, @RegisterUser, GetDate()
	)

	declare @SampleSize as integer = (
		select top 1 I.SampleSize from spc_ItemCheckByType I inner join spc_Result R 
		on I.FactoryCode = R.FactoryCode and I.ItemTypeCode = R.ItemTypeCode and I.LineCode = R.LineCode and I.ItemCheckCode = R.ItemCheckCode		
		where R.SPCResultID = @SPCResultID
	)

	declare @ActualSample as integer = (
		select count(*) from vw_SPCResultDetailOK where SPCResultID = @SPCResultID 
	)

	declare @CompleteStatus char(1) = NULL
	if @ActualSample >= @SampleSize 
		set @CompleteStatus = '1'
	else
		set @CompleteStatus = NULL

	update spc_Result set CompleteStatus = @CompleteStatus where SPCResultID = @SPCResultID

	if @CompleteStatus = '1'
	begin
		declare @FactoryCode varchar(15) = (select top 1 FactoryCode from spc_Result where SPCResultID = @SPCResultID),
		@ItemTypeCode varchar(25) = (select top 1 ItemTypeCode from spc_Result where SPCResultID = @SPCResultID),
		@LineCode varchar(15) = (select top 1 LineCode from spc_Result where SPCResultID = @SPCResultID),
		@ItemCheckCode varchar(15) = (select top 1 ItemCheckCode from spc_Result where SPCResultID = @SPCResultID),
		@ProdDate date = (select top 1 ProdDate from spc_Result where SPCResultID = @SPCResultID),
		@ShiftCode varchar(15) = (select top 1 ShiftCode from spc_Result where SPCResultID = @SPCResultID),
		@ResultSeqNo varchar(25) = (select top 1 SequenceNo from spc_Result where SPCResultID = @SPCResultID)
	end

END
