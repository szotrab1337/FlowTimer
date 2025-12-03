using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.Navigation;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class EditSessionViewModel(
        ISessionService sessionService,
        ILogger<EditSessionViewModel> logger,
        INavigationService navigationService) : ObservableValidator
    {
        private readonly ILogger<EditSessionViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly ISessionService _sessionService = sessionService;

        [ObservableProperty]
        [Required(ErrorMessage = "To pole jest wymagane")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        private DateTime _endTime;

        private int _sessionId;

        [ObservableProperty]
        [Required(ErrorMessage = "To pole jest wymagane")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        private DateTime _startTime;

        public async Task Initialize(int sessionId)
        {
            _sessionId = sessionId;

            var session = await _sessionService.GetById(_sessionId);
            if (session is not null)
            {
                StartTime = session.StartTime;
                EndTime = session.EndTime;
            }
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
                await _sessionService.Edit(_sessionId, StartTime, EndTime);
                _navigationService.NavigateBack();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a session.");
            }
        }
    }
}