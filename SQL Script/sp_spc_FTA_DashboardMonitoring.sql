
-- =============================================
-- Author:		<RIANA>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_spc_FTA_DashboardMonitoring]
	@UserID		As Varchar(25)	
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @CURRENTDATE DATETIME = GETDATE()

	DECLARE @Station1 As varchar(25), @Station2 As varchar(25), @Station3 As varchar(25), @Station4 As varchar(25),
			@Station5 As varchar(25), @Station6 As varchar(25), @Station7 As varchar(25), @Station8 As varchar(25),
			@Station9 As varchar(25), @Station10 As varchar(25), @Station11 As varchar(25), @Station12 As varchar(25),
			@Station13 As varchar(25), @Station14 As varchar(25), @Station15 As varchar(25), @Station16 As varchar(25), 
			@DelayNGResult AS char(5), @DelayInputResult AS char(5)

	DECLARE @NoProduction	As varchar(15) = 'NO PRODUCTION',
			@Safe			As varchar(15) = 'SAFE',
			@DelayInput		As varchar(15) = 'DELAY INPUT',
			@DelayNG		As varchar(15) = 'NG'

	DECLARE @Clr_NoProduction	As varchar(15) = 'blue',
			@Clr_Safe			As varchar(15) = 'green',
			@Clr_DelayInput		As varchar(15) = 'yellow',
			@Clr_DelayNG		As varchar(15) = 'red'

	DECLARE @Font_NoProduction	As varchar(15) = 'white',
			@Font_Safe			As varchar(15) = 'white',
			@Font_DelayInput	As varchar(15) = 'black',
			@Font_DelayNG		As varchar(15) = 'white'

	DECLARE @StationID varchar(5)

	DROP TABLE IF EXISTS #Tbl_DelayNG
	DROP TABLE IF EXISTS #Tbl_DelayInput
	DROP TABLE IF EXISTS #Tbl_Production

	CREATE TABLE #Tbl_DelayNG(
		  Edit					Varchar(MAX)
		, Label					Varchar(MAX)
		, Link					Varchar(MAX)
		, RValueSPCDashboard	Varchar(MAX)
		, ActionFTA				Varchar(MAX)
		, FTAResultID			INT
		, FactoryCode			Varchar(25)
		, FactoryName			Varchar(50)
		, ItemTypeCode			Varchar(25)
		, ItemTypeNameLink		Varchar(50)
		, ItemTypeName			Varchar(MAX)
		, LineCode				Varchar(25)
		, LineName				Varchar(50)
		, LineDesc				Varchar(25)
		, ItemCheckCode			Varchar(25)
		, ItemCheck				Varchar(MAX)
		, ItemCheckDesc			Varchar(MAX)
		, Date					Varchar(25)
		, LinkDate				Varchar(25)
		, ShiftCode				Varchar(25)
		, ShiftCodeHeader		Varchar(25)
		, SequenceNo			Varchar(25)
		, USL					numeric(18,4)
		, LSL					numeric(18,4)
		, UCL					numeric(18,4)
		, LCL					numeric(18,4)
		, MaxValue				numeric(18,4)
		, MinValue				numeric(18,4)
		, Average				numeric(18,4)
		, RValue				Varchar(25)
		, RColor				Varchar(25)
		, StatusNG				Varchar(25)
		, Operator				Varchar(25)
		, MK					Varchar(25)
		, QC					Varchar(25)
		, UpdateDate			datetime
		, CharacteristicStatus	Varchar(25)
		, StatusFTA				VARCHAR(MAX)
		, MKFTA					VARCHAR(MAX)
		, QCFTA					VARCHAR(MAX)
	)

	CREATE TABLE #Tbl_DelayInput(
		  DelayHeader		Varchar(25)
		, Label				Varchar(max)
		, Edit				Varchar(max)
		, Link				Varchar(max)
		, TypeHeader		Varchar(max)
		, FactoryCode		Varchar(25)
		, FactoryName		Varchar(50)
		, ItemTypeName		Varchar(50)
		, LineName			Varchar(50)
		, ItemCheck			Varchar(50)
		, Date				Varchar(25)
		, LinkDate			Varchar(25)
		, ShiftCode			Varchar(25)
		, ShiftCodeHeader	Varchar(25)
		, SequenceNo		Varchar(25)
		, StartTime			Varchar(25)
		, EndTime			Varchar(25)
		, Delay				Varchar(25)
		, ItemTypeCode		Varchar(25)
		, LineCode			Varchar(25)
		, ItemCheckCode		Varchar(25)
		, FrequencyCode		Varchar(25)
		, UpdateTime		datetime
	)

	--> STEP 1
	--> SET STATION TO NO PRODUCTION ALL

	SET @Station1 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station2 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station3 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station4 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station5 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station6 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station7 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station8 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station9 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station10 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station11 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station12 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station13 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station14 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station15 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @Station16 = concat(@NoProduction,'|',@Clr_NoProduction, '|', @Font_NoProduction)
	SET @DelayInputResult = '0'
	SET @DelayNGResult = '0'

	--> STEP 2
	--> SHORTIR DATA SAFE (ADA PRODUKSI)
 
	DECLARE @LASTPRODUCTION AS DATE

	SELECT @LASTPRODUCTION = MAX(Schedule_Date)
	FROM Daily_Production AS DP
		INNER JOIN MS_Factory AS MF ON DP.Factory_code = MF.FactoryCode
		INNER JOIN MS_Process AS MP ON DP.process_Code = MP.ProcessCode
		INNER JOIN MS_Line AS ML ON DP.Line_Code = ML.LineCode
		INNER JOIN MS_Item AS MI ON DP.Item_code = MI.Item_Code
		INNER JOIN MS_ItemType AS MIT ON MI.Item_Type = MIT.ItemTypeCode
		WHERE DP.Factory_code = 'F001' AND CAST(Schedule_Date AS DATE) < CAST(@CURRENTDATE AS DATE)

	SELECT DISTINCT 
		   ScheduleDate = Format(Schedule_Date, 'yyyy-MM-dd')
		 , FactoryCode = DP.Factory_code
		 , LineCode = DP.Line_Code
		 , ML.LineName
		 , ItemCode = DP.Item_code
		 , ItemTypeCode = MIT.ItemTypeCode
	INTO #Tbl_Production
	FROM Daily_Production AS DP
		INNER JOIN MS_Factory AS MF ON DP.Factory_code = MF.FactoryCode
		INNER JOIN MS_Process AS MP ON DP.process_Code = MP.ProcessCode
		INNER JOIN MS_Line AS ML ON DP.Line_Code = ML.LineCode
		INNER JOIN MS_Item AS MI ON DP.Item_code = MI.Item_Code
		INNER JOIN MS_ItemType AS MIT ON MI.Item_Type = MIT.ItemTypeCode
	WHERE CAST(Schedule_Date AS DATE) BETWEEN CAST(@LASTPRODUCTION AS DATE) AND CAST(@CURRENTDATE AS DATE)

	IF EXISTS (SELECT * FROM #Tbl_Production)
	BEGIN
		SET @StationID = NULL
		DECLARE cursor_Production CURSOR FOR

		SELECT DISTINCT IC.StationID FROM #Tbl_Production P
		INNER JOIN spc_ItemCheckByType IC 
		ON P.FactoryCode = IC.FactoryCode AND P.ItemTypeCode = IC.ItemTypeCode 
		AND P.LineCode = IC.LineCode
		WHERE IC.StationID is not null

		OPEN cursor_Production
		FETCH NEXT FROM cursor_Production INTO @StationID
		WHILE @@FETCH_STATUS = 0
		BEGIN

			IF @StationID = '1' SET @Station1 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe) 
			ELSE IF @StationID = '2' SET @Station2 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '3' SET @Station3 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '4' SET @Station4 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '5' SET @Station5 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '6' SET @Station6 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '7' SET @Station7 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '8' SET @Station8 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '9' SET @Station9 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '10' SET @Station10 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '11' SET @Station11 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '12' SET @Station12 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '13' SET @Station13 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '14' SET @Station14 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '15' SET @Station15 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  
			ELSE IF @StationID = '16' SET @Station16 = Concat(@Safe,'|',@Clr_Safe, '|',@Font_Safe)  

		FETCH NEXT FROM cursor_Production INTO @StationID
		END
		CLOSE cursor_Production
		DEALLOCATE cursor_Production
	END

	-->STEP 3 
	-->SHORTIR BY DELAY INPUT

	INSERT INTO #Tbl_DelayInput
	exec sp_SPC_GetDelayInput @UserID, 'ALL', '1', '1', @CURRENTDATE
	--exec sp_SPC_GetDelayInput 'Admintos', 'ALL', '1', '1', @CURRENTDATE

	SELECT @DelayInputResult = COUNT(LineCode) FROM #Tbl_DelayInput

	IF EXISTS (SELECT * FROM #Tbl_DelayInput)
	BEGIN
		SET @StationID = NULL
		DECLARE cursor_DelayInput CURSOR FOR

		SELECT DISTINCT IC.StationID FROM #Tbl_DelayInput DI
		INNER JOIN spc_ItemCheckByType IC ON DI.FactoryCode = IC.FactoryCode AND DI.ItemTypeCode = IC.ItemTypeCode AND DI.LineCode = IC.LineCode AND DI.ItemCheckCode = IC.ItemCheckCode
		WHERE IC.StationID is not null

		OPEN cursor_DelayInput
		FETCH NEXT FROM cursor_DelayInput INTO @StationID
		WHILE @@FETCH_STATUS = 0
		BEGIN

			IF @StationID = '1' SET @Station1 = concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '2' SET @Station2 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '3' SET @Station3 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '4' SET @Station4 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '5' SET @Station5 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '6' SET @Station6 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '7' SET @Station7 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '8' SET @Station8 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '9' SET @Station9 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '10' SET @Station10 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '11' SET @Station11 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '12' SET @Station12 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '13' SET @Station13 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '14' SET @Station14 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '15' SET @Station15 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)
			ELSE IF @StationID = '16' SET @Station16 =  concat(@DelayInput,'|',@Clr_DelayInput, '|',@Font_DelayInput)

		FETCH NEXT FROM cursor_DelayInput INTO @StationID
		END
		CLOSE cursor_DelayInput
		DEALLOCATE cursor_DelayInput
	END


	-->STEP 4 
	-->SHORTIR BY NG

	INSERT INTO #Tbl_DelayNG
	exec sp_SPC_GetNGInput @UserID, 'ALL', '1','1', @CURRENTDATE
	--exec sp_SPC_GetNGInput 'Admintos', 'ALL', '1','1', @CURRENTDATE

	SELECT @DelayNGResult = COUNT(LineCode) FROM #Tbl_DelayNG

	IF EXISTS (SELECT * FROM #Tbl_DelayNG)
	BEGIN
		SET @StationID = NULL
		DECLARE cursor_DelayNG CURSOR FOR

		SELECT DISTINCT IC.StationID FROM #Tbl_DelayNG NG
		INNER JOIN spc_ItemCheckByType IC ON NG.FactoryCode = IC.FactoryCode AND NG.ItemTypeCode = IC.ItemTypeCode AND NG.LineCode = IC.LineCode AND NG.ItemCheckCode = IC.ItemCheckCode
		WHERE IC.StationID is not null

		OPEN cursor_DelayNG
		FETCH NEXT FROM cursor_DelayNG INTO @StationID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @StationID = '1' SET @Station1 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '2' SET @Station2 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '3' SET @Station3 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '4' SET @Station4 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '5' SET @Station5 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '6' SET @Station6 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '7' SET @Station7 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '8' SET @Station8 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '9' SET @Station9 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '10' SET @Station10 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '11' SET @Station11 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '12' SET @Station12 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '13' SET @Station13 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '14' SET @Station14 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '15' SET @Station15 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)
			ELSE IF @StationID = '16' SET @Station16 = concat(@DelayNG,'|',@Clr_DelayNG, '|',@Font_DelayNG)

		FETCH NEXT FROM cursor_DelayNG INTO @StationID
		END
		CLOSE cursor_DelayNG
		DEALLOCATE cursor_DelayNG
	END

	SELECT Station1			= @Station1
		 , Station2			= @Station2
		 , Station3			= @Station3
		 , Station4			= @Station4
		 , Station5			= @Station5
		 , Station6			= @Station6
		 , Station7			= @Station7
		 , Station8			= @Station8
		 , Station9			= @Station9
		 , Station10		= @Station10
		 , Station11		= @Station11
		 , Station12		= @Station12
		 , Station13		= @Station13
		 , Station14		= @Station14
		 , Station15		= @Station15
		 , Station16		= @Station16
		 , DelayInputResult	= @DelayInputResult
		 , DelayNGResult	= @DelayNGResult

	DROP TABLE #Tbl_DelayNG
	DROP TABLE #Tbl_DelayInput
	DROP TABLE #Tbl_Production

END
