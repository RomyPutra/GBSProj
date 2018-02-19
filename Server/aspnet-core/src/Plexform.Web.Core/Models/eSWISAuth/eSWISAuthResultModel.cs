namespace Plexform.Models
{
    public class ESWISAuthResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public string UserId { get; set; }

        public string GroupName { get; set; }
    }
}
