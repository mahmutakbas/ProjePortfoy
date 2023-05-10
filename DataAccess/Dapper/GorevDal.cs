using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IGorevDal : IBaseRepository<Gorev>
    {
    }
    public class GorevDal : IGorevDal
    {
        public async Task<int> Add(Gorev entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Gorevs ( ProjeId, BaslangicTarihi, BitisTarihi, TamamlanmaDurumu, Aciklama, GorevAdi) VALUES (@ProjeId, @BaslangicTarihi, @BitisTarihi, @TamamlanmaDurumu, @Aciklama, @GorevAdi);", new { ProjeId = entity.ProjeId, BaslangicTarihi = entity.BaslangicTarihi, BitisTarihi = entity.BitisTarihi, TamamlanmaDurumu = entity.TamamlanmaDurumu, Aciklama = entity.Aciklama, GorevAdi = entity.GorevAdi });

                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result =await con.ExecuteAsync("DELETE FROM Gorevs WHERE Id=@Id", new { Id = id });
                return result;
            }
        }

        public async Task<Gorev> Get(int id)
        {
            using (var con=new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result =await con.QueryFirstOrDefaultAsync<Gorev>("SELECT * FROM Gorevs WHERE Id=@Id", new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Gorev>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<Gorev>("SELECT * FROM Gorevs");
                return result;
            }
        }

        public async Task<int> Update(Gorev entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE Gorevs SET ProjeId=@ProjeId, BaslangicTarihi=@BaslangicTarihi, BitisTarihi=@BitisTarihi, TamamlanmaDurumu=@TamamlanmaDurumu, Aciklama=@Aciklama, GorevAdi=@GorevAdi WHERE ID = @Id ;", new { Id=entity.Id,ProjeId = entity.ProjeId, BaslangicTarihi = entity.BaslangicTarihi, BitisTarihi = entity.BitisTarihi, TamamlanmaDurumu = entity.TamamlanmaDurumu, Aciklama = entity.Aciklama, GorevAdi = entity.GorevAdi });
                return result;
            }
        }
    }
}
