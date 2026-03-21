Feature: AddMedicalRecord
User will try to add a Medical Record
Background: 
Given The system is ready for medical record to be added
 
@AddMedicalRecord
Scenario: User tries to add a medical record
	Given There are prisoners in the database
	When User tries to add Medical Record for prisoner with PIDN 11221
	Then Medical Record will be added for the prisoner
