Feature: AddBehaviorRecord
User tries to add medical record
Background:
Given The system is ready for behavior records

Scenario: Successfully adding a good behavior record by an Officer
	Given There are three prisoners registered in the system
	And User is logged in with "Correctional Officer" role
	When User adds a Behavior Record with Severity "Good", Description "Helped" for prisoner with PIDN 11221
	Then The behavior record is saved successfully

Scenario: User tries to add behavior record for prisoner without the correct role
	Given There are three prisoners registered in the system
	And User is assigned with a "Medic" role
	When User adds a Behavior Record with Severity "Bad", Description "Fight" for prisoner with PIDN 11221
	Then The system returns error "You are not authorized to perform this kind of action!"

Scenario: Adding record for non-existing prisoner
	Given There are no prisoners in the database for behavior record to be added
	And User is logged in with "Admin" role
	When User adds a Behavior Record with Severity "Bad", Description "Escape" for prisoner with PIDN 00001
	Then The system returns error "Prisoner not found!"

Scenario: Adding record with empty description
	Given There are three prisoners registered in the system
	And User is logged in with "Admin" role
	When User adds a Behavior Record with Severity "Good", Description "" for prisoner with PIDN 11221
	Then The system returns error "Description cannot be empty!"

Scenario: Admin can add multiple behavior records for one prisoner
	Given There are three prisoners registered in the system
	And User is logged in with "Admin" role
	When User adds a Behavior Record with Severity "Bad", Description "Fight" for prisoner with PIDN 11221
	And User adds a Behavior Record with Severity "Bad", Description "Cop Fight" for prisoner with PIDN 11221
	Then The prisoner has 2 behavior records in total