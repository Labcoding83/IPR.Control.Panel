// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Security;
using System.Text;
using System.Threading;
using IPR.Hardware.Tools.Hardware.Motherboard;
using Microsoft.Win32;

namespace IPR.Hardware.Tools.Hardware.Controller.Dell
{
    internal class DellGroup : IGroup
    {
        private readonly List<Dell> _hardware = new();
        private readonly StringBuilder _report = new();

        public DellGroup(SMBios sMBios)
        {
            if (!sMBios.System.ManufacturerName.StartsWith(Manufacturer.Dell.ToString()))
                return;

            if (Software.OperatingSystem.IsUnix)
                return;

            _hardware.Add(new Dell(sMBios.Board.ProductName));
        }

        public IReadOnlyList<IHardware> Hardware => _hardware;

        IEnumerable<IHardware> IGroup.Hardware => _hardware;

        public string GetReport()
        {
            if (_report.Length > 0)
            {
                StringBuilder r = new();
                r.AppendLine("Serial Port Heatmaster");
                r.AppendLine();
                r.Append(_report);
                r.AppendLine();
                return r.ToString();
            }

            return null;
        }

        public void Close()
        {
 
        }

      

    }
}
