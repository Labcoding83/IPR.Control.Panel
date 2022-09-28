// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) IPR.Drivers and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

namespace IPR.Drivers.Software
{
    /// <summary>
    /// Contains basic information about the operating system.
    /// </summary>
    public static class OperatingSystem
    {
        /// <summary>
        /// Statically checks if the current system <see cref="IsUnix"/>.
        /// </summary>
        static OperatingSystem()
        {
            // The operating system doesn't change during execution so let's query it just one time.
            PlatformID platform = Environment.OSVersion.Platform;
            IsUnix = platform is PlatformID.Unix or PlatformID.MacOSX;
        }

        /// <summary>
        /// Gets information about whether the current system is Unix based.
        /// </summary>
        public static bool IsUnix { get; }
    }
}
