using System.ComponentModel.DataAnnotations;

namespace UserAuthentication.Data.Entities
{
    public class User
    {
        [Key]
        public int PersonId { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Lastname { get; set; }

        public bool IsAdministrator { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
