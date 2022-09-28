using IPR.Hardware.Tools.Hardware;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IPR.Control.Panel.Services
{
    public class ComputerService : IComputerService
    {
        private readonly ILogger _logger;
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        private IComputer _computer;
        private bool _isOpened;

        public ComputerService(ILogger logger)
        {
            _logger = logger;
        }

        public bool Open()
        {
            if (_isOpened)
                return true;

            _computer = IComputer.Instance;
            _computer.IsCpuEnabled = true;
            _computer.IsGpuEnabled = true;
            _computer.IsBatteryEnabled = true;
            _computer.IsControllerEnabled = true;
            _computer.IsMemoryEnabled = true;
            _computer.IsMotherboardEnabled = true;
            _computer.IsNetworkEnabled = true;
            _computer.IsPsuEnabled = true;
            _computer.IsStorageEnabled = true;
            _isOpened = _computer.IsOpened;
            if (_isOpened)
                Connected?.Invoke(this, EventArgs.Empty);
            return _isOpened;
        }

        public void Monitor()
        {
            _computer.Monitor();
        }

        public void Close()
        {
            _computer.Close();
            _isOpened = false;
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public string GetReport()
        {
            return _computer.GetReport();
        }

        public IEnumerable<IHardware> Hardwares
        {
            get { return _computer.Hardware; }
        }
        public IEnumerable<IControl> Controls
        {
            get { return _computer.Hardware.SelectMany(x => x.Controls); }
        }
        public IEnumerable<ISensor> Sensors
        {
            get { return _computer.Hardware.SelectMany(x => x.Sensors); }
        }

#if DEBUG
        public void ThrowException()
        {
            try
            {
                _computer.ThrowError();
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }
#endif
    }
}
