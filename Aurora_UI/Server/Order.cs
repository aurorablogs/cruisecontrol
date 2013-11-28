using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Order
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private string _buysell;
        public string Buysell
        {
            get { return _buysell; }
            set { _buysell = value; }
        }
        private int _limitprice;
        public int LimitPrice
        {
            get { return _limitprice; }
            set { _limitprice = value; }
        }
        private int _stopprice;
        public int StopPrice
        {
            get { return _stopprice; }
            set { _stopprice = value; }
        }

        public Order()
        {
            _id = "";
            _symbol = "";
            _buysell = "SELL";
            Quantity = 0;
            Type = "";
            LimitPrice = 0;
            StopPrice = 0;
        }
        public Order(string id,string symbol,string type,string buysell,int quantity,int limitprice,int stopprice)
        {
            _id = id;
            _symbol = symbol;
            _type = type;
            _buysell = buysell;
            _quantity = quantity;
            _limitprice = limitprice;
            _stopprice = stopprice;
        }
    }
}
