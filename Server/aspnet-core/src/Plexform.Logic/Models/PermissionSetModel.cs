using System;
using System.Collections.Generic;
using System.Text;

namespace Plexform.Logic.Models
{
    public class PermissionSetModel
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public bool Expanded { get; set; }
        public IList<eSWIS.Logic.UserSecurity.Container.PermissionSet> Functions { get; set; }
    }
}
