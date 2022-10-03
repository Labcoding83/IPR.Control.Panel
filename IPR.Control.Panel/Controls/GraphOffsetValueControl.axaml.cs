using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using IPR.Control.Panel.ViewModels;
using ScottPlot.Avalonia;
using System.Linq;
using System;
using ScottPlot.Plottable;
using System.Drawing;
using IPR.Control.Panel.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IPR.Control.Panel.Common;

namespace IPR.Control.Panel.Controls
{
    public partial class GraphOffsetValueControl : UserControl
    {
        private AvaPlot _graphEditor;
        private ScatterPlot _controlPlot;
        private SignalPlot _historyPlot;
        private Models.Control _control;
        private Sensor _sensor;
        public GraphOffsetValueControl()
        {
            InitializeComponent();
            _graphEditor = this.Find<AvaPlot>("graphEditor");
            _graphEditor.Plot.Frameless();
            _graphEditor.Configuration.LeftClickDragPan = false;
            _graphEditor.Configuration.RightClickDragZoom = false;
            _graphEditor.Configuration.DoubleClickBenchmark = false;
            _graphEditor.Configuration.ScrollWheelZoom = false;
            _graphEditor.ContextMenu = null;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            if (DataContext is not Models.Control)
                return;
            _control = DataContext as Models.Control;
            InitGraph();
        }

        private void InitGraph()
        {
            SetMarker();
            SetSensor();
        }
        private void SetSensor()
        {
            _sensor = _control.Sensor;
            if(_sensor == null)
                return;
            _graphEditor.Plot.SetAxisLimits(yMin: _sensor.MinValue, yMax: _sensor.MaxValue, yAxisIndex: 1);
            _sensor.History.CollectionChanged += History_CollectionChanged;
        }

        private void History_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var history = sender as ObservableCollection<ValueHistory>;
            if (history == null)
                return;
            RefreshSensorPlot(history);
        }

        private void RefreshSensorPlot(IEnumerable<ValueHistory> history)
        {
            var ys = history.Select(y => y.Value).ToArray();
            if (_historyPlot != null)
                _graphEditor.Plot.Remove(_historyPlot);
            _historyPlot = _graphEditor.Plot.AddSignal(ys, color: Color.DarkBlue);
            _graphEditor.Plot.SetAxisLimits(yMin: ys.Min(), yMax: ys.Max(), xMin: 0, xMax: Constants.HISTORY_TIMESPAN, yAxisIndex: 1);
            _historyPlot.MarkerShape = ScottPlot.MarkerShape.none;
            _historyPlot.YAxisIndex = 1;
            _graphEditor.Refresh();
        }

        private void SetMarker()
        {
            if(_control.Markers == null || !_control.Markers.Any())
                return;
            var xs = _control.Markers.Select(x => x.X).ToArray();
            var ys = _control.Markers.Select(x => x.Y).ToArray();
            if (_controlPlot != null)
                _graphEditor.Plot.Remove(_controlPlot);
            _graphEditor.Plot.SetAxisLimits(yMin: ys.Min(), yMax: ys.Max(), xMin: 0, xMax: Constants.HISTORY_TIMESPAN, yAxisIndex: 0);
            _controlPlot = _graphEditor.Plot.AddScatterLines(xs, ys, Color.LightBlue);            
            _controlPlot.YAxisIndex = 0;
            _graphEditor.Refresh();
        }

        private void EditGraph_Clicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var controlViewModel = ((Avalonia.StyledElement)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)sender).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).DataContext as ControlsViewModel;
            controlViewModel.OpenDialog(_control, SetMarker);
        }
    }
}
