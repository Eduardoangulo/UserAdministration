using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdministration.Core.Entities
{
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }
        public string State { get; set; }
    }
}
