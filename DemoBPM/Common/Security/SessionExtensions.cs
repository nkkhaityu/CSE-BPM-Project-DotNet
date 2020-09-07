using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoBPM.Common.Security
{
    public class SessionExtensions
    {
        public static Dictionary<string, string> _session = new Dictionary<string, string>();
        public static void Set(string key, string value)
        {
            //if (!_session.ContainsKey(key))
            _session[key] = value;
        }

        public static string Get(string key)
        {
            var value = _session[key];

            return value;
        }

        public static void Clear()
        {
            _session.Clear();
        }

        public static string key_UserId = "UserId";
        public static string key_UserName = "UserName";
    }
}