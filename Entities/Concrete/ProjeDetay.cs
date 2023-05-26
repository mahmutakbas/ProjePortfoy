namespace Entities.Concrete
{
    public class ProjeDetay : IBaseEntitiy
    {

        public int ProjeId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? Name{ get; set; }
        public string? Status { get; set; }
        public int KaynakId { get; set; }
        public int KaynakMiktari { get; set; }
    }
}
