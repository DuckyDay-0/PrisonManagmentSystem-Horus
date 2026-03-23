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
	Given There are no prisoners in the database
	When User tries to get all prisoners
	Then The system will return an exception
	
Scenario: User is trying to get prisoner by ID
	Given There are three prisoners in the database
	When User tries to get prisoner with ID 343123
	Then The system will return prisoner with ID 3

Scenario: User is trying to get prisoner by FirstName and LastName
	Given There are three prisoners in the database
	When User tries to get prisoner with FirstName "Michel" and LastName "Thist"
	Then The system will return prisoner with Name "Michel" and ID 2

Scenario: User tries to get prisoner with the wrong or non existing FirstName and LastName
	Given There are three prisoners in the database
	When User tries to get prisoner with FirstName "Tony" and LastName "Habib"
	But Prisoner with this Name does not exists
	Then The system will throw an error message