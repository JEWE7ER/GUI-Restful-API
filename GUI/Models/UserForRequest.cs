using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models
{
    class UserForRequest(string username, string password)
    {
        public string login { get; set; } = username;
        public string password { get; set; } = password;
    }
}
