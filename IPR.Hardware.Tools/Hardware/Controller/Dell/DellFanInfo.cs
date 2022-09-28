using DellFanManagement.DellSmbiozBzhLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.Hardware.Tools.Hardware.Controller.Dell
{
    internal class DellFanInfo
    {
        public bool CanInit { get; set; }
        public int FanLevels { get; set; }
        public string ErrorMessage { get; set; }
        public bool CanControlFan { get; set; }
        public bool CanControlFanAlternate { get; set; }

        public List<Tuple<BzhFanIndex, BzhFanLevel, uint?>> FanDetails { get; set; } = new();


    }
}
