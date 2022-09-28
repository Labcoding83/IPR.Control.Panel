// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

namespace IPR.Hardware.Tools.Hardware
{
    public enum ParameterType
    {
        Value,
        Range
    }

    internal class Parameter : IParameter
    {
        public Parameter(string name, string description, float value)
        {
            Name = name;
            Description = description;
            Value = value;
            ParameterType = ParameterType.Value;
        }

        public Parameter(string name, string description, float minValue, float maxValue)
        {
            Name = name;
            Description = description;
            MinValue = minValue;
            MaxValue = maxValue;
            ParameterType = ParameterType.Range;
        }

        public ParameterType ParameterType { get; private set; }

        /// <summary>
        /// Gets a name of the parent <see cref="ISensor"/> or <see cref="IControl"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a description of the parent <see cref="ISensor"/> or <see cref="IControl"/>.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a default value of the parent <see cref="ISensor"/> or <see cref="IControl"/>.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Gets a min range value of the parent <see cref="ISensor"/> or <see cref="IControl"/>.
        /// </summary>
        public float MinValue { get; }
        /// <summary>
        /// Gets a max range value of the parent <see cref="ISensor"/> or <see cref="IControl"/>.
        /// </summary>
        public float MaxValue { get; }
    }
}
