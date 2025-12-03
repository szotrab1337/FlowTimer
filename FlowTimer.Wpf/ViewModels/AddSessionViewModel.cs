using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.Navigation;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class AddSessionViewModel(
        ISessionService sessionService,
        INavigationService navigationService,
        ILogger<AddSessionViewModel> logger) : ObservableValidator
    {
        private readonly ILogger<AddSessionViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly ISessionService _sessionService = sessionService;

        [ObservableProperty]
        [Required(ErrorMessage = "To pole jest wymagane")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        private DateTime? _endTime;

        [ObservableProperty]
        [Required(ErrorMessage = "To pole jest wymagane")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        private DateTime? _startTime;

        private int _workItemId;

        public void Initialize(int workItemId)
        {
            _workItemId = workItemId;
        }

        [RelayCommand]
        private void Cancel()
        {
            _navigationService.NavigateBack();
        }

        private bool CanSave()
        {
            var validDates = EndTime > StartTime;
            
            return !HasErrors && validDates;
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task Save()
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                return;
            }

            try
            {
                await _sessionService.CreateManual(_workItemId, StartTime!.Value, EndTime!.Value);
                _navigationService.NavigateBack();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new session.");
            }
        }
    }
}