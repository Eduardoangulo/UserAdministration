using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserAdministration.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<Roles> Roles { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
        public string State { get; set; }
    }
}
