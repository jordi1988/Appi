﻿using Core.Abstractions;
using Core.Helper;
using Core.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="ISourceService"/> using a file approach.
    /// </summary>
    /// <seealso cref="ISourceService" />
    public partial class FileSourceService : ISourceService
    {
        private readonly Preferences _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSourceService"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="System.ArgumentNullException">options</exception>
        public FileSourceService(IOptions<Preferences> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Reads the sources from the setting's file.
        /// </summary>
        /// <returns>All sources found in the file.</returns>
        public IEnumerable<ISource> ReadSources()
        {
            var fileContents = File.ReadAllText(_options.SourcesFilename);
            var fileSources = JsonSerializer.Deserialize<IEnumerable<JsonFileSource>>(fileContents);

            RepairFileSourcesAndSave(fileSources);

            return fileSources ?? Enumerable.Empty<ISource>();
        }

        /// <summary>
        /// Write the provided sources into the setting's file.
        /// </summary>
        /// <param name="sources">The sources.</param>
        public void SaveSources(IEnumerable<ISource> sources)
        {
            var content = JsonSerializer.Serialize(sources,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(_options.SourcesFilename, content);
        }

        [GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial Regex AsciiCharactersOnlyRegex();

        private void RepairFileSourcesAndSave(IEnumerable<JsonFileSource>? fileSources)
        {
            if (fileSources is null || !fileSources.Any())
            {
                return;
            }

            var sourcesToRepair = fileSources.Where(
                source => string.IsNullOrWhiteSpace(source.Alias) ||
                          source.Alias.Contains(' ') ||
                          source.IsQueryCommand is null ||
                          source.Groups is null);

            if (!sourcesToRepair.Any())
            {
                return;
            }

            foreach (var source in sourcesToRepair)
            {
                source.Alias = AsciiCharactersOnlyRegex()
                    .Replace(source.Name, string.Empty)
                    .ToLowerInvariant();

                source.IsQueryCommand ??= true;
                source.Groups ??= Array.Empty<string>();
            }

            SaveSources(fileSources);
        }
    }
}
