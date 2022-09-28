using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using IPR.Control.Panel.ViewModels;
using IPR.Hardware.Tools;
using System;
using System.Linq;

namespace IPR.Control.Panel.Controls
{
    public partial class FixedOffsetSettingsControl : UserControl
    {
        private TextBlock _tbValue;
        private Slider _fixedOffsetSelector;
        private float _minValue;
        private float _maxValue;

        public FixedOffsetSettingsControl()
        {
            InitializeComponent();
            _tbValue = this.Find<TextBlock>("tbValue");
            _fixedOffsetSelector = this.Find<Slider>("fixedOffsetSelector");
            _fixedOffsetSelector.PropertyChanged += _fixedOffsetSelector_PropertyChanged;
        }

        private void _fixedOffsetSelector_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name != "Value")
                return;
            double newValue = (double)e.NewValue;
            var rounded = Math.Round(newValue, 2);
            _tbValue.Text = rounded.ToString();
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
            var model = this.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.Parent?.DataContext;
        }

        private void InitDefaultValue()
        {
            _fixedOffsetSelector.Minimum = (DataContext as Models.Control).MinValue;
            _fixedOffsetSelector.Maximum = (DataContext as Models.Control).MaxValue;

            _fixedOffsetSelector.Value = (DataContext as Models.Control).Value;
        }

        private void ApplyValue_Clicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var controlViewModel = ((Avalonia.StyledElement)((Avalonia.StyledElement)((Avalonia.StyledElement)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.StyledElement)((Avalonia.StyledElement)((Avalonia.StyledElement)((Avalonia.StyledElement)((Avalonia.StyledElement)((Avalonia.StyledElement)((Avalonia.StyledElement)((Avalonia.Controls.Control)sender).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).Parent).DataContext as ControlsViewModel;
            (DataContext as Models.Control).Value = (DataContext as Models.Control).ChangeValue(_fixedOffsetSelector.Value);
        }
    }
}
