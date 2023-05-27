using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate{ get; set; }
        public string? Status { get; set; }
        public decimal Budget { get; set; }
        public decimal Revenue { get; set; }
        public string? Description { get; set; }
        public string? Customer { get; set; }
        public int DepartmentId { get; set; }
        public int manCount { get; set; }
        public int ResourcePercent { get; set; }
        public KPI[] Kpis { get; set; }
        public ProjeDetay[] Subtasks { get; set; }
        public Risk[] risks { get; set; }

        /*
         Type String 
         
         */
    }
}
