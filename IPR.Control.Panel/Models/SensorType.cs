using System.Collections.ObjectModel;

namespace IPR.Control.Panel.Models
{
    public class SensorType
    {
        public string Name { get; set; }
        public bool IsExpanded { get => true; }
        public ObservableCollection<Sensor> Sensors { get; set; } = new();
    }
}
