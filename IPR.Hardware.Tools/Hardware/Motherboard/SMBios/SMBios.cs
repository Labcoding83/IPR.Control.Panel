// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using IPR.Hardware.Tools.Hardware;
using IPR.Drivers.Interop;

namespace LibreHardwareMonitor.Hardware
{
    /// <summary>
    /// Reads and processes information encoded in an SMBIOS table.
    /// </summary>
    public class SMBios
    {
        private readonly byte[] _raw;
        private readonly Version _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="SMBios" /> class.
        /// </summary>
        public SMBios()
        {
            if (IPR.Hardware.Tools.Software.OperatingSystem.IsUnix)
            {
                _raw = null;

                string boardVendor = ReadSysFs("/sys/class/dmi/id/board_vendor");
                string boardName = ReadSysFs("/sys/class/dmi/id/board_name");
                string boardVersion = ReadSysFs("/sys/class/dmi/id/board_version");
                Board = new BaseBoardInformation(boardVendor, boardName, boardVersion, null);

                string systemVendor = ReadSysFs("/sys/class/dmi/id/sys_vendor");
                string productName = ReadSysFs("/sys/class/dmi/id/product_name");
                string productVersion = ReadSysFs("/sys/class/dmi/id/product_version");
                System = new SystemInformation(systemVendor, productName, productVersion, null, null);

                string biosVendor = ReadSysFs("/sys/class/dmi/id/bios_vendor");
                string biosVersion = ReadSysFs("/sys/class/dmi/id/bios_version");
                string biosDate = ReadSysFs("/sys/class/dmi/id/bios_date");
                Bios = new BiosInformation(biosVendor, biosVersion, biosDate);

                MemoryDevices = Array.Empty<MemoryDevice>();
            }
            else
            {
                List<MemoryDevice> memoryDeviceList = new();
                List<CacheInformation> processorCacheList = new();
                List<ProcessorInformation> processorInformationList = new();
                List<LaptopBattery> batteryList = new();
                List<Tuple<byte, InformationBase>> miscList = new();
                List<CallingInterface> callingInterfaceList = new();
                List<CoolingDevice> coolingDevices = new();
                List<TemperatureProbe> temperatureProbes = new();

                string[] tables = FirmwareTable.EnumerateTables(Kernel32.Provider.RSMB);
                if (tables is { Length: > 0 })
                {
                    _raw = FirmwareTable.GetTable(Kernel32.Provider.RSMB, tables[0]);
                    if (_raw == null || _raw.Length == 0)
                        return;

                    byte majorVersion = _raw[1];
                    byte minorVersion = _raw[2];

                    if (majorVersion > 0 || minorVersion > 0)
                        _version = new Version(majorVersion, minorVersion);

                    if (_raw is { Length: > 0 })
                    {
                        int offset = 8;
                        byte type = _raw[offset];

                        while (offset + 4 < _raw.Length && type != 127)
                        {
                            type = _raw[offset];
                            int length = _raw[offset + 1];

                            if (offset + length > _raw.Length)
                                break;

                            byte[] data = new byte[length];
                            Array.Copy(_raw, offset, data, 0, length);
                            offset += length;

                            List<string> strings = new();
                            if (offset < _raw.Length && _raw[offset] == 0)
                                offset++;

                            while (offset < _raw.Length && _raw[offset] != 0)
                            {
                                StringBuilder stringBuilder = new();

                                while (offset < _raw.Length && _raw[offset] != 0)
                                {
                                    stringBuilder.Append((char)_raw[offset]);
                                    offset++;
                                }

                                offset++;

                                strings.Add(stringBuilder.ToString());
                            }

                            offset++;
                            switch (type)
                            {
                                case 0x00:
                                    Bios = new BiosInformation(data, strings);
                                    break;
                                case 0x01:
                                    System = new SystemInformation(data, strings);
                                    break;
                                case 0x02:
                                    Board = new BaseBoardInformation(data, strings);
                                    break;
                                case 0x03:
                                    SystemEnclosure = new SystemEnclosure(data, strings);
                                    break;
                                case 0x04:
                                    processorInformationList.Add(new ProcessorInformation(data, strings));
                                    break;
                                case 0x07:
                                    processorCacheList.Add(new CacheInformation(data, strings));
                                    break;
                                case 0x0b:  //OEM Strings
                                    miscList.Add(new Tuple<byte, InformationBase>(type, new InformationBase(data, strings)));
                                    break;
                                case 0x11:
                                    memoryDeviceList.Add(new MemoryDevice(data, strings));
                                    break;
                                case 0x16:
                                case 0x7e:
                                    batteryList.Add(new LaptopBattery(data, strings));
                                    break;
                                case 0xb1:  //MiscellaneousBiosFlags
                                    BiosFlags = new BiosFlags(data, strings);
                                    break;
                                case 0xd0:  //RevisionsAndIds
                                    miscList.Add(new Tuple<byte, InformationBase>(type, new InformationBase(data, strings)));
                                    break;
                                case 0xda:  //CallingInterface
                                    callingInterfaceList.Add(new CallingInterface(data, strings));
                                    break;
                                case 0x1b:
                                    coolingDevices.Add(new CoolingDevice(data, strings));
                                    break;
                                case 0x1c: // Temperature probe
                                    temperatureProbes.Add(new TemperatureProbe(data, strings));
                                    break;
                                default:
                                    miscList.Add(new Tuple<byte, InformationBase>(type, new InformationBase(data, strings)));
                                    break;
                            }
                        }
                    }
                }

                MemoryDevices = memoryDeviceList.ToArray();
                ProcessorCaches = processorCacheList.ToArray();
                Processors = processorInformationList.ToArray();
                CallingInterfaces = callingInterfaceList.ToArray();
                CoolingDevices = coolingDevices.ToArray();
                TemperatureProbes = temperatureProbes.ToArray();
            }
        }

