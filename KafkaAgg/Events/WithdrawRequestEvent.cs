using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaAgg.Events
{
    public class WithdrawRequestEvent
    {
        public int WalletId { get; set; }
        public int Amount { get; set; }
    }
}
