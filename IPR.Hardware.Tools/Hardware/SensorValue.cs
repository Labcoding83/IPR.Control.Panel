// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;

namespace IPR.Hardware.Tools.Hardware
{
    /// <summary>
    /// Stores the readed value and the time in which it was recorded.
    /// </summary>
    public struct SensorValue
    {
        /// <param name="value"><see cref="Value"/> of the sensor.</param>
        /// <param name="time">The time code during which the <see cref="Value"/> was recorded.</param>
        public SensorValue(float value, DateTime time)
        {
            Value = value;
            Time = time;
        }

        /// <summary>
        /// Gets the value of the sensor
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Gets the time code during which the <see cref="Value"/> was recorded.
        /// </summary>
        public DateTime Time { get; }
    }
}