        /// <summary>
        /// Gets <inheritdoc cref="BiosInformation" />
        /// </summary>
        public BiosInformation Bios { get; }

        /// <summary>
        /// Gets <inheritdoc cref="BaseBoardInformation" />
        /// </summary>
        public BaseBoardInformation Board { get; }

        /// <summary>
        /// Gets <inheritdoc cref="MemoryDevice" />
        /// </summary>
        public MemoryDevice[] MemoryDevices { get; }

        /// <summary>
        /// Gets <inheritdoc cref="CacheInformation" />
        /// </summary>
        public CacheInformation[] ProcessorCaches { get; }

        /// <summary>
        /// Gets <inheritdoc cref="ProcessorInformation" />
        /// </summary>
        public ProcessorInformation[] Processors { get; }

        /// <summary>
        /// Gets <inheritdoc cref="SystemInformation" />
        /// </summary>
        public SystemInformation System { get; }

        /// <summary>

        /* Unmerged change from project 'LibreHardwareMonitorLib (net6.0)'
        Before:
                /// Gets <inheritdoc cref="LibreHardwareMonitor.Hardware.SystemEnclosure" />
        After:
                /// Gets <inheritdoc cref="LibreHardwareMonitor.Hardware.SMBios.SystemEnclosure" />
        */
        /// Gets <inheritdoc cref="SystemEnclosure" />
        /// </summary>
        public SystemEnclosure SystemEnclosure { get; }

        public BiosFlags BiosFlags { get; }

        public CallingInterface[] CallingInterfaces { get; }

        public TemperatureProbe[] TemperatureProbes { get; }

        public CoolingDevice[] CoolingDevices { get; }


