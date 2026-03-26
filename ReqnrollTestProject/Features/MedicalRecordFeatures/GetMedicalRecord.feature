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

Scenario: Attempt to get medical record for non-existing prisoner
    Given There are prisoners in the database.
    When User tries to get Medical Record for prisoner with PIDN 00001
    Then The system should show an error saying "No Prisoner Found With This PIDN"

 Scenario: Attempt to get medical record for existing prisoner who has no medical record
    Given There are prisoners in the database.
    And The prisoner with PIDN 343123 does not have a medical record
    When User tries to get Medical Record for prisoner with PIDN 343123
    Then The system should show an error saying "No Med Records Available"

 Scenario: Attempt to get medical record without proper authorization (wrong role)
    Given There are prisoners in the database.
    And The prisoner with PIDN 11221 has a medical record.
    And User is assigned with "Correctional Officer" role
    When User tries to get Medical Record for prisoner with PIDN 11221
    Then The system should show an error saying "You are not authorized to perform this actions!"
