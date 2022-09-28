// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using IPR.Drivers.Ring0;
using IPR.Hardware.Tools.Hardware.Battery;
using IPR.Hardware.Tools.Hardware.Controller.AeroCool;
using IPR.Hardware.Tools.Hardware.Controller.AquaComputer;
using IPR.Hardware.Tools.Hardware.Controller.Dell;
using IPR.Hardware.Tools.Hardware.Controller.Heatmaster;
using IPR.Hardware.Tools.Hardware.Controller.Nzxt;
using IPR.Hardware.Tools.Hardware.Controller.TBalancer;
using IPR.Hardware.Tools.Hardware.CPU;
using IPR.Hardware.Tools.Hardware.Gpu;
using IPR.Hardware.Tools.Hardware.Memory;
using IPR.Hardware.Tools.Hardware.Motherboard;
using IPR.Hardware.Tools.Hardware.Network;
using IPR.Hardware.Tools.Hardware.Psu.Corsair;
using IPR.Hardware.Tools.Hardware.Storage;

namespace IPR.Hardware.Tools.Hardware
{
    /// <summary>
    /// Stores all hardware groups and decides which devices should be enabled and updated.
    /// </summary>
    internal class Computer : IComputer
    {
        private readonly List<IGroup> _groups = new();
        private readonly object _lock = new();
        private CancellationTokenSource _cancellationTokenSource = new ();
        
        private bool _batteryEnabled;
        private bool _controllerEnabled;
        private bool _cpuEnabled;
        private bool _gpuEnabled;
        private bool _memoryEnabled;
        private bool _motherboardEnabled;
        private bool _networkEnabled;
        private bool _open;
        private bool _psuEnabled;
        private SMBios _smbios;
        private bool _storageEnabled;
        private string _name;
        /// <summary>
        /// Creates a new <see cref="IComputer" /> instance />.
        /// </summary>
        public Computer()
        {
            _name = Environment.MachineName;
        }

        public string Name => _name;
        /// <inheritdoc />
        public bool IsOpened { get { return _open; } }

        /// <inheritdoc />
        public IEnumerable<IHardware> Hardware
        {
            get
            {
                return _groups.SelectMany(h => h.Hardware.Union(h.Hardware.SelectMany(s => s.SubHardware)));
            }
        }

        /// <inheritdoc />
        public Identifier Identifier
        {
            get { return new Identifier(nameof(Computer)); }
        }

        /// <inheritdoc />
        public bool IsBatteryEnabled
        {
            get { return _batteryEnabled; }
            set
            {
                if (_open && value != _batteryEnabled)
                {
                    if (value)
                    {
                        Add(new BatteryGroup());
                    }
                    else
                    {
                        RemoveType<BatteryGroup>();
                    }
                }

                _batteryEnabled = value;
            }
        }

        /// <inheritdoc />
        public bool IsControllerEnabled
        {
            get { return _controllerEnabled; }
            set
            {
                if (_open && value != _controllerEnabled)
                {
                    if (value)
                    {
                        Add(new TBalancerGroup());
                        Add(new HeatmasterGroup());
                        Add(new AquaComputerGroup());
                        Add(new AeroCoolGroup());
                        Add(new NzxtGroup());
                        Add(new DellGroup(_smbios));
                    }
                    else
                    {
                        RemoveType<TBalancerGroup>();
                        RemoveType<HeatmasterGroup>();
                        RemoveType<AquaComputerGroup>();
                        RemoveType<AeroCoolGroup>();
                        RemoveType<NzxtGroup>();
                        RemoveType<DellGroup>();
                    }
                }

                _controllerEnabled = value;
            }
        }

        /// <inheritdoc />
        public bool IsCpuEnabled
        {
            get { return _cpuEnabled; }
            set
            {
                if (_open && value != _cpuEnabled)
                {
                    if (value)
                        Add(new CpuGroup());
                    else
                        RemoveType<CpuGroup>();
                }

                _cpuEnabled = value;
            }
        }

        /// <inheritdoc />
        public bool IsGpuEnabled
        {
            get { return _gpuEnabled; }
            set
            {
                if (_open && value != _gpuEnabled)
                {
                    if (value)
                    {
                        Add(new AmdGpuGroup());
                        Add(new NvidiaGroup());
                        Add(new IntelGpuGroup(GetIntelCpus()));
                    }
                    else
                    {
                        RemoveType<AmdGpuGroup>();
                        RemoveType<NvidiaGroup>();
                        RemoveType<IntelGpuGroup>();
                    }
                }

                _gpuEnabled = value;
            }
        }

        /// <inheritdoc />
        public bool IsMemoryEnabled
        {
            get { return _memoryEnabled; }
            set
            {
                if (_open && value != _memoryEnabled)
                {
                    if (value)
                        Add(new MemoryGroup());
                    else
                        RemoveType<MemoryGroup>();
                }

                _memoryEnabled = value;
            }
        }

