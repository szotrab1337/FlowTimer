using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.Navigation;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class EditProjectViewModel(
        INavigationService navigationService,
        IProjectService projectService,
        ILogger<EditProjectViewModel> logger) : ObservableValidator
    {
        private readonly ILogger<EditProjectViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly IProjectService _projectService = projectService;

        [ObservableProperty]
        [MaxLength(256, ErrorMessage = "Opis nie może być dłuższy niż 256 znaków.")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        private string? _description;

        [ObservableProperty]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [MaxLength(128, ErrorMessage = "Nazwa nie może być dłuższa niż 128 znaków.")]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [NotifyDataErrorInfo]
        private string _name = string.Empty;

        private int _projectId;

        public async Task Initialize(int projectId)
        {
            try
            {
                _projectId = projectId;

                var project = await _projectService.GetById(_projectId);

                if (project is not null)
                {
                    Name = project.Name;
                    Description = project.Description;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading project.");
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
                await _projectService.Edit(_projectId, Name, Description);
                _navigationService.NavigateBack();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a project.");
            }
        }
    }
}