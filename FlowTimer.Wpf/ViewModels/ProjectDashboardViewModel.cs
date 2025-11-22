using System.Collections.ObjectModel;
using System.Windows;
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
        INavigationService navigationService,
        IWorkItemService workItemService) : ObservableObject
    {
        private readonly ILogger<ProjectDashboardViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly IProjectService _projectService = projectService;
        private readonly IWorkItemService _workItemService = workItemService;

        [ObservableProperty]
        private ProjectItemViewModel _project = default!;

        private int _projectId;

        [ObservableProperty]
        private ObservableCollection<WorkItemViewModel> _workItems = [];

        public void Cleanup()
        {
            _workItemService.WorkItemArchived -= OnWorkItemArchived;
        }

        public async Task Initialize(int projectId)
        {
            _projectId = projectId;

            await LoadProjectInfo();
            await LoadWorkItems();

            _workItemService.WorkItemArchived += OnWorkItemArchived;
        }

        [RelayCommand]
        private void AddWorkItem()
        {
            _navigationService.Navigate(typeof(AddWorkItemPage), _projectId);
        }

        [RelayCommand]
        private void ArchiveWorkItem(WorkItemViewModel vm)
        {
            try
            {
                var result = MessageBox.Show(
                    $"Czy na pewno chcesz zarchiwizować zadanie '{vm.Name}'?",
                    "Potwierdzenie",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _workItemService.Archive(vm.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while archiving work item.");
            }
        }

        [RelayCommand]
        private void EditProject()
        {
            _navigationService.Navigate(typeof(EditProjectPage), _projectId);
        }

        [RelayCommand]
        private void EditWorkItem(WorkItemViewModel vm)
        {
            _navigationService.Navigate(typeof(EditWorkItemPage), vm.Id);
        }

        [RelayCommand]
        private void GoBack()
        {
            _navigationService.Navigate(typeof(ProjectsPage));
        }

        private async Task LoadProjectInfo()
        {
            var project = await _projectService.GetById(_projectId);

            if (project is not null)
            {
                Project = new ProjectItemViewModel(project);
            }
        }

        private async Task LoadWorkItems()
        {
            var workItems = await _workItemService.GetByProjectId(_projectId);
            var vms = workItems.Select(x => new WorkItemViewModel(x));

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                WorkItems = new ObservableCollection<WorkItemViewModel>(vms);
            });
        }

        private void OnWorkItemArchived(object? sender, int e)
        {
            var workItem = WorkItems.FirstOrDefault(x => x.Id == e);

            if (workItem is not null)
            {
                WorkItems.Remove(workItem);
            }
        }

        [RelayCommand]
        private async Task ToggleCompleted(WorkItemViewModel vm)
        {
            try
            {
                await _workItemService.Edit(vm.Id, vm.Name, vm.Description, vm.IsCompleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a work item.");
            }
        }

        [RelayCommand]
        private void ToggleTimer(WorkItemViewModel vm)
        {
        }
    }
}