namespace Entities.Concrete
{
    public class Risk : IBaseEntitiy
    {
        public int ProjeId { get; set; }
        public string? RiskTanimi { get; set; }
        public string? RiskKategorisi { get; set; }
        public int Olasilik { get; set; }
        public int Etki { get; set; }
        public int RiskSkoru { get; set; }
        public int RiskOnceligi { get; set; }
        public bool RiskDurumu { get; set; }

        /*
         id int
        proje id int
        name string
        status string
         */
    }
}
