using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace WebApplication1
{
    public class Program
    {
        public static string[,] kisisel_veriler;
        public static string[,] cok_kisisel_veriler;
        public static string[,] normal_veriler;
        public static string[,] tum_veriler;
        public static string[] kisisel_baslik;
        public static string[] cok_kisisel_baslik;
        public static string[] normal_baslik;
        public static string[] tum_baslik;
        public static void Main(string[] args)
        {
            kisisel_veriler = Elastic.tabloYap(Elastic.sekmeler.kisisel);
            kisisel_baslik = Elastic.basliklar.ToArray();
            cok_kisisel_veriler = Elastic.tabloYap(Elastic.sekmeler.cok_kisisel);
            cok_kisisel_baslik = Elastic.basliklar.ToArray();
            normal_veriler = Elastic.tabloYap(Elastic.sekmeler.normal);
            normal_baslik = Elastic.basliklar.ToArray();
            tum_veriler = Elastic.tabloYap(Elastic.sekmeler.tum);
            tum_baslik = Elastic.basliklar.ToArray();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
