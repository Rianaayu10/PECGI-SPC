
/****** Object:  StoredProcedure [dbo].[SP_SPC_ProdSampleVerificationList_FillCombo]    Script Date: 12/20/2022 12:06:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Riana Ayu.A>
-- Create date: <2022-08-05>
-- Update date: <2022-12-20>
-- Description:	<Add Fill Combo>
-- =============================================
ALTER PROCEDURE [dbo].[SP_SPC_ProdSampleVerificationList_FillCombo]
	@Status	AS CHAR(5),
	@FactoryCode AS VARCHAR(25),
	@LineCode AS VARCHAR(5),
	@ItemTypeCode AS VARCHAR(25),
	@ProcessGroup AS VARCHAR(25),
	@LineGroup AS VARCHAR(25),
	@ProcessCode AS VARCHAR(25),
	@UserID As VARCHAR(50)

AS
BEGIN

	IF @STATUS = '1' --FACTORY 
	BEGIN
		SELECT CODE = MF.FactoryCode, CODENAME = FactoryName 
		FROM MS_Factory MF
		INNER JOIN spc_UserSetup US ON MF.FactoryCode = US.FactoryCode
		WHERE US.UserID = @UserID
		ORDER BY FactoryName
	END
	ELSE IF @STATUS = '2' --ITEM TYPE
	BEGIN
		SELECT DISTINCT CODE = A.ItemTypeCode, CODENAME = Description
		FROM MS_ItemType A
		INNER JOIN MS_TypeDetail TD ON A.ItemTypeCode = TD.ItemTypeCode
		INNER JOIN spc_ItemCheckByType B ON A.ItemTypeCode = B.ItemTypeCode AND B.FactoryCode = TD.FactoryCode AND B.ActiveStatus = '1'
		INNER JOIN spc_UserLine C ON B.LineCode = C.LineCode AND C.AppID = 'SPC' AND C.AllowShow = '1' AND C.UserID = @UserID
		WHERE TD.FactoryCode = @FactoryCode AND TD.ProcessCode = @ProcessCode AND TD.LineCode = @LineCode
		--AND 1 = CASE WHEN @LineCode = 'ALL' THEN 1
		--		WHEN @LineCode <> 'ALL' AND TD.LineCode = @LineCode THEN 1
		--		ELSE 0 END
		GROUP BY A.ItemTypeCode, A.Description
		ORDER BY Description
	END
	ELSE IF @STATUS = '3' --LINE
	BEGIN
		--SELECT CODE, CODENAME
		--FROM (
		--		SELECT NO = 1, CODE = 'ALL', CODENAME = 'ALL'
		--		UNION ALL
		--		SELECT  NO = 2, CODE = A.LineCode, CODENAME = CONCAT(A.LineCode, ' - ', A.LineName) 
		--		FROM MS_Line A
		--		INNER JOIN spc_UserLine B ON A.LineCode = B.LineCode AND B.AppID = 'SPC' AND B.AllowShow = '1'
		--		INNER JOIN spc_ItemCheckByType C ON A.LineCode = C.LineCode AND C.FactoryCode = A.FactoryCode AND C.ActiveStatus = '1'
		--		WHERE B.UserID = @UserID AND A.ProcessCode = @ProcessCode AND A.FactoryCode = @FactoryCode 					
		--		GROUP BY A.LineCode, A.LineName
		--		) A
		--ORDER BY NO, CODE	
		SELECT  CODE = A.LineCode, CODENAME = CONCAT(A.LineCode, ' - ', A.LineName) 
		FROM MS_Line A
		INNER JOIN spc_UserLine B ON A.LineCode = B.LineCode AND B.AppID = 'SPC' AND B.AllowShow = '1'
		INNER JOIN spc_ItemCheckByType C ON A.LineCode = C.LineCode AND C.FactoryCode = A.FactoryCode AND C.ActiveStatus = '1'
		WHERE B.UserID = @UserID AND A.ProcessCode = @ProcessCode AND A.FactoryCode = @FactoryCode 					
		GROUP BY A.LineCode, A.LineName
		ORDER BY CODENAME
	END
	ELSE IF @STATUS = '4' --ITEM CHECK
	BEGIN
		IF @LineCode = '' SET @LineCode = 'ALL' 

		SELECT CODE, CODENAME
		FROM (
				SELECT NO = 1, CODE = 'ALL', CODENAME = 'ALL'
				UNION ALL
				SELECT NO = 2, CODE = A.ItemCheckCode, CODENAME = CONCAT(A.ItemCheckCode,' - ', B.ItemCheck)
				FROM spc_ItemCheckByType A
				JOIN spc_ItemCheckMaster B ON A.ItemCheckCode = B.ItemCheckCode
				JOIN MS_Line ML ON A.FactoryCode = ML.FactoryCode AND A.LineCode = ML.LineCode
				JOIN spc_UserLine C ON ML.LineCode = C.LineCode AND C.AppID = 'SPC' AND C.AllowShow = '1'AND  C.UserID = @UserID
				WHERE A.FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode AND A.ActiveStatus = '1'
				AND 1 = CASE WHEN @LineCode = 'ALL' THEN 1
							WHEN @LineCode <> 'ALL' AND @LineCode = C.LineCode THEN 1
							ELSE 0 END
				) A
		ORDER BY NO, CODE
	END
	ELSE IF @STATUS = '5' --MK VERIFICATION
	BEGIN
		SELECT CODE, CODENAME
		FROM (
				SELECT NO = 1, CODE = 'ALL', CODENAME = 'ALL'
				UNION ALL
				SELECT NO = 2, CODE = '1', CODENAME = 'YES'
				UNION ALL
				SELECT NO = 3, CODE = '0', CODENAME = 'NO'				
			) A
		ORDER BY NO, CODENAME
	END
	ELSE IF @STATUS = '6' --QC VERIFICATION
	BEGIN
		SELECT CODE, CODENAME
		FROM (
				SELECT NO = 1, CODE = 'ALL', CODENAME = 'ALL'
				UNION ALL
				SELECT NO = 2, CODE = '1', CODENAME = 'YES'
				UNION ALL
				SELECT NO = 3, CODE = '0', CODENAME = 'NO'
			) A
		ORDER BY NO, CODENAME
	END
	ELSE IF @STATUS = '7' -- PROCESS GROUP
	BEGIN
		SELECT CODE = ProcessGroup, CODENAME = ProcessGroup + ' - ' + ProcessGroupName 
		FROM MS_ProcessGroup WHERE FactoryCode = @FactoryCode
		ORDER BY ProcessGroupName
	END
	ELSE IF @STATUS = '8' -- LINE GROUP
	BEGIN
		SELECT CODE = LineGroup, CODENAME =  LineGroup + ' - ' +  LineGroupName
		FROM MS_LineGroup WHERE FactoryCode = @FactoryCode and ProcessGroup = @ProcessGroup
		ORDER BY LineGroupName
	END
	ELSE IF @STATUS = '9' -- MACHINE
	BEGIN
		SELECT CODE = ProcessCode, CODENAME = ProcessCode + ' - '+  ProcessName
		FROM MS_Process WHERE FactoryCode = @FactoryCode and ProcessGroup = @ProcessGroup and LineGroup = @LineGroup
		ORDER BY ProcessName
	END
	
END

