using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlApp.Models
{
    public interface IModel
    {
        public Task<Result> Execute(Command cmd);

        public bool isConnected { get; }

    }
}
