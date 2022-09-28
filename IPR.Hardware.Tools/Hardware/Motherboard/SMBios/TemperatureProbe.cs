using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
{
    [DebuggerDisplay("{ToString()}")]
    public class TemperatureProbe : InformationBase
    {
        public string Description { get; private set; }
        public TemperatureProbeLocation Location { get; private set; }
        public TemperatureProbeStatus Status { get; private set; }
        public ushort MaximumValue { get; private set; }
        public ushort MinimumValue { get; private set; }
        public ushort Resolution { get; private set; }
        public ushort Tolerance { get; private set; }
        public ushort Accuracy { get; private set; }
        public uint OEMdefined { get; private set; }
        public ushort NominalValue { get; private set; }

        internal TemperatureProbe(byte[] data, IList<string> strings) : base(data, strings)
        {
            Description = strings[0];
            SetLocationAndStatus();
            MaximumValue = (ushort)(GetWord(0x06) / 10);
            MinimumValue = (ushort)(GetWord(0x08) / 10);
            Resolution = (ushort)(GetWord(0x0a) / 1000);
            Tolerance = (ushort)(GetWord(0x0c) / 10);
            Accuracy = (ushort)(GetWord(0x0e) / 100);
            OEMdefined = GetDword(0x10);
            NominalValue = (ushort)(GetWord(0x14) / 10);
        }

        private void SetLocationAndStatus()
        {
            var data = GetByte(0x05);
            var bits = Convert.ToString(data, 2).PadLeft(8, '0');
            var statusBits = bits.Substring(0, 3);
            var typeBits = bits.Substring(3);


            switch (statusBits)
            {
                case "001":
                    Status = TemperatureProbeStatus.Other;
                    break;
                case "010":
                    Status = TemperatureProbeStatus.Unkown;
                    break;
                case "011":
                    Status = TemperatureProbeStatus.OK;
                    break;
                case "100":
                    Status = TemperatureProbeStatus.NonCritical;
                    break;
                case "111":
                    Status = TemperatureProbeStatus.Critical;
                    break;
                case "110":
                    Status = TemperatureProbeStatus.NonRecoverable;
                    break;
            }

            switch (typeBits)
            {
                case "00001":
                    Location = TemperatureProbeLocation.Other;
                    break;
                case "00010":
                    Location = TemperatureProbeLocation.Unknown;
                    break;
                case "00011":
                    Location = TemperatureProbeLocation.Processor;
                    break;
                case "00100":
                    Location = TemperatureProbeLocation.Disk;
                    break;
                case "00101":
                    Location = TemperatureProbeLocation.PeripheralBay;
                    break;
                case "00110":
                    Location = TemperatureProbeLocation.SystemManagementModule;
                    break;
                case "00111":
                    Location = TemperatureProbeLocation.Motherboard;
                    break;
                case "01000":
                    Location = TemperatureProbeLocation.MemoryModule;
                    break;
                case "01001":
                    Location = TemperatureProbeLocation.ProcessorModule;
                    break;
                case "01010":
                    Location = TemperatureProbeLocation.PowerUnit;
                    break;
                case "01011":
                    Location = TemperatureProbeLocation.AddinCard;
                    break;
                case "01100":
                    Location = TemperatureProbeLocation.FrontPanelBoard;
                    break;
                case "01101":
                    Location = TemperatureProbeLocation.BackPanelBoard;
                    break;
                case "01111":
                    Location = TemperatureProbeLocation.DriveBackPlane;
                    break;
            }
        }

        public override string ToString()
        {
            return $"Description: {Description}, LocationandStatus: {Location} - {Status}, MaximumValue: {MaximumValue}," +
                $"MinimumValue: {MinimumValue}, Resolution: {Resolution}, Tolerance: {Tolerance}" +
                $"Accuracy: {Accuracy}, OEMdefined: {OEMdefined}, NominalValue: {NominalValue}";
        }
    }

    public enum TemperatureProbeLocation
    {
        Other = 1,
        Unknown,
        Processor,
        Disk,
        PeripheralBay,
        SystemManagementModule,
        Motherboard,
        MemoryModule,
        ProcessorModule,
        PowerUnit,
        AddinCard,
        FrontPanelBoard,
        BackPanelBoard,
        DriveBackPlane
    }

    public enum TemperatureProbeStatus
    {
        Other = 1,
        Unkown,
        OK,
        NonCritical,
        Critical,
        NonRecoverable
    }
}
