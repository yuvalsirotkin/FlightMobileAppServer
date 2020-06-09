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
    [Route("api")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly SimulatorModel simModel;
        public CommandController()
        {
            int port = 5402;
            this.simModel = new SimulatorModel(new TcpClient());
            simModel.Connect("127.0.0.1", port);
        }

        //Error log property
        public string VM_Errlog
        {
            get
            {
                return simModel.Errlog;
            }
        }


        [HttpPost("command")]
        public ActionResult PostCommand(Command command)
        {
            // if the data is invalid - return error         ------------------------change to command invalid test
            if (command.Throttle > 1 || command.Throttle < 0 ||
                command.Aileron > 1 || command.Aileron < -1 ||
                command.Elevator > 1 || command.Elevator < -1 ||
                command.Rudder > 1 || command.Rudder < -1)
            {
                Response.StatusCode = 422;
                return Content("Invalid data");
                //return BadRequest();
            }

            simModel.SendCommand(command);
            if(this.VM_Errlog == "error")
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
