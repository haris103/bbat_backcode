namespace WEBAPIVersion5.Models
{
    public class Teamsystem
    {
        public Guid id { get; set; }
        public string team_name { get; set; }
        public string businesunit_name { get; set; }
        public Guid systemunit_id { get; set; }
        public string role { get; set; }
        public string access_rights { get; set; }

    }
}
