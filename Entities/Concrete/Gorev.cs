namespace Entities.Concrete
{
    public class Gorev : IBaseEntitiy
    {

        public int ProjeId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public bool TamamlanmaDurumu { get; set; }
        public string? Aciklama { get; set; }
        public string? GorevAdi { get; set; }
    }
}
