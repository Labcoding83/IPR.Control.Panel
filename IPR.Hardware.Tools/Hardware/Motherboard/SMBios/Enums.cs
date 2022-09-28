using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
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

    public enum KeyboardIllumination
    {
        TaskLights,
        KeyboardBacklighting,
        Unsupported
    }

    public enum PciEnumerationMethod
    {
        Unknown = 0,
        BreadthFirst = 0x200,
        DepthFirst = 0x400,
        Reserved = 1536
    }
}
