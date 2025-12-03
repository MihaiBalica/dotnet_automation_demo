Feature: NiFi Process Groups Management
    As a NiFi administrator
    I want to manage process groups
    So that I can organize and monitor my data flows

Background:
    Given I am authenticated with NiFi

Scenario: List all process groups in root
    When I list all process groups in "root"
    Then I should receive a successful response
    And the process groups list should not be empty

Scenario: List process groups and verify count
    When I list all process groups in "root"
    Then I should receive a successful response
    And I should have at least 0 process groups

Scenario: Verify process group has a name
    When I list all process groups in "root"
    And I get the first process group
    Then the process group should have a valid name
    And the process group should have a valid ID
