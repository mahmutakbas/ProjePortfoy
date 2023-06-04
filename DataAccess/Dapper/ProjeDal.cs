using Dapper;
using Entities.Concrete;
using Entities.DTOs;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IProjeDal : IBaseRepository<Proje>
    {
        Task<bool> IsExist(Proje entity);
        Task<int> UpdateStatu(ProjeStatus entity);
        Task<IEnumerable<object>> GetAllDto();
    }
    public class ProjeDal : IProjeDal
    {
        public async Task<int> Add(Proje entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Projes (ProjeAdi, BaslangicTarihi, BitisTarihi, ProjeAciklama, ProjeDurum, ProjeMusteri, ProjeButcesi,                    ProjeKategoriId, DepartmanId, Strateji, ProjeGeliri, ProjeGideri,KaynakYuzdesi,IsciSayisi) VALUES (@ProjeAdi, @BaslangicTarihi, @BitisTarihi, @ProjeAciklama, @ProjeDurum, @ProjeMusteri, @ProjeButcesi,@ProjeKategoriId, @DepartmanId, @Strateji, @ProjeGeliri, @ProjeGideri,@KaynakYuzdesi,@IsciSayisi);", new
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
                    ProjeGideri = entity.ProjeGideri,
                    KaynakYuzdesi = entity.KaynakYuzdesi,
                    IsciSayisi = entity.IsciSayisi
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

        public Task<IEnumerable<Proje>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetAllDto()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<object>(@"SELECT p.id                AS Id,
                                                                  p.ProjeAdi          AS Name,
                                                                  pk.ProjeKategoriAdi AS Type,
                                                                  p.BaslangicTarihi   AS StartDate,
                                                                  p.BitisTarihi       AS FinishDate,
                                                                  p.ProjeDurum        AS Status,
                                                                  p.ProjeButcesi      AS Budget,
                                                                  p.ProjeGeliri       AS Revenue,
                                                                  p.ProjeAciklama     AS Description,
                                                                  p.ProjeMusteri      AS Customer,
                                                                  p.DepartmanId       AS DepartmentId,
                                                                  p.IsciSayisi        AS manCount,
                                                                  p.KaynakYuzdesi     AS ResourcePercent
                                                          FROM Projes p
                                                                    INNER JOIN ProjeKategoris pk ON p.ProjeKategoriId = pk.Id");
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
                var result = await con.ExecuteAsync("UPDATE Projes SET ProjeAdi = @ProjeAdi, BaslangicTarihi=@BaslangicTarihi, BitisTarihi=@BitisTarihi, ProjeAciklama=@ProjeAciklama, ProjeDurum=@ProjeDurum, ProjeMusteri=@ProjeMusteri, ProjeButcesi=@ProjeButcesi,                    ProjeKategoriId=@ProjeKategoriId, DepartmanId=@DepartmanId, Strateji=@Strateji, ProjeGeliri=@ProjeGeliri, ProjeGideri=@ProjeGideri,KaynakYuzdesi,IsciSayisi WHERE ID =@Id", new
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

        public async Task<int> UpdateStatu(ProjeStatus entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE Projes SET ProjeDurum=@ProjeDurum WHERE ID =@Id", new
                {
                    Id = entity.Id,
                    ProjeDurum = entity.Status
                });
                return result;
            }
        }
    }
}
