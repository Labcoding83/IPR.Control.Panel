// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System.Threading;
using System;

namespace IPR.Hardware.Tools.Hardware
{
    /// <summary>
    /// Basic abstract with methods for the class which can store all hardware and decides which devices are to be checked and updated.
    /// </summary>
    public interface IComputer : IGroup, IElement
    {
        private static readonly Lazy<IComputer> _instance = new Lazy<IComputer>(() =>
        {
            var computer = new Computer();
            computer.Open();
            return computer;
        }, LazyThreadSafetyMode.ExecutionAndPublication);
        static IComputer Instance { get { return _instance.Value; } }
        /// <summary>
        /// Get value indicating whether Computer is opened.
        /// </summary>
        bool IsOpened { get; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Battery" /> devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsBatteryEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about:
        /// <list>
        ///     <item>
        ///         <see cref="Controller.TBalancer.TBalancerGroup" />
        ///     </item>
        ///     <item>
        ///         <see cref="Controller.Heatmaster.HeatmasterGroup" />
        ///     </item>
        ///     <item>
        ///         <see cref="Controller.AquaComputer.AquaComputerGroup" />
        ///     </item>
        ///     <item>
        ///         <see cref="Controller.AeroCool.AeroCoolGroup" />
        ///     </item>
        ///     <item>
        ///         <see cref="Controller.Nzxt.NzxtGroup" />
        ///     </item>
        /// </list>
        /// devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsControllerEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Cpu" /> devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsCpuEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.GpuAmd" /> or <see cref="HardwareType.GpuNvidia" /> devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsGpuEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Memory" /> devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsMemoryEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Motherboard" /> devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsMotherboardEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Network" /> devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsNetworkEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Psu" /> devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsPsuEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collecting information about <see cref="HardwareType.Storage" /> devices should be enabled and updated.
        /// </summary>
        /// <returns><see langword="true" /> if a given category of devices is already enabled.</returns>
        bool IsStorageEnabled { get; set; }

        /// <summary>
        /// Refreshes continuously the information stored in <see cref="Sensors"/> array
        /// </summary>
        void Monitor();

#if DEBUG
        void ThrowError();
#endif
    }
}
