using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Utilities
{
    public static class JsonExtensions
    {
        public static string[] ToArray(this JsonValue jsonValue)
        {
            string[] array = new string[jsonValue.Count];

            for (int i = 0; i < jsonValue.Count; i++)
            {
                array[i] = jsonValue[i];
            }

            return array;
        }
    }
}
