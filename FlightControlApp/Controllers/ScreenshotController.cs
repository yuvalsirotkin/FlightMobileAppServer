using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FlightControlApp.Controllers
{
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        private readonly IConfiguration MyConfig;
        private int Port;
        private string Ip;
        public ScreenshotController(IConfiguration config)
        {
            MyConfig = config;
            this.Ip = MyConfig.GetValue<string>("Logging:SimulatorInfo:IP");
            this.Port = MyConfig.GetValue<int>("Logging:SimulatorInfo:HttpPort");
        }


        [HttpGet("screenshot")]
        public async Task<IActionResult> GetScreenshot()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(100)
            };
            try
            {
                HttpResponseMessage response = await client.GetAsync
                    ("http://" + this.Ip + ":" + this.Port.ToString() + "/screenshot");
                var image = await response.Content.ReadAsStreamAsync();
                if (image == null)
                {
                    return NotFound(0);
                }
                return File(image, "image/jpg");
            } catch 
            {
                return NotFound("not found screenShot");
            }


        }
    }
}