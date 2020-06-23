using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
        public bool WriteSet(string command)
        {
            try
            {
                if (!this.TCPClient.Connected)
                {
                    throw new Exception();
                }
                Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(command + "\r\n");
                TCPStream.Write(bytes, 0, bytes.Length);
                return true;
            }
            catch (Exception)
            {
                return false;  //error in sendig msg
            }

        }
        public bool WriteGet(string command)
        {
            Byte[] getMsg = new byte[256];
            try
            {
                if(!this.TCPClient.Connected)
                {
                    throw new Exception();
                }
                Byte[] bytes = Encoding.ASCII.GetBytes(command + "\r\n");
                TCPStream.Write(bytes, 0, bytes.Length);
                TCPStream.Read(getMsg, 0, getMsg.Length);
                double value = Convert.ToDouble(Encoding.UTF8.GetString(getMsg));
                return true;
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
            Byte[] bytes = System.Text.Encoding.ASCII.GetBytes("data\n");
            TCPStream.Write(bytes, 0, bytes.Length);
        }
    }
}
