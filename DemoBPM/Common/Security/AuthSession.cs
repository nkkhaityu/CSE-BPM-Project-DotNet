using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace DemoBPM.Common.Security
{
    public class AuthSession
    {
        private AuthSession()
        {

        }

        public int UserId { get; set; }
        public string UserName { get; set; }

        private static AuthSession RefreshInternal()
        {
            try
            {
                var _session = new AuthSession();
                _session.UserId = int.Parse(SessionExtensions.Get(SessionExtensions.key_UserId));
                _session.UserName = SessionExtensions.Get(SessionExtensions.key_UserName);

                return _session;
            }
            catch (Exception ex)
            {
                throw new Exception(HttpStatusCode.Unauthorized.ToString());
            }
        }

        public static AuthSession Current
        {
            get
            {
                return RefreshInternal();
            }
        }
    }
}