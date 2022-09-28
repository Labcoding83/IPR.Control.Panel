using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using IPR.Control.Panel.Common;
using IPR.Control.Panel.Services;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;

namespace IPR.Control.Panel.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IComputerService _computerService;
        private readonly INotificationService _notificationService;
        private readonly IContextService _contextService;


        private string _rightViewWidth;
        public string RightViewWidth
        {
            get => _rightViewWidth;
            set => SetProperty(ref _rightViewWidth, value);
        }

        private bool _isRightViewVisible;
        public bool IsRightViewVisible
        {
            get => _isRightViewVisible;
            set => SetProperty(ref _isRightViewVisible, value);
        }

        public MainWindowViewModel(
            IComputerService computerService,
            INotificationService notificationService,
            IContextService contextService)
        {
            _notificationService = notificationService;
            _computerService = computerService;
            _contextService = contextService;
            
            _contextService.RegionVisibiltyChanged += _contextService_RegionVisibiltyChanged;
            _contextService.SelectedSensorChanged += _contextService_SelectedSensorChanged;
        }

        private void _contextService_SelectedSensorChanged(object? sender, EventArgs e)
        {
            IsRightViewVisible = sender != null;
            RightViewWidth = _contextService.Sensor != null ? "*" : "0";
        }

        private void _contextService_RegionVisibiltyChanged(object? sender, RegionVisibiltyChangedEventArgs e)
        {
            switch (e.RegionName)
            {
                case RegionNames.RightRegion:
                    IsRightViewVisible = e.IsVisible && _contextService.Sensor != null;
                    RightViewWidth = e.IsVisible && _contextService.Sensor != null ? "*" : "0";
                    break;
            }

        }

        public string Greeting => "Welcome to Avalonia!";

        public ObservableCollection<MenuItem> MenuItems { get; } = new ObservableCollection<MenuItem>();

        /// <summary>Shutdown classic desktop application.</summary>
        public DelegateCommand<Window> CmdMenuExitAppLifetime => new DelegateCommand<Window>((win) =>
        {
            // NOTE:
            //  We configured the app as a Classic Desktop Lifetime
            //    `public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);`
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                _computerService.Close();
                desktopLifetime.Shutdown();
            }
            else
            {
                System.Console.WriteLine("Shut down application issue..");
                System.Diagnostics.Debugger.Break();
            }
        });


    }
}
