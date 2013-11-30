using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFsimulator
{
    class Order
    {
        public Order()
        {

            Symbol = "";
            Quantity = 0;
            LimitPrice = 0;
            StopPrice = 0;
            Type = "";
            Id = "";
            OrderType = "";
        }
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public int LimitPrice { get; set; }
        public int StopPrice { get; set; }
        public int Quantity { get; set; }
        public string OrderType { get; set; }
    }
}
