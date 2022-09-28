using Prism.Commands;
using IPR.Control.Panel.Common;
using IPR.Control.Panel.Services;

namespace IPR.Control.Panel.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly IComputerService _computerService;

        private string _report = string.Empty;
        public string Report
        {
            get => _report;
            set => SetProperty(ref _report, value);
        }

        public ReportViewModel(
            IComputerService computerService)
        {
            _computerService = computerService;

            Title = "Hardware Report";

            RefreshReport();
        }

        private void RefreshReport()
        {
            Report = _computerService.GetReport();
        }

        public DelegateCommand CmdGetReport => new DelegateCommand(() =>
        {
            RefreshReport();
        }); 

        public DelegateCommand CmdCopyReport => new DelegateCommand(async () =>
        {
            await App.Current.Clipboard.SetTextAsync(Report);
        });
    }
}
