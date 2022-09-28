using System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreHardwareMonitor.Hardware
{
    public class CallingInterface : InformationBase
    {
        public ushort CommandIoAddress { get; private set; }

        public byte CommandIoCode { get; private set; }

        public uint SupportedCommands { get; private set; }

        public List<SmbiosToken> TokenList { get; private set; }

        private Dictionary<uint, SmbiosToken> TokenDictionary;

        public CallingInterface(byte[] data, IList<string> strings) : base(data, strings)
        {
            TokenList = new List<SmbiosToken>();
            TokenDictionary = new Dictionary<uint, SmbiosToken>();
            Parse(data);
        }

        public void Parse(byte[] smbiosDataBuffer)
        {
            CommandIoAddress = GetWord(0x04);
            CommandIoCode = GetByte(0x06);
            SupportedCommands = GetDword(0x07);
            ParseTokens(smbiosDataBuffer, 0x0b);
        }

        private void ParseTokens(byte[] smbiosData, int startingOffset)
        {
            int num = startingOffset;
            bool flag = false;
            while (num + 6 < smbiosData.Length && !flag)
            {
                var id = BitConverter.ToUInt16(_data, num); 
                if (id == ushort.MaxValue)
                {
                    flag = true;
                    continue;
                }

                SmbiosToken smbiosToken = new SmbiosToken();
                smbiosToken.ID = id;
                smbiosToken.Location = BitConverter.ToUInt16(_data, num + 2);
                smbiosToken.Value = BitConverter.ToUInt16(_data, num + 4);
                TokenList.Add(smbiosToken);
                num += 6;
            }
        }
    }

    [DebuggerDisplay("{ToString()}")]
    public class SmbiosToken
    {
        public uint ID { get; set; }

        public uint Location { get; set; }

        public uint Value { get; set; }

        public override string ToString()
        {
            return "Token ID: 0x" + ID.ToString("X4") + ", Token Location: 0x" + Location.ToString("X4") + ", Token Value: 0x" + Value.ToString("X4");
        }
    }
}
