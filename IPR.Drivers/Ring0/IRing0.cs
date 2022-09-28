using IPR.Drivers.Hardware;

namespace IPR.Drivers.Ring0
{
    public interface IRing0
    {
        bool IsOpen { get; }
        bool Open();
        bool ReadMsr(uint index, out uint eax, out uint edx);
        bool ReadMsr(uint index, out uint eax, out uint edx, GroupAffinity affinity);
        bool WriteMsr(uint index, uint eax, uint edx);
        byte ReadIoPort(uint port);
        void WriteIoPort(uint port, byte value);
        uint GetPciAddress(byte bus, byte device, byte function);
        bool ReadPciConfig(uint pciAddress, uint regAddress, out uint value);
        bool WritePciConfig(uint pciAddress, uint regAddress, uint value);
        bool WaitPciBusMutex(int millisecondsTimeout);
        void ReleasePciBusMutex();
        bool WaitEcMutex(int millisecondsTimeout);
        void ReleaseEcMutex();
        bool WaitIsaBusMutex(int millisecondsTimeout);
        void ReleaseIsaBusMutex();
        void Close();

    }
}
