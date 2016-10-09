using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWeb
{
    public class StatView
    {
        public int TestID { get; set; }

        public byte Rank { get; set; }

        private string _BestResult;
        public string BestResult {
            get { return _BestResult; }
            set { _BestResult = value == "0" ? "-" : value; }
        }

        public string TestName { get; set; }
    }
}
