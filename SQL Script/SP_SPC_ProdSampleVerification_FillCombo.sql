
/****** Object:  StoredProcedure [dbo].[SP_SPC_ProdSampleVerification_FillCombo]    Script Date: 12/20/2022 12:06:45 PM ******/
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
ALTER PROCEDURE [dbo].[SP_SPC_ProdSampleVerification_FillCombo]
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
		SELECT CODE = ProcessGroup, CODENAME = ProcessGroup + ' - ' + ProcessGroupName 
		FROM MS_ProcessGroup WHERE FactoryCode = @FactoryCode
		ORDER BY ProcessGroupName
	END
	ELSE IF @STATUS = '10' -- LINE GROUP
	BEGIN
		SELECT CODE = LineGroup, CODENAME =  LineGroup + ' - ' +  LineGroupName
		FROM MS_LineGroup WHERE FactoryCode = @FactoryCode and ProcessGroup = @ProcessGroup
		ORDER BY LineGroupName
	END
	ELSE IF @STATUS = '11' -- MACHINE
	BEGIN
		SELECT CODE = ProcessCode, CODENAME = ProcessCode + ' - '+  ProcessName
		FROM MS_Process WHERE FactoryCode = @FactoryCode and ProcessGroup = @ProcessGroup and LineGroup = @LineGroup
		ORDER BY ProcessName
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
