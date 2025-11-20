using System.Collections.ObjectModel;

namespace FlowTimer.Wpf.Models
{
    public class MenuItem
    {
        public required string Title { get; set; }
        public required string Icon { get; set; }
        public required Type TargetPage { get; set; }
        public ObservableCollection<MenuItem> Items { get; set; } = [];
    }
}