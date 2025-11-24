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
    public partial class ProjectDashboardViewModel(
        IProjectService projectService,
        ILogger<ProjectDashboardViewModel> logger,
        INavigationService navigationService,
        IWorkItemService workItemService,
        ISessionTimerService sessionTimerService) : ObservableObject
    {
        private readonly ILogger<ProjectDashboardViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly IProjectService _projectService = projectService;
        private readonly ISessionTimerService _sessionTimerService = sessionTimerService;
        private readonly IWorkItemService _workItemService = workItemService;

        [ObservableProperty]
        private ProjectItemViewModel _project = default!;

        private int _projectId;

        [ObservableProperty]
        private ObservableCollection<WorkItemViewModel> _workItems = [];

        public void Cleanup()
        {
            _workItemService.WorkItemArchived -= OnWorkItemArchived;
            _sessionTimerService.Tick -= OnSessionTimerTick;
            _sessionTimerService.SessionStarted -= OnSessionStarted;
            _sessionTimerService.SessionStopped -= OnSessionStopped;
        }

        public async Task Initialize(int projectId)
        {
            _projectId = projectId;

            await LoadProjectInfo();
            await LoadWorkItems();

            if (_sessionTimerService.IsRunning)
            {
                var workItem = WorkItems.FirstOrDefault(x => x.Id == _sessionTimerService.ActiveWorkItemId);
                workItem?.IsSessionActive = true;
            }

            _workItemService.WorkItemArchived += OnWorkItemArchived;
            _sessionTimerService.Tick += OnSessionTimerTick;
            _sessionTimerService.SessionStarted += OnSessionStarted;
            _sessionTimerService.SessionStopped += OnSessionStopped;
        }

        [RelayCommand]
        private void AddWorkItem()
        {
            _navigationService.Navigate(typeof(AddWorkItemPage), _projectId);
        }

        [RelayCommand]
        private async Task ArchiveWorkItem(WorkItemViewModel vm)
        {
            try
            {
                var confirmed = await FluentMessageBox.Confirm(
                    $"Czy na pewno chcesz zarchiwizować zadanie '{vm.Name}'?");

                if (confirmed)
                {
                    await _workItemService.Archive(vm.Id);
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
            var vms = workItems.Select(x => new WorkItemViewModel(x, _sessionTimerService.ActiveSessionId));

            WorkItems = new ObservableCollection<WorkItemViewModel>(vms);
        }

        private void OnSessionStarted(object? sender, SessionStartedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var workItem = WorkItems.FirstOrDefault(x => x.Id == e.WorkItemId);
                workItem?.IsSessionActive = true;
            });
        }

        private void OnSessionStopped(object? sender, SessionStoppedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var workItem = WorkItems.FirstOrDefault(x => x.Id == e.WorkItemId);
                workItem?.IsSessionActive = false;
                workItem?.AddSessionTime(e.Duration);
            });
        }

        private void OnSessionTimerTick(object? sender, SessionTimerTickEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var workItem = WorkItems.FirstOrDefault(x => x.Id == e.WorkItemId);
                workItem?.UpdateTime(e.Elapsed);
            });
        }

        private void OnWorkItemArchived(object? sender, int e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var workItem = WorkItems.FirstOrDefault(x => x.Id == e);

                if (workItem is not null)
                {
                    WorkItems.Remove(workItem);
                }
            });
        }

        [RelayCommand]
        private async Task ToggleCompleted(WorkItemViewModel vm)
        {
            try
            {
                await _workItemService.Edit(vm.Id, vm.Name, vm.Description, vm.IsCompleted);

                if (_sessionTimerService.ActiveWorkItemId == vm.Id)
                {
                    await _sessionTimerService.Stop();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing a work item.");
            }
        }

        [RelayCommand]
        private async Task ToggleTimer(WorkItemViewModel vm)
        {
            try
            {
                if (_sessionTimerService.ActiveWorkItemId == vm.Id)
                {
                    await _sessionTimerService.Stop();
                }
                else
                {
                    await _sessionTimerService.Start(_projectId, vm.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling timer.");
            }
        }
    }
}