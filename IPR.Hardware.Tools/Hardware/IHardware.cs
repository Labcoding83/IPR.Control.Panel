// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Threading;

namespace IPR.Hardware.Tools.Hardware
{
    /// <summary>
    /// Abstract object that stores information about a device. All sensors are available as an array of <see cref="Sensors"/>.
    /// <para>
    /// Can contain <see cref="SubHardware"/>.
    /// Type specified in <see cref="HardwareType"/>.
    /// </para>
    /// </summary>
    public interface IHardware : IElement
    {
        /// <summary>
        /// <inheritdoc cref="LibreHardwareMonitor.Hardware.HardwareType"/>
        /// </summary>
        HardwareType HardwareType { get; }

        /// <summary>
        /// Gets the device that is the parent of the current hardware. For example, the motherboard is the parent of SuperIO.
        /// </summary>
        IHardware Parent { get; }

        /// <summary>
        /// Gets an array of all sensors such as <see cref="SensorType.Temperature"/>, <see cref="SensorType.Clock"/>, <see cref="SensorType.Load"/> etc.
        /// </summary>
        ISensor[] Sensors { get; }

        /// <summary>
        /// Gets an array of all controls such as <see cref="ControlType.VoltageOffset"/>, <see cref="SensorType.Clock"/>, <see cref="SensorType.Load"/> etc.
        /// </summary>
        IControl[] Controls { get; }

        /// <summary>
        /// Gets child devices, e.g. <see cref="LibreHardwareMonitor.Hardware.Motherboard.Lpc.LpcIO"/> of the <see cref="LibreHardwareMonitor.Hardware.Motherboard.Motherboard"/>.
        /// </summary>
        IHardware[] SubHardware { get; }

        /// <summary>
        /// Delay after each Update during <see cref="UpdatingContinuously"/>. 
        /// </summary>
        TimeSpan UpdateDelay { get; }

        /// <summary>
        /// Report containing most of the known information about the current device.
        /// </summary>
        /// <returns>A formatted text string with hardware information.</returns>
        string GetReport();

        /// <summary>
        /// Refreshes the information stored in <see cref="Sensors"/> array.
        /// </summary>
        void Update();

        /// <summary>
        /// Refreshes continuously the information stored in <see cref="Sensors"/> array
        /// </summary>
        void UpdatingContinuously(CancellationToken cancellationToken);

        /// <summary>
        /// Gets rarely changed hardware properties that can't be represented as sensors.
        /// </summary>
        IDictionary<string, string> Properties { get; }
    }
}
