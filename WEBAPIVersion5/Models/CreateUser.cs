namespace WEBAPIVersion5.Models
{
    public class CreateUser
    {
        public string firstName { get; set; }

        public string lastName { get; set; }
        public string DisplayName { get; set; }
        public string MailNickname { get; set; }

      //  public string UserPrincipalName { get; set; }

        public string JobTitle { get; set; }

        public string UserType { get; set; }

        public string CompanyName { get; set; }

        public string Department { get; set; }

        public string OfficeLocation { get; set; }

        public string City { get; set; }
        public string Mail { get; set; }
    }
}
