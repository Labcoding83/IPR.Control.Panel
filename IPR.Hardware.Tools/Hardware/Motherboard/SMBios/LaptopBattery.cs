using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
{
    public class LaptopBattery : InformationBase
    {
        public const int MAXIMUM_BATTERY_COUNT = 6;

        public const int BASE_SMBIOS_BATTERY_HANDLE = 5632;

        public LaptopBattery(byte[] data, IList<string> strings) : base(data, strings)
        {
            Location = GetString(0x04).Trim();
            Manufacturer = GetString(0x05).Trim();
        }

        public string Location { get; protected set; }

        public string Manufacturer { get; protected set; }

        public string ManufactureDate { get; protected set; }

        public string SerialNumber { get; protected set; }

        public string DeviceName { get; protected set; }

        public string DeviceChemistry { get; protected set; }

        public int DesignCapacity { get; protected set; }

        public int DesignVoltage { get; protected set; }

        public string SbdsVersionNumber { get; protected set; }

        public int MaximumErrorInBatteryData { get; protected set; }

        public int SbdsSerialNumber { get; protected set; }

        public int SbdsManufactureDate { get; protected set; }

        public string SbdsDeviceChemistry { get; protected set; }

        public int DesignCapacityMultiplier { get; protected set; }

        public int OemSpecific1 { get; protected set; }

        public int OemSpecific2 { get; protected set; }

        public int OemSpecific3 { get; protected set; }

        public int BatteryNumber { get; protected set; }

        public string PiecePartId { get; protected set; }

       // public bool IsActive => base. == 22;
    }
}