        private static string ReadSysFs(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    using StreamReader reader = new(path);

                    return reader.ReadLine();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Report containing most of the information that could be read from the SMBIOS table.
        /// </summary>
        /// <returns>A formatted text string with computer information and the entire SMBIOS table.</returns>
        public string GetReport()
        {
            StringBuilder r = new();

            if (_version != null)
            {
                r.Append("SMBios Version: ");
                r.AppendLine(_version.ToString(2));
                r.AppendLine();
            }

            if (Bios != null)
            {
                r.Append("BIOS Vendor: ");
                r.AppendLine(Bios.Vendor);
                r.Append("BIOS Version: ");
                r.AppendLine(Bios.Version);
                if (Bios.Date != null)
                {
                    r.Append("BIOS Date: ");
                    r.AppendLine(Bios.Date.Value.ToShortDateString());
                }

                if (Bios.Size != null)
                {
                    const int megabyte = 1024 * 1024;
                    r.Append("BIOS Size: ");
                    if (Bios.Size > megabyte)
                        r.AppendLine(Bios.Size.Value / megabyte + " MB");
                    else
                        r.AppendLine(Bios.Size.Value / 1024 + " KB");
                }

                r.AppendLine();
            }

            if (System != null)
            {
                r.Append("System Manufacturer: ");
                r.AppendLine(System.ManufacturerName);
                r.Append("System Name: ");
                r.AppendLine(System.ProductName);
                r.Append("System Version: ");
                r.AppendLine(System.Version);
                r.Append("System Wakeup: ");
                r.AppendLine(System.WakeUp.ToString());
                r.AppendLine();
            }

            if (Board != null)
            {
                r.Append("Motherboard Manufacturer: ");
                r.AppendLine(Board.ManufacturerName);
                r.Append("Motherboard Name: ");
                r.AppendLine(Board.ProductName);
                r.Append("Motherboard Version: ");
                r.AppendLine(Board.Version);
                r.Append("Motherboard Serial: ");
                r.AppendLine(Board.SerialNumber);
                r.AppendLine();
            }

            if (SystemEnclosure != null)
            {
                r.Append("System Enclosure Type: ");
                r.AppendLine(SystemEnclosure.Type.ToString());
                r.Append("System Enclosure Manufacturer: ");
                r.AppendLine(SystemEnclosure.ManufacturerName);
                r.Append("System Enclosure Version: ");
                r.AppendLine(SystemEnclosure.Version);
                r.Append("System Enclosure Serial: ");
                r.AppendLine(SystemEnclosure.SerialNumber);
                r.Append("System Enclosure Asset Tag: ");
                r.AppendLine(SystemEnclosure.AssetTag);
                if (!string.IsNullOrEmpty(SystemEnclosure.SKU))
                {
                    r.Append("System Enclosure SKU: ");
                    r.AppendLine(SystemEnclosure.SKU);
                }

                r.Append("System Enclosure Boot Up State: ");
                r.AppendLine(SystemEnclosure.BootUpState.ToString());
                r.Append("System Enclosure Power Supply State: ");
                r.AppendLine(SystemEnclosure.PowerSupplyState.ToString());
                r.Append("System Enclosure Thermal State: ");
                r.AppendLine(SystemEnclosure.ThermalState.ToString());
                r.Append("System Enclosure Power Cords: ");
                r.AppendLine(SystemEnclosure.PowerCords.ToString());
                if (SystemEnclosure.RackHeight > 0)
                {
                    r.Append("System Enclosure Rack Height: ");
                    r.AppendLine(SystemEnclosure.RackHeight.ToString());
                }

                r.Append("System Enclosure Lock Detected: ");
                r.AppendLine(SystemEnclosure.LockDetected ? "Yes" : "No");
                r.Append("System Enclosure Security Status: ");
                r.AppendLine(SystemEnclosure.SecurityStatus.ToString());
                r.AppendLine();
            }

            if (Processors != null)
            {
                foreach (ProcessorInformation processor in Processors)
                {
                    r.Append("Processor Manufacturer: ");
                    r.AppendLine(processor.ManufacturerName);
                    r.Append("Processor Type: ");
                    r.AppendLine(processor.ProcessorType.ToString());
                    r.Append("Processor Version: ");
                    r.AppendLine(processor.Version);
                    r.Append("Processor Serial: ");
                    r.AppendLine(processor.Serial);
                    r.Append("Processor Socket Destignation: ");
                    r.AppendLine(processor.SocketDesignation);
                    r.Append("Processor Socket: ");
                    r.AppendLine(processor.Socket.ToString());
                    r.Append("Processor Version: ");
                    r.AppendLine(processor.Version);
                    r.Append("Processor Family: ");
                    r.AppendLine(processor.Family.ToString());
                    r.Append("Processor Core Count: ");
                    r.AppendLine(processor.CoreCount.ToString());
                    r.Append("Processor Core Enabled: ");
                    r.AppendLine(processor.CoreEnabled.ToString());
                    r.Append("Processor Thread Count: ");
                    r.AppendLine(processor.ThreadCount.ToString());
                    r.Append("Processor External Clock: ");
                    r.Append(processor.ExternalClock);
                    r.AppendLine(" Mhz");
                    r.Append("Processor Max Speed: ");
                    r.Append(processor.MaxSpeed);
                    r.AppendLine(" Mhz");
                    r.Append("Processor Current Speed: ");
                    r.Append(processor.CurrentSpeed);
                    r.AppendLine(" Mhz");
                    r.AppendLine();
                }
            }

            for (int i = 0; i < ProcessorCaches.Length; i++)
            {
                r.Append("Cache [" + ProcessorCaches[i].Designation + "] Size: ");
                r.AppendLine(ProcessorCaches[i].Size.ToString());
                r.Append("Cache [" + ProcessorCaches[i].Designation + "] Associativity: ");
                r.AppendLine(ProcessorCaches[i].Associativity.ToString().Replace("_", string.Empty));
                r.AppendLine();
            }

            for (int i = 0; i < MemoryDevices.Length; i++)
            {
                r.Append("Memory Device [" + i + "] Manufacturer: ");
                r.AppendLine(MemoryDevices[i].ManufacturerName);
                r.Append("Memory Device [" + i + "] Part Number: ");
                r.AppendLine(MemoryDevices[i].PartNumber);
                r.Append("Memory Device [" + i + "] Device Locator: ");
                r.AppendLine(MemoryDevices[i].DeviceLocator);
                r.Append("Memory Device [" + i + "] Bank Locator: ");
                r.AppendLine(MemoryDevices[i].BankLocator);
                r.Append("Memory Device [" + i + "] Speed: ");
                r.AppendLine(MemoryDevices[i].Speed.ToString());
                r.Append("Memory Device [" + i + "] Size: ");
                r.Append(MemoryDevices[i].Size.ToString());
                r.AppendLine(" MB");
                r.AppendLine();
            }

            if (_raw != null)
            {
                string base64 = Convert.ToBase64String(_raw);
                r.AppendLine("SMBios Table");
                r.AppendLine();

                for (int i = 0; i < Math.Ceiling(base64.Length / 64.0); i++)
                {
                    r.Append(" ");
                    for (int j = 0; j < 0x40; j++)
                    {
                        int index = i << 6 | j;
                        if (index < base64.Length)
                        {
                            r.Append(base64[index]);
                        }
                    }

                    r.AppendLine();
                }

                r.AppendLine();
            }

            return r.ToString();
        }
    }
}
