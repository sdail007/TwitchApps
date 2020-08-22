using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Command;
using TwitchAPI.Endpoints;

namespace TwitchAPI.V5.Commands
{
    public abstract class KrakenCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : CommandBase
    {
        protected KrakenEndpoint Endpoint { get; }

        public KrakenCommandHandler(KrakenEndpoint endpoint)
        {
            Endpoint = endpoint;
        }

        public abstract Task HandleAsync(TCommand command);
    }
}
