/****** Object:  StoredProcedure [dbo].[SP_SPC_ProdSampleVerificationList_FillCombo]    Script Date: 1/4/2023 1:21:21 PM ******/
DROP PROCEDURE [dbo].[SP_SPC_ProdSampleVerificationList_FillCombo]
GO
/****** Object:  StoredProcedure [dbo].[SP_SPC_ProdSampleVerification_FillCombo]    Script Date: 1/4/2023 1:21:21 PM ******/
DROP PROCEDURE [dbo].[SP_SPC_ProdSampleVerification_FillCombo]
GO
/****** Object:  StoredProcedure [dbo].[SP_SPC_ProdSampleVerification_FillCombo]    Script Date: 1/4/2023 1:21:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Riana Ayu.A>
-- Create date: <2022-08-05>
-- Update date: <2022-12-20>
-- Description:	<->
-- =============================================
CREATE PROCEDURE [dbo].[SP_SPC_ProdSampleVerification_FillCombo]
	@Status	AS CHAR(2),
	@User	AS VARCHAR(50),
	@FactoryCode AS VARCHAR(25),	
	@ItemTypeCode AS VARCHAR(25),
	@LineCode AS VARCHAR(5),
	@ItemCheckCode AS VARCHAR(25),
	@ShiftCode AS VARCHAR(25),
	@ProcessGroup AS VARCHAR(25),
	@LineGroup AS VARCHAR(25),
	@ProcessCode AS VARCHAR(25)

AS
BEGIN
	IF @STATUS = '1' --FACTORY 
	BEGIN
		SELECT CODE = MF.FactoryCode, CODENAME = FactoryName 
		FROM MS_Factory MF
		INNER JOIN spc_UserSetup US ON MF.FactoryCode = US.FactoryCode
		WHERE US.UserID = @User
		ORDER BY FactoryName
	END
	ELSE IF @STATUS = '2' --ITEM TYPE
	BEGIN

		SELECT DISTINCT CODE = A.ItemTypeCode, CODENAME = Description
		FROM MS_ItemType A
		INNER JOIN MS_TypeDetail TD ON A.ItemTypeCode = TD.ItemTypeCode
		INNER JOIN spc_ItemCheckByType B ON A.ItemTypeCode = B.ItemTypeCode AND B.FactoryCode = TD.FactoryCode AND B.ActiveStatus = '1'
		INNER JOIN spc_UserLine C ON B.LineCode = C.LineCode AND C.AppID = 'SPC' AND C.AllowShow = '1' AND C.UserID = @User
		WHERE TD.FactoryCode = @FactoryCode AND TD.ProcessCode = @ProcessCode AND TD.LineCode = @LineCode
		GROUP BY A.ItemTypeCode, A.Description
		ORDER BY Description
		--SELECT CODE = A.ItemTypeCode, CODENAME = Description
		--FROM MS_ItemType A
		--INNER JOIN spc_ItemCheckByType B ON A.ItemTypeCode = B.ItemTypeCode AND B.FactoryCode = @FactoryCode AND B.ActiveStatus = '1'
		--INNER JOIN spc_UserLine C ON B.LineCode = C.LineCode AND C.AppID = 'SPC' AND C.AllowShow = '1' AND C.UserID = @User
		--GROUP BY A.ItemTypeCode, A.Description
		--ORDER BY Description
	END
	ELSE IF @STATUS = '3' --LINE
	BEGIN
		SELECT CODE = A.LineCode, CODENAME = CONCAT(A.LineCode, ' - ', A.LineName) 
		FROM MS_Line A
		INNER JOIN spc_UserLine B ON A.LineCode = B.LineCode AND B.AppID = 'SPC' AND B.AllowShow = '1'
		INNER JOIN spc_ItemCheckByType C ON A.LineCode = C.LineCode AND C.FactoryCode = A.FactoryCode AND C.ActiveStatus = '1'
		WHERE B.UserID = @User AND A.ProcessCode = @ProcessCode AND A.FactoryCode = @FactoryCode 
		GROUP BY A.LineCode, A.LineName
		ORDER BY CODE
	END
	ELSE IF @STATUS = '4' --ITEM CHECK
	BEGIN
		SELECT CODE = A.ItemCheckCode, CODENAME = CONCAT(A.ItemCheckCode,' - ', B.ItemCheck)
		FROM spc_ItemCheckByType A
		JOIN spc_ItemCheckMaster B ON A.ItemCheckCode = B.ItemCheckCode
		--JOIN spc_UserLine C ON A.LineCode = C.LineCode AND C.AppID = 'SPC' AND C.AllowShow = '1'
		WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode AND @LineCode = A.LineCode  AND A.ActiveStatus = '1'
		--AND C.UserID = @User
		ORDER BY A.ItemCheckCode
	END
	ELSE IF @STATUS = '5' --SHIFT
	BEGIN

		SELECT DISTINCT CODE = ShiftCode, CODENAME = ShiftCode
		FROM spc_ItemCheckByType A
		JOIN spc_MS_Frequency B ON A.FrequencyCode = B.FrequencyCode
		WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode 
		AND @LineCode = LineCode AND @ItemCheckCode = ItemCheckCode
		ORDER BY ShiftCode
		
	END
	ELSE IF @STATUS = '6' --SEQ
	BEGIN		
		SELECT DISTINCT CODE = SequenceNo, CODENAME = SequenceNo
		FROM spc_ItemCheckByType A
		JOIN spc_MS_Frequency B ON A.FrequencyCode = B.FrequencyCode
		WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode 
		AND @LineCode = LineCode AND @ItemCheckCode = ItemCheckCode
		AND @ShiftCode = ShiftCode
		ORDER BY SequenceNo
	END

	ELSE IF @STATUS = '7' --SEQ
	BEGIN		
		SELECT DISTINCT CODE = UserID, CODENAME = UserID
		FROM spc_UserSetup	
		ORDER BY UserID
	END

	ELSE IF @STATUS = '8' --USER
	BEGIN		
		SELECT CODE = A.UserID, CODENAME = A.FullName
		FROM spc_UserSetup A
		JOIN spc_UserLine B ON A.UserID = B.UserID
		WHERE B.LineCode = @LineCode
	END
	ELSE IF @STATUS = '9' -- PROCESS GROUP
	BEGIN
		SELECT CODE = MPG.ProcessGroup, CODENAME = MPG.ProcessGroup + ' - ' + MPG.ProcessGroupName 
		FROM MS_ProcessGroup MPG 
		INNER JOIN MS_LineGroup MLG on MPG.FactoryCode = MLG.FactoryCode and MPG.ProcessGroup = MLG.ProcessGroup
		INNER JOIN MS_Process MP on MLG.FactoryCode = MP.FactoryCode and MLG.LineGroup = MP.LineGroup and MLG.ProcessGroup = MP.ProcessGroup
		INNER JOIN ms_line ML on MP.FactoryCode = ML.FactoryCode and MP.ProcessCode = ML.ProcessCode
		INNER JOIN MS_TypeDetail MTD on ML.FactoryCode = MTD.FactoryCode and ML.ProcessCode = MTD.ProcessCode and ML.LineCode = MTD.LineCode
		INNER JOIN MS_ItemType MIT on MTD.ItemTypeCode = MIT.ItemTypeCode
		INNER JOIN spc_ItemCheckByType ICT ON MTD.FactoryCode = ICT.FactoryCode AND MTD.LineCode = ICT.LineCode AND MIT.ItemTypeCode = ICT.ItemTypeCode 
		INNER JOIN spc_UserLine UL on ICT.LineCode = UL.LineCode
		WHERE UL.UserID = @User and UL.AllowShow = 1 and ICT.ActiveStatus = 1
		AND MPG.FactoryCode = @FactoryCode 
		GROUP BY MPG.ProcessGroup, MPG.ProcessGroupName
		ORDER BY CODENAME
	END
	ELSE IF @STATUS = '10' -- LINE GROUP
	BEGIN
		SELECT CODE = MLG.LineGroup, CODENAME =  MLG.LineGroup + ' - ' +  MLG.LineGroupName
		FROM MS_LineGroup MLG
		INNER JOIN MS_Process MP on MLG.FactoryCode = MP.FactoryCode and MLG.LineGroup = MP.LineGroup and MLG.ProcessGroup = MP.ProcessGroup
		INNER JOIN ms_line ML on MP.FactoryCode = ML.FactoryCode and MP.ProcessCode = ML.ProcessCode
		INNER JOIN MS_TypeDetail MTD on ML.FactoryCode = MTD.FactoryCode and ML.ProcessCode = MTD.ProcessCode and ML.LineCode = MTD.LineCode
		INNER JOIN MS_ItemType MIT on MTD.ItemTypeCode = MIT.ItemTypeCode
		INNER JOIN spc_ItemCheckByType ICT ON MTD.FactoryCode = ICT.FactoryCode AND MTD.LineCode = ICT.LineCode AND MIT.ItemTypeCode = ICT.ItemTypeCode 
		INNER JOIN spc_UserLine UL on ICT.LineCode = UL.LineCode
		WHERE UL.UserID = @User and UL.AllowShow = 1 and ICT.ActiveStatus = 1
		AND MLG.FactoryCode = @FactoryCode and MLG.ProcessGroup = @ProcessGroup
		GROUP BY MLG.LineGroup, MLG.LineGroupName
		ORDER BY CODENAME
	END
	ELSE IF @STATUS = '11' -- MACHINE
	BEGIN
		SELECT CODE = MP.ProcessCode, CODENAME = MP.ProcessCode + ' - '+  MP.ProcessName
		FROM MS_Process MP
		INNER JOIN ms_line ML on MP.FactoryCode = ML.FactoryCode and MP.ProcessCode = ML.ProcessCode
		INNER JOIN MS_TypeDetail MTD on ML.FactoryCode = MTD.FactoryCode and ML.ProcessCode = MTD.ProcessCode and ML.LineCode = MTD.LineCode
		INNER JOIN MS_ItemType MIT on MTD.ItemTypeCode = MIT.ItemTypeCode
		INNER JOIN spc_ItemCheckByType ICT ON MTD.FactoryCode = ICT.FactoryCode AND MTD.LineCode = ICT.LineCode AND MIT.ItemTypeCode = ICT.ItemTypeCode 
		INNER JOIN spc_UserLine UL on ICT.LineCode = UL.LineCode
		WHERE UL.UserID = @User and UL.AllowShow = 1 and ICT.ActiveStatus = 1
		AND MP.FactoryCode = @FactoryCode and MP.ProcessGroup = @ProcessGroup and MP.LineGroup = @LineGroup
		GROUP BY MP.ProcessCode, MP.ProcessName
		ORDER BY CODENAME
	END

	ELSE IF @Status = '12' -- FILTER COMBO GET SPC FILTER
	BEGIN
		SELECT F.FactoryCode, MPG.ProcessGroup, MLG.LineGroup, MP.ProcessCode, ML.LineCode, MIT.ItemTypeCode --, ICT.ItemCheckCode
		FROM MS_Factory F 
		INNER JOIN MS_ProcessGroup MPG on F.FactoryCode = MPG.FactoryCode
		INNER JOIN MS_LineGroup MLG on MPG.FactoryCode = MLG.FactoryCode and MPG.ProcessGroup = MLG.ProcessGroup
		INNER JOIN MS_Process MP on MLG.FactoryCode = MP.FactoryCode and MLG.LineGroup = MP.LineGroup and MLG.ProcessGroup = MP.ProcessGroup
		INNER JOIN ms_line ML on MP.FactoryCode = ML.FactoryCode and MP.ProcessCode = ML.ProcessCode
		INNER JOIN MS_TypeDetail MTD on ML.FactoryCode = MTD.FactoryCode and ML.ProcessCode = MTD.ProcessCode and ML.LineCode = MTD.LineCode
		INNER JOIN MS_ItemType MIT on MTD.ItemTypeCode = MIT.ItemTypeCode
		--INNER JOIN spc_ItemCheckByType ICT ON MID.FactoryCode = ICT.FactoryCode AND MID.LineCode = ICT.LineCode AND MIT.ItemTypeCode = ICT.ItemTypeCode 
		where F.FactoryCode = @FactoryCode AND ML.LineCode = @LineCode AND MIT.ItemTypeCode = @ItemTypeCode
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_SPC_ProdSampleVerificationList_FillCombo]    Script Date: 1/4/2023 1:21:21 PM ******/
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
CREATE PROCEDURE [dbo].[SP_SPC_ProdSampleVerificationList_FillCombo]
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
		SELECT CODE = MPG.ProcessGroup, CODENAME = MPG.ProcessGroup + ' - ' + MPG.ProcessGroupName 
		FROM MS_ProcessGroup MPG 
		INNER JOIN MS_LineGroup MLG on MPG.FactoryCode = MLG.FactoryCode and MPG.ProcessGroup = MLG.ProcessGroup
		INNER JOIN MS_Process MP on MLG.FactoryCode = MP.FactoryCode and MLG.LineGroup = MP.LineGroup and MLG.ProcessGroup = MP.ProcessGroup
		INNER JOIN ms_line ML on MP.FactoryCode = ML.FactoryCode and MP.ProcessCode = ML.ProcessCode
		INNER JOIN MS_TypeDetail MTD on ML.FactoryCode = MTD.FactoryCode and ML.ProcessCode = MTD.ProcessCode and ML.LineCode = MTD.LineCode
		INNER JOIN MS_ItemType MIT on MTD.ItemTypeCode = MIT.ItemTypeCode
		INNER JOIN spc_ItemCheckByType ICT ON MTD.FactoryCode = ICT.FactoryCode AND MTD.LineCode = ICT.LineCode AND MIT.ItemTypeCode = ICT.ItemTypeCode 
		INNER JOIN spc_UserLine UL on ICT.LineCode = UL.LineCode
		WHERE UL.UserID = @UserID and UL.AllowShow = 1 and ICT.ActiveStatus = 1
		AND MPG.FactoryCode = @FactoryCode 
		GROUP BY MPG.ProcessGroup, MPG.ProcessGroupName
		ORDER BY CODENAME
	END
	ELSE IF @STATUS = '8' -- LINE GROUP
	BEGIN
		SELECT CODE = MLG.LineGroup, CODENAME =  MLG.LineGroup + ' - ' +  MLG.LineGroupName
		FROM MS_LineGroup MLG
		INNER JOIN MS_Process MP on MLG.FactoryCode = MP.FactoryCode and MLG.LineGroup = MP.LineGroup and MLG.ProcessGroup = MP.ProcessGroup
		INNER JOIN ms_line ML on MP.FactoryCode = ML.FactoryCode and MP.ProcessCode = ML.ProcessCode
		INNER JOIN MS_TypeDetail MTD on ML.FactoryCode = MTD.FactoryCode and ML.ProcessCode = MTD.ProcessCode and ML.LineCode = MTD.LineCode
		INNER JOIN MS_ItemType MIT on MTD.ItemTypeCode = MIT.ItemTypeCode
		INNER JOIN spc_ItemCheckByType ICT ON MTD.FactoryCode = ICT.FactoryCode AND MTD.LineCode = ICT.LineCode AND MIT.ItemTypeCode = ICT.ItemTypeCode 
		INNER JOIN spc_UserLine UL on ICT.LineCode = UL.LineCode
		WHERE UL.UserID = @UserID and UL.AllowShow = 1 and ICT.ActiveStatus = 1
		AND MLG.FactoryCode = @FactoryCode and MLG.ProcessGroup = @ProcessGroup
		GROUP BY MLG.LineGroup, MLG.LineGroupName
		ORDER BY CODENAME
	END
	ELSE IF @STATUS = '9' -- MACHINE
	BEGIN
		SELECT CODE = MP.ProcessCode, CODENAME = MP.ProcessCode + ' - '+  MP.ProcessName
		FROM MS_Process MP
		INNER JOIN ms_line ML on MP.FactoryCode = ML.FactoryCode and MP.ProcessCode = ML.ProcessCode
		INNER JOIN MS_TypeDetail MTD on ML.FactoryCode = MTD.FactoryCode and ML.ProcessCode = MTD.ProcessCode and ML.LineCode = MTD.LineCode
		INNER JOIN MS_ItemType MIT on MTD.ItemTypeCode = MIT.ItemTypeCode
		INNER JOIN spc_ItemCheckByType ICT ON MTD.FactoryCode = ICT.FactoryCode AND MTD.LineCode = ICT.LineCode AND MIT.ItemTypeCode = ICT.ItemTypeCode 
		INNER JOIN spc_UserLine UL on ICT.LineCode = UL.LineCode
		WHERE UL.UserID = @UserID and UL.AllowShow = 1 and ICT.ActiveStatus = 1
		AND MP.FactoryCode = @FactoryCode and MP.ProcessGroup = @ProcessGroup and MP.LineGroup = @LineGroup
		GROUP BY MP.ProcessCode, MP.ProcessName
		ORDER BY CODENAME
	END
	ELSE IF @Status = '10' -- FILTER COMBO GET SPC FILTER
	BEGIN
		SELECT F.FactoryCode, MPG.ProcessGroup, MLG.LineGroup, MP.ProcessCode, ML.LineCode, MIT.ItemTypeCode --, ICT.ItemCheckCode
		FROM MS_Factory F 
		INNER JOIN MS_ProcessGroup MPG on F.FactoryCode = MPG.FactoryCode
		INNER JOIN MS_LineGroup MLG on MPG.FactoryCode = MLG.FactoryCode and MPG.ProcessGroup = MLG.ProcessGroup
		INNER JOIN MS_Process MP on MLG.FactoryCode = MP.FactoryCode and MLG.LineGroup = MP.LineGroup and MLG.ProcessGroup = MP.ProcessGroup
		INNER JOIN ms_line ML on MP.FactoryCode = ML.FactoryCode and MP.ProcessCode = ML.ProcessCode
		INNER JOIN MS_TypeDetail MTD on ML.FactoryCode = MTD.FactoryCode and ML.ProcessCode = MTD.ProcessCode and ML.LineCode = MTD.LineCode
		INNER JOIN MS_ItemType MIT on MTD.ItemTypeCode = MIT.ItemTypeCode
		--INNER JOIN spc_ItemCheckByType ICT ON MID.FactoryCode = ICT.FactoryCode AND MID.LineCode = ICT.LineCode AND MIT.ItemTypeCode = ICT.ItemTypeCode 
		where F.FactoryCode = @FactoryCode AND ML.LineCode = @LineCode AND MIT.ItemTypeCode = @ItemTypeCode
	END
	
END
GO
