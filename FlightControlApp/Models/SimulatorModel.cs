using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightControlApp.Models
{
    public class SimulatorModel : IModel 
    {
        IClientSimulator client;
        private readonly BlockingCollection<AsyncCommand> _queue;
        public bool isConnected { get; }
        private bool shouldStop = false;

        // ShouldStop for ShouldStop the thread in the staet method

        public SimulatorModel(int port, string ip)
        {
            _queue = new BlockingCollection<AsyncCommand>();
            this.client = new ClinetSimulator(new TcpClient());
            try
            {
                Connect(ip, port);
                isConnected = true;
            }
            catch (Exception)
            {
                isConnected = false;
            }
        }

        // Called by the WebApi Controller, it will await on the returned Task<>
        // This is not an async method, since it does not await anything.
        public Task<Result> Execute(Command cmd)
        {
            var asyncCommand = new AsyncCommand(cmd);
            _queue.Add(asyncCommand);
            return asyncCommand.Task;
        }

        //create connection with the server(simulator)
        // catch the exp if the server ip or port does not exsit 
        public void Connect(string ip, int port)
        {
            this.client.Connect(ip, port);
            Start();
        }

        // ShouldStop the thread and log out
        public void Disconnect()
        {
            this.client.Disconnect();
            this.shouldStop = true;
        }

        public void Start()
        {
            Task.Factory.StartNew(ProcessCommands);
        }

        public void ProcessCommands()
        {
            while (! this.shouldStop)
            {
                foreach (AsyncCommand command in _queue.GetConsumingEnumerable())
                {

                    string ailron = "set /controls/flight/aileron " + command.Command.Aileron.ToString();
                    string thortle = "set /controls/engines/current-engine/throttle " + command.Command.Throttle.ToString();
                    string elvetor = "set /controls/flight/elevator " + command.Command.Elevator.ToString();
                    string rudder = "set /controls/flight/rudder " + command.Command.Rudder.ToString();
                    string ailronGet = "get /controls/flight/aileron";
                    string thortleGet = "get /controls/engines/current-engine/throttle";
                    string elvetorGet = "get /controls/flight/elevator";
                    string rudderGet = "get /controls/flight/rudder";
                    bool c1 = this.client.WriteSet(ailron);
                    bool c2 = this.client.WriteSet(thortle);
                    bool c3 = this.client.WriteSet(elvetor);
                    bool c4 = this.client.WriteSet(rudder);
                    bool c5 = this.client.WriteGet(ailronGet);
                    bool c6 = this.client.WriteGet(thortleGet);
                    bool c7 = this.client.WriteGet(elvetorGet);
                    bool c8 = this.client.WriteGet(rudderGet);
                    // recvBuffer to Result
                    // TaskCompletionSource allows an external thread to set
                    // the result (or the exceptino) on the associated task object
                    Result res;
                    if (!c1 || !c2 || !c3 || !c4 || !c5 || !c6 || !c7 || !c8)
                    {
                        res = Result.NotOk;
                    }
                    else
                    {
                        res = Result.Ok;
                    }
                    command.Completion.SetResult(res);
                }
            }
        }
    }
}
