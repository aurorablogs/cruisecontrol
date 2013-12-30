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
        [Test]
        [Category("Unit")]
        public void OrderTestCase()
        {
            var manualLogonEvent = new ManualResetEvent(false);
            Order order = new Order("1", "AAPL", "Market", "Buy", 100, 0, 0);

            Assert.AreEqual(order.ID, "1");


        }
        [Test]
        [Category("Unit")]
        public void PriceTestCase()
        {
            Price price=new Price("100","10","99","10","1","AAPL","99");
            Assert.AreEqual(price.Contract, "AAPL");


        }
      
    }
}
