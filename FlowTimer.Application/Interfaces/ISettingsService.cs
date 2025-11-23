namespace FlowTimer.Application.Interfaces
{
    public interface ISettingsService
    {
        string GetTheme();
        void SetTheme(string theme);
    }
}