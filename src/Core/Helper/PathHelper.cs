using Core.Exceptions;

namespace Core.Helper
{
    /// <summary>
    /// Represents a helper class when dealing with paths.
    /// </summary>
    /// <remarks>Thanks to <seealso href="https://stackoverflow.com/a/1634549/1321339"/>.</remarks>
    public static class PathHelper
    {
        /// <summary>
        /// Normalizes the filepath.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        public static string NormalizeFilepath(string filepath)
        {
            string result = Path.GetFullPath(filepath).ToLowerInvariant();

            result = result.TrimEnd('\\');

            return result;
        }

        /// <summary>
        /// Gets the relative path.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="fullPath">The full path.</param>
        /// <returns></returns>
        /// <exception cref="CoreException">Could not find rootPath in fullPath when calculating relative path.</exception>
        public static string GetRelativePath(string rootPath, string fullPath)
        {
            rootPath = NormalizeFilepath(rootPath);
            fullPath = NormalizeFilepath(fullPath);

            if (!fullPath.StartsWith(rootPath))
                throw new CoreException("Could not find rootPath in fullPath when calculating relative path.");

            return fullPath[rootPath.Length..];
        }

        /// <summary>
        /// Determines whether the given file ends with dll.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>
        ///   <c>true</c> if it is a DLL file; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDllFile(string? filename)
        {
            if (filename is null)
            {
                return false;
            }

            return Path
                .GetExtension(filename)
                .Equals(".dll", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
