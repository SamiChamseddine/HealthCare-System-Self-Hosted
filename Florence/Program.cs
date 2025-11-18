using Florence.ApiHost;

namespace Florence
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var api = ApiHostBuilder.BuildApi(args);
            api.Run();
        }
    }
}
