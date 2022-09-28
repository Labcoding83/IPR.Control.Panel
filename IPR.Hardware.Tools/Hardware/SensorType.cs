// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

namespace IPR.Hardware.Tools.Hardware
{
    /// <summary>
    /// Category of what type the selected sensor is.
    /// </summary>
    public enum SensorType
    {
        Voltage, // V
        VoltageOffset,
        Current, // A
        Power, // W
        Clock, // MHz
        Temperature, // °C
        TemperatureTjMax, // °C
        Load, // %
        Frequency, // Hz
        Fan, // RPM
        FanLevel, // Integer
        Flow, // L/h
        Level, // %
        Factor, // 1
        Data, // GB = 2^30 Bytes
        SmallData, // MB = 2^20 Bytes
        Throughput, // B/s
        TimeSpan, // Seconds 
        Energy // milliwatt-hour (mWh)
    }
}
