using Core.Abstractions;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Infrastructure.Services
{
    public class WindowsPluginService : IPluginService
    {
        private const string _pluginKeyName = "AllowExternalLibraries";
        private const string _pluginKeyValueAllowed = "1";
        private const string _pluginKeyValueProhibited = "0";

        public WindowsPluginService()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOperationException("This class can only be instantiated on Windows OS.");
            }

            EnsureRegistryKeyExsists();
        }

#pragma warning disable CA1416 // Validate platform compatibility

        public void Allow()
        {
            var registry = GetSubKeyRegistry(true);
            registry.Key?.SetValue(_pluginKeyName, _pluginKeyValueAllowed);
            registry.Key?.Close();
        }

        public void Prohibit()
        {
            var registry = GetSubKeyRegistry(true);
            registry.Key?.SetValue(_pluginKeyName, _pluginKeyValueProhibited);
            registry.Key?.Close();
        }

        public bool IsAllowed()
        {
            var registry = GetSubKeyRegistry();
            var allowed = registry.Key!.GetValue(_pluginKeyName, _pluginKeyValueProhibited) as string;

            registry.Key.Close();

            return allowed == _pluginKeyValueAllowed;
        }

        private void EnsureRegistryKeyExsists()
        {
            var registry = GetSubKeyRegistry();

            if (registry.Key is null)
            {
                registry.Key = Registry.CurrentUser.CreateSubKey(registry.Path);
                registry.Key.SetValue(_pluginKeyName, _pluginKeyValueProhibited);
            }

            registry.Key.Close();
        }

        private (RegistryKey? Key, string Path) GetSubKeyRegistry(bool writable = false)
        {
            var currentAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Appi";
            var subKeyPath = @$"SOFTWARE\{currentAssemblyName}";
            var key = Registry.CurrentUser.OpenSubKey(subKeyPath, writable);

            return (key, subKeyPath);
        }

#pragma warning restore CA1416 // Validate platform compatibility
    }
}
