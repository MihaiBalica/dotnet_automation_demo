Feature: NiFi Processors Management
    As a NiFi administrator
    I want to manage processors
    So that I can control data processing flows

Background:
    Given I am authenticated with NiFi

Scenario: List all processors in root process group
    When I list all processors in process group "root"
    Then I should receive a successful response
    And the processors list should not be null

Scenario: Start a processor
    Given I have a processor in "root" process group
    When I start the processor
    Then the processor should be in "RUNNING" state

Scenario: Stop a processor
    Given I have a running processor in "root" process group
    When I stop the processor
    Then the processor should be in "STOPPED" state

Scenario: List processors and count them
    When I list all processors in process group "root"
    Then I should receive a successful response
    And I should have at least 0 processors
