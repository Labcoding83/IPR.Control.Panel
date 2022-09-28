// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IPR.Hardware.Tools.Hardware.Motherboard.Lpc;
using IPR.Hardware.Tools.Hardware.Motherboard.Lpc.EC;
using OperatingSystem = IPR.Hardware.Tools.Software.OperatingSystem;

namespace IPR.Hardware.Tools.Hardware.Motherboard
{
    /// <summary>
    /// Represents the motherboard of a computer with its <see cref="LpcIO"/> and <see cref="EmbeddedController"/> as <see cref="SubHardware"/>.
    /// </summary>
    public class Motherboard : IHardware
    {
        private readonly LMSensors _lmSensors;
        private readonly LpcIO _lpcIO;
        private readonly string _name;
        private string _customName;

        /// <summary>
        /// Creates motherboard instance by retrieving information from <see cref="LibreHardwareMonitor.Hardware.Motherboard.SMBios"/> and creates a new <see cref="SubHardware"/> based on data from <see cref="LpcIO"/> and <see cref="EmbeddedController"/>.
        /// </summary>
        /// <param name="smBios"><see cref="LibreHardwareMonitor.Hardware.Motherboard.SMBios"/> table containing motherboard data.</param>
        /// <param name="settings">Additional settings passed by <see cref="IComputer"/>.</param>
        public Motherboard(SMBios smBios)
        {
            IReadOnlyList<ISuperIO> superIO;
            SMBios = smBios;

            Manufacturer manufacturer = smBios.Board == null ? Manufacturer.Unknown : Identification.GetManufacturer(smBios.Board.ManufacturerName);
            Model model = smBios.Board == null ? Model.Unknown : Identification.GetModel(smBios.Board.ProductName);

            if (smBios.Board != null)
            {
                if (!string.IsNullOrEmpty(smBios.Board.ProductName))
                {
                    if (manufacturer == Manufacturer.Unknown)
                        _name = smBios.Board.ProductName;
                    else
                        _name = manufacturer + " " + smBios.Board.ProductName;
                }
                else
                {
                    _name = manufacturer.ToString();
                }
            }
            else
            {
                _name = nameof(Manufacturer.Unknown);
            }

            if (OperatingSystem.IsUnix)
            {
                _lmSensors = new LMSensors();
                superIO = _lmSensors.SuperIO;
            }
            else
            {
                _lpcIO = new LpcIO();
                superIO = _lpcIO.SuperIO;
            }

            EmbeddedController embeddedController = EmbeddedController.Create(model);

            SubHardware = new IHardware[superIO.Count + (embeddedController != null ? 1 : 0)];
            for (int i = 0; i < superIO.Count; i++)
                SubHardware[i] = new SuperIOHardware(this, superIO[i], manufacturer, model);

            if (embeddedController != null)
                SubHardware[superIO.Count] = embeddedController;
        }

        /// <returns><see cref="HardwareType.Motherboard"/></returns>
        public HardwareType HardwareType
        {
            get { return HardwareType.Motherboard; }
        }

        /// <inheritdoc/>
        public Identifier Identifier
        {
            get { return new Identifier("motherboard"); }
        }

        /// <summary>
        /// Gets the name obtained from <see cref="LibreHardwareMonitor.Hardware.Motherboard.SMBios"/>.
        /// </summary>
        public string Name
        {
            get { return _customName; }
            set
            {
                _customName = !string.IsNullOrEmpty(value) ? value : _name;
            }
        }

        /// <inheritdoc/>
        /// <returns>Always <see langword="null"/></returns>
        public virtual IHardware Parent
        {
            get { return null; }
        }

        /// <inheritdoc/>
        public virtual IDictionary<string, string> Properties => new SortedDictionary<string, string>();

        /// <inheritdoc/>
        public ISensor[] Sensors
        {
            get { return Array.Empty<ISensor>(); }
        }

        /// <inheritdoc/>
        public IControl[] Controls
        {
            get { return Array.Empty<IControl>(); }
        }

        /// <summary>
        /// Gets the <see cref="LibreHardwareMonitor.Hardware.Motherboard.SMBios"/> information.
        /// </summary>
        public SMBios SMBios { get; }

        /// <inheritdoc/>
        public IHardware[] SubHardware { get; }

        /// <inheritdoc/>
        public TimeSpan UpdateDelay => TimeSpan.FromSeconds(1);

        /// <inheritdoc/>
        public string GetReport()
        {
            StringBuilder r = new();

            r.AppendLine("Motherboard");
            r.AppendLine();
            r.Append(SMBios.GetReport());

            if (_lpcIO != null)
                r.Append(_lpcIO.GetReport());

            return r.ToString();
        }

        /// <summary>
        /// Motherboard itself cannot be updated. Update <see cref="SubHardware"/> instead.
        /// </summary>
        public void Update()
        {}

        /// <inheritdoc/>
        public void UpdatingContinuously(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                do
                {
                    Update();
                    await Task.Delay((int)UpdateDelay.TotalMilliseconds, cancellationToken);
                } while (!cancellationToken.IsCancellationRequested);
            }, cancellationToken);
        }

        /// <summary>
        /// Closes <see cref="SubHardware"/> using <see cref="Hardware.Close"/>.
        /// </summary>
        public void Close()
        {
            _lmSensors?.Close();
            foreach (IHardware iHardware in SubHardware)
            {
                if (iHardware is Hardware hardware)
                    hardware.Close();
            }
        }
    }
}
