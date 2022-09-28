using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace IPR.Control.Panel.Views
{
  public partial class ReportView : UserControl
  {
    public ReportView()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
