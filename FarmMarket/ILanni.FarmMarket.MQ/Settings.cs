using System;
using System.Collections.Generic;
using System.Text;

namespace ILanni.FarmMarket.MQ
{
    public class Settings
    {
        public string ExchangeForProduct { get; set; } = "farmmarket.dw.product";
        public string RoutingKeyForProduct { get; set; } = "farmmarket.dw.product";

        public string QueueForProduct { get; set; } = "farmmarket.dw.product";

    }
}
