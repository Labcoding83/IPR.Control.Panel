// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IPR.Drivers.Interop;
using LibreHardwareMonitor.Hardware;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace IPR.Hardware.Tools.Hardware.Motherboard
{
    /// <summary>
    /// System enclosure security status based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.4.3</see>.
    /// </summary>
    public enum SystemEnclosureSecurityStatus
    {
        Other = 1,
        Unknown,
        None,
        ExternalInterfaceLockedOut,
        ExternalInterfaceEnabled
    }

    /// <summary>
    /// System enclosure state based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.4.2</see>.
    /// </summary>
    public enum SystemEnclosureState
    {
        Other = 1,
        Unknown,
        Safe,
        Warning,
        Critical,
        NonRecoverable
    }

    /// <summary>
    /// System enclosure type based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.4.1</see>.
    /// </summary>
    public enum SystemEnclosureType
    {
        Other = 1,
        Unknown,
        Desktop,
        LowProfileDesktop,
        PizzaBox,
        MiniTower,
        Tower,
        Portable,
        Laptop,
        Notebook,
        HandHeld,
        DockingStation,
        AllInOne,
        SubNotebook,
        SpaceSaving,
        LunchBox,
        MainServerChassis,
        ExpansionChassis,
        SubChassis,
        BusExpansionChassis,
        PeripheralChassis,
        RaidChassis,
        RackMountChassis,
        SealedCasePc,
        MultiSystemChassis,
        CompactPci,
        AdvancedTca,
        Blade,
        BladeEnclosure,
        Tablet,
        Convertible,
        Detachable,
        IoTGateway,
        EmbeddedPc,
        MiniPc,
        StickPc
    }

    /// <summary>
    /// Processor family based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.5.2</see>.
    /// </summary>
    public enum ProcessorFamily
    {
        Other = 1,
        Intel8086 = 3,
        Intel80286 = 4,
        Intel386,
        Intel486,
        Intel8087,
        Intel80287,
        Intel80387,
        Intel80487,
        IntelPentium,
        IntelPentiumPro,
        IntelPentiumII,
        IntelPentiumMMX,
        IntelCeleron,
        IntelPentiumIIXeon,
        IntelPentiumIII,
        M1,
        M2,
        IntelCeleronM,
        IntelPentium4HT,
        AmdDuron = 24,
        AmdK5,
        AmdK6,
        AmdK62,
        AmdK63,
        AmdAthlon,
        Amd2900,
        AmdK62Plus,
        PowerPc,
        PowerPc601,
        PowerPc603,
        PowerPc603Plus,
        PowerPc604,
        PowerPc620,
        PowerPcx704,
        PowerPc750,
        IntelCoreDuo,
        IntelCoreDuoMobile,
        IntelCoreSoloMobile,
        IntelAtom,
        IntelCoreM,
        IntelCoreM3,
        IntelCoreM5,
        IntelCoreM7,
        Alpha,
        Alpha21064,
        Alpha21066,
        Alpha21164,
        Alpha21164Pc,
        Alpha21164a,
        Alpha21264,
        Alpha21364,
        AmdTurionIIUltraDualCoreMobileM,
        AmdTurionDualCoreMobileM,
        AmdAthlonIIDualCoreM,
        AmdOpteron6100Series,
        AmdOpteron4100Series,
        AmdOpteron6200Series,
        AmdOpteron4200Series,
        AmdFxSeries,
        Mips,
        MipsR4000,
        MipsR4200,
        MipsR4400,
        MipsR4600,
        MipsR10000,
        AmdCSeries,
        AmdESeries,
        AmdASeries,
        AmdGSeries,
        AmdZSeries,
        AmdRSeries,
        AmdOpteron4300Series,
        AmdOpteron6300Series,
        AmdOpteron3300Series,
        AmdFireProSeries,
        Sparc,
        SuperSparc,
        MicroSparcII,
        MicroSparcIIep,
        UltraSparc,
        UltraSparcII,
        UltraSparcIIi,
        UltraSparcIII,
        UltraSparcIIIi,
        Motorola68040 = 96,
        Motorola68xxx,
        Motorola68000,
        Motorola68010,
        Motorola68020,
        Motorola68030,
        AmdAthlonX4QuadCore,
        AmdOpteronX1000Series,
        AmdOpteronX2000Series,
        AmdOpteronASeries,
        AmdOpteronX3000Series,
        AmdZen,
        Hobbit = 112,
        CrusoeTm5000 = 120,
        CrusoeTm3000,
        EfficeonTm8000,
        Weitek = 128,
        IntelItanium = 130,
        AmdAthlon64,
        AmdOpteron,
        AmdSempron,
        AmdTurio64Mobile,
        AmdOpteronDualCore,
        AmdAthlon64X2DualCore,
        AmdTurion64X2Mobile,
        AmdOpteronQuadCore,
        AmdOpteronThirdGen,
        AmdPhenomFXQuadCore,
        AmdPhenomX4QuadCore,
        AmdPhenomX2DualCore,
        AmdAthlonX2DualCore,
        PaRisc,
        PaRisc8500,
        PaRisc8000,
        PaRisc7300LC,
        PaRisc7200,
        PaRisc7100LC,
        PaRisc7100,
        V30 = 160,
        IntelXeon3200QuadCoreSeries,
        IntelXeon3000DualCoreSeries,
        IntelXeon5300QuadCoreSeries,
        IntelXeon5100DualCoreSeries,
        IntelXeon5000DualCoreSeries,
        IntelXeonLVDualCore,
        IntelXeonULVDualCore,
        IntelXeon7100Series,
        IntelXeon5400Series,
        IntelXeonQuadCore,
        IntelXeon5200DualCoreSeries,
        IntelXeon7200DualCoreSeries,
        IntelXeon7300QuadCoreSeries,
        IntelXeon7400QuadCoreSeries,
        IntelXeon7400MultiCoreSeries,
        IntelPentiumIIIXeon,
        IntelPentiumIIISpeedStep,
        IntelPentium4,
        IntelXeon,
        As400,
        IntelXeonMP,
        AmdAthlonXP,
        AmdAthlonMP,
        IntelItanium2,
        IntelPentiumM,
        IntelCeleronD,
        IntelPentiumD,
        IntelPentiumExtreme,
        IntelCoreSolo,
        IntelCore2Duo = 191,
        IntelCore2Solo,
        IntelCore2Extreme,
        IntelCore2Quad,
        IntelCore2ExtremeMobile,
        IntelCore2DuoMobile,
        IntelCore2SoloMobile,
        IntelCoreI7,
        IntelCeleronDualCore,
        Ibm390,
        PowerPcG4,
        PowerPcG5,
        Esa390G6,
        ZArchitecture,
        IntelCoreI5,
        IntelCoreI3,
        IntelCoreI9,
        ViaC7M = 210,
        ViaC7D,
        ViaC7,
        ViaEden,
        IntelXeonMultiCore,
        IntelXeon3xxxDualCoreSeries,
        IntelXeon3xxxQuadCoreSeries,
        ViaNano,
        IntelXeon5xxxDualCoreSeries,
        IntelXeon5xxxQuadCoreSeries,
        IntelXeon7xxxDualCoreSeries = 221,
        IntelXeon7xxxQuadCoreSeries,
        IntelXeon7xxxMultiCoreSeries,
        IntelXeon3400MultiCoreSeries,
        AmdOpteron3000Series = 228,
        AmdSempronII,
        AmdOpteronQuadCoreEmbedded,
        AmdPhenomTripleCore,
        AmdTurionUltraDualCoreMobile,
        AmdTurionDualCoreMobile,
        AmdTurionDualCore,
        AmdAthlonDualCore,
        AmdSempronSI,
        AmdPhenomII,
        AmdAthlonII,
        AmdOpteronSixCore,
        AmdSempronM,
        IntelI860 = 250,
        IntelI960,
        ArmV7 = 256,
        ArmV8,
        HitachiSh3,
        HitachiSh4,
        Arm,
        StrongArm,
        _686,
        MediaGX,
        MII,
        WinChip,
        Dsp,
        VideoProcessor
    }

    /// <summary>
    /// Processor characteristics based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.5.9</see>.
    /// </summary>
    [Flags]
    public enum ProcessorCharacteristics
    {
        None = 0,
        _64BitCapable = 1,
        MultiCore = 2,
        HardwareThread = 4,
        ExecuteProtection = 8,
        EnhancedVirtualization = 16,
        PowerPerformanceControl = 32,
        _128BitCapable = 64
    }

    /// <summary>
    /// Processor type based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.5.1</see>.
    /// </summary>
    public enum ProcessorType
    {
        Other = 1,
        Unknown,
        CentralProcessor,
        MathProcessor,
        DspProcessor,
        VideoProcessor
    }

    /// <summary>
    /// Processor socket based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.5.5</see>.
    /// </summary>
    public enum ProcessorSocket
    {
        Other = 1,
        Unknown,
        DaughterBoard,
        ZifSocket,
        PiggyBack,
        None,
        LifSocket,
        Zif423 = 13,
        A,
        Zif478,
        Zif754,
        Zif940,
        Zif939,
        MPga604,
        Lga771,
        Lga775,
        S1,
        AM2,
        F,
        Lga1366,
        G34,
        AM3,
        C32,
        Lga1156,
        Lga1567,
        Pga988A,
        Bga1288,
        RPga088B,
        Bga1023,
        Bga1224,
        Lga1155,
        Lga1356,
        Lga2011,
        FS1,
        FS2,
        FM1,
        FM2,
        Lga20113,
        Lga13563,
        Lga1150,
        Bga1168,
        Bga1234,
        Bga1364,
        AM4,
        Lga1151,
        Bga1356,
        Bga1440,
        Bga1515,
        Lga36471,
        SP3,
        SP3R2,
        Lga2066,
        Bga1510,
        Bga1528,
        Lga4189
    }

    /// <summary>
    /// System wake-up type based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.2.2</see>.
    /// </summary>
    public enum SystemWakeUp
    {
        Reserved,
        Other,
        Unknown,
        ApmTimer,
        ModemRing,
        LanRemote,
        PowerSwitch,
        PciPme,
        AcPowerRestored
    }

    /// <summary>
    /// Cache associativity based on <see href="https://www.dmtf.org/dsp/DSP0134">DMTF SMBIOS Reference Specification v.3.3.0, Chapter 7.8.5</see>.
    /// </summary>
    public enum CacheAssociativity
    {
        Other = 1,
        Unknown,
        DirectMapped,
        _2Way,
        _4Way,
        FullyAssociative,
        _8Way,
        _16Way,
        _12Way,
        _24Way,
        _32Way,
        _48Way,
        _64Way,
        _20Way,
    }

    /// <summary>
    /// Processor cache level.
    /// </summary>
    public enum CacheDesignation
    {
        Other,
        L1,
        L2,
        L3
    }

    /// <summary>
    /// Memory type.
    /// </summary>
    public enum MemoryType
    {
        Other = 0x01,
        Unknown = 0x02,
        DRAM = 0x03,
        EDRAM = 0x04,
        VRAM = 0x05,
        SRAM = 0x06,
        RAM = 0x07,
        ROM = 0x08,
        FLASH = 0x09,
        EEPROM = 0x0a,
        FEPROM = 0x0b,
        EPROM = 0x0c,
        CDRAM = 0x0d,
        _3DRAM = 0x0e,
        SDRAM = 0x0f,
        SGRAM = 0x10,
        RDRAM = 0x11,
        DDR = 0x12,
        DDR2 = 0x13,
        DDR2_FBDIMM = 0x14,
        DDR3 = 0x18,
        FBD2 = 0x19,
        DDR4 = 0x1a,
        LPDDR = 0x1b,
        LPDDR2 = 0x1c,
        LPDDR3 = 0x1d,
        LPDDR4 = 0x1e,
        LogicalNonVolatileDevice = 0x1f,
        HBM = 0x20,
        HBM2 = 0x21,
        DDR5 = 0x22,
        LPDDR5 = 0x23,
    }


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
            if (Software.OperatingSystem.IsUnix)
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
        /// Gets <inheritdoc cref="LibreHardwareMonitor.Hardware.Motherboard.SystemEnclosure" />
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
