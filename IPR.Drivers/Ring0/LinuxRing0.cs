using IPR.Drivers.Hardware;

namespace IPR.Drivers.Ring0
{
    internal class LinuxRing0 : IRing0
    {
        public bool IsOpen { get; private set; }

        public LinuxRing0()
        {
            Open();
        }

        public bool Open()
        {
            return true;
        }

        public bool ReadMsr(uint index, out uint eax, out uint edx)
        {
            eax = 0;
            edx = 0;
            return false;
        }

        public bool ReadMsr(uint index, out uint eax, out uint edx, GroupAffinity affinity)
        {
            eax = 0;
            edx = 0;
            return false;
        }

        public bool WriteMsr(uint index, uint eax, uint edx)
        {
            throw new NotImplementedException();
        }

        public byte ReadIoPort(uint port)
        {
            throw new NotImplementedException();
        }

        public void WriteIoPort(uint port, byte value)
        {
            throw new NotImplementedException();
        }

        public uint GetPciAddress(byte bus, byte device, byte function)
        {
            throw new NotImplementedException();
        }

        public bool ReadPciConfig(uint pciAddress, uint regAddress, out uint value)
        {
            throw new NotImplementedException();
        }

        public bool WritePciConfig(uint pciAddress, uint regAddress, uint value)
        {
            throw new NotImplementedException();
        }

        public bool WaitPciBusMutex(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public void ReleasePciBusMutex()
        {
            throw new NotImplementedException();
        }

        public bool WaitEcMutex(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public void ReleaseEcMutex()
        {
            throw new NotImplementedException();
        }

        public bool WaitIsaBusMutex(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public void ReleaseIsaBusMutex()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
