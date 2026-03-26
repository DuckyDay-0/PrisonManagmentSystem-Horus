Feature: UpdatePrisoner
User tries to update prisoner

Background: 
Given The system is ready for prisoner to be updated
And There are prisoners in the database to be updated

@UpdatePrisoner
Scenario: Successfully update a prisoner's first name
    And User is logged in with "Admin" role
    When User chooses option number 1 on the menu to updates the FirstName of prisoner with PersonalIDNumber 11221 to "Alexander"
    Then The prisoner's FirstName should be successfully updated to "Alexander"

  Scenario: Successfully update sentence length and recalculate release date
    And User is logged in with "Admin" role
    When User choose option number 4 on the menu to updates the Sentence Length of prisoner with PersonalIDNumber 11221 to 5 years
    Then The Sentence Length should be updated to 5
    And The Release Date should be automatically recalculated

  Scenario: Attempt to update prisoner without admin rights
    And User is logged in with "Correctional Officer" role
    When User choose option number 2 on the menu to update the Age of prisoner with PersonalIDNumber 11221 to 35
    Then The system will show an error message saying "You don't have the authorization to perform this action!"

  Scenario: Attempt to update non-existing prisoner
    Given User is logged in with "Admin" role
    When User chooses option number 3 on the menu to update the Crime of prisoner with PersonalIDNumber 999999999 to "Assault"
    Then The system will show an error message saying "No Prisoner found"
