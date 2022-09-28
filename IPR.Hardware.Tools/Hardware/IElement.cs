// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

namespace IPR.Hardware.Tools.Hardware
{
    /// <summary>
    /// Abstract parent with logic for the abstract class that stores data.
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Gets or sets device name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets a unique hardware ID that represents its location.
        /// </summary>
        Identifier Identifier { get; }
    }
}
