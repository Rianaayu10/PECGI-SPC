---====== Required to fill (Mandatory) ====---
DECLARE @FactoryCode AS VARCHAR(25) = 'F001', --> Mandatory to change (Get FactoryID from MS Factory)
		@LineCode AS VARCHAR(5) = '015', --> Mandatory to change (Get Line from MS Line)
		@EmployeeID AS CHAR(5) = '001', --> Mandatory to change (Get EmployeeID from SPC_UserSetup)
		@StartDate AS DATE = CAST(GETDATE() AS DATE), --> Mandatory to change
		@EndDate AS DATE = CAST(DATEADD(DAY,10,GETDATE()) AS DATE) -->Mandatory andatory to change
---========================================----

---====== No need to change (HardCode) ====---
DECLARE	@ProcessCode AS VARCHAR(25) = '', --> No need to change (Automotic fill by query)
		@SequenceNO AS CHAR(1) = '1', --> No need to change (hardcode SequenceNO => 1)
		@SkillCode AS VARCHAR(15) = 'SPC001', --> No need to change (Skil Code for SPC => SPC001 )
		@ManPower AS VARCHAR(15) = '1', --> No need to change (hardcode ManPower => 1)
		@RegisterUser AS VARCHAR(50) = 'AdminTos', --> Optional to Change
		@RegisterDate AS DATE = GETDATE() --> Optional to Change
---========================================----
		
SELECT @ProcessCode = ProcessCode FROM MS_Line 
WHERE FactoryCode = @FactoryCode AND LineCode = @LineCode

--INSERT MACHINE SKILL MAP
  IF NOT EXISTS (SELECT * FROM MS_MachineSkillSetting WHERE FactoryCode = @FactoryCode AND LineCode = @LineCode AND SkillCode = @SkillCode)
  BEGIN
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