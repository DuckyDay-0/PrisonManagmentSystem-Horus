Feature: GetMedicalRecord
User will try to get a Medical Record
Background: 
Given The system is ready to get a medical record

@GetMedicalRecord
Scenario: User tries to get a medical record
	Given There are prisoners in the database.
	And The prisoner with PIDN 11221 has a medical record.
	When User tries to get Medical Record for prisoner with PIDN 11221.
	Then The system will return the medical record for prisoner with PIDN 11221.