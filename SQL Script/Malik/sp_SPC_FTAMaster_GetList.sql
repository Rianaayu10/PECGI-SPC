/****** Object:  StoredProcedure [dbo].[sp_SPC_FTAMaster_GetList]    Script Date: 4/4/2023 12:34:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author: Malik Ilman	
-- Create date: 10-Mar-2023
-- Description:	Get List FTA Master
-- =============================================

ALTER PROCEDURE [dbo].[sp_SPC_FTAMaster_GetList]
	@FactoryCode			AS Varchar(25),
	@ItemCheckCode			AS Varchar(15),
	@ItemTypeCode			AS Varchar(25)
As
SET NOCOUNT ON
BEGIN
		
		SELECT [FactoryCode]
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
	  FROM spc_MS_FTA WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode AND  ItemCheckCode = @ItemCheckCode
	  ORDER BY CheckOrder ASC
		

END
SET NOCOUNT OFF
