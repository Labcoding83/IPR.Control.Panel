using IPR.Drivers.Hardware;

namespace IPR.Drivers.Ring0
{
    internal class WinRing0 : IRing0
    {
        public bool IsOpen { get; private set; }

        public WinRing0()
        {
            Open();
        }

        public bool Open()
        {
            Hardware.Ring0Windows.Open();
            IsOpen = Hardware.Ring0Windows.IsOpen;
            return IsOpen;
        }

        public bool ReadMsr(uint index, out uint eax, out uint edx)
        {
            return Hardware.Ring0Windows.ReadMsr(index, out eax, out edx);
        }

        public bool ReadMsr(uint index, out uint eax, out uint edx, GroupAffinity affinity)
        {
            return Hardware.Ring0Windows.ReadMsr(index, out eax, out edx, affinity);
        }

        public bool WriteMsr(uint index, uint eax, uint edx)
        {
            return Hardware.Ring0Windows.WriteMsr(index, eax, edx);
        }

        public byte ReadIoPort(uint port)
        {
            return Hardware.Ring0Windows.ReadIoPort(port);
        }

        public void WriteIoPort(uint port, byte value)
        {
            Hardware.Ring0Windows.WriteIoPort(port, value);
        }

        public uint GetPciAddress(byte bus, byte device, byte function)
        {
            return Hardware.Ring0Windows.GetPciAddress(bus, device, function);
        }

        public bool ReadPciConfig(uint pciAddress, uint regAddress, out uint value)
        {
            return Hardware.Ring0Windows.ReadPciConfig(pciAddress, regAddress, out value); 
        }

        public bool WritePciConfig(uint pciAddress, uint regAddress, uint value)
        {
            return Hardware.Ring0Windows.WritePciConfig(pciAddress, regAddress, value);
        }

        public bool WaitPciBusMutex(int millisecondsTimeout)
        {
            return Hardware.Ring0Windows.WaitPciBusMutex(millisecondsTimeout);
        }

        public void ReleasePciBusMutex()
        {
            Hardware.Ring0Windows.ReleasePciBusMutex();
        }

        public bool WaitEcMutex(int millisecondsTimeout)
        {
            return Hardware.Ring0Windows.WaitEcMutex(millisecondsTimeout);
        }

        public void ReleaseEcMutex()
        {
            Hardware.Ring0Windows.ReleaseEcMutex();
        }

        public bool WaitIsaBusMutex(int millisecondsTimeout)
        {
            return Hardware.Ring0Windows.WaitIsaBusMutex(millisecondsTimeout);
        }

        public void ReleaseIsaBusMutex()
        {
            Hardware.Ring0Windows.ReleaseIsaBusMutex();
        }

        public void Close()
        {
            Hardware.Ring0Windows.Close();
        }
    }
}
