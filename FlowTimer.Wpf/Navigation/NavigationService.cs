using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace FlowTimer.Wpf.Navigation
{
    public interface INavigationService
    {
        void Navigate(Type type);
        void Navigate(Type type, object parameter);
        void NavigateBack();
        void SetFrame(Frame frame);
    }

    public interface INavigable
    {
        void OnNavigatedTo(object parameter);
    }

    public class NavigationService(IServiceProvider serviceProvider) : INavigationService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private Type? _currentPage;
        private object? _currentPageParameter;
        private Frame? _frame;
        private Type? _previousPage;
        private object? _previousPageParameter;

        public void Navigate(Type type)
        {
            _previousPage = _currentPage;
            _previousPageParameter = _currentPageParameter;

            _currentPage = type;
            _currentPageParameter = null;

            var page = _serviceProvider.GetRequiredService(type);
            _frame?.Navigate(page);
        }

        public void Navigate(Type type, object parameter)
        {
            _previousPage = _currentPage;
            _previousPageParameter = _currentPageParameter;

            _currentPage = type;
            _currentPageParameter = parameter;

            var page = _serviceProvider.GetRequiredService(type);

            if (page is INavigable navigable)
            {
                navigable.OnNavigatedTo(parameter);
            }

            _frame?.Navigate(page);
        }

        public void NavigateBack()
        {
            if (_previousPage is null)
            {
                return;
            }

            if (_previousPageParameter is not null)
            {
                Navigate(_previousPage, _previousPageParameter);
                return;
            }

            Navigate(_previousPage);
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }
    }
}