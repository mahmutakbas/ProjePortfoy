using Dapper;
using Entities.Concrete;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IKaynakDal : IBaseRepository<Kaynak>
    {
        Task<bool> IsExist(Kaynak entity);
    }
    public class KaynakDal : IKaynakDal
    {
        public async Task<int> Add(Kaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Kaynaks (KaynakAdi, KaynakMiktari,DepartmanId) VALUES (@KaynakAdi, @KaynakMiktari,@DepartmanId)", new { KaynakAdi = entity.KaynakAdi, KaynakMiktari = entity.KaynakMiktari, DepartmanId = entity.DepartmanId });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM Kaynaks WHERE ID = @Id"
                    , new { Id = id });
                return result;
            }
        }

        public async Task<Kaynak> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Kaynak>("SELECT k.Id AS Id,k.KaynakAdi AS KaynakAdi,k.KaynakMiktari AS KaynakMiktari,d.DepartmanAdi AS DepartmanAdi,k.DepartmanId AS DepartmanId FROM Kaynaks k INNER JOIN Departmans d ON k.DepartmanId=d.Id WHERE k.Id = @Id"
                    , new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Kaynak>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<Kaynak>("SELECT k.Id AS Id,k.KaynakAdi AS KaynakAdi,k.KaynakMiktari AS KaynakMiktari,d.DepartmanAdi AS DepartmanAdi,k.DepartmanId AS DepartmanId FROM Departmans d INNER JOIN Kaynaks k ON d.Id = k.DepartmanId");
                return result;
            }
        }

        public async Task<bool> IsExist(Kaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Kaynak>("SELECT * FROM Kaynaks WHERE KaynakAdi=@KaynakAdi AND ID <> @Id", new { KaynakAdi = entity.KaynakAdi, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<int> Update(Kaynak entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE Kaynaks SET KaynakAdi = @KaynakAdi, KaynakMiktari = @KaynakMiktari,DepartmanId=@DepartmanId WHERE Id = @Id", new { KaynakAdi = entity.KaynakAdi, KaynakMiktari = entity.KaynakMiktari, DepartmanId = entity.DepartmanId, Id = entity.Id });

                return result;
            }
        }
    }
}
