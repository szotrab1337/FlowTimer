using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace FlowTimer.Wpf.Navigation
{
    public interface INavigationService
    {
        void Navigate(Type type);
        void Navigate(Type type, object parameter);
        void SetFrame(Frame frame);
    }

    public interface INavigable
    {
        void OnNavigatedTo(object parameter);
    }

    public class NavigationService(IServiceProvider serviceProvider) : INavigationService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private Frame? _frame;

        public void Navigate(Type type)
        {
            var page = _serviceProvider.GetRequiredService(type);

            _frame?.Navigate(page);
        }

        public void Navigate(Type type, object parameter)
        {
            var page = serviceProvider.GetRequiredService(type);

            if (page is INavigable navigable)
            {
                navigable.OnNavigatedTo(parameter);
            }

            _frame?.Navigate(page);
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }
    }
}