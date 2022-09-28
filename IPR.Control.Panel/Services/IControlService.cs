using IPR.Control.Panel.Models;
using System.Collections.ObjectModel;
using System.Threading;

namespace IPR.Control.Panel.Services
{
    public interface IControlService
    {
        void Init(ObservableCollection<HardWare> hardwares);
        void UpdateContinuously(CancellationToken token);
    }
}
