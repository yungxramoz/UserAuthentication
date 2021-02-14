using System.ComponentModel.DataAnnotations;

namespace UserAuthentication.Data.Entities
{
    public class User
    {
        [Key]
        public int PersonId { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Lastname { get; set; }

        [StringLength(64)]
        public byte[] PasswordHash { get; set; }

        [StringLength(128)]
        public byte[] PasswordSalt { get; set; }

    }
}
