using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.Views;
using MenuItem = FlowTimer.Wpf.Models.MenuItem;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class MainViewModel(INavigationService navigationService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;

        [ObservableProperty]
        private bool _canNavigateBack;

        [ObservableProperty]
        private ICollection<MenuItem> _menuItems =
        [
            new()
            {
                Title = "Strona główna",
                TargetPage = typeof(HomePage),
                Icon = "\ue80f"
            },
            new()
            {
                Title = "Projekty",
                TargetPage = typeof(ProjectsPage),
                Icon = "\uebc6"
            }
        ];

        public void Navigate(MenuItem? value)
        {
            if (value is null)
            {
                return;
            }

            _navigationService.Navigate(value.TargetPage);
        }

        [RelayCommand]
        private void Settings()
        {
            _navigationService.Navigate(typeof(SettingsPage));
        }
    }
}