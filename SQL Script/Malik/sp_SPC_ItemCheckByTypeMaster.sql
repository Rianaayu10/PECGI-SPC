/****** Object:  StoredProcedure [dbo].[sp_SPC_ItemCheckByTypeMaster]    Script Date: 5/5/2023 9:00:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==================================================
-- Author:		Malik Ilman Nafia
-- Create date: 20-Jan-2022
-- Description:	SP All Function In Item Check By Type
-- ==================================================

ALTER   PROCEDURE [dbo].[sp_SPC_ItemCheckByTypeMaster]
	@FactoryCode varchar(50) = '',
	@ItemTypeCode varchar(50) = '',
	@LineCode varchar(50) = '',
	@ItemCheckCode varchar(50) = '',
	@Machine varchar(50) = '',
	@ItemCheck varchar(50) = '',
	@FrequencyCode varchar(50) = '',
	@RegistrationNo varchar(50) = '',
    @SampleSize varchar(50) = '',
	@Remark varchar(50) = '',
	@Evaluation varchar(50) = '',
	@CharacteristicItem varchar(50) = '',
	@ProcessTableLineCode varchar(50) = '',
	@FTARatio numeric(5, 2) = 0,
	@StationID numeric(5, 2) = 0,
	@PrevItemCheck char(15) = '',
	@PrevValue int = 1,
	@ActiveStatus varchar(50) = '',
	@CreateUser varchar(50) = '',
	@UpdateUser varchar(50) = '',
	@TypeProcess integer = 0 -- 1 = GetData, 2 = GetMachineProccess, 3 = GetItemCheckMaster, 4 = ValidationDelete, 5 = GetListItemType, 6 = Insert Data, 7 = Delete Data, 8 = Update data
As
BEGIN

	If @TypeProcess = 1
		BEGIN
			select ICT.*, ItemTypeName = IT.Description, LineName = MSL.LineCode + ' - ' + MSL.LineName , ItemCheck = ICM.ItemCheckCode + ' - ' + ICM.ItemCheck
			from spc_ItemCheckByType ICT 
			inner join MS_ItemType IT ON ICT.ItemTypeCode = IT.ItemTypeCode 
			LEFT JOIN MS_Line MSL ON ICT.FactoryCode = MSL.FactoryCode AND ICT.LineCode = MSL.LineCode  
			LEFT JOIN spc_ItemCheckMaster ICM ON ICT.ItemCheckCode = ICM.ItemCheckCode 
			where ICT.FactoryCode = @FactoryCode and ICT.ItemTypeCode = @ItemTypeCode and ICT.LineCode = @LineCode and ICT.ItemCheckCode = @ItemCheckCode
		END
	Else If @TypeProcess = 2 
		BEGIN
			If @FactoryCode <> '' 
				BEGIN
					select distinct Number = 2, L.FactoryCode, L.ProcessCode, L.LineCode, L.LineCode + ' - ' + L.LineName as LineName from MS_Line L
					where L.FactoryCode = @FactoryCode AND L.ProcessCode = @Machine
				END
			ELSE
				BEGIN
					select distinct Number = 2, L.FactoryCode, L.ProcessCode, L.LineCode, L.LineCode + ' - ' + L.LineName as LineName from MS_Line L
				END
		END
	Else If @TypeProcess = 3
		BEGIN
			SELECT 'ALL' ItemCheckCode, 'ALL' ItemCheck UNION SELECT  ItemCheckCode, ItemCheck from spc_ItemCheckMaster
		END
	Else If @TypeProcess = 4
		BEGIN
			select top 1 * from spc_Result where FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheckCode
		END
	Else If @TypeProcess = 5
		BEGIN
			select ItemTypeCode, Description from MS_ItemType order by Description ASC
		END
	Else If @TypeProcess = 6
		BEGIN
			INSERT INTO spc_ItemCheckByType 
            VALUES ( @FactoryCode, @ItemTypeCode, @LineCode, @ItemCheck, @FrequencyCode, @RegistrationNo,
            @SampleSize, @Remark, @Evaluation, @CharacteristicItem, @ProcessTableLineCode, @FTARatio, @StationID, @PrevItemCheck, @PrevValue, @ActiveStatus, @CreateUser, GETDATE(), @CreateUser, GETDATE() ) 
		END
	Else If @TypeProcess = 7
		BEGIN
			Delete from spc_ItemCheckByType WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheck
		END
	Else If @TypeProcess = 8
		BEGIN
			UPDATE spc_ItemCheckByType SET FrequencyCode = @FrequencyCode, RegistrationNo = @RegistrationNo, SampleSize = @SampleSize, Remark = @Remark, 
               Evaluation = @Evaluation, CharacteristicStatus = @CharacteristicItem, ProcessTableLineCode = @ProcessTableLineCode, FTARatio = @FTARatio, StationID = @StationID, PrevItemCheck = @PrevItemCheck, PrevValue = @PrevValue, ActiveStatus = @ActiveStatus, UpdateUser = @UpdateUser, UpdateDate = GETDATE() 
               WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode and ItemCheckCode = @ItemCheck 
		END
	
END
