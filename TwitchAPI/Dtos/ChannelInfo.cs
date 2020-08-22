using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI
{
    [DataContract]
    public class ChannelInfo
    {
        [DataMember]
        public int ID { get; private set; }

        [DataMember]
        public string DisplayName { get; private set; }

        [DataMember]
        public string GameName { get; private set; }

        [DataMember]
        public string Status { get; private set; }

        [DataMember]
        public Uri LogoPath { get; private set; }

        public ChannelInfo(int id, string displayName, string gameName, string status, Uri logoPath)        
        {
            ID = id;
            DisplayName = displayName;
            GameName = gameName;
            Status = status;
            LogoPath = logoPath;
        }
    }
}
