using IPR.Hardware.Tools.Hardware;
using System;
using System.Collections.Generic;

namespace IPR.Control.Panel.Services
{
    public interface IComputerService
    {
        IEnumerable<IHardware> Hardwares { get; }
        IEnumerable<IControl> Controls { get; }
        IEnumerable<ISensor> Sensors { get; }
        event EventHandler Connected;
        event EventHandler Disconnected;
        bool Open();
        void Monitor();
        void Close();
        string GetReport();
#if DEBUG
        void ThrowException();
#endif
    }
}
