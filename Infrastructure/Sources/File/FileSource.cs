﻿using Domain.Entities;
using Domain.Interfaces;
using IoFile = System.IO.File;

namespace Infrastructure.Sources.File
{
    public abstract class FileSource : ISource
    {
        public abstract string TypeName { get; set; }
        public abstract string Name { get; set; }
        public abstract string Description { get; set; }
        public abstract bool IsActive { get; set; }
        public abstract int SortOrder { get; set; }
        public abstract string? Path { get; set; }

        protected FileSource()
        {
        }

        public virtual async Task<IEnumerable<Result>> ReadAsync()
        {
            ValidateFile();

            var output = (await IoFile
                .ReadAllLinesAsync(Path!))
                .Select(Parse);

            return output;
        }

        protected abstract Result Parse(string row, int rowNumber);

        private void ValidateFile()
        {
            if (!IoFile.Exists(Path))
            {
                throw new FileNotFoundException("The given file was not found.", Path);
            }
        }
    }
}