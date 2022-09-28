using Avalonia.Threading;
using IPR.Control.Panel.Common;
using IPR.Control.Panel.Models;
using Serilog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IPR.Control.Panel.Services
{
    public class SensorService : ISensorService
    {
        private readonly ILogger _logger;
        private readonly IComputerService _computerService;
        public event EventHandler SensorsUpdated;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private ObservableCollection<HardWare> _hardWares;

        public SensorService(
            IComputerService computerService,
            ILogger logger
            )
        {
            _logger = logger;
            _computerService = computerService;
        }

        public void Init(ObservableCollection<HardWare> hardwares)
        {
            _hardWares = hardwares;
            foreach (var hardware in _computerService.Hardwares)
            {
                var h = new HardWare()
                {
                    Name = hardware.Name,
                    Type = hardware.HardwareType.ToString()
                };
                foreach (var sensorType in hardware.Sensors.GroupBy(x => x.SensorType))
                {
                    var t = new SensorType()
                    {
                        Name = sensorType.Key.ToString()
                    };

                    foreach (var sensor in sensorType)
                    {
                        var s = new Sensor()
                        {
                            Name = sensor.Name,
                            Id = sensor.Identifier.ToString(),
                            Value = sensor.Value,
                            Identifier = sensor.Identifier,
                            Index = sensor.Index,
                            SensorType = sensor.SensorType,
                            Parameters = sensor.Parameters,
                            UnitType = sensor.UnitType
                        };
                        t.Sensors.Add(s);
                    }

                    h.SensorTypes.Add(t);
                }

                _hardWares.Add(h);
            }
        }

        public void UpdateContinuously(CancellationToken token, Action onStarted)
        {
            Task.Factory.StartNew(async () => {
                await Task.Delay(Constants.UPDATE_DELAY);
                onStarted();
                await UpdateValues(token);
            }, token);
        }

        private async Task UpdateValues(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var sensorNodes = _hardWares
                        .SelectMany(x => x.SensorTypes)
                        .SelectMany(x => x.Sensors);

                try
                {
                    await _semaphoreSlim.WaitAsync();
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        foreach (var sensorNode in sensorNodes)
                        {
                            var updatedSensor = _computerService.Sensors.First(x => x.Identifier.ToString() == sensorNode.Id);
                            sensorNode.Value = updatedSensor.Value;

                        }
                        SensorsUpdated?.Invoke(this, EventArgs.Empty);
                    });
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        foreach (var sensorNode in sensorNodes)
                        {
                            sensorNode.Value = 0;
                        }
                    });
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
                await Task.Delay(Constants.UPDATE_DELAY);
            }
        }
    }
}
