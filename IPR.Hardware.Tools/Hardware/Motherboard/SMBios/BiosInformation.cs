using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
{
    /// <summary>
    /// Motherboard BIOS information obtained from the SMBIOS table.
    /// </summary>
    public class BiosInformation : InformationBase
    {
        internal BiosInformation(string vendor, string version, string date = null, ulong? size = null) : base(null, null)
        {
            Vendor = vendor;
            Version = version;
            Date = GetDate(date);
            Size = size;
        }

        internal BiosInformation(byte[] data, IList<string> strings) : base(data, strings)
        {
            Vendor = GetString(0x04);
            Version = GetString(0x05);
            Date = GetDate(GetString(0x08));
            Size = GetSize();
        }

        /// <summary>
        /// Gets the BIOS release date.
        /// </summary>
        public DateTime? Date { get; }

        /// <summary>
        /// Gets the size of the physical device containing the BIOS.
        /// </summary>
        public ulong? Size { get; }

        /// <summary>
        /// Gets the string number of the BIOS Vendor’s Name.
        /// </summary>
        public string Vendor { get; }

        /// <summary>
        /// Gets the string number of the BIOS Version. This value is a free-form string that may contain Core and OEM version information.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <returns><see cref="Nullable{Int64}" />.</returns>
        private ulong? GetSize()
        {
            int biosRomSize = GetByte(0x09);
            ushort extendedBiosRomSize = GetWord(0x18);

            bool isExtendedBiosRomSize = biosRomSize == 0xFF && extendedBiosRomSize != 0;
            if (!isExtendedBiosRomSize)
                return 65536 * (ulong)(biosRomSize + 1);

            int unit = (extendedBiosRomSize & 0xC000) >> 14;
            ulong extendedSize = (ulong)(extendedBiosRomSize & ~0xC000) * 1024 * 1024;

            switch (unit)
            {
                case 0x00: return extendedSize; // Megabytes
                case 0x01: return extendedSize * 1024; // Gigabytes - might overflow in the future
            }

            return null; // Other patterns not defined in DMI 3.2.0
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <param name="date">The bios date.</param>
        /// <returns><see cref="Nullable{DateTime}" />.</returns>
        private static DateTime? GetDate(string date)
        {
            string[] parts = (date ?? string.Empty).Split('/');

            if (parts.Length == 3 &&
                int.TryParse(parts[0], out int month) &&
                int.TryParse(parts[1], out int day) &&
                int.TryParse(parts[2], out int year))
            {
                if (month > 12)
                {
                    (month, day) = (day, month);
                }

                return new DateTime(year < 100 ? 1900 + year : year, month, day);
            }

            return null;
        }
    }
}
