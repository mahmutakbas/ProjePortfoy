namespace Entities.Concrete
{
    public class ProjeKPI : IBaseEntitiy
    {
        public int ProjeId { get; set; }
        public string? Name { get; set; }
        public int Goal { get; set; }
    }
}
