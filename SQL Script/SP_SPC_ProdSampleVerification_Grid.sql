
/****** Object:  StoredProcedure [dbo].[SP_SPC_ProdSampleVerification_Grid]    Script Date: 12/28/2022 2:11:16 PM ******/
DROP PROCEDURE [dbo].[SP_SPC_ProdSampleVerification_Grid]
GO

/****** Object:  StoredProcedure [dbo].[SP_SPC_ProdSampleVerification_Grid]    Script Date: 12/28/2022 2:11:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Riana Ayu.A>
-- Create date: <2022-08-05>
-- Description:	<->
-- =============================================
CREATE PROCEDURE [dbo].[SP_SPC_ProdSampleVerification_Grid]
(
	@FactoryCode AS VARCHAR(25),	
	@ItemTypeCode AS VARCHAR(25),
	@LineCode AS VARCHAR(5),
	@ItemCheckCode AS VARCHAR(15),
	@ProdDate AS VARCHAR(10),
	@ShiftCode AS VARCHAR(10),
	@Seq AS CHAR(1),
	@ShowVerify AS CHAR(1),
	@ActionGrid CHAR(1),
	@ProdDate_Grid AS VARCHAR(10),
	@ShiftCode_Grid AS VARCHAR(5),
	@User AS VARCHAR(50)
)

AS
BEGIN

	SET NOCOUNT ON

	DECLARE @ProdDate_From AS Date,  @ProdDate_To AS Date

	DECLARE @Clr_Default AS VARCHAR(10) = '#FFFFFF'
				, @Clr_Yellow AS VARCHAR(10) = '#FFFF00'
				, @Clr_LightYellow AS VARCHAR(10) = '#FFFE91'
				, @Clr_Red AS VARCHAR(10) = '#ff0000'
				, @Clr_Orange AS VARCHAR(10) = '#FFA500'
				, @Clr_Pink AS VARCHAR(10) = '#FFC0CB'
				, @Clr_Green AS VARCHAR(10) = '#377D22'

	SELECT @ProdDate_From = MAX(ProdDate) 
	FROM spc_Result A
		JOIN spc_ChartSetup CS ON A.FactoryCode = CS.FactoryCode AND A.LineCode = CS.LineCode AND A.ItemTypeCode = CS.ItemTypeCode AND A.ItemCheckCode = CS.ItemCheckCode AND CAST(A.ProdDate AS DATE) BETWEEN CAST(CS.StartDate AS DATE) AND CAST(CS.EndDate AS DATE)
	WHERE A.FactoryCode = @FactoryCode AND A.ItemTypeCode = @ItemTypeCode AND A.LineCode = @LineCode 
		AND A.ItemCheckCode = @ItemCheckCode AND A.ProdDate < CAST(@ProdDate  AS DATE) AND CompleteStatus = '1'

	SELECT @ProdDate_To = MAX(ProdDate) 
	FROM spc_Result A
		JOIN spc_ChartSetup CS ON A.FactoryCode = CS.FactoryCode AND A.LineCode = CS.LineCode AND A.ItemTypeCode = CS.ItemTypeCode AND A.ItemCheckCode = CS.ItemCheckCode AND CAST(A.ProdDate AS DATE) BETWEEN CAST(CS.StartDate AS DATE) AND CAST(CS.EndDate AS DATE)
	WHERE A.FactoryCode = @FactoryCode AND A.ItemTypeCode = @ItemTypeCode AND A.LineCode = @LineCode 
		AND A.ItemCheckCode = @ItemCheckCode AND ProdDate = CAST(@ProdDate  AS DATE)  AND CompleteStatus = '1'
	
	/*==================== GET HEADER GRID (PROD DATE) ========================--*/
	IF @ActionGrid = '1' 
	BEGIN
		SELECT ProdDate
		FROM (
			SELECT DISTINCT FORMAT(ProdDate, 'dd MMM yyyy') As ProdDate, ProdDate As nDate
			FROM spc_Result A
			JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
			) B ON A.SPCResultID = B.SPCResultID
			WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode
			AND LineCode = @LineCode AND ItemCheckCode = @ItemCheckCode
			AND ProdDate IN (@ProdDate_From,@ProdDate_To) AND CompleteStatus = '1'
		) A ORDER BY nDate
	END
	/*=============================================================================*/

	/*==================== GET GET HEADER GRID (SHIFT CODE) ========================--*/
	ELSE IF @ActionGrid = '2' 
	BEGIN

		SELECT DISTINCT ShiftCode
		FROM spc_Result A
		JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
		) B ON A.SPCResultID = B.SPCResultID
		WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode 
		AND LineCode = @LineCode AND ItemCheckCode = @ItemCheckCode
		AND 1 = CASE WHEN ProdDate = CAST(@ProdDate AS DATE) AND @ShiftCode = 'SH001' THEN 		
						CASE WHEN ShiftCode = 'SH001' THEN 1
						ELSE 0 END
				ELSE	
						CASE WHEN ProdDate = CAST(@ProdDate_Grid AS DATE) THEN 1
						ELSE 0 END 
				END

		AND ProdDate = CAST(@ProdDate_Grid AS DATE) AND CompleteStatus = '1'
		ORDER BY ShiftCode
	END
	/*=============================================================================*/

	/*====================== GET GET HEADER GRID (SEQ) ===========================--*/
	ELSE IF @ActionGrid = '3' 
	BEGIN
		SELECT nTimeDesc = FORMAT(RegisterDate, 'HH:mm')
		     , nTime = CONCAT(FORMAT(ProdDate,'yyyyMMdd') ,'_',ShiftCode,'_',SequenceNo)
		FROM spc_Result A
		JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
		) B ON A.SPCResultID = B.SPCResultID
		WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode 
		AND LineCode = @LineCode AND ItemCheckCode = @ItemCheckCode
		AND 1 = CASE WHEN ProdDate = CAST(@ProdDate AS DATE) AND ShiftCode = @ShiftCode THEN 		
						CASE WHEN ShiftCode = @ShiftCode_Grid AND ProdDate = CAST(@ProdDate_Grid AS DATE) AND SequenceNo <= @Seq THEN 1
						ELSE 0 END
				ELSE	
						CASE WHEN ShiftCode = @ShiftCode_Grid AND ProdDate = CAST(@ProdDate_Grid AS DATE) THEN 1
						ELSE 0 END 
				END	
		AND CompleteStatus = '1'
		ORDER BY SequenceNo
	END
	/*=============================================================================*/

	/*====================== GET CHILD GRID DATA =================================--*/
	ELSE IF @ActionGrid = '4'
	BEGIN
		DECLARE @pFactoryCode AS VARCHAR(25),
				@pLineCode AS VARCHAR(5),
				@pItemTypeCode AS VARCHAR(25),
				@pItemCheckCode AS VARCHAR(15),
				@pProdDate AS VARCHAR(10),
				@pSequenceNo AS VARCHAR(10),
				@pSPCResultID AS VARCHAR(10)

		CREATE TABLE #TempXbar  
				( nTime varchar(50), nMin varchar(50),
				  nMax varchar(50), nAvg varchar(50),
				  nR varchar(50), SubLotNo varchar(50),
				  Operator varchar(50), MK varchar(50),
				  QC varchar(50), Judgement varchar (MAX),
				  Correction Varchar(50)
				)
		
		--========= INSERT DATA EACH DATA TO TABLE TEMPORARY ===================--
		SELECT nTime = CONCAT(FORMAT(A.ProdDate,'yyyyMMdd') ,'_',A.ShiftCode,'_',A.SequenceNo)
			 , Value = CONCAT(C.Value, '|', (CASE WHEN C.Value > SpecUSL OR C.Value < SpecLSL THEN @Clr_Red
												  WHEN (C.Value > CPUCL OR C.Value < CPLCL) THEN @Clr_Pink
												   ELSE @Clr_Default END ))
			 --, nDesc = C.SequenceNo
			 , nDesc = ROW_NUMBER() over (partition by C.SPCResultID order by C.SequenceNo)
		INTO #TempEachData
		FROM spc_Result A	
		JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode AND A.ItemTypeCode = B.ItemTypeCode 
						AND A.LineCode = B.LineCode AND A.ItemCheckCode = B.ItemCheckCode 
						AND(A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
		JOIN vw_SPCResultDetailOK C ON A.SPCResultID = C.SPCResultID
		WHERE A.FactoryCode = @FactoryCode AND A.ItemTypeCode = @ItemTypeCode 
		AND A.LineCode = @LineCode AND A.ItemCheckCode = @ItemCheckCode
		AND 1 = CASE WHEN ProdDate = @ProdDate_From THEN 1
				WHEN ProdDate = @ProdDate_To AND A.SequenceNo <= @Seq THEN 1
				ELSE 0 END
		AND 1 = CASE WHEN @ShowVerify = '1' AND (C.Value > SpecUSL OR C.Value < SpecLSL) THEN 0 
			ELSE 1 END
		AND CompleteStatus = '1'
		--======================================================================---

		--========= INSERT DATA XBar TO TABLR TEMPORARY ===================--

		DECLARE CURSOR_RESULT CURSOR FOR 
			SELECT A.ProdDate, A.FactoryCode, A.ItemTypeCode, A.LineCode, A.ItemCheckCode,  A.SequenceNo, A.SPCResultID		
			FROM  spc_Result A 	
			JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode AND A.ItemTypeCode = B.ItemTypeCode 
						AND A.LineCode = B.LineCode AND A.ItemCheckCode = B.ItemCheckCode 
						AND(A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
			JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
			) C ON A.SPCResultID = C.SPCResultID
			WHERE A.FactoryCode = @FactoryCode 
				AND A.ItemTypeCode = @ItemTypeCode
				AND A.LineCode = @LineCode 
				AND A.ItemCheckCode = @ItemCheckCode
				AND 1 = CASE WHEN ProdDate = @ProdDate_From THEN 1
					WHEN ProdDate = @ProdDate_To AND A.SequenceNo <= @Seq THEN 1
					ELSE 0 END
				AND CompleteStatus = '1'
			GROUP BY A.ProdDate, A.FactoryCode, A.ItemTypeCode, A.LineCode, A.ItemCheckCode,  A.SequenceNo, A.SPCResultID		
		OPEN CURSOR_RESULT
		FETCH NEXT FROM CURSOR_RESULT INTO @pProdDate,@pFactoryCode,@pItemTypeCode,@pLineCode,@pItemCheckCode, @pSequenceNo, @pSPCResultID
		WHILE @@FETCH_STATUS = 0 
		BEGIN
			DECLARE @Last_ProdDate DATE = NULL, @Last_Seq CHAR(1) = NULL, @Last_SPCResultID INT = NULL, @R_OutSpec INT = NULL

			IF @pSequenceNo = '1'
			BEGIN
				SELECT @Last_ProdDate = MAX(ProdDate) FROM spc_Result A
				JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
				) B ON A.SPCResultID = B.SPCResultID
				WHERE FactoryCode = @pFactoryCode and ItemTypeCode = @pItemTypeCode and LineCode = @pLineCode 
				AND ItemCheckCode = @pItemCheckCode AND ProdDate < @pProdDate AND CompleteStatus = '1'

				SELECT @Last_Seq = MAX(SequenceNo) FROM spc_Result A
				JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
				) B ON A.SPCResultID = B.SPCResultID
				WHERE FactoryCode = @pFactoryCode and ItemTypeCode = @pItemTypeCode and LineCode = @pLineCode 
				AND ItemCheckCode = @pItemCheckCode AND ProdDate = @Last_ProdDate AND CompleteStatus = '1'

				SELECT @Last_SPCResultID = SPCResultID FROM spc_Result A
				WHERE FactoryCode = @pFactoryCode and ItemTypeCode = @pItemTypeCode and LineCode = @pLineCode 
				AND ItemCheckCode = @pItemCheckCode AND ProdDate = @Last_ProdDate AND SequenceNo = @Last_Seq
				AND CompleteStatus = '1'
			END
			ELSE
			BEGIN
				SET @Last_ProdDate = @pProdDate

				SELECT @Last_Seq = MAX(SequenceNo) FROM spc_Result A
				JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
				) B ON A.SPCResultID = B.SPCResultID
				WHERE FactoryCode = @pFactoryCode and ItemTypeCode = @pItemTypeCode and LineCode = @pLineCode 
				AND ItemCheckCode = @pItemCheckCode AND ProdDate = @Last_ProdDate AND SequenceNo < @pSequenceNo
				AND CompleteStatus = '1'

				SELECT @Last_SPCResultID = SPCResultID FROM spc_Result A
				WHERE FactoryCode = @pFactoryCode and ItemTypeCode = @pItemTypeCode and LineCode = @pLineCode 
				AND ItemCheckCode = @pItemCheckCode AND ProdDate = @Last_ProdDate AND SequenceNo = @Last_Seq
				AND CompleteStatus = '1'
			END

			SELECT @R_OutSpec = RD.SPCResultID 
			FROM spc_ResultDetail RD
			JOIN spc_Result R ON RD.SPCResultID = R.SPCResultID
			JOIN spc_ChartSetup CS ON  R.FactoryCode = CS.FactoryCode AND R.ItemTypeCode = CS.ItemTypeCode
				AND R.LineCode = CS.LineCode AND R.ItemCheckCode = CS.ItemCheckCode 
				AND R.ProdDate BETWEEN CAST(CS.StartDate AS DATE) AND CAST(CS.EndDate AS DATE)
			WHERE R.SPCResultID = @Last_SPCResultID 
				AND 1 = CASE WHEN @ShowVerify = '1' AND (Value > SpecUSL OR Value < SpecLSL) THEN 0 ELSE 1 END 
				AND CompleteStatus = '1'
			GROUP BY RD.SPCResultID ,RUCL, RLCL
			HAVING (MAX(RD.Value) - MIN(RD.Value)) > CS.RUCL OR (MAX(RD.Value) - MIN(RD.Value)) < CS.RLCL

			SELECT nTime = CONCAT(FORMAT(A.ProdDate,'yyyyMMdd'),'_',ShiftCode,'_',A.SequenceNo)
			 , nMin = CONCAT(MIN(Value), '|', (CASE WHEN MIN(Value) > SpecUSL OR MIN(Value) < SpecLSL THEN @Clr_Red
												  WHEN MIN(Value)> CPUCL OR  MIN(Value) < CPLCL THEN
													CASE WHEN H.CharacteristicStatus = '1' THEN @Clr_LightYellow 
													ELSE @Clr_Pink END
												  ELSE @Clr_Default END ))
			 , nMax = CONCAT(MAX(Value), '|', (CASE WHEN MAX(Value) > SpecUSL OR MAX(Value) < SpecLSL THEN @Clr_Red
												  WHEN MAX(Value)> CPUCL OR  MAX(Value) < CPLCL THEN 
													CASE WHEN H.CharacteristicStatus = '1' THEN @Clr_LightYellow 
													ELSE @Clr_Pink END
												  ELSE @Clr_Default END ))
			 , nAvg = CONCAT(CAST(AVG(Value) AS numeric(18,3)), '|', (CASE WHEN AVG(Value) > SpecUSL OR AVG(Value) < SpecLSL THEN @Clr_Red
																		   WHEN AVG(Value)> CPUCL  OR AVG(Value) < CPLCL THEN
																			CASE WHEN H.CharacteristicStatus = '1' THEN @Clr_LightYellow 
																			ELSE @Clr_Pink END
																		   ELSE @Clr_Default END ))
			 , nR = CONCAT(CAST(MAX([Value]) - MIN([Value]) AS NUMERIC(18,3)), '|', (CASE WHEN CAST(MAX([Value]) - MIN([Value]) AS NUMERIC(18,3)) > RUCL OR CAST(MAX([Value]) - MIN([Value]) AS NUMERIC(18,3)) < RLCL THEN 
																			( CASE WHEN H.CharacteristicStatus = '1' THEN 
																				(CASE WHEN @R_OutSpec IS NOT NULL THEN @Clr_Pink ELSE @Clr_Yellow END )
																				ELSE @Clr_Default  END )
																			ELSE @Clr_Default END ))

			 , SubLotNo = CASE WHEN ISNULL(SubLotNo,'') = '' THEN '-' ELSE SubLotNo END
			 , Operator = CASE WHEN U_OP.FullName IS NOT NULL THEN U_OP.FullName ELSE A.RegisterUser END
			 , MK = CASE WHEN A.ProdDate = CAST(@ProdDate AS DATE) AND A.ShiftCode = @ShiftCode AND A.SequenceNo = @Seq AND ISNULL(MKVerificationUser,'') = '' THEN
					CONCAT(CASE WHEN U_MK.FullName IS NOT NULL THEN u_MK.FullName ELSE MKVerificationUser END,'|',@Clr_Yellow) 
					ELSE CONCAT(CASE WHEN U_MK.FullName IS NOT NULL THEN u_MK.FullName ELSE MKVerificationUser END,'|',@Clr_Default) END
			 , QC = CASE WHEN A.ProdDate = CAST(@ProdDate AS DATE) AND A.ShiftCode = @ShiftCode AND A.SequenceNo = @Seq AND ISNULL(QCVerificationUser,'') = '' THEN 
					CONCAT(CASE WHEN U_QC.FullName IS NOT NULL THEN u_QC.FullName ELSE QCVerificationUser END,'|',@Clr_Yellow) 
					ELSE CONCAT(CASE WHEN U_QC.FullName IS NOT NULL THEN u_QC.FullName ELSE QCVerificationUser END,'|',@Clr_Default) END
			 , Judgement = CONCAT ( 
							CASE WHEN MIN([Value]) < SpecLSL OR MIN([Value]) > SpecUSL OR MAX([Value]) < SpecLSL OR MAX([Value]) > SpecUSL THEN CONCAT('NG', '|', @Clr_Red) 
							WHEN MIN([Value]) >= SpecLSL AND MAX([Value]) <= SpecUSL AND G.nValue >= H.SampleSize THEN CONCAT('OK', '|', @Clr_Default)
							ELSE CONCAT('', '|', @Clr_Default) END ,'|',
							CONCAT('ProdSampleInput.aspx?menu=ProdSampleVerification.aspx','&FactoryCode=',@FactoryCode,'&ItemTypeCode=',@ItemTypeCode
								,'&Line=',@LineCode,'&ItemCheckCode=',@ItemCheckCode,'&ProdDate=',A.ProdDate,'&Shift=',ShiftCode,'&Sequence=',A.SequenceNo)
							)
			 , Correction =  CASE WHEN E.SPCResultID IS NOT NULL THEN CONCAT('C', '|', @Clr_Orange)ELSE CONCAT('', '|', @Clr_Default) END
			Into #Tmp_Result
			FROM  spc_Result A 	
			LEFT JOIN spc_UserSetup U_OP ON A.RegisterUser = U_OP.UserID
			LEFT JOIN spc_UserSetup U_MK ON A.MKVerificationUser = U_MK.UserID
			LEFT JOIN spc_UserSetup U_QC ON A.QCVerificationUser = U_QC.UserID
			JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode AND A.ItemTypeCode = B.ItemTypeCode 
							AND A.LineCode = B.LineCode AND A.ItemCheckCode = B.ItemCheckCode 
							AND(A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
			LEFT JOIN (SELECT DISTINCT SPCResultID FROM spc_ResultDetail WHERE ISNULL(DeleteStatus,'0') = '1' ) AS E ON A.SPCResultID = E.SPCResultID	
			LEFT JOIN spc_ResultDetail F ON A.SPCResultID = F.SPCResultID AND ISNULL(F.DeleteStatus, '0') = '0'
			LEFT JOIN (SELECT COUNT(C.SPCResultID) AS nValue, A.SPCResultID
					   FROM spc_Result A
					   JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode AND A.ItemTypeCode = B.ItemTypeCode
							AND A.LineCode = B.LineCode AND A.ItemCheckCode = B.ItemCheckCode 
							AND (A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
					   JOIN spc_ResultDetail C ON A.SPCResultID = C.SPCResultID AND ISNULL(C.DeleteStatus,'0') <> '1'
					   WHERE C.VALUE <= B.SpecUSL AND C.Value >= B.SpecLSL
					   GROUP BY A.SPCResultID
						) G ON A.SPCResultID = G.SPCResultID
			LEFT JOIN spc_ItemCheckByType H ON A.FactoryCode = H.FactoryCode AND A.ItemTypeCode = H.ItemTypeCode
						AND A.LineCode = H.LineCode AND A.ItemCheckCode = H.ItemCheckCode
			WHERE A.SPCResultID = @pSPCResultID AND 1 = CASE WHEN @ShowVerify = '1' AND (Value > SpecUSL OR Value < SpecLSL) THEN 0 ELSE 1 END
				AND CompleteStatus = '1'
			GROUP BY A.ProdDate, A.ShiftCode, A.SPCResultID,A.SequenceNo, A.SubLotNo, A.MKVerificationStatus, A.QCVerificationStatus, A.MKVerificationUser
					, A.QCVerificationUser, A.RegisterUser, SpecUSL, SpecLSL, CPUCL, CPLCL,H.SampleSize, G.nValue, E.SPCResultID, H.CharacteristicStatus
					, RUCL, RLCL, U_OP.FullName, U_MK.FullName, U_QC.FullName

			Insert into #TempXbar (
			nTime, nMin, nMax, nAvg, nR, SubLotNo
			, Operator,MK,QC,Judgement,Correction
			)
			
			SELECT  nTime, nMin, nMax, nAvg, nR, SubLotNo
			, Operator,MK,QC, Judgement,Correction

			FROM #Tmp_Result
			Drop Table #Tmp_Result

		FETCH NEXT FROM CURSOR_RESULT INTO @pProdDate,@pFactoryCode,@pItemTypeCode,@pLineCode,@pItemCheckCode, @pSequenceNo, @pSPCResultID
		END
		CLOSE CURSOR_RESULT
		DEALLOCATE CURSOR_RESULT

		--======================================================================---

		--========= INSERT DATA TABLE GRID ===================--
		SELECT DISTINCT nTime = CONCAT(FORMAT(A.ProdDate,'yyyyMMdd') ,'_',A.ShiftCode,'_',A.SequenceNo)
			 , nValue = ''
			 , nDesc = ''
		INTO #TempGridNothing
		FROM spc_Result A	
		WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode 
		AND LineCode = @LineCode AND ItemCheckCode = @ItemCheckCode
		AND 1 = CASE WHEN ProdDate = @ProdDate_From THEN 1
				WHEN ProdDate = @ProdDate_To AND A.SequenceNo <= @Seq THEN 1
				ELSE 0 END
		AND CompleteStatus = '1'
		--======================================================================---
		IF NOT EXISTS( SELECT ProdDate, SpecUSL, SpecLSL FROM (
						SELECT DISTINCT A.ProdDate, B.* FROM spc_Result A
						LEFT JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode AND A.ItemTypeCode = B.ItemTypeCode AND A.LineCode = B.LineCode
							AND A.ItemCheckCode = B.ItemCheckCode AND (A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
						WHERE A.FactoryCode = @FactoryCode AND A.ItemTypeCode = @ItemTypeCode AND A.LineCode = @LineCode 
							AND A.ItemCheckCode = @ItemCheckCode AND A.ProdDate IN ( @ProdDate_From, @ProdDate_To)
							AND CompleteStatus = '1'
						) AS A WHERE A.SpecUSL IS NULL OR A.SpecLSL IS NULL )
		BEGIN

			--=========== DECLARATION HEADER TABLE ========================---
			DECLARE @cols AS NVARCHAR(MAX),
					@query  AS NVARCHAR(MAX)

			SELECT @cols = STUFF((SELECT ',' + QUOTENAME(nTime) FROM 
									(SELECT nTime FROM 
										(

										SELECT nTime = CONCAT(FORMAT(ProdDate,'yyyyMMdd') ,'_',ShiftCode,'_',SequenceNo)
										FROM spc_Result 
										WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode 
										AND LineCode = @LineCode AND ItemCheckCode = @ItemCheckCode
										AND 1 = CASE WHEN ProdDate = @ProdDate_From THEN 1
											WHEN ProdDate = @ProdDate_To AND SequenceNo <= @Seq THEN 1 
											ELSE 0 END
										AND CompleteStatus = '1'
										) A 
									 ) C 				
									group by nTime
									order by nTime
							FOR XML PATH(''), TYPE
							).value('.', 'NVARCHAR(MAX)') 
						,1,1,'')

			--=========== GET ALL VALUE =============--
			SET @query = N'SELECT nDescIndex, nDesc,' + @cols + N' FROM
						 (
							SELECT nTime, nDesc = CAST(nDesc AS CHAR(1)), Value = CAST(Value AS VARCHAR(21)), nDescIndex = ''EachData''
							FROM #TempEachData
						) A
						PIVOT 
						(
							MAX(Value)
							FOR nTime in (' + @cols + N')
						) p '

			--=========== GET GRID VALUE =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc , nValue, nDescIndex = ''GridNothing'' 
							FROM #TempGridNothing
						) A
						PIVOT 
						(
							MAX(nValue)
							for nTime in (' + @cols + N')
						) p2 '

			--=========== GET MIN VALUE =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''Min'' , nMin = CAST(nMin AS VARCHAR(21)), nDescIndex = ''XBar'' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(nMin)
							for nTime in (' + @cols + N')
						) p2 '

			----=========== GET MAX VALUE =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''Max'' , nMax = CAST(nMax AS VARCHAR(21)), nDescIndex = ''XBar'' 
							FROM #TempXbar
						) A
						PIVOT  
						(
							MAX(nMax)
							for nTime in (' + @cols + N')
						) p3 '

			----=========== GET AVG VALUE =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''Avg'' ,nAVG = CAST(nAVG AS VARCHAR(21)), nDescIndex = ''XBar'' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(nAVG)
							for nTime in (' + @cols + N')
						) p4 '

			----=========== GET R VALUE =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''R'' , nR = CAST(nR AS VARCHAR(21)), nDescIndex = ''XBar'' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(nR)
							for nTime in (' + @cols + N')
						) p5 '

			--=========== GET GRID VALUE =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc , nValue, nDescIndex = ''GridNothing'' 
							FROM #TempGridNothing
						) A
						PIVOT 
						(
							MAX(nValue)
							for nTime in (' + @cols + N')
						) p2 '

			----=========== GET SUB LOT NO =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''Sub. Lot No'' , SubLotNo, nDescIndex = '''' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(SubLotNo)
							for nTime in (' + @cols + N')
						) p6 '

			----=========== GET OPERATOR USER =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''Operator'' , Operator, nDescIndex = '''' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(Operator)
							for nTime in (' + @cols + N')
						) p7 '

			----=========== GET MK VERIFICATION USER =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''MK/GK/SC'' , MK, nDescIndex = ''Verification'' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(MK)
							for nTime in (' + @cols + N')
						) p8 '

			----=========== GET QC VERIFICATION USER =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''QC'' , QC, nDescIndex = ''Verification'' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(QC)
							for nTime in (' + @cols + N')
						) p9 '
			
			----=========== GET RESULT STATUS =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''Judgement'' , Judgement, nDescIndex = ''Judgement'' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(Judgement)
							for nTime in (' + @cols + N')
						) p10 '

			----=========== GET RESULT CORRECTION =============--
			SET @Query += 'UNION ALL'
			SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
						 (
							SELECT nTime, nDesc = ''Correction'' , Correction, nDescIndex = ''Correction'' 
							FROM #TempXbar
						) A
						PIVOT 
						(
							MAX(Correction)
							for nTime in (' + @cols + N')
						) p11 '

			----=========== GET View Button =============--
			--SET @Query += 'UNION ALL'
			--SET @query += N' SELECT nDescIndex, nDesc,' + @cols + N' from 
			--			 (
			--				SELECT nTime, nDesc = ''View'', nView , nDescIndex = ''View'' 
			--				FROM #TempXbar
			--			) A
			--			PIVOT 
			--			(
			--				MAX(nView)
			--				for nTime in (' + @cols + N')
			--			) p12 '
			----===========================================================---

			EXEC sp_executesql @query;

			DROP TABLE #TempXbar
			DROP TABLE #TempEachData
			DROP TABLE #TempGridNothing
		END
		SELECT nDescIndex = '', nDesc = ''
	END	
	/*=============================================================================*/

	/*=========================== GET ACTIVITES ===================================*/
	ELSE IF @ActionGrid = '5' 
	BEGIN

		SELECT 
		   B.FactoryCode
		  ,B.FactoryName
		  ,C.ItemTypeCode
		  ,ItemTypeName = C.Description
		  ,D.LineCode
		  ,LineName = CONCAT(D.LineCode , ' - ', D.LineName)
		  ,E.ItemCheckCode
		  ,ItemCheckName = CONCAT(F.ItemCheckCode , ' - ',  F.ItemCheck) 
		  ,[ShiftCode]
		  ,ShiftName = ShiftCode 
		  ,[ActivityID]
		  ,ProdDate = FORMAT([ProdDate], 'dd MMM yyyy')
		  ,ProdDate As nDate
		  ,[PIC]
		  ,[Action]
		  ,[Result]
		  ,A.Remark
		  ,[Time] = CAST([Time] AS datetime)
		  ,Time_Desc = FORMAT(CAST([Time] AS datetime), 'HH:mm')
		  ,LastUser = CASE WHEN U_OP.FullName IS NOT NULL THEN U_OP.FullName ELSE A.UpdateUser END
		  ,LastDate = FORMAT(A.UpdateDate, 'dd MMM yyyy')
	  FROM dbo.spc_Activities A
	  LEFT JOIN spc_UserSetup U_OP ON A.UpdateUser = U_OP.UserID
	  JOIN MS_Factory B ON A.FactoryCode = B.FactoryCode
	  JOIN MS_ItemType C ON A.ItemTypeCode = C.ItemTypeCode
	  JOIN MS_Line D ON A.FactoryCode = D.FactoryCode AND A.LineCode = D.LineCode
	  JOIN spc_ItemCheckByType E ON A.FactoryCode = E.FactoryCode AND A.ItemTypeCode = E.ItemTypeCode AND A.LineCode = E.LineCode AND A.ItemCheckCode = E.ItemCheckCode
	  JOIN spc_ItemCheckMaster F ON E.ItemCheckCode = F.ItemCheckCode
	  WHERE A.FactoryCode = @FactoryCode AND A.ItemTypeCode = @ItemTypeCode 
		    AND A.LineCode = @LineCode AND A.ItemCheckCode = @ItemCheckCode 
			AND A.ProdDate IN (	@ProdDate_From,@ProdDate_To)
			ORDER BY nDate
	END
	/*=============================================================================*/

	/*=========================== GET CHART SETUP ===================================*/
	ELSE IF @ActionGrid = '6' 
	BEGIN
		
		IF @Seq = '1'
			BEGIN
				SELECT @Last_ProdDate = MAX(ProdDate) FROM spc_Result A
				JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
				) B ON A.SPCResultID = B.SPCResultID
				WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode 
				AND ItemCheckCode = @ItemCheckCode AND ProdDate < CAST(@ProdDate AS DATE)
				AND CompleteStatus = '1'

				SELECT @Last_Seq = MAX(SequenceNo) FROM spc_Result A
				JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
				) B ON A.SPCResultID = B.SPCResultID
				WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode 
				AND ItemCheckCode = @ItemCheckCode AND ProdDate = @Last_ProdDate
				AND CompleteStatus = '1'

				SELECT @Last_SPCResultID = SPCResultID FROM spc_Result A
				WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode 
				AND ItemCheckCode = @ItemCheckCode AND ProdDate = @Last_ProdDate AND SequenceNo = @Last_Seq
				AND CompleteStatus = '1'

			END
			ELSE
			BEGIN
				SET @Last_ProdDate = CAST(@ProdDate AS DATE)

				SELECT @Last_Seq = MAX(SequenceNo) FROM spc_Result A
				JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID
				) B ON A.SPCResultID = B.SPCResultID
				WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode 
				AND ItemCheckCode = @ItemCheckCode AND ProdDate = @Last_ProdDate AND SequenceNo < @Seq
				AND CompleteStatus = '1'

				SELECT @Last_SPCResultID = SPCResultID FROM spc_Result A
				WHERE FactoryCode = @FactoryCode and ItemTypeCode = @ItemTypeCode and LineCode = @LineCode 
				AND ItemCheckCode = @ItemCheckCode AND ProdDate = @Last_ProdDate AND SequenceNo = @Last_Seq
				AND CompleteStatus = '1'
			END

			SELECT @R_OutSpec = RD.SPCResultID 
			FROM spc_ResultDetail RD
			JOIN spc_Result R ON RD.SPCResultID = R.SPCResultID
			JOIN spc_ChartSetup CS ON  R.FactoryCode = CS.FactoryCode AND R.ItemTypeCode = CS.ItemTypeCode
				AND R.LineCode = CS.LineCode AND R.ItemCheckCode = CS.ItemCheckCode 
				AND R.ProdDate BETWEEN CAST(CS.StartDate AS DATE) AND CAST(CS.EndDate AS DATE)
			WHERE R.SPCResultID = @Last_SPCResultID 
				AND 1 = CASE WHEN @ShowVerify = '1' AND (Value > SpecUSL OR Value < SpecLSL) THEN 0 ELSE 1 END 
				AND CompleteStatus = '1'
			GROUP BY RD.SPCResultID ,RUCL, RLCL
			HAVING (MAX(RD.Value) - MIN(RD.Value)) > CS.RUCL OR (MAX(RD.Value) - MIN(RD.Value)) < CS.RLCL

		SELECT 		  
			  UCL = CPUCL 
			, LCL = CPLCL
			, XBarUCL 
			, XBarLCL 
			, USL = SpecUSL
			, LSL = SpecLSL
			, RUCL = RUCL
			, RLCL = RLCL
			, nMIN = MIN(C.Value) 
			, nMINClr = CASE WHEN MIN(C.Value) > SpecUSL OR MIN(Value) < SpecLSL THEN @Clr_Red
						WHEN MIN(C.Value) > CPUCL OR MIN(Value) < CPLCL THEN
						CASE WHEN ICT.CharacteristicStatus = '1' THEN @Clr_LightYellow ELSE @Clr_Pink  END ELSE @Clr_Default END
			, nMAX = MAX(C.Value) 
			, nMAXClr = CASE WHEN MAX(C.Value) > SpecUSL OR MAX(Value) < SpecLSL THEN @Clr_Red
						WHEN MAX(C.Value) > CPUCL OR MAX(Value) < CPLCL THEN
						CASE WHEN ICT.CharacteristicStatus = '1' THEN @Clr_LightYellow ELSE @Clr_Pink  END ELSE @Clr_Default END
			, nAVG = CAST(AVG(C.Value) AS decimal(18,3))
			, nAVGClr = CASE WHEN AVG(C.Value) > SpecUSL OR AVG(Value) < SpecLSL THEN @Clr_Red
						WHEN AVG(C.Value) > CPUCL OR AVG(Value) < CPLCL THEN
						CASE WHEN ICT.CharacteristicStatus = '1' THEN @Clr_LightYellow ELSE @Clr_Pink  END ELSE @Clr_Default END
			, nR = CAST((MAX(C.Value) - MIN(C.Value)) AS decimal(18,3))
			, nRClr = CASE WHEN CAST((MAX(C.Value) - MIN(C.Value)) AS decimal(18,3)) > RUCL OR CAST((MAX(C.Value) - MIN(C.Value)) AS decimal(18,3)) < RLCL THEN 
					  ( CASE WHEN ICT.CharacteristicStatus = '1' THEN 
						 ( CASE WHEN @R_OutSpec IS NOT NULL THEN @Clr_Pink ELSE @Clr_Yellow END )
						ELSE @Clr_Default END )
					   ELSE @Clr_Default END
			, C = CASE WHEN D.SPCResultID IS NULL THEN '' ELSE 'C' END
			, C_Clr = CASE WHEN D.SPCResultID IS NULL THEN @Clr_Default ELSE @Clr_Orange END
			, NG = CASE WHEN B.SPCResultID IS NOT NULL THEN CASE WHEN MIN([Value]) >= SpecLSL AND MAX([Value]) <= SpecUSL THEN 'OK' ELSE 'NG' END ELSE '' END
			, NG_Clr = CASE WHEN B.SPCResultID IS NOT NULL THEN CASE WHEN MIN([Value]) >= SpecLSL AND MAX([Value]) <= SpecUSL THEN @Clr_Green ELSE @Clr_Red END ELSE @Clr_Default END
			, CS = ICT.CharacteristicStatus
			, SubLotNo
			, ICT.ProcessTableLineCode
			, B.SPCResultID	
		FROM spc_ChartSetup A
		LEFT JOIN spc_Result B ON B.FactoryCode = A.FactoryCode AND B.ItemTypeCode = A.ItemTypeCode
			AND B.LineCode = A.LineCode AND B.ItemCheckCode = A.ItemCheckCode AND ProdDate = CAST(@ProdDate AS DATE) 
			AND ShiftCode = @ShiftCode AND B.SequenceNo = @Seq 
			AND SPCResultID IN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus <> '1' GROUP BY SPCResultID )
		LEFT JOIN spc_ItemCheckByType ICT ON A.FactoryCode = ICT.FactoryCode AND A.ItemTypeCode = ICT.ItemTypeCode
			AND A.LineCode = ICT.LineCode AND A.ItemCheckCode = ICT.ItemCheckCode
		LEFT JOIN spc_ResultDetail C ON B.SPCResultID = C.SPCResultID AND COALESCE(C.DeleteStatus,'0') = '0'
					AND 1 = CASE WHEN @ShowVerify = '1' AND (Value > SpecUSL OR Value < SpecLSL) THEN 0 
					ELSE 1 END
		LEFT JOIN (SELECT SPCResultID FROM spc_ResultDetail WHERE DeleteStatus = '1' GROUP BY SPCResultID ) AS D ON B.SPCResultID = D.SPCResultID
		WHERE A.FactoryCode = @FactoryCode AND A.ItemTypeCode = @ItemTypeCode 
			AND A.LineCode = @LineCode AND A.ItemCheckCode = @ItemCheckCode 
			AND (CAST(@ProdDate AS DATE) BETWEEN CAST(StartDate AS DATE) AND CAST(EndDate AS DATE))
			AND CompleteStatus = '1'
		GROUP BY SpecLSL, SpecUSL, CPUCL, CPLCL, D.SPCResultID, B.SPCResultID, CharacteristicStatus, 
		SubLotNo, RUCL, RLCL, XBarLCL, XBarUCL, ProcessTableLineCode

	END


	/*=========================== GET Verify Privilege ==============================*/
	ELSE IF @ActionGrid = '7' 
	BEGIN	
		DECLARE   @UserPosition AS CHAR(2) = ''
				, @VerifyPrivilege AS CHAR(1) = '0'
				, @MKVerify AS VARCHAR(50) = ''
				, @QCVerify AS VARCHAR(50) = ''

		SELECT @UserPosition = JobPosition FROM spc_UserSetup Where UserID = @User 

		SELECT @MKVerify = ISNULL(MKVerificationUser,''), @QCVerify = ISNULL(QCVerificationUser,'')
		FROM spc_Result A
		WHERE A.FactoryCode = @FactoryCode AND A.ItemTypeCode = @ItemTypeCode 
			AND A.LineCode = @LineCode AND A.ItemCheckCode = @ItemCheckCode
			AND A.ProdDate = CAST(@ProdDate AS DATE) AND A.ShiftCode = @ShiftCode AND A.SequenceNo = @Seq
			AND CompleteStatus = '1'
		IF @UserPosition = 'MK' AND @MKVerify = ''
		BEGIN		
			SET @VerifyPrivilege = '1'
		END

		ELSE IF @UserPosition = 'QC' AND @QCVerify = '' AND @MKVerify <> ''
		BEGIN
			SET @VerifyPrivilege = '1'
		END

		SELECT VerifyPrivilege = @VerifyPrivilege

	END
	/*===============================================================================*/

	/*=================== GET Verify Specification Chart Setup ======================*/
	ELSE IF @ActionGrid = '8' 
	BEGIN	
		DECLARE   @Response AS VARCHAR(250) = ''
					
		SET @ProdDate_From = CAST(@ProdDate AS DATE)
		SET @ProdDate_To = (
			SELECT MAX(ProdDate) FROM spc_Result
			WHERE FactoryCode = @FactoryCode AND ItemTypeCode = @ItemTypeCode AND LineCode = @LineCode 
			AND ItemCheckCode = @ItemCheckCode AND ProdDate < CAST(@ProdDate  AS DATE))

	IF NOT EXISTS (SELECT * FROM spc_Result A
					JOIN spc_ChartSetup B ON A.FactoryCode = B.FactoryCode AND A.ItemTypeCode = B.ItemTypeCode AND A.LineCode = B.LineCode
						AND A.ItemCheckCode = B.ItemCheckCode AND (A.ProdDate BETWEEN CAST(B.StartDate AS DATE) AND CAST(B.EndDate AS DATE))
					WHERE A.FactoryCode = @FactoryCode AND A.ItemTypeCode = @ItemTypeCode AND A.LineCode = @LineCode 
						AND A.ItemCheckCode = @ItemCheckCode AND A.ProdDate IN (@ProdDate_From, @ProdDate_To)) 
	BEGIN
		SET @Response = 'Chart Setup Master Not Found ! '
	END

	SELECT Response = @Response

	END
	/*===============================================================================*/
END
GO


