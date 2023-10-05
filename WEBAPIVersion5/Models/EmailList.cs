namespace WEBAPIVersion5.Models
{
    //public class EmailList
    //{
    //    public List<string> Emails { get; set; }

    //}
    public class EmailList
    {
    //   public string TenantId { get; set; }
        public string ClientId { get; set; }
      //  public string ClientSecretId { get; set; }
        public List<EmailInfo> Emails { get; set; }
    }

    public class EmailInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }

    }
}
