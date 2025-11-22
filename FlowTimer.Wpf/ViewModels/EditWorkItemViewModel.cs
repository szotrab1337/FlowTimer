using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.Navigation;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class EditWorkItemViewModel(
        IWorkItemService workItemService,
        ILogger<EditWorkItemViewModel> logger,
        INavigationService navigationService) : ObservableValidator
    {
        private readonly ILogger<EditWorkItemViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly IWorkItemService _workItemService = workItemService;

        [ObservableProperty]
        [MaxLength(256, ErrorMessage = "Opis nie może być dłuższy niż 256 znaków.")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        private string? _description;

        private bool _isCompleted;

        [ObservableProperty]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [MaxLength(128, ErrorMessage = "Nazwa nie może być dłuższa niż 128 znaków.")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        private string _name = string.Empty;

        private int _workItemId;

        public async Task Initialize(int workItemId)
        {
            _workItemId = workItemId;

            var workItem = await _workItemService.GetById(_workItemId);
            if (workItem is not null)
            {
                Name = workItem.Name;
                Description = workItem.Description;
                _isCompleted = workItem.IsCompleted;
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            _navigationService.NavigateBack();
        }

        private bool CanSave()
        {
            return !HasErrors;
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
                await _workItemService.Edit(_workItemId, Name, Description, _isCompleted);
                _navigationService.NavigateBack();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a work item.");
            }
        }
    }
}