using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class Price
    {
        private string _contract;
        public string Contract
        {
            get { return _contract; }
            set { _contract = value; }
        }

        private string _bid;
        public string Bid
        {
            get { return _bid; }
            set { _bid = value; }
        }

        private string _last;
        public string Last
        {
            get { return _last; }
            set { _last = value; }
        }

        private string _ask;
        public string Ask
        {
            get { return _ask; }
            set { _ask = value; }
        }

        private string _asksize;
        public string Asksize
        {
            get { return _asksize; }
            set { _asksize = value; }
        }

        private string _bidsize;
        public string Bidsize
        {
            get { return _bidsize; }
            set { _bidsize = value; }
        }

        private string _change;
        public string Change
        {
            get { return _change; }
            set { _change = value; }
        }

        public Price()
        {

        }

        public Price(string ask, string asksize, string bid, string bidsize, string change, string contract, string last)
        {
            _ask = ask;
            _asksize = asksize;
            _bid = bid;
            _bidsize = bidsize;
            _change = change;
            _contract = contract;
            _last = last;
        }
    }
}
