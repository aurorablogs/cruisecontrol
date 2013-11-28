using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Server
{
  
   public  class TcpServer : IDataSource
    {
        Socket serverSocket;
        private Socket _replySocket;
        byte[] byteData = new byte[1024];
        public event Action<Price> PriceArrived;
        public event Action<bool> Connected;
       public event Action<string> Notification;
       

       public TcpServer()
       {
        
       }
       public void start()
       {
           try
           {
               //We are using TCP sockets
               serverSocket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream,
                                         ProtocolType.Tcp);

               //Assign the any IP of the machine and listen on port number 1000
               IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 1000);

               //Bind and listen on the given address
               serverSocket.Bind(ipEndPoint);
               serverSocket.Listen(4);

               //Accept the incoming clients
               serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
               
           }
           catch (Exception ex)
           {
              TraceSourceLogger.Logger.Error(ex,"TcpServer","Start");
           }
       }

        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = serverSocket.EndAccept(ar);
                _replySocket = clientSocket;
                //Start listening for more clients
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
                if (Connected != null)
                    Connected(true);
                //Once the client connects then start receiving the commands from her
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, 
                    new AsyncCallback(OnReceive), clientSocket);                
            }
            catch (Exception ex)
            {
                TraceSourceLogger.Logger.Error(ex, "TcpServer","OnAccept");
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                clientSocket.EndReceive(ar);
                ASCIIEncoding enc = new ASCIIEncoding();
                string msgReceived= enc.GetString(byteData);
                if(msgReceived.Contains("Disconnect"))
                {
                    if (Connected != null)
                        Connected(false);
                }
                else if(msgReceived.Contains("Cancel Succedded"))
                {
                    if (Notification != null)
                        Notification(msgReceived);
                }
                else if(msgReceived.Contains("updated")||msgReceived.Contains("Cannot update")||msgReceived.Contains("Your Order cancel"))
                {
                    if (Notification != null)
                        Notification(msgReceived);
                }
                else if (msgReceived.Contains("Market Order") || msgReceived.Contains("limit") || msgReceived.Contains("Stop-Loss") || msgReceived.Contains("Your order Request Received"))
                {
                    if (Notification != null)
                        Notification(msgReceived);
                }
                else
                {
                    char[] delimeter = new char[2]; //defining delimeter comas
                    delimeter[0] = ',';
                    delimeter[1] = ',';
                    int start = msgReceived.IndexOf('(');
                    int end = msgReceived.IndexOf(')');
                    string substring = msgReceived.Substring(start + 1, end - start - 1);
                    string[] pricefeed = substring.Split(delimeter, 7); //price feed comma sepaated
                    Price newprice = new Price()
                                         {
                                             Contract = pricefeed[0],
                                             Bid = pricefeed[1],
                                             Last = pricefeed[2],
                                             Ask = pricefeed[3],
                                             Asksize = pricefeed[4],
                                             Bidsize = pricefeed[5],
                                             Change = pricefeed[6]
                                         };
                    if (PriceArrived != null)
                        PriceArrived(newprice);
                    TraceSourceLogger.Logger.Debug(substring, "tcpserver", "OnReceive");
                }
                //Start listening to the message send by the user
                    clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                                              new AsyncCallback(OnReceive), clientSocket);
                
            }
            catch (Exception ex)
            {
                TraceSourceLogger.Logger.Error(ex, "TcpServer","OnReceive");
            }
        }
       public void Send(string subcribe)
       {
           try
           {

               byte[] bytes = Encoding.ASCII.GetBytes(subcribe);
               _replySocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(OnSend), _replySocket);
               
           }
           catch (Exception exception)
           {
               TraceSourceLogger.Logger.Error(exception, "TcpServer", "OnSend");
           }    
       }

       public void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);                
            }
            catch (Exception ex)
            {
                TraceSourceLogger.Logger.Error(ex, "TcpServer","OnSend");
            }
        }

       
    }

    //The data structure by which the server and the client interact with 
    //each other
    
    
}
