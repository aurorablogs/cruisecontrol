using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Server;

namespace Aurora_UI.ViewModels
{
    public class PriceViewModel:DependencyObject
    {
        public PriceViewModel()
        {

        }
        public Price Price
        {
            set
            {
                this.Ask = value.Ask;
                this.Asksize = value.Asksize;
                this.Bid = value.Bid;
                this.Bidsize = value.Bidsize;
                this.Change = value.Change;
                this.Contract = value.Contract;
                this.Last = value.Last;

            }
            get { return new Price(this.Ask, this.Asksize, this.Bid, this.Bidsize, this.Change, this.Contract, this.Last); }

        }

        public static readonly DependencyProperty ContractProperty =
            DependencyProperty.Register("Contract", typeof (string), typeof (PriceViewModel), new PropertyMetadata(default(string)));

        public string Contract
        {
            get { return (string) GetValue(ContractProperty); }
            set { SetValue(ContractProperty, value); }
        }

        public static readonly DependencyProperty AskProperty =
            DependencyProperty.Register("Ask", typeof (string), typeof (PriceViewModel), new PropertyMetadata(default(string)));

        public string Ask
        {
            get { return (string) GetValue(AskProperty); }
            set { SetValue(AskProperty, value); }
        }

        public static readonly DependencyProperty BidProperty =
            DependencyProperty.Register("Bid", typeof (string), typeof (PriceViewModel), new PropertyMetadata(default(string)));

        public string Bid
        {
            get { return (string) GetValue(BidProperty); }
            set { SetValue(BidProperty, value); }
        }

        public static readonly DependencyProperty LastProperty =
            DependencyProperty.Register("Last", typeof (string), typeof (PriceViewModel), new PropertyMetadata(default(string)));

        public string Last
        {
            get { return (string) GetValue(LastProperty); }
            set { SetValue(LastProperty, value); }
        }

        public static readonly DependencyProperty AsksizeProperty =
            DependencyProperty.Register("Asksize", typeof (string), typeof (PriceViewModel), new PropertyMetadata(default(string)));

        public string Asksize
        {
            get { return (string) GetValue(AsksizeProperty); }
            set { SetValue(AsksizeProperty, value); }
        }

        public static readonly DependencyProperty BidsizeProperty =
            DependencyProperty.Register("Bidsize", typeof (string), typeof (PriceViewModel), new PropertyMetadata(default(string)));

        public string Bidsize
        {
            get { return (string) GetValue(BidsizeProperty); }
            set { SetValue(BidsizeProperty, value); }
        }

        public static readonly DependencyProperty ChangeProperty =
            DependencyProperty.Register("Change", typeof (string), typeof (PriceViewModel), new PropertyMetadata(default(string)));

        public string Change
        {
            get { return (string) GetValue(ChangeProperty); }
            set { SetValue(ChangeProperty, value); }
        }
        

    }
}
