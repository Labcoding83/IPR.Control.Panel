using IPR.Control.Panel.Common;
using IPR.Hardware.Tools.Hardware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace IPR.Control.Panel.Models
{
    public class Control : IControl, INotifyPropertyChanged
    {
        public Control() { }
        private IControl _control;
        public Control(IControl control) 
        {
            _control = control;
        }
        public string Name { get => _control.Name; }
        public float MinValue { get => _control.MinValue; }
        public float MaxValue { get => _control.MaxValue;}

        public IPR.Hardware.Tools.Hardware.ControlType ControlType { get => _control.ControlType; }

        public int Index { get => _control.Index; }

        public UnitType UnitType { get => _control.UnitType; }
        private Identifier _identifier;
        public Identifier Identifier { get => _identifier; }
        public string Id { get; set; }

        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                var isDifferent = _value != value;
                _value = value;
                if (isDifferent)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayValue)));
                }
            }
        }

        private ControllerType _controllerType;
        public ControllerType ControllerType 
        { 
            get => _controllerType;
            set
            {
                var isDifferent = _controllerType != value;
                _controllerType = value;
                if (!isDifferent)
                    return;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ControllerType)));
            }
        }

        public string DisplayValue
        {
            get { return $"{Value} {UnitType}"; }
        }

        private bool _isLocked = true;
        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                var isDifferent = _isLocked != value;
                _isLocked = value;
                if (!isDifferent)
                    return;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLocked)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsUnlocked)));
            }
        }
        public void LockUnlock()
        {
            IsLocked = !IsLocked;
        }

        private Sensor _sensor;
        public Sensor Sensor
        {
            get => _sensor;
            set
            {
                _sensor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sensor)));
            }
        }
        public bool IsUnlocked { get { return !_isLocked; } }
        private string _bindedSensorId;
        public string BindedSensorId
        {
            get => _bindedSensorId;
            set
            {
                var isDifferent = _bindedSensorId != value;
                _bindedSensorId = value;
                if (!isDifferent)
                    return;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BindedSensorId)));
            }
        }
        public Sensor BindedSensor { get; set; }
        private ObservableCollection<Point> _markers;
        public ObservableCollection<Point> Markers
        {
            get => _markers;
            set
            {
                _markers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Markers)));
            }
        }
        public List<Point> GraphCoordinates { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public bool UpdateValueAfterChange { get; set; } = true;

        public double ChangeValue(double value)
        {
            return _control.ChangeValue(value);
        }

        public void SetDefaultValue(double value)
        {
            _control.SetDefaultValue(value);
        }
    }
}
