namespace IPR.Drivers.Ring0
{
    public class Ring0
    {
        public const uint INVALID_PCI_ADDRESS = 0xFFFFFFFF;

        private static readonly Lazy<IRing0> _Ring0 = new Lazy<IRing0>(() =>
        {
            if (Software.OperatingSystem.IsUnix)
                throw new NotImplementedException();
            return new WinRing0();
        }, LazyThreadSafetyMode.ExecutionAndPublication);
        public static IRing0 Instance { get { return _Ring0.Value; } }
    }
}
