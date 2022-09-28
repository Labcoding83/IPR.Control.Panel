using IPR.Control.Panel.Common;
using IPR.Control.Panel.Models;
using IPR.Control.Panel.Services;
using ScottPlot.Avalonia;
using System;
using System.Linq;

namespace IPR.Control.Panel.ViewModels
{
    public class SensorHistoryViewModel : ViewModelBase
    {
        private readonly IComputerService _computerService;
        private readonly IContextService _contextService;
        private AvaPlot _avaPlot;

        private Sensor? _sensor;
        public Sensor? Sensor
        {
            get => _sensor;
            set
            {
                SetProperty(ref _sensor, value);
            }
        }

        public SensorHistoryViewModel(
            IComputerService computerService,
            IContextService contextService)
        {
            _computerService = computerService;
            _contextService = contextService;

            _contextService.SelectedSensorChanged += _contextService_SelectedSensorChanged;
        }

        public void SetPlot(AvaPlot plot)
        {
            _avaPlot = plot;
        }

        private void _contextService_SelectedSensorChanged(object? sender, EventArgs e)
        {
            if (Sensor != null) 
                Sensor.History.CollectionChanged -= History_CollectionChanged;

            Sensor = _contextService.Sensor;
            _avaPlot.Plot.XAxis.Ticks(false);
            _avaPlot.Plot.XAxis.Grid(false);
            _avaPlot.Configuration.LeftClickDragPan = false;
            _avaPlot.Configuration.RightClickDragZoom = false;
            _avaPlot.Configuration.DoubleClickBenchmark = false;
            _avaPlot.Plot.Clear();            

            RefreshPlot();
            
            if (Sensor != null)
                Sensor.History.CollectionChanged += History_CollectionChanged;
        }

        private void History_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _avaPlot.Plot.Clear();
            if (_contextService.Sensor == null)
                return;
            RefreshPlot();
            _avaPlot.Refresh();
        }

        private void RefreshPlot()
        {
            if (Sensor == null)
                return;
            var xs = Sensor.History.Select(x => x.Time).ToArray();
            var ys = Sensor.History.Select(y => y.Value).ToArray();
            var scatterPlot = _avaPlot.Plot.AddScatter(xs, ys);
            scatterPlot.MarkerShape = ScottPlot.MarkerShape.none;
            _avaPlot.Plot.SetAxisLimits(yMin: Sensor.MinValue, yMax: Sensor.MaxValue);
        }
    }
}
