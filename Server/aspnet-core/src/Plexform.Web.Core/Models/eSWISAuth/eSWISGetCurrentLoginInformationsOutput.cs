using Plexform.Sessions.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plexform.Models.eSWISAuth
{
    public class eSWISGetCurrentLoginInformationsOutput
    {
        public ApplicationInfoDto Application { get; set; }

        public UserLoginInfoDto User { get; set; }
    }
}
