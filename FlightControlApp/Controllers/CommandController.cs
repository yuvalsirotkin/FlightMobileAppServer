using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using FlightControlApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommandController : ControllerBase
    {

        private IModel SimModel;
        public CommandController(IModel Model)
        {
            this.SimModel = Model;
        }


        [HttpPost("command")]
        public async Task<ActionResult> PostCommandAsync(Command command)
        {
            if (!this.SimModel.isConnected)
            {
                return NotFound("Failed Connection1");
            }
            if (command.Throttle > 1 || command.Throttle < 0 ||
                command.Aileron > 1 || command.Aileron < -1 ||
                command.Elevator > 1 || command.Elevator < -1 ||
                command.Rudder > 1 || command.Rudder < -1)
            {
                Response.StatusCode = 422;
                return Content("Invalid data");
                //return BadRequest();
            }
            var myResult = await  this.SimModel.Execute(command);
            if(myResult == Result.Ok)
            {
                return Ok();
            }
            return NotFound("Failed Connection2");
        }
    }
}