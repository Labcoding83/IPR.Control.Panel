// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) LibreHardwareMonitor and Contributors.
// Partial Copyright (C) Michael Möller <mmoeller@openhardwaremonitor.org> and Contributors.
// All Rights Reserved.

using DellFanManagement.DellSmbiozBzhLib;
using IPR.Hardware.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace IPR.Hardware.Tools.Hardware.Controller.Dell
{
    internal sealed class Dell : Hardware
    {
        private readonly List<Sensor> _fanSensors = new();
        private readonly List<Control> _fanControls = new();
        private DellSmbiosBzh _dellSmb;

        public Dell(string boardName) : base(nameof(Dell), new Identifier(nameof(Dell), boardName))
        {
            try
            {
                _dellSmb = new();
                var initialized = _dellSmb.Initialize();
                if (!initialized)
                    return;

                foreach(var fan in Enum.GetValues(typeof(BzhFanIndex)).Cast<BzhFanIndex>().Select((x, i) => new { x, i}))
                {
                    var rpm = _dellSmb.GetFanRpm(fan.x);
                    if (rpm == null)
                        continue;
                    var sensorLevel = new Sensor($"Fan{fan.i} Level", fan.i, SensorType.FanLevel, this);
                    sensorLevel.AddValueParameter(Constants.FAN, Constants.FAN_DESC, 0);
                    sensorLevel.AddRangeParameter((int)BzhFanLevel.Level0, (int)BzhFanLevel.Level2);
                    _fanSensors.Add(sensorLevel);
                    ActivateSensor(sensorLevel);
                    var sensorSpeed = new Sensor($"Fan{fan.i} Speed", fan.i, SensorType.Fan, this);
                    sensorSpeed.AddValueParameter(Constants.FAN, Constants.FAN_DESC, 0);
                    _fanSensors.Add(sensorSpeed);
                    ActivateSensor(sensorSpeed);


                    var control = new Control($"Fan{fan.i} FanLevel", fan.i, this, ControlType.FanLevel, (int)BzhFanLevel.Level0, (int)BzhFanLevel.Level2
                        , (index, value) =>
                        {
                            bool success = _dellSmb.DisableAutomaticFanControl(true);
                            _dellSmb.SetFanLevel((BzhFanIndex)index, (BzhFanLevel)value);
                            return value;
                        },
                         (index, value) =>
                         {
                             _dellSmb.SetFanLevel((BzhFanIndex)index, (BzhFanLevel)value);
                             _dellSmb.EnableAutomaticFanControl(true);
                         });
                    ActivateControl(control);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public override HardwareType HardwareType
        {
            get { return HardwareType.Cooler; }
        }

        public override string GetReport()
        {
            StringBuilder r = new();
            r.AppendLine("Dell");
            r.AppendLine();

            r.AppendLine();
            return r.ToString();
        }

        public override void Close()
        {
            base.Close();
            _dellSmb.DisableAutomaticFanControl(true);
            _dellSmb.Shutdown();
        }

        public override void Update()
        {
            try
            {
                foreach (var fan in _fanSensors)
                {
                    var rpm = _dellSmb.GetFanLevel((BzhFanIndex)fan.Index);

                    fan.Value = rpm.Value;
                }
            }
            catch
            {

            }
        }

        public DellFanInfo GetFanInfo()
        {
            var info = new DellFanInfo();
            try
            {
                var initialized = _dellSmb.IsInitialized || _dellSmb.Initialize();
                info.CanInit = initialized;
                if (!initialized)
                    throw new Exception("Failed to init.");

                var levels = Enum.GetValues(typeof(BzhFanLevel)).Cast<BzhFanLevel>();
                var fans = Enum.GetValues(typeof(BzhFanIndex)).Cast<BzhFanIndex>();
                info.FanLevels = levels.Count();

                Calibrate(info, _dellSmb, fans, levels);
                if (!info.FanDetails.Any())
                {
                    info.CanControlFan = false;
                    Calibrate(info, _dellSmb, fans, levels, true);
                    if(!info.FanDetails.Any())
                    {
                        info.CanControlFanAlternate = false;
                    }
                    else
                    {
                        info.CanControlFanAlternate = true;
                    }
                }
                else
                {
                    info.CanControlFan = true;
                }

                if (!info.CanControlFan && !info.CanControlFanAlternate)
                    throw new Exception("Unable to control fans, shuting down service");

            }
            catch(Exception ex)
            {
                info.ErrorMessage = ex.Message;
                _dellSmb.Shutdown();
            }
            return info;
        }

        private static void Calibrate(
            DellFanInfo info, 
            DellSmbiosBzh smb, 
            IEnumerable<BzhFanIndex> fans,
            IEnumerable<BzhFanLevel> levels,
            bool alternateControl = false)
        {
            info.CanControlFan = smb.EnableAutomaticFanControl(alternateControl);
            info.CanControlFan = info.CanControlFan && smb.DisableAutomaticFanControl(alternateControl);

            if (info.CanControlFan)
            {
                smb.EnableAutomaticFanControl(alternateControl);
                foreach (var fan in fans)
                {
                    foreach (var level in levels)
                    {
                        var isSuccessful = smb.SetFanLevel(fan, level);
                        if (!isSuccessful)
                            continue;
                        var newLevel = smb.GetFanLevel(fan);
                        if (!newLevel.HasValue)
                            continue;
                        var rpm = smb.GetFanRpm(fan);
                        if (!rpm.HasValue)
                            continue;
                        info.FanDetails.Add(new Tuple<BzhFanIndex, BzhFanLevel, uint?>(fan, level, rpm));
                    }
                }
                smb.DisableAutomaticFanControl(alternateControl);
            }
        }
    }
}
