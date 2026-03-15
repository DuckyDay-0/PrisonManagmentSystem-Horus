Feature: AddPrisoner
As staff member
User wants to add prisoner
So that he can manage the database

Background: 
Given The system is ready for prisoner to be added

@AddPrisoner
Scenario: Successfully adding a prisoner with valid data
	When User adds a prisoner with FirsName "Test", LastName "Prisoner", Age 30, crime "Test Crime", Entry Date "03/05/2026", Sentence Lenght 1, Release Date "03/05/2027", Prison Block "O Block", Prison Cell 1
	Then Prisoner will be added, an ID will be generated

Scenario: Trying to add a prisoner without the correct role(not authorized to add prisoners)
	Given User is not with an "Admin" role
	When User adds a prisoner with FirsName "Test 1", LastName "Prisoner 1", Age 30, crime "Test Crime", Entry Date "03/05/2026", Sentence Lenght 1, Release Date "03/05/2027", Prison Block "O Block", Prison Cell 1
	Then No prisoner will be added

Scenario: Trying to add a prisoner with invalid Name
	Given User is with an "Admin" role
	When User tries to add a prisoner with no Name
	Then User receives an exception and no prisoner is added