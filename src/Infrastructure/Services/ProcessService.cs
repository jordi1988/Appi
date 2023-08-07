using System.Diagnostics;

namespace Infrastructure.Services
{
    public static class ProcessService
    {
        public static void Start(string filename, string? arguments)
        {
            _ = Process.Start(
                new ProcessStartInfo(filename, arguments ?? string.Empty)
                {
                    UseShellExecute = true
                });
        }
    }
}
