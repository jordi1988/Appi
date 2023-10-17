using System.Diagnostics;

namespace Infrastructure.Services
{
    /// <summary>
    /// Represents a process service for hadling OS processes.
    /// </summary>
    public static class ProcessService
    {
        /// <summary>
        /// Starts the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Start(string filename, string? arguments = null)
        {
            _ = Process.Start(
                new ProcessStartInfo(filename, arguments ?? string.Empty)
                {
                    UseShellExecute = true
                });
        }
    }
}
