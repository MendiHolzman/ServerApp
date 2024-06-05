using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Model
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string Password { get; set; }

        public override string ToString()
        {
            return string.Format($"User ID: {ID},  Name: {Name},  Email: {Email},  Password: {Password}\n");
        }

    }
}
