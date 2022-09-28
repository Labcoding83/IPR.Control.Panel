// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System.Collections.Generic;

namespace IPR.Hardware.Tools.Hardware
{

    /// <summary>
    /// Stores information about the readed values and the time in which they were collected.
    /// </summary>
    public interface ISensor : IElement
    {
        /// <summary>
        /// Gets the unique identifier of this sensor for a given <see cref="IHardware"/>.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets or sets a sensor name.
        /// <para>By default determined by the library.</para>
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Parameters needed for initializing/updating <see cref="Value"/>
        /// </summary>
        List<IParameter> Parameters { get; }

        /// <summary>
        /// <inheritdoc cref="LibreHardwareMonitor.Hardware.SensorType"/>
        /// </summary>
        SensorType SensorType { get; }

        /// <summary>
        /// Unit type of <see cref="Value"/>
        /// </summary>
        UnitType UnitType { get; }

        /// <summary>
        /// Gets the last recorded value for the given sensor.
        /// </summary>
        double Value { get; }
    }
}
