Feature: DeleteMedicalRecord
User will try to delete a medical record

Background: 
Given The system is ready for medical record to be deleted

@DeleteMedicalRecord
Scenario: The user will try to delete a medical record for an existing prisoner
	Given There are 3 prisoners registered in the system
	Given The user is assigned with a "Medic" role
	And Prisoner with PIDN 11221 has a medical record
	When The user tries to delete the medical record for prisoner with PIDN 11221
	Then The medical record will be removed from the database and message will be shown

Scenario: The user will try to delete a medical record for a non existing prisoner
	Given There are 3 prisoners registered in the system
	Given The user is assigned with a "Medic" role
	And There is no prisoner with PIDN 0000
	When The user tries to delete the medical record for prisoner with PIDN 0000
	Then The system will show an error message and the result service will return false

Scenario: The user will try to delete a medical record for an existing prisoner without and existing medical record
	Given There are 3 prisoners registered in the system
	Given The user is assigned with a "Medic" role
	And Prisoner with PIDN 11221 does not have a medical record
	When The user tries to delete the medical record for prisoner with PIDN 11221
	Then Then the system will show an error message and result service will return false

Scenario: The user will try to delete a medical record without the correct role
	Given There are 3 prisoners registered in the system
	And Prisoner with PIDN 11221 has a medical record
	But The User is assigned a "Correctional Officer" role
	When The user tries to delete the medical record for prisoner with PIDN 11221
	Then Then the system will show a role error message and result service will return false
