// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace IPR.Hardware.Tools.Hardware.CPU
{
    internal sealed partial class IntelCpu : GenericCpu
    {
        public IntelCpu(int processorIndex, CpuId[][] cpuId) : base(processorIndex, cpuId)
        {
            InitSensors(cpuId);
            InitControls();
            Update();
        }

        partial void InitSensors(CpuId[][] cpuId);
        partial void InitControls();

        public float EnergyUnitsMultiplier { get; private set; }

        protected override uint[] GetMsrs()
        {
            return new[]
            {
                MSR_PLATFORM_INFO,
                IA32_PERF_STATUS,
                IA32_THERM_STATUS_MSR,
                IA32_TEMPERATURE_TARGET,
                IA32_PACKAGE_THERM_STATUS,
                MSR_RAPL_POWER_UNIT,
                MSR_PKG_ENERGY_STATUS,
                MSR_DRAM_POWER_LIMIT,
                MSR_DRAM_ENERGY_STATUS,
                MSR_PP0_ENERGY_STATUS,
                MSR_PP1_ENERGY_STATUS,
                MSR_TURBO_RATIO_LIMIT,
                MSR_IA32_OC_MAILBOX
            };
        }

        public override string GetReport()
        {
            StringBuilder r = new();
            r.Append(base.GetReport());
            r.Append("MicroArchitecture: ");
            r.AppendLine(_microArchitecture.ToString());
            r.Append("Time Stamp Counter Multiplier: ");
            r.AppendLine(_timeStampCounterMultiplier.ToString(CultureInfo.InvariantCulture));
            r.AppendLine();
            return r.ToString();
        }

        public override void Update()
        {
            base.Update();
            UpdateSensors();
        }

        partial void UpdateSensors();

        [SuppressMessage("ReSharper", "IdentifierTypo")]
        private enum MicroArchitecture
        {
            Airmont,
            AlderLake,
            Atom,
            Broadwell,
            CannonLake,
            CometLake,
            Core,
            Goldmont,
            GoldmontPlus,
            Haswell,
            IceLake,
            IvyBridge,
            JasperLake,
            KabyLake,
            Nehalem,
            NetBurst,
            RocketLake,
            SandyBridge,
            Silvermont,
            Skylake,
            TigerLake,
            Tremont,
            Unknown
        }

        // ReSharper disable InconsistentNaming
        private const uint IA32_PACKAGE_THERM_STATUS = 0x1B1;
        private const uint IA32_PERF_STATUS = 0x0198;
        private const uint IA32_TEMPERATURE_TARGET = 0x01A2;
        private const uint IA32_THERM_STATUS_MSR = 0x019C;

        private const uint MSR_DRAM_POWER_LIMIT = 0x618;
        private const uint MSR_DRAM_ENERGY_STATUS = 0x619;
        private const uint MSR_PKG_ENERGY_STATUS = 0x611;
        private const uint MSR_PLATFORM_INFO = 0xCE;
        private const uint MSR_IA32_OC_MAILBOX = 0x150;
        private const uint MSR_TURBO_RATIO_LIMIT = 0x1AD;
        private const uint MSR_PP0_ENERGY_STATUS = 0x639;
        private const uint MSR_PP1_ENERGY_STATUS = 0x641;

        private const uint MSR_RAPL_POWER_UNIT = 0x606;
        // ReSharper restore InconsistentNaming
    }
}
