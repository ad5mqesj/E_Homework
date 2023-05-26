using Dapr.Client;
using Microsoft.Extensions.Logging;
using Moq;
using Protect.Events.DTO.Validators;
using Protect.Events.DTO.Models;
using Xunit;
using System;
using FluentAssertions;
using FluentValidation;
using System.Threading.Tasks;
using Protect.Event.Command.Data.Interface;
using Protect.Event.Command.Controllers;
using Microsoft.AspNetCore.Mvc;
using Grpc.Core;

namespace Protect.Event.CommandService.Test
{
    public class EventCommandControllerTests
    {
        private readonly IEventCommand _mockEventprovider;        
        private readonly EventCommandController _evtcmdController;
        public EventCommandControllerTests()
        {
            _mockEventprovider = new MoqProvider().MockeventBusProvider.Object;
            var logger = new LoggerFactory().CreateLogger<EventCommandController>();
            var dtovalidator = new EventDtoValidator();
            _evtcmdController = new EventCommandController(logger, _mockEventprovider,dtovalidator);
        }

            [Fact]
            public Task PublishuEventDtoHappyPath()
            {
                var source = new EventDto()
                {

                    Class = EventClassifications.Other,
                    Domain = EventDomains.Other,
                    EventType = "not empty",
                    Host = "host.domaian",
                    HostIp = "10.10.10.10",
                    OriginationTime = DateTimeOffset.UtcNow,
                    RecievedTime = DateTimeOffset.UtcNow,
                    Producer = "xUnit",
                    UserName = "somebody@somewhere.com",
                    ApplicationVersion = "1.0.0.0",
                    WorkgroupId = "12",
                    CountryId = "11",
                    LocationId = "11",
                    BusinessUnitId = "11",
                    Rule = "Rule1",
                    Trigger = "once",
                    Occurrences = 1,
                    AgentSession = "Agent session",
                    IsQueued = true,
                    AlertEventType = AlertEvents.Operations,
                    AlertSubType = "Useralert subtype",
                    Alert = "User Alert",
                    CallStatus = "Ready",
                    WindowName = "window1",
                    UserEventlabel = "userevt label",
                    AuditId = "Audit1"
                };
                var target = _evtcmdController.InsertEvent(source);
                var result = (OkObjectResult)target.Result;
                result.Should().NotBeNull();
                Assert.IsType<OkObjectResult>(result);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.Contains("Published successfully", result.Value.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return Task.CompletedTask;
            }

            [Fact]
            public Task PublishWithUninitializedEventTypeThrowsValidationException()
            {
                var source = new EventDto()
                {
                    Class = EventClassifications.Other,
                    Domain = EventDomains.Other,               
                    Host = "host.domaian",
                    HostIp = "10.10.10.10",
                    OriginationTime = DateTimeOffset.UtcNow,
                    RecievedTime = DateTimeOffset.UtcNow,
                    Producer = "xUnit",
                    UserName = "somebody@somewhere.com",
                    ApplicationVersion = "1.0.0.0",
                    WorkgroupId = "12",
                    CountryId = "11",
                    LocationId = "11",
                    BusinessUnitId = "11",
                    Rule = "Rule1",
                    Trigger = "once",
                    Occurrences = 1,
                    AgentSession = "Agent session",
                    IsQueued = true,
                    //AlertEventType = AlertEvents.Operations,
                    AlertSubType = "Useralert subtype",
                    Alert = "User Alert",
                    CallStatus = "Ready",
                    WindowName = "window1",
                    UserEventlabel = "userevt label",
                    AuditId = "Audit1"
                };
                var target = _evtcmdController.InsertEvent(source);
                var result = (BadRequestObjectResult)target.Result;
                result.Should().NotBeNull();
                Assert.IsType<BadRequestObjectResult>(result);
                Assert.True(result.StatusCode == 400);
                return Task.CompletedTask;
            }       
    }
}