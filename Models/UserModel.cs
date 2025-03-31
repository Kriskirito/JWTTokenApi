using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTTestproj.Models
{

    [Table("user_table")]
    public class User_Table
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
    public class UserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
