using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ProjeDDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ProjeKategori Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string? Status { get; set; }
        public decimal Budget { get; set; }
        public decimal Revenue { get; set; }
        public string? Description { get; set; }
        public string? Customer { get; set; }
        public Departman DepartmentId { get; set; }
        public int manCount { get; set; }
        public int ResourcePercent { get; set; }
        public KPIDto[] Kpis { get; set; }
        public ProjeDetayDto[] Subtasks { get; set; }
        public RiskDto[] risks { get; set; }
        public KaynakDto[] Resources { get; set; }
    }
}
