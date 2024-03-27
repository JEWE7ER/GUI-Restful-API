using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models;

namespace GUI.Models
{
    public class UserAccount
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public List<string>? Rights { get; set; }
        public bool Write { get {return Rights!.Contains("WRITE");}}
    }
}
