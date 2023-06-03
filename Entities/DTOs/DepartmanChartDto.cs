using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class DepartmanChartDto
    {
        public string? DepartmantName { get; set; }
        public int TotalUseResource { get; set; }
        public int TotalResource { get; set; }
    }
}
