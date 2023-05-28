namespace Entities.Concrete
{
    public class Risk : IBaseEntitiy
    {
        public int ProjeId { get; set; }
        public string? RiskTanimi { get; set; }
        public string? RiskDurumu { get; set; }

        /*
         id int
        proje id int
        name string
        status string
         */
    }
}
