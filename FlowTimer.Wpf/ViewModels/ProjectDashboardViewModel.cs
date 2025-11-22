using CommunityToolkit.Mvvm.ComponentModel;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class ProjectDashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _projectId;

        public void Initialize(int projectId)
        {
            ProjectId = projectId;
        }
    }
}