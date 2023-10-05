namespace WEBAPIVersion5.Models
{
    public class Security
    {
        public Guid id { get; set; }
        public string employee_id { get; set; }
        public Guid team_id { get; set; }
        public string access_rights { get; set; }
        public string security_role { get; set; }
        public Guid businessunit_id { get; set; }


    }
}
