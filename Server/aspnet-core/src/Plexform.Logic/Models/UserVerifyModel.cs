using System;
using System.Collections.Generic;
using System.Text;

namespace Plexform.Models
{
    public class UserVerifyModel
    {
        //[Required]
        //[StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserName { get; set; }

        public string FieldCond { get; set; }
    }

   
    public class EditApprovalDto 
    {
        public string userID { get; set; }
        public string Status { get; set; }
        public string statusDesc { get; set; }
        public string RejectRemark { get; set; }
    }
}
