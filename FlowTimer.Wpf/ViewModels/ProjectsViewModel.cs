using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.ViewModels.Items;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class ProjectsViewModel(IProjectService projectService) : ObservableObject
    {
        private readonly IProjectService _projectService = projectService;

        [ObservableProperty]
        private ObservableCollection<ProjectViewModel> _projects = [];

        [ObservableProperty]
        private ProjectViewModel? _selectedProject;

        public async Task Initialize()
        {
            var projects = await _projectService.GetAll();
            var vms = projects.Select(x => new ProjectViewModel(x));

            Projects = new ObservableCollection<ProjectViewModel>(vms);
        }

        [RelayCommand]
        private void AddProject()
        {
        }

        [RelayCommand]
        private void ArchiveProject(ProjectViewModel vm)
        {
        }

        [RelayCommand]
        private void EditProject(ProjectViewModel vm)
        {
        }

        partial void OnSelectedProjectChanged(ProjectViewModel? value)
        {
        }
    }
}