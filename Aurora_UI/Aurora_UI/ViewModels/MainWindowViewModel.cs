using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Aurora_UI.Commands;
using Aurora_UI.ViewModels;
using Aurora_UI.Views;
using Server;

namespace Aurora_UI
{
    public class MainWindowViewModel:DependencyObject
    {
        
        private TcpServer _tcpServer;
   
        public ObservableCollection<PriceViewModel> Newprice=new ObservableCollection<PriceViewModel>(); 
        public List<OrderViewModel> Orders=new List<OrderViewModel>();
        public ObservableCollection<string> Ordertypes=new ObservableCollection<string>(); 
        public ICommand Unsubscribe { get; set; }
        public ICommand Subscribe { get; set; }
        public ICommand SendOrder { get; set; }
        public ICommand UpdateOrder { get; set; }
        public ICommand CancelOrder { get; set; }
        public ICommand selectionchanged { get; set; }
        public ObservableCollection<string> Subscribers = new ObservableCollection<string>();
        public ObservableCollection<string> OrderTypes=new ObservableCollection<string>();
        public List<string> BuySellSelect=new List<string>(); 
        private Dispatcher _currentDispatcher;

        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public string Status
        {
            get { return (string)GetValue(ConnectionStatus); }
            set { SetValue(ConnectionStatus, value); }
        }
        //dependency properties section
        #region

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof (int), typeof (MainWindowViewModel), new PropertyMetadata(default(int)));

