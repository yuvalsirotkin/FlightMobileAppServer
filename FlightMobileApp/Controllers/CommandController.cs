using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly SimulatorModel simModel;
        public CommandController(SimulatorModel MysimModel)
        {
            simModel = MysimModel;
        }


        [HttpPost]
        public ActionResult PostCommand(Command command)
        {
            // if the data is invalid - return error         ------------------------change to command invalid test
            if (command.throttle > 1 || command.throttle < 0 ||
                command.Aileron > 1 || command.Aileron < -1 ||
                command.elevator > 1 || command.elevator < -1 ||
                command.rudder > 1 || command.rudder < -1)
            {
                Response.StatusCode = 422;
                return Content("Invalid data");
                //return BadRequest();
            }

            if (simModel.SendCommand(command) != 0)
            {
                Response.StatusCode = 422;  // change to the right status code
                return Content("Invalid data");
            }
            else
            {
                return Ok(); // check that
            }
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
