using System.Collections.ObjectModel;

namespace IPR.Control.Panel.Models
{
    public class ControlType
    {
        public ControlType() { }
        public string Name { get; set; }
        public bool IsExpanded { get => true; }
        public ObservableCollection<Control> Controls { get; set; } = new();
    }
}
