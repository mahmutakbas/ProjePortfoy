using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IProjeDal : IBaseRepository<Proje>
    {
        Task<bool> IsExist(Proje entity);
    }
    public class ProjeDal : IProjeDal
    {
        public async Task<int> Add(Proje entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Projes (ProjeAdi, BaslangicTarihi, BitisTarihi, ProjeAciklama, ProjeDurum, ProjeMusteri, ProjeButcesi,                    ProjeKategoriId, DepartmanId, Strateji, ProjeGeliri, ProjeGideri) VALUES (@ProjeAdi, @BaslangicTarihi, @BitisTarihi, @ProjeAciklama, @ProjeDurum, @ProjeMusteri, @ProjeButcesi,@ProjeKategoriId, @DepartmanId, @Strateji, @ProjeGeliri, @ProjeGideri);", new
                {
                    ProjeAdi = entity.ProjeAdi,
                    BaslangicTarihi = entity.BaslangicTarihi,
                    BitisTarihi = entity.BitisTarihi,
                    ProjeAciklama = entity.ProjeAciklama,
                    ProjeDurum = entity.ProjeDurum,
                    ProjeMusteri = entity.ProjeMusteri,
                    ProjeButcesi = entity.ProjeButcesi,
                    ProjeKategoriId = entity.ProjeKategoriId,
                    DepartmanId = entity.DepartmanId,
                    Strateji = entity.Strateji,
                    ProjeGeliri = entity.ProjeGeliri,
                    ProjeGideri = entity.ProjeGideri
                });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM Projes WHERE ID = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<Proje> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Proje>("SELECT * FROM Projes WHERE ID = @Id", new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Proje>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<Proje>("SELECT * FROM Projes");
                return result;
            }
        }

        public async Task<bool> IsExist(Proje entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Proje>("SELECT * FROM Projes WHERE ProjeAdi=@ProjeAdi AND ID <> @Id", new { ProjeAdi = entity.ProjeAdi, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<int> Update(Proje entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE Projes SET ProjeAdi = @ProjeAdi, BaslangicTarihi=@BaslangicTarihi, BitisTarihi=@BitisTarihi, ProjeAciklama=@ProjeAciklama, ProjeDurum=@ProjeDurum, ProjeMusteri=@ProjeMusteri, ProjeButcesi=@ProjeButcesi,                    ProjeKategoriId=@ProjeKategoriId, DepartmanId=@DepartmanId, Strateji=@Strateji, ProjeGeliri=@ProjeGeliri, ProjeGideri=@ProjeGideri WHERE ID =@Id", new
                {
                    Id = entity.Id,
                    ProjeAdi = entity.ProjeAdi,
                    BaslangicTarihi = entity.BaslangicTarihi,
                    BitisTarihi = entity.BitisTarihi,
                    ProjeAciklama = entity.ProjeAciklama,
                    ProjeDurum = entity.ProjeDurum,
                    ProjeMusteri = entity.ProjeMusteri,
                    ProjeButcesi = entity.ProjeButcesi,
                    ProjeKategoriId = entity.ProjeKategoriId,
                    DepartmanId = entity.DepartmanId,
                    Strateji = entity.Strateji,
                    ProjeGeliri = entity.ProjeGeliri,
                    ProjeGideri = entity.ProjeGideri
                });
                return result;
            }
        }
    }
}
