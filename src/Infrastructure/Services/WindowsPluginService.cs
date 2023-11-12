using Core.Abstractions;
using Core.Helper;
using Core.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="IPluginService"/> for the Windows OS.
    /// </summary>
    /// <seealso cref="IPluginService" />
    public class WindowsPluginService : IPluginService
    {
        private const string _pluginKeyName = "AllowExternalLibraries";
        private const string _pluginKeyValueAllowed = "1";
        private const string _pluginKeyValueProhibited = "0";
        private readonly Preferences _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsPluginService"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">This class can only be instantiated on Windows OS.</exception>
        public WindowsPluginService(
            IStringLocalizer<InfrastructureLayerLocalization> localization,
            IOptions<Preferences> options)
        {
            ArgumentNullException.ThrowIfNull(localization);
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOperationException(localization["This class can only be instantiated on Windows OS."]);
            }

            EnsureRegistryKeyExsists();
        }

#pragma warning disable CA1416 // Validate platform compatibility

        /// <inheritdoc cref="IPluginService.Allow"/>
        public void Allow()
        {
            var registry = GetSubKeyRegistry(true);
            registry.Key?.SetValue(_pluginKeyName, _pluginKeyValueAllowed);
            registry.Key?.Close();
        }

        /// <inheritdoc cref="IPluginService.Prohibit"/>
        public void Prohibit()
        {
            var registry = GetSubKeyRegistry(true);
            registry.Key?.SetValue(_pluginKeyName, _pluginKeyValueProhibited);
            registry.Key?.Close();
        }

        /// <inheritdoc cref="IPluginService.IsAllowed"/>
        public bool IsAllowed()
        {
            var registry = GetSubKeyRegistry();
            var allowed = registry.Key!.GetValue(_pluginKeyName, _pluginKeyValueProhibited) as string;

            registry.Key.Close();

            return allowed == _pluginKeyValueAllowed;
        }

        private static void EnsureRegistryKeyExsists()
        {
            var registry = GetSubKeyRegistry();

            if (registry.Key is null)
            {
                registry.Key = Registry.CurrentUser.CreateSubKey(registry.Path);
                registry.Key.SetValue(_pluginKeyName, _pluginKeyValueProhibited);
            }

            registry.Key.Close();
        }

        private static (RegistryKey? Key, string Path) GetSubKeyRegistry(bool writable = false)
        {
            var currentAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Appi";
            var subKeyPath = @$"SOFTWARE\{currentAssemblyName}";
            var key = Registry.CurrentUser.OpenSubKey(subKeyPath, writable);

            return (key, subKeyPath);
        }

        /// <inheritdoc cref="IPluginService.ActivateExternalPluginsIfAllowed"/>
        public void ActivateExternalPluginsIfAllowed()
        {
            ActivateExternalPlugins(_options.AppDataDirectory, "*.dll");

            var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ArgumentException.ThrowIfNullOrEmpty(executablePath);

            ActivateExternalPlugins(executablePath, "Appi.Plugin.*.dll");
        }

        private static void ActivateExternalPlugins(string path, string searchPattern)
        {
            var externalAssemblyFiles = Directory.GetFiles(path, searchPattern);
            foreach (var filename in externalAssemblyFiles)
            {
                Assembly.UnsafeLoadFrom(filename);
            }
        }

#pragma warning restore CA1416 // Validate platform compatibility
    }
}
