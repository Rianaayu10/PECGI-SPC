
-- =============================================
-- Author:		<Riana Ayu.A>
-- Create date: <2022-08-05>
-- Description:	<->
-- =============================================
ALTER PROCEDURE [dbo].[SP_SPC_ProdSampleVerification_FillCombo]
	@Status	AS CHAR(1),
	@User	AS VARCHAR(50),
	@FactoryCode AS VARCHAR(25),	
	@ItemTypeCode AS VARCHAR(25),
	@LineCode AS VARCHAR(5),
	@ItemCheckCode AS VARCHAR(25),
	@ShiftCode AS VARCHAR(25)

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
		SELECT CODE = A.ItemTypeCode, CODENAME = Description
		FROM MS_ItemType A
		INNER JOIN spc_ItemCheckByType B ON A.ItemTypeCode = B.ItemTypeCode AND B.FactoryCode = @FactoryCode AND B.ActiveStatus = '1'
		INNER JOIN spc_UserLine C ON B.LineCode = C.LineCode AND C.AppID = 'SPC' AND C.AllowShow = '1' AND C.UserID = @User
		GROUP BY A.ItemTypeCode, A.Description
		ORDER BY Description
	END
	ELSE IF @STATUS = '3' --LINE
	BEGIN
		SELECT CODE = A.LineCode, CODENAME = CONCAT(A.LineCode, ' - ', LineName) 
		FROM MS_Line A
		JOIN spc_UserLine B ON A.LineCode = B.LineCode AND B.FactoryCode = @FactoryCode AND B.AppID = 'SPC' AND B.AllowShow = '1'
		INNER JOIN spc_ItemCheckByType C ON A.LineCode = C.LineCode AND C.ItemTypeCode = @ItemTypeCode AND C.FactoryCode = @FactoryCode AND C.ActiveStatus = '1'
		WHERE B.UserID = @User
		GROUP BY A.LineCode, A.LineName
		ORDER BY A.LineCode
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
END
