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
    public class CoolingDevice : InformationBase
    {
        public ushort TemperatureProbeHandle { get; private set; }
        public CoolingDeviceType DeviceType { get; private set; }
        public CoolingDeviceStatus DeviceStatus { get; private set; }
        public byte CoolingUnitGroup { get; private set; }
        public uint OEMdefined { get; private set; }
        public ushort NominalSpeed { get; private set; }
        public string Description { get; private set; }

        internal CoolingDevice(byte[] data, IList<string> strings) : base(data, strings)
        {
            TemperatureProbeHandle = GetWord(0x04);
            CoolingUnitGroup = GetByte(0x07);
            OEMdefined = GetDword(0x08);
            NominalSpeed = GetWord(0x0c);
            Description = strings[0];
            SetDeviceTypeAndStatus();
        }

        private void SetDeviceTypeAndStatus()
        {
            var data = GetWord(0x06);
            var bits = Convert.ToString(data, 2).PadLeft(8, '0');
            var statusBits = bits.Substring(0, 3);
            var typeBits = bits.Substring(3);


            switch (statusBits)
            {
                case "001":
                    DeviceStatus = CoolingDeviceStatus.Other;
                    break;
                case "010":
                    DeviceStatus = CoolingDeviceStatus.Unkown;
                    break;
                case "011":
                    DeviceStatus = CoolingDeviceStatus.OK;
                    break;
                case "100":
                    DeviceStatus = CoolingDeviceStatus.NonCritical;
                    break;
                case "111":
                    DeviceStatus = CoolingDeviceStatus.Critical;
                    break;
                case "110":
                    DeviceStatus = CoolingDeviceStatus.NonRecoverable;
                    break;
            }

            switch(typeBits)
            {
                case "00001":
                    DeviceType = CoolingDeviceType.Other;
                    break;
                case "00010":
                    DeviceType = CoolingDeviceType.Unknown;
                    break;
                case "00011":
                    DeviceType = CoolingDeviceType.Fan;
                    break;
                case "00100":
                    DeviceType = CoolingDeviceType.CentrifugalBlower;
                    break;
                case "00101":
                    DeviceType = CoolingDeviceType.ChipFan;
                    break;
                case "00110":
                    DeviceType = CoolingDeviceType.CabinetFan;
                    break;
                case "00111":
                    DeviceType = CoolingDeviceType.PowerSupplyFan;
                    break;
                case "01000":
                    DeviceType = CoolingDeviceType.HeatPipe;
                    break;
                case "01001":
                    DeviceType = CoolingDeviceType.IntegratedRefrigeration;
                    break;
                case "10000":
                    DeviceType = CoolingDeviceType.ActiveCooling;
                    break;
                case "10001":
                    DeviceType = CoolingDeviceType.PassiveCooling;
                    break;
            }
        }

        public override string ToString()
        {
            return $"TemperatureProbeHandle: {TemperatureProbeHandle}, DeviceTypeandStatus: {DeviceType} - {DeviceStatus}, CoolingUnitGroup: {CoolingUnitGroup}," +
                $"OEMdefined: {OEMdefined}, NominalSpeed: {NominalSpeed}, Description: {Description}";
        }
    }

    public enum CoolingDeviceStatus : uint
    {
        Other = 1,
        Unkown,
        OK,
        NonCritical,
        Critical,
        NonRecoverable,
        None
    }

    public enum CoolingDeviceType
    {
        Other,
        Unknown,
        Fan,
        CentrifugalBlower,
        ChipFan,
        CabinetFan,
        PowerSupplyFan,
        HeatPipe,
        IntegratedRefrigeration,
        ActiveCooling,
        PassiveCooling,
        None
    }
}
