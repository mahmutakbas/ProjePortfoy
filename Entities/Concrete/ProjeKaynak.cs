namespace Entities.Concrete
{
    public class ProjeKaynak : IBaseEntitiy
    {
        public int ProjeId { get; set; }
        public int KaynakId { get; set; }
        public int KaynakMiktari { get; set; }
    }
}
