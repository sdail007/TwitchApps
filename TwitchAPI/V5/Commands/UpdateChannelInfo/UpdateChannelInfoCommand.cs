using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Command;
using TwitchAPI.Endpoints;

namespace TwitchAPI.V5.Commands.UpdateChannelInfo
{
    public class UpdateChannelInfoCommand : CommandBase
    {
        public int ChannelID { get; }
        public string Status { get; }
        public string Game { get; }

        public UpdateChannelInfoCommand(int channelID, string status, string game)
        {
            ChannelID = channelID;
            Status = status;
            Game = game;
        }
    }
}
