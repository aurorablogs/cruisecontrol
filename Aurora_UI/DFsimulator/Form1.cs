using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DFsimulator
{
   
    public partial class Form1 : Form
    {
        List<string> Market=new List<string>();
        List<Order> Limit=new List<Order>();
        List<Order> Stoploss=new List<Order>(); 
        List<int> ApplBuy=new List<int>();
        List<int> ApplSell = new List<int>();
        List<int> GoogBuy = new List<int>();
        List<int> GoogSell = new List<int>();

        public Socket clientSocket;
        
        private byte[] byteData = new byte[1024];
        private int bid = 400;
        private int ask = 400;
        private int newbid = 0;
        private int newask = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                //Server is listening on port 1000
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1000);

                //Connect to the server
                clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SGSclient", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            byteData = new byte[1024];
            //Start listening to the data asynchronously
            clientSocket.BeginReceive(byteData,
                                       0,
                                       byteData.Length,
                                       SocketFlags.None,
                                       new AsyncCallback(OnReceive),
                                       null);

            timer1.Interval = int.Parse(textBox1.Text);
            timer1.Enabled = true;
            timer1.Stop();
            timer2.Interval = int.Parse(textBox1.Text);
            timer2.Enabled = true;
            timer2.Stop();
            timer3.Enabled = true;
            timer3.Interval = 30;
            button1.Enabled = false;
        }
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
                
                
//                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SGSclient", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);
                            
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SGSclient", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndReceive(ar);
                ASCIIEncoding enc = new ASCIIEncoding();
                string receivedData = enc.GetString(byteData);
                if (receivedData.Contains("UNAAPL"))
                    timer1.Stop();
                else if (receivedData.Contains("UNGOOG"))
                    timer2.Stop();
                else if (receivedData.Contains("SAAPL"))
                {
                    Action act = () => timer1.Start();
                    this.BeginInvoke(act);

                }
                else if (receivedData.Contains("SGOOG"))
                {
                    Action act = () => timer2.Start();
                    this.BeginInvoke(act);
                }
                
                else if (receivedData.Contains("Cancel Market Order"))
                    send("Your Order cancel request is rejected,1,1");
                else if(receivedData.Contains("Cancel"))
                {
                    char[] delimeter = new char[2]; //defining delimeter comas
                    delimeter[0] = ',';
                    delimeter[1] = ',';
                    string[] splited = receivedData.Split(delimeter, 3); //price feed comma sepaated
                    cancel(splited[1]);
                }
                else if(receivedData.Contains("Market Order"))
                {
                    Market.Add(receivedData);
                    send("Your order Request Received,1");

                }
                else if(receivedData.Contains("Update"))
                {
                    char[] delimeter = new char[2]; //defining delimeter comas
                    delimeter[0] = ',';
                    delimeter[1] = ',';
                    string[] splited = receivedData.Split(delimeter, 7); //price feed comma sepaated
                    Order or = new Order();
                    or.LimitPrice = int.Parse(splited[4]);
                    or.Quantity = int.Parse(splited[2]);
                    or.Type = splited[3];
                    or.Id = splited[5];
                    or.OrderType = splited[0];
                    or.Symbol = splited[1];
                    or.StopPrice = int.Parse(splited[4]);
                    Update(or);
                }
                else if(receivedData.Contains("Stop-Loss")||receivedData.Contains("Limit Order"))
                {
                    char[] delimeter = new char[2]; //defining delimeter comas
                    delimeter[0] = ',';
                    delimeter[1] = ',';
                    string[] splited = receivedData.Split(delimeter, 7); //price feed comma sepaated
                    if(splited[0]=="Limit Order")
                    {
                        Order or=new Order();
                        or.LimitPrice = int.Parse(splited[4]);
                        or.Quantity = int.Parse(splited[2]);
                        or.Type = splited[3];
                        or.Id = splited[5];
                        Limit.Add(or);

                        send("Your order Request Received,1");
                    }
                    if (splited[0] == "Stop-Loss Order")
                    {
                        Order or = new Order();
                        or.StopPrice = int.Parse(splited[4]);
                        or.Quantity = int.Parse(splited[2]);
                        or.Symbol = splited[1];
                        or.Id = splited[5];
                        Stoploss.Add(or);
                        send("Your order Request Received,1");

                    }

                    
                }
                
                clientSocket.BeginReceive(byteData,
                                          0,
                                          byteData.Length,
                                          SocketFlags.None,
                                          new AsyncCallback(OnReceive),
                                          null);

            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SGSclientTCP: " , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void timer1start()
        {
            timer1.Start();
        }
        private void cancel(string id)
        {
            bool a = false;
            for (int i = 0; i < Limit.Count; i++)
            {
                if (Limit[i].Id == id)
                {
                    Limit.RemoveAt(i);
                    a = true;
                    break;
                }
            }
            for (int i = 0; i < Stoploss.Count; i++)
            {
                if (Stoploss[i].Id == id)
                {
                    Stoploss.RemoveAt(i);
                    a = true;
                    break;
                }
            }
            if(a)
            {
                send("Cancel Succedded,"+id+",1");
            }
            else
            {
                send("Your Order cancel request is rejected," + id + ",1");
            }

        }

        public void send(string msg)
        {
            try
            {
                //lock (this)
                //{

                    byte[] bytes = Encoding.ASCII.GetBytes(msg);

                    //Send it to the server
                    clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
            //    }

            }
            catch (Exception)
            {
                timer1.Stop();
                timer2.Stop();
                MessageBox.Show("Unable to send message to the server.", "SGSclientTCP: ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            start("AAPL");
        }
        public void start(string msg)
        {
            lock (this)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                int rand = r.Next(0, 30);
                newbid = bid + rand;
                newask = ask + rand;
                if (msg == "AAPL")
                {
                    if (ApplBuy.Count < 10)
                    {
                        ApplBuy.Add(newask);
                        ApplSell.Add(newbid);
                    }
                    else
                    {
                        int min = ApplBuy.Min();
                        int min1 = ApplSell.Min();
                        for(int i=0;i<ApplBuy.Count;i++)
                        {
                            if (ApplBuy[i] == min)
                                ApplBuy[i] = newask;
                            if (ApplSell[i] == min1)
                                ApplSell[i] = newbid;
                            
                            
                        }

                    }

                }

                if (msg == "GOOG")
                {
                    if (GoogBuy.Count < 10)
                    {
                        GoogBuy.Add(newask);
                        GoogSell.Add(newbid);
                    }
                    else
                    {
                        int min = GoogBuy.Min();
                        int min1 = GoogSell.Min();
                        for (int i = 0; i < GoogBuy.Count; i++)
                        {
                            if (GoogBuy[i] == min)
                                GoogBuy[i] = newask;
                            if (GoogSell[i] == min1)
                                GoogSell[i] = newbid;

                        }

                    }

                }
                
            string stream = string.Format("(" + msg + ",{0},418.28,{1},5,1,-47.2)", newbid.ToString(), newask.ToString());

               send(stream);


            }
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            start("GOOG");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            button1.Enabled = true;
        }
        private void Update(Order order)
        {
            bool a = false;
            if(order.OrderType=="Limit Order")
            {
                for(int i=0;i<Limit.Count;i++)
                {
                    if(Limit[i].Id==order.Id)
                    {
                        Limit[i] = order;
                        a = true;
                        break;
                    }
                }

            }
            else if(order.OrderType=="Stop-Loss Order")
            {
                for (int i = 0; i < Stoploss.Count; i++)
                {
                    if (Stoploss[i].Id == order.Id)
                    {
                        Stoploss[i] = order;
                        a = true;
                        break;
                    }
                }
            }
            if(a)
            {
                send("Order updated,"+order.Id+",1");
            }
            else
            {
                send("Cannot update," + order.Id + ",1");
               
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to Exit?", "SGSclient: " ,
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes("Disconnect");

                //Send it to the server
                clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SGSclientTCP: " , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            char[] delimeter = new char[2]; //defining delimeter comas
            delimeter[0] = ',';
            delimeter[1] = ',';
            if (Market.Count > 0)
            {
                for (int i = 0; i < Market.Count; i++)
                {
                    string[] splitted = Market[i].Split(delimeter, 5);
                    string orderid = splitted[4];
                        
                    if (Market[i].Contains("SELL"))
                        send("Market Order," + newask + ",SELL,"+orderid+",1");
                    if (Market[i].Contains("BUY"))
                        send("Market Order," + newbid + ",BUY,"+orderid+",1");
                }
                Market.Clear();
            }
            if(Limit.Count>0)
            {
                for (int i = 0; i < Limit.Count; i++)
                {
                    Order or = Limit[i];
                    if (or.Type == "BUY")
                    {
                        for (int j = 0; j < ApplBuy.Count; j++)
                        {
                            if (or.LimitPrice == ApplBuy[j])
                            {
                                send("Your limit order of Quantity is Fullfilled by " + "AAPL,"+or.Id+",1");
                                Limit.RemoveAt(i);
                                break;
                            }

                        }
                        for (int j = 0; j < GoogBuy.Count; j++)
                        {
                            if (or.LimitPrice == GoogBuy[j])
                            {
                                send("Your limit order of Quantity is Fullfilled by " + "Goog," + or.Id + ",1");
                                Limit.RemoveAt(i);
                                break;
                            }

                        }


                    }
                    if (or.Type == "SELL")
                    {
                        for (int j = 0; j< ApplSell.Count; j++)
                        {
                            if (or.LimitPrice == ApplSell[j])
                            {
                                send("Your limit order of Quantity is Fullfilled by " + "AAPL," + or.Id + ",1");
                                Limit.RemoveAt(i);
                                break;
                            }

                        }
                        for (int j = 0; j < GoogSell.Count; j++)
                        {
                            if (or.LimitPrice == GoogSell[j])
                            {
                                send("Your limit order of Quantity is Fullfilled by " + "Goog," + or.Id + ",1");
                                Limit.RemoveAt(i);
                                break;
                            }

                        }


                    }

                }

            }
                
            
            if(Stoploss.Count>0)
            {
                for (int i = 0; i < Stoploss.Count; i++)
                {
                    bool a = false;
                    int price = 0;
                    Order or = Stoploss[i];
                    if (or.Symbol == "AAPL")
                    {
                        for (int j = 0; j < ApplSell.Count; j++)
                        {
                            if (or.StopPrice > ApplSell[j])
                            {
                                a = true;
                                price = ApplSell[j];
                                
//                             
                            }

                        }
                        if(a)
                        {
                            a = false;
                            send("Your Stop-Loss order of AAPL is fullfilled at " + price + "," + or.Id + ",1");
                            Stoploss.RemoveAt(i);
                        }
                    }
                    if (or.Symbol == "GOOG")
                    {
                        for (int j = 0; j < GoogSell.Count; j++)
                        {
                            if (or.StopPrice > GoogSell[j])
                            {
                                a = true;
                                price = GoogSell[j];
                                
                               // break;
                            }

                        }
                        if(a)
                        {
                            a = false;
                            send("Your Stop-Loss order of GOOG is fulfilled at " + price + "," + or.Id + ",1");
                            Stoploss.RemoveAt(i);
                        }


                    }

                }
            }
        }
    }
    
}
