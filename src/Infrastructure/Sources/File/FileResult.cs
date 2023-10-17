using Core.Abstractions;

namespace Infrastructure.Sources.File
{
    /// <summary>
    /// Represents the base class for file item results.
    /// </summary>
    /// <seealso cref="ResultItemBase" />
    public abstract class FileResult : ResultItemBase
    {
        /// <inheritdoc cref="ResultItemBase.ToString"/>
        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
