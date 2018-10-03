using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace Assignment4Part2.Messages
{
    class ViewModelMessage : MessageBase
    {
        public int MId { get; set; }
        public String MName { get; set; }
        public String Text { get; set; }
        public Customer cust { get; set; }
        public String SName { get; set; }
    }
}
