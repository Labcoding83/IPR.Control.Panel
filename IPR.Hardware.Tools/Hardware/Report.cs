using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.Hardware.Tools.Hardware
{
    internal static class Report
    {
        public static string GetReport(Computer computer)
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

            foreach (IGroup group in computer.Groups)
            {
                foreach (IHardware hardware in group.Hardware)
                    ReportHardwareSensorTree(hardware, w, string.Empty);
            }

            w.WriteLine();

            NewSection(w);
            w.WriteLine("Sensors Parameters");
            w.WriteLine();

            foreach (IGroup group in computer.Groups)
            {
                foreach (IHardware hardware in group.Hardware)
                    ReportHardwareSensorParameterTree(hardware, w, string.Empty);
            }

            NewSection(w);
            w.WriteLine("Controls");
            w.WriteLine();

            foreach (IGroup group in computer.Groups)
            {
                foreach (IHardware hardware in group.Hardware)
                    ReportHardwareControlTree(hardware, w, string.Empty);
            }

            w.WriteLine();

            foreach (IGroup group in computer.Groups)
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

        private static void ReportHardwareSensorTree(IHardware hardware, TextWriter w, string space)
        {
            w.WriteLine("{0}|", space);
            w.WriteLine("{0}+- {1} ({2})", space, hardware.Name, hardware.Identifier);

            ISensor[] sensors = hardware.Sensors
                .OrderBy(x => x.SensorType)
                .ThenBy(x => x.Index)
                .ToArray();

            foreach (ISensor sensor in sensors)
                w.WriteLine("{0}|  +- {1,-14} : {2,8:G6} ({3})", space, sensor.Name, sensor.Value, sensor.Identifier);

            foreach (IHardware subHardware in hardware.SubHardware)
                ReportHardwareSensorTree(subHardware, w, "|  ");
        }

        private static void ReportHardwareControlTree(IHardware hardware, TextWriter w, string space)
        {
            w.WriteLine("{0}|", space);
            w.WriteLine("{0}+- {1} ({2})", space, hardware.Name, hardware.Identifier);

            IControl[] sensors = hardware.Controls
                .OrderBy(x => x.ControlType)
                .ThenBy(x => x.Index)
                .ToArray();

            foreach (IControl sensor in sensors)
                w.WriteLine("{0}|  +- {1,-14} : {2,8:G6} ({3})", space, sensor.Name, sensor.Value, sensor.Identifier);

            foreach (IHardware subHardware in hardware.SubHardware)
                ReportHardwareControlTree(subHardware, w, "|  ");
        }

        private static void ReportHardwareSensorParameterTree(IHardware hardware, TextWriter w, string space)
        {
            w.WriteLine("{0}|", space);
            w.WriteLine("{0}+- {1} ({2})", space, hardware.Name, hardware.Identifier);

            ISensor[] sensors = hardware.Sensors
                .OrderBy(x => x.SensorType)
                .ThenBy(x => x.Index)
                .ToArray();

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
                ReportHardwareSensorParameterTree(subHardware, w, "|  ");
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

        private static void NewSection(TextWriter writer)
        {
            for (int i = 0; i < 8; i++)
                writer.Write("----------");

            writer.WriteLine();
            writer.WriteLine();
        }
    }
}
