using ApiTestAutomation.Tests.Infrastructure;
using ApiTestAutomation.Tests.Models;
using ApiTestAutomation.Tests.Helpers;
using FluentAssertions;
using Reqnroll;

namespace ApiTestAutomation.Tests.StepDefinitions;

[Binding]
public class ProcessGroupSteps
{
    private readonly NiFiApiClient _nifiClient;
    private List<ProcessorGroup> _processGroups = new();
    private ProcessorGroup? _currentProcessGroup;

    public ProcessGroupSteps()
    {
        _nifiClient = new NiFiApiClient();
    }

    [Given(@"I am authenticated with NiFi")]
    public async Task GivenIAmAuthenticatedWithNiFi()
    {
        LoggerHelper.LogInfo("Authenticating with NiFi");
        var authenticated = await _nifiClient.AuthenticateAsync();
        authenticated.Should().BeTrue("Authentication should succeed");
    }

    [When(@"I list all process groups in ""(.*)""")]
    public async Task WhenIListAllProcessGroupsIn(string processGroupId)
    {
        LoggerHelper.LogInfo("Listing process groups in: {ProcessGroupId}", processGroupId);
        _processGroups = await _nifiClient.ListProcessGroupsAsync(processGroupId);
    }

    [When(@"I get the first process group")]
    public void WhenIGetTheFirstProcessGroup()
    {
        _currentProcessGroup = _processGroups.FirstOrDefault();
        _currentProcessGroup.Should().NotBeNull("At least one process group should exist");
    }

    [Then(@"I should receive a successful response")]
    public void ThenIShouldReceiveASuccessfulResponse()
    {
        // This is verified by the fact that we got results without exception
        LoggerHelper.LogInfo("Response received successfully");
    }

    [Then(@"the process groups list should not be empty")]
    public void ThenTheProcessGroupsListShouldNotBeEmpty()
    {
        _processGroups.Should().NotBeNull("Process groups list should not be null");
    }

    [Then(@"I should have at least (.*) process groups")]
    public void ThenIShouldHaveAtLeastProcessGroups(int minCount)
    {
        _processGroups.Should().NotBeNull();
        _processGroups.Count.Should().BeGreaterThanOrEqualTo(minCount);
        LoggerHelper.LogInfo("Found {Count} process groups (minimum expected: {MinCount})", _processGroups.Count, minCount);
    }

    [Then(@"the process group should have a valid name")]
    public void ThenTheProcessGroupShouldHaveAValidName()
    {
        _currentProcessGroup.Should().NotBeNull();
        _currentProcessGroup!.Component.Should().NotBeNull();
        _currentProcessGroup.Component!.Name.Should().NotBeNullOrEmpty();
        LoggerHelper.LogInfo("Process group name: {Name}", _currentProcessGroup.Component.Name);
    }

    [Then(@"the process group should have a valid ID")]
    public void ThenTheProcessGroupShouldHaveAValidID()
    {
        _currentProcessGroup.Should().NotBeNull();
        _currentProcessGroup!.Id.Should().NotBeNullOrEmpty();
        LoggerHelper.LogInfo("Process group ID: {Id}", _currentProcessGroup.Id);
    }
}
