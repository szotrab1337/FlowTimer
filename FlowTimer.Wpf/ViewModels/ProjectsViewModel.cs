using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.Dialogs;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels.Items;
using FlowTimer.Wpf.Views;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class ProjectsViewModel(
        IProjectService projectService,
        INavigationService navigationService,
        ILogger<ProjectsViewModel> logger,
        ISessionTimerService sessionTimerService) : ObservableObject
    {
        private readonly ILogger<ProjectsViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly IProjectService _projectService = projectService;
        private readonly ISessionTimerService _sessionTimerService = sessionTimerService;

        [ObservableProperty]
        private ObservableCollection<ProjectItemViewModel> _projects = [];

        [ObservableProperty]
        private ProjectItemViewModel? _selectedProject;

        public void Cleanup()
        {
            _projectService.ProjectArchived -= OnProjectArchived;
            _sessionTimerService.Tick -= OnSessionTimerTick;
        }

        public async Task Initialize()
        {
            var projects = await _projectService.GetAll();
            var vms = projects.Select(x => new ProjectItemViewModel(x));

            Projects = new ObservableCollection<ProjectItemViewModel>(vms);

            var project = Projects.FirstOrDefault(x => x.Id == _sessionTimerService.ActiveProjectId);
            project?.IsSessionActive = true;

            _projectService.ProjectArchived += OnProjectArchived;
            _sessionTimerService.Tick += OnSessionTimerTick;
        }

        [RelayCommand]
        private void AddProject()
        {
            _navigationService.Navigate(typeof(AddProjectPage));
        }

        [RelayCommand]
        private async Task ArchiveProject(ProjectItemViewModel vm)
        {
            try
            {
                var result = await FluentMessageBox.Confirm(
                    $"Czy na pewno chcesz zarchiwizować projekt '{vm.Name}'?");

                if (result)
                {
                    await _projectService.Archive(vm.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while archiving project.");
            }
        }

        [RelayCommand]
        private void EditProject(ProjectItemViewModel vm)
        {
            _navigationService.Navigate(typeof(EditProjectPage), vm.Id);
        }

        private void OnProjectArchived(object? sender, int e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var project = Projects.FirstOrDefault(x => x.Id == e);

                if (project is not null)
                {
                    Projects.Remove(project);
                }
            });
        }

        partial void OnSelectedProjectChanged(ProjectItemViewModel? value)
        {
            if (value is null)
            {
                return;
            }

            _navigationService.Navigate(typeof(ProjectDashboardPage), value.Id);
        }

        private void OnSessionTimerTick(object? sender, SessionTimerTickEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var project = Projects.FirstOrDefault(x => x.Id == e.ProjectId);
                project?.UpdateTime(e.Elapsed);
            });
        }
    }
}