using IPR.Drivers.Hardware;

namespace IPR.Drivers.Ring0
{
    internal class LinuxRing0 : IRing0
    {
        public bool IsOpen { get; private set; }

        public LinuxRing0()
        {
            Open();
        }

        public bool Open()
        {
            IsOpen = CheckVersion();
            return IsOpen;
        }

        private bool CheckVersion()
        {
            try
            {
                string rdResult = "";
                using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
                {
                    proc.StartInfo.FileName = "rdmsr";
                    proc.StartInfo.Arguments = "--version ";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.Start();

                    rdResult += proc.StandardOutput.ReadToEnd();
                    rdResult += proc.StandardError.ReadToEnd();

                    proc.WaitForExit();
                }

                string wrResult = "";
                using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
                {
                    proc.StartInfo.FileName = "wrmsr";
                    proc.StartInfo.Arguments = "--version ";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.Start();

                    wrResult += proc.StandardOutput.ReadToEnd();
                    wrResult += proc.StandardError.ReadToEnd();

                    proc.WaitForExit();
                }

                return !string.IsNullOrEmpty(rdResult) && !string.IsNullOrEmpty(wrResult)
                                                       && rdResult.Contains("version msr-tools") &&
                                                       wrResult.Contains("version msr-tools");
            }
            catch (Exception e)
            {
                return false;
            }
        }

      

        public bool ReadMsr(uint index, out uint eax, out uint edx)
        {
            eax = 0;
            edx = 0;
            if (!IsOpen)
            {
                return false;
            }

            string rawresuld = "";
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                
                proc.StartInfo.FileName = "rdmsr";
                proc.StartInfo.Arguments = "0x" + index.ToString("X");
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                rawresuld += proc.StandardOutput.ReadToEnd().Replace(Environment.NewLine, "");
                rawresuld += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }

            try
            {
                ulong result = Convert.ToUInt64(rawresuld, 16);
                edx = (uint)((result >> 32) & 0xFFFFFFFF);
                eax = (uint)(result & 0xFFFFFFFF);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ReadMsr(uint index, out uint eax, out uint edx, GroupAffinity affinity)
        {
            GroupAffinity previousAffinity = ThreadAffinity.Set(affinity);
            bool result = ReadMsr(index, out eax, out edx);
            ThreadAffinity.Set(previousAffinity);
            return result;
        }

        public bool WriteMsr(uint index, uint eax, uint edx)
        {
            if (!IsOpen)
            {
                return false;
            }

            try
            {
                string rawresuld = "";
                using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
                {
                    proc.StartInfo.FileName = "wrmsr";
                    proc.StartInfo.Arguments = "0x" + index.ToString("X") + " " + "0x" + edx.ToString("x8") +
                                               eax.ToString("x8");
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.Start();
                    

                    rawresuld += proc.StandardOutput.ReadToEnd().Replace(Environment.NewLine, "");
                    rawresuld += proc.StandardError.ReadToEnd();

                    proc.WaitForExit();
                }

                return string.IsNullOrEmpty(rawresuld);
            }
            catch
            {
                return false;
            }
        }

        public byte ReadIoPort(uint port)
        {
            throw new NotImplementedException();
        }

        public void WriteIoPort(uint port, byte value)
        {
            throw new NotImplementedException();
        }

        public uint GetPciAddress(byte bus, byte device, byte function)
        {
            throw new NotImplementedException();
        }

        public bool ReadPciConfig(uint pciAddress, uint regAddress, out uint value)
        {
            throw new NotImplementedException();
        }

        public bool WritePciConfig(uint pciAddress, uint regAddress, uint value)
        {
            throw new NotImplementedException();
        }

        public bool WaitPciBusMutex(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public void ReleasePciBusMutex()
        {
            throw new NotImplementedException();
        }

        public bool WaitEcMutex(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public void ReleaseEcMutex()
        {
            throw new NotImplementedException();
        }

        public bool WaitIsaBusMutex(int millisecondsTimeout)
        {
            throw new NotImplementedException();
        }

        public void ReleaseIsaBusMutex()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
