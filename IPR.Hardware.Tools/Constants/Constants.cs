using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.Hardware.Tools
{
    public static class Constants
    {
        public const string CPU_OFFSET = "mVOffset";
        public const string CPU_OFFSET_DESC = "Offset in mV";

        public const string CPU_LOAD = "Load";
        public const string CPU_LOAD_DESC = "Load %";

        public const string FAN = "FAN";
        public const string FAN_DESC = "Fan speed in RPM";
        public const string FAN_LEVEL = "FanLevel";
        public const string FAN_LEVEL_DESC = "Fan level: off, medium, high";

        public const string RANGE = "Range";
        public const string HIGH_RANGE = "High Range";
        public const string FULL_RANGE = "Full Range";

        public const string TEMP = "Temp";
        public const string POWER = "Power";
    }
}
