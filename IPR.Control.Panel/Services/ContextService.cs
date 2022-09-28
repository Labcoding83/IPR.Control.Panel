using IPR.Control.Panel.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace IPR.Control.Panel.Services
{
    public class ContextService : IContextService
    {
        private readonly ILogger _logger;
        private readonly IComputerService _computerService;
        private readonly ISensorService _sensorService;
        private readonly IControlService _controlService;
        private CancellationTokenSource _cancellationTokenSource = new();
        
        public ObservableCollection<HardWare> Hardware { get; private set; } = new ObservableCollection<HardWare>();
        
        public event EventHandler<RegionVisibiltyChangedEventArgs> RegionVisibiltyChanged;

        private Sensor? _sensor;
        public Sensor? Sensor
        {
            get => _sensor;
            set
            {
                var changed = _sensor?.Id != value?.Id;
                _sensor = value;
                if (changed)
                    SelectedSensorChanged?.Invoke(value, EventArgs.Empty);
            }
        }

        public Models.Control SelectedControl { get; set; }

        public event EventHandler SelectedSensorChanged;

        private bool _isInitialized = false;

        public ContextService(
            IComputerService computerService,
            ISensorService sensorService,
            IControlService controlService,
            ILogger logger)
        {
            _logger = logger;
            _computerService = computerService;
            _sensorService = sensorService;
            _controlService = controlService;
        }

        public void Init()
        {
            _computerService.Open();
            _computerService.Monitor();
            _sensorService.Init(Hardware);
            _controlService.Init(Hardware);
            LoadSettings();

            _cancellationTokenSource = new CancellationTokenSource();
            _sensorService.UpdateContinuously(_cancellationTokenSource.Token, () => _isInitialized = true);
            _controlService.UpdateContinuously(_cancellationTokenSource.Token);
            foreach(var control in Hardware.SelectMany(x => x.ControlTypes).SelectMany(x => x.Controls))
            {
                control.PropertyChanged += Control_PropertyChanged;
            }
        }

        public void OnRegionVisibiltyChanged(string region, bool isVisible)
        {
            RegionVisibiltyChanged?.Invoke(this, new RegionVisibiltyChangedEventArgs(region, isVisible));
        }

        private void Control_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!_isInitialized)
                return;
            SaveControllerSettings();
        }

        public void SaveControllerSettings()
        {
            string jsonString = JsonSerializer.Serialize(Hardware);
            File.WriteAllText("appstate.json", jsonString);
        }

        private void LoadSettings()
        {
            try
            {
                var json = File.ReadAllText("appstate.json");
                var hardwares = JsonSerializer.Deserialize<List<HardWare>>(json);

                if (hardwares == null || !hardwares.Any())
                    return;

                var controlsToLoad = hardwares
                    .SelectMany(x => x.ControlTypes)
                    .SelectMany(x => x.Controls)
                    .ToList();

                var controls = Hardware
                                .SelectMany(x => x.ControlTypes)
                                .SelectMany(x => x.Controls)
                                .ToList();
                var sensors = Hardware
                                .SelectMany(x => x.SensorTypes)
                                .SelectMany(x => x.Sensors)
                                .ToList();

                foreach (var controlToLoad in controlsToLoad)
                {
                    var control = controls.FirstOrDefault(x => x.Id == controlToLoad.Id);
                    if (control == null)
                        continue;
                    control.IsLocked = controlToLoad.IsLocked;
                    control.ControllerType = controlToLoad.ControllerType;
                    control.Value = controlToLoad.Value;
                    control.BindedSensorId = controlToLoad.BindedSensorId;
                    if (!string.IsNullOrEmpty(controlToLoad.BindedSensorId))
                        control.BindedSensor = sensors.First(x => x.Id == controlToLoad.BindedSensorId);
                    control.Markers = controlToLoad.Markers;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }
    }

    public class RegionVisibiltyChangedEventArgs : EventArgs
    {
        public string RegionName { get; set; }
        public bool IsVisible { get; set; }
        public RegionVisibiltyChangedEventArgs(string regionName, bool isVisible)
        {
            RegionName = regionName;
            IsVisible = isVisible;
        }
    }
}
