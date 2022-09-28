using IPR.Control.Panel.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace IPR.Control.Panel.Services
{
    public interface ISensorService
    {
        event EventHandler SensorsUpdated;
        void Init(ObservableCollection<HardWare> hardwares);
        void UpdateContinuously(CancellationToken token, Action onStarted);
    }
}
