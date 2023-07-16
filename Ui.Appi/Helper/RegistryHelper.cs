using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ui.Appi.Helper
{
    // TODO: make interface, rename class and methods and decouple from registry, implement interface with registry dependency on windows
    internal static class RegistryHelper
    {
        private const string _externalLibsKeyName = "AllowExternalLibraries";

        static RegistryHelper()
        {
            EnsureRegistryKeyExsists();
        }

        public static bool IsExternalLibrariesAllowed()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Sorry for now, Linux and others
                return false;
            }

            var registry = GetSubKeyRegistry();
            var allowed = registry.Key!.GetValue(_externalLibsKeyName, "0") as string;

            registry.Key.Close();

            return allowed == "1";
        }

        public static void AllowExternalLibraries()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var registry = GetSubKeyRegistry(true);
            registry.Key?.SetValue(_externalLibsKeyName, "1");
            registry.Key?.Close();
        }

        public static void DisallowExternalLibraries()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var registry = GetSubKeyRegistry(true);
            registry.Key?.SetValue(_externalLibsKeyName, "0");
            registry.Key?.Close();
        }

        private static void EnsureRegistryKeyExsists()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var registry = GetSubKeyRegistry();
            if (registry.Key is null)
            {
                registry.Key = Registry.CurrentUser.CreateSubKey(registry.Path);
                registry.Key.SetValue(_externalLibsKeyName, "0");
            }

            registry.Key.Close();
        }

        private static (RegistryKey? Key, string Path) GetSubKeyRegistry(bool writable = false)
        {
            var currentAssemblyName = Assembly.GetCallingAssembly().FullName ?? "Appi";
            var subKeyPath = @$"SOFTWARE\{currentAssemblyName}";
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return (null, subKeyPath);
            }

            var key = Registry.CurrentUser.OpenSubKey(subKeyPath, writable);

            return (key, subKeyPath);
        }
    }
}
