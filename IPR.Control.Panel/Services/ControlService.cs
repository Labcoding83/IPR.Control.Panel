using IPR.Control.Panel.Common;
using IPR.Control.Panel.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IPR.Control.Panel.Services
{
    public class ControlService : IControlService
    {
        private readonly ILogger _logger;
        private readonly IComputerService _computerService;
        private ObservableCollection<HardWare> _hardWares;
        private IEnumerable<Models.Control> _controls;
        private IEnumerable<Sensor> _sensors;

        public ControlService(
            IComputerService computerService,
            ILogger logger)
        {
            _logger = logger;
            _computerService = computerService;
        }

        public void Init(ObservableCollection<HardWare> hardwares)
        {
            _hardWares = hardwares;

            foreach (var hardware in _computerService.Hardwares)
            {
                var h = _hardWares.First(x => x.Name == hardware.Name && x.Type == hardware.HardwareType.ToString());
                foreach (var controlType in hardware.Controls.GroupBy(x => x.ControlType))
                {
                    var t = new ControlType()
                    {
                        Name = controlType.Key.ToString()
                    };

                    foreach (var control in controlType)
                    {
                        var c = new Models.Control(control)
                        {
                            Value = control.Value,
                            Id = control.Identifier.ToString()
                        };
                        c.PropertyChanged += C_PropertyChanged;
                        t.Controls.Add(c);
                    }

                    h.ControlTypes.Add(t);
                }
            }

            _controls = _hardWares.SelectMany(x => x.ControlTypes).SelectMany(x => x.Controls);
            _sensors = _hardWares.SelectMany(x => x.SensorTypes).SelectMany(x => x.Sensors);
        }

        private void C_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var control = (sender as Models.Control);
            switch (e.PropertyName)
            {
                case nameof(Models.Control.BindedSensorId):
                    control.BindedSensor = _sensors.First(x => x.Id == control.BindedSensorId);
                    break;
                case nameof(Models.Control.ControllerType):
                    switch(control.ControllerType)
                    {
                        case ControllerType.Default:
                        case ControllerType.Fixed:
                            control.GraphCoordinates = new();
                            break;
                        case ControllerType.Graph:
                            if (control.Markers == null || !control.Markers.Any())
                                return;
                            SetGraphCoordinates(control);
                            break;
                    }
                    break;
                case nameof(Models.Control.Markers):
                    if (control.ControllerType == ControllerType.Graph)
                        SetGraphCoordinates(control);
                    break;
            }
        }

        private static void SetGraphCoordinates(Models.Control control)
        {
            control.GraphCoordinates = new();
            var xs = control.Markers.Select(x => x.X).ToArray();
            var ys = control.Markers.Select(x => x.Y).ToArray();
            (double[] bzX, double[] bzY) = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(xs, ys, Constants.HISTORY_TIMESPAN);
            for (int i = 0; i < Constants.HISTORY_TIMESPAN; i++)
            {
                var x = Math.Round(bzX[i]);
                var y = Math.Round(bzY[i], 2);
                var point = new Point() { X = x, Y = y };
                control.GraphCoordinates.Add(point);
            }
        }

        public void UpdateContinuously(CancellationToken token)
        {
            Task.Factory.StartNew(async () => {
                await Task.Delay(Constants.UPDATE_DELAY);
                await SetControlValues(token);
            }, token);
        }

        private async Task SetControlValues(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var controlsToUpdate = _controls.Where(x => x.ControllerType == ControllerType.Fixed || x.ControllerType == ControllerType.Graph);

                    foreach (var control in controlsToUpdate)
                    {
                        if (control.ControllerType == ControllerType.Fixed)
                            CheckAndSetControlFixedValue(control);
                        if (control.ControllerType == ControllerType.Graph)
                            CheckAndSetControlGraphValue(control);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                }
                finally
                {
                    await Task.Delay(Constants.UPDATE_DELAY);
                }
            }
        }

        private void CheckAndSetControlGraphValue(Models.Control control)
        {
            if (control.BindedSensor == null)
                return;
            var bindedSensor = control.BindedSensor.Value;
            var nearestValue = control.GraphCoordinates
                .Aggregate((x, y) => Math.Abs(x.X - bindedSensor) < Math.Abs(y.X - bindedSensor) ? x : y);
            if (control.Sensor == null)
                return;
            var sensorValue = Math.Round(control.Sensor.Value, 0);
            var wantedValue = Math.Round(nearestValue.Y, 0);
            if (sensorValue == wantedValue)
                return;
            control.ChangeValue(wantedValue);
        }

        private void CheckAndSetControlFixedValue(Models.Control control)
        {
            if (control.Sensor == null)
                return;
            var sensorValue = control.Sensor.Value;
            var controlValue = control.Value;
            if (sensorValue == controlValue)
                return;
            control.ChangeValue(control.Value);
        }
    }
}
