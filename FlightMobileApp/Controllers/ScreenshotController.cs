﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileApp.Controllers
{
    [ApiController]
    public class ScreenshotController : ControllerBase
    {

        public ScreenshotController()
        {

        }


        [HttpGet("screenshot")]
        public  async Task<IActionResult> GetScreenshot()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(100)
            };
            HttpResponseMessage response = await client.GetAsync("http://localhost:8080/screenshot");
            var image = await response.Content.ReadAsStreamAsync();
            return File(image, "image/jpg");
        }
    }
}
