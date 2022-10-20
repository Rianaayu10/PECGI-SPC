--MANDATORY CHANGE
DECLARE @FactoryCode AS VARCHAR(25) = 'F001',
		@LineCode AS VARCHAR(5) = '015',
		@ProcessCode AS VARCHAR(25) = 'P002',
		@EmployeeID AS CHAR(5) = '001'

--OPTIONAL CHANGE
DECLARE	@SequenceNO AS CHAR(1) = '1',
		@SkillCode AS VARCHAR(15) = 'SPC001',
		@ManPower AS VARCHAR(15) = '1',
		@StartDate AS DATE = CAST(GETDATE() AS DATE),
		@EndDate AS DATE = CAST(DATEADD(DAY,10,GETDATE()) AS DATE),	
		@RegisterUser AS VARCHAR(50) = 'admintos',
		@RegisterDate AS DATE = GETDATE()
		
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