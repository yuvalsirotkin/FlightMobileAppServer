using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace FlightMobileApp.Models
{
    public class SimulatorModel
    {
        IClientSimulator client; 
        // ShouldStop for ShouldStop the thread in the staet method

        public SimulatorModel(ClinetSimulator T)
        {
            this.client = T;
        }

        public bool SendCommand(Command command)
        {
            if(SetAileron(command.Aileron) && SetThrottle(command.Throttle) && 
                SetRudder(command.Rudder) && SetElevator(command.Elevator))
            {
                return true;
            }
            return false;
        }

        public void getScreenshot()
        {

        }
        
        // commands for set the value of aileron and send to flightgear
        public bool SetAileron(double aileron)
        {
            double value_to_send;

            //checking the range value and change it according to aileron range
            if (aileron < 1)
            {
                if (aileron > -1)
                {
                    value_to_send = aileron;
                }
                else
                {
                    value_to_send = -1;
                }
            }
            else
            {
                value_to_send = 1;
            }
            string msg = "set /controls/flight/aileron " + value_to_send.ToString();
            return (this.client.Write(msg));
        }

        // commands for set the value of throttle and send to flightgear
        public bool SetThrottle(double throttle)
        {
            double value_to_send;

            //checking the range value and change it according to throttle range
            if (throttle < 1)
            {
                if (throttle > 0)
                {
                    value_to_send = throttle;
                }
                else
                {
                    value_to_send = 0;
                }
            }
            else
            {
                value_to_send = 1;
            }

            string msg = "set /controls/engines/current-engine/throttle " + value_to_send.ToString();
            return (this.client.Write(msg));
        }

        // commands for set the value of x_rudder and send to flightgear   
        public bool SetRudder(double x_rudder)
        {
            double value_to_send;
            //checking the range value and change it according to x_rudder range
            if (x_rudder < 1)
            {
                if (x_rudder > -1)
                {
                    value_to_send = x_rudder;
                }
                else
                {
                    value_to_send = -1;
                }
            }
            else
            {
                value_to_send = 1;
            }

            string msg = "set /controls/flight/rudder " + value_to_send.ToString();
            return (this.client.Write(msg));
        }

        // commands for set the value of y_elevator and send to flightgear
        public bool SetElevator(double y_elevator)
        {
            double value_to_send;
            //checking the range value and change it according to y_elevator range
            if (y_elevator < 1)
            {
                if (y_elevator > -1)
                {
                    value_to_send = y_elevator;
                }
                else
                {
                    value_to_send = -1;
                }
            }
            else
            {
                value_to_send = 1;
            }
            string msg = "set /controls/flight/elevator " + value_to_send.ToString();
            return (this.client.Write(msg));
        }


        //create connection with the server(simulator)
        // catch the exp if the server ip or port does not exsit 
        public void Connect(string ip, int port)
        {
           this.client.Connect(ip, port);
        } 

        // ShouldStop the thread and log out
        public void Disconnect()
        {
            this.client.Disconnect();
        }
    }
}
