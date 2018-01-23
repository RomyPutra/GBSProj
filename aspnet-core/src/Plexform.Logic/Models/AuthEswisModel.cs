using System;
using System.Collections.Generic;
using System.Text;

namespace Plexform.Logic.Models
{
    public class AuthEswisModel
    {
        public int AppID { get; set; }
        public string AccessCode { get; set; }
        public Dictionary<string, bool> Permissions { get; set; } 
    }
}
