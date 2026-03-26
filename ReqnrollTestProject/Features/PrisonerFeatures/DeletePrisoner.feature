Feature: DeletePrisoner
User will try to delete prisoner
Background: 
Given The system is ready for prisoner to be deleted

@DeletePrisoner
Scenario: User tries to delete an existing prisoner
	Given There are three prisoners in the database ready to be deleted
	And The User is assigned with an "Admin" role
	When User tries to delete prisoner with PIDN 11221
	Then The system will return a message and the prisoner with PIDN 11221 will be deleted

Scenario: User tries to delete a non existing prisoner
	Given There are three prisoners in the database ready to be deleted
	And The User is assigned with an "Admin" role
	When User tries to delete prisoner with PIDN 0000
	Then The system will return a message saying "Prisoner with this PIDN does not exist!" and resultService will return false

Scenario: User tries to delete an existing prisoner with the incorrect role
	Given There are three prisoners in the database ready to be deleted
	And The User is assigned with an "Medic" role
	When User tries to delete prisoner with PIDN 11221
	Then The system will return a message saying "You don't have the authorization to perform this action!" and resultService will return false

Scenario: User tries to delete an existing prisoner with the correct role and the prisoner has a medical record
	Given There are three prisoners in the database ready to be deleted
	And The User is assigned with an "Admin" role
	And The Prisoner with PIDN 12345 has a Medical Record
	When User tries to delete prisoner with PIDN 12345
	Then The system will return a message saying "Prisoner Deleted", resultService will return true and the Medical Record for Prisoner with PIDN 12345 will be removed
