using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Server;

namespace Aurora_UI.ViewModels
{
    public class OrderViewModel:DependencyObject
    {
        public OrderViewModel()
        {
            
        }
        public Order Order
        {
            set 
            { 
                this.ID = value.ID;
                this.Symbol = value.Symbol;
                this.Buysell = value.Buysell;
                this.Type = value.Type;
                this.Quantity = value.Quantity;
                this.LimitPrice = value.LimitPrice;
                this.StopPrice = value.StopPrice;
            }
            get{return new Order(this.ID,this.Symbol,this.Type,this.Buysell,this.Quantity,this.LimitPrice,this.StopPrice);}
        }

        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof (string), typeof (OrderViewModel), new PropertyMetadata(default(string)));

        public string ID
        {
            get { return (string) GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register("Symbol", typeof (string), typeof (OrderViewModel), new PropertyMetadata(default(string)));

        public string Symbol
        {
            get { return (string) GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        public static readonly DependencyProperty BuysellProperty =
            DependencyProperty.Register("Buysell", typeof (string), typeof (OrderViewModel), new PropertyMetadata(default(string)));

        public string Buysell
        {
            get { return (string) GetValue(BuysellProperty); }
            set { SetValue(BuysellProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof (string), typeof (OrderViewModel), new PropertyMetadata(default(string)));

        public string Type
        {
            get { return (string) GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register("Quantity", typeof (int), typeof (OrderViewModel), new PropertyMetadata(default(int)));

        public int Quantity
        {
            get { return (int) GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }

        public static readonly DependencyProperty LimitPriceProperty =
            DependencyProperty.Register("LimitPrice", typeof (int), typeof (OrderViewModel), new PropertyMetadata(default(int)));

        public int LimitPrice
        {
            get { return (int) GetValue(LimitPriceProperty); }
            set { SetValue(LimitPriceProperty, value); }
        }

        public static readonly DependencyProperty StopPriceProperty =
            DependencyProperty.Register("StopPrice", typeof (int), typeof (OrderViewModel), new PropertyMetadata(default(int)));

        public int StopPrice
        {
            get { return (int) GetValue(StopPriceProperty); }
            set { SetValue(StopPriceProperty, value); }
        }
    }
}
