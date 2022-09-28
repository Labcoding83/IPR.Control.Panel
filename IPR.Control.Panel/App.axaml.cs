using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using DryIoc;
using IPR.Control.Panel.Common;
using IPR.Control.Panel.Common.RegionAdapters;
using IPR.Control.Panel.Services;
using IPR.Control.Panel.ViewModels;
using IPR.Control.Panel.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Serilog;
using Serilog.Sinks.ListOfString;
using System;
using System.Collections.Generic;

namespace IPR.Control.Panel
{
    public partial class App : PrismApplication
    {
        private static List<String> _logs = new();
        public static List<String> Logs { get { return _logs; } }
        private Serilog.ILogger _logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .WriteTo.File(path: "logs.log")
              .WriteTo.StringList(_logs)
              .CreateLogger();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Error("Unhandled exception", e.ExceptionObject);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
            regionAdapterMappings.RegisterMapping(typeof(Grid), Container.Resolve<GridRegionAdapter>());
        }

        protected override IAvaloniaObject CreateShell()
        {
            var contextService = Container.Resolve<IContextService>();
            contextService.Init();

            var mainWindow = this.Container.Resolve<MainWindow>();
            
            var notifyService = Prism.Ioc.ContainerLocator.Current.Resolve<INotificationService>();
            notifyService.SetHostWindow(mainWindow);

            return mainWindow;
        }

        protected override void OnInitialized()
        {
            // Register Views to Region it will appear in. Don't register them in the ViewModel.
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.LeftRegion, typeof(MenuView));
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ControlsView));
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(SensorsView));
            regionManager.RegisterViewWithRegion(RegionNames.RightRegion, typeof(SensorHistoryView));
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ReportView));
            //regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(SettingsView));
#if DEBUG
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DebugView));
#endif

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();

            // Services
            containerRegistry.RegisterInstance<ILogger>(_logger);
            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();
            containerRegistry.RegisterSingleton<IComputerService, ComputerService>();
            containerRegistry.RegisterSingleton<IContextService, ContextService>();
            containerRegistry.RegisterSingleton<IControlService, ControlService>();
            containerRegistry.RegisterSingleton<ISensorService, SensorService>();

            // Views - Generic
            containerRegistry.Register<MainWindow>();
            containerRegistry.Register<StackPanelRegionAdapter>();
            containerRegistry.Register<GridRegionAdapter>();

            // Views - Region Navigation
            containerRegistry.RegisterForNavigation<DebugView, DebugViewModel>();
            containerRegistry.RegisterForNavigation<ReportView, ReportViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<SensorsView, SensorsViewModel>();
            containerRegistry.RegisterForNavigation<SensorsViewModel, SensorHistoryViewModel>();
            containerRegistry.RegisterForNavigation<ControlsView, ControlsViewModel>();
            containerRegistry.RegisterDialog<GraphDialogView, GraphDialogViewViewModel>();
        }


    }
}
