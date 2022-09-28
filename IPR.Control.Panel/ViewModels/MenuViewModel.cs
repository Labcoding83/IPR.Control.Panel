using IPR.Control.Panel.Common;
using IPR.Control.Panel.Services;
using IPR.Control.Panel.Views;
using Prism.Commands;
using Prism.Regions;
using System;

namespace IPR.Control.Panel.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IContextService _contextService;
        public bool IsDebug
        {
#if DEBUG
            get => true;
#else
            get => false;
#endif
        }

        public MenuViewModel(
            IRegionManager regionManager,
            IContextService contextService
            )
        {
            _regionManager = regionManager;
            _contextService = contextService;
        }

        public DelegateCommand CmdViewControls => new DelegateCommand(() =>
        {
            HideRightSideView();
            _regionManager.RequestNavigate(
                RegionNames.ContentRegion, 
                new Uri(nameof(ControlsView), UriKind.Relative));
        });

        public DelegateCommand CmdViewReport => new DelegateCommand(() =>
        {
            HideRightSideView();
            _regionManager.RequestNavigate(
                RegionNames.ContentRegion, 
                new Uri(nameof(ReportView), UriKind.Relative));
        });

        public DelegateCommand CmdViewSensors => new DelegateCommand(() =>
        {
            DisplayRightSideView(typeof(SensorHistoryView));
            _regionManager.RequestNavigate(
               RegionNames.ContentRegion,
               new Uri(nameof(SensorsView), UriKind.Relative));
        });

        public DelegateCommand CmdViewSettings => new DelegateCommand(() =>
        {
            HideRightSideView();
            _regionManager.RequestNavigate(
                RegionNames.ContentRegion, 
                new Uri(nameof(SettingsView), UriKind.Relative));
        });

        public DelegateCommand CmdViewDebug => new DelegateCommand(() =>
        {
            HideRightSideView();
            _regionManager.RequestNavigate(
                RegionNames.ContentRegion, 
                new Uri(nameof(DebugView), UriKind.Relative));
        });

        private void HideRightSideView()
        {
            var region = _regionManager.Regions[RegionNames.RightRegion];
            foreach (Avalonia.Layout.Layoutable view in region.Views)
                view.IsVisible = false;
            _contextService.OnRegionVisibiltyChanged(RegionNames.RightRegion, false);
        }

        private void DisplayRightSideView(Type viewModelType)
        {
            var region = _regionManager.Regions[RegionNames.RightRegion];
            foreach (Avalonia.Layout.Layoutable view in region.Views)
                view.IsVisible = true;
            _contextService.OnRegionVisibiltyChanged(RegionNames.RightRegion, true);
        }
    }
}
