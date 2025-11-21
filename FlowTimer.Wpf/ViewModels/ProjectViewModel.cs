using CommunityToolkit.Mvvm.ComponentModel;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class ProjectViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _projectId;

        public void Initialize(int projectId)
        {
            ProjectId = projectId;
        }
    }
}