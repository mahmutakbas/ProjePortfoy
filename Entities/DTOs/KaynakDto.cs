using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class KaynakDto:IBaseEntitiy
    {
        public string? Name { get; set; }
        public string? DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public int Item { get; set; }
    }
}
