using IPR.Control.Panel.Models;
using System;
using System.Collections.ObjectModel;

namespace IPR.Control.Panel.Services
{
    public interface IContextService
    {
        ObservableCollection<HardWare> Hardware { get; }

        void Init();
        Sensor? Sensor { get; set; }
        event EventHandler SelectedSensorChanged;
        void OnRegionVisibiltyChanged(string region, bool isVisible);
        event EventHandler<RegionVisibiltyChangedEventArgs> RegionVisibiltyChanged;
        Models.Control SelectedControl { get; set; }
    }
}
