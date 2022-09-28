using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using IPR.Control.Panel.ViewModels;
using ScottPlot.Avalonia;

namespace IPR.Control.Panel.Views
{
    public partial class SensorHistoryView : UserControl
    {
        public SensorHistoryView()
        {
            InitializeComponent();
            (DataContext as SensorHistoryViewModel)?.SetPlot(this.Find<AvaPlot>("AvaPlot1"));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
