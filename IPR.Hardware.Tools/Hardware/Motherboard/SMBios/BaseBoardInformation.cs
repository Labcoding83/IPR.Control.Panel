using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
{
    /// <summary>
    /// Motherboard information obtained from the SMBIOS table.
    /// </summary>
    public class BaseBoardInformation : InformationBase
    {
        internal BaseBoardInformation(string manufacturerName, string productName, string version, string serialNumber) : base(null, null)
        {
            ManufacturerName = manufacturerName;
            ProductName = productName;
            Version = version;
            SerialNumber = serialNumber;
        }

        internal BaseBoardInformation(byte[] data, IList<string> strings) : base(data, strings)
        {
            ManufacturerName = GetString(0x04).Trim();
            ProductName = GetString(0x05).Trim();
            Version = GetString(0x06).Trim();
            SerialNumber = GetString(0x07).Trim();
        }

        /// <summary>
        /// Gets the value that represents the manufacturer's name.
        /// </summary>
        public string ManufacturerName { get; }

        /// <summary>
        /// Gets the value that represents the motherboard's name.
        /// </summary>
        public string ProductName { get; }

        /// <summary>
        /// Gets the value that represents the motherboard's serial number.
        /// </summary>
        public string SerialNumber { get; }

        /// <summary>
        /// Gets the value that represents the motherboard's revision number.
        /// </summary>
        public string Version { get; }
    }
}
