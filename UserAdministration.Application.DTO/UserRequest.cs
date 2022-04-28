using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserAdministration.Application.DTO
{
    public class UserRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        public List<RoleRequest> Roles { get; set; }
    }
}
