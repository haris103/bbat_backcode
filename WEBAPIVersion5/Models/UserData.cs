using System.ComponentModel.DataAnnotations;

namespace WEBAPIVersion5.Models
{
    public class Userdata
    {
        public int id { get; set; }
        public string employee_id { get; set; }
        public string employee_name { get; set; }
        //[Required]
        //public string employee_role { get; set; }
        public string employee_token { get; set; }

    }
}
