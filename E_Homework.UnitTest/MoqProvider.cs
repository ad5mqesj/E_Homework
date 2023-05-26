using Moq;
using Protect.Event.Command.Data.Interface;
using Protect.Events.DTO.Models;
using System.Threading.Tasks;

namespace Protect.Event.CommandService.Test
{
    public class MoqProvider
    {
        public Mock<IEventCommand> MockeventBusProvider { get; set; }

        
        public MoqProvider()
        {
            this.MockeventBusProvider = new Mock<IEventCommand>();
        
            SetupAlertEventsProvider();
        }
        private void SetupAlertEventsProvider()
        {
            this.MockeventBusProvider.Setup(x => x.Publish(It.IsAny<EventDto>())).Returns(Task.CompletedTask);
        }
    }
}