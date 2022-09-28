// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IPR.Hardware.Tools.Hardware
{
    /// <summary>
    /// Object representing a component of the computer.
    /// <para>
    /// Individual information can be read from the <see cref="Sensors"/>.
    /// </para>
    /// </summary>
    public abstract class Hardware : IHardware
    {
        protected readonly HashSet<ISensor> _active = new();
        protected readonly HashSet<IControl> _controls = new();
        private string _name;

        /// <summary>
        /// Creates a new <see cref="Hardware"/> instance based on the data provided.
        /// </summary>
        /// <param name="name">Component name.</param>
        /// <param name="identifier">Identifier that will be assigned to the device. Based on <see cref="Identifier"/></param>
        /// <param name="settings">Additional settings passed by the <see cref="IComputer"/>.</param>
        protected Hardware(string name, Identifier identifier)
        {
            _name = name;
            Identifier = identifier;
        }

        /// <inheritdoc />
        public abstract HardwareType HardwareType { get; }

        /// <inheritdoc />
        public Identifier Identifier { get; }

        /// <inheritdoc />
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <inheritdoc />
        public virtual IHardware Parent
        {
            get { return null; }
        }

        /// <inheritdoc />
        public virtual IDictionary<string, string> Properties => new SortedDictionary<string, string>();

        /// <inheritdoc />
        public virtual ISensor[] Sensors
        {
            get { return _active.ToArray(); }
        } 
        
        /// <inheritdoc />
        public virtual IControl[] Controls
        {
            get { return _controls.ToArray(); }
        }

        /// <inheritdoc />
        public IHardware[] SubHardware
        {
            get { return Array.Empty<IHardware>(); }
        }

        /// <inheritdoc />
        public virtual TimeSpan UpdateDelay => TimeSpan.FromSeconds(10);

        /// <inheritdoc />
        public virtual string GetReport()
        {
            return null;
        }

        /// <inheritdoc />
        public abstract void Update();

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

        /// <inheritdoc />
        protected virtual void ActivateSensor(ISensor sensor)
        {
            _active.Add(sensor);
        }

        /// <inheritdoc />
        protected virtual void DeactivateSensor(ISensor sensor)
        {
            _active.Remove(sensor);
        }

        /// <inheritdoc />
        protected virtual void ActivateControl(IControl control)
        {
            _controls.Add(control);
        }

        /// <inheritdoc />
        protected virtual void DeactivateControl(IControl control)
        {
            _controls.Remove(control);
        }

        /// <inheritdoc />
        public virtual void Close()
        {
        }
    }
}
