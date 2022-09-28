using IPR.Control.Panel.Common;
using IPR.Control.Panel.Models;
using IPR.Control.Panel.Services;
using IPR.Control.Panel.Views; 
using Newtonsoft.Json.Linq;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IPR.Control.Panel.ViewModels
{
    internal class ControlsViewModel : ViewModelBase
    {
        private readonly IComputerService _computerService;
        private readonly IContextService _contextService;
        private readonly IDialogService _dialogService;
        
        private bool _isCpuOffsetAvailable;
        public bool IsCpuOffsetAvailable
        {
            get { return _isCpuOffsetAvailable; }
            set { SetProperty(ref _isCpuOffsetAvailable, value); }
        }
        private ObservableCollection<Sensor> _cpuOffsetSensor;
        private ObservableCollection<Models.Control> _cpuOffset;
        public ObservableCollection<Models.Control> CpuOffsets
        {
            get => _cpuOffset;
            set
            {
                SetProperty(ref _cpuOffset, value);
            }
        }

        private bool _isFanAvailable;
        public bool IsFanAvailable
        {
            get { return _isFanAvailable; }
            set { SetProperty(ref _isFanAvailable, value); }
        }
        private ObservableCollection<Sensor> _fanSensor;
        private ObservableCollection<Models.Control> _fanControls;
        public ObservableCollection<Models.Control> FanControls
        {
            get => _fanControls;
            set
            {
                SetProperty(ref _fanControls, value);
            }
        }

        public ControlsViewModel(
            IComputerService computerService,
           IContextService contextService,
           IDialogService dialogService)
        {
            _computerService = computerService;
            _contextService = contextService;
            _dialogService = dialogService;

            InitCpuOffsets();
            InitFans();
        }

        private void InitFans()
        {
            var fans = _contextService.Hardware
                .SelectMany(x => x.ControlTypes)
                .Where(x => x.Name == IPR.Hardware.Tools.Hardware.ControlType.FanLevel.ToString());
            if (!fans.Any())
            {
                IsFanAvailable = false;
                return;
            }
            IsFanAvailable = true;
            FanControls = fans.First(x => x.Name == IPR.Hardware.Tools.Hardware.ControlType.FanLevel.ToString()).Controls;
            foreach (var fanControl in FanControls)
                fanControl.UpdateValueAfterChange = false;

            var _fanSensor = _contextService.Hardware
                .SelectMany(x => x.SensorTypes)
                .Where(x => x.Name == IPR.Hardware.Tools.Hardware.SensorType.FanLevel.ToString())
                .SelectMany(x => x.Sensors);

            foreach (var sensor in _fanSensor)
            {
                SetControlValue(FanControls, sensor);
            }
        }

        private void InitCpuOffsets()
        {
            var cpuOffsets = _contextService.Hardware
                .SelectMany(x => x.ControlTypes)
                .FirstOrDefault(x => x.Name == IPR.Hardware.Tools.Hardware.ControlType.VoltageOffset.ToString());
            if(cpuOffsets == null)
            {
                IsCpuOffsetAvailable = false;
                return;
            }

            IsCpuOffsetAvailable = true;
            CpuOffsets = cpuOffsets.Controls;

            var cpuSensor = _contextService.Hardware
                .SelectMany(x => x.SensorTypes)
                .Where(x => x.Name == IPR.Hardware.Tools.Hardware.SensorType.VoltageOffset.ToString())
                .SelectMany(x => x.Sensors);

            _cpuOffsetSensor = new ObservableCollection<Sensor>(cpuSensor);
            foreach (var sensor in cpuSensor)
            {
                SetControlValue(CpuOffsets, sensor);
            }
        }

        private static void SetControlValue(IEnumerable<Models.Control> controls, Sensor sensor)
        {
            controls.First(x => x.Index == sensor.Index).Sensor = sensor;
        }

        public void SetDefaultValue(Models.Control control, double value)
        {
            control.SetDefaultValue(value);
        }

        public void ChangeValue(Models.Control control, double value)
        {
            control.Value = control.ChangeValue(value);
        }

        public void OpenDialog(Models.Control control, Action callback)
        {
            _contextService.SelectedControl = control;
            _dialogService.ShowDialog(nameof(GraphDialogView), r =>
            {
                callback();
            });
            _contextService.SelectedControl = null;
        }

        public void ToggleControl(Models.Control control)
        {
            control.LockUnlock();
            if (control.IsLocked)
            {
                control.SetDefaultValue(control.Value);
            }
        }
    }
}
