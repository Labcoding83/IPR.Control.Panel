using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
{
    /// <summary>
    /// Memory information obtained from the SMBIOS table.
    /// </summary>
    public class MemoryDevice : InformationBase
    {
        internal MemoryDevice(byte[] data, IList<string> strings) : base(data, strings)
        {
            DeviceLocator = GetString(0x10).Trim();
            BankLocator = GetString(0x11).Trim();
            ManufacturerName = GetString(0x17).Trim();
            SerialNumber = GetString(0x18).Trim();
            PartNumber = GetString(0x1A).Trim();
            Speed = GetWord(0x15);
            Size = GetWord(0x0C);
            Type = (MemoryType)GetByte(0x12);

            if (GetWord(0x1C) > 0)
                Size += GetWord(0x1C);
        }

        /// <summary>
        /// Gets the string number of the string that identifies the physically labeled bank where the memory device is located.
        /// </summary>
        public string BankLocator { get; }

        /// <summary>
        /// Gets the string number of the string that identifies the physically-labeled socket or board position where the memory device is located.
        /// </summary>
        public string DeviceLocator { get; }

        /// <summary>
        /// Gets the string number for the manufacturer of this memory device.
        /// </summary>
        public string ManufacturerName { get; }

        /// <summary>
        /// Gets the string number for the part number of this memory device.
        /// </summary>
        public string PartNumber { get; }

        /// <summary>
        /// Gets the string number for the serial number of this memory device.
        /// </summary>
        public string SerialNumber { get; }

        /// <summary>
        /// Gets the size of the memory device. If the value is 0, no memory device is installed in the socket.
        /// </summary>
        public ushort Size { get; }

        /// <summary>
        /// Gets the value that identifies the maximum capable speed of the device, in mega transfers per second (MT/s).
        /// </summary>
        public ushort Speed { get; }

        /// <summary>
        /// Gets the type of this memory device.
        /// </summary>
        /// <value>The type.</value>
        public MemoryType Type { get; }
    }
}
