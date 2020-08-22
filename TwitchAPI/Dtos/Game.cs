using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI
{
    [DataContract]
    public class Game
    {
        [DataMember]
        public string Title { get; private set; }

        [DataMember]
        public Uri Image { get; private set; }

        public Game(string title, Uri image)
        {
            Title = title;
            Image = image;
        }

        public override bool Equals(object obj)
        {
            if (obj is Game other)
            {
                return other.GetHashCode() == GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}
