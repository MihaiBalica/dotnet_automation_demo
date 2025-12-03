using ApiTestAutomation.Tests.Infrastructure;
using ApiTestAutomation.Tests.Models;
using ApiTestAutomation.Tests.Helpers;
using FluentAssertions;
using Reqnroll;

namespace ApiTestAutomation.Tests.StepDefinitions;

[Binding]
public class QueueManagementSteps
{
    private readonly NiFiApiClient _nifiClient;
    private List<Connection> _connections = new();
    private Connection? _currentConnection;
    private int _queueCount;

    public QueueManagementSteps()
    {
        _nifiClient = new NiFiApiClient();
    }

    [When(@"I list all connections in process group ""(.*)""")]
    public async Task WhenIListAllConnectionsInProcessGroup(string processGroupId)
    {
        LoggerHelper.LogInfo("Listing connections in process group: {ProcessGroupId}", processGroupId);
        await _nifiClient.AuthenticateAsync();
        _connections = await _nifiClient.ListConnectionsAsync(processGroupId);
    }

    [Given(@"I have a connection in ""(.*)"" process group")]
    public async Task GivenIHaveAConnectionInProcessGroup(string processGroupId)
    {
        await _nifiClient.AuthenticateAsync();
        _connections = await _nifiClient.ListConnectionsAsync(processGroupId);
        _currentConnection = _connections.FirstOrDefault();
        
        if (_currentConnection == null)
        {
            LoggerHelper.LogWarning("No connections found in process group: {ProcessGroupId}", processGroupId);
        }
    }

    [When(@"I get the queue count for the connection")]
    public async Task WhenIGetTheQueueCountForTheConnection()
    {
        if (_currentConnection != null)
        {
            _queueCount = await _nifiClient.GetQueueCountAsync(_currentConnection.Id);
            LoggerHelper.LogInfo("Queue count: {Count}", _queueCount);
        }
    }

    [When(@"I get the first connection")]
    public void WhenIGetTheFirstConnection()
    {
        _currentConnection = _connections.FirstOrDefault();
        _currentConnection.Should().NotBeNull("At least one connection should exist");
    }

    [Then(@"the connections list should not be null")]
    public void ThenTheConnectionsListShouldNotBeNull()
    {
        _connections.Should().NotBeNull("Connections list should not be null");
        LoggerHelper.LogInfo("Found {Count} connections", _connections.Count);
    }

    [Then(@"the queue count should be a valid number")]
    public void ThenTheQueueCountShouldBeAValidNumber()
    {
        _queueCount.Should().BeGreaterThanOrEqualTo(0, "Queue count should be a valid non-negative number");
    }

    [Then(@"the queue count should be greater than or equal to (.*)")]
    public void ThenTheQueueCountShouldBeGreaterThanOrEqualTo(int minCount)
    {
        _queueCount.Should().BeGreaterThanOrEqualTo(minCount);
        LoggerHelper.LogInfo("Queue count {Count} is >= {MinCount}", _queueCount, minCount);
    }

    [Then(@"the connection should have a valid ID")]
    public void ThenTheConnectionShouldHaveAValidID()
    {
        _currentConnection.Should().NotBeNull();
        _currentConnection!.Id.Should().NotBeNullOrEmpty();
        LoggerHelper.LogInfo("Connection ID: {Id}", _currentConnection.Id);
    }

    [Then(@"the connection should have source and destination")]
    public void ThenTheConnectionShouldHaveSourceAndDestination()
    {
        _currentConnection.Should().NotBeNull();
        _currentConnection!.Component.Should().NotBeNull();
        _currentConnection.Component!.Source.Should().NotBeNull("Connection should have a source");
        _currentConnection.Component.Destination.Should().NotBeNull("Connection should have a destination");
        
        LoggerHelper.LogInfo("Connection from {Source} to {Destination}", 
            _currentConnection.Component.Source?.Name ?? "Unknown",
            _currentConnection.Component.Destination?.Name ?? "Unknown");
    }
}
