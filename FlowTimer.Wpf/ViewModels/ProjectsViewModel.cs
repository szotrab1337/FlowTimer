using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels.Items;
using FlowTimer.Wpf.Views;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class ProjectsViewModel(
        IProjectService projectService,
        INavigationService navigationService) : ObservableObject
    {
        private readonly IProjectService _projectService = projectService;
        private readonly INavigationService _navigationService = navigationService;

        [ObservableProperty]
        private ObservableCollection<ProjectItemViewModel> _projects = [];

        [ObservableProperty]
        private ProjectItemViewModel? _selectedProject;

        public async Task Initialize()
        {
            var projects = await _projectService.GetAll();
            var vms = projects.Select(x => new ProjectItemViewModel(x));

            Projects = new ObservableCollection<ProjectItemViewModel>(vms);
        }

        [RelayCommand]
        private void AddProject()
        {
        }

        [RelayCommand]
        private void ArchiveProject(ProjectItemViewModel vm)
        {
        }

        [RelayCommand]
        private void EditProject(ProjectItemViewModel vm)
        {
        }

        partial void OnSelectedProjectChanged(ProjectItemViewModel? value)
        {
            if (value is null)
            {
                return;
            }

            _navigationService.Navigate(typeof(ProjectPage), value.Id);
        }
    }
}