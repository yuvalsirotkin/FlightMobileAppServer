using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightControlApp.Models
{
    public class ClinetSimulator : IClientSimulator
    {
        // tcpclient for the comunication with the server
        private TcpClient TCPClient;
        private NetworkStream TCPStream;
        public ClinetSimulator(TcpClient T)
        {
            this.TCPClient = T;
            // Sets the receive time out using the ReceiveTimeout public property.
            TCPClient.ReceiveTimeout = 10000;
            // Gets the receive time out using the ReceiveTimeout public property.
            if (TCPClient.ReceiveTimeout == 10000)
                Debug.WriteLine("The receive time out limit was successfully set " + TCPClient.ReceiveTimeout.ToString());
        }
        public bool Write(string command)
        {
            try
            {
                Byte[] getMsg = new byte[256];
                Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(command + "\r\n");
                TCPStream.Write(bytes, 0, bytes.Length);
                //Int32 bytes32 = TCPStream.Read(getMsg, 0, getMsg.Length);
                //String responseData = System.Text.Encoding.ASCII.GetString(getMsg, 0, bytes32);
                /*if(responseData == "ERR") // CHECK THE STRING THAT RETURN FORM THE FG.
                {
                    throw new Exception();
                }*/
                return true;  //*****check if sucsses to send the msg!!
            }
            catch (Exception)
            {
                return false;  //error in sendig msg
            }

        }

        public void Disconnect()
        {
            this.TCPClient.Close();
        }

        public void Connect(string ip, int port)
        {
            TCPClient.Connect(ip, port);
            TCPStream = TCPClient.GetStream();
            this.Write("data");
        }
    }
}
