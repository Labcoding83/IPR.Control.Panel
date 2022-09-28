using IPR.Control.Panel.Common;
using IPR.Hardware.Tools;
using IPR.Hardware.Tools.Hardware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using Constants = IPR.Control.Panel.Common.Constants;

namespace IPR.Control.Panel.Models
{
    public class Sensor : ISensor, INotifyPropertyChanged
    {
        public Sensor()
        {
            for (var i = Constants.HISTORY_TIMESPAN; i > 0; i--)
            {
                var time = (DateTime.UtcNow.AddSeconds(-1 * i)).ToOADate();
                History.Add(new ValueHistory() { Time = time, Value = 0 });
            }
        }
        public string Name { get; set; }
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
                while (History.Count > Constants.HISTORY_TIMESPAN)
                    History.RemoveAt(0);
                History.Add(new ValueHistory() { Time = DateTime.UtcNow.ToOADate(), Value = value });

                if (value >= Max)
                    Max = value;
            }
        }

        public int Index { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public List<IParameter> Parameters { get; set; }

        public IPR.Hardware.Tools.Hardware.SensorType SensorType { get; set; }

        public UnitType UnitType { get; set; }

        public Identifier Identifier { get; set; }

        public string DisplayValue
        {
            get { return $"{Value} {UnitType}"; }
        }

        public double DefaultValue
        {
            get
            {
                var defaultValParameter = Parameters
                    .Where(x => x.ParameterType == ParameterType.Value);
                if (!defaultValParameter.Any())
                    return 0;// throw new ArgumentException("No default values found");
                if (defaultValParameter.Count() > 1)
                    throw new ArgumentException("Only one default value allowed.");
                return defaultValParameter.First().Value;
            }
        }

        private double _max;
        public double Max
        {
            get => _max;
            set
            {
                var isDifferent = _max != value;
                _max = value;
                if (isDifferent)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Max)));
            }
        }

        public double? MinValue
        {
            get => Parameters.FirstOrDefault(x => x.Name == IPR.Hardware.Tools.Constants.RANGE)?.MinValue;
        }
        
        public double? MaxValue
        {
            get => Parameters.FirstOrDefault(x => x.Name == IPR.Hardware.Tools.Constants.RANGE)?.MaxValue;
        }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public ObservableCollection<ValueHistory> History { get; set; } = new();
        public bool IsExpanded { get => false; }

        public event PropertyChangedEventHandler? PropertyChanged;


    }
}
