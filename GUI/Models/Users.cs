using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models;

namespace GUI.Models
{
    internal static class Users
    {
        public static bool IsLoggedIn { get; set; } = false;
        public static UserAccount? account { get; set; }
        public static void SaveInfo(string name, string pass, List<string> rights)
        {
            account = new UserAccount
            {
                Rights = rights,
                Username = name,
                Password = pass
            };
        }
    }
}
