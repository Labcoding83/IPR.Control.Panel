using HidSharp.Reports;
using IPR.Control.Panel.Common;
using IPR.Control.Panel.Services;
using Prism.Commands;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace IPR.Control.Panel.ViewModels
{
    public class DebugViewModel : ViewModelBase
    {
        private readonly IComputerService _computerService;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        private string _log = string.Empty;
        public string Log
        {
            get => _log;
            set => SetProperty(ref _log, value);
        }
        public DebugViewModel(
            IComputerService computerService,
            INotificationService notificationService,
            ILogger logger)
        {
            _logger = logger;
            _computerService = computerService;
            _notificationService = notificationService;
            Title = "Debug view";
            RefreshLog();
        }

        private void RefreshLog()
        {
            if (App.Logs.Any())
                Log = App.Logs.Aggregate((a, b) => a + Environment.NewLine + b);
        }

        public DelegateCommand CmdGetLog => new DelegateCommand(() =>
        {
            RefreshLog();
        });

        public DelegateCommand CmdCopyLog => new DelegateCommand(async () =>
        {
            await App.Current.Clipboard.SetTextAsync(Log);
        });
#if DEBUG
        public DelegateCommand CmdError => new DelegateCommand(() =>
        {
            _computerService.ThrowException();
        });
#endif
    }
}
