using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Entities;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels.Items;
using FlowTimer.Wpf.Views;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class ProjectsViewModel(
        IProjectService projectService,
        INavigationService navigationService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;
        private readonly IProjectService _projectService = projectService;

        [ObservableProperty]
        private ObservableCollection<ProjectItemViewModel> _projects = [];

        [ObservableProperty]
        private ProjectItemViewModel? _selectedProject;

        public void Cleanup()
        {
            _projectService.ProjectCreated -= OnProjectCreated;
        }

        public async Task Initialize()
        {
            var projects = await _projectService.GetAll();
            var vms = projects.Select(x => new ProjectItemViewModel(x));

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Projects = new ObservableCollection<ProjectItemViewModel>(vms);
            });

            _projectService.ProjectCreated += OnProjectCreated;
        }

        [RelayCommand]
        private void AddProject()
        {
            _navigationService.Navigate(typeof(AddProjectPage));
        }

        [RelayCommand]
        private void ArchiveProject(ProjectItemViewModel vm)
        {
        }

        [RelayCommand]
        private void EditProject(ProjectItemViewModel vm)
        {
        }

        private void OnProjectCreated(object? sender, Project e)
        {
            var vm = new ProjectItemViewModel(e);

            System.Windows.Application.Current.Dispatcher.Invoke(() => { Projects.Add(vm); });
        }

        partial void OnSelectedProjectChanged(ProjectItemViewModel? value)
        {
            if (value is null)
            {
                return;
            }

            _navigationService.Navigate(typeof(ProjectDashboardPage), value.Id);
        }
    }
}