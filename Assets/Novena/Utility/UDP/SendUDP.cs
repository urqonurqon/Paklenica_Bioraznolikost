using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Novena.Utility.UDP
{
    public class SendUDP : MonoBehaviour
    {
        [SerializeField] string computerIp = "192.168.1.19";
        public int port = 3201;
    
        // For returning error messages to main Unity thread
        private ConcurrentQueue<string> errorQueue = new ConcurrentQueue<string>();

        public void OnSendButton()
        {
            Send(port, "Poslano");

        }

        public void Send(int port, string message)
        {
            var sendTask = new System.Threading.Tasks.Task(() => SendTask(port, message));
            sendTask.Start();
        }


        public void SendTask(int port, string message)
        {
            using (UdpClient client = new UdpClient())
            {
                try
                {
                    // String address to IPAddress
                    Debug.Log("Send Message port: " + port + ": message: " + message);
                    IPAddress ip;
                    IPAddress.TryParse(computerIp, out ip);
                    // Convert message to byte array
                    byte[] buffer = Encoding.ASCII.GetBytes(message);
                    Debug.Log("buffer.Length" + buffer.Length);
                    // Creatte IPEndPoint from the given address and port
                    IPEndPoint endPoint = new IPEndPoint(ip, port);
                    // Send the contents of buffer
                    client.Send(buffer, buffer.Length, endPoint);
                }
                catch (SocketException se)
                {
                    PrintError("SocketException sending to " + computerIp + ":" + port + ": " + se.ToString());
                    Debug.Log("SocketException sending to " + computerIp + ":" + port + ": " + se.ToString());
                }
                catch (Exception e)
                {
                    PrintError("Unexpected exception sending to " + computerIp + ":" + port + ": " + e.ToString());
                    Debug.Log("SocketException sending to " + computerIp + ":" + port + ": " + e.ToString());
                }
            }
        }

        private void PrintError(string e)
        {
            errorQueue.Enqueue(e);
        }

        // For container to check for networking errors.
        public bool GetError(out string outError)
        {
            // Out promises initialization
            return errorQueue.TryDequeue(out outError);
        }
    }
}
