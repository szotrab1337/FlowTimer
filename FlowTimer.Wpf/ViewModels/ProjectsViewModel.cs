using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Entities;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels.Items;
using FlowTimer.Wpf.Views;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class ProjectsViewModel(
        IProjectService projectService,
        INavigationService navigationService,
        ILogger<ProjectsViewModel> logger) : ObservableObject
    {
        private readonly ILogger<ProjectsViewModel> _logger = logger;
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
            _projectService.ProjectArchived += OnProjectArchived;
            _projectService.ProjectEdited += OnProjectEdited;
        }

        private void OnProjectEdited(object? sender, Project e)
        {
            var project = Projects.FirstOrDefault(x => x.Id == e.Id);

            if (project is not null)
            {
                project.Update(e);
            }
        }

        [RelayCommand]
        private void AddProject()
        {
            _navigationService.Navigate(typeof(AddProjectPage));
        }

        [RelayCommand]
        private void ArchiveProject(ProjectItemViewModel vm)
        {
            try
            {
                var result = MessageBox.Show(
                    "Czy na pewno chcesz zarchiwizować ten projekt?",
                    "Potwierdzenie",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _projectService.Archive(vm.Id);
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
            var project = Projects.FirstOrDefault(x => x.Id == e);

            if (project is not null)
            {
                Projects.Remove(project);
            }
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