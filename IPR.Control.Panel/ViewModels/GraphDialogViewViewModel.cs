using Avalonia.Controls;
using DynamicData;
using IPR.Control.Panel.Models;
using IPR.Control.Panel.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace IPR.Control.Panel.ViewModels
{
    public class GraphDialogViewViewModel : BindableBase, IDialogAware
    {
        private readonly IContextService _contextService;
        private readonly IDialogService _dialog;
        private AvaPlot _plot;
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private Models.Control _control;
        public Models.Control Control
        {
            get => _control;
            set => SetProperty(ref _control, value);
        }

        private ObservableCollection<Sensor> _sensors;
        public ObservableCollection<Sensor> Sensors
        {
            get => _sensors;
            set => SetProperty(ref _sensors, value);
        }
        private Sensor _selectedSensor;
        public Sensor SelectedSensor
        {
            get => _selectedSensor;
            set
            {                 
                SetProperty(ref _selectedSensor, value);
                CanSave = value != null;
                if (_plot == null)
                    return;
                ResetPlot();
                RefreshPlot();
                CanSave = value != null;
            }
        }
        private bool _canSave;
        public bool CanSave
        {
            get => _canSave;
            set => SetProperty(ref _canSave, value);
        }

        private List<DraggableMarkerPlot> _markers = new();

        public GraphDialogViewViewModel(
            IContextService contextService)
        {
            _contextService = contextService;
            var allSensors = _contextService.Hardware
                .SelectMany(x => x.SensorTypes)
                .SelectMany(x => x.Sensors);

            Sensors = new(allSensors
                .Where(x => x.MinValue.HasValue && x.MaxValue.HasValue));

            Control = _contextService.SelectedControl;
            if (!string.IsNullOrEmpty(Control.BindedSensorId))
            {
                SelectedSensor = _contextService.Hardware
                    .SelectMany(x => x.SensorTypes)
                    .SelectMany(x => x.Sensors)
                    .First(x => x.Id == Control.BindedSensorId);
            }
        }

        public DelegateCommand<Window> CmdClearSelection => new DelegateCommand<Window>((win) =>
        {
            SelectedSensor = null;
            ResetPlot();
            RefreshPlot();
        });

        private void ResetPlot()
        {
            var plottables = _plot.Plot.GetPlottables();
            foreach (var plottable in plottables)
                _plot.Plot.Remove(plottable);
        }

        public void SetPlot(AvaPlot plot)
        {
            _plot = plot;
            _plot.Configuration.LeftClickDragPan = false;
            _plot.Configuration.RightClickDragZoom = false;
            _plot.Configuration.DoubleClickBenchmark = false;
            _plot.Configuration.ScrollWheelZoom = false;
            _plot.Plot.YAxis.Label($"{Control.Name} {Control.UnitType}");
            _plot.RightClicked -= _plot.DefaultRightClickEvent;
            _plot.ContextMenu = null;

            RefreshPlot();            
        }

        public void RefreshPlot()
        {
            InitAxis();
            InitMarkers();
            RefreshSignal();
            _plot.Refresh();
        }

        private void InitMarkers()
        {
            _markers = new();
            for (int i = 0; i < 3; i++)
            {
                var x = i switch
                {
                    0 => 0,
                    1 => (SelectedSensor?.MaxValue ?? 100) / 2,
                    2 => (SelectedSensor?.MaxValue ?? 100),
                    _ => 0,
                };

                var marker = _plot.Plot.AddMarkerDraggable(x, 0, MarkerShape.filledDiamond, 20);
                marker.Dragged += Marker_Dragged;
                marker.DragXLimitMin = i switch
                {
                    //2 => (SelectedSensor?.MinValue ?? 100),
                    _ => 0
                }; 
                marker.DragXLimitMax = i switch
                {
                    //0 => 0,
                    _ => (SelectedSensor?.MaxValue ?? 100)
                };
                marker.DragYLimitMin = Control.MinValue;
                marker.DragYLimitMax = Control.MaxValue;

                if(Control.Markers != null && Control.Markers.Any())
                {
                    marker.X = Control.Markers[i].X;
                    marker.Y = Control.Markers[i].Y;
                }

                _markers.Add(marker);
            }
        }

        private void InitAxis()
        {
            var defaultValue = Control.Sensor.DefaultValue;
            if (SelectedSensor == null)
            {
                _plot.Plot.SetAxisLimits(xMin: 0, xMax: 100, yMin: Control.MinValue, yMax: Control.MaxValue);
            }
            else
            {
                _plot.Plot.SetAxisLimits(
                    xMin: SelectedSensor.MinValue.Value,
                    xMax: SelectedSensor.MaxValue.Value,
                    yMin: Control.MinValue,
                    yMax: Control.MaxValue);
            }
        }

        private void RefreshSignal()
        {
            var plottables = _plot.Plot.GetPlottables()
                .Where(x => x is DraggableMarkerPlot)
                .Cast<DraggableMarkerPlot>()
                .ToList();
            if (!plottables.Any())
                return;

            var oldPlot = _plot.Plot.GetPlottables()
                .Where(x => x is ScatterPlot);
            var xs = plottables.Select(x => x.X).ToArray();
            var ys = plottables.Select(x => x.Y).ToArray();
            (double[] bzX, double[] bzY) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(xs, ys, 200);
            if (oldPlot.Any())
                _plot.Plot.Remove(oldPlot.First());
            _plot.Plot.AddScatterLines(xs, ys, Color.LightBlue);
        }

        private void Marker_Dragged(object? sender, EventArgs e)
        {
            var dragger = sender as DraggableMarkerPlot;
            dragger.Y = Math.Round(dragger.Y, 0);
            dragger.X = Math.Round(dragger.X, 0);

            var index = _markers.IndexOf(dragger);
            if (index == -1)
                return;
            if(index == 0)
            {
                var next = _markers[index + 1];
                if (dragger.X >= next.X)
                    dragger.X = next.X;
            }
            if (index > 0 && index < _markers.Count - 1)
            {
                var previous = _markers[index - 1];
                var next = _markers[index + 1];

                if (dragger.X >= next.X)
                    dragger.X = next.X;
                if (dragger.X <= previous.X)
                    dragger.X = previous.X;

            }
            if (index == _markers.Count - 1)
            {
                var previous = _markers[_markers.Count - 2];
                if (dragger.X <= previous.X)
                    dragger.X = previous.X;
            }

            RefreshSignal();
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        public DelegateCommand<string> CmdResult => new DelegateCommand<string>((param) =>
        {
            // None = 0
            // OK = 1
            // Cancel = 2
            // Abort = 3
            // Retry = 4
            // Ignore = 5
            // Yes = 6
            // No = 7
            ButtonResult result = ButtonResult.None;

            if (int.TryParse(param, out int intResult))
                result = (ButtonResult)intResult;

            if (result == ButtonResult.OK)
                Save();

            RaiseRequestClose(new DialogResult(result));
        });

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private void Save()
        {
            var markerPoints = _markers
                .Select(x => new Models.Point() { X = x.X, Y = x.Y })
                .ToList();
            Control.Markers = new ObservableCollection<Models.Point>(markerPoints);
            Control.BindedSensorId = SelectedSensor.Id;
            Control.BindedSensor = SelectedSensor;
        }
    }
}
