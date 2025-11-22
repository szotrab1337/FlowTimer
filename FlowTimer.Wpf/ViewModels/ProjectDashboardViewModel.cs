using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels.Items;
using FlowTimer.Wpf.Views;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class ProjectDashboardViewModel(
        IProjectService projectService,
        ILogger<ProjectDashboardViewModel> logger,
        INavigationService navigationService) : ObservableObject
    {
        private readonly ILogger<ProjectDashboardViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly IProjectService _projectService = projectService;

        [ObservableProperty]
        private ProjectItemViewModel _project = default!;

        private int _projectId;

        [ObservableProperty]
        private ObservableCollection<WorkItemViewModel> _workItems = [];

        public async Task Initialize(int projectId)
        {
            _projectId = projectId;

            var project = await _projectService.GetById(_projectId);

            if (project is not null)
            {
                Project = new ProjectItemViewModel(project);
            }
        }

        [RelayCommand]
        private void AddWorkItem()
        {
        }

        [RelayCommand]
        private void ArchiveWorkItem(WorkItemViewModel vm)
        {
        }

        [RelayCommand]
        private void EditProject()
        {
            _navigationService.Navigate(typeof(EditProjectPage), _projectId);
        }

        [RelayCommand]
        private void EditWorkItem(WorkItemViewModel vm)
        {
        }

        [RelayCommand]
        private void GoBack()
        {
            _navigationService.Navigate(typeof(ProjectsPage));
        }

        [RelayCommand]
        private void ToggleCompleted(WorkItemViewModel vm)
        {
        }

        [RelayCommand]
        private void ToggleTimer(WorkItemViewModel vm)
        {
        }
    }
}