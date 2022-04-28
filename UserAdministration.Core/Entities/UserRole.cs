using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdministration.Core.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public int Id_user { get; set; }
        public int Id_role { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
        public string State { get; set; }
    }
}
