SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ari
-- Create date: 15-Dec-2022
-- Description:	
-- =============================================
CREATE OR ALTER PROCEDURE sp_SPC_FillCombo
	@ComboType int = 0, -- 0:ProcessGroup 1:LineGroup 2:Process 3:Line
	@UserID varchar(50) = 'admin',
	@FactoryCode varchar(15) = 'F001',
	@ProcessGroup varchar(15) = '',
	@LineGroup varchar(15) = '',
	@ProcessCode varchar(15) = ''
AS
BEGIN

	SET NOCOUNT ON;

	if @ComboType = 0 
		select distinct PG.FactoryCode, PG.ProcessGroup, PG.ProcessGroupName
		from MS_ProcessGroup PG inner join MS_LineGroup LG on PG.FactoryCode = LG.FactoryCode and PG.ProcessGroup = LG.ProcessGroup
		inner join MS_Process P on LG.FactoryCode = P.FactoryCode and LG.ProcessGroup = P.ProcessGroup and LG.LineGroup = P.LineGroup
		inner join MS_Line L on P.FactoryCode = L.FactoryCode and P.ProcessCode = L.ProcessCode
		inner join spc_ItemCheckByType I on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode
		inner join spc_UserLine UL on L.LineCode = UL.LineCode
		where UL.UserID = @UserID and UL.AllowShow = 1 and I.ActiveStatus = 1
		and PG.FactoryCode = case when @FactoryCode = '' then PG.FactoryCode else @FactoryCode end
		and LG.LineGroup = case when @LineGroup = '' then LG.LineGroup else @LineGroup end
		and P.ProcessCode = case when @ProcessCode = '' then P.ProcessCode else @ProcessCode end
	else if @ComboType = 1
		select distinct PG.FactoryCode, PG.ProcessGroup, LG.LineGroup, LG.LineGroupName
		from MS_ProcessGroup PG inner join MS_LineGroup LG on PG.FactoryCode = LG.FactoryCode and PG.ProcessGroup = LG.ProcessGroup
		inner join MS_Process P on LG.FactoryCode = P.FactoryCode and LG.ProcessGroup = P.ProcessGroup and LG.LineGroup = P.LineGroup
		inner join MS_Line L on P.FactoryCode = L.FactoryCode and P.ProcessCode = L.ProcessCode
		inner join spc_ItemCheckByType I on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode
		inner join spc_UserLine UL on L.LineCode = UL.LineCode
		where UL.UserID = @UserID and UL.AllowShow = 1 and I.ActiveStatus = 1
		and PG.FactoryCode = case when @FactoryCode = '' then PG.FactoryCode else @FactoryCode end
		and LG.LineGroup = case when @LineGroup = '' then LG.LineGroup else @LineGroup end
		and P.ProcessCode = case when @ProcessCode = '' then P.ProcessCode else @ProcessCode end
	else if @ComboType = 2
		select distinct PG.FactoryCode, PG.ProcessGroup, LG.LineGroup, P.ProcessCode, P.ProcessName
		from MS_ProcessGroup PG inner join MS_LineGroup LG on PG.FactoryCode = LG.FactoryCode and PG.ProcessGroup = LG.ProcessGroup
		inner join MS_Process P on LG.FactoryCode = P.FactoryCode and LG.ProcessGroup = P.ProcessGroup and LG.LineGroup = P.LineGroup
		inner join MS_Line L on P.FactoryCode = L.FactoryCode and P.ProcessCode = L.ProcessCode
		inner join spc_ItemCheckByType I on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode
		inner join spc_UserLine UL on L.LineCode = UL.LineCode
		where UL.UserID = @UserID and UL.AllowShow = 1 and I.ActiveStatus = 1
		and PG.FactoryCode = case when @FactoryCode = '' then PG.FactoryCode else @FactoryCode end
		and LG.LineGroup = case when @LineGroup = '' then LG.LineGroup else @LineGroup end
		and P.ProcessCode = case when @ProcessCode = '' then P.ProcessCode else @ProcessCode end
	else if @ComboType = 3
		select distinct PG.FactoryCode, PG.ProcessGroup, LG.LineGroup, P.ProcessCode, L.LineCode, L.LineName
		from MS_ProcessGroup PG inner join MS_LineGroup LG on PG.FactoryCode = LG.FactoryCode and PG.ProcessGroup = LG.ProcessGroup
		inner join MS_Process P on LG.FactoryCode = P.FactoryCode and LG.ProcessGroup = P.ProcessGroup and LG.LineGroup = P.LineGroup
		inner join MS_Line L on P.FactoryCode = L.FactoryCode and P.ProcessCode = L.ProcessCode
		inner join spc_ItemCheckByType I on L.FactoryCode = I.FactoryCode and L.LineCode = I.LineCode
		inner join spc_UserLine UL on L.LineCode = UL.LineCode
		where UL.UserID = @UserID and UL.AllowShow = 1 and I.ActiveStatus = 1
		and PG.FactoryCode = case when @FactoryCode = '' then PG.FactoryCode else @FactoryCode end
		and LG.LineGroup = case when @LineGroup = '' then LG.LineGroup else @LineGroup end
		and P.ProcessCode = case when @ProcessCode = '' then P.ProcessCode else @ProcessCode end

END
GO
