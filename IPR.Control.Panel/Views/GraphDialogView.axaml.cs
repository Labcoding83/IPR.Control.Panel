using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using IPR.Control.Panel.ViewModels;
using ScottPlot.Avalonia;

namespace IPR.Control.Panel.Views
{
    public partial class GraphDialogView : UserControl
    {
        public GraphDialogView()
        {
            InitializeComponent();
            (DataContext as GraphDialogViewViewModel)?.SetPlot(this.Find<AvaPlot>("plot"));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
