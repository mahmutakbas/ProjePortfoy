using Entities.Concrete;

namespace Entities.DTOs
{
    public class ProjeDetayDto : IBaseEntitiy
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
