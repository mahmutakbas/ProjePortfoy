namespace Entities.Concrete
{
    public class ProjeDetay : IBaseEntitiy
    {
        public int ProjeId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? Aciklama{ get; set; }
        public string? Durum { get; set; }
    }
}
