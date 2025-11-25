using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;
using FlowTimer.Wpf.Helpers;
using Microsoft.Win32;

namespace FlowTimer.Wpf.Views
{
    public abstract class BaseWindow : Window
    {
        protected BaseWindow()
        {
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

            Loaded += (_, _) =>
            {
                UpdateWindowBackground();
                UpdateMainWindowVisuals();
            };
        }

        protected abstract Grid BaseMainGrid { get; }
        protected abstract Border BaseHighContrastBorder { get; }
        protected abstract Button BaseCloseButton { get; }
        protected abstract Button? BaseMinimizeButton { get; }
        protected abstract Button? BaseMaximizeButton { get; }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            Dispatcher.Invoke(UpdateMainWindowVisuals);
        }

        private void UpdateMainWindowVisuals()
        {
            BaseMainGrid.Margin = default;

            if (WindowState == WindowState.Maximized)
            {
                BaseMainGrid.Margin = SystemParameters.HighContrast ? new Thickness(0, 8, 0, 0) : new Thickness(8);
            }

            UpdateTitleBarButtonsVisibility();

            if (SystemParameters.HighContrast)
            {
                BaseHighContrastBorder.SetResourceReference(BorderBrushProperty,
                    IsActive ? SystemColors.ActiveCaptionBrushKey : SystemColors.InactiveCaptionBrushKey);
                BaseHighContrastBorder.BorderThickness = new Thickness(8, 1, 8, 8);

                var wc = WindowChrome.GetWindowChrome(this);
                wc?.NonClientFrameEdges = NonClientFrameEdges.None;
            }
            else
            {
                BaseHighContrastBorder.BorderBrush = Brushes.Transparent;
                BaseHighContrastBorder.BorderThickness = new Thickness(0);

                var wc = WindowChrome.GetWindowChrome(this);
                wc?.NonClientFrameEdges = NonClientFrameEdges.Right | NonClientFrameEdges.Bottom |
                                          NonClientFrameEdges.Left;
            }
        }

        private void UpdateTitleBarButtonsVisibility()
        {
            if (Utility.IsBackdropDisabled() || !Utility.IsBackdropSupported() || SystemParameters.HighContrast)
            {
                BaseMinimizeButton?.Visibility = Visibility.Visible;
                BaseMaximizeButton?.Visibility = Visibility.Visible;
                BaseCloseButton.Visibility = Visibility.Visible;
            }
            else
            {
                BaseMinimizeButton?.Visibility = Visibility.Collapsed;
                BaseMaximizeButton?.Visibility = Visibility.Collapsed;
                BaseCloseButton.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateWindowBackground()
        {
            if (!Utility.IsBackdropDisabled() && !Utility.IsBackdropSupported())
            {
                SetResourceReference(BackgroundProperty, "WindowBackground");
            }
        }

        protected void CloseWindow(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        protected void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        protected void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}