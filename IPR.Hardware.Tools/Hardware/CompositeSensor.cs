// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// All Rights Reserved.

using System;
using System.Linq;

namespace IPR.Hardware.Tools.Hardware
{
    internal class CompositeSensor : Sensor
    {
        private readonly ISensor[] _components;
        private readonly Func<double, ISensor, double> _reducer;
        private readonly double _seedValue;

        public CompositeSensor
        (
            string name,
            int index,
            SensorType sensorType,
            Hardware hardware,
            ISensor[] components,
            Func<double, ISensor, double> reducer,
            double seedValue = .0f)
            : base(name, index, sensorType, hardware)
        {
            _components = components;
            _reducer = reducer;
            _seedValue = seedValue;
        }

        public override double Value
        {
            get { return _components.Aggregate(_seedValue, _reducer); }
            set => throw new NotImplementedException();
        }
    }
}
