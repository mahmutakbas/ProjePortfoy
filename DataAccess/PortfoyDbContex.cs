namespace DataAccess
{
    public class PortfoyDbContex
    {
        public static readonly string ConnectionString;

       static  PortfoyDbContex()
        {
            ConnectionString = "Server=aws.connect.psdb.cloud;Database=testdb;user=pqriepaxppysivpx5jyf;password=pscale_pw_LxVsTSEc3JmUjw0NR2Hulib0gYNH6fhT8pQlLiXuDO4;SslMode=VerifyFull;";
        }
    }
}
