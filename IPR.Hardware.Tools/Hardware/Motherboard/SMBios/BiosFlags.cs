using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
{
    public class BiosFlags : InformationBase
    {
        public bool RemoteBiosUpdateSupported { get; protected set; }

        public bool AcpiWmiSupported { get; protected set; }

        public bool LomCableDetectSupported { get; protected set; }

        public bool KeyboardIlluminationSupported { get; protected set; }

        public KeyboardIllumination KeyboardIlluminationMethod { get; protected set; }

        public bool SystemIsAllTerrainGrade { get; protected set; }

        public bool BssSupported { get; protected set; }

        public bool BiosSupportsAlternateLockingMechanism { get; protected set; }

        public bool BlackTopHardwarePresent { get; protected set; }

        public PciEnumerationMethod PciEnumerationMethod { get; protected set; }

        public BiosFlags(byte[] data, IList<string> strings) : base(data, strings)
        {
            SetPropertyValuesFromSmbiosBuffer(data);
        }

        private void SetPropertyValuesFromSmbiosBuffer(byte[] smbiosDataBuffer)
        {
            RemoteBiosUpdateSupported = (GetQword(0x04) & 1) > 0;
            AcpiWmiSupported = (GetQword(0x04) & 2) > 0;
            LomCableDetectSupported = (GetQword(0x04) & 4) > 0;
            KeyboardIlluminationSupported = (GetQword(0x04) & 8) > 0;
            if (KeyboardIlluminationSupported)
            {
                if ((GetQword(0x04) & 0x10) == 0)
                {
                    KeyboardIlluminationMethod = KeyboardIllumination.TaskLights;
                }
                else
                {
                    KeyboardIlluminationMethod = KeyboardIllumination.KeyboardBacklighting;
                }
            }
            else
            {
                KeyboardIlluminationMethod = KeyboardIllumination.Unsupported;
            }

            SystemIsAllTerrainGrade = (GetQword(0x04) & 0x20) > 0;
            BssSupported = (GetQword(0x04) & 0x40) > 0;
            BiosSupportsAlternateLockingMechanism = (GetQword(0x04) & 0x80) > 0;
            BlackTopHardwarePresent = (GetQword(0x04) & 0x100) > 0;
            PciEnumerationMethod = (PciEnumerationMethod)(GetQword(0x04) & 0x600u);
        }
    }
}
