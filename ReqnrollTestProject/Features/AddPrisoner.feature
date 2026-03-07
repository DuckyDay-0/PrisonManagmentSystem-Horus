Feature: AddPrisoner
As staff member
I wanto to add prisoner
So that i can manage the database

@AddPrisoner
Scenario: Successfully adding a prisoner with valid data
	Given The prison capacity is 100
	When I add a prisoner with Name "Test Prisoner", Age 30, crime "Test Crime", Entry Date "03/05/2026", Sentence Lenght 1, Release Date "03/05/2027"
	Then Prisoner will be added, an ID will be generated and the Current Prisoner Count will increase

Scenario: Trying to add a prisoner without the correct role(not authorized to add prisoners)
	Given The prison capacity is 50 and im not authorized to add prisoners
	When I try to add a prisoner with Name "Test Prisoner", Age 30, crime "Test Crime", Entry Date "03/05/2026", Sentence Lenght 1, Release Date "03/05/2027"
	Then I receive an exception message

