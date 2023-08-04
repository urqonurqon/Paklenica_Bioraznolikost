using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Novena.Utility.UDP
{
    public class ReceiveUdp : MonoBehaviour
    {
        // For reading new messages from Queue
        // without exposing Queue to other classes.

        private void Start()
        {
            StartListening(3201);
        }


        // Start the network listening thread.
        public void StartListening(int port)
        {
            Debug.Log("Start Listeneing port: " + port);
            var listenTask = new System.Threading.Tasks.Task(() => ListenTask(port), TaskCreationOptions.LongRunning);
            listenTask.Start();
        }

        private void ListenTask(int port)
        {

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                UdpClient udpClient = new UdpClient(port);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    Byte[] buffer = udpClient.Receive(ref remoteEndPoint);

                    string content = Encoding.ASCII.GetString(buffer);
                    UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread(content));
                }

            }
            catch (Exception e)
            {
                PrintError("Error opening UDP socket: " + e.ToString());
            }

        }

        public IEnumerator ThisWillBeExecutedOnTheMainThread(string content)
        {
            //Debug.Log("This is executed from the main thread");
            Debug.Log("Receve: " + content);
            yield return null;
        }

        private void PrintError(string e)
        {
            Debug.Log(e);
        }
        /*
    // For container to check for networking errors.
    public bool GetError(out string outError)
    {
        // Out promises initialization
        return Debug..TryDequeue(out outError);
    }*/
    }
}