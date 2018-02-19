using Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace Plexform.Models
{
    public class ESWISAuthModel
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }
    }
}
