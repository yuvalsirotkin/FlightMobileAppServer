using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {

        private readonly SimulatorModel simModel;
        public ScreenshotController(SimulatorModel MysimModel)
        {
            simModel = MysimModel;
        }

        // GET: api/Screenshot   -----need to change it to without api
        //[HttpGet]
        /*public ActionResult<image> GetScreenshot(object value)
        {

            simModel.getScreenshot();

        }*/
    }
}
