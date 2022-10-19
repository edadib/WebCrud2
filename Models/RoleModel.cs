using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebCrud2.Models
{
    public class RoleModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Please enter your email.", AllowEmptyStrings = false)]
        [DisplayName("Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Please enter your password.", AllowEmptyStrings = false)]
        [DisplayName("Password")]
        public string password { get; set; }
        public DateTime create_date { get; set; }
        public DateTime? update_date { get; set; }
    }
}
