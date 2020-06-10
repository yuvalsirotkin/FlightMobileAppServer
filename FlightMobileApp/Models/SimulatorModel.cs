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
        // ShouldStop for ShouldStop the thread in the staet method
        // tcpclient for the comunication with the server

        volatile Boolean ShouldStop;
        private TcpClient TCPClient;
        private NetworkStream TCPStream;
        private List<Byte[]> PostData = new List<Byte[]>();
        private readonly object bBalanceLock = new object();
        private readonly object bBalanceLockForImg = new object();
        private bool ShouldAdd;
        private bool GerImgReq;
        //member Error Log
        private string errlog = "";

        // event
        public event PropertyChangedEventHandler PropertyChanged;
        public SimulatorModel(TcpClient T)
        {
            this.ShouldStop = false;
            this.ShouldAdd = true;
            this.GerImgReq = false;
            this.TCPClient = T;
            // Sets the receive time out using the ReceiveTimeout public property.
            TCPClient.ReceiveTimeout = 10000;
            // Gets the receive time out using the ReceiveTimeout public property.
            if (TCPClient.ReceiveTimeout == 10000)
                Debug.WriteLine("The receive time out limit was successfully set " + TCPClient.ReceiveTimeout.ToString());
        }
        //property
        public string Errlog
        {
            get { return errlog; }
            set
            {
                errlog = value;
                NotifyPropertyChanged("Errlog");
            }
        }

        public void SendCommand(Command command)
        {
            SetAileron(command.Aileron);
            SetThrottle(command.Throttle);
            SetDirection(command.Rudder, command.Elevator);
        }

        public void getScreenshot()
        {
            lock (bBalanceLockForImg)
            {
                this.GerImgReq = true;
            }
        }
        
        // commands for set the value of aileron ,add the path to the send list
        public void SetAileron(double aileron)
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
            string msg = "set /controls/flight/aileron " + value_to_send.ToString() + "\n";

            Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
            lock (bBalanceLock)
            {
                if (this.ShouldAdd)
                {
                    this.PostData.Add(bytes);
                }
            }
        }

        // commands for set the value of throttle ,add the path to the send list
        public void SetThrottle(double throttle)
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

            string msg = "set /controls/engines/current-engine/throttle " + value_to_send.ToString() + "\n";
            Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
            lock (bBalanceLock)
            {
                if (this.ShouldAdd)
                {
                    this.PostData.Add(bytes);
                }

            }
        }

        // commands for set the value of x_rudder and  y_elevator,add the path to the send list
        public void SetDirection(double x_rudder, double y_elevator)
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

            string msg = "set /controls/flight/rudder " + value_to_send.ToString() + "\n";
            Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
            lock (bBalanceLock)
            {
                if (this.ShouldAdd)
                {
                    this.PostData.Add(bytes);

                }
            }

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
            msg = "set /controls/flight/elevator " + value_to_send.ToString() + "\n";
            Byte[] bytes2 = System.Text.Encoding.ASCII.GetBytes(msg);
            lock (bBalanceLock)
            {
                if (this.ShouldAdd)
                {
                    this.PostData.Add(bytes2);
                }
            }
        }


        //create connection with the server(simulator)
        // catch the exp if the server ip or port does not exsit 
        public void Connect(string ip, int port)
        {
           TCPClient.Connect(ip, port);
           this.Start(); //*****thread!!!

        }

        // ShouldStop the thread and log out
        public void Disconnect()
        {
            TCPClient.Close();
            this.ShouldStop = true;
        }
        public void Start()
        {

            Thread T = new Thread(delegate ()
            {
                this.TCPStream = TCPClient.GetStream();
                string init = "data\n";
                Byte[] init_view_data = System.Text.Encoding.ASCII.GetBytes(init);
                TCPStream.Write(init_view_data, 0, init_view_data.Length);
                while (!ShouldStop)
                {
                    /**
                     * *************************************
                     * lock section and send the get request for screen shot from the flight gear!!
                     * **************************************
                     */
                     lock (bBalanceLockForImg)
                    {
                        if(this.GerImgReq)
                        {

                        }
                    }
                     /**
                      * send Post command to flight gear
                      */
                    lock (bBalanceLock)
                    {
                        try
                        {
                            if (this.PostData.Count != 0)
                            {
                                int j = 0;
                                do
                                {

                                    TCPStream.Write(PostData[j], 0, PostData[j].Length);
                                    Int32 bytes32 = TCPStream.Read(PostData[j], 0, PostData[j].Length);
                                    PostData.Remove(PostData[j]);

                                } while (this.PostData.Count != 0);
                            }
                        }
                        // catch if passed 10 sec and the server did not return answer 'update the errLog 
                        catch (IOException e)
                        {
                            Debug.WriteLine(e);
                            if (e.ToString().Contains("connected party did not properly respond after a period of time"))
                            {
                                //Errlog = "Conection Timeout";
                            }
                            else
                            {
                                Errlog = "error";
                            }
                        }
                    }
                }
            });
            T.Start();
        }

        // this method activate the observable DP 
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
