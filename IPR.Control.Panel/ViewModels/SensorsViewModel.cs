using IPR.Control.Panel.Services;
using System.Collections.ObjectModel;
using System.Linq;
using IPR.Control.Panel.Models;
using Avalonia.Threading;
using Prism.Commands;
using IPR.Control.Panel.Common;

namespace IPR.Control.Panel.ViewModels
{
    public class SensorsViewModel : ViewModelBase
    {
        private readonly IContextService _contextService;
       
        private ObservableCollection<HardWare> _source;

        public ObservableCollection<HardWare> Source
        {
            get => _source;
            set
            {
                SetProperty(ref _source, value);
            }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                _contextService.Sensor = value is Models.Sensor ? value as Models.Sensor : null;
            }
        }

        public SensorsViewModel(
            IContextService contextService)
        {
            _contextService = contextService;
            Source = _contextService.Hardware;
        }

        public DelegateCommand CmdReset => new DelegateCommand(async () =>
        {
            var sensorNodes = _contextService.Hardware
                .SelectMany(x => x.SensorTypes)
                .SelectMany(x => x.Sensors);

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (var sensorNode in sensorNodes)
                    sensorNode.Max = 0;
            });
        });
    }
}
