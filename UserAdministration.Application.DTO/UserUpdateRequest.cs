using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserAdministration.Application.DTO
{
    public class UserUpdateRequest
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public List<RoleRequest> Roles { get; set; }
    }
}
