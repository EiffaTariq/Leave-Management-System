using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Compare("Password")]
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; }

    }

}
