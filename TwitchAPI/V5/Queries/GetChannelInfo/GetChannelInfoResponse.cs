using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Query.GetChannelInfo
{
    public class GetChannelInfoResponse : ResponseBase
    {
        public ChannelInfo ChannelInfo { get; private set; }

        public GetChannelInfoResponse(ChannelInfo channelInfo)
        {
            ChannelInfo = channelInfo;
        }
    }
}
