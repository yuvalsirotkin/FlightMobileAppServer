using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlApp.Models
{
    public interface IClientSimulator
    {
        public bool WriteSet(string command);
        public bool WriteGet(string command);
        public void Disconnect();
        public void Connect(string ip, int port);
    }
}
