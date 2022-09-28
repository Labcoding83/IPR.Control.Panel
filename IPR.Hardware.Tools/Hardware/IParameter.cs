// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

namespace IPR.Hardware.Tools.Hardware
{
    /// <summary>
    /// Abstract object that represents additional parameters included in <see cref="ISensor"/>.
    /// </summary>
    public interface IParameter
    {
        string Name { get; }
        /// <summary>
        /// Gets a parameter default value defined by library.
        /// </summary>
        double Value { get; }

        /// <summary>
        /// Gets a parameter description defined by library.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Gets a min range value of the parent <see cref="ISensor"/> or <see cref="IControl"/>.
        /// </summary>
        public float MinValue { get; }
        /// <summary>
        /// Gets a max range value of the parent <see cref="ISensor"/> or <see cref="IControl"/>.
        /// </summary>
        public float MaxValue { get; }
        /// <summary>
        /// Value or range of values
        /// </summary>
        ParameterType ParameterType { get; }
    }
}
