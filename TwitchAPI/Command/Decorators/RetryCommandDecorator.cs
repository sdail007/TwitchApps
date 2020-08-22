using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Command.Decorators
{
    public class RetryCommandDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand: CommandBase
    {
        private readonly ICommandHandler<TCommand> decorated;

        public RetryCommandDecorator(ICommandHandler<TCommand> decorated)
        {
            this.decorated = decorated;
        }

        public async Task HandleAsync(TCommand command)
        {
            int delay = 1;

            while (true)
            {
                try
                {
                    await decorated.HandleAsync(command);
                    return;
                }
                catch (HttpRequestException)
                {
                    await Task.Delay(delay * 1000);

                    //Increase delay exponentially, capped at 60
                    delay = Math.Min(delay * 2, 60);
                }
            }
        }
    }
}
