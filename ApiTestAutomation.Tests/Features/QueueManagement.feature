Feature: NiFi Queue Management
    As a NiFi administrator
    I want to monitor queues
    So that I can track flow file processing

Background:
    Given I am authenticated with NiFi

Scenario: List all connections in root process group
    When I list all connections in process group "root"
    Then I should receive a successful response
    And the connections list should not be null

Scenario: Count flow files in a queue
    Given I have a connection in "root" process group
    When I get the queue count for the connection
    Then the queue count should be a valid number
    And the queue count should be greater than or equal to 0

Scenario: Verify queue has valid properties
    When I list all connections in process group "root"
    And I get the first connection
    Then the connection should have a valid ID
    And the connection should have source and destination
