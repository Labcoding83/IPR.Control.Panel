using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IPR.Control.Panel.Views
{
    public partial class DebugView : UserControl
    {
        public DebugView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
