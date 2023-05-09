/****** Object:  StoredProcedure [dbo].[sp_SPC_FTAMaster_GetList]    Script Date: 5/9/2023 4:18:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author: Malik Ilman	
-- Create date: 10-Mar-2023
-- Description:	Get List FTA Master
-- =============================================

ALTER   PROCEDURE [dbo].[sp_SPC_FTAMaster_GetList]
	@FactoryCode			AS Varchar(25),
	@ItemCheckCode			AS Varchar(15),
	@ItemTypeCode			AS Varchar(25)
As
SET NOCOUNT ON
BEGIN
		
		SELECT 
			RowNumber,
			FactoryCode,
			[ItemTypeCode]
			,[ItemCheckCode]
			,[FTAID]
			,
			CASE 
				WHEN [Factor1] = '-' AND ( RowNumber % 2 ) = 0 THEN ' -'
				WHEN [Factor1] = '-' AND ( RowNumber % 2 ) = 1 THEN '-'
				Else Factor1
			END Factor1
			,
			CASE 
				WHEN [Factor2] = '-' AND ( RowNumber % 2 ) = 0 THEN ' -'
				WHEN [Factor2] = '-' AND ( RowNumber % 2 ) = 1 THEN '-'
				Else Factor2
			END Factor2
			,
			CASE 
				WHEN [Factor3] = '-' AND ( RowNumber % 2 ) = 0 THEN ' -'
				WHEN [Factor3] = '-' AND ( RowNumber % 2 ) = 1 THEN '-'
				Else Factor3
			END Factor3
			,
			CASE 
				WHEN [Factor4] = '-' AND ( RowNumber % 2 ) = 0 THEN ' -'
				WHEN [Factor4] = '-' AND ( RowNumber % 2 ) = 1 THEN '-'
				Else Factor4
			END Factor4
			,[CounterMeasure]
			,[CheckItem]
			,[CheckOrder]
			,[IK]
			,[Remark]
			,[ActiveStatus]
			,[RegisterUser]
			,[RegisterDate]
			,[UpdateUser]
			,[UpdateDate]
			,LineName 
			,ItemCheck 
			FROM (
				SELECT 
				ROW_NUMBER() OVER (ORDER By [Factor1]) as RowNumber,
				[FactoryCode]
					  ,[ItemTypeCode]
					  ,[ItemCheckCode]
					  ,[FTAID]
					  ,[Factor1]
					  ,[Factor2]
					  ,[Factor3]
					  ,[Factor4]
					  ,[CounterMeasure]
					  ,[CheckItem]
					  ,[CheckOrder]
					  ,[IK]
					  ,[Remark]
					  ,[ActiveStatus]
					  ,[RegisterUser]
					  ,[RegisterDate]
					  ,[UpdateUser]
					  ,[UpdateDate]
					  ,LineName = ''
					  ,ItemCheck = ''
					  FROM spc_MS_FTA 
					  WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode AND  ItemCheckCode = @ItemCheckCode
					  --ORDER BY Factor1, Factor2, Factor3, Factor4, CounterMeasure ASC

			) TBL
		

END
SET NOCOUNT OFF

