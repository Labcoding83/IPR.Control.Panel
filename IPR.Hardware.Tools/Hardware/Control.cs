// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Globalization;

namespace IPR.Hardware.Tools.Hardware
{
    internal delegate void ControlEventHandler(Control control);

    internal class Control : IControl
    {
        public Control
        (
            string name,
            int index,
            Hardware hardware,
            ControlType controlType,
            float minValue,
            float maxValue,
            Func<int, double, double> changeValue,
            Action<int, double> setDefaultValue = null)
        {
            Name = name;
            Index = index;
            ControlType = controlType;
            MinValue = minValue;
            MaxValue = maxValue;
            _changeValue = changeValue;
            _setDefaultValue = setDefaultValue;
            Identifier = new Identifier(hardware.Identifier, ControlType.ToString().ToLowerInvariant(), Index.ToString(CultureInfo.InvariantCulture));
        }

        public string Name { get; set; }
        public int Index { get; }
        public ControlType ControlType { get; }
        public Identifier Identifier { get; private set; }

        public virtual double Value { get; set; }

        public float MaxValue { get; }

        public float MinValue { get; }

        public UnitType UnitType
        {
            get
            {
                switch (ControlType)
                {
                    case ControlType.Control:
                        return UnitType.Undefined;
                    case ControlType.VoltageOffset:
                        return UnitType.mV;
                    case ControlType.Fan:
                        return UnitType.RPM;
                    default:
                        return UnitType.Undefined;
                }
            }
        }

        private Func<int, double, double> _changeValue;
        public double ChangeValue(double value)
        {
            if(_changeValue is null)
                throw new ArgumentNullException("Action must be defined.");
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException();
            return _changeValue(Index, value);
        }

        private Action<int, double> _setDefaultValue;
        public void SetDefaultValue(double value)
        {
            if (_changeValue is null)
                throw new ArgumentNullException("Action must be defined.");
            _setDefaultValue(Index, value);
        }
    }
}
