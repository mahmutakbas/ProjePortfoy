using Dapper;
using Entities.Concrete;
using Entities.DTOs;
using MySql.Data.MySqlClient;

namespace DataAccess.Dapper
{
    public interface IDepartmentDal : IBaseRepository<Departman>
    {
        Task<bool> IsExist(Departman entity);
        Task<IEnumerable<DepartmanChartDto>> GetDepartmentChart();

    }
    public class DepartmanDal : IDepartmentDal
    {
        public async Task<int> Add(Departman entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("INSERT INTO Departmans (DepartmanAdi) VALUES (@DepartmanAdi);", new { DepartmanAdi = entity.DepartmanAdi });
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("DELETE FROM Departmans WHERE Id=@Id", new { Id = id });
                return result;
            }
        }

        public async Task<Departman> Get(int id)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Departman>("SELECT * FROM Departmans WHERE Id=@Id", new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Departman>> GetAll()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<Departman>("SELECT * FROM Departmans");
                return result;
            }
        }

        public async Task<IEnumerable<DepartmanChartDto>> GetDepartmentChart()
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryAsync<DepartmanChartDto>(@"sELECT d.DepartmanAdi AS DepartmantName,k.KaynakMiktari AS TotalResource,sum(PK.KaynakMiktari) AS TotalUseResource, k.KaynakAdi AS ResourceName
                                                                        FROM Departmans d
                                                                                    INNER JOIN Kaynaks k ON d.Id = k.DepartmanId
                                                                                    INNER JOIN ProjeKaynak PK ON k.Id = PK.KaynakId
                                                                        GROUP BY d.DepartmanAdi, k.KaynakMiktari,k.KaynakAdi;");
                return result;
            }
        }

        public async Task<bool> IsExist(Departman entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.QueryFirstOrDefaultAsync<Departman>("SELECT * FROM Departmans WHERE DepartmanAdi=@DepartmanAdi AND ID <> @Id", new { DepartmanAdi = entity.DepartmanAdi, Id = entity.Id });

                return result == null ? false : true;
            }
        }

        public async Task<int> Update(Departman entity)
        {
            using (var con = new MySqlConnection(PortfoyDbContex.ConnectionString))
            {
                var result = await con.ExecuteAsync("UPDATE Departmans SET DepartmanAdi = @DepartmanAdi WHERE Id=@Id;", new { DepartmanAdi = entity.DepartmanAdi, Id = entity.Id });
                return result;
            }
        }
    }
}
