using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IPR.Control.Panel.Views
{
    public partial class ControlsView : UserControl
    {
        public ControlsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
