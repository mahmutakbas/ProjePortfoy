namespace Entities.Concrete
{
    public class Kaynak : IBaseEntitiy
    {
        public string? KaynakAdi { get; set; }
        public int DepartmanId { get; set; }
        public int? KaynakMiktari { get; set; } //10 adet vinç gibi
        public int? KalanMiktar { get; set; }//100
    }
}
  