        public int SelectedIndex
        {
            
            get { return (int) GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register("Symbol", typeof (string), typeof (MainWindowViewModel), new PropertyMetadata("AAPL"));

        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register("Quantity", typeof (string), typeof (MainWindowViewModel), new PropertyMetadata("1"));

        public static readonly DependencyProperty LimitPriceProperty =
            DependencyProperty.Register("LimitPrice", typeof (string), typeof (MainWindowViewModel), new PropertyMetadata("0"));

        public static readonly DependencyProperty StopPriceProperty =
            DependencyProperty.Register("StopPrice", typeof (string), typeof (MainWindowViewModel), new PropertyMetadata("0"));

        public string StopPrice
        {
            get { return (string) GetValue(StopPriceProperty); }
            set { SetValue(StopPriceProperty, value); }
        }
        public string LimitPrice
        {
            get { return (string) GetValue(LimitPriceProperty); }
            set { SetValue(LimitPriceProperty, value); }
        }
        public string Quantity
        {
            get { return (string) GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }
        public string Symbol
        {
            get { return (string) GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }
        public static readonly DependencyProperty BuySellProperty =
            DependencyProperty.Register("BuySell", typeof (string), typeof (MainWindowViewModel), new PropertyMetadata(default(string)));

        public string BuySell
        {
            get { return (string) GetValue(BuySellProperty); }
            set { SetValue(BuySellProperty, value); }
        }

        public static readonly DependencyProperty SelectedOrderProperty =
            DependencyProperty.Register("SelectedOrder", typeof (string), typeof (MainWindowViewModel), new PropertyMetadata(default(string)));

        public string SelectedOrder
        {
            get { return (string) GetValue(SelectedOrderProperty); }
            set { SetValue(SelectedOrderProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(string), typeof(MainWindow), new UIPropertyMetadata(""));

        public static readonly DependencyProperty ConnectionStatus =
            DependencyProperty.Register("Status", typeof(string), typeof(MainWindowViewModel), new UIPropertyMetadata(""));

      
        #endregion


        public MainWindowViewModel()
        {
            
            _currentDispatcher = Dispatcher.CurrentDispatcher;
            Subscribe=new SubscribeCommand(this);
            Unsubscribe = new UnsubscribeCommand(this);
            SendOrder=new SendOrderCommand(this);
            UpdateOrder=new UpdateOrderCommand(this);
            CancelOrder=new CancelOrderCommand(this);
            selectionchanged=new SelectionChangedCommand(this);
            Subscribers.Add("AAPL");
            Subscribers.Add("GOOG");
            OrderTypes.Add("Market Order");
            OrderTypes.Add("Limit Order");
            OrderTypes.Add("Stop-Loss Order");
            SelectedItem = "AAPL";
            SelectedOrder = "Market Order";
            BuySellSelect.Add("BUY");
            BuySellSelect.Add("SELL");
            BuySell = "BUY";
            Status = "Status:Disconnected";
            _tcpServer = new TcpServer();
            _tcpServer.start();
            _tcpServer.PriceArrived += new Action<Price>(PriceArrived);
            _tcpServer.Connected += new Action<bool>(TcpServerConnected); 
            _tcpServer.Notification+=new Action<string>(TcpServeNotification);
          
        }
        public void SelectionChanged()
        {
            if (SelectedIndex >= 0)
            {

                SelectedOrder = Orders[SelectedIndex].Order.Type;
                LimitPrice = Orders[SelectedIndex].Order.LimitPrice.ToString();
                StopPrice = Orders[SelectedIndex].Order.StopPrice.ToString();
                BuySell = Orders[SelectedIndex].Order.Buysell;
                Quantity = Orders[SelectedIndex].Order.Quantity.ToString();
                Symbol = Orders[SelectedIndex].Order.Symbol;
            }

        }

        public void SendOrderExecute()
        {
            try
            {
                OrderViewModel neworder = new OrderViewModel();
                string id = DateTime.Now.ToString();
                string order = "";
                if (SelectedOrder == "Market Order")
                {
                    neworder.Order = new Order(id, Symbol, SelectedOrder, BuySell, int.Parse(Quantity), 0, 0);
                    order = string.Format("{0},{1},{2},{3},{4},", SelectedOrder, Symbol, Quantity, BuySell, id);
                }
                else if (SelectedOrder == "Limit Order")
                {
                    neworder.Order = new Order(id, Symbol, SelectedOrder, BuySell, int.Parse(Quantity),
                                               int.Parse(LimitPrice), 0);
                    order = string.Format("{0},{1},{2},{3},{4},{5},1", SelectedOrder, Symbol, Quantity, BuySell,
                                          LimitPrice, id);
                }
                else if (SelectedOrder == "Stop-Loss Order")
                {
                    neworder.Order = new Order(id, Symbol, SelectedOrder, BuySell, int.Parse(Quantity), 0,
                                               int.Parse(StopPrice));
                    order = string.Format("{0},{1},{2},{3},{4},{5},1", SelectedOrder, Symbol, Quantity, BuySell,
                                          StopPrice, id);
                }

                Orders.Add(neworder);
                _tcpServer.Send(order);
                Action action = () => Ordertypes.Add(SelectedOrder);
                _currentDispatcher.BeginInvoke(action);
            }
            catch(Exception exception)
            {
                TraceSourceLogger.Logger.Error(exception,"MainWinodowViewModel","SendOrderExecute");
                
            }
        }
        public void CancelOrderExecute()
        {
            try
            {
                if (SelectedIndex >= 0)
                {
                    string cancelreq = "";
                    if (SelectedOrder == "Market Order")
                        cancelreq = "Cancel Market Order";
                    else
                    {
                        cancelreq = "Cancel," + Orders[SelectedIndex].ID + ",";
                    }
                    _tcpServer.Send(cancelreq);
                }
                else
                {
                    Notification view = new Notification("First Select Order from List!!");
                    view.ShowDialog();

                }
            }
            catch (Exception exception)
            {
                TraceSourceLogger.Logger.Error(exception, "MainWinodowViewModel", "CancelOrderExecute");

            }
        }
        public void UpdateOrderExecute()
        {
            try
            {
                if (SelectedIndex >= 0)
                {
                    string order = "";
                    if (SelectedOrder == "Market Order")
                    {

                        order = string.Format("{0},{1},{2},{3},{4},{5},{6},", SelectedOrder, Symbol, Quantity, BuySell,
                                              0,
                                              Orders[SelectedIndex].ID, "Update");
                    }
                    else if (SelectedOrder == "Limit Order")
                    {

                        order = string.Format("{0},{1},{2},{3},{4},{5},{6},1", SelectedOrder, Symbol, Quantity, BuySell,
                                              LimitPrice, Orders[SelectedIndex].ID, "Update");
                    }
                    else if (SelectedOrder == "Stop-Loss Order")
                    {

                        order = string.Format("{0},{1},{2},{3},{4},{5},{6},1", SelectedOrder, Symbol, Quantity, BuySell,
                                              StopPrice, Orders[SelectedIndex].ID, "Update");
                    }
                    _tcpServer.Send(order);
                }
                else
                {
                    Notification view = new Notification("First Select Order from List!!");
                    view.ShowDialog();
                }
            }
            catch (Exception exception)
            {
                TraceSourceLogger.Logger.Error(exception, "MainWinodowViewModel", "UpdateOrderExecute");

            }
        }
        void PriceArrived(Price price)
        {
            PriceViewModel priceViewModel;
            bool itemexist = false;

            _currentDispatcher.BeginInvoke(DispatcherPriority.Normal,( Action)(()=>
                                {
                                    var model = (from viewModel1 in Newprice where viewModel1.Price.Contract.Equals(price.Contract) select viewModel1).FirstOrDefault();
                                    if(model!=null)
                                    {
                                        model.Price = price;
                                    }
                                    else
                                    {
                                        priceViewModel = new PriceViewModel();
                                        priceViewModel.Price = price;
                                        Newprice.Add(priceViewModel);
                                    }
                                
                                }));
       

        }
      public  void Subscribed()
        {
          _tcpServer.Send("S"+SelectedItem);

        }
      public void Unsubscribed()
      {
          _tcpServer.Send("UN"+SelectedItem);
          
          
      }
        private void TcpServeNotification(string result)
        {
            Action act = () =>
                             {
                                 char[] delimeter = new char[2]; //defining delimeter comas
                                 delimeter[0] = ',';
                                 delimeter[1] = ',';
                                 string orderid = "";
                                 string notify = "";
                                 if (result.Contains("Your limit order of Quantity is Fullfilled by")||result.Contains("Stop-Loss"))
                                 {
                                     string[] splited = result.Split(delimeter, 3); //price feed comma sepaated
                                     notify = splited[0]+",order id="+splited[1];
                                     orderid = splited[1];
                                 }
                                 else if(result.Contains("Cancel Succedded"))
                                 {
                                     string[] splited = result.Split(delimeter, 3); //price feed comma sepaated
                                     notify = splited[0]+" order id="+splited[1];
                                     orderid = splited[1];
                                 }
                                 else if(result.Contains("Your Order cancel"))
                                 {
                                     string[] splited = result.Split(delimeter, 3); //price feed comma sepaated
                                     notify = splited[0];

                                 }
                                 else if(result.Contains("updated")||result.Contains("Cannot update"))
                                 {
                                     string[] splited = result.Split(delimeter, 3); //price feed comma sepaated
                                     notify = splited[0] + ",order id=" + splited[1];
                                     
                                 }
                                 else if (result.Contains("Your order Request Received"))
                                 {
                                     string[] splited = result.Split(delimeter, 2);
                                     notify = splited[0];
                                 }
                                 else
                                 {
                                     string[] splited = result.Split(delimeter, 5); //price feed comma sepaated
                                     notify = "Your Market Order is fullfilled at " + splited[1]+" Order id="+splited[3];
                                     orderid = splited[3];
                                 }
                                 for(int i=0;i<Orders.Count;i++)
                                 {
                                    if(Orders[i].Order.ID==orderid)
                                    {
                                        Orders.RemoveAt(i);
                                        Ordertypes.RemoveAt(i);
                                        
                                    } 
                                 }
                                 Notification view = new Notification(notify);
                                 view.ShowDialog();
                             };
            _currentDispatcher.BeginInvoke(act);
        }

        private void TcpServerConnected(bool result)
        {
            if(result)
            {
               Action act = () => Status = "Status:Connected";
               _currentDispatcher.BeginInvoke(act);
            }
            else
            {
                Action act = () => Status = "Status:Disconnected";
                _currentDispatcher.BeginInvoke(act);
            }

        }

       


    }

    
}
