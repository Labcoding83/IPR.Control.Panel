using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace IPR.Control.Panel.Models
{
    public class HardWare
    {
        public HardWare() { }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsExpanded { get => true; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public ObservableCollection<SensorType> SensorTypes { get; set; } = new();
        public ObservableCollection<ControlType> ControlTypes { get; set; } = new();
    }
}
