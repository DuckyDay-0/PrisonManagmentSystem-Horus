Feature: GetPrisoner
User wants to get prisoners either by 
name or either just all prisoners with the correct or incorrect role 
Background: 
Given The system is ready to get prisoners


@GetPrisoner
Scenario: User is trying to get all prisoners
	Given There are three prisoners in the database
	When User tries to get all prisoners 
	Then The system will return all prisoners

Scenario: There are no prisoners in the database
	When User tries to get all prisoners
	Then The system will return an exception
	