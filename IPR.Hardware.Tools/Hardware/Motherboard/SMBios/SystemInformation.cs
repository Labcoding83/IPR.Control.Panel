using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
{
    /// <summary>
    /// System information obtained from the SMBIOS table.
    /// </summary>
    public class SystemInformation : InformationBase
    {
        internal SystemInformation
        (
            string manufacturerName,
            string productName,
            string version,
            string serialNumber,
            string family,
            SystemWakeUp wakeUp = SystemWakeUp.Unknown) : base(null, null)
        {
            ManufacturerName = manufacturerName;
            ProductName = productName;
            Version = version;
            SerialNumber = serialNumber;
            Family = family;
            WakeUp = wakeUp;
        }

        internal SystemInformation(byte[] data, IList<string> strings) : base(data, strings)
        {
            ManufacturerName = GetString(0x04);
            ProductName = GetString(0x05);
            Version = GetString(0x06);
            SerialNumber = GetString(0x07);
            Family = GetString(0x1A);
            WakeUp = (SystemWakeUp)GetByte(0x18);
        }

        /// <summary>
        /// Gets the family associated with system.
        /// <para>
        /// This text string identifies the family to which a particular computer belongs. A family refers to a set of computers that are similar but not identical from a hardware or software point of
        /// view. Typically, a family is composed of different computer models, which have different configurations and pricing points. Computers in the same family often have similar branding and cosmetic
        /// features.
        /// </para>
        /// </summary>
        public string Family { get; }

        /// <summary>
        /// Gets the manufacturer name associated with system.
        /// </summary>
        public string ManufacturerName { get; }

        /// <summary>
        /// Gets the product name associated with system.
        /// </summary>
        public string ProductName { get; }

        /// <summary>
        /// Gets the serial number string associated with system.
        /// </summary>
        public string SerialNumber { get; }

        /// <summary>
        /// Gets the version string associated with system.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets <inheritdoc cref="SystemWakeUp" />
        /// </summary>
        public SystemWakeUp WakeUp { get; }
    }
}
