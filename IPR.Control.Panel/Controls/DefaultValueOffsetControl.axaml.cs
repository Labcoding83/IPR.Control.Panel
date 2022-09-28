using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using IPR.Control.Panel.ViewModels;
using System;

namespace IPR.Control.Panel.Controls
{
    public partial class DefaultValueOffsetControl : UserControl
    {
        private TextBlock tbDefaultValue;
        private Models.Sensor _sensor;
        
        public DefaultValueOffsetControl()
        {
            InitializeComponent();
            tbDefaultValue = this.Find<TextBlock>(nameof(tbDefaultValue));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            if (DataContext is Models.Control)
                InitDefaultValue();
        }

        private void InitDefaultValue()
        {
            _sensor = (DataContext as Models.Control).Sensor;
            tbDefaultValue.Text = _sensor.DefaultValue.ToString();
        }

        private void TbDefaultValue_Tapped(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_sensor == null)
                return;

            var controlViewModel = ((Avalonia.StyledElement)((Avalonia.Controls.Control)((Avalonia.StyledElement)((Avalonia.Controls.Control)((Avalonia.StyledElement)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.StyledElement)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)sender).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).DataContext as ControlsViewModel;
            controlViewModel.SetDefaultValue(DataContext as Models.Control, _sensor.DefaultValue);
        }
    }
}
