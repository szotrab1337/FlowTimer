using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using FlowTimer.Wpf.Helpers;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;
using Microsoft.Win32;
using MenuItem = FlowTimer.Wpf.Models.MenuItem;

namespace FlowTimer.Wpf.Views
{
    public partial class MainWindow
    {
        public MainWindow(MainViewModel viewModel, INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();

            UpdateWindowBackground();
            UpdateMainWindowVisuals();

            navigationService.SetFrame(RootContentFrame);
            navigationService.Navigate(typeof(HomePage));

            WindowChrome.SetWindowChrome(
                this,
                new WindowChrome
                {
                    CaptionHeight = 50,
                    CornerRadius = new CornerRadius(12),
                    GlassFrameThickness = new Thickness(-1),
                    ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                    UseAeroCaptionButtons = true,
                    NonClientFrameEdges = SystemParameters.HighContrast
                        ? NonClientFrameEdges.None
                        : NonClientFrameEdges.Right | NonClientFrameEdges.Bottom | NonClientFrameEdges.Left
                }
            );

            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            StateChanged += (_, _) => UpdateMainWindowVisuals();
            Activated += (_, _) => UpdateMainWindowVisuals();
            Deactivated += (_, _) => UpdateMainWindowVisuals();
        }

        public MainViewModel ViewModel { get; }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaximizeIcon.Text = "\uE922";
            }
            else
            {
                WindowState = WindowState.Maximized;
                MaximizeIcon.Text = "\uE923";
            }
        }

        private void Menu_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            NavigateToSelectedMenuItem();
        }

        private void Menu_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NavigateToSelectedMenuItem();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void NavigateToSelectedMenuItem()
        {
            if (Menu.SelectedItem is not MenuItem navItem)
            {
                return;
            }

            ViewModel.Navigate(navItem);
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            Dispatcher.Invoke(UpdateMainWindowVisuals);
        }

        private void UpdateMainWindowVisuals()
        {
            MainGrid.Margin = default;
            if (WindowState == WindowState.Maximized)
            {
                MainGrid.Margin = SystemParameters.HighContrast ? new Thickness(0, 8, 0, 0) : new Thickness(8);
            }

            UpdateTitleBarButtonsVisibility();

            if (SystemParameters.HighContrast)
            {
                HighContrastBorder.SetResourceReference(BorderBrushProperty,
                    IsActive ? SystemColors.ActiveCaptionBrushKey : SystemColors.InactiveCaptionBrushKey);
                HighContrastBorder.BorderThickness = new Thickness(8, 1, 8, 8);

                var wc = WindowChrome.GetWindowChrome(this);
                wc?.NonClientFrameEdges = NonClientFrameEdges.None;
            }
            else
            {
                HighContrastBorder.BorderBrush = Brushes.Transparent;
                HighContrastBorder.BorderThickness = new Thickness(0);

                var wc = WindowChrome.GetWindowChrome(this);
                wc?.NonClientFrameEdges = NonClientFrameEdges.Right | NonClientFrameEdges.Bottom |
                                          NonClientFrameEdges.Left;
            }
        }

        private void UpdateTitleBarButtonsVisibility()
        {
            if (Utility.IsBackdropDisabled() || !Utility.IsBackdropSupported() || SystemParameters.HighContrast)
            {
                MinimizeButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Visible;
                CloseButton.Visibility = Visibility.Visible;
            }
            else
            {
                MinimizeButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Collapsed;
                CloseButton.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateWindowBackground()
        {
            if (!Utility.IsBackdropDisabled() && !Utility.IsBackdropSupported())
            {
                SetResourceReference(BackgroundProperty, "WindowBackground");
            }
        }
    }
}