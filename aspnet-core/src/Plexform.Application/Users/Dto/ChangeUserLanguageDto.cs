using System.ComponentModel.DataAnnotations;

namespace Plexform.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}