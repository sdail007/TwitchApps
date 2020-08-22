using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Command
{
    public interface ICommandHandler<TCommand>
        where TCommand: CommandBase
    {
        Task HandleAsync(TCommand command);
    }
}
