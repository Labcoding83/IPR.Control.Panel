using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IPR.Control.Panel.Views
{
    public partial class SensorsView : UserControl
    {
        public SensorsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
