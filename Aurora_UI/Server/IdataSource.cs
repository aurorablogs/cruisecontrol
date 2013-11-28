using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public interface IDataSource
    {
        event Action<Price> PriceArrived;
        event Action<bool> Connected;
        event Action<string> Notification;
    }
}
