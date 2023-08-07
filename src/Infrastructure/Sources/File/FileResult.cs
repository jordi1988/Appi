using Core.Abstractions;

namespace Infrastructure.Sources.File
{
    public abstract class FileResult : ResultItemBase
    {
        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
