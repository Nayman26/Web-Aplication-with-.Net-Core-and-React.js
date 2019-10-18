using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VerilerController : Controller
    {
        [HttpGet]
        [ActionName("kisisel")]
        public string[,] Get01()
        {
            return Program.kisisel_veriler;
        }

        [HttpGet]
        [ActionName("cok_kisisel")]
        public string[,] Get02()
        {
            return Program.cok_kisisel_veriler;
        }

        [HttpGet]
        [ActionName("normal")]
        public string[,] Get03()
        {
            return Program.normal_veriler;
        }

        [HttpGet]
        [ActionName("tum")]
        public string[,] Get04()
        {
            return Program.tum_veriler;
        }

        [HttpGet]
        [ActionName("b1")]
        public string[] Get05()
        {
            return Program.kisisel_baslik;
        }

        [HttpGet]
        [ActionName("b2")]
        public string[] Get06()
        {
            return Program.cok_kisisel_baslik;
        }

        [HttpGet]
        [ActionName("b3")]
        public string[] Get07()
        {
            return Program.normal_baslik;
        }

        [HttpGet]
        [ActionName("b4")]
        public string[] Get08()
        {
            return Program.tum_baslik;
        }
    }
}
