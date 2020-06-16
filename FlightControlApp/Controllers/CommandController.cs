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
            return NotFound();
        }



        //// GET: api/FlightPlan
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<FlightPlan>>> GetFlightPlan(object value)
        //{
        //    List<FlightPlan> list = await _context.FlightPlan.ToListAsync();
        //    // insert all the location's data and segments's data
        //    foreach (FlightPlan flight in list)
        //    {
        //        string tempId = flight.id;
        //        List<Location> locationsList = await _context.Locations.ToListAsync();
        //        List<Segment> segmentsList = await _context.Segments.ToListAsync();
        //        //get the location and the segments according to the id
        //        Location thisLocation = locationsList.Where(a => a.id == tempId).First();
        //        List<Segment> thisSegments = segmentsList.Where(a => a.id == tempId).ToList();

        //        flight.Segments = thisSegments;
        //        flight.Initial_location = thisLocation;
        //    }
        //    return await _context.FlightPlan.ToListAsync();
        //}

    }
}