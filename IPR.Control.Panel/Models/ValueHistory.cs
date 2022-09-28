using System;

namespace IPR.Control.Panel.Models
{
    public class ValueHistory
    {
        public double Time { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0:#0.0} {1:##0.0}", Time, Value);
        }
    }
}
