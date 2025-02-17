using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyFantasy.PlayerClasses
{
    internal interface Player
    {
        public decimal GetAvgPoints { get; set; }
        public string GetPosition { get; set; }
        public string GetName { get; set; }
        public string GetID { get; set; }
    }
}
