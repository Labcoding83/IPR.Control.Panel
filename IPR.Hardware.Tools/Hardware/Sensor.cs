// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace IPR.Hardware.Tools.Hardware
{
    internal class Sensor : ISensor
    {
        private double _currentValue;
        private string _name;

        public Sensor
        (
            string name,
            int index,
            SensorType sensorType,
            Hardware hardware)
        {
            _name = name;
            Index = index;
            SensorType = sensorType;

            Parameters = new();
            Identifier = new Identifier(hardware.Identifier, SensorType.ToString().ToLowerInvariant(), Index.ToString(CultureInfo.InvariantCulture));
        }

        public Identifier Identifier { get; private set; }

        public int Index { get; }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<IParameter> Parameters { get; set; }
        public void AddValueParameter(string name, string description, float defaultValue)
        {
            Parameters.Add(new Parameter(name, description, defaultValue));
        }
        public void AddRangeParameter(float min, float max)
        {
            Parameters.Add(new Parameter(Constants.RANGE, string.Empty, min, max));
        }

        public SensorType SensorType { get; }

        public UnitType UnitType
        {
            get
            {
                switch (SensorType)
                {
                    case SensorType.Voltage:
                        return UnitType.Volt;
                    case SensorType.VoltageOffset:
                        return UnitType.mV;
                    case SensorType.Current:
                        return UnitType.Amp;
                    case SensorType.Power:
                        return UnitType.Watt;
                    case SensorType.Clock:
                        return UnitType.MHz;
                    case SensorType.Frequency:
                        return UnitType.Hz;
                    case SensorType.Temperature:
                    case SensorType.TemperatureTjMax:
                        return UnitType.Celsius;
                    case SensorType.Load:
                    case SensorType.Level:
                        return UnitType.Percent;
                    case SensorType.Fan:
                        return UnitType.RPM;
                    case SensorType.Flow:
                        return UnitType.Lh;
                    case SensorType.Data:
                        return UnitType.GB;
                    case SensorType.SmallData:
                        return UnitType.MB;
                    case SensorType.Throughput:
                        return UnitType.Bs;
                    case SensorType.TimeSpan:
                        return UnitType.Seconds;
                    case SensorType.Energy:
                        return UnitType.mWh;
                    case SensorType.FanLevel:
                        return UnitType.Level;
                    case SensorType.Factor:
                    default:
                        return UnitType.Undefined;
                }
            }
        }

        public virtual double Value
        {
            get { return _currentValue; }
            set
            {
                _currentValue = Math.Round(value, 2);
                _valueTime = DateTime.UtcNow;
            }
        }

        private DateTime _valueTime;
        public DateTime ValueTime
        {
            get => _valueTime;
        }
    }
}
