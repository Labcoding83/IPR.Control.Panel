using IPR.Drivers.Ring0;
using System;

namespace IPR.Hardware.Tools.Hardware.CPU
{
    internal partial class IntelCpu
    {
        private Control[] _cpuVoltageOffsetControls;

        partial void InitControls()
        {
            if (!Ring0.Instance.IsOpen)
                return;
            
            _cpuVoltageOffsetControls = new Control[offsetLabels.Length];
            for (int i = 0; i < offsetLabels.Length; i++)
            {
                _cpuVoltageOffsetControls[i] = new Control(offsetLabels[i], i, this, ControlType.VoltageOffset, -150, 150, (index, value) =>
                {
                    return SetOffset(index, value);
                },
                (i, v) => SetOffset(i, v));
                ActivateControl(_cpuVoltageOffsetControls[i]);
            }
        }

        private static double SetOffset(int index, double value)
        {
            uint edx = (uint)(0x80000011 | (index << 8));
            uint eax = (uint)(0xFFE00000 & (((int)Math.Round(value * 1.024) & 0xFFF) << 21));
            var result = Ring0.Instance.WriteMsr(MSR_IA32_OC_MAILBOX, eax, edx);
            edx = (uint)(0x80000010 | (index << 8));
            eax = 0x00000000;
            Ring0.Instance.WriteMsr(MSR_IA32_OC_MAILBOX, eax, edx);
            result = Ring0.Instance.ReadMsr(MSR_IA32_OC_MAILBOX, out eax, out edx);
            var offset = eax >> 21;
            var newVoltage = (float)((offset <= 1024 ? offset : -(2048 - offset)) / 1.024);
            return Math.Round(newVoltage, 2);
        }
    }
}