        /// <inheritdoc />
        public bool IsMotherboardEnabled
        {
            get { return _motherboardEnabled; }
            set
            {
                if (_open && value != _motherboardEnabled)
                {
                    if (value)
                        Add(new MotherboardGroup(_smbios));
                    else
                        RemoveType<MotherboardGroup>();
                }

                _motherboardEnabled = value;
            }
        }

        /// <inheritdoc />
        public bool IsNetworkEnabled
        {
            get { return _networkEnabled; }
            set
            {
                if (_open && value != _networkEnabled)
                {
                    if (value)
                        Add(new NetworkGroup());
                    else
                        RemoveType<NetworkGroup>();
                }

                _networkEnabled = value;
            }
        }

        /// <inheritdoc />
        public bool IsPsuEnabled
        {
            get { return _psuEnabled; }
            set
            {
                if (_open && value != _psuEnabled)
                {
                    if (value)
                    {
                        Add(new CorsairPsuGroup());
                    }
                    else
                    {
                        RemoveType<CorsairPsuGroup>();
                    }
                }

                _psuEnabled = value;
            }
        }

        /// <inheritdoc />
        public bool IsStorageEnabled
        {
            get { return _storageEnabled; }
            set
            {
                if (_open && value != _storageEnabled)
                {
                    if (value)
                        Add(new StorageGroup());
                    else
                        RemoveType<StorageGroup>();
                }

                _storageEnabled = value;
            }
        }

        /// <summary>
        /// Contains computer information table read in accordance with <see href="https://www.dmtf.org/standards/smbios">System Management BIOS (SMBIOS) Reference Specification</see>.
        /// </summary>
        public SMBios SMBios
        {
            get
            {
                if (!_open)
                    throw new InvalidOperationException("SMBIOS cannot be accessed before opening.");

                return _smbios;
            }
        }

        //// Report
        ///
        public string GetReport()
        {
            lock (_lock)
            {
                using StringWriter w = new(CultureInfo.InvariantCulture);

                w.WriteLine();
                w.WriteLine(nameof(IPR.Hardware.Tools) + " Report");
                w.WriteLine();

                Version version = typeof(Computer).Assembly.GetName().Version;

                NewSection(w);
                w.Write("Version: ");
                w.WriteLine(version.ToString());
                w.WriteLine();

                NewSection(w);
                w.Write("Common Language Runtime: ");
                w.WriteLine(Environment.Version.ToString());
                w.Write("Operating System: ");
                w.WriteLine(Environment.OSVersion.ToString());
                w.Write("Process Type: ");
                w.WriteLine(IntPtr.Size == 4 ? "32-Bit" : "64-Bit");
                w.WriteLine();

                NewSection(w);
                w.WriteLine("Sensors");
                w.WriteLine();

                foreach (IGroup group in _groups)
                {
                    foreach (IHardware hardware in group.Hardware)
                        ReportHardwareSensorTree(hardware, w, string.Empty);
                }

                w.WriteLine();

                NewSection(w);
                w.WriteLine("Parameters");
                w.WriteLine();

                foreach (IGroup group in _groups)
                {
                    foreach (IHardware hardware in group.Hardware)
                        ReportHardwareParameterTree(hardware, w, string.Empty);
                }

                w.WriteLine();

                foreach (IGroup group in _groups)
                {
                    string report = group.GetReport();
                    if (!string.IsNullOrEmpty(report))
                    {
                        NewSection(w);
                        w.Write(report);
                    }

                    foreach (IHardware hardware in (IEnumerable<IHardware>)group.Hardware)
                        ReportHardware(hardware, w);
                }

                return w.ToString();
            }
        }

        private void Add(IGroup group)
        {
            if (group == null)
                return;

            lock (_lock)
            {
                if (_groups.Contains(group))
                    return;

                _groups.Add(group);
            }
        }

        private void Remove(IGroup group)
        {
            lock (_lock)
            {
                if (!_groups.Contains(group))
                    return;

                _groups.Remove(group);
                group.Close();
            }

            group.Close();
        }

        private void RemoveType<T>() where T : IGroup
        {
            List<T> list = new();

            lock (_lock)
            {
                foreach (IGroup group in _groups)
                {
                    if (group is T t)
                        list.Add(t);
                }
            }

            foreach (T group in list)
                Remove(group);
        }

        /// <summary>
        /// If hasn't been opened before, opens <see cref="SMBios" />, <see cref="Ring0.Instance" />, <see cref="OpCode" /> and triggers the private <see cref="AddGroups" /> method depending on which categories are
        /// enabled.
        /// </summary>
        public void Open()
        {
            if (_open)
                return;

            _smbios = new SMBios();
            var ring0 = Ring0.Instance;
            ring0.Open();
            OpCode.Open();

            AddGroups();

            _open = true;
        }

