namespace Entities.Concrete
{
    public class Proje : IBaseEntitiy
    {

        public string? ProjeAdi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? ProjeAciklama { get; set; }
        public bool ProjeDurum { get; set; }
        public string? ProjeMusteri { get; set; }
        public decimal ProjeButcesi { get; set; }
        public int ProjeKategoriId { get; set; }
        public int DepartmanId { get; set; }
        public int Strateji { get; set; }
        public decimal ProjeGeliri { get; set; }
        public decimal ProjeGideri { get; set; }
    }
}


