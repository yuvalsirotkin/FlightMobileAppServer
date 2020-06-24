using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using FlightControlApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

            // check if the json invalid###
            bool pass = CheckTheCommand( command);
            if(!pass)
            {
                return NotFound("invalid JSON");
            }
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

        //check if threre is a element that miss in the JSON file we get from the user.
        private bool CheckTheCommand(Command command)
        {
            if(command.Throttle == -2 || command.Aileron == -2 
                || command.Elevator == -2 || command.Rudder == -2)
            {
                return false;
            }
            return true;
        }
    }
}