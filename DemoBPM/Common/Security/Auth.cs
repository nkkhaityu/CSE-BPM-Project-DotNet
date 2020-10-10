using System.Linq;
using DemoBPM.Database;

namespace DemoBPM.Common.Security
{
    public static class Auth
    {
        public static string Login(string userName, string password)
        {
            using (Entities _db = new Entities())
            {
                var hashedPwd = AuthSuport.GetMD5(password);
                var user = _db.tbUsers.Where(x => x.UserName.Trim() == userName && x.Password.Trim() == hashedPwd).FirstOrDefault();
                if (user == null)
                    return "Invalid UserName or Password.";

                SessionExtensions.Set(SessionExtensions.key_UserId, user.ID.ToString());
                SessionExtensions.Set(SessionExtensions.key_UserName, user.UserName);

                //FormsAuthentication.SetAuthCookie(user.UserName, true);
                //FormsAuthentication.SetAuthCookie(user.ID.ToString(), true);
                //var value = FormsAuthentication.GetAuthCookie(user.UserName, true);
                //var value1 = FormsAuthentication.GetAuthCookie(user.ID.ToString(), true);

                return "true";
            }
        }

    }
}