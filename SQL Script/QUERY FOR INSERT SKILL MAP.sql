---====== Required to fill (Mandatory) ====---
DECLARE @FactoryCode AS VARCHAR(25) = 'F001', --> Mandatory to change (Get FactoryID from MS Factory)
		@LineCode AS VARCHAR(5) = '015', --> Mandatory to change (Get Line from MS Line)
		@EmployeeID AS CHAR(5) = '007', --> Mandatory to change (Get EmployeeID from SPC_UserSetup)
		@StartDate AS DATE = '2022-11-30', --> Mandatory to change
		@EndDate AS DATE = '2023-01-01' -->Mandatory andatory to change
---========================================----

---====== No need to change (HardCode) ====---
DECLARE	@ProcessCode AS VARCHAR(25) = '', --> No need to change (Automotic fill by query)
		@SequenceNO AS CHAR(1) = '0', --> No need to change (hardcode SequenceNO => 1)
		@SkillCode AS VARCHAR(15) = 'SPC001', --> No need to change (Skil Code for SPC => SPC001 )
		@SkillDesc AS VARCHAR(15) = 'SPC Skill', --> No need to change (Skil Code for SPC => SPC001 )
		@ManPower AS VARCHAR(15) = '1', --> No need to change (hardcode ManPower => 1)
		@RegisterUser AS VARCHAR(50) = 'AdminTos', --> Optional to Change
		@RegisterDate AS DATE = GETDATE() --> Optional to Change
---========================================----
		
SELECT @ProcessCode = ProcessCode FROM MS_Line 
WHERE FactoryCode = @FactoryCode AND LineCode = @LineCode

--INSERT MASTER SKILL
	IF NOT EXISTS(SELECT * FROM MS_Skill WHERE SkillCode = @SkillCode)
	BEGIN
		INSERT INTO MS_Skill (SkillCode,Description,RegisterBy,RegisterDate,UpdateBy,UpdateDate)
		VALUES(@SkillCode,@SkillDesc,@RegisterUser,@RegisterDate,@RegisterUser,@RegisterDate)
	END

--INSERT MACHINE SKILL MAP
  IF NOT EXISTS (SELECT * FROM MS_MachineSkillSetting WHERE FactoryCode = @FactoryCode AND LineCode = @LineCode AND SkillCode = @SkillCode AND ProcessCode = @ProcessCode) 
  BEGIN
	
	SELECT @SequenceNO =COALESCE(MAX(SequenceNo),'0') FROM MS_MachineSkillSetting WHERE FactoryCode = @FactoryCode AND LineCode = @LineCode AND ProcessCode = @ProcessCode
	SET @SequenceNO = @SequenceNO + 1

	INSERT INTO MS_MachineSkillSetting
		( FactoryCode
		, ProcessCode
		, LineCode
		, SequenceNo
		, SkillCode
		, ManPower
		, RegisterBy
		, RegisterDate
		, UpdateBy
		, UpdateDate )
	VALUES
		( @FactoryCode
		, @ProcessCode
		, @LineCode
		, @SequenceNO
		, @SkillCode
		, @ManPower
		, @RegisterUser
		, @RegisterDate
		, @RegisterUser
		, @RegisterDate )
  END

--INSERT MS EMPLOYEE
  IF NOT EXISTS (SELECT * FROM MS_Employee WHERE EmployeeID = @EmployeeID)
  BEGIN
	INSERT INTO MS_Employee(
		 EmployeeID
		,FullName
		,Education
		,Address
		,Telephone
		,EmployeeStatus
		,RegisterBy
		,RegisterDate
		,UpdateBy
		,UpdateDate
		,QRCode
		,Joindate
		,StartDate_Contract
		,EndDate_Contract
	) 
	VALUES(
		@EmployeeID
		,'SPC Integration Test'
		,'03'
		,''
		,''
		,'01'
		,@RegisterUser
		,@RegisterDate
		,@RegisterUser
		,@RegisterDate
		,''
		,@RegisterDate
		,@StartDate
		,@EndDate
	)
  END
  ELSE
  BEGIN
	UPDATE MS_Employee
	SET StartDate_Contract = @StartDate, EndDate_Contract = @EndDate
	WHERE EmployeeID = @EmployeeID
  END

--INSERT EMPLOYEE SKILL MAP
  IF NOT EXISTS (SELECT * FROM MS_EmployeeSkill WHERE EmployeeID = @EmployeeID AND SkillCode = @SkillCode)
  BEGIN
	INSERT INTO MS_EmployeeSkill
		( EmployeeID
		, SkillCode
		, StartDate
		, EndDate
		, RegisterBy
		, RegisterDate
		, UpdateBy
		, UpdateDate )
	VALUES 
		( @EmployeeID
		, @SkillCode
		, @StartDate
		, @EndDate
		, @RegisterUser
		, @RegisterDate
		, @RegisterUser
		, @RegisterDate )
  END
  ELSE
  BEGIN
	UPDATE MS_EmployeeSkill
	SET StartDate = @StartDate, EndDate = @EndDate, UpdateDate = @RegisterDate
	WHERE SkillCode = @SkillCode AND EmployeeID = @EmployeeID
  END