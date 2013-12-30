using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Server.Tests
{
    [TestFixture]
    class ServerTests
    {
        private Socket clientSocket;
        private byte[] byteData = new byte[1024];
        private TcpServer server;
        [SetUp]
        public void SetUp()
        {
            
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //Server is listening on port 1000
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1000);
            
            //Connect to the server
            clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);

            server = new TcpServer();
            
        }
       
        private void OnConnect(IAsyncResult ar)
        {
          clientSocket.EndConnect(ar);

          
        }
        [Test]
        [Category("Unit")]
        public void ServerConnectedTest()
        {
            var manualLogonEvent = new ManualResetEvent(false);
            bool connected = false;
            server.Connected += delegate
            {
                connected = true;
                manualLogonEvent.Set();
            };

            server.start();
            manualLogonEvent.WaitOne(300000, false);
            
            Assert.AreEqual(true,connected);
           
           
        }
      
    }
}
