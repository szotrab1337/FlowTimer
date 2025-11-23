using System.Text.Json;
using FlowTimer.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Infrastructure.Settings
{
    public class SettingsService : ISettingsService
    {
        private const string SettingsFileName = "usersettings.json";
        private readonly ILogger<SettingsService> _logger;
        private readonly UserSettings _settings;
        private readonly string _settingsPath;

        public SettingsService(ILogger<SettingsService> logger)
        {
            _logger = logger;

            _settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Flow Timer",
                SettingsFileName
            );

            _settings = LoadSettings();
        }

        public string GetTheme()
        {
            return _settings.Theme;
        }

        public void SetTheme(string theme)
        {
            _settings.Theme = theme;
            SaveSettings();
        }

        private UserSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    return JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load user settings.");
            }

            return new UserSettings();
        }

        private void SaveSettings()
        {
            try
            {
                var directory = Path.GetDirectoryName(_settingsPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save user settings.");
            }
        }

        private class UserSettings
        {
            public string Theme { get; set; } = "System";
        }
    }
}