/****** Object:  StoredProcedure [dbo].[sp_SPCResultDetail_Upd]    Script Date: 06/04/2023 11:08:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ari
-- Create date: 
-- 
-- =============================================
ALTER PROCEDURE [dbo].[sp_SPCResultDetail_Upd]
	@SPCResultID int, 
	@SequenceNo numeric(2, 0) = 0, 
	@Remark varchar(50) = '', 
	@DeleteStatus char(1) = 0, 
	@UpdateUser varchar(50) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	Update spc_ResultDetail set Remark = @Remark, DeleteStatus = @DeleteStatus, UpdateDate = GetDate(), UpdateUser = @UpdateUser
	where SPCResultID = @SPCResultID and SequenceNo = @SequenceNo

	declare @SampleSize as integer = (
		select top 1 I.SampleSize from spc_ItemCheckByType I inner join spc_Result R 
		on I.FactoryCode = R.FactoryCode and I.ItemTypeCode = R.ItemTypeCode and I.LineCode = R.LineCode and I.ItemCheckCode = R.ItemCheckCode		
		where R.SPCResultID = @SPCResultID
	)

	declare @ActualSample as integer = (
		select count(*) from vw_SPCResultDetailOK where SPCResultID = @SPCResultID and Result = '1'
	)

	declare @CompleteStatus char(1) = NULL
	if @ActualSample >= @SampleSize 
		set @CompleteStatus = '1'
	else
		set @CompleteStatus = NULL

	update spc_Result set CompleteStatus = @CompleteStatus where SPCResultID = @SPCResultID

	if exists (select * from vw_SPCResultRatioNG where SPCResultID = @SPCResultID and RatioNG >= FTARatio)
		update spc_Result set FTAStatus = 1 where SPCResultID = @SPCResultID
	else
		update spc_Result set FTAStatus = NULL where SPCResultID = @SPCResultID

	if @CompleteStatus = '1'
	begin
		update spc_Result set MKVerificationStatus = 1, MKVerificationDate = GetDate(), MKVerificationUser = '-' where SPCResultID = @SPCResultID and FTAStatus is Null
		update spc_Result set QCVerificationStatus = 1, QCVerificationDate = GetDate(), QCVerificationUser = '-' where SPCResultID = @SPCResultID and FTAStatus is Null
	end
	else
	begin
		update spc_Result set MKVerificationStatus = NULL, MKVerificationDate = NULL, MKVerificationUser = NULL where SPCResultID = @SPCResultID and FTAStatus is Null
		update spc_Result set QCVerificationStatus = NULL, QCVerificationDate = NULL, QCVerificationUser = NULL where SPCResultID = @SPCResultID and FTAStatus is Null
	end
END