        private void AddGroups()
        {
            if (_motherboardEnabled)
                Add(new MotherboardGroup(_smbios));

            if (_cpuEnabled)
                Add(new CpuGroup());

            if (_memoryEnabled)
                Add(new MemoryGroup());

            if (_gpuEnabled)
            {
                Add(new AmdGpuGroup());
                Add(new NvidiaGroup());
                Add(new IntelGpuGroup(GetIntelCpus()));
            }

            if (_controllerEnabled)
            {
                Add(new TBalancerGroup());
                Add(new HeatmasterGroup());
                Add(new AquaComputerGroup());
                Add(new AeroCoolGroup());
                Add(new NzxtGroup());
                Add(new DellGroup(_smbios));
            }

            if (_storageEnabled)
                Add(new StorageGroup());

            if (_networkEnabled)
                Add(new NetworkGroup());

            if (_psuEnabled)
                Add(new CorsairPsuGroup());

            if (_batteryEnabled)
                Add(new BatteryGroup());
        }

        private static void NewSection(TextWriter writer)
        {
            for (int i = 0; i < 8; i++)
                writer.Write("----------");

            writer.WriteLine();
            writer.WriteLine();
        }

        private static int CompareSensor(ISensor a, ISensor b)
        {
            int c = a.SensorType.CompareTo(b.SensorType);
            if (c == 0)
                return a.Index.CompareTo(b.Index);

            return c;
        }

        private static void ReportHardwareSensorTree(IHardware hardware, TextWriter w, string space)
        {
            w.WriteLine("{0}|", space);
            w.WriteLine("{0}+- {1} ({2})", space, hardware.Name, hardware.Identifier);

            ISensor[] sensors = hardware.Sensors;
            Array.Sort(sensors, CompareSensor);

            foreach (ISensor sensor in sensors)
                w.WriteLine("{0}|  +- {1,-14} : {2,8:G6} ({3})", space, sensor.Name, sensor.Value, sensor.Identifier);

            foreach (IHardware subHardware in hardware.SubHardware)
                ReportHardwareSensorTree(subHardware, w, "|  ");
        }

        private static void ReportHardwareParameterTree(IHardware hardware, TextWriter w, string space)
        {
            w.WriteLine("{0}|", space);
            w.WriteLine("{0}+- {1} ({2})", space, hardware.Name, hardware.Identifier);

            ISensor[] sensors = hardware.Sensors;
            Array.Sort(sensors, CompareSensor);

            foreach (ISensor sensor in sensors)
            {
                string innerSpace = space + "|  ";
                if (sensor.Parameters.Count > 0)
                {
                    w.WriteLine("{0}|", innerSpace);
                    w.WriteLine("{0}+- {1} ({2})", innerSpace, sensor.Name, sensor.Identifier);

                    foreach (IParameter parameter in sensor.Parameters)
                    {
                        string innerInnerSpace = innerSpace + "|  ";
                        if (parameter.ParameterType == ParameterType.Value)
                            w.WriteLine("{0}+- {1} : {2}", innerInnerSpace, parameter.Name, parameter.Value);
                        else
                            w.WriteLine("{0}+- {1} : {2} - {3}", innerInnerSpace, parameter.Name, parameter.MinValue, parameter.MaxValue);
                    }
                }
            }

            foreach (IHardware subHardware in hardware.SubHardware)
                ReportHardwareParameterTree(subHardware, w, "|  ");
        }

        private static void ReportHardware(IHardware hardware, TextWriter w)
        {
            string hardwareReport = hardware.GetReport();
            if (!string.IsNullOrEmpty(hardwareReport))
            {
                NewSection(w);
                w.Write(hardwareReport);
            }

            foreach (IHardware subHardware in hardware.SubHardware)
                ReportHardware(subHardware, w);
        }

        /// <inheritdoc/>
        public void Monitor()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource = new CancellationTokenSource();
            foreach (var hardware in Hardware)
                hardware.UpdatingContinuously(_cancellationTokenSource.Token);
        }

        /// <summary>
        /// If opened before, removes all <see cref="IGroup" /> and triggers <see cref="OpCode.Close" />, <see cref="InpOut.Close" /> and <see cref="Close" />.
        /// </summary>
        public void Close()
        {
            if (!_open)
                return;

            _cancellationTokenSource.Cancel();

            lock (_lock)
            {
                while (_groups.Count > 0)
                {
                    IGroup group = _groups[_groups.Count - 1];
                    Remove(group);
                }
            }

            OpCode.Close();
            InpOut.Close();
            Ring0.Instance.Close();

            _smbios = null;
            _open = false;
        }

        /// <summary>
        /// If opened before, removes all <see cref="IGroup" /> and recreates it.
        /// </summary>
        public void Reset()
        {
            if (!_open)
                return;

            RemoveGroups();
            AddGroups();
        }

        private void RemoveGroups()
        {
            lock (_lock)
            {
                while (_groups.Count > 0)
                {
                    IGroup group = _groups[_groups.Count - 1];
                    Remove(group);
                }
            }
        }

        private List<IntelCpu> GetIntelCpus()
        {
            // Create a temporary cpu group if one has not been added.
            lock (_lock)
            {
                IGroup cpuGroup = _groups.Find(x => x is CpuGroup) ?? new CpuGroup();
                return cpuGroup.Hardware.Select(x => x as IntelCpu).ToList();
            }
        }

#if DEBUG
        public void ThrowError()
        {
            throw new Exception("Test");
        }
#endif
    }
}
