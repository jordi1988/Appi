using Core.Abstractions;
using Core.Models;
using IoFile = System.IO.File;

namespace Infrastructure.Sources.File
{
    /// <summary>
    /// Represents a file source.
    /// </summary>
    /// <seealso cref="ISource" />
    public abstract class FileSource : ISource
    {
        /// <inheritdoc cref="ISource.TypeName"/>
        public abstract string TypeName { get; set; }

        /// <inheritdoc cref="ISource.Name"/>
        public abstract string Name { get; set; }

        /// <inheritdoc cref="ISource.Alias"/>
        public abstract string Alias { get; set; }

        /// <inheritdoc cref="ISource.Description"/>
        public abstract string Description { get; set; }

        /// <inheritdoc cref="ISource.IsActive"/>
        public abstract bool IsActive { get; set; }

        /// <inheritdoc cref="ISource.SortOrder"/>
        public abstract int SortOrder { get; set; }

        /// <inheritdoc cref="ISource.Path"/>
        public abstract string? Path { get; set; }

        /// <inheritdoc cref="ISource.Arguments"/>
        public abstract string? Arguments { get; set; }

        /// <inheritdoc cref="ISource.IsQueryCommand"/>
        public abstract bool? IsQueryCommand { get; set; }

        /// <inheritdoc cref="ISource.Groups"/>
        public string[]? Groups { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSource"/> class.
        /// </summary>
        protected FileSource()
        {
        }

        /// <summary>
        /// Fetches the data from the the text file.
        /// </summary>
        /// <param name="options">Options related to the query.</param>
        /// <returns>The mapped results.</returns>
        public virtual async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            ValidateFile();

            var output = (await IoFile
                .ReadAllLinesAsync(Path!))
                .Select(Parse);

            return output;
        }

        /// <summary>
        /// Parses the selected rows into result output objects.
        /// </summary>
        /// <param name="row">Current full row of the text file.</param>
        /// <param name="rowNumber">Current row number of the text file.</param>
        /// <returns>The mapped results.</returns>
        protected abstract ResultItemBase Parse(string row, int rowNumber);

        private void ValidateFile()
        {
            if (!IoFile.Exists(Path))
            {
                throw new FileNotFoundException("The given file was not found.", Path);
            }
        }
    }
}
