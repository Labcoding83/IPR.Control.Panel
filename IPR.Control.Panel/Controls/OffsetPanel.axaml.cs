using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using IPR.Control.Panel.Models;
using IPR.Control.Panel.ViewModels;
using System;

namespace IPR.Control.Panel.Controls
{
    public partial class OffsetPanel : UserControl
    {
        private TabControl _offsetTabs;
        public OffsetPanel()
        {
            InitializeComponent();
            _offsetTabs = this.Find<TabControl>("offsetTabs");
        }

        private void ControlTypeSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0 || e.AddedItems[0] == null)
            {
                return;
            }
            ControllerType type = (ControllerType)((Avalonia.Controls.Primitives.SelectingItemsControl)sender).SelectedIndex;
            (this.DataContext as Models.Control).ControllerType = type;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void LockUnlock_OnClick(object? sender, RoutedEventArgs e)
        {
            if (sender is not StyledElement
                || (sender as StyledElement).DataContext is not Models.Control)
                return;
            var control = ((sender as StyledElement).DataContext as Models.Control);
            var viewModel = ((Avalonia.StyledElement)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.Controls.Control)((Avalonia.StyledElement)sender).Parent).Parent).Parent).Parent).Parent).Parent).DataContext as ControlsViewModel;
            viewModel.ToggleControl(control);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            if (DataContext is Models.Control)
                InitControl();
        }

        private void InitControl()
        {
            var tabIndex = (int)(DataContext as Models.Control).ControllerType;
            if (_offsetTabs.SelectedIndex != tabIndex)
                _offsetTabs.SelectedIndex = tabIndex;
        }
    }
}
