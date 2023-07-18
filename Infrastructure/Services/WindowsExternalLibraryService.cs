using Domain.Interfaces;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Infrastructure.Services
{
    public class WindowsExternalLibraryService : IExternalLibraryService
    {
        private const string _externalLibsKeyName = "AllowExternalLibraries";
        private const string _externalLibsKeyValueAllowed = "1";
        private const string _externalLibsKeyValueProhibited = "0";

        public WindowsExternalLibraryService()
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
            registry.Key?.SetValue(_externalLibsKeyName, _externalLibsKeyValueAllowed);
            registry.Key?.Close();
        }

        public void Prohibit()
        {
            var registry = GetSubKeyRegistry(true);
            registry.Key?.SetValue(_externalLibsKeyName, _externalLibsKeyValueProhibited);
            registry.Key?.Close();
        }

        public bool IsAllowed()
        {
            var registry = GetSubKeyRegistry();
            var allowed = registry.Key!.GetValue(_externalLibsKeyName, _externalLibsKeyValueProhibited) as string;

            registry.Key.Close();

            return allowed == _externalLibsKeyValueAllowed;
        }

        private void EnsureRegistryKeyExsists()
        {
            var registry = GetSubKeyRegistry();

            if (registry.Key is null)
            {
                registry.Key = Registry.CurrentUser.CreateSubKey(registry.Path);
                registry.Key.SetValue(_externalLibsKeyName, _externalLibsKeyValueProhibited);
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
