namespace FlowTimer.Wpf.Navigation
{
    public class NavigatingEventArgs
    {
        public NavigatingEventArgs()
        {
        }

        public NavigatingEventArgs(Type pageType)
        {
            PageType = pageType;
        }

        public Type? PageType { get; set; }
    }
}