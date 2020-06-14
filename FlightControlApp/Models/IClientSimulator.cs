using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlApp.Models
{
    interface IClientSimulator
    {
        public bool Write(string command);
        public void Disconnect();
        public void Connect(string ip, int port);
    }
}
