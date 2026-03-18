Feature: GetPrisoner
User wants to get prisoners either by 
name or either just all prisoners
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
	
Scenario: User is trying to get prisoner by ID
	Given There are three prisoners in the database
	When User tries to get prisoner with ID 3
	Then The system will return prisoner with ID 3

Scenario: User is trying to get prisoner by Name
	Given There are three prisoners in the database
	When User tries to get prisoner with Name "Michel"
	Then The system will return prisoner with Name "Michel" and ID 2
