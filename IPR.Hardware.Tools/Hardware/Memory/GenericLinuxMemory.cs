// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System.IO;
using System.Linq;

namespace IPR.Hardware.Tools.Hardware.Memory
{
    internal sealed class GenericLinuxMemory : Hardware
    {
        private readonly Sensor _physicalMemoryAvailable;
        private readonly Sensor _physicalMemoryLoad;
        private readonly Sensor _physicalMemoryUsed;
        private readonly Sensor _virtualMemoryAvailable;
        private readonly Sensor _virtualMemoryLoad;
        private readonly Sensor _virtualMemoryUsed;


        public override HardwareType HardwareType => HardwareType.Memory;


        public GenericLinuxMemory(string name) : base(name, new Identifier("ram"))
        {
            _physicalMemoryUsed = new Sensor("Memory Used", 0, SensorType.Data, this);
            ActivateSensor(_physicalMemoryUsed);

            _physicalMemoryAvailable = new Sensor("Memory Available", 1, SensorType.Data, this);
            ActivateSensor(_physicalMemoryAvailable);

            _physicalMemoryLoad = new Sensor("Memory", 0, SensorType.Load, this);
            _physicalMemoryLoad.AddRangeParameter(0, 100);
            ActivateSensor(_physicalMemoryLoad);

            _virtualMemoryUsed = new Sensor("Virtual Memory Used", 2, SensorType.Data, this);
            ActivateSensor(_virtualMemoryUsed);

            _virtualMemoryAvailable = new Sensor("Virtual Memory Available", 3, SensorType.Data, this);
            ActivateSensor(_virtualMemoryAvailable);

            _virtualMemoryLoad = new Sensor("Virtual Memory", 1, SensorType.Load, this);
            _virtualMemoryLoad.AddRangeParameter(0, 100);
            ActivateSensor(_virtualMemoryLoad);
        }


        public override void Update()
        {
            try
            {
                string[] memoryInfo = File.ReadAllLines("/proc/meminfo");

                {
                    float totalMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("MemTotal:"))) / 1024.0f / 1024.0f;
                    float freeMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("MemFree:"))) / 1024.0f / 1024.0f;
                    float cachedMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("Cached:"))) / 1024.0f / 1024.0f;

                    float usedMemory_GB = totalMemory_GB - freeMemory_GB - cachedMemory_GB;

                    _physicalMemoryUsed.Value = usedMemory_GB;
                    _physicalMemoryAvailable.Value = totalMemory_GB;
                    _physicalMemoryLoad.Value = 100.0f * (usedMemory_GB / totalMemory_GB);
                }
                {
                    float totalSwapMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("SwapTotal"))) / 1024.0f / 1024.0f;
                    float freeSwapMemory_GB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("SwapFree"))) / 1024.0f / 1024.0f;
                    float usedSwapMemory_GB = totalSwapMemory_GB - freeSwapMemory_GB;

                    _virtualMemoryUsed.Value = usedSwapMemory_GB;
                    _virtualMemoryAvailable.Value = totalSwapMemory_GB;
                    _virtualMemoryLoad.Value = 100.0f * (usedSwapMemory_GB / totalSwapMemory_GB);
                }
            }
            catch
            {
                _physicalMemoryUsed.Value = 0;
                _physicalMemoryAvailable.Value = 0;
                _physicalMemoryLoad.Value = 0;

                _virtualMemoryUsed.Value = 0;
                _virtualMemoryAvailable.Value = 0;
                _virtualMemoryLoad.Value = 0;
            }
        }

        private long GetMemInfoValue(string line)
        {
            // Example: "MemTotal:       32849676 kB"

            string valueWithUnit = line.Split(':').Skip(1).First().Trim();
            string valueAsString = valueWithUnit.Split(' ').First();

            return long.Parse(valueAsString);
        }
    }
}